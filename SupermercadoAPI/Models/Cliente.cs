namespace SupermercadoAPI.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Identificacion { get; set; } = "";
        public string Nombre { get; set; } = "";
        public string Apellido { get; set; } = "";
        public string Direccion { get; set; } = "";
        public string Telefono { get; set; } = "";
        public string Email { get; set; } = "";
        public DateTime FechaRegistro { get; set; }
        public List<Venta>? Ventas { get; set; }
    }
}
