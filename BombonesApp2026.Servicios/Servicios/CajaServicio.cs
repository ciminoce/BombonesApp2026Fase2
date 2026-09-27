using Bombones2026.Servicios.DTOs.Paginacion;
using BombonesApp2026.Entidades.Entidades;
using BombonesApp2026.Servicios.DTOs.Caja;
using BombonesApp2026.Servicios.Interfaces;
using BombonesApp2026.Servicios.Mapeadores;
using CajaesApp2026.Datos.Interfaces;

namespace CajaesApp2026.Servicios.Servicios
{
    public class CajaServicio : ICajaServicio
    {
        private readonly ICajaRepositorio _cajaRepositorio;

        public CajaServicio(ICajaRepositorio cajaRepositorio)
        {
            _cajaRepositorio = cajaRepositorio;
        }

        public int Agregar(CajaCreateDto cajaCreateDto)
        {
            Caja caja = cajaCreateDto.ToEntidad();
            if (_cajaRepositorio.ExisteCaja(caja))
            {
                throw new ArgumentException(nameof(caja), $"Ya existe un caja con el nombre {caja.Nombre}\n en esa provincia");
            }
            try
            {
                _cajaRepositorio.Agregar(caja);
                return caja.ProductoId;
            }
            catch (Exception ex)
            {

                throw new Exception($"Error al intentar guardar un caja {ex.Message}");
            }
        }

        public CajaEditDto? ObtenerParaEditar(int cajaId)
        {
            // AJUSTE: Validación defensiva del ID antes de operar
            if (cajaId <= 0)
                throw new ArgumentException("El ID caja debe ser entero mayor a cero.", nameof(cajaId));


            Caja? caja = _cajaRepositorio.ObtenerPorId(cajaId);
            if (caja is null) throw new ArgumentException(nameof(cajaId), $"Id {cajaId} no encontrado");
            CajaEditDto cajaDto = caja.ToEditDto();
            return cajaDto;
        }

        public void Editar(CajaEditDto cajaDto)
        {
            if (cajaDto == null)
                throw new ArgumentNullException(nameof(cajaDto), "La forma de pago no puede ser nula");
            if (cajaDto.ProductoId == 0)
            {
                throw new ArgumentException(nameof(cajaDto.ProductoId), "El ID del bombón debe ser mayor a 0");
            }
            Caja caja = cajaDto.ToEntidad();
            if (_cajaRepositorio.ExisteCaja(caja)) throw new InvalidOperationException($"Ya existe una caja {caja.Nombre}");
            _cajaRepositorio.Editar(caja);

        }

        public int ObtenerPaginaRegistro(string nombre, int cantidadPorPagina,
            bool? filtroActivo = null, string? textoBuscar = null)
        {
            int posicion = _cajaRepositorio
                .ObtenerPosicionAlfabetica(nombre, filtroActivo, textoBuscar);
            return (int)Math.Ceiling((double)posicion / cantidadPorPagina);
        }
        public ResultadoPaginacionDto<CajaListDto> ObtenerPagina(int paginaActual,
                    int cantidadPorPagina, bool? filtroActivo = null, string? textoBuscar = null)
        {
            try
            {
                var resultado = _cajaRepositorio.ObtenerPagina(paginaActual,
                    cantidadPorPagina, filtroActivo, textoBuscar);
                var listaDto = resultado.lista
                        .Select(b => b.ToListDto()).ToList();
                return new ResultadoPaginacionDto<CajaListDto>
                {
                    Items = listaDto,
                    TotalRegistros = resultado.cantidadRegistros,
                    CantidadPorPagina = cantidadPorPagina,
                    PaginaActual = paginaActual
                };
            }
            catch (Exception)
            {

                throw;
            }
        }



        public List<CajaListDto> ObtenerTodas()
        {
            var listaCajas = _cajaRepositorio.ObtenerTodos()
                .Select(c => c.ToListDto()).ToList();
            return listaCajas;
        }

        public void Borrar(int productoId)
        {
            if (productoId <= 0)
            {
                throw new ArgumentException(nameof(productoId),
                    "El ID debe ser positivo");
            }
            var bombon = _cajaRepositorio.ObtenerPorId(productoId);
            if (bombon is null)
            {
                throw new KeyNotFoundException($"No se encontró una caja con el ID {productoId}");
            }
            //OJO falta ver si el registro está relacionado
            try
            {
                _cajaRepositorio.Borrar(productoId);
            }
            catch (Exception ex)
            {

                throw new Exception($"Error al intentar borrar un bombon: {ex.Message}");
            }
        }

    }
}
