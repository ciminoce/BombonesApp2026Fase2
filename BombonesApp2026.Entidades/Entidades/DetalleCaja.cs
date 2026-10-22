namespace BombonesApp2026.Entidades.Entidades
{
    public class DetalleCaja
    {
        public DetalleCaja(Caja caja, Bombon bombon, int cantidad)
        {
            CajaId = caja.ProductoId;
            BombonId = bombon.ProductoId;
            Cantidad = cantidad;
        }
        public DetalleCaja()
        {

        }
        public int CajaId { get; set; }
        public Caja Caja { get; set; } = null!;
        public int BombonId { get; set; }
        public Bombon Bombon { get; set; } = null!;
        public int Cantidad { get; set; }
        public decimal Subtotal => Bombon.Precio * Cantidad;
    }
}
