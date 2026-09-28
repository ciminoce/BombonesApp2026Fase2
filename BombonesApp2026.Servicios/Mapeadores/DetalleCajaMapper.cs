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
    }
}
