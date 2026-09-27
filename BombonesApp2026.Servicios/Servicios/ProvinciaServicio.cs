using Bombones2026.Servicios.DTOs.Paginacion;
using Bombones2026.Servicios.DTOs.Provincia;
using BombonesApp2026.Datos.Interfaces;
using BombonesApp2026.Entidades.Entidades;
using BombonesApp2026.Entidades.Enum;
using BombonesApp2026.Servicios.Interfaces;

namespace Bombones2026.Servicios.Servicios
{
    public class ProvinciaServicio : IProvinciaServicio
    {
        private readonly IProvinciaRepositorio _provinciaRepositorio;
        public ProvinciaServicio(IProvinciaRepositorio provinciaRepositorio)
        {
            _provinciaRepositorio = provinciaRepositorio;
        }

        public ResultadoPaginacionDto<ProvinciaListDto> ObtenerPagina(int paginaActual,
        int cantidadPorPagina, bool? filtroActivo = null, string? textoBuscar = null)
        {
            try
            {
                var resultado = _provinciaRepositorio.ObtenerPagina(paginaActual,
                    cantidadPorPagina, filtroActivo, textoBuscar);
                var listaDto = resultado.lista
                .Select(p => new ProvinciaListDto
                {
                    ProvinciaId = p.ProvinciaId,
                    Nombre = p.NombreProvincia,
                }).ToList();
                return new ResultadoPaginacionDto<ProvinciaListDto>
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

        public List<ProvinciaListDto> ObtenerTodos()
        {
            return _provinciaRepositorio.ObtenerTodos()
                .Select(p => new ProvinciaListDto
                {
                    ProvinciaId = p.ProvinciaId,
                    Nombre = p.NombreProvincia,
                }).ToList();
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
            _provinciaRepositorio.Borrar(provinciaId);
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
            _provinciaRepositorio.Editar(provincia);
        }
        public ProvinciaEditDto ObtenerParaEditar(int id)
        {
            // AJUSTE: Validación defensiva del ID antes de operar
            if (id <= 0)
                throw new ArgumentException("El ID de la provincia debe ser un entero mayor a cero.", nameof(id));


            Provincia? provincia = _provinciaRepositorio.ObtenerPorId(id);
            if (provincia is null) throw new ArgumentException(nameof(id), $"Id {id} no encontrado");
            ProvinciaEditDto provinciaDto = new ProvinciaEditDto
            {
                ProvinciaId = provincia.ProvinciaId,
                Nombre = provincia.NombreProvincia
            };
            return provinciaDto;
        }

        public int ObtenerPaginaRegistro(string nombre, int cantidadPorPagina,
            bool? filtroActivo = null, string? textoBuscar = null)
        {
            int posicion = _provinciaRepositorio
                .ObtenerPosicionAlfabetica(nombre, filtroActivo, textoBuscar);
            return (int)Math.Ceiling((double)posicion / cantidadPorPagina);
        }

        public List<ProvinciaListDto> ObtenerDatosCombo(TipoProvinciaDefault tipoDefault)
        {
            var lista = _provinciaRepositorio.ObtenerTodos()
                .Select(p => new ProvinciaListDto
                {
                    ProvinciaId = p.ProvinciaId,
                    Nombre = p.NombreProvincia
                }).ToList();
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
    }

}
