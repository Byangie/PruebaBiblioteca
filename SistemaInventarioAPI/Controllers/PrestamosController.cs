using Microsoft.AspNetCore.Mvc;
using SistemaInventarioAPI.Services;

namespace SistemaInventarioAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrestamosController : ControllerBase
    {
        private readonly BibliotecaService _bibliotecaService;

        public PrestamosController(BibliotecaService bibliotecaService)
        {
            _bibliotecaService = bibliotecaService;
        }

        [HttpGet]
        public IActionResult ObtenerPrestamos()
        {
            return Ok(_bibliotecaService.ObtenerPrestamos());
        }

        [HttpPost("prestar")]
        public IActionResult PrestarLibro(int libroId, int usuarioId)
        {
            try
            {
                var prestamo = _bibliotecaService.PrestarLibro(libroId, usuarioId);
                return Ok(prestamo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("devolver/{prestamoId}")]
        public IActionResult DevolverLibro(int prestamoId)
        {
            try
            {
                var resultado = _bibliotecaService.DevolverLibro(prestamoId);

                if (!resultado)
                    return NotFound("Préstamo no encontrado");

                return Ok("Libro devuelto correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}