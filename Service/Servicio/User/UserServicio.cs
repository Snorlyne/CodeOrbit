using Domain.Entidades;
using Domain.Entidades.ViewModels;
using Domain.Util;
using Microsoft.Extensions.Logging;
using Repository.Context;
using Repository.Repositorio;
using Service.IServicio.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Service.Servicio.User
{
    public class UserServicio : IUserServicio
    {
        private readonly ILogger _logger;
        private readonly GenericRepository<Personas> _genericRepositoryPersonas;

        public UserServicio(ApplicationDBContext context, ILogger<UserServicio> logger)
        {
            _logger = logger;
            _genericRepositoryPersonas = new GenericRepository<Personas>(context);
        }


        public async Task<ResponseHelper> Crear(PersonasVM model)
        {
            ResponseHelper response = new ResponseHelper();
            try
            {
                Expression<Func<Personas, bool>> query = x => x.CURP == model.CURP;
                var existe = await _genericRepositoryPersonas.BuscarUnElemento(query);
                if(existe == null)
                {
                    Personas persona = new()
                    {
                        CURP = model.CURP,
                        Apellidos = model.Apellidos,
                        IsDeleted = model.IsDeleted,
                        Nombre = model.Nombre
                    };
                    if (await _genericRepositoryPersonas.Crear(persona) > 0)
                    {
                        response.Success = true;
                        response.Message = "Usuario Creado Con Éxito";
                        response.HelperData = persona.Id;
                        _logger.LogInformation(response.Message);
                        return response;
                    }
                }
                response.Success = false;
                response.Message = "El Usuario ya existe";
            }
            catch(Exception ex)
            {
                response.Success = false;
                response.Message = "Ha ocurrido un error";
            }
            return response;
        }

        public async Task<List<PersonasVM>> ObtenerLista()
        {
            List<PersonasVM> lista = new();
            try
            {
                var personas = await _genericRepositoryPersonas.ObtieneLista();
                lista = personas.Select(x => new PersonasVM
                {
                  Nombre = x.Nombre,
                  Apellidos = x.Apellidos,
                  CURP = x.CURP,
                  IsDeleted = x.IsDeleted, 
                }).ToList();

            }catch(Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return lista;
        }
    }
}
