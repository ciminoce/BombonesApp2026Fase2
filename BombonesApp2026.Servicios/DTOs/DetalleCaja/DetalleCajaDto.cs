using System.ComponentModel;

namespace BombonesApp2026.Servicios.DTOs.DetalleCaja
{
    public class DetalleCajaDto
    {
        public int BombonId { get; set; }
        public string NombreBombon { get; set; } = null!;
        [Browsable(false)]
        public decimal PrecioBombon { get; set; }
        public int Cantidad { get; set; }
        [Browsable(false)]
        public decimal Subtotal => PrecioBombon * Cantidad;
    }
}
