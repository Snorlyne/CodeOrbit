using Domain.Entidades.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.IServicio.Routes;

namespace BaseWeb.Controllers.API.Routes
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIRoutesController : ControllerBase
    {
        private readonly IRoutesServicio _routesServicio;

        public APIRoutesController(IRoutesServicio routesServicio)
        {
            _routesServicio = routesServicio;
        }
        [HttpGet("list")]
        public async Task<IActionResult> ObtenerLista()
        {
            var response = await _routesServicio.ObtenerLista();
            return Ok(response);
        }
        [HttpGet("list/{id}")]
        public async Task<IActionResult> ObtenerPorId(int? id)
        {
            var routes = await _routesServicio.ObtenerPorId(id);
            return Ok(routes);
        }
        [HttpPost("create")]
        public async Task<ActionResult> Crear([FromForm] RoutesCreacionVM routes)
        {
            StreamReader r = new StreamReader(routes.GeoJSON.OpenReadStream());
            string jsonString = r.ReadToEnd();
            var response = await _routesServicio.Crear(routes, jsonString);
            return Ok(response);
        }
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Editar([FromForm] RoutesVM routes, int id)
        {
            var response = await _routesServicio.Editar(routes, id);
            return Ok(response);
        }
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> Eliminar(int? id)
        {
            var response = await _routesServicio.Eliminar(id);

            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
    }
}
