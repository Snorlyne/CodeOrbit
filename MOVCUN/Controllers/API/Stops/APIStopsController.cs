using Domain.Entidades.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.IServicio.Routes;
using Services.IServicio.Stops;

namespace BaseWeb.Controllers.API.Stops
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIStopsController : ControllerBase
    {
        private readonly IStopsServicio _stopsServicio;

        public APIStopsController(IStopsServicio stopsServicio)
        {
            _stopsServicio = stopsServicio;
        }
        [HttpGet("list")]
        public async Task<IActionResult> ObtenerLista()
        {
            var response = await _stopsServicio.ObtenerLista();
            return Ok(response);
        }
        [HttpGet("list/{id}")]
        public async Task<IActionResult> ObtenerPorId(int? id)
        {
            var stops = await _stopsServicio.ObtenerPorId(id);
            return Ok(stops);
        }
        [HttpPost("create")]
        public async Task<ActionResult> Crear([FromForm] StopsCreacionVM stops)
        {
            var response = await _stopsServicio.Crear(stops);
            return Ok(response);
        }
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Editar([FromForm] StopsVM stops, int id)
        {
            var response = await _stopsServicio.Editar(stops, id);
            return Ok(response);
        }
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> Eliminar(int? id)
        {
            var response = await _stopsServicio.Eliminar(id);

            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
    }
}
