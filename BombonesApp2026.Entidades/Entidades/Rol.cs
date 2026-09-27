namespace BombonesApp2026.Entidades.Entidades
{
    public class Rol
    {
        // Campos privados
        private int _rolId;
        private string _nombre = null!;
        private string? _descripcion;

        // Propiedades con validación interna
        public int RolId
        {
            get => _rolId;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(RolId), "El ID del rol no puede ser negativo.");

                _rolId = value;
            }
        }

        public string Nombre
        {
            get => _nombre;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El nombre del rol es obligatorio.", nameof(Nombre));

                if (value.Trim().Length > 50)
                    throw new ArgumentException("El nombre del rol no puede superar los 50 caracteres.", nameof(Nombre));

                _nombre = value.Trim();
            }
        }

        public string? Descripcion
        {
            get => _descripcion;
            set
            {
                if (value != null && value.Trim().Length > 250)
                    throw new ArgumentException("La descripción del rol no puede superar los 250 caracteres.", nameof(Descripcion));

                _descripcion = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
            }
        }

        // El tipo bool solo admite true/false (no requiere validación explícita)
        public bool Activo { get; set; } = true;

        // Constructores
        public Rol()
        {
        }

        public Rol(string nombre, string? descripcion = null, bool activo = true)
        {
            Nombre = nombre;
            Descripcion = descripcion;
            Activo = activo;
        }

        public Rol(int rolId, string nombre, string? descripcion = null, bool activo = true)
            : this(nombre, descripcion, activo)
        {
            RolId = rolId;
        }
    }
}
