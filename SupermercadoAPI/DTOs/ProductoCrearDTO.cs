using System.ComponentModel.DataAnnotations;

namespace SupermercadoAPI.DTOs
{
    public class ProductoCrearDTO
    {
        [Required(ErrorMessage = "Código obligatorio")]
        [MinLength(2, ErrorMessage = "Código inválido")]
        public string CodigoProducto { get; set; } = "";

        [Required(ErrorMessage = "Nombre obligatorio")]
        [MinLength(3, ErrorMessage = "Nombre inválido")]
        public string NombreProducto { get; set; } = "";

        [Required(ErrorMessage = "Valor obligatorio")]
        [Range(1, 1000000000, ErrorMessage = "Valor debe ser mayor a 0")]
        public decimal ValorUnitario { get; set; }

        [Required(ErrorMessage = "Unidades obligatorias")]
        [Range(0, 1000000, ErrorMessage = "Unidades inválidas")]
        public int UnidadesDisponibles { get; set; }
    }
}
