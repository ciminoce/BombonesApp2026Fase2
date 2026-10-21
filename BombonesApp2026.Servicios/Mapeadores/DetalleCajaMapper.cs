using BombonesApp2026.Entidades.Entidades;
using BombonesApp2026.Servicios.DTOs.DetalleCaja;

namespace BombonesApp2026.Servicios.Mapeadores
{
    public static class DetalleCajaMapper
    {
        public static DetalleCajaDto ToDto(this DetalleCaja detalle)
        {
            return new DetalleCajaDto
            {
                BombonId = detalle.BombonId,
                NombreBombon = detalle.Bombon.Nombre,
                PrecioBombon= detalle.Bombon.Precio,
                Cantidad = detalle.Cantidad,
            };
        }
        public static DetalleCajaDto ToListDto(this DetalleCajaDto detalleDto)
        {
            return new DetalleCajaDto
            {
                BombonId = detalleDto.BombonId,
                NombreBombon = detalleDto.NombreBombon,
                PrecioBombon=detalleDto.PrecioBombon,
                Cantidad = detalleDto.Cantidad
            };
        }

    }
}
