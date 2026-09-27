using BombonesApp2026.Entidades.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BombonesApp2026.Datos.EntityTypeConfigurations
{
    public class TransporteEntityTypeConfiguration : IEntityTypeConfiguration<Transporte>
    {
        public void Configure(EntityTypeBuilder<Transporte> builder)
        {
            builder.ToTable("Transportes");
            builder.HasKey(t => t.TransporteId);
            builder.Property(t=>t.NombreEmpresa).IsRequired()
                .HasMaxLength(50);
            builder.Property(t=>t.Telefono).IsRequired().HasMaxLength(20);
            builder.Property(t => t.Email).IsRequired().HasMaxLength(100);
            builder.HasIndex(t => new { t.ProvinciaId, t.NombreEmpresa },
                        "IX_Transportes_ProvinciaId_NombreEmpresa").IsUnique();
            builder.HasOne(t => t.Provincia)
                .WithMany(p => p.Transportes)
                .HasForeignKey(t => t.ProvinciaId)
                .OnDelete(DeleteBehavior.Restrict);


        }
    }
}
