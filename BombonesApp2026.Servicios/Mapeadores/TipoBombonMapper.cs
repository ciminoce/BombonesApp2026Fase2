using Bombones2026.Servicios.DTOs.TipoBombon;
using BombonesApp2026.Entidades.Entidades;

namespace BombonesApp2026.Servicios.Mapeadores
{
    public static class TipoBombonMapper
    {
        public static TipoBombonListDto ToListDto(this TipoBombon tipo)
        {
            return new TipoBombonListDto
            {
                TipoBombonId = tipo.TipoBombonId,
                Nombre = tipo.Nombre
            };
        }
    }
}
