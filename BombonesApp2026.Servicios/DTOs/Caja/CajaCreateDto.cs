namespace BombonesApp2026.Servicios.DTOs.Caja
{
    public class CajaCreateDto
    {
        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }
        public int Stock { get; set; }
        public decimal Precio { get; set; }
        public int CantidadBombones { get; set; }
        public bool EsSurtida { get; set; }

    }
}
