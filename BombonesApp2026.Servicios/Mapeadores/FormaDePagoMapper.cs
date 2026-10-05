using Bombones2026.Servicios.DTOs.FormaDePago;
using BombonesApp2026.Entidades.Entidades;
using Microsoft.SqlServer.Server;

namespace BombonesApp2026.Servicios.Mapeadores
{
    public static class FormaDePagoMapper
    {
        public static FormaDePagoListDto ToListDto(this FormaDePago formaDePago)
        {
            return new FormaDePagoListDto
            {
                FormaDePagoId = formaDePago.FormaDePagoId,
                Nombre = formaDePago.Nombre,
                Activo = formaDePago.Activo,
            };
        }
        public static FormaDePago ToEntidad(this FormaDePagoCreateDto formaDto)
        {
            return new FormaDePago
            {
                Nombre = formaDto.Nombre,
                Activo = true
            };
        }
        public static FormaDePago ToEntidad(this FormaDePagoEditDto formaDto)
        {
            return new FormaDePago
            {
                FormaDePagoId=formaDto.FormaDePagoId,
                Nombre = formaDto.Nombre,
                Activo = formaDto.Activo
            };

        }
        public static FormaDePagoEditDto ToEditDto(this FormaDePago forma)
        {
            return new FormaDePagoEditDto
            {
                FormaDePagoId = forma.FormaDePagoId,
                Nombre = forma.Nombre,
                Activo = forma.Activo

            };
        }
    }
}
