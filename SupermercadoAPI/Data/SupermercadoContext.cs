using Microsoft.EntityFrameworkCore;
using SupermercadoAPI.Models;

namespace SupermercadoAPI.Data

{
    public class SupermercadoContext : DbContext
    {
        public SupermercadoContext(DbContextOptions options) : base(options) { }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<DetalleVenta> DetalleVentas { get; set; }
    }
}