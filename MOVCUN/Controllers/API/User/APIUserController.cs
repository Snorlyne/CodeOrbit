using Domain.Entidades.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.IServicio.User;

namespace BaseWeb.Controllers.API.User
{
    [Route("api/user")]
    [ApiController]
    [Authorize]
    public class APIUserController : ControllerBase
    {
        private readonly IUserServicio _UserServicio;
        public APIUserController(IUserServicio userServicio)
        {
            _UserServicio = userServicio;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Crear([FromBody] PersonasVM model)
        {
            var response = await _UserServicio.Crear(model);
            return Ok(response);
        }
        [HttpGet("lista")]
        public async Task<IActionResult> Lista()
        {
            var response = await _UserServicio.ObtenerLista();
            return Ok(response);
        }
    }
}
