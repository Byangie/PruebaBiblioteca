using SistemaInventarioAPI.Models;
using SistemaInventarioAPI.Services;
using Xunit;

namespace SistemaInventarioAPI.Tests.UnitTests
{
    public class BibliotecaServiceTests
    {
        [Fact]
        public void CrearLibro_DatosValidos_CreaLibroCorrectamente()
        {
            var service = new BibliotecaService();

            var libro = new Libro
            {
                Titulo = "Boulevard",
                Autor = "Flor Salvador",
                Stock = 10
            };

            var resultado = service.CrearLibro(libro);

            Assert.Equal(1, resultado.Id);
            Assert.Equal("Boulevard", resultado.Titulo);
            Assert.Equal("Flor Salvador", resultado.Autor);
            Assert.Equal(10, resultado.Stock);
        }

        [Fact]
        public void CrearLibro_TituloVacio_LanzaExcepcion()
        {
            var service = new BibliotecaService();

            var libro = new Libro
            {
                Titulo = "",
                Autor = "Autor de prueba",
                Stock = 5
            };

            Assert.Throws<Exception>(() => service.CrearLibro(libro));
        }

        [Fact]
        public void CrearLibro_AutorVacio_LanzaExcepcion()
        {
            var service = new BibliotecaService();

            var libro = new Libro
            {
                Titulo = "Libro de prueba",
                Autor = "",
                Stock = 5
            };

            Assert.Throws<Exception>(() => service.CrearLibro(libro));
        }

        [Fact]
        public void CrearLibro_StockNegativo_LanzaExcepcion()
        {
            var service = new BibliotecaService();

            var libro = new Libro
            {
                Titulo = "Libro de prueba",
                Autor = "Autor de prueba",
                Stock = -1
            };

            Assert.Throws<Exception>(() => service.CrearLibro(libro));
        }

        [Fact]
        public void CrearUsuario_DatosValidos_CreaUsuarioCorrectamente()
        {
            var service = new BibliotecaService();

            var usuario = new Usuario
            {
                Nombre = "Angelica",
                Correo = "angelica@test.com"
            };

            var resultado = service.CrearUsuario(usuario);

            Assert.Equal(1, resultado.Id);
            Assert.Equal("Angelica", resultado.Nombre);
            Assert.Equal("angelica@test.com", resultado.Correo);
        }

        [Fact]
        public void CrearUsuario_NombreVacio_LanzaExcepcion()
        {
            var service = new BibliotecaService();

            var usuario = new Usuario
            {
                Nombre = "",
                Correo = "correo@test.com"
            };

            Assert.Throws<Exception>(() => service.CrearUsuario(usuario));
        }

        [Fact]
        public void CrearUsuario_CorreoVacio_LanzaExcepcion()
        {
            var service = new BibliotecaService();

            var usuario = new Usuario
            {
                Nombre = "Angelica",
                Correo = ""
            };

            Assert.Throws<Exception>(() => service.CrearUsuario(usuario));
        }

        [Fact]
        public void PrestarLibro_ConStock_DisminuyeStock()
        {
            var service = new BibliotecaService();

            var libro = service.CrearLibro(new Libro
            {
                Titulo = "Boulevard",
                Autor = "Flor Salvador",
                Stock = 2
            });

            var usuario = service.CrearUsuario(new Usuario
            {
                Nombre = "Angelica",
                Correo = "angelica@test.com"
            });

            var prestamo = service.PrestarLibro(libro.Id, usuario.Id);

            var libroActualizado = service.ObtenerLibroPorId(libro.Id);

            Assert.NotNull(prestamo);
            Assert.Equal(1, libroActualizado!.Stock);
        }

        [Fact]
        public void PrestarLibro_SinStock_LanzaExcepcion()
        {
            var service = new BibliotecaService();

            var libro = service.CrearLibro(new Libro
            {
                Titulo = "Boulevard",
                Autor = "Flor Salvador",
                Stock = 0
            });

            var usuario = service.CrearUsuario(new Usuario
            {
                Nombre = "Angelica",
                Correo = "angelica@test.com"
            });

            Assert.Throws<Exception>(() => service.PrestarLibro(libro.Id, usuario.Id));
        }

        [Fact]
        public void DevolverLibro_PrestamoValido_AumentaStock()
        {
            var service = new BibliotecaService();

            var libro = service.CrearLibro(new Libro
            {
                Titulo = "Boulevard",
                Autor = "Flor Salvador",
                Stock = 1
            });

            var usuario = service.CrearUsuario(new Usuario
            {
                Nombre = "Angelica",
                Correo = "angelica@test.com"
            });

            var prestamo = service.PrestarLibro(libro.Id, usuario.Id);

            service.DevolverLibro(prestamo.Id);

            var libroActualizado = service.ObtenerLibroPorId(libro.Id);

            Assert.Equal(1, libroActualizado!.Stock);
        }
    }
}