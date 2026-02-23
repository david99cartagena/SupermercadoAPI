using System.ComponentModel.DataAnnotations;

namespace SupermercadoAPI.DTOs
{
    public class UsuarioCrearDTO
    {

        [Required(ErrorMessage = "Nombre obligatorio")]
        [MinLength(3, ErrorMessage = "Mínimo 3 caracteres")]
        public string Nombre { get; set; } = "";

        [Required(ErrorMessage = "Email obligatorio")]
        [MinLength(5, ErrorMessage = "Email obligatorio")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "Password obligatorio")]
        [MinLength(4, ErrorMessage = "Mínimo 4 caracteres")]
        public string Password { get; set; } = "";
    }
}
