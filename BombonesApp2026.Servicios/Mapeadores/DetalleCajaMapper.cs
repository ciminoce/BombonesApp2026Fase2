using BombonesApp2026.Entidades.Entidades;
using BombonesApp2026.Servicios.DTOs.DetalleCaja;

namespace BombonesApp2026.Servicios.Mapeadores
{
    public static class DetalleCajaMapper
    {
        public static DetalleCajaListDto ToDto(this DetalleCaja detalle)
        {
            return new DetalleCajaListDto
            {
                BombonId = detalle.BombonId,
                NombreBombon = detalle.Bombon.Nombre,
                PrecioBombon= detalle.Bombon.Precio,
                Cantidad = detalle.Cantidad,
            };
        }
        public static DetalleCajaListDto ToListDto(this DetalleCajaListDto detalleDto)
        {
            return new DetalleCajaListDto
            {
                BombonId = detalleDto.BombonId,
                NombreBombon = detalleDto.NombreBombon,
                PrecioBombon=detalleDto.PrecioBombon,
                Cantidad = detalleDto.Cantidad
            };
        }

    }
}
