using Domain.Entidades.ViewModels;
using Domain.Util;
using Microsoft.Extensions.Logging;
using Repository.Context;
using Repository.Repositorio;
using Services.IServicio.Stops;
using Services.Servicio.Routes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Services.Servicio.Stops
{
    public class StopsServicio : IStopsServicio
    {
        private readonly ILogger _logger;
        private readonly GenericRepository<StopsVM> _genericRepositoryStops;

        public StopsServicio(ApplicationDBContext context, ILogger<StopsServicio> logger)
        {
            _logger = logger;
            _genericRepositoryStops = new GenericRepository<StopsVM>(context);
        }
        public async Task<ResponseHelper> Crear(StopsCreacionVM stops)
        {
            ResponseHelper response = new ResponseHelper();
            try
            {
                var stops1 = new StopsVM
                {
                    Latitude = stops.Latitude,
                    longitude = stops.longitude,
                    stop_name = stops.stop_name,
                    description = stops.description,
                    delay_time = stops.delay_time,
                    created_at = DateTime.Now,
                    updated_at = DateTime.Now
                };

                if (await _genericRepositoryStops.Crear(stops1) > 0)
                {
                    response.Success = true;
                    response.Message = "Creación exitosa";
                    response.HelperData = stops1.Id;
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

        public async Task<ResponseHelper> Editar(StopsVM stops, int? id)
        {
            ResponseHelper response = new ResponseHelper();
            try
            {
                var stops1 = await _genericRepositoryStops.BuscarPorId(id);

                if (stops1 != null)
                {
                    stops.Latitude = stops.Latitude;
                    stops.longitude = stops.Latitude;
                    stops.stop_name = stops.stop_name;
                    stops.description = stops.description;
                    stops.delay_time = stops.delay_time;
                    stops.created_at = stops.created_at;
                    stops.updated_at = stops.updated_at;


                    if (await _genericRepositoryStops.Actualizar(stops) > 0)
                    {
                        response.Success = true;
                        response.Message = "Se ha editado correctamente.";
                        response.HelperData = stops.Id;
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
                if (await _genericRepositoryStops.Eliminar(id) > 0)
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

        public async Task<List<StopsVM>> ObtenerLista()
        {
            List<StopsVM> lista = new();
            try
            {
                var stops = await _genericRepositoryStops.ObtieneLista();
                lista = stops.Select(x => new StopsVM
                {
                    Latitude = x.Latitude,
                    longitude = x.longitude,
                    stop_name = x.stop_name,
                    description = x.description,
                    delay_time = x.delay_time,
                    created_at = x.created_at,
                    updated_at = x.updated_at,
                }).ToList();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return lista;
        }

        public async Task<StopsVM> ObtenerPorId(int? id)
        {
            StopsVM model = new();
            try
            {
                var stops = await _genericRepositoryStops.BuscarPorId(id);
                model = new StopsVM()
                {
                    Id = stops.Id,
                    Latitude = stops.Latitude,
                    longitude = stops.longitude,
                    stop_name = stops.stop_name,
                    description = stops.description,
                    delay_time = stops.delay_time,
                    created_at = stops.created_at,
                    updated_at = stops.updated_at,
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
