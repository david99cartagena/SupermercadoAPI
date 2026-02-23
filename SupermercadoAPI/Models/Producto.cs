namespace SupermercadoAPI.Models
{
    public class Producto
    {
        public int Id { get; set; }
        public string CodigoProducto { get; set; } = "";
        public string NombreProducto { get; set; } = "";
        public decimal ValorUnitario { get; set; }
        public int UnidadesDisponibles { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
