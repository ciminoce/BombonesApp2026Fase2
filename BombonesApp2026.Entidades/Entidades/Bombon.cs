namespace BombonesApp2026.Entidades.Entidades
{
    public class Bombon : Producto
    {
        // Campos privados
        private int _pesoEnGramos;
        private int _tipoBombonId;
        private decimal _precio;

        // Propiedades con validación interna
        public int PesoEnGramos
        {
            get => _pesoEnGramos;
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException(nameof(PesoEnGramos),
                        "El peso en gramos debe ser un valor mayor a cero.");

                _pesoEnGramos = value;
            }
        }

        public int TipoBombonId
        {
            get => _tipoBombonId;
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException(nameof(TipoBombonId),
                        "Debe asociar un tipo de bombón válido (TipoBombonId mayor a 0).");

                _tipoBombonId = value;
            }
        }

        // El tipo bool solo admite true/false (no requiere validación explícita)
        public bool TieneAzucar { get; set; }

        // Propiedad de navegación
        public TipoBombon? TipoBombon { get; set; }

        public override decimal Precio => _precio;

        // Constructores
        public Bombon() : base()
        {
        }
        public ICollection<DetalleCaja> Detalles { get; set; } = new List<DetalleCaja>();
        public Bombon(
            string nombre,
            decimal precio,
            int stock,
            int pesoEnGramos,
            bool tieneAzucar,
            int tipoBombonId,
            bool activo = true,
            string? descripcion = null)
            : base(nombre, stock, activo, descripcion)
        {
            // Asignamos a través de las propiedades para disparar sus validaciones
            PesoEnGramos = pesoEnGramos;
            TieneAzucar = tieneAzucar;
            TipoBombonId = tipoBombonId;
            CambiarPrecio(precio);
        }

        // Sobrescribimos el método abstracto de la clase Producto
        public override string MostrarDatos()
        {
            string estadoAzucar = TieneAzucar ? "Sí" : "No";
            return $"Bombón: {Nombre} | Precio: {Precio:C2} | Peso: {PesoEnGramos}g | Tiene Azúcar: {estadoAzucar}";
        }
        public void CambiarPrecio(decimal precio)
        {
            if (precio <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(precio),
                    "El precio debe ser positivo");
            }
            _precio= precio;
        }
    }
}
