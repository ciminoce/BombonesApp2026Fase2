using BombonesApp2026.Entidades.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BombonesApp2026.Datos.EntityTypeConfigurations
{
    public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            // Nombre de la tabla (opcional o según tu convención)
            builder.ToTable("Clientes");

            // Clave Primaria
            builder.HasKey(c => c.ClienteId);

            // Propiedades Requeridas y Longitudes Máximas
            builder.Property(c => c.Nombre)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(c => c.Apellido)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(c => c.Documento)
                .IsRequired()
                .HasMaxLength(20);

            // Índice único opcional para evitar documentos duplicados
            builder.HasIndex(c => c.Documento)
                .IsUnique();

            // Propiedades Opcionales (Nullable)
            builder.Property(c => c.Telefono)
                .IsRequired(false)
                .HasMaxLength(30);

            builder.Property(c => c.Email)
                .IsRequired(false)
                .HasMaxLength(100);

            builder.Property(c => c.Calle)
                .IsRequired(false)
                .HasMaxLength(100);

            builder.Property(c => c.Numero)
                .IsRequired(false)
                .HasMaxLength(20);

            builder.Property(c => c.CodigoPostal)
                .IsRequired(false)
                .HasMaxLength(10);

            // Propiedad Requerida
            builder.Property(c => c.Activo)
                .IsRequired()
                .HasDefaultValue(true);

            // Clave Foránea a Ciudad
            builder.Property(c => c.CiudadId)
                .IsRequired();

            // Relación con Ciudad (asumiendo que tenés la propiedad de navegación Ciudad en Cliente 
            // o configurándola vía FK si no tenés la navegación explícita)
            builder.HasOne(c=>c.Ciudad) // Reemplazá por el nombre de tu entidad Ciudad si difiere
                .WithMany(ci=>ci.Clientes)
                .HasForeignKey(c => c.CiudadId)
                .OnDelete(DeleteBehavior.Restrict); // Evita borrado en cascada accidental de la ciudad
        }
    }
}
