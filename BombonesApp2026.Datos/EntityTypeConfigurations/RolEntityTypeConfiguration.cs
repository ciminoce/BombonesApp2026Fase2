using BombonesApp2026.Entidades.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BombonesApp2026.Datos.EntityTypeConfigurations
{
    public class RolEntityTypeConfiguration : IEntityTypeConfiguration<Rol>
    {
        public void Configure(EntityTypeBuilder<Rol> builder)
        {
            builder.ToTable("Roles");
            builder.HasKey(r => r.RolId);
            builder.Property(r => r.Nombre).HasMaxLength(30).IsRequired();
            builder.Property(r => r.Descripcion).HasMaxLength(100);
            builder.HasIndex(r => r.Nombre, "IX_Roles_Nombre").IsUnique();
        }
    }
}
