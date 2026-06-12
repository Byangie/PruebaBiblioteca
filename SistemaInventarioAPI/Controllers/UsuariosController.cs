using Microsoft.AspNetCore.Mvc;
using SistemaInventarioAPI.Models;
using SistemaInventarioAPI.Services;

namespace SistemaInventarioAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly BibliotecaService _bibliotecaService;

        public UsuariosController(BibliotecaService bibliotecaService)
        {
            _bibliotecaService = bibliotecaService;
        }

        [HttpGet]
        public ActionResult<List<Usuario>> ObtenerUsuarios()
        {
            return Ok(_bibliotecaService.ObtenerUsuarios());
        }

        [HttpPost]
        public ActionResult CrearUsuario(Usuario usuario)
        {
            try
            {
                var nuevoUsuario = _bibliotecaService.CrearUsuario(usuario);
                return Ok(nuevoUsuario);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}