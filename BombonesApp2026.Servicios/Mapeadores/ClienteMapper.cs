using BombonesApp2026.Entidades.Entidades;
using BombonesApp2026.Servicios.DTOs.Cliente;

namespace BombonesApp2026.Servicios.Mapeadores
{
    public static class ClienteMapper
    {
        public static ClienteListDto ToListDto(this Cliente cliente)
        {
            return new ClienteListDto
            {
                ClienteId = cliente.ClienteId,
                NombreCompleto = $"{cliente.Nombre} {cliente.Apellido}",
                Telefono=cliente.Telefono,
                Direccion = $"{cliente.Calle} {cliente.Numero}",
                Ciudad = $"{cliente.Ciudad.Nombre}",
                Provincia = $"{cliente.Ciudad.Provincia!.NombreProvincia}",
                Activo = cliente.Activo
            };

        }
        public static ClienteEditDto ToEditDto(this Cliente cliente)
        {
            return new ClienteEditDto
            {
                ClienteId = cliente.ClienteId,
                Nombre = cliente.Nombre,
                Apellido = cliente.Apellido,
                Documento = cliente.Documento,
                Telefono = cliente.Telefono,
                Email = cliente.Email,
                Calle = cliente.Calle,
                Numero = cliente.Numero,
                CiudadId = cliente.CiudadId,
                ProvinciaId = cliente.Ciudad?.ProvinciaId ?? 0,
                CodigoPostal = cliente.CodigoPostal,
                Activo = cliente.Activo
            };
        }
        public static Cliente ToEntidad(this ClienteEditDto clienteDto)
        {
            return new Cliente
            {
                ClienteId = clienteDto.ClienteId,
                Nombre = clienteDto.Nombre,
                Apellido = clienteDto.Apellido,
                Documento = clienteDto.Documento,
                Telefono = clienteDto.Telefono,
                Email = clienteDto.Email,
                Calle = clienteDto.Calle,
                Numero = clienteDto.Numero,
                CiudadId = clienteDto.CiudadId,
                CodigoPostal = clienteDto.CodigoPostal,
                Activo = clienteDto.Activo
            };
        }
        public static Cliente ToEntidad(this ClienteCreateDto clienteDto)
        {
            return new Cliente
            {
                Nombre = clienteDto.Nombre,
                Apellido = clienteDto.Apellido,
                Documento = clienteDto.Documento,
                Telefono = clienteDto.Telefono,
                Email = clienteDto.Email,
                Calle = clienteDto.Calle,
                Numero = clienteDto.Numero,
                CiudadId = clienteDto.CiudadId,
                CodigoPostal = clienteDto.CodigoPostal
                // ClienteId no se mapea porque la BD genera la Identity
                // Activo no se asigna porque la entidad ya lo inicializa en 'true' por defecto
            };
        }
        public static ClienteCreateDto ToCreateDto(this ClienteEditDto clienteEditDto)
        {
            return new ClienteCreateDto
            {
                Nombre = clienteEditDto.Nombre,
                Apellido = clienteEditDto.Apellido,
                Documento = clienteEditDto.Documento,
                Telefono = clienteEditDto.Telefono,
                Email = clienteEditDto.Email,
                Calle = clienteEditDto.Calle,
                Numero = clienteEditDto.Numero,
                CiudadId = clienteEditDto.CiudadId,
                CodigoPostal = clienteEditDto.CodigoPostal
                // Se omiten ClienteId y Activo porque no pertenecen a ClienteCreateDto
            };
        }
    }
}
