using Domain.Entidades.ViewModels.Rutas;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using GeoJSON.Net.Geometry;
using System.Xml;
using NetTopologySuite.IO;
using GeoJSON.Net.Converters;

namespace BaseWeb.Controllers.API.Rutas
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIRutasController : ControllerBase
    {
        [HttpGet("obtenerRuta")]
        public async Task<IActionResult> ObtenerRuta([FromForm] RutasVM model)
        {
            using (var memoryStream = new MemoryStream())
            {
                var KmlFile = model.Ruta;
                await model.Ruta.CopyToAsync(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);

                var kml = KmlFile.(memoryStream);
                var converter = new GeoJsonConverter();

                var features = new GeoJSONObjectCollection();

                foreach (var placemark in kml.Root.Flatten().OfType<Placemark>())
                {
                    var feature = converter.Convert(placemark.Geometry);
                    features.Add(feature);
                }

                var serializerSettings = new Newtonsoft.Json.JsonSerializerSettings
                {
                    NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
                    Formatting = Newtonsoft.Json.Formatting.Indented
                };

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(features, serializerSettings);

                return json;
            }
        }
    }
}
    

