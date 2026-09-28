using BombonesApp2026.Entidades.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BombonesApp2026.Datos.EntityTypeConfigurations
{
    public class BombonEntityTypeConfiguration : IEntityTypeConfiguration<Bombon>
    {
        public void Configure(EntityTypeBuilder<Bombon> builder)
        {
            builder.ToTable("Bombones");
            builder.Property(b => b.TipoBombonId).IsRequired();
            builder.Property(b => b.TieneAzucar).IsRequired();
            builder.Property(b => b.PesoEnGramos)
                .IsRequired();

            builder.Property<decimal>("_precio")
                .HasColumnName("Precio")
                .HasPrecision(18,2)
                .IsRequired();

            builder.HasOne(b => b.TipoBombon)
                .WithMany(t=>t.Bombones)//Joder me faltó esto
                .HasForeignKey(b => b.TipoBombonId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
