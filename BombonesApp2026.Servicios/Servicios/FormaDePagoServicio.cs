using Bombones2026.Servicios.DTOs.FormaDePago;
using Bombones2026.Servicios.DTOs.Paginacion;
using BombonesApp2026.Datos.Interfaces;
using BombonesApp2026.Entidades.Entidades;
using BombonesApp2026.Servicios.Interfaces;

namespace Bombones2026.Servicios.Servicios
{
    public class FormaDePagoServicio : IFormaDePagoServicio
    {
        private readonly IFormaDePagoRepositorio _formasDePagoRepositorio;
        public FormaDePagoServicio(IFormaDePagoRepositorio formasDePagoRepositorio)
        {
            _formasDePagoRepositorio = formasDePagoRepositorio;
        }

        public ResultadoPaginacionDto<FormaDePagoListDto> ObtenerPagina(int paginaActual,
                int cantidadPorPagina, bool? filtroActivo = null, string? textoBuscar = null)
        {
            try
            {
                var resultado = _formasDePagoRepositorio.ObtenerPagina(paginaActual,
                    cantidadPorPagina, filtroActivo, textoBuscar);
                var listaDto = resultado.lista
                        .Select(f => new FormaDePagoListDto
                        {
                            FormaDePagoId = f.FormaDePagoId,
                            Nombre = f.Nombre,
                            Activo = f.Activo,
                        }).ToList();
                return new ResultadoPaginacionDto<FormaDePagoListDto>
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


        public List<FormaDePagoListDto> ObtenerTodos()
        {
            return _formasDePagoRepositorio.ObtenerTodos()
                .Select(f => new FormaDePagoListDto
                {
                    FormaDePagoId = f.FormaDePagoId,
                    Nombre = f.Nombre,
                    Activo = f.Activo,
                }).ToList();
        }
        public int Agregar(FormaDePagoCreateDto? formaDePagoDto)
        {
            if (formaDePagoDto == null)
                throw new ArgumentNullException(nameof(formaDePagoDto), "La forma de pago no puede ser nula");
            if (string.IsNullOrWhiteSpace(formaDePagoDto.Nombre))
                throw new ArgumentException(nameof(formaDePagoDto.Nombre), "El nombre de la forma de pago es requerido");
            FormaDePago formaDePago = new FormaDePago
            {
                Nombre = formaDePagoDto.Nombre,
                Activo = true//Nuevas formas de pago son activas por defecto
            };
            if (_formasDePagoRepositorio.ExisteFormaDePago(formaDePago)) throw new InvalidCastException($"Ya existe una Forma de Pago {formaDePago.Nombre}");
            try
            {
                _formasDePagoRepositorio.Agregar(formaDePago);
                return formaDePago.FormaDePagoId;
            }
            catch (Exception ex)
            {

                throw new Exception($"Error al intentar guardar una forma de pago {ex.Message}");
            }
        }
        public void Borrar(int formaDePagoId)
        {
            // AJUSTE: Validación defensiva del ID antes de operar
            if (formaDePagoId <= 0)
                throw new ArgumentException("El ID de la forma de pago debe ser un entero mayor a cero.", nameof(formaDePagoId));
            // 2. NUEVO AJUSTE: Verificar existencia real en el sistema antes que cualquier otra cosa
            // Nota: Asumiendo que tu repositorio tiene un método Existe(id) o similar
            var formaDePagoInDb = _formasDePagoRepositorio.ObtenerPorId(formaDePagoId);
            if (formaDePagoInDb is null)
            {
                throw new KeyNotFoundException($"No se puede borrar. No existe ninguna forma de pago con el ID {formaDePagoId}.");
            }

            // AJUSTE: Se cambia Exception genérica por InvalidOperationException
            if (_formasDePagoRepositorio.TieneRegistrosRelacionados(formaDePagoId))
            {
                throw new InvalidOperationException($"No se puede eliminar la forma de pago (ID: {formaDePagoId}) porque tiene registros relacionados en el sistema.");
            }
            _formasDePagoRepositorio.Borrar(formaDePagoId);
        }

        public void Editar(FormaDePagoEditDto? formaDePagoDto)
        {
            if (formaDePagoDto == null)
                throw new ArgumentNullException(nameof(formaDePagoDto), "La forma de pago no puede ser nula");
            if (string.IsNullOrWhiteSpace(formaDePagoDto.Nombre))
                throw new ArgumentException(nameof(formaDePagoDto.Nombre), "El nombre de la forma de pago es requerido");
            FormaDePago formaDePago = new FormaDePago
            {
                FormaDePagoId = formaDePagoDto.FormaDePagoId,
                Nombre = formaDePagoDto.Nombre,
                Activo = formaDePagoDto.Activo
            };
            if (_formasDePagoRepositorio.ExisteFormaDePago(formaDePago)) throw new InvalidOperationException($"Ya existe una Forma de Pago {formaDePago.Nombre}");
            _formasDePagoRepositorio.Editar(formaDePago);
        }
        public FormaDePagoEditDto ObtenerParaEditar(int id)
        {
            // AJUSTE: Validación defensiva del ID antes de operar
            if (id <= 0)
                throw new ArgumentException("El ID de la forma de pago debe ser un entero mayor a cero.", nameof(id));


            FormaDePago? formaDePago = _formasDePagoRepositorio.ObtenerPorId(id);
            if (formaDePago is null) throw new ArgumentException(nameof(id), $"Id {id} no encontrado");
            FormaDePagoEditDto formaDePagoDto = new FormaDePagoEditDto
            {
                FormaDePagoId = formaDePago.FormaDePagoId,
                Nombre = formaDePago.Nombre,
                Activo = formaDePago.Activo
            };
            return formaDePagoDto;
        }
        public List<FormaDePagoListDto> FiltrarPorActivo(bool activo)
        {
            return _formasDePagoRepositorio.FiltrarPorActivo(activo)
                .Select(f => new FormaDePagoListDto
                {
                    FormaDePagoId = f.FormaDePagoId,
                    Nombre = f.Nombre,
                    Activo = f.Activo
                }).ToList();
        }

        public int ObtenerPaginaRegistro(string nombre, int cantidadPorPagina,
            bool? filtroActivo = null, string? textoBuscar = null)
        {
            int posicion = _formasDePagoRepositorio
                .ObtenerPosicionAlfabetica(nombre, filtroActivo, textoBuscar);
            return (int)Math.Ceiling((double)posicion / cantidadPorPagina);
        }
    }
}
