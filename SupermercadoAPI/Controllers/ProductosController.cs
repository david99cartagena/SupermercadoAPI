using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SupermercadoAPI.Data;
using SupermercadoAPI.DTOs;
using SupermercadoAPI.Models;

[ApiController]
[Route("api/productos")]
public class ProductosController : ControllerBase
{
    private readonly SupermercadoContext _context;
    public ProductosController(
        SupermercadoContext context)
    {
        _context = context;
    }

    // Crear producto
    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] ProductoCrearDTO dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (
                string.IsNullOrWhiteSpace(dto.CodigoProducto) ||
                string.IsNullOrWhiteSpace(dto.NombreProducto)
            )
                return BadRequest("Campos obligatorios vacíos");

            if (dto.ValorUnitario <= 0)
                return BadRequest("Precio inválido");

            if (dto.UnidadesDisponibles < 0)
                return BadRequest("Stock inválido");

            var existe = await _context.Productos
            .AnyAsync(x => x.CodigoProducto == dto.CodigoProducto);

            if (existe)
                return BadRequest("Producto ya existe");

            var producto = new Producto
            {
                CodigoProducto = dto.CodigoProducto,
                NombreProducto = dto.NombreProducto,
                ValorUnitario = dto.ValorUnitario,
                UnidadesDisponibles = dto.UnidadesDisponibles,
                FechaCreacion = DateTime.Now
            };

            _context.Productos.Add(producto);

            await _context.SaveChangesAsync();

            return Ok(producto);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    // Editar producto
    [HttpPut("{id}")]
    public async Task<IActionResult> Editar(int id, [FromBody] ProductoEditarDTO dto)
    {
        try
        {
            var producto = await _context.Productos.FindAsync(id);

            if (producto == null)
                return NotFound("Producto no existe");

            // Codigo
            if (!string.IsNullOrWhiteSpace(dto.CodigoProducto))
            {
                var existeCodigo = await _context.Productos
                .AnyAsync(x => x.CodigoProducto == dto.CodigoProducto && x.Id != id);

                if (existeCodigo)
                    return BadRequest("Código ya existe");

                producto.CodigoProducto = dto.CodigoProducto;
            }

            // Nombre
            if (!string.IsNullOrWhiteSpace(dto.NombreProducto))
                producto.NombreProducto = dto.NombreProducto;

            // Precio
            if (dto.ValorUnitario != null)
            {
                if (dto.ValorUnitario <= 0)
                    return BadRequest("Precio inválido");

                producto.ValorUnitario = dto.ValorUnitario.Value;
            }

            // Stock
            if (dto.UnidadesDisponibles != null)
            {
                if (dto.UnidadesDisponibles < 0)
                    return BadRequest("Stock inválido");

                producto.UnidadesDisponibles = dto.UnidadesDisponibles.Value;
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensaje = "Producto actualizado",
                producto.Id,
                producto.NombreProducto
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    // Eliminar producto
    [HttpDelete("{id}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        try
        {
            var producto = await _context.Productos.FindAsync(id);

            if (producto == null)
                return NotFound("Producto no existe");

            _context.Productos.Remove(producto);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensaje = "Producto eliminado"
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    // Listar productos
    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        try
        {
            var productos = await _context
            .Productos
            .Select(x => new
            {
                x.Id,
                x.CodigoProducto,
                x.NombreProducto,
                x.ValorUnitario,
                x.UnidadesDisponibles
            })
            .ToListAsync();

            if (productos.Count == 0)
                return NotFound("No hay productos");

            return Ok(productos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    // Buscar por Id producto
    [HttpGet("{id}")]
    public async Task<IActionResult> Buscar(int id)
    {
        try
        {
            var producto = await _context
            .Productos
            .Where(x => x.Id == id)
            .Select(x => new
            {
                x.Id,
                x.CodigoProducto,
                x.NombreProducto,
                x.ValorUnitario,
                x.UnidadesDisponibles
            })
            .FirstOrDefaultAsync();

            if (producto == null)
                return NotFound("Producto no existe");

            return Ok(producto);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
