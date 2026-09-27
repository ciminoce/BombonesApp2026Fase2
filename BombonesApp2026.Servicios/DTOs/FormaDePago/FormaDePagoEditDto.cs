namespace Bombones2026.Servicios.DTOs.FormaDePago
{
    public class FormaDePagoEditDto
    {
        public int FormaDePagoId { get; set; }
        public string Nombre { get; set; } = null!;
        public bool Activo { get; set; }

    }
}
