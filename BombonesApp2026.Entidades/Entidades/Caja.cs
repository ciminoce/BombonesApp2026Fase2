namespace BombonesApp2026.Entidades.Entidades
{
    public class Caja : Producto
    {

        public bool EsSurtida { get; set; }
        private int _cantidadBombones;

        public int CantidadBombones
        {
            get { return _cantidadBombones; }
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException("La cantidad de bombones debe ser un valor positivo.");

                _cantidadBombones = value;
            }
        }
        public Caja() : base()
        {

        }
        public Caja(string nombre, decimal precio, int stock, int cantidadBombones,
            bool esSurtida = true, bool activo = true, string? descripcion = null)
            : base(nombre, precio, stock, activo, descripcion)
        {
            CantidadBombones = cantidadBombones;
            EsSurtida = esSurtida;
        }
        public override string MostrarDatos()
        {
            return $"Caja: Nombre: {Nombre} - Precio:{Precio:C2} - Cantidad de Bombones: {CantidadBombones}u.";
        }

    }
}
