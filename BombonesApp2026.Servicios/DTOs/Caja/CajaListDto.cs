namespace BombonesApp2026.Servicios.DTOs.Caja
{
    public class CajaListDto
    {
        public int ProductoId { get; set; }
        public string Nombre { get; set; } = null!;
        public int Stock { get; set; }
        public decimal Precio { get; set; }
        public int CantidadBombones { get; set; }
        public bool EsSurtida { get; set; }
        public bool Activo { get; set; }
    }
}
