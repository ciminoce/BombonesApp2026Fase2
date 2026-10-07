using Bombones2026.Servicios.DTOs.Ciudad;
using Bombones2026.Servicios.DTOs.Paginacion;
using BombonesApp2026.Datos;
using BombonesApp2026.Datos.Interfaces;
using BombonesApp2026.Entidades.Entidades;
using BombonesApp2026.Entidades.Enum;
using BombonesApp2026.Servicios.Interfaces;
using BombonesApp2026.Servicios.Mapeadores;

namespace Bombones2026.Servicios.Servicios
{
    public class CiudadServicio : ICiudadServicio
    {
        private readonly ICiudadRepositorio _ciudadRepositorio;
        private readonly IUnitOfWork _unitOfWork;
        public CiudadServicio(ICiudadRepositorio ciudadRepositorio, IUnitOfWork unitOfWork)
        {
            _ciudadRepositorio = ciudadRepositorio ?? throw new ArgumentNullException(nameof(ciudadRepositorio));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public ResultadoPaginacionDto<CiudadListDto> ObtenerPagina(int paginaActual,
        int cantidadPorPagina, bool? filtroActivo = null, string? textoBuscar = null)
        {
            try
            {
                var resultado = _ciudadRepositorio.ObtenerPagina(paginaActual,
                    cantidadPorPagina, filtroActivo, textoBuscar);
                var listaDto = resultado.lista
                        .Select(c => c.ToListDto()).ToList();
                return new ResultadoPaginacionDto<CiudadListDto>
                {
                    Items = listaDto,
                    TotalRegistros = resultado.cantidadRegistros,
                    CantidadPorPagina = cantidadPorPagina,
                    PaginaActual = paginaActual
                };
            }
            catch (Exception ex)
            {

                throw new Exception("Error al obtener la página de ciudades.", ex);
            }
        }

        public List<CiudadListDto> ObtenerTodos()
        {
            try
            {
                return _ciudadRepositorio.ObtenerTodos()
            .Select(c => c.ToListDto()).ToList();

            }
            catch (Exception ex)
            {

                throw new Exception("Error al obtener el listado completo de ciudades.", ex);
            }
        }
        public int Agregar(CiudadCreateDto ciudadDto)
        {
            Ciudad ciudad = ciudadDto.ToEntidad();
            if (_ciudadRepositorio.ExisteCiudad(ciudad))
            {
                throw new ArgumentException(nameof(ciudad), $"Ya existe una ciudad con el nombre {ciudad.Nombre}\n en esa provincia");
            }
            try
            {
                _ciudadRepositorio.Agregar(ciudad);
                _unitOfWork.Guardar();
                return ciudad.CiudadId;
            }
            catch (Exception ex)
            {

                throw new Exception($"Error al intentar guardar una ciudad {ex.Message}");
            }
        }

        public void Borrar(int ciudadId)
        {
            if (ciudadId <= 0)
            {
                throw new ArgumentException(nameof(ciudadId),
                    "El ID debe ser positivo");
            }
            var ciudad = _ciudadRepositorio.ObtenerPorId(ciudadId);
            if (ciudad is null)
            {
                throw new KeyNotFoundException($"No se encontró una ciudad con el ID {ciudadId}");
            }
            if (ciudad.EstaRelacionada(ciudad.CiudadId))
            {
                throw new InvalidOperationException($"No se puede borrar la ciudad {ciudad.Nombre} porque tiene clientes asociados.");
            }
            try
            {
                _ciudadRepositorio.Borrar(ciudadId);
                _unitOfWork.Guardar();
            }
            catch (Exception ex)
            {

                throw new Exception($"Error al intentar borrar una ciudad: {ex.Message}");
            }
        }
        public CiudadEditDto? ObtenerParaEditar(int ciudadId)
        {
            // AJUSTE: Validación defensiva del ID antes de operar
            if (ciudadId <= 0)
                throw new ArgumentException("El ID ciudad debe ser entero mayor a cero.", nameof(ciudadId));


            Ciudad? ciudad = _ciudadRepositorio.ObtenerPorId(ciudadId);
            if (ciudad is null) throw new ArgumentException(nameof(ciudadId), $"Id {ciudadId} no encontrado");
            try
            {
                CiudadEditDto ciudadDto = ciudad.ToEditDto();
                return ciudadDto;

            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener la ciudad con ID {ciudadId} para edición: {ex.Message}", ex);
            }
        }

        public void Editar(CiudadEditDto ciudadDto)
        {
            if (ciudadDto == null)
                throw new ArgumentNullException(nameof(ciudadDto), "La forma de pago no puede ser nula");
            if (string.IsNullOrWhiteSpace(ciudadDto.Nombre))
                throw new ArgumentException(nameof(ciudadDto.Nombre), "El nombre de la forma de pago es requerido");
            if (ciudadDto.ProvinciaId == 0)
            {
                throw new ArgumentException(nameof(ciudadDto.ProvinciaId), "El ID de la provincia debe ser mayor a 0");
            }
            Ciudad ciudad = ciudadDto.ToEntidad();
            if (_ciudadRepositorio.ExisteCiudad(ciudad)) throw new InvalidOperationException($"Ya existe una ciudad {ciudad.Nombre}");
            try
            {
                _ciudadRepositorio.Editar(ciudad);
                _unitOfWork.Guardar();

            }
            catch (Exception ex)
            {
                throw new Exception($"Error al intentar actualizar la ciudad con ID {ciudad.CiudadId}.", ex);
            }
        }

        public int ObtenerPaginaRegistro(string nombre, int cantidadPorPagina,
            bool? filtroActivo = null, string? textoBuscar = null)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre no puede estar vacío.", nameof(nombre));

            if (cantidadPorPagina <= 0) cantidadPorPagina = 10;

            int posicion = _ciudadRepositorio
                .ObtenerPosicionAlfabetica(nombre, filtroActivo, textoBuscar);

            if (posicion <= 0) return 1;

            try
            {
                return (int)Math.Ceiling((double)posicion / cantidadPorPagina);

            }
            catch (Exception ex)
            {

                throw new Exception($"Error al obtener la página de registro para la ciudad '{nombre}': {ex.Message}", ex);
            }
        }

        public List<CiudadListDto> ObtenerDatosCombo(TipoCiudadDefault tipoDefault, int provinciaId)
        {
            try
            {
                var lista = _ciudadRepositorio.ObtenerTodos(provinciaId)
                        .Select(c => c.ToListDto()).ToList();
                if (tipoDefault == TipoCiudadDefault.Todas)
                {
                    var defaultCiudad = new CiudadListDto
                    {
                        CiudadId = 0,
                        Ciudad = "Todas"
                    };
                    lista.Insert(0, defaultCiudad);

                }
                else
                {
                    var defaultCiudad = new CiudadListDto
                    {
                        CiudadId = 0,
                        Ciudad = "Seleccione"
                    };
                    lista.Insert(0, defaultCiudad);
                }
                return lista;

            }
            catch (Exception ex)
            {
                throw new Exception($"Error al poblar los datos del combo de ciudades: {ex.Message}", ex);
            }
        }
    }
}
