using System.ComponentModel.DataAnnotations;

namespace SupermercadoAPI.DTOs
{
    public class VentaDTO
    {
        [Required(ErrorMessage = "Cliente obligatorio")]
        [Range(1, int.MaxValue,
        ErrorMessage = "Cliente inválido")]
        public int ClienteId { get; set; }

        [Required(ErrorMessage = "Usuario obligatorio")]
        [Range(1, int.MaxValue,
        ErrorMessage = "Usuario inválido")]
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "Debe agregar productos")]
        [MinLength(1,
        ErrorMessage = "Debe agregar productos")]
        public List<DetalleVentaDTO> Detalles { get; set; } = new();
    }
}
