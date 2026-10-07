using Bombones2026.Servicios.DTOs.FormaDePago;
using Bombones2026.Servicios.DTOs.Paginacion;
using BombonesApp2026.Datos;
using BombonesApp2026.Datos.Interfaces;
using BombonesApp2026.Entidades.Entidades;
using BombonesApp2026.Servicios.Interfaces;
using BombonesApp2026.Servicios.Mapeadores;

namespace Bombones2026.Servicios.Servicios
{
    public class FormaDePagoServicio : IFormaDePagoServicio
    {
        private readonly IFormaDePagoRepositorio _formasDePagoRepositorio;
        private readonly IUnitOfWork _unitOfWork;

        public FormaDePagoServicio(IFormaDePagoRepositorio formasDePagoRepositorio, IUnitOfWork unitOfWork)
        {
            _formasDePagoRepositorio = formasDePagoRepositorio ?? throw new ArgumentNullException(nameof(formasDePagoRepositorio));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public ResultadoPaginacionDto<FormaDePagoListDto> ObtenerPagina(int paginaActual,
            int cantidadPorPagina, bool? filtroActivo = null, string? textoBuscar = null)
        {
            try
            {
                var resultado = _formasDePagoRepositorio.ObtenerPagina(paginaActual,
                    cantidadPorPagina, filtroActivo, textoBuscar);

                var listaDto = resultado.lista
                    .Select(f => f.ToListDto())
                    .ToList();

                return new ResultadoPaginacionDto<FormaDePagoListDto>
                {
                    Items = listaDto,
                    TotalRegistros = resultado.cantidadRegistros,
                    CantidadPorPagina = cantidadPorPagina,
                    PaginaActual = paginaActual
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al intentar obtener la página de formas de pago: {ex.Message}", ex);
            }
        }

        public List<FormaDePagoListDto> ObtenerTodos()
        {
            try
            {
                return _formasDePagoRepositorio.ObtenerTodos()
                    .Select(f => f.ToListDto())
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener la lista de formas de pago: {ex.Message}", ex);
            }
        }

        public int Agregar(FormaDePagoCreateDto? formaDePagoDto)
        {
            if (formaDePagoDto is null)
            {
                throw new ArgumentNullException(nameof(formaDePagoDto), "La forma de pago no puede ser nula");
            }
            if (string.IsNullOrWhiteSpace(formaDePagoDto.Nombre))
            {
                throw new ArgumentException("El nombre de la forma de pago es requerido", nameof(formaDePagoDto.Nombre));
            }

            FormaDePago formaDePago = formaDePagoDto.ToEntidad();

            if (_formasDePagoRepositorio.ExisteFormaDePago(formaDePago))
            {
                throw new InvalidOperationException($"Ya existe una forma de pago con el nombre '{formaDePago.Nombre}'");
            }

            try
            {
                _formasDePagoRepositorio.Agregar(formaDePago);
                _unitOfWork.Guardar();
                return formaDePago.FormaDePagoId;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al intentar agregar la forma de pago '{formaDePagoDto.Nombre}': {ex.Message}", ex);
            }
        }

        public void Borrar(int formaDePagoId)
        {
            if (formaDePagoId <= 0)
            {
                throw new ArgumentException("El ID de la forma de pago debe ser un entero mayor a cero.", nameof(formaDePagoId));
            }

            var formaDePagoInDb = _formasDePagoRepositorio.ObtenerPorId(formaDePagoId);
            if (formaDePagoInDb is null)
            {
                throw new KeyNotFoundException($"No se puede borrar. No existe ninguna forma de pago con el ID {formaDePagoId}.");
            }

            if (_formasDePagoRepositorio.TieneRegistrosRelacionados(formaDePagoId))
            {
                throw new InvalidOperationException($"No se puede eliminar la forma de pago (ID: {formaDePagoId}) porque tiene registros relacionados en el sistema.");
            }

            try
            {
                _formasDePagoRepositorio.Borrar(formaDePagoId);
                _unitOfWork.Guardar();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al intentar borrar la forma de pago con ID {formaDePagoId}: {ex.Message}", ex);
            }
        }

        public void Editar(FormaDePagoEditDto? formaDePagoDto)
        {
            if (formaDePagoDto is null)
            {
                throw new ArgumentNullException(nameof(formaDePagoDto), "La forma de pago no puede ser nula");
            }
            if (formaDePagoDto.FormaDePagoId <= 0)
            {
                throw new ArgumentException("El ID de la forma de pago no es válido.", nameof(formaDePagoDto.FormaDePagoId));
            }
            if (string.IsNullOrWhiteSpace(formaDePagoDto.Nombre))
            {
                throw new ArgumentException("El nombre de la forma de pago es requerido", nameof(formaDePagoDto.Nombre));
            }

            var formaInDb = _formasDePagoRepositorio.ObtenerPorId(formaDePagoDto.FormaDePagoId);
            if (formaInDb is null)
            {
                throw new KeyNotFoundException($"No existe ninguna forma de pago con el ID {formaDePagoDto.FormaDePagoId}.");
            }

            FormaDePago formaDePago = formaDePagoDto.ToEntidad();

            if (_formasDePagoRepositorio.ExisteFormaDePago(formaDePago))
            {
                throw new InvalidOperationException($"Ya existe otra forma de pago con el nombre '{formaDePago.Nombre}'");
            }

            try
            {
                _formasDePagoRepositorio.Editar(formaDePago, formaDePago.FormaDePagoId);
                _unitOfWork.Guardar();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al intentar editar la forma de pago con ID {formaDePagoDto.FormaDePagoId}: {ex.Message}", ex);
            }
        }

        public FormaDePagoEditDto ObtenerParaEditar(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("El ID de la forma de pago debe ser un entero mayor a cero.", nameof(id));
            }

            try
            {
                FormaDePago? formaDePago = _formasDePagoRepositorio.ObtenerPorId(id);
                if (formaDePago is null)
                {
                    throw new KeyNotFoundException($"No se encontró la forma de pago con ID {id}");
                }

                return formaDePago.ToEditDto();
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener la forma de pago con ID {id} para edición: {ex.Message}", ex);
            }
        }

        public List<FormaDePagoListDto> FiltrarPorActivo(bool activo)
        {
            try
            {
                return _formasDePagoRepositorio.FiltrarPorActivo(activo)
                    .Select(f => f.ToListDto())
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al filtrar formas de pago por estado activo ({activo}): {ex.Message}", ex);
            }
        }

        public int ObtenerPaginaRegistro(string nombre, int cantidadPorPagina,
            bool? filtroActivo = null, string? textoBuscar = null)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new ArgumentException("El nombre es requerido para calcular la página del registro.", nameof(nombre));
            }
            if (cantidadPorPagina <= 0)
            {
                throw new ArgumentException("La cantidad por página debe ser mayor a cero.", nameof(cantidadPorPagina));
            }

            try
            {
                int posicion = _formasDePagoRepositorio.ObtenerPosicionAlfabetica(nombre, filtroActivo, textoBuscar);
                return (int)Math.Ceiling((double)posicion / cantidadPorPagina);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener la posición de la página para la forma de pago '{nombre}': {ex.Message}", ex);
            }
        }
    }
}
