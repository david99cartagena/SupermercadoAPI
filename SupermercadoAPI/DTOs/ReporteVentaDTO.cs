namespace SupermercadoAPI.DTOs
{
    public class ReporteVentaDTO
    {
        public int VentaId { get; set; }
        public string Cliente { get; set; }
        public string Usuario { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
    }
}
