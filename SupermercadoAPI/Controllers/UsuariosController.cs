using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SupermercadoAPI.Data;
using SupermercadoAPI.DTOs;
using SupermercadoAPI.Models;

[ApiController]
[Route("api/usuarios")]
public class UsuariosController : ControllerBase
{
    private readonly SupermercadoContext _context;
    public UsuariosController(SupermercadoContext context)
    {
        _context = context;
    }

    // Crear Usuario
    [HttpPost("crear")]
    public async Task<IActionResult> CrearUsuario([FromBody] UsuarioCrearDTO dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existe = await _context.Usuarios
                .AnyAsync(x => x.Email == dto.Email);

            if (existe)
                return BadRequest("Usuario ya existe");

            var usuario = new Usuario
            {
                Nombre = dto.Nombre,
                Email = dto.Email,
                //Password = dto.Password,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Activo = true,
                FechaRegistro = DateTime.Now
            };

            _context.Usuarios.Add(usuario);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                usuario.Id,
                usuario.Nombre,
                usuario.Email
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    // Editar Usuario
    [HttpPut("{id}")]
    public async Task<IActionResult> Editar(int id, UsuarioEditarDTO dto)
    {
        try
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
                return NotFound("Usuario no existe");

            // Nombre
            if (!string.IsNullOrWhiteSpace(dto.Nombre))
                usuario.Nombre = dto.Nombre;

            // Email
            if (!string.IsNullOrWhiteSpace(dto.Email))
            {
                var existeEmail = await _context.Usuarios
                .AnyAsync(x =>
                    x.Email == dto.Email &&
                    x.Id != id);

                if (existeEmail)
                    return BadRequest("Email ya existe");

                usuario.Email = dto.Email;
            }

            // Password
            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                usuario.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            }

            // Activo
            if (dto.Activo != null)
                usuario.Activo = dto.Activo.Value;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensaje = "Usuario actualizado",
                usuario.Id,
                usuario.Nombre
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    // Login Usuario
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(x =>
                x.Email == dto.Email &&
                //x.Password == dto.Password &&
                x.Activo == true
            );

            if (usuario == null)
                return Unauthorized("Usuario no existe");

            bool passwordCorrecto = BCrypt.Net.BCrypt.Verify(dto.Password, usuario.Password);

            if (!passwordCorrecto)
                return Unauthorized("Password incorrecto");

            return Ok(new
            {
                usuario.Id,
                usuario.Nombre,
                usuario.Email
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    // Listar Usuarios
    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        try
        {
            var usuarios = await _context.Usuarios
                .Select(x => new
                {
                    x.Id,
                    x.Nombre,
                    x.Email
                })
                .ToListAsync();

            if (usuarios.Count == 0)
                return NotFound("No hay usuarios");

            return Ok(usuarios);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    // Buscar por Id usuario
    [HttpGet("{id}")]
    public async Task<IActionResult> Buscar(int id)
    {
        try
        {
            var usuario = await _context.Usuarios
            .Where(x => x.Id == id)
            .Select(x => new
            {
                x.Id,
                x.Nombre,
                x.Email
            })
            .FirstOrDefaultAsync();

            if (usuario == null)
                return NotFound("Usuario no existe");

            return Ok(usuario);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
