using Bombones2026.Servicios.DTOs.Paginacion;
using Bombones2026.Servicios.DTOs.TipoBombon;
using BombonesApp2026.Datos;
using BombonesApp2026.Datos.Interfaces;
using BombonesApp2026.Entidades.Entidades;
using BombonesApp2026.Entidades.Enum;
using BombonesApp2026.Servicios.Interfaces;
using BombonesApp2026.Servicios.Mapeadores;

namespace Bombones2026.Servicios.Servicios
{
    public class TipoBombonServicio : ITipoBombonServicio
    {
        private readonly ITipoBombonRepositorio _tipoBombonRepositorio;
        private readonly IUnitOfWork _unitOfWork;

        public TipoBombonServicio(ITipoBombonRepositorio tipoBombonRepositorio, IUnitOfWork unitOfWork)
        {
            _tipoBombonRepositorio = tipoBombonRepositorio ?? throw new ArgumentNullException(nameof(tipoBombonRepositorio));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public ResultadoPaginacionDto<TipoBombonListDto> ObtenerPagina(int paginaActual,
            int cantidadPorPagina, bool? filtroActivo = null, string? textoBuscar = null)
        {
            try
            {
                var resultado = _tipoBombonRepositorio.ObtenerPagina(paginaActual,
                    cantidadPorPagina, filtroActivo, textoBuscar);

                var listaDto = resultado.lista
                    .Select(tb => tb.ToListDto())
                    .ToList();

                return new ResultadoPaginacionDto<TipoBombonListDto>
                {
                    Items = listaDto,
                    TotalRegistros = resultado.cantidadRegistros,
                    CantidadPorPagina = cantidadPorPagina,
                    PaginaActual = paginaActual
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al intentar obtener la página de tipos de bombón: {ex.Message}", ex);
            }
        }

        public List<TipoBombonListDto> ObtenerTodos()
        {
            try
            {
                return _tipoBombonRepositorio.ObtenerTodos()
                    .Select(tb => tb.ToListDto())
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener la lista de tipos de bombón: {ex.Message}", ex);
            }
        }

        public int Agregar(TipoBombonCreateDto? tipoDto)
        {
            if (tipoDto is null)
            {
                throw new ArgumentNullException(nameof(tipoDto), "El tipo de bombón no puede ser nulo");
            }
            if (string.IsNullOrWhiteSpace(tipoDto.Nombre))
            {
                throw new ArgumentException("El nombre del tipo de bombón es requerido", nameof(tipoDto.Nombre));
            }

            TipoBombon tipo = tipoDto.ToEntidad();

            if (_tipoBombonRepositorio.ExisteTipoBombon(tipo))
            {
                throw new InvalidOperationException($"Ya existe un tipo de bombón con el nombre '{tipo.Nombre}'");
            }

            try
            {
                _tipoBombonRepositorio.Agregar(tipo);
                _unitOfWork.Commit();
                return tipo.TipoBombonId;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al intentar agregar el tipo de bombón '{tipoDto.Nombre}': {ex.Message}", ex);
            }
        }

        public void Borrar(int tipoBombonId)
        {
            if (tipoBombonId <= 0)
            {
                throw new ArgumentException("El ID del tipo de bombón debe ser un entero mayor a cero.", nameof(tipoBombonId));
            }

            var tipoBombonInDb = _tipoBombonRepositorio.ObtenerPorId(tipoBombonId);
            if (tipoBombonInDb is null)
            {
                throw new KeyNotFoundException($"No se puede borrar. No existe ningún tipo de bombón con el ID {tipoBombonId}.");
            }

            if (_tipoBombonRepositorio.TieneRegistrosRelacionados(tipoBombonId))
            {
                throw new InvalidOperationException($"No se puede eliminar el tipo de bombón (ID: {tipoBombonId}) porque tiene registros relacionados en el sistema.");
            }

            try
            {
                _tipoBombonRepositorio.Borrar(tipoBombonId);
                _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al intentar borrar el tipo de bombón con ID {tipoBombonId}: {ex.Message}", ex);
            }
        }

        public void Editar(TipoBombonEditDto? tipoDto)
        {
            if (tipoDto == null)
            {
                throw new ArgumentNullException(nameof(tipoDto), "El tipo de bombón no puede ser nulo");
            }
            if (tipoDto.TipoBombonId <= 0)
            {
                throw new ArgumentException("El ID del tipo de bombón no es válido.", nameof(tipoDto.TipoBombonId));
            }
            if (string.IsNullOrWhiteSpace(tipoDto.Nombre))
            {
                throw new ArgumentException("El nombre del tipo de bombón es requerido", nameof(tipoDto.Nombre));
            }

            var tipoInDb = _tipoBombonRepositorio.ObtenerPorId(tipoDto.TipoBombonId);
            if (tipoInDb is null)
            {
                throw new KeyNotFoundException($"No existe ningún tipo de bombón con el ID {tipoDto.TipoBombonId}.");
            }

            TipoBombon tipo = tipoDto.ToEntidad();

            if (_tipoBombonRepositorio.ExisteTipoBombon(tipo))
            {
                throw new InvalidOperationException($"Ya existe otro tipo de bombón con el nombre '{tipo.Nombre}'");
            }

            try
            {
                _tipoBombonRepositorio.Editar(tipo, tipo.TipoBombonId);
                _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al intentar editar el tipo de bombón con ID {tipoDto.TipoBombonId}: {ex.Message}", ex);
            }
        }

        public TipoBombonEditDto ObtenerParaEditar(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("El ID del tipo de bombón debe ser un entero mayor a cero.", nameof(id));
            }

            try
            {
                TipoBombon? tipo = _tipoBombonRepositorio.ObtenerPorId(id);
                if (tipo is null)
                {
                    throw new KeyNotFoundException($"No se encontró el tipo de bombón con ID {id}");
                }

                return tipo.ToEditDto();
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener el tipo de bombón con ID {id} para edición: {ex.Message}", ex);
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
                int posicion = _tipoBombonRepositorio.ObtenerPosicionAlfabetica(nombre, filtroActivo, textoBuscar);
                return (int)Math.Ceiling((double)posicion / cantidadPorPagina);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener la posición de la página para '{nombre}': {ex.Message}", ex);
            }
        }

        public List<TipoBombonListDto> ObtenerDatosCombo(TipoBombonDefault tipoDefault)
        {
            try
            {
                var lista = _tipoBombonRepositorio.ObtenerTodos()
                    .Select(tp => tp.ToListDto())
                    .ToList();

                var defaultTipo = new TipoBombonListDto
                {
                    TipoBombonId = 0,
                    Nombre = tipoDefault == TipoBombonDefault.Todos ? "Todos" : "Seleccione"
                };

                lista.Insert(0, defaultTipo);
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al poblar los datos del combo de tipos de bombón: {ex.Message}", ex);
            }
        }
    }
}
