namespace SistemaInventarioAPI.Models
{
    public class Prestamo
    {
        public int Id { get; set; }
        public int LibroId { get; set; }
        public int UsuarioId { get; set; }
        public DateTime FechaPrestamo { get; set; }
        public bool Devuelto { get; set; }
        public DateTime? FechaDevolucion { get; set; }
    }
}