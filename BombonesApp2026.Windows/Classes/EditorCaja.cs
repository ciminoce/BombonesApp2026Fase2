using BombonesApp2026.Servicios.DTOs.Bombon;
using BombonesApp2026.Servicios.DTOs.Caja;
using BombonesApp2026.Servicios.DTOs.DetalleCaja;

namespace BombonesApp2026.Windows.Classes
{
    public class EditorCaja
    {
        private CajaEditDto _cajaDto = null!;

        public EditorCaja(CajaEditDto cajaDto)
        {
            _cajaDto = cajaDto;
        }
        public void AgregarBombon(BombonListDto bombon, int cantidad)
        {
            if (bombon is null)
            {
                throw new ArgumentNullException(nameof(bombon),
                    "El bombón no puede ser nulo");
            }
            if (!bombon.Activo)
            {
                throw new InvalidOperationException("No se puede agregar un bombón inactivo");
            }
            if (cantidad <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(cantidad),
                    "La cantidad debe ser mayor a cero");
            }
            var detalle = _cajaDto.Detalles.FirstOrDefault(d => d.BombonId == bombon.ProductoId);
            if (detalle is null)
            {
                _cajaDto.Detalles.Add(new DetalleCajaDto
                {
                    BombonId = bombon.ProductoId,
                    NombreBombon = bombon.Nombre,
                    PrecioBombon = bombon.Precio,
                    Cantidad = cantidad
                });
                return;
            }
            detalle.Cantidad += cantidad;

        }
        public (IReadOnlyCollection<DetalleCajaDto> Detalles,
            int Cantidad, decimal Precio, bool EsSurtida) ObtenerResumen()
        {

            var detalles = _cajaDto.Detalles
                .ToList().AsReadOnly();
            var cantidad = _cajaDto.Detalles.Sum(d => d.Cantidad);
            var precio = Math.Ceiling(_cajaDto.Detalles
                .Sum(d => d.PrecioBombon * d.Cantidad) * 1.2m / 100) * 100;
            var esSurtida = _cajaDto.Detalles.Count > 1;
            return (detalles, cantidad, precio, esSurtida);
        }

        internal void EliminarBombon(int bombonId)
        {
            if (_cajaDto.Detalles.Count == 1)
            {
                throw new InvalidOperationException(
                    "El detalle debe tener al menos un bombón");
            }
            if (bombonId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(bombonId),
                    "El ID del bombón debe ser mayor a cero");
            }
            var detalleEnCaja = _cajaDto.Detalles
                .FirstOrDefault(d => d.BombonId == bombonId);
            if(detalleEnCaja is null)
            {
                throw new InvalidOperationException(
                    $"El bombón ID {bombonId} no forma parte del detalle");
            }
            _cajaDto.Detalles.Remove(detalleEnCaja);
        }

        internal void ModificarCantidadBombon(BombonListDto bombon,
            int cantidad)
        {
            if (bombon is null)
            {
                throw new ArgumentNullException(nameof(bombon),
                    "El bombón no puede ser nulo");
            }
            if (!bombon.Activo)
            {
                throw new InvalidOperationException("No se puede agregar un bombón inactivo");
            }
            if (cantidad <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(cantidad),
                    "La cantidad debe ser mayor a cero");
            }
            var detalleEnCaja = _cajaDto.Detalles
                .FirstOrDefault(d => d.BombonId == bombon.ProductoId);
            if(detalleEnCaja is null)
            {
                throw new InvalidOperationException($"El bombón ID {bombon.ProductoId}" +
                    $" no pertenece al detalle");
            }
            detalleEnCaja.Cantidad = cantidad;
        }
    }
}
