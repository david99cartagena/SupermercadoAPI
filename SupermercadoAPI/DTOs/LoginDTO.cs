using System.ComponentModel.DataAnnotations;

namespace SupermercadoAPI.DTOs
{
    public class LoginDTO
    {

        [Required(ErrorMessage = "Email obligatorio")]
        [MinLength(5, ErrorMessage = "Email obligatorio")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "Password obligatorio")]
        [MinLength(1, ErrorMessage = "Password obligatorio")]
        public string Password { get; set; } = "";
    }
}
