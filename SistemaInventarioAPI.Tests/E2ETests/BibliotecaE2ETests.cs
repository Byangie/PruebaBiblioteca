using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using SistemaInventarioAPI.Models;

namespace SistemaInventarioAPI.Tests.E2ETests
{
    public class BibliotecaE2ETests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public BibliotecaE2ETests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task FlujoCompleto_Biblioteca_FuncionaCorrectamente()
        {
            // 1. Crear usuario
            var usuario = new Usuario
            {
                Nombre = "Angelica",
                Correo = "angelica@test.com"
            };

            var usuarioResponse = await _client.PostAsJsonAsync("/api/Usuarios", usuario);

            Assert.Equal(HttpStatusCode.OK, usuarioResponse.StatusCode);

            var usuarioCreado = await usuarioResponse.Content.ReadFromJsonAsync<Usuario>();

            // 2. Crear libro
            var libro = new Libro
            {
                Titulo = "Boulevard",
                Autor = "Flor Salvador",
                Stock = 1
            };

            var libroResponse = await _client.PostAsJsonAsync("/api/Libros", libro);

            Assert.Equal(HttpStatusCode.OK, libroResponse.StatusCode);

            var libroCreado = await libroResponse.Content.ReadFromJsonAsync<Libro>();

            // 3. Prestar libro
            var prestamoResponse = await _client.PostAsync(
                $"/api/Prestamos/prestar?libroId={libroCreado!.Id}&usuarioId={usuarioCreado!.Id}",
                null
            );

            Assert.Equal(HttpStatusCode.OK, prestamoResponse.StatusCode);

            var prestamoCreado = await prestamoResponse.Content.ReadFromJsonAsync<Prestamo>();

            // 4. Verificar que stock bajó a 0
            var libroSinStockResponse = await _client.GetAsync($"/api/Libros/{libroCreado.Id}");
            var libroSinStock = await libroSinStockResponse.Content.ReadFromJsonAsync<Libro>();

            Assert.Equal(0, libroSinStock!.Stock);

            // 5. Devolver libro
            var devolucionResponse = await _client.PostAsync(
                $"/api/Prestamos/devolver/{prestamoCreado!.Id}",
                null
            );

            Assert.Equal(HttpStatusCode.OK, devolucionResponse.StatusCode);

            // 6. Verificar que stock volvió a 1
            var libroDevueltoResponse = await _client.GetAsync($"/api/Libros/{libroCreado.Id}");
            var libroDevuelto = await libroDevueltoResponse.Content.ReadFromJsonAsync<Libro>();

            Assert.Equal(1, libroDevuelto!.Stock);
        }
    }
}