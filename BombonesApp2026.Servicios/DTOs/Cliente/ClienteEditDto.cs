namespace BombonesApp2026.Servicios.DTOs.Cliente
{
    public class ClienteEditDto
    {
        public int ClienteId { get; set; }

        public string Nombre { get; set; } = null!;

        public string Apellido { get; set; } = null!;

        public string Documento { get; set; } = null!;

        public string? Telefono { get; set; }
        public string? Email { get; set; }

        public string? Calle { get; set; }

        public string? Numero { get; set; }

        public int CiudadId { get; set; }
        public int ProvinciaId { get; set; }

        public string? CodigoPostal { get; set; }

        public bool Activo { get; set; }
    }
}
