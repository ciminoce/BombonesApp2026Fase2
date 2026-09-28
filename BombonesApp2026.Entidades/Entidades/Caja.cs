namespace BombonesApp2026.Entidades.Entidades
{
    public class Caja : Producto
    {

        public bool EsSurtida { get { return Detalles.Count > 1; } }

        public int CantidadBombones
        {
            get { return Detalles.Sum(d => d.Cantidad); }
        }
        public ICollection<DetalleCaja> Detalles { get; set; } = new List<DetalleCaja>();

        public override decimal Precio
        {
            get
            {
                decimal subTotal = Detalles.Sum(d => d.Subtotal);
                decimal recargoEmpaquetado = subTotal * 1.20M;
                return Math.Ceiling(recargoEmpaquetado / 100) * 100;
            }
        }

        public Caja() : base()
        {

        }
        public Caja(string nombre, int stock,
             bool activo = true, string? descripcion = null)
            : base(nombre, stock, activo, descripcion)
        {
        }
        public override string MostrarDatos()
        {
            return $"Caja: Nombre: {Nombre} - Precio:{Precio:C2} - Cantidad de Bombones: {CantidadBombones}u.";
        }

    }
}
