namespace SupermercadoAPI.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public bool Activo { get; set; }
        public DateTime FechaRegistro { get; set; }
        public List<Venta>? Ventas { get; set; }
    }
}
