using Bombones2026.Servicios.DTOs.Provincia;
using BombonesApp2026.Entidades.Entidades;

namespace BombonesApp2026.Servicios.Mapeadores
{
    public static class ProvinciaMapper
    {
        public static ProvinciaListDto ToListDto(this Provincia provincia)
        {
            if (provincia == null) throw new ArgumentNullException(nameof(provincia));
            return new ProvinciaListDto
            {
                ProvinciaId = provincia.ProvinciaId,
                Nombre = provincia.NombreProvincia,
            };
        }
        public static ProvinciaEditDto ToEditDto(this Provincia provincia)
        {
            if (provincia == null) throw new ArgumentNullException(nameof(provincia));
            return new ProvinciaEditDto
            {
                ProvinciaId = provincia.ProvinciaId,
                Nombre = provincia.NombreProvincia,
            };
        }
        public static Provincia ToEntidad (this ProvinciaCreateDto provinciaDto)
        {
            if (provinciaDto == null) throw new ArgumentNullException(nameof(provinciaDto));
            return new Provincia
            {
                NombreProvincia = provinciaDto.Nombre,
            };
        }
        public static Provincia ToEntidad(this ProvinciaEditDto provinciaDto)
        {
            if (provinciaDto == null) throw new ArgumentNullException(nameof(provinciaDto));
            return new Provincia
            {
                ProvinciaId = provinciaDto.ProvinciaId,
                NombreProvincia = provinciaDto.Nombre,
            };
        }
    }
}
