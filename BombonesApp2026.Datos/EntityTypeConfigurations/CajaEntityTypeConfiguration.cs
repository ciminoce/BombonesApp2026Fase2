using BombonesApp2026.Entidades.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BombonesApp2026.Datos.EntityTypeConfigurations
{
    public class CajaEntityTypeConfiguration : IEntityTypeConfiguration<Caja>
    {
        public void Configure(EntityTypeBuilder<Caja> builder)
        {
            builder.ToTable("Cajas");
            builder.Ignore(c=>c.CantidadBombones);
            builder.Ignore(c => c.EsSurtida);
            builder.Ignore(c => c.Precio);

            builder.HasMany(c => c.Detalles)
                .WithOne(d => d.Caja)
                .HasForeignKey(d => d.CajaId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
