using System.ComponentModel.DataAnnotations;

namespace SupermercadoAPI.DTOs
{
    public class ClienteCrearDTO
    {
        [Required]
        [MinLength(3)]
        public string Identificacion { get; set; } = "";

        [Required]
        [MinLength(3)]
        public string Nombre { get; set; } = "";

        [Required]
        [MinLength(3)]
        public string Apellido { get; set; } = "";

        [Required]
        [MinLength(5)]
        public string Direccion { get; set; } = "";

        [Required]
        [MinLength(7)]
        public string Telefono { get; set; } = "";

        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";
    }
}
