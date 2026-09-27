using BombonesApp2026.Entidades.Entidades;
using BombonesApp2026.Servicios.DTOs.Caja;

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
                Precio = cajaDto.Precio,
                CantidadBombones = cajaDto.CantidadBombones,
                EsSurtida = cajaDto.EsSurtida,
            };
        }
        public static Caja ToEntidad(this CajaCreateDto cajaDto)
        {
            return new Caja
            {
                Nombre = cajaDto.Nombre,
                Descripcion=cajaDto.Descripcion,
                Stock = cajaDto.Stock,
                Precio = cajaDto.Precio,
                CantidadBombones = cajaDto.CantidadBombones,
                EsSurtida = cajaDto.EsSurtida,

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
                Precio = cajaDto.Precio,
                CantidadBombones = cajaDto.CantidadBombones,
                EsSurtida = cajaDto.EsSurtida,
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
                Precio = caja.Precio,
                CantidadBombones = caja.CantidadBombones,
                EsSurtida = caja.EsSurtida,
                Activo = caja.Activo
            };
        }
    }
}
