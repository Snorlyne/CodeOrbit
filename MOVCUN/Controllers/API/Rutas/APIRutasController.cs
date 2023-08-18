using Domain.Entidades.ViewModels.Rutas;
using Microsoft.AspNetCore.Mvc;
using KmlToGeoJson;
using Newtonsoft.Json;
using GeoJSON.Net.Feature;
using System.Xml.Linq;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Xml.XPath;
using Domain.Entidades;
using Service.IServicio.User;
using Services.IServicio.Rutas;

namespace BaseWeb.Controllers.API.Rutas
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIRutasController : ControllerBase
    {
        private readonly IRutasServicio _rutasServicio;

        public APIRutasController(IRutasServicio rutasServicio)
        {
            _rutasServicio = rutasServicio;
        }

        [HttpPost("obtenerRuta")]
        public async Task<IActionResult> ObtenerRuta([FromForm] IFormFile model)
        {
            StreamReader r = new StreamReader(model.OpenReadStream());
            string jsonString = r.ReadToEnd();
            var response = await _rutasServicio.Crear(jsonString);
            return Ok(response);

        }
    }
}
    

