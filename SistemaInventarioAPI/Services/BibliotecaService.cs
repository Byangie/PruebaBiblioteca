using SistemaInventarioAPI.Models;

namespace SistemaInventarioAPI.Services
{
    public class BibliotecaService
    {
        private readonly List<Libro> _libros = new();
        private readonly List<Usuario> _usuarios = new();
        private readonly List<Prestamo> _prestamos = new();

        private int _siguienteLibroId = 1;
        private int _siguienteUsuarioId = 1;
        private int _siguientePrestamoId = 1;

        public List<Libro> ObtenerLibros()
        {
            return _libros;
        }

        public Libro? ObtenerLibroPorId(int id)
        {
            return _libros.FirstOrDefault(l => l.Id == id);
        }

        public Libro CrearLibro(Libro libro)
        {
            if (string.IsNullOrWhiteSpace(libro.Titulo))
                throw new Exception("El título es obligatorio");

            if (string.IsNullOrWhiteSpace(libro.Autor))
                throw new Exception("El autor es obligatorio");

            if (libro.Stock < 0)
                throw new Exception("El stock no puede ser negativo");

            libro.Id = _siguienteLibroId++;
            _libros.Add(libro);

            return libro;
        }

        public bool ActualizarLibro(int id, Libro libroActualizado)
        {
            var libro = ObtenerLibroPorId(id);

            if (libro == null)
                return false;

            if (string.IsNullOrWhiteSpace(libroActualizado.Titulo))
                throw new Exception("El título es obligatorio");

            if (string.IsNullOrWhiteSpace(libroActualizado.Autor))
                throw new Exception("El autor es obligatorio");

            if (libroActualizado.Stock < 0)
                throw new Exception("El stock no puede ser negativo");

            libro.Titulo = libroActualizado.Titulo;
            libro.Autor = libroActualizado.Autor;
            libro.Stock = libroActualizado.Stock;

            return true;
        }

        public bool EliminarLibro(int id)
        {
            var libro = ObtenerLibroPorId(id);

            if (libro == null)
                return false;

            _libros.Remove(libro);
            return true;
        }

        public List<Usuario> ObtenerUsuarios()
        {
            return _usuarios;
        }

        public Usuario CrearUsuario(Usuario usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario.Nombre))
                throw new Exception("El nombre del usuario es obligatorio");

            if (string.IsNullOrWhiteSpace(usuario.Correo))
                throw new Exception("El correo del usuario es obligatorio");

            usuario.Id = _siguienteUsuarioId++;
            _usuarios.Add(usuario);

            return usuario;
        }

        public List<Prestamo> ObtenerPrestamos()
        {
            return _prestamos;
        }

        public Prestamo PrestarLibro(int libroId, int usuarioId)
        {
            var libro = _libros.FirstOrDefault(l => l.Id == libroId);
            var usuario = _usuarios.FirstOrDefault(u => u.Id == usuarioId);

            if (libro == null)
                throw new Exception("El libro no existe");

            if (usuario == null)
                throw new Exception("El usuario no existe");

            if (libro.Stock <= 0)
                throw new Exception("No hay ejemplares disponibles");

            libro.Stock--;

            var prestamo = new Prestamo
            {
                Id = _siguientePrestamoId++,
                LibroId = libro.Id,
                UsuarioId = usuario.Id,
                FechaPrestamo = DateTime.Now,
                Devuelto = false
            };

            _prestamos.Add(prestamo);

            return prestamo;
        }

        public bool DevolverLibro(int prestamoId)
        {
            var prestamo = _prestamos.FirstOrDefault(p => p.Id == prestamoId);

            if (prestamo == null)
                return false;

            if (prestamo.Devuelto)
                throw new Exception("El préstamo ya fue devuelto");

            var libro = _libros.FirstOrDefault(l => l.Id == prestamo.LibroId);

            if (libro == null)
                throw new Exception("El libro no existe");

            libro.Stock++;
            prestamo.Devuelto = true;
            prestamo.FechaDevolucion = DateTime.Now;

            return true;
        }
    }
}