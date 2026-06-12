using Microsoft.AspNetCore.Mvc;
using SistemaInventarioAPI.Models;
using SistemaInventarioAPI.Services;

namespace SistemaInventarioAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LibrosController : ControllerBase
    {
        private readonly BibliotecaService _bibliotecaService;

        public LibrosController(BibliotecaService bibliotecaService)
        {
            _bibliotecaService = bibliotecaService;
        }

        [HttpGet]
        public ActionResult<List<Libro>> ObtenerLibros()
        {
            return Ok(_bibliotecaService.ObtenerLibros());
        }

        [HttpGet("{id}")]
        public ActionResult<Libro> ObtenerLibroPorId(int id)
        {
            var libro = _bibliotecaService.ObtenerLibroPorId(id);

            if (libro == null)
                return NotFound("Libro no encontrado");

            return Ok(libro);
        }

        [HttpPost]
        public ActionResult CrearLibro(Libro libro)
        {
            try
            {
                var nuevoLibro = _bibliotecaService.CrearLibro(libro);
                return Ok(nuevoLibro);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult ActualizarLibro(int id, Libro libro)
        {
            try
            {
                var actualizado = _bibliotecaService.ActualizarLibro(id, libro);

                if (!actualizado)
                    return NotFound("Libro no encontrado");

                return Ok("Libro actualizado correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult EliminarLibro(int id)
        {
            var eliminado = _bibliotecaService.EliminarLibro(id);

            if (!eliminado)
                return NotFound("Libro no encontrado");

            return Ok("Libro eliminado correctamente");
        }
    }
}