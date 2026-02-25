using Microsoft.EntityFrameworkCore;
using ComercializadoraelExito.Models;

namespace ComercializadoraelExito.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Producto> Productos { get; set; }

        public DbSet<Factura> Facturas { get; set; }

        public DbSet<DetalleFactura> DetallesFactura { get; set; }
    }
}