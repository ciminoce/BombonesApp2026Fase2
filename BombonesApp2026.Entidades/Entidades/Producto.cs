namespace BombonesApp2026.Entidades.Entidades
{
    public abstract class Producto
    {
        // Campos privados
        private int _productoId;
        private string _nombre = null!;
        private string? _descripcion;
        private decimal _precio;
        private int _stock;

        // Propiedades con validación interna
        public int ProductoId
        {
            get => _productoId;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(ProductoId), "El ID del producto no puede ser negativo.");
                _productoId = value;
            }
        }

        public string Nombre
        {
            get => _nombre;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El nombre del producto es obligatorio.", nameof(Nombre));

                if (value.Trim().Length > 150)
                    throw new ArgumentException("El nombre del producto no puede superar los 150 caracteres.", nameof(Nombre));

                _nombre = value.Trim();
            }
        }

        public string? Descripcion
        {
            get => _descripcion;
            set
            {
                if (value != null && value.Trim().Length > 500)
                    throw new ArgumentException("La descripción no puede superar los 500 caracteres.", nameof(Descripcion));

                _descripcion = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
            }
        }

        public decimal Precio
        {
            get => _precio;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(Precio), "El precio no puede ser negativo.");
                _precio = value;
            }
        }

        public int Stock
        {
            get => _stock;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(Stock), "El stock no puede ser negativo.");
                _stock = value;
            }
        }

        // El tipo bool solo admite true/false (no requiere validación explícita)
        public bool Activo { get; set; } = true;

        // Constructores
        public Producto()
        {
        }

        public Producto(string nombre, decimal precio, int stock, bool activo = true, string? descripcion = null)
        {
            // Asignamos a través de las propiedades para disparar sus validaciones
            Nombre = nombre;
            Precio = precio;
            Stock = stock;
            Activo = activo;
            Descripcion = descripcion;
        }

        // Método abstracto a implementar por las clases derivadas
        public abstract string MostrarDatos();
    }
}
