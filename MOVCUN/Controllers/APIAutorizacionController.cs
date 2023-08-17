using Domain.Entidades;
using Domain.Entidades.ViewModels;
using Domain.Entidades.ViewModels.Autorizacion;
using Domain.Util;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Repository.Context;
using Repository.Repositorio;
using Services.IServicio;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using Ubiety.Dns.Core;

namespace BaseWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIAutorizacionController : ControllerBase
    {
        private readonly GenericRepository<ApplicationUser> _genericRepositoryApplicationUser;
        private readonly string secretKey;
        private readonly IAutorizacionServicio _autorizacionServicio;

        public APIAutorizacionController(ApplicationDBContext context,IConfiguration config, IAutorizacionServicio autorizacionServicio)
        {
            _genericRepositoryApplicationUser = new GenericRepository<ApplicationUser>(context);
            secretKey = config.GetSection("settings").GetSection("secretKey").ToString();
            _autorizacionServicio = autorizacionServicio;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] AutorizacionRequest request)
        {
            ResponseHelper response = new ResponseHelper();

            ApplicationUser au = new();
            Expression<Func<ApplicationUser, bool>> query = x => x.Password == request.Clave && x.Email == request.Email;
            try
            {
                au = await _genericRepositoryApplicationUser.BuscarUnElemento(query);

                if (au != null)
                {
                    string accessToken = await _autorizacionServicio.GenerarToken(au);
                    string refreshToken = await _autorizacionServicio.GenerarRefreshToken();
                    au.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
                    if (accessToken != "" && au.RefreshToken != "")
                    {
                        au.RefreshToken = refreshToken;
                        if(await _genericRepositoryApplicationUser.Actualizar(au) > 0)
                        {
                            response.Success = true;
                            response.Message = "Se ha iniciado sesión correctamente";

                            AutorizacionResponse jwtVM = new()
                            {
                                AppUser = au.AppUser,
                                Email = au.Email,
                                RefreshToken = refreshToken,
                                Token = accessToken,
                            };
                            response.HelperData = jwtVM;
                            return Ok(response);
                        }
                        response.Success = false;
                        response.Message = "Error al inicar sesion. comuniquese con el administrador";
                        return BadRequest(response);
                    }
                    response.Success = false;
                    response.Message = "Error al inicar sesion. comuniquese con el administrador";
                    return BadRequest(response);
                }
                response.Success = false;
                response.Message = "Datos invalidos, verificar correo y contraseña";
                return BadRequest(response);
            }
            catch (Exception e)
            {
                return BadRequest(new { Success = false, Message = "Error: " + e.Message });
            }
        }
        [HttpPost]
        [Route("Refresh")]
        public async Task<IActionResult> Refresh([FromBody] AutorizacionResponse viewModel)
        {
            try
            {
                ResponseHelper response = new ResponseHelper();
                ApplicationUser au = await _genericRepositoryApplicationUser.BuscarUnElemento(x => x.Email == viewModel.Email);
                if (au != null)
                {
                    if (au.RefreshTokenExpiryTime != null)
                    {
                        if (DateTime.Now > au.RefreshTokenExpiryTime)
                        {
                            response.Success = false;
                            response.Message = "Su sesión ha expirado, vuelva a iniciar sesión";
                            return BadRequest(response);
                        }
                    }
                        var newAccessToken = await _autorizacionServicio.GenerarToken(au);
                        var newRefreshToken = await _autorizacionServicio.GenerarRefreshToken();

                        au.RefreshToken = newRefreshToken;
                        au.RefreshTokenExpiryTime = DateTime.Now.AddDays(2);

                        if (newAccessToken.Length > 0 && newRefreshToken.Length > 0)
                        {
                           if(await _genericRepositoryApplicationUser.Actualizar(au) > 0)
                            {
                                response.Success = true;

                                AutorizacionResponse jwtVM = new()
                                {
                                    AppUser = au.AppUser,
                                    Email = au.Email,
                                    RefreshToken = newRefreshToken,
                                    Token = newAccessToken,
                                };
                                response.HelperData = jwtVM;
                                return Ok(response);
                            }
                            response.Success = false;
                            response.Message = "Error, comuniquese con el administrador";
                            return BadRequest(response);
                        }
                        response.Success = false;
                        response.Message = "Error, comuniquese con el administrador";
                        return BadRequest(response);

                }
                response.Success = false;
                response.Message = "Email no encontrado";
                return NotFound(response);

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
