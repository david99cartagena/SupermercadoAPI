using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SupermercadoAPI.Data;
using SupermercadoAPI.DTOs;
using SupermercadoAPI.Models;

[ApiController]
[Route("api/[controller]")]
public class ClientesController : ControllerBase
{
    private readonly SupermercadoContext _context;
    public ClientesController(SupermercadoContext context)
    {
        _context = context;
    }

    // Buscar cliente por identificación
    [HttpGet("identificacion/{identificacion}")]
    public async Task<IActionResult> Buscar(string identificacion)
    {
        try
        {
            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(x => x.Identificacion == identificacion);

            if (cliente == null)
                return NotFound("Cliente no existe");

            return Ok(cliente);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    // Crear cliente
    [HttpPost]
    public async Task<IActionResult> Crear(ClienteCrearDTO dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existe = await _context.Clientes
                .AnyAsync(x => x.Identificacion == dto.Identificacion);

            if (existe)
                return BadRequest("Cliente ya existe");

            var cliente = new Cliente
            {
                Identificacion = dto.Identificacion,
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Direccion = dto.Direccion,
                Telefono = dto.Telefono,
                Email = dto.Email,
                FechaRegistro = DateTime.Now
            };

            _context.Clientes.Add(cliente);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                cliente.Id,
                cliente.Nombre,
                cliente.Identificacion
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    // Editar cliente
    [HttpPut("{id}")]
    public async Task<IActionResult> Editar(int id, ClienteEditarDTO dto)
    {
        try
        {
            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente == null)
                return NotFound("Cliente no existe");

            // Validar identificación si viene
            if (!string.IsNullOrWhiteSpace(dto.Identificacion))
            {
                var existeIdentificacion = await _context.Clientes
                .AnyAsync(x => x.Identificacion == dto.Identificacion && x.Id != id);

                if (existeIdentificacion)
                    return BadRequest("Identificación ya existe");

                cliente.Identificacion = dto.Identificacion;
            }

            // Validar email si viene
            if (!string.IsNullOrWhiteSpace(dto.Email))
            {
                var existeEmail = await _context.Clientes
                .AnyAsync(x => x.Email == dto.Email && x.Id != id);

                if (existeEmail)
                    return BadRequest("Email ya existe");

                cliente.Email = dto.Email;
            }

            // Actualizar solo si vienen datos
            if (!string.IsNullOrWhiteSpace(dto.Nombre))
                cliente.Nombre = dto.Nombre;

            if (!string.IsNullOrWhiteSpace(dto.Apellido))
                cliente.Apellido = dto.Apellido;

            if (!string.IsNullOrWhiteSpace(dto.Direccion))
                cliente.Direccion = dto.Direccion;

            if (!string.IsNullOrWhiteSpace(dto.Telefono))
                cliente.Telefono = dto.Telefono;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensaje = "Cliente actualizado",
                cliente.Id,
                cliente.Nombre
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    // Listar clientes
    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        try
        {
            var clientes = await _context
                .Clientes
                .Select(x => new
                {
                    x.Id,
                    x.Nombre,
                    x.Apellido,
                    x.Identificacion,
                    x.Telefono
                })
                .ToListAsync();

            if (clientes.Count == 0)
                return NotFound("No hay clientes");

            return Ok(clientes);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
