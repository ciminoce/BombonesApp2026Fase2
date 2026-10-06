using Bombones2026.Servicios.DTOs.Paginacion;
using Bombones2026.Servicios.DTOs.Transporte;
using BombonesApp2026.Datos;
using BombonesApp2026.Datos.Interfaces;
using BombonesApp2026.Entidades.Entidades;
using BombonesApp2026.Servicios.Interfaces;
using BombonesApp2026.Servicios.Mapeadores;

namespace Bombones2026.Servicios.Servicios
{
    public class TransporteServicio : ITransporteServicio
    {
        private readonly ITransporteRepositorio _transporteRepositorio;
        private readonly IUnitOfWork _unitOfWork;

        public TransporteServicio(ITransporteRepositorio transporteRepositorio, IUnitOfWork unitOfWork)
        {
            _transporteRepositorio = transporteRepositorio ?? throw new ArgumentNullException(nameof(transporteRepositorio));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public ResultadoPaginacionDto<TransporteListDto> ObtenerPagina(int paginaActual,
            int cantidadPorPagina, bool? filtroActivo = null,
            int? provinciaIdFiltro = null, string? textoBuscar = null)
        {
            try
            {
                var resultado = _transporteRepositorio.ObtenerPagina(paginaActual,
                    cantidadPorPagina, filtroActivo, provinciaIdFiltro, textoBuscar);

                var listaDto = resultado.lista
                    .Select(t => t.ToListDto())
                    .ToList();

                return new ResultadoPaginacionDto<TransporteListDto>
                {
                    Items = listaDto,
                    TotalRegistros = resultado.cantidadRegistros,
                    CantidadPorPagina = cantidadPorPagina,
                    PaginaActual = paginaActual
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al intentar obtener la página de transportes: {ex.Message}", ex);
            }
        }

        public List<TransporteListDto> ObtenerTodos()
        {
            try
            {
                return _transporteRepositorio.ObtenerTodos()
                    .Select(t => t.ToListDto())
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener la lista completa de transportes: {ex.Message}", ex);
            }
        }

        public int Agregar(TransporteCreateDto? transporteDto)
        {
            if (transporteDto is null)
            {
                throw new ArgumentNullException(nameof(transporteDto), "El transporte no puede ser nulo");
            }
            if (string.IsNullOrWhiteSpace(transporteDto.NombreEmpresa))
            {
                throw new ArgumentException("El nombre de la empresa es requerido", nameof(transporteDto.NombreEmpresa));
            }
            if (string.IsNullOrWhiteSpace(transporteDto.Telefono))
            {
                throw new ArgumentException("El teléfono es requerido", nameof(transporteDto.Telefono));
            }
            if (string.IsNullOrWhiteSpace(transporteDto.Email))
            {
                throw new ArgumentException("El Email es requerido", nameof(transporteDto.Email));
            }

            Transporte transporte = transporteDto.ToEntidad();

            if (_transporteRepositorio.ExisteTransporte(transporte))
            {
                throw new InvalidOperationException($"Ya existe un transporte registrado como '{transporte.NombreEmpresa}'");
            }

            try
            {
                _transporteRepositorio.Agregar(transporte);
                _unitOfWork.Commit();
                return transporte.TransporteId;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al intentar agregar el transporte '{transporteDto.NombreEmpresa}': {ex.Message}", ex);
            }
        }

        public void Borrar(int transporteId)
        {
            if (transporteId <= 0)
            {
                throw new ArgumentException("El ID del transporte debe ser un entero mayor a cero.", nameof(transporteId));
            }

            var transporteInDb = _transporteRepositorio.ObtenerPorId(transporteId);
            if (transporteInDb is null)
            {
                throw new KeyNotFoundException($"No se puede borrar. No existe ningún transporte con el ID {transporteId}.");
            }

            if (_transporteRepositorio.TieneRegistrosRelacionados(transporteId))
            {
                throw new InvalidOperationException($"No se puede eliminar el transporte (ID: {transporteId}) porque tiene registros relacionados en el sistema.");
            }

            try
            {
                _transporteRepositorio.Borrar(transporteId);
                _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al intentar borrar el transporte con ID {transporteId}: {ex.Message}", ex);
            }
        }

        public void Editar(TransporteEditDto? transporteDto)
        {
            if (transporteDto is null)
            {
                throw new ArgumentNullException(nameof(transporteDto), "El transporte no puede ser nulo");
            }
            if (transporteDto.TransporteId <= 0)
            {
                throw new ArgumentException("El ID del transporte no es válido.", nameof(transporteDto.TransporteId));
            }
            if (string.IsNullOrWhiteSpace(transporteDto.NombreEmpresa))
            {
                throw new ArgumentException("El nombre de la empresa es requerido", nameof(transporteDto.NombreEmpresa));
            }
            if (string.IsNullOrWhiteSpace(transporteDto.Telefono))
            {
                throw new ArgumentException("El teléfono es requerido", nameof(transporteDto.Telefono));
            }
            if (string.IsNullOrWhiteSpace(transporteDto.Email))
            {
                throw new ArgumentException("El Email es requerido", nameof(transporteDto.Email));
            }

            var transporteInDb = _transporteRepositorio.ObtenerPorId(transporteDto.TransporteId);
            if (transporteInDb is null)
            {
                throw new KeyNotFoundException($"No existe ningún transporte con el ID {transporteDto.TransporteId}.");
            }

            Transporte transporte = transporteDto.ToEntidad();

            if (_transporteRepositorio.ExisteTransporte(transporte))
            {
                throw new InvalidOperationException($"Ya existe otro transporte con el nombre '{transporte.NombreEmpresa}'");
            }

            try
            {
                _transporteRepositorio.Editar(transporte, transporte.TransporteId);
                _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al intentar editar el transporte con ID {transporteDto.TransporteId}: {ex.Message}", ex);
            }
        }

        public TransporteEditDto ObtenerParaEditar(int transporteId)
        {
            if (transporteId <= 0)
            {
                throw new ArgumentException("El ID del transporte debe ser un entero mayor a cero.", nameof(transporteId));
            }

            try
            {
                Transporte? transporte = _transporteRepositorio.ObtenerPorId(transporteId);
                if (transporte is null)
                {
                    throw new KeyNotFoundException($"No se encontró el transporte con ID {transporteId}");
                }

                return transporte.ToEditDto();
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al intentar obtener el transporte con ID {transporteId} para edición: {ex.Message}", ex);
            }
        }

        public int ObtenerPaginaRegistro(string nombre, int cantidadPorPagina,
            bool? filtroActivo = null, int? provinciaIdFiltro = null, string? textoBuscar = null)
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
                int posicion = _transporteRepositorio.ObtenerPosicionAlfabetica(nombre, filtroActivo, provinciaIdFiltro, textoBuscar);
                return (int)Math.Ceiling((double)posicion / cantidadPorPagina);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener la página del transporte '{nombre}': {ex.Message}", ex);
            }
        }
    }
}
