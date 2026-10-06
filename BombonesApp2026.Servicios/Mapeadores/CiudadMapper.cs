using Bombones2026.Servicios.DTOs.Ciudad;
using BombonesApp2026.Entidades.Entidades;

namespace BombonesApp2026.Servicios.Mapeadores
{
    public static class CiudadMapper
    {
        public static CiudadListDto ToListDto(this Ciudad ciudad)
        {
            if (ciudad == null) throw new ArgumentNullException(nameof(ciudad));
            return new CiudadListDto
            {
                CiudadId = ciudad.CiudadId,
                Ciudad = ciudad.Nombre,
                Provincia = ciudad.Provincia!.NombreProvincia
            };
        }
        public static CiudadEditDto ToEditDto(this Ciudad ciudad)
        {
            if (ciudad == null) throw new ArgumentNullException(nameof(ciudad));
            return new CiudadEditDto
            {
                CiudadId = ciudad.CiudadId,
                Nombre = ciudad.Nombre,
                ProvinciaId = ciudad.ProvinciaId
            };
        }
        public static Ciudad ToEntidad(this CiudadCreateDto ciudadDto)
        {
            if (ciudadDto == null) throw new ArgumentNullException(nameof(ciudadDto));
            return new Ciudad
            {
                Nombre = ciudadDto.Nombre,
                ProvinciaId = ciudadDto.ProvinciaId
            };
        }
        public static Ciudad ToEntidad(this CiudadEditDto ciudadDto)
        {
            if (ciudadDto == null) throw new ArgumentNullException(nameof(ciudadDto));
            return new Ciudad
            {
                CiudadId = ciudadDto.CiudadId,
                Nombre = ciudadDto.Nombre,
                ProvinciaId = ciudadDto.ProvinciaId
            };
        }

    }
}
