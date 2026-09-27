using BombonesApp2026.Datos.EntityTypeConfigurations;
using BombonesApp2026.Entidades.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace BombonesApp2026.Datos
{
    public class BombonesDbContext:DbContext
    {
        public DbSet<Rol> Roles { get; set; }
        public DbSet<TipoBombon> TipoBombones { get; set; }
        public DbSet<Provincia> Provincias { get; set; }
        public DbSet<FormaDePago> FormasDePago { get; set; }
        public DbSet<Ciudad> Ciudades { get; set; }
        public DbSet<Transporte> Transportes { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Bombon> Bombones { get; set; }
        public DbSet<Caja> Cajas { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public BombonesDbContext(
            DbContextOptions<BombonesDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(RolEntityTypeConfiguration).Assembly);
        }
    }
}
