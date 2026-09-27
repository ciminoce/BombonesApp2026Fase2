using Bombones2026.Servicios.DTOs.Paginacion;
using Bombones2026.Servicios.DTOs.Transporte;
using BombonesApp2026.Datos.Interfaces;
using BombonesApp2026.Entidades.Entidades;
using BombonesApp2026.Servicios.Interfaces;

namespace Bombones2026.Servicios.Servicios
{
    public class TransporteServicio : ITransporteServicio
    {
        private readonly ITransporteRepositorio _transporteRepositorio;
        public TransporteServicio(ITransporteRepositorio transporteRepositorio)
        {
            _transporteRepositorio = transporteRepositorio;
        }
        public ResultadoPaginacionDto<TransporteListDto> ObtenerPagina(int paginaActual,
                int cantidadPorPagina, bool? filtroActivo = null,
                int? provinciaIdFiltro = null,
                string? textoBuscar = null)
        {
            try
            {
                var resultado = _transporteRepositorio.ObtenerPagina(paginaActual,
                    cantidadPorPagina, filtroActivo, provinciaIdFiltro, textoBuscar);
                var listaDto = resultado.lista
                    .Select(tb => new TransporteListDto
                    {
                        TransporteId = tb.TransporteId,
                        NombreEmpresa = tb.NombreEmpresa,
                        Telefono = tb.Telefono,
                        Email = tb.Email,
                        Provincia = tb.Provincia!.NombreProvincia,
                        Activo = tb.Activo
                    }).ToList();
                return new ResultadoPaginacionDto<TransporteListDto>
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

        public int Agregar(TransporteCreateDto? transporteDto)
        {
            if (transporteDto is null)
            {
                throw new ArgumentNullException(nameof(transporteDto), "El transporte no puede ser nulo");
            }
            if (string.IsNullOrWhiteSpace(transporteDto.NombreEmpresa))
            {
                throw new ArgumentNullException(nameof(transporteDto.NombreEmpresa), "El nombre es requerido");
            }
            if (string.IsNullOrWhiteSpace(transporteDto.Telefono))
            {
                throw new ArgumentNullException(nameof(transporteDto.Telefono), "El teléfono es requerido");
            }
            if (string.IsNullOrWhiteSpace(transporteDto.Email))
            {
                throw new ArgumentNullException(nameof(transporteDto.Email), "El Email es requerido");
            }

            Transporte transporte = new Transporte()
            {
                NombreEmpresa = transporteDto.NombreEmpresa,
                Telefono = transporteDto.Telefono,
                Email = transporteDto.Email,
                ProvinciaId = transporteDto.ProvinciaId,
                Activo = true
            };
            if (_transporteRepositorio.ExisteTransporte(transporte))
            {
                throw new InvalidOperationException($"Ya existe un transporte {transporte.NombreEmpresa}");
            }
            try
            {
                _transporteRepositorio.Agregar(transporte);
                return transporte.TransporteId;
            }
            catch (Exception ex)
            {

                throw new Exception($"Error al intentar agregar un transporte: {ex.Message}");
            }
        }

        public void Borrar(int transporteId)
        {
            if (transporteId <= 0)
                throw new ArgumentException("El ID del tipo de bombón debe ser un entero mayor a cero.", nameof(transporteId));
            var transporteInDb = _transporteRepositorio.ObtenerPorId(transporteId);
            if (transporteInDb is null)
            {
                throw new KeyNotFoundException($"No se puede borrar. No existe ningún transporte con el ID {transporteId}.");
            }

            // AJUSTE: Se cambia Exception genérica por InvalidOperationException
            if (_transporteRepositorio.TieneRegistrosRelacionados(transporteId))
            {
                throw new InvalidOperationException($"No se puede eliminar el tipo de bombón (ID: {transporteId}) porque tiene registros relacionados en el sistema.");
            }
            try
            {
                _transporteRepositorio.Borrar(transporteId);

            }
            catch (Exception ex)
            {

                throw new Exception($"Error al intentar borrar un transporte: {ex.Message}");
            }
        }

        public void Editar(TransporteEditDto? transporteDto)
        {
            if (transporteDto is null)
            {
                throw new ArgumentNullException(nameof(transporteDto), "El transporte no puede ser nulo");
            }
            if (string.IsNullOrWhiteSpace(transporteDto.NombreEmpresa))
            {
                throw new ArgumentNullException(nameof(transporteDto.NombreEmpresa), "El nombre es requerido");
            }
            if (string.IsNullOrWhiteSpace(transporteDto.Telefono))
            {
                throw new ArgumentNullException(nameof(transporteDto.Telefono), "El teléfono es requerido");
            }
            if (string.IsNullOrWhiteSpace(transporteDto.Email))
            {
                throw new ArgumentNullException(nameof(transporteDto.Email), "El Email es requerido");
            }
            Transporte transporte = new Transporte()
            {
                TransporteId = transporteDto.TransporteId,
                NombreEmpresa = transporteDto.NombreEmpresa,
                Telefono = transporteDto.Telefono,
                Email = transporteDto.Email,
                ProvinciaId = transporteDto.ProvinciaId,
                Activo = transporteDto.Activo
            };
            if (_transporteRepositorio.ExisteTransporte(transporte))
            {
                throw new InvalidOperationException($"Ya existe un transporte {transporte.NombreEmpresa}");
            }
            try
            {
                _transporteRepositorio.Editar(transporte);
            }
            catch (Exception ex)
            {

                throw new Exception($"Error al intentar editar un transporte: {ex.Message}");
            }
        }

        public TransporteEditDto? ObtenerParaEditar(int transporteId)
        {
            try
            {
                // AJUSTE: Validación defensiva del ID antes de operar
                if (transporteId <= 0)
                    throw new ArgumentException("El ID del transporte debe ser un entero mayor a cero.", nameof(transporteId));


                Transporte? transporte = _transporteRepositorio.ObtenerPorId(transporteId);
                if (transporte is null) throw new ArgumentException(nameof(transporteId), $"Id {transporteId} no encontrado");
                TransporteEditDto transporteDto = new TransporteEditDto
                {
                    TransporteId = transporte.TransporteId,
                    NombreEmpresa = transporte.NombreEmpresa,
                    Telefono = transporte.Telefono,
                    Email = transporte.Email,
                    ProvinciaId = transporte.ProvinciaId,
                    Activo = transporte.Activo
                };
                return transporteDto;

            }
            catch (Exception ex)
            {

                throw new Exception($"Error al intentar obtener un transporte: {ex.Message}");
            }
        }

        public List<TransporteListDto> ObtenerTodos()
        {
            return _transporteRepositorio.ObtenerTodos()
                .Select(t => new TransporteListDto
                {
                    TransporteId = t.TransporteId,
                    NombreEmpresa = t.NombreEmpresa,
                    Provincia = t.Provincia is not null ? t.Provincia.NombreProvincia : "Sin Provincia",
                    Telefono = t.Telefono,
                    Email = t.Email,
                    Activo = t.Activo

                }).ToList();
        }

        public int ObtenerPaginaRegistro(string nombre, int cantidadPorPagina,
            bool? filtroActivo = null, int? provinciaIdFiltro = null,
            string? textoBuscar = null)
        {
            int posicion = _transporteRepositorio
                .ObtenerPosicionAlfabetica(nombre, filtroActivo,
                provinciaIdFiltro,
                textoBuscar);
            return (int)Math.Ceiling((double)posicion / cantidadPorPagina);
        }
    }
}
