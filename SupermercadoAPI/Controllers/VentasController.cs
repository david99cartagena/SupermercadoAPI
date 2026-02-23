using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SupermercadoAPI.Data;
using SupermercadoAPI.DTOs;
using SupermercadoAPI.Services;

[ApiController]
[Route("api/[controller]")]
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
    
    // Datos Venta
    [HttpGet("datos-venta")]
    public async Task<IActionResult> DatosVenta()
    {
        var clientes = await _service.ListarClientes();
        var usuarios = await _service.ListarUsuarios();
        var productos = await _service.ListarProductos();

        return Ok(new { 
            Clientes = clientes, 
            Usuarios = usuarios, 
            Productos = productos 
        });
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
