namespace BombonesApp2026.Entidades.Entidades
{
    public class Transporte
    {
        // Campos privados
        private int _transporteId;
        private string _nombreEmpresa = null!;
        private string _telefono = null!;
        private string _email = null!;
        private int _provinciaId;

        // Propiedades con validación interna
        public int TransporteId
        {
            get => _transporteId;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(TransporteId), "El ID del transporte no puede ser negativo.");

                _transporteId = value;
            }
        }

        public string NombreEmpresa
        {
            get => _nombreEmpresa;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El nombre de la empresa de transporte es obligatorio.", nameof(NombreEmpresa));

                if (value.Trim().Length > 100)
                    throw new ArgumentException("El nombre de la empresa no puede superar los 100 caracteres.", nameof(NombreEmpresa));

                _nombreEmpresa = value.Trim();
            }
        }

        public string Telefono
        {
            get => _telefono;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El teléfono es obligatorio.", nameof(Telefono));

                string limpio = value.Trim();

                if (limpio.Length < 6 || limpio.Length > 20)
                    throw new ArgumentException("El teléfono debe tener entre 6 y 20 caracteres.", nameof(Telefono));

                _telefono = limpio;
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El correo electrónico es obligatorio.", nameof(Email));

                string correo = value.Trim();

                // Validación de estructura básica de email
                if (!correo.Contains('@') || !correo.Contains('.'))
                    throw new ArgumentException("El formato del correo electrónico no es válido.", nameof(Email));

                if (correo.Length > 150)
                    throw new ArgumentException("El correo electrónico no puede superar los 150 caracteres.", nameof(Email));

                _email = correo;
            }
        }

        public int ProvinciaId
        {
            get => _provinciaId;
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException(nameof(ProvinciaId), "Debe asociar una provincia válida (ProvinciaId mayor a 0).");

                _provinciaId = value;
            }
        }

        // El tipo bool solo admite true/false (no requiere validación explícita)
        public bool Activo { get; set; } = true;

        // Propiedad de navegación
        public Provincia? Provincia { get; set; }

        // Constructores
        public Transporte()
        {
        }

        public Transporte(
            string nombreEmpresa,
            string telefono,
            string email,
            int provinciaId,
            bool activo = true)
        {
            NombreEmpresa = nombreEmpresa;
            Telefono = telefono;
            Email = email;
            ProvinciaId = provinciaId;
            Activo = activo;
        }

        public Transporte(
            int transporteId,
            string nombreEmpresa,
            string telefono,
            string email,
            int provinciaId,
            bool activo = true)
            : this(nombreEmpresa, telefono, email, provinciaId, activo)
        {
            TransporteId = transporteId;
        }
    }
}
