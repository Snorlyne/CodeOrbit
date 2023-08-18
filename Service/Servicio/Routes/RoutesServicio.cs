using Domain.Entidades;
using Domain.Entidades.ViewModels;
using Domain.Util;
using Microsoft.Extensions.Logging;
using Repository.Context;
using Repository.Repositorio;
using Service.Servicio.User;
using Services.IServicio.Routes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Servicio.Routes
{
    public class RoutesServicio : IRoutesServicio
    {
        private readonly ILogger _logger;
        private readonly GenericRepository<RoutesVM> _genericRepositoryRoutes;

        public RoutesServicio(ApplicationDBContext context, ILogger<RoutesServicio> logger)
        {
            _logger = logger;
            _genericRepositoryRoutes = new GenericRepository<RoutesVM>(context);
        }
        public async Task<ResponseHelper> Crear(RoutesCreacionVM routes)
        {
            ResponseHelper response = new ResponseHelper();
            try
            {
                var routes1 = new RoutesVM
                {
                    bus_type = routes.bus_type,
                    route_name = routes.route_name,
                    start_time = routes.start_time,
                    end_time = routes.end_time,
                    fare = routes.fare,
                    created_at = DateTime.Now,
                    updated_at = DateTime.Now
                };

                if (await _genericRepositoryRoutes.Crear(routes1) > 0)
                {
                    response.Success = true;
                    response.Message = "Creación exitosa";
                    response.HelperData = routes1.Id;
                    _logger.LogInformation(response.Message);
                    return response;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                response.Message = "Ha ocurrido un error";
            }
            return response;
        }

        public async Task<ResponseHelper> Editar(RoutesVM routes, int? id)
        {
            ResponseHelper response = new ResponseHelper();
            try
            {
                var routes2 = await _genericRepositoryRoutes.BuscarPorId(id);

                if (routes2 != null)
                {
                    routes2.bus_type = routes2.bus_type;
                    routes2.route_name = routes2.route_name;
                    routes2.start_time = routes2.start_time;
                    routes2.end_time = routes2.end_time;
                    routes2.fare = routes2.fare;
                    routes2.created_at = DateTime.Now;
                    routes2.updated_at = DateTime.Now;

                        if (await _genericRepositoryRoutes.Actualizar(routes) > 0)
                        {
                            response.Success = true;
                            response.Message = "Se ha editado correctamente.";
                            response.HelperData = routes.Id;
                        }
                        else
                        {
                            response.Success = false;
                            response.Message = "Ocurrió un error al editar.";
                        }
                }
                else
                {
                    response.Success = false;
                    response.Message = "No existe.";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Se ha producido un error.";
                response.HelperData = ex.Message;
                _logger.LogError(ex.Message);
            }
            return response;
        }

        public async Task<ResponseHelper> Eliminar(int? id)
        {
            ResponseHelper response = new ResponseHelper();
            try
            {
                if (await _genericRepositoryRoutes.Eliminar(id) > 0)
                {
                    response.Success = true;
                    response.Message = "Datos eliminados exitosamente.";
                    _logger.LogInformation(response.Message);
                    return response;
                }
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = "Se ha producido un error.";
                response.HelperData = e.Message;
                _logger.LogError(e.Message);
            }
            return response;
        }

        public async Task<List<RoutesVM>> ObtenerLista()
        {
            List<RoutesVM> lista = new();
            try
            {
                var routes = await _genericRepositoryRoutes.ObtieneLista();
                lista = routes.Select(x => new RoutesVM
                {
                    bus_type = x.bus_type,
                    route_name = x.route_name,
                    start_time = x.start_time,
                    end_time = x.end_time,
                    fare = x.fare,
                    created_at = x.created_at,
                    updated_at = x.updated_at
                }).ToList();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return lista;
        }

        public async Task<RoutesVM> ObtenerPorId(int? id)
        {
            RoutesVM model = new();
            try
            {
                var routes = await _genericRepositoryRoutes.ObtenerPorId(id);
                model =  new RoutesVM()
                {
                    Id = routes.Id,
                    bus_type = routes.bus_type,
                    route_name = routes.route_name,
                    start_time = routes.start_time,
                    end_time = routes.end_time,
                    fare = routes.fare,
                    created_at = routes.created_at,
                    updated_at= routes.updated_at

                };
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
            return model;
        }
    }
}
