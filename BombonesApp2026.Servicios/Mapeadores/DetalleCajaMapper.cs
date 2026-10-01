using BombonesApp2026.Entidades.Entidades;
using BombonesApp2026.Servicios.DTOs.DetalleCaja;

namespace BombonesApp2026.Servicios.Mapeadores
{
    public static class DetalleCajaMapper
    {
        public static DetalleCajaListDto ToListDto(this DetalleCaja detalle)
        {
            return new DetalleCajaListDto
            {
                BombonId = detalle.BombonId,
                NombreBombon = detalle.Bombon.Nombre,
                Cantidad = detalle.Cantidad,
            };
        }
        public static DetalleCajaListDto ToListDto(this DetalleCajaCreateDto detalleDto)
        {
            return new DetalleCajaListDto
            {
                BombonId = detalleDto.BombonId,
                NombreBombon = detalleDto.NombreBombon,
                Cantidad = detalleDto.Cantidad
            };
        }

    }
}
