using BombonesApp2026.Servicios.DTOs.DetalleCaja;

namespace BombonesApp2026.Servicios.DTOs.Caja
{
    public class CajaEditDto
    {
        public int ProductoId { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }
        public int Stock { get; set; }
        public bool Activo { get; set; }
        public List<DetalleCajaCreateDto> Detalles { get; set; } = new();
    }
}
