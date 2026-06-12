using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using SistemaInventarioAPI.Models;

namespace SistemaInventarioAPI.Tests.IntegrationTests
{
    public class BibliotecaIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public BibliotecaIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CrearLibro_YConsultarLibros_RetornaLibroCreado()
        {
            var libro = new Libro
            {
                Titulo = "El Principito",
                Autor = "Antoine de Saint-Exupéry",
                Stock = 5
            };

            var postResponse = await _client.PostAsJsonAsync("/api/Libros", libro);

            Assert.Equal(HttpStatusCode.OK, postResponse.StatusCode);

            var getResponse = await _client.GetAsync("/api/Libros");

            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

            var libros = await getResponse.Content.ReadFromJsonAsync<List<Libro>>();

            Assert.NotNull(libros);
            Assert.Contains(libros, l => l.Titulo == "El Principito");
        }

        [Fact]
        public async Task CrearUsuario_YConsultarUsuarios_RetornaUsuarioCreado()
        {
            var usuario = new Usuario
            {
                Nombre = "Angelica",
                Correo = "angelica@test.com"
            };

            var postResponse = await _client.PostAsJsonAsync("/api/Usuarios", usuario);

            Assert.Equal(HttpStatusCode.OK, postResponse.StatusCode);

            var getResponse = await _client.GetAsync("/api/Usuarios");

            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

            var usuarios = await getResponse.Content.ReadFromJsonAsync<List<Usuario>>();

            Assert.NotNull(usuarios);
            Assert.Contains(usuarios, u => u.Nombre == "Angelica");
        }

        [Fact]
        public async Task PrestarLibro_DisminuyeStock()
        {
            var libro = new Libro
            {
                Titulo = "El Principito",
                Autor = "Antoine de Saint-Exupéry",
                Stock = 2
            };

            var libroResponse = await _client.PostAsJsonAsync("/api/Libros", libro);
            var libroCreado = await libroResponse.Content.ReadFromJsonAsync<Libro>();

            var usuario = new Usuario
            {
                Nombre = "Angelica",
                Correo = "angelica@test.com"
            };

            var usuarioResponse = await _client.PostAsJsonAsync("/api/Usuarios", usuario);
            var usuarioCreado = await usuarioResponse.Content.ReadFromJsonAsync<Usuario>();

            var prestamoResponse = await _client.PostAsync(
                $"/api/Prestamos/prestar?libroId={libroCreado!.Id}&usuarioId={usuarioCreado!.Id}",
                null
            );

            Assert.Equal(HttpStatusCode.OK, prestamoResponse.StatusCode);

            var getLibroResponse = await _client.GetAsync($"/api/Libros/{libroCreado.Id}");
            var libroActualizado = await getLibroResponse.Content.ReadFromJsonAsync<Libro>();

            Assert.Equal(1, libroActualizado!.Stock);
        }
    }
}

