namespace BombonesApp2026.Entidades.Entidades
{
    public class DetalleCaja
    {
        public DetalleCaja(Caja caja, Bombon bombon, int cantidad)
        {
            Caja = caja;
            CajaId = caja.ProductoId;
            Bombon = bombon;
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
