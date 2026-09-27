namespace Bombones2026.Servicios.DTOs.Ciudad
{
    public class CiudadEditDto
    {
        public int CiudadId { get; set; }
        public string Nombre { get; set; } = null!;
        public int ProvinciaId { get; set; }

    }
}
