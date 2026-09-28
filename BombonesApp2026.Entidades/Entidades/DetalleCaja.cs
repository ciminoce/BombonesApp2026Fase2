namespace BombonesApp2026.Entidades.Entidades
{
    public class DetalleCaja
    {
        public int CajaId { get; set; }
        public Caja Caja { get; set; } = null!;
        public int BombonId { get; set; }
        public Bombon Bombon { get; set; } = null!;
        public int Cantidad { get; set; }
        public decimal Subtotal => Bombon.Precio * Cantidad;
    }
}
