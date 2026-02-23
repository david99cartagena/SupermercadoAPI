using System.ComponentModel.DataAnnotations;

namespace SupermercadoAPI.DTOs
{
    public class UsuarioEditarDTO
    {
        public string? Nombre { get; set; }
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string? Email { get; set; }
        public string? Password { get; set; }
        public bool? Activo { get; set; }
    }
}
