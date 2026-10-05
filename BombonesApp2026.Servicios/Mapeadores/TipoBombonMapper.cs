using Bombones2026.Servicios.DTOs.TipoBombon;
using BombonesApp2026.Entidades.Entidades;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace BombonesApp2026.Servicios.Mapeadores
{
    public static class TipoBombonMapper
    {
        public static TipoBombonListDto ToListDto(this TipoBombon tipo)
        {
            return new TipoBombonListDto
            {
                TipoBombonId = tipo.TipoBombonId,
                Nombre = tipo.Nombre,
                Activo = tipo.Activo,
            };
        }
        public static TipoBombon ToEntidad(this TipoBombonCreateDto tipoDto)
        {
            return new TipoBombon
            {
                Nombre = tipoDto.Nombre,
                Descripcion = tipoDto.Descripcion,
                Activo = true
            };
        }
        public static TipoBombon ToEntidad(this TipoBombonEditDto tipoDto)
        {
            return new TipoBombon
            {
                TipoBombonId = tipoDto.TipoBombonId,
                Nombre = tipoDto.Nombre,
                Descripcion = tipoDto.Descripcion,
                Activo = tipoDto.Activo

            };
        }
        public static TipoBombonEditDto ToEditDto(this TipoBombon tipo)
        {
            return new TipoBombonEditDto
            {
                TipoBombonId = tipo.TipoBombonId,
                Nombre = tipo.Nombre,
                Descripcion = tipo.Descripcion,
                Activo = tipo.Activo
            };
        }
    }
}
