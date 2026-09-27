namespace BombonesApp2026.Entidades.Entidades
{
    public class TipoBombon
    {
        // Campos privados
        private int _tipoBombonId;
        private string _nombre = null!;
        private string? _descripcion;

        // Propiedades con validación interna
        public int TipoBombonId
        {
            get => _tipoBombonId;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(TipoBombonId), "El ID del tipo de bombón no puede ser negativo.");

                _tipoBombonId = value;
            }
        }

        public string Nombre
        {
            get => _nombre;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El nombre del tipo de bombón es obligatorio.", nameof(Nombre));

                if (value.Trim().Length > 100)
                    throw new ArgumentException("El nombre del tipo de bombón no puede superar los 100 caracteres.", nameof(Nombre));

                _nombre = value.Trim();
            }
        }

        public string? Descripcion
        {
            get => _descripcion;
            set
            {
                if (value != null && value.Trim().Length > 300)
                    throw new ArgumentException("La descripción del tipo de bombón no puede superar los 300 caracteres.", nameof(Descripcion));

                _descripcion = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
            }
        }

        // El tipo bool solo admite true/false (no requiere validación explícita)
        public bool Activo { get; set; } = true;
        public ICollection<Bombon> Bombones { get; set; } = new List<Bombon>();

        // Constructores
        public TipoBombon()
        {
        }

        public TipoBombon(string nombre, string? descripcion = null, bool activo = true)
        {
            Nombre = nombre;
            Descripcion = descripcion;
            Activo = activo;
        }

        public TipoBombon(int tipoBombonId, string nombre, string? descripcion = null, bool activo = true)
            : this(nombre, descripcion, activo)
        {
            TipoBombonId = tipoBombonId;
        }
    }
}
