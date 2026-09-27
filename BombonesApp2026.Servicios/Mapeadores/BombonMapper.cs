using BombonesApp2026.Entidades.Entidades;
using BombonesApp2026.Servicios.DTOs.Bombon;

namespace BombonesApp2026.Servicios.Mapeadores
{
    public static class BombonMapper
    {
        public static BombonListDto ToListDto(this Bombon bombon)
        {
            return new BombonListDto
            {
                ProductoId = bombon.ProductoId,
                Nombre = bombon.Nombre,
                Precio = bombon.Precio,
                Stock = bombon.Stock,
                TipoBombon = bombon.TipoBombon!.Nombre,
                TieneAzucar = bombon.TieneAzucar,
                Activo = bombon.Activo
            };
        }
        public static Bombon ToEntidad(this BombonCreateDto bombonDto)
        {
            return new Bombon
            {
                Nombre = bombonDto.Nombre,
                Descripcion = bombonDto.Descripcion,
                Precio = bombonDto.Precio,
                Stock = bombonDto.Stock,
                Activo = bombonDto.Activo,
                TieneAzucar = bombonDto.TieneAzucar,
                PesoEnGramos = bombonDto.PesoEnGramos,
                TipoBombonId = bombonDto.TipoBombonId
            };
        }
        public static BombonEditDto ToEditDto(this Bombon bombon)
        {
            return new BombonEditDto
            {
                ProductoId = bombon.ProductoId,
                Nombre = bombon.Nombre,
                Descripcion = bombon.Descripcion,
                Precio = bombon.Precio,
                Stock = bombon.Stock,
                Activo = bombon.Activo,
                TieneAzucar = bombon.TieneAzucar,
                PesoEnGramos = bombon.PesoEnGramos,
                TipoBombonId = bombon.TipoBombonId

            };
        }
        public static Bombon ToEntidad(this BombonEditDto bombonDto)
        {
            return new Bombon
            {
                ProductoId = bombonDto.ProductoId,
                Nombre = bombonDto.Nombre,
                Descripcion = bombonDto.Descripcion,
                Precio = bombonDto.Precio,
                Stock = bombonDto.Stock,
                Activo = bombonDto.Activo,
                TieneAzucar = bombonDto.TieneAzucar,
                PesoEnGramos = bombonDto.PesoEnGramos,
                TipoBombonId = bombonDto.TipoBombonId
            };
        }

        public static BombonCreateDto ToCreateDto(this BombonEditDto bombonDto)
        {
            return new BombonCreateDto
            {
                Nombre = bombonDto.Nombre,
                Descripcion = bombonDto.Descripcion,
                Precio = bombonDto.Precio,
                Stock = bombonDto.Stock,
                Activo = bombonDto.Activo,
                TieneAzucar = bombonDto.TieneAzucar,
                PesoEnGramos = bombonDto.PesoEnGramos,
                TipoBombonId = bombonDto.TipoBombonId

            };
        }
    }
}
