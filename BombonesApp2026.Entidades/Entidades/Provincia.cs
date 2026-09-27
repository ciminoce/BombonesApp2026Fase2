namespace BombonesApp2026.Entidades.Entidades
{
    public class Provincia
    {
        // Campos privados
        private int _provinciaId;
        private string _nombreProvincia = null!;
        private ICollection<Ciudad> _ciudades = new List<Ciudad>();
        private ICollection<Transporte> _transportes = new List<Transporte>();

        // Propiedades con validación interna
        public int ProvinciaId
        {
            get => _provinciaId;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(ProvinciaId), "El ID de la provincia no puede ser negativo.");

                _provinciaId = value;
            }
        }

        public string NombreProvincia
        {
            get => _nombreProvincia;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El nombre de la provincia es obligatorio.", nameof(NombreProvincia));

                if (value.Trim().Length > 100)
                    throw new ArgumentException("El nombre de la provincia no puede superar los 100 caracteres.", nameof(NombreProvincia));

                _nombreProvincia = value.Trim();
            }
        }

        // Propiedades de navegación (Colecciones)
        public ICollection<Ciudad> Ciudades
        {
            get => _ciudades;
            set => _ciudades = value ?? throw new ArgumentNullException(nameof(Ciudades), "La lista de ciudades no puede ser nula.");
        }

        public ICollection<Transporte> Transportes
        {
            get => _transportes;
            set => _transportes = value ?? throw new ArgumentNullException(nameof(Transportes), "La lista de transportes no puede ser nula.");
        }

        // Constructores
        public Provincia()
        {
        }

        public Provincia(string nombreProvincia)
        {
            NombreProvincia = nombreProvincia;
        }

        public Provincia(int provinciaId, string nombreProvincia)
        {
            ProvinciaId = provinciaId;
            NombreProvincia = nombreProvincia;
        }
    }
}
