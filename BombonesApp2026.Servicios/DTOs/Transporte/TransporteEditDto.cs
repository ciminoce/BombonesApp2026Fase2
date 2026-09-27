namespace Bombones2026.Servicios.DTOs.Transporte
{
    public class TransporteEditDto
    {
        public int TransporteId { get; set; }
        public string NombreEmpresa { get; set; } = null!;
        public string Telefono { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int ProvinciaId { get; set; }
        public bool Activo { get; set; }

    }
}
