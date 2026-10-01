using BombonesApp2026.Entidades.Entidades;
using BombonesApp2026.Servicios.DTOs.Caja;
using BombonesApp2026.Servicios.DTOs.DetalleCaja;

namespace BombonesApp2026.Servicios.Mapeadores
{
    public static class CajaMapper
    {
        public static CajaListDto ToListDto(this Caja caja)
        {
            return new CajaListDto
            {
                ProductoId = caja.ProductoId,
                Nombre = caja.Nombre,
                Stock = caja.Stock,
                Precio = caja.Precio,
                CantidadBombones = caja.CantidadBombones,
                EsSurtida = caja.EsSurtida,
                Activo=caja.Activo
            };
        }
        public static CajaCreateDto ToCreateDto(this CajaEditDto cajaDto)
        {
            return new CajaCreateDto
            {
                Nombre = cajaDto.Nombre,
                Descripcion=cajaDto.Descripcion,
                Stock = cajaDto.Stock,
            };
        }
        public static Caja ToEntidad(this CajaCreateDto cajaDto)
        {
            return new Caja
            {
                Nombre = cajaDto.Nombre,
                Descripcion=cajaDto.Descripcion,
                Stock = cajaDto.Stock,

            };
        }
        public static Caja ToEntidad(this CajaEditDto cajaDto)
        {
            return new Caja
            {
                ProductoId=cajaDto.ProductoId,
                Nombre = cajaDto.Nombre,
                Descripcion = cajaDto.Descripcion,
                Stock = cajaDto.Stock,
                Activo=cajaDto.Activo
            };
        }
        public static CajaEditDto ToEditDto(this Caja caja)
        {
            return new CajaEditDto
            {
                ProductoId = caja.ProductoId,
                Nombre = caja.Nombre,
                Descripcion = caja.Descripcion,
                Stock = caja.Stock,
                Activo = caja.Activo
            };
        }
        public static CajaDetailDto ToDetailDto(this Caja caja)
        {
            return new CajaDetailDto
            {
                ProductoId = caja.ProductoId,
                Nombre = caja.Nombre,
                Descripcion = caja.Descripcion,
                Stock = caja.Stock,
                Precio = caja.Precio,
                CantidadBombones = caja.CantidadBombones,
                EsSurtida = caja.EsSurtida,
                Activo = caja.Activo,
                Detalles=caja.Detalles.Select(d=>d.ToListDto()).ToList()??new List<DetalleCajaListDto>()
            };
        }
    }
}
