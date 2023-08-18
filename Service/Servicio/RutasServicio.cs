using Domain.Entidades;
using Domain.Entidades.ViewModels.Rutas;
using Domain.Util;
using GeoJSON.Net.Geometry;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Repository.Context;
using Repository.Repositorio;
using Services.IServicio.Rutas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Services.Servicio
{
    public class RutasServicio : IRutasServicio
    {
        private readonly ApplicationDBContext _context;
        //private readonly GenericRepository<Routes> _genericRepositoryRoutes;
        private readonly IConfiguration _configuration;

        public RutasServicio(ApplicationDBContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<ResponseHelper> Crear(string json)
        {
            ResponseHelper response = new ResponseHelper();

            try
            {
                List<RoutesVM> routes = new List<RoutesVM>();

                JObject jObject = JsonConvert.DeserializeObject<JObject>(json);
                var featuresArray = jObject["features"].Skip(1).ToArray();

                var newJson = new JObject
                {
                    ["type"] = jObject["type"],
                    ["features"] = new JArray(featuresArray)
                };
                foreach (var feature in featuresArray)
                {
                    var featureObject = (JObject)feature;
                    RoutesVM routeVM = new RoutesVM();

                    var properties =  (JObject)featureObject["properties"];
                    routeVM.Nombre = properties["name"].ToString();

                    var geometryObject = (JObject)featureObject["geometry"];
                    var coordinates = geometryObject["coordinates"];

                    routeVM.Latitud = coordinates[0].ToObject<double>();
                    routeVM.Longitud = coordinates[1].ToObject<double>();

                    routes.Add(routeVM);
                }
                response.HelperData = routes;
                return response;
            }
            catch (Exception ex)
            {

            }
            return response;
        }
    }
}
