using BombonesApp2026.Entidades.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BombonesApp2026.Datos.EntityTypeConfigurations
{
    public class FormaDePagoEntityTypeConfiguration : IEntityTypeConfiguration<FormaDePago>
    {
        public void Configure(EntityTypeBuilder<FormaDePago> builder)
        {
            builder.ToTable("FormasDePago");
            builder.HasKey(fp => fp.FormaDePagoId);
            builder.Property(fp => fp.Nombre)
                .IsRequired()
                .HasMaxLength(50);
            builder.HasIndex(fp => fp.Nombre,"IX_FormasDePago_Nombre")
                .IsUnique();
        }
    }
}
