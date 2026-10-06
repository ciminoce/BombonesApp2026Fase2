using Bombones2026.Servicios.DTOs.Paginacion;
using BombonesApp2026.Datos;
using BombonesApp2026.Datos.Interfaces;
using BombonesApp2026.Entidades.Entidades;
using BombonesApp2026.Servicios.DTOs.Cliente;
using BombonesApp2026.Servicios.Interfaces;
using BombonesApp2026.Servicios.Mapeadores;

namespace BombonesApp2026.Servicios.Servicios
{
    public class ClienteServicio : IClienteServicio
    {
        private readonly IClienteRepositorio _clienteRepositorio;
        private readonly IUnitOfWork _unitOfWork;

        public ClienteServicio(IClienteRepositorio clienteRepositorio, IUnitOfWork unitOfWork)
        {
            _clienteRepositorio = clienteRepositorio ?? throw new ArgumentNullException(nameof(clienteRepositorio));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public ResultadoPaginacionDto<ClienteListDto> ObtenerPagina(int paginaActual,
            int cantidadPorPagina, bool? filtroActivo = null, string? textoBuscar = null)
        {
            if (paginaActual <= 0) paginaActual = 1;
            if (cantidadPorPagina <= 0) cantidadPorPagina = 10;

            try
            {
                var resultado = _clienteRepositorio.ObtenerPagina(paginaActual,
                    cantidadPorPagina, filtroActivo, textoBuscar);

                var listaDto = resultado.lista
                        .Select(b => b.ToListDto()).ToList();

                return new ResultadoPaginacionDto<ClienteListDto>
                {
                    Items = listaDto,
                    TotalRegistros = resultado.cantidadRegistros,
                    CantidadPorPagina = cantidadPorPagina,
                    PaginaActual = paginaActual
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"No se pudo obtener la página de clientes.", ex);
            }
        }

        public List<ClienteListDto> ObtenerTodos()
        {
            try
            {
                return _clienteRepositorio.ObtenerTodos()
                    .Select(b => b.ToListDto()).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el listado completo de clientes.", ex);
            }
        }

        public int Agregar(ClienteCreateDto clienteDto)
        {
            if (clienteDto is null)
                throw new ArgumentNullException(nameof(clienteDto), "El cliente no puede ser nulo.");

            Cliente cliente = clienteDto.ToEntidad();

            if (_clienteRepositorio.ExisteCliente(cliente))
            {
                throw new InvalidOperationException($"Ya existe un cliente con el documento {cliente.Documento}");
            }

            try
            {
                _clienteRepositorio.Agregar(cliente);
                _unitOfWork.Commit();
                return cliente.ClienteId;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al intentar guardar el cliente.", ex);
            }
        }

        public void Borrar(int clienteId)
        {
            if (clienteId <= 0)
            {
                throw new ArgumentException("El ID debe ser mayor a cero.", nameof(clienteId));
            }

            var cliente = _clienteRepositorio.ObtenerPorId(clienteId);
            if (cliente is null)
            {
                throw new KeyNotFoundException($"No se encontró un cliente con el ID {clienteId}");
            }

            // Verificación de registros relacionados antes de eliminar
            if (_clienteRepositorio.EstaRelacionado(clienteId))
            {
                throw new InvalidOperationException($"El cliente con ID {clienteId} no se puede eliminar porque tiene registros asociados.");
            }

            try
            {
                _clienteRepositorio.Borrar(clienteId);
                _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al intentar borrar el cliente con ID {clienteId}.", ex);
            }
        }

        public ClienteEditDto? ObtenerParaEditar(int clienteId)
        {
            try
            {
                if (clienteId <= 0)
                    throw new ArgumentException("El ID del cliente debe ser un entero mayor a cero.", nameof(clienteId));

                Cliente? cliente = _clienteRepositorio.ObtenerPorId(clienteId);
                if (cliente is null)
                    throw new KeyNotFoundException($"No se encontró un cliente con el ID {clienteId}");

                return cliente.ToEditDto();

            }
            catch (Exception ex)
            {

                throw new Exception($"Error al obtener el cliente con ID {clienteId} para edición: {ex.Message}", ex);
            } 
        }

        public void Editar(ClienteEditDto clienteDto)
        {
            if (clienteDto is null)
                throw new ArgumentNullException(nameof(clienteDto), "El cliente no puede ser nulo.");

            if (clienteDto.ClienteId <= 0)
                throw new ArgumentException("El ID del cliente debe ser mayor a 0.", nameof(clienteDto.ClienteId));

            Cliente cliente = clienteDto.ToEntidad();

            if (_clienteRepositorio.ExisteCliente(cliente))
            {
                throw new InvalidOperationException($"Ya existe un cliente registrado con el documento {cliente.Documento}");
            }

            try
            {
                _clienteRepositorio.Editar(cliente, cliente.ClienteId);
                _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al intentar actualizar el cliente con ID {cliente.ClienteId}.", ex);
            }
        }

        public int ObtenerPaginaRegistro(string documento, int cantidadPorPagina,
            bool? filtroActivo = null, string? textoBuscar = null)
        {
            if (string.IsNullOrWhiteSpace(documento))
                throw new ArgumentException("El documento no puede estar vacío.", nameof(documento));

            if (cantidadPorPagina <= 0) cantidadPorPagina = 10;

            int posicion = _clienteRepositorio
                .ObtenerPosicionAlfabetica(documento, filtroActivo, textoBuscar);

            if (posicion <= 0) return 1;

            try
            {
                return (int)Math.Ceiling((double)posicion / cantidadPorPagina);

            }
            catch (Exception ex)
            {

                throw new Exception($"Error al obtener la página de registro para el cliente con documento {documento}: {ex.Message}", ex);
            } 
        }
    }
}
