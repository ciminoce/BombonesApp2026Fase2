namespace BombonesApp2026.Entidades.Entidades
{
    public class Ciudad
    {
        private int _ciudadId;
        private string _nombre = null!;
        private int _provinciaId;

        public int CiudadId
        {
            get => _ciudadId;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(CiudadId), "El ID de la ciudad no puede ser negativo.");
                _ciudadId = value;
            }
        }

        public string Nombre
        {
            get => _nombre;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El nombre de la ciudad es obligatorio.", nameof(Nombre));

                if (value.Trim().Length > 100)
                    throw new ArgumentException("El nombre de la ciudad no puede superar los 100 caracteres.", nameof(Nombre));

                _nombre = value.Trim();
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

        // Propiedad de navegación (Entity Framework)
        public Provincia? Provincia { get; set; }
        public ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();
    }
}
