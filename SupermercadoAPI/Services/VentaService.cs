using SupermercadoAPI.Data;
using SupermercadoAPI.DTOs;
using SupermercadoAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace SupermercadoAPI.Services
{
    public class VentaService
    {
        private readonly SupermercadoContext _context;
        public VentaService(SupermercadoContext context)
        {
            _context = context;
        }
        public async Task<object> CrearVenta(VentaDTO dto)
        {
            using var transaction =
            await _context.Database.BeginTransactionAsync();

            try
            {
                // Validar lista
                if (dto.Detalles == null || dto.Detalles.Count == 0)
                    throw new Exception("Venta sin productos");

                // Cliente existe
                var cliente = await _context.Clientes.FindAsync(dto.ClienteId);

                if (cliente == null)
                    throw new Exception("Cliente no existe");

                // Usuario existe
                var usuario = await _context.Usuarios.FindAsync(dto.UsuarioId);

                if (usuario == null)
                    throw new Exception("Usuario no existe");

                // Productos duplicados
                var duplicados = dto.Detalles
                .GroupBy(x => x.ProductoId)
                .Any(x => x.Count() > 1);

                if (duplicados)
                    throw new Exception("Productos duplicados");

                decimal total = 0;

                var venta = new Venta
                {
                    ClienteId = dto.ClienteId,
                    UsuarioId = dto.UsuarioId,
                    FechaVenta = DateTime.Now,
                    Detalles = new List<DetalleVenta>()
                };

                foreach (var item in dto.Detalles)
                {
                    var producto = await _context.Productos.FindAsync(item.ProductoId);

                    if (producto == null)
                        throw new Exception($"Producto {item.ProductoId} no existe");

                    if (item.Cantidad <= 0)
                        throw new Exception("Cantidad inválida");


                    if (producto.UnidadesDisponibles < item.Cantidad)
                        throw new Exception($"Stock insuficiente: {producto.NombreProducto}");

                    producto.UnidadesDisponibles -= item.Cantidad;

                    var subtotal = producto.ValorUnitario * item.Cantidad;

                    total += subtotal;

                    venta.Detalles.Add(new DetalleVenta
                        {
                            ProductoId = item.ProductoId,
                            Cantidad = item.Cantidad,
                            ValorUnitario =
                            producto.ValorUnitario,
                            Subtotal = subtotal
                        });
                    }

                venta.TotalVenta = total;

                _context.Ventas.Add(venta);

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return new
                {
                    mensaje = "Venta creada",
                    ventaId = venta.Id,
                    total = total,
                    fecha = venta.FechaVenta
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                throw new Exception(ex.Message);
            }
        }

        public async Task<object> DetalleVenta(int id)
        {

            if (id <= 0)
                throw new Exception("Id inválido");

            var venta = await _context.Ventas
            .Where(v => v.Id == id)
            .Select(v => new
            {
                VentaId = v.Id,
                Cliente = v.Cliente.Nombre,
                Usuario = v.Usuario.Nombre,
                Fecha = v.FechaVenta,
                Total = v.TotalVenta,
                Productos = v.Detalles.Select(d => new {
                    Producto =d.Producto.NombreProducto,
                    Cantidad = d.Cantidad,
                    Precio = d.ValorUnitario,
                    Subtotal = d.Subtotal
                })
            })
            .FirstOrDefaultAsync();

            if (venta == null)
                throw new Exception("Venta no existe");

            return venta;
        }
    }
}
