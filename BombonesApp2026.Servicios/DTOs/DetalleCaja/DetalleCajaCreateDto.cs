namespace BombonesApp2026.Servicios.DTOs.DetalleCaja
{
    public class DetalleCajaCreateDto
    {
        public int BombonId { get; set; }
        public string NombreBombon { get; set; } = null!;
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }

    }
}
