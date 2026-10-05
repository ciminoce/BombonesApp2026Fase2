using Bombones2026.Servicios.DTOs.Transporte;
using BombonesApp2026.Entidades.Entidades;

namespace BombonesApp2026.Servicios.Mapeadores
{
    public static class TransporteMapper
    {
        public static TransporteListDto ToListDto(this Transporte t)
        {
            return new TransporteListDto
            {
                TransporteId = t.TransporteId,
                NombreEmpresa = t.NombreEmpresa,
                Telefono = t.Telefono,
                Email = t.Email,
                Provincia = t.Provincia!.NombreProvincia,
                Activo = t.Activo

            };
        }
        public static TransporteEditDto ToEditDto(this Transporte transporte)
        {
            return new TransporteEditDto
            {
                TransporteId = transporte.TransporteId,
                NombreEmpresa = transporte.NombreEmpresa,
                Telefono = transporte.Telefono,
                Email = transporte.Email,
                ProvinciaId = transporte.ProvinciaId,
                Activo = transporte.Activo

            };
        }
        public static Transporte ToEntidad(this TransporteCreateDto transporteDto)
        {
            return new Transporte
            {
                NombreEmpresa = transporteDto.NombreEmpresa,
                Telefono = transporteDto.Telefono,
                Email = transporteDto.Email,
                ProvinciaId = transporteDto.ProvinciaId,
                Activo = true

            };
        }
        public static Transporte ToEntidad(this TransporteEditDto transporteDto)
        {
            return new Transporte
            {
                TransporteId = transporteDto.TransporteId,
                NombreEmpresa = transporteDto.NombreEmpresa,
                Telefono = transporteDto.Telefono,
                Email = transporteDto.Email,
                ProvinciaId = transporteDto.ProvinciaId,
                Activo = transporteDto.Activo

            };
        }
    }
}
