using Bombones2026.Servicios.DTOs.Paginacion;
using Bombones2026.Servicios.DTOs.Provincia;
using BombonesApp2026.Datos;
using BombonesApp2026.Datos.Interfaces;
using BombonesApp2026.Entidades.Entidades;
using BombonesApp2026.Entidades.Enum;
using BombonesApp2026.Servicios.Interfaces;
using BombonesApp2026.Servicios.Mapeadores;

namespace Bombones2026.Servicios.Servicios
{
    public class ProvinciaServicio : IProvinciaServicio
    {
        private readonly IProvinciaRepositorio _provinciaRepositorio;
        private readonly IUnitOfWork _unitOfWork;
        public ProvinciaServicio(IProvinciaRepositorio provinciaRepositorio, IUnitOfWork unitOfWork)
        {
            _provinciaRepositorio = provinciaRepositorio;
            _unitOfWork = unitOfWork;
        }

        public ResultadoPaginacionDto<ProvinciaListDto> ObtenerPagina(int paginaActual,
        int cantidadPorPagina, bool? filtroActivo = null, string? textoBuscar = null)
        {
            try
            {
                var resultado = _provinciaRepositorio.ObtenerPagina(paginaActual,
                    cantidadPorPagina, filtroActivo, textoBuscar);
                var listaDto = resultado.lista
                .Select(p => p.ToListDto()).ToList();
                return new ResultadoPaginacionDto<ProvinciaListDto>
                {
                    Items = listaDto,
                    TotalRegistros = resultado.cantidadRegistros,
                    CantidadPorPagina = cantidadPorPagina,
                    PaginaActual = paginaActual
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al intentar obtener la página de provincias: {ex.Message}", ex);
            }
        }

        public List<ProvinciaListDto> ObtenerTodos()
        {
            try
            {
                return _provinciaRepositorio.ObtenerTodos()
            .Select(p => p.ToListDto()).ToList();

            }
            catch (Exception ex)
            {

                throw new Exception($"Error al obtener la lista completa de provincias: {ex.Message}", ex);
            }
        }
        public int Agregar(ProvinciaCreateDto? provinciaDto)
        {
            if (provinciaDto == null)
                throw new ArgumentNullException(nameof(provinciaDto), "La provincia no puede ser nula");
            if (string.IsNullOrWhiteSpace(provinciaDto.Nombre))
                throw new ArgumentException(nameof(provinciaDto.Nombre), "El nombre de la provincia es requerido");
            Provincia provincia = new Provincia
            {
                NombreProvincia = provinciaDto.Nombre,
            };
            if (_provinciaRepositorio.ExisteProvincia(provincia)) throw new InvalidCastException($"Ya existe una Provincia {provincia.NombreProvincia}");
            try
            {
                _provinciaRepositorio.Agregar(provincia);
                _unitOfWork.Commit();
                return provincia.ProvinciaId;
            }
            catch (Exception ex)
            {

                throw new Exception($"Error al intentar guardar una provincia {ex.Message}");
            }
        }
        public void Borrar(int provinciaId)
        {
            // AJUSTE: Validación defensiva del ID antes de operar
            if (provinciaId <= 0)
                throw new ArgumentException("El ID de la provincia debe ser un entero mayor a cero.", nameof(provinciaId));
            // 2. NUEVO AJUSTE: Verificar existencia real en el sistema antes que cualquier otra cosa
            // Nota: Asumiendo que tu repositorio tiene un método Existe(id) o similar
            var provinciaInDb = _provinciaRepositorio.ObtenerPorId(provinciaId);
            if (provinciaInDb is null)
            {
                throw new KeyNotFoundException($"No se puede borrar. No existe ninguna provincia con el ID {provinciaId}.");
            }

            // AJUSTE: Se cambia Exception genérica por InvalidOperationException
            if (_provinciaRepositorio.TieneRegistrosRelacionados(provinciaId))
            {
                throw new InvalidOperationException($"No se puede eliminar la provincia (ID: {provinciaId}) porque tiene registros relacionados en el sistema.");
            }
            try
            {
                _provinciaRepositorio.Borrar(provinciaId);
                _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al intentar borrar la provincia con ID {provinciaId}: {ex.Message}", ex);
            }
        }

        public void Editar(ProvinciaEditDto? provinciaDto)
        {
            if (provinciaDto == null)
                throw new ArgumentNullException(nameof(provinciaDto), "La provincia no puede ser nula");
            if (string.IsNullOrWhiteSpace(provinciaDto.Nombre))
                throw new ArgumentException(nameof(provinciaDto.Nombre), "El nombre de la provincia es requerido");
            Provincia provincia = new Provincia
            {
                ProvinciaId = provinciaDto.ProvinciaId,
                NombreProvincia = provinciaDto.Nombre
            };
            if (_provinciaRepositorio.ExisteProvincia(provincia)) throw new InvalidOperationException($"Ya existe una Provincia {provincia.NombreProvincia}");
            try
            {
                _provinciaRepositorio.Editar(provincia);
                _unitOfWork.Commit();

            }
            catch (Exception ex)
            {

                throw new Exception($"Error al intentar editar la provincia con ID {provinciaDto.ProvinciaId}: {ex.Message}", ex);
            }
        }
        public ProvinciaEditDto ObtenerParaEditar(int id)
        {
            // AJUSTE: Validación defensiva del ID antes de operar
            if (id <= 0)
                throw new ArgumentException("El ID de la provincia debe ser un entero mayor a cero.", nameof(id));


            Provincia? provincia = _provinciaRepositorio.ObtenerPorId(id);
            if (provincia is null) throw new ArgumentException(nameof(id), $"Id {id} no encontrado");
            try
            {
                ProvinciaEditDto provinciaDto = provincia.ToEditDto();
                return provinciaDto;

            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al intentar obtener la provincia con ID {id} para edición: {ex.Message}", ex);
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
                int posicion = _provinciaRepositorio.ObtenerPosicionAlfabetica(nombre, filtroActivo, textoBuscar);
                return (int)Math.Ceiling((double)posicion / cantidadPorPagina);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener la página de la provincia '{nombre}': {ex.Message}", ex);
            }
        }

        public List<ProvinciaListDto> ObtenerDatosCombo(TipoProvinciaDefault tipoDefault)
        {
            try
            {
                var lista = _provinciaRepositorio.ObtenerTodos()
            .Select(p => p.ToListDto())
            .ToList();
                if (tipoDefault == TipoProvinciaDefault.Todas)
                {
                    var defaultProvincia = new ProvinciaListDto
                    {
                        ProvinciaId = 0,
                        Nombre = "Todas"
                    };
                    lista.Insert(0, defaultProvincia);

                }
                else
                {
                    var defaultProvincia = new ProvinciaListDto
                    {
                        ProvinciaId = 0,
                        Nombre = "Seleccione"
                    };
                    lista.Insert(0, defaultProvincia);
                }
                return lista;

            }
            catch (Exception ex)
            {
                throw new Exception($"Error al poblar los datos del combo de provincias: {ex.Message}", ex);
            }
        }
    }
}
