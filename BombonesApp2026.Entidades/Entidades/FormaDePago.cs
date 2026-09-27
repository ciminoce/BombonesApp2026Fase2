namespace BombonesApp2026.Entidades.Entidades
{
    public class FormaDePago
    {
        private int _formaDePagoId;
        private string _nombre = null!;

        public int FormaDePagoId
        {
            get => _formaDePagoId;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(FormaDePagoId), "El ID de la forma de pago no puede ser negativo.");
                _formaDePagoId = value;
            }
        }

        public string Nombre
        {
            get => _nombre;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El nombre de la forma de pago es obligatorio.", nameof(Nombre));

                if (value.Trim().Length > 50)
                    throw new ArgumentException("El nombre de la forma de pago no puede superar los 50 caracteres.", nameof(Nombre));

                _nombre = value.Trim();
            }
        }

        // El tipo bool solo admite true/false, por lo que no requiere validación explícita
        public bool Activo { get; set; } = true;
    }
}
