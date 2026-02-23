using System.ComponentModel.DataAnnotations;
public class DetalleVentaDTO
{
    [Required(ErrorMessage = "Producto obligatorio")]
    [Range(1, int.MaxValue,
    ErrorMessage = "Producto inválido")]
    public int ProductoId { get; set; }

    [Required(ErrorMessage = "Cantidad obligatoria")]
    [Range(1, 10000,
    ErrorMessage = "Cantidad inválida")]
    public int Cantidad { get; set; }
}
