namespace BombonesApp2026.Servicios.DTOs.Cliente
{
    public class ClienteListDto
    {
        public int ClienteId { get; set; }
        public string NombreCompleto { get; set; } = null!;
        public string? Telefono { get; set; }
        public string? Direccion { get; set; }
        public string Ciudad { get; set; } = null!;
        public string Provincia { get; set; } = null!;
        public bool Activo { get; set; }
    }
}
