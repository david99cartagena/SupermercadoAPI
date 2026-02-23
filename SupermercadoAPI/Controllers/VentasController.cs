using Microsoft.AspNetCore.Mvc;
using SupermercadoAPI.DTOs;
using SupermercadoAPI.Services;

[ApiController]
[Route("api/ventas")]
public class VentasController : ControllerBase
{
    private readonly VentaService _service;
    public VentasController(VentaService service)
    {
        _service = service;
    }

    // Crear venta
    [HttpPost]
    public async Task<IActionResult> CrearVenta([FromBody] VentaDTO dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultado =
                await _service.CrearVenta(dto);

            return Ok(resultado);
        }
        catch (Exception ex)
        {
            return StatusCode(400, ex.Message);
        }
    }

    // Detalle venta
    [HttpGet("{id}")]
    public async Task<IActionResult> Detalle(int id)
    {
        try
        {
            var venta = await _service.DetalleVenta(id);

            return Ok(venta);
        }
        catch (Exception ex)
        {
            return StatusCode(400, ex.Message);
        }
    }
}
