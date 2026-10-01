namespace BombonesApp2026.Entidades.Entidades
{
    public class Caja : Producto
    {
        private List<DetalleCaja> _detalles = new();
        public bool EsSurtida { get { return _detalles.Count > 1; } }

        public int CantidadBombones
        {
            get { return _detalles.Sum(d => d.Cantidad); }
        }
        public IReadOnlyCollection<DetalleCaja> Detalles => _detalles;

        public override decimal Precio
        {
            get
            {
                decimal subTotal = _detalles.Sum(d => d.Subtotal);
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

        public void AgregarBombon(Bombon bombon, int cantidad)
        {
            if(bombon is null)
            {
                throw new ArgumentNullException(nameof(bombon),
                    "El bombón no puede ser nulo");
            }
            if (!bombon.Activo)
            {
                throw new InvalidOperationException("No se puede agregar un bombón inactivo");
            }
            var detalle = _detalles.FirstOrDefault(d => d.BombonId == bombon.ProductoId);
            if(detalle is null)
            {
                _detalles.Add(new DetalleCaja(this, bombon, cantidad));
                return;
            }
            detalle.Cantidad += cantidad;
        }
    }
}
