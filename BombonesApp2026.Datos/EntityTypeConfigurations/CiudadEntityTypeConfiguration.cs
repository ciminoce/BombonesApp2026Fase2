using BombonesApp2026.Entidades.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BombonesApp2026.Datos.EntityTypeConfigurations
{
    public class CiudadEntityTypeConfiguration : IEntityTypeConfiguration<Ciudad>
    {
        public void Configure(EntityTypeBuilder<Ciudad> builder)
        {
            builder.ToTable("Ciudades");
            builder.HasKey(c => c.CiudadId);
            builder.Property(c => c.Nombre).IsRequired()
                .HasColumnName("NombreCiudad")
                .HasMaxLength(100);
            builder.HasIndex(c => new {c.ProvinciaId, c.Nombre},"IX_Ciudades_ProvinciaId_NombreCiudad")
                .IsUnique();

            builder.HasOne(c => c.Provincia)
                .WithMany(p => p.Ciudades)
                .HasForeignKey(c => c.ProvinciaId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
