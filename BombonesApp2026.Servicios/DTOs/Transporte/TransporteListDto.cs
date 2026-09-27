namespace Bombones2026.Servicios.DTOs.Transporte
{
    public class TransporteListDto
    {
        public int TransporteId { get; set; }
        public string NombreEmpresa { get; set; } = null!;
        public string Provincia { get; set; } = null!;
        public string Telefono { get; set; } = null!;
        public string Email { get; set; } = null!;
        public bool Activo { get; set; }
    }
}
