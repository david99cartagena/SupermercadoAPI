using System.ComponentModel.DataAnnotations;

namespace SupermercadoAPI.DTOs
{
    public class ProductoEditarDTO
    {
        public string? CodigoProducto { get; set; }
        public string? NombreProducto { get; set; }
        [Range(1, 1000000000, ErrorMessage = "Valor inválido")]
        public decimal? ValorUnitario { get; set; }
        [Range(0, 1000000, ErrorMessage = "Stock inválido")]
        public int? UnidadesDisponibles { get; set; }
    }
}
