using Domain.Entidades;
using Domain.Entidades.ViewModels;
using Domain.Util;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Repository.Repositorio;
using Services.IServicio.Routes;
using Service.Servicio.User;
using Repository.Context;
using System.Linq.Expressions;
using static System.Formats.Asn1.AsnWriter;

namespace Services.Servicio.Route
{
    public class RoutesServicio : IRoutesServicio
    {
        private readonly ILogger _logger;
        private readonly GenericRepository<Routes> _genericRepositoryRoutes;
        private readonly GenericRepository<Stops> _genericRepositoryStops;
        private readonly GenericRepository<RouteStop> _genericRepositoryRouteStops;

        public RoutesServicio(ApplicationDBContext context, ILogger<RoutesServicio> logger)
        {
            _logger = logger;
            _genericRepositoryRoutes = new GenericRepository<Routes>(context);
            _genericRepositoryStops = new GenericRepository<Stops>(context);
            _genericRepositoryRouteStops = new GenericRepository<RouteStop>(context);
        }

        public async Task<ResponseHelper> Crear(RoutesCreacionVM routes, string json)
        {
            ResponseHelper response = new ResponseHelper();
            try
            {
                
                Routes routes1 = new Routes
                {
                    bus_type = routes.bus_type,
                    route_name = routes.route_name,
                    start_time = TimeSpan.FromMinutes(2),
                    end_time = TimeSpan.FromMinutes(2),
                    fare = routes.fare,
                    created_at = DateTime.Now,
                    updated_at = DateTime.Now,
                    IsDeleted = false,
                };

                if (await _genericRepositoryRoutes.Crear(routes1) > 0)
                {
                    int id_route = routes1.Id;

                    JObject jObject = JsonConvert.DeserializeObject<JObject>(json);
                    var featuresArray = jObject["features"].Skip(1).ToArray();

                    var newJson = new JObject
                    {
                        ["type"] = jObject["type"],
                        ["features"] = new JArray(featuresArray)
                    };

                    foreach (var feature in featuresArray)
                    {
                        Stops stops = new();

                        var featureObject = (JObject)feature;

                        var properties = (JObject)featureObject["properties"];
                        stops.stop_name = properties["name"].ToString();
                        
                        if (properties["description"] != null)
                        {
                            stops.description = properties["description"].ToString();
                        }
                        else
                        {
                            stops.description = "Sin descripción";
                        }

                        var geometryObject = (JObject)featureObject["geometry"];
                        var coordinates = geometryObject["coordinates"];
                        stops.Latitude = coordinates[0].ToObject<decimal>();
                        stops.longitude = coordinates[1].ToObject<decimal>();

                        stops.IsDeleted = false;
                        stops.created_at = DateTime.Now;
                        stops.updated_at = DateTime.Now;

                        if(await _genericRepositoryStops.Crear(stops) > 0)
                        {
                            RouteStop routeStop = new();
                            routeStop.id_route = routes1.Id;
                            routeStop.stop_id = stops.Id;
                            await _genericRepositoryRouteStops.Crear(routeStop);
                        }
                    }

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
                Routes routes2 = await _genericRepositoryRoutes.BuscarPorId(id);

                if (routes2 != null)
                {
                    routes2.bus_type = routes.bus_type;
                    routes2.route_name = routes.route_name;
                    routes2.start_time = routes.start_time;
                    routes2.end_time = routes.end_time;
                    routes2.fare = routes.fare;
                    routes2.created_at = DateTime.Now;
                    routes2.updated_at = DateTime.Now;

                    if (await _genericRepositoryRoutes.Actualizar(routes2) > 0)
                    {
                        response.Success = true;
                        response.Message = "Se ha editado correctamente.";
                        response.HelperData = routes2.Id;
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
            List<StopsListaVM> lista = new();
            try
            {
                Expression<Func<RouteStop, bool>> query = x => x.id_route == id;
                List<RouteStop> stops = await _genericRepositoryRouteStops.ObtieneLista("Stop");


                lista = stops.Select(x => new StopsListaVM
                {
                    stop_name = x.Stop.stop_name,
                    Latitude = x.Stop.Latitude,
                    longitude = x.Stop.longitude, 
                    description = x.Stop.description,
                    delay_time = x.Stop.delay_time

                }).ToList();

                var routes = await _genericRepositoryRoutes.ObtenerPorId(id);
                model = new RoutesVM()
                {
                    Id = routes.Id,
                    bus_type = routes.bus_type,
                    route_name = routes.route_name,
                    start_time = routes.start_time,
                    end_time = routes.end_time,
                    fare = routes.fare,
                    created_at = routes.created_at,
                    updated_at = routes.updated_at,
                    stops = lista

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
