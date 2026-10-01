using BombonesApp2026.Servicios.DTOs.Bombon;
using BombonesApp2026.Servicios.DTOs.Caja;
using BombonesApp2026.Servicios.DTOs.DetalleCaja;
using BombonesApp2026.Servicios.Mapeadores;

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
            var detalle = _cajaDto.Detalles.FirstOrDefault(d => d.BombonId == bombon.ProductoId);
            if (detalle is null)
            {
                _cajaDto.Detalles.Add(new DetalleCajaCreateDto
                {
                    BombonId = bombon.ProductoId,
                    NombreBombon = bombon.Nombre,
                    Precio = bombon.Precio,
                    Cantidad = cantidad
                });
                return;
            }
            detalle.Cantidad += cantidad;

        }
        public (IReadOnlyCollection<DetalleCajaListDto> Detalles,
            int Cantidad, decimal Precio, bool EsSurtida) ObtenerResumen(){

            var detalles = _cajaDto.Detalles
                .Select(d => d.ToListDto()).ToList().AsReadOnly();
            var cantidad = _cajaDto.Detalles.Sum(d => d.Cantidad);
            var precio = Math.Ceiling(_cajaDto.Detalles
                .Sum(d => d.Precio * d.Cantidad) * 1.2m / 100) * 100;
            var esSurtida = _cajaDto.Detalles.Count > 1;
            return (detalles, cantidad, precio, esSurtida);
        }
    }
}
