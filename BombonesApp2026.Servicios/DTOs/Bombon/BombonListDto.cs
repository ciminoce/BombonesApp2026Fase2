namespace BombonesApp2026.Servicios.DTOs.Bombon
{
    public class BombonListDto
    {
        public int ProductoId { get; set; }
        public string Nombre { get; set; } = null!;
        public string TipoBombon { get; set; } = null!;
        public int Stock { get; set; }
        public decimal Precio { get; set; }
        public bool TieneAzucar { get; set; }
        public bool Activo { get; set; }
    }
}
