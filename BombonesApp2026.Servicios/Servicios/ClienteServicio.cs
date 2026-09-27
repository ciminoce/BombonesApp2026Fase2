using Bombones2026.Servicios.DTOs.Paginacion;
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
        public ClienteServicio(IClienteRepositorio bombonRepositorio)
        {
            _clienteRepositorio = bombonRepositorio;
        }
        public ResultadoPaginacionDto<ClienteListDto> ObtenerPagina(int paginaActual,
            int cantidadPorPagina, bool? filtroActivo = null, string? textoBuscar = null)
        {
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
            catch (Exception)
            {

                throw;
            }
        }

        public List<ClienteListDto> ObtenerTodos()
        {
            return _clienteRepositorio.ObtenerTodos()
                .Select(b => b.ToListDto()).ToList();
        }
        public int Agregar(ClienteCreateDto clienteDto)
        {
            Cliente cliente = clienteDto.ToEntidad();
            if (_clienteRepositorio.ExisteCliente(cliente))
            {
                throw new ArgumentException(nameof(cliente), $"Ya existe un cliente con el documento {cliente.Documento}");
            }
            try
            {
                _clienteRepositorio.Agregar(cliente);
                return cliente.ClienteId;
            }
            catch (Exception ex)
            {

                throw new Exception($"Error al intentar guardar un cliente {ex.Message}");
            }
        }

        public void Borrar(int clienteId)
        {
            if (clienteId <= 0)
            {
                throw new ArgumentException(nameof(clienteId),
                    "El ID debe ser positivo");
            }
            var cliente = _clienteRepositorio.ObtenerPorId(clienteId);
            if (cliente is null)
            {
                throw new KeyNotFoundException($"No se encontró un cliente con el ID {clienteId}");
            }
            //OJO falta ver si el registro está relacionado
            try
            {
                _clienteRepositorio.Borrar(clienteId);
            }
            catch (Exception ex)
            {

                throw new Exception($"Error al intentar borrar un cliente: {ex.Message}");
            }
        }
        public ClienteEditDto? ObtenerParaEditar(int clienteId)
        {
            // AJUSTE: Validación defensiva del ID antes de operar
            if (clienteId <= 0)
                throw new ArgumentException("El ID cliente debe ser entero mayor a cero.", nameof(clienteId));


            Cliente? cliente = _clienteRepositorio.ObtenerPorId(clienteId);
            if (cliente is null) throw new ArgumentException(nameof(clienteId), $"Id {clienteId} no encontrado");
            ClienteEditDto clienteDto = cliente.ToEditDto();
            return clienteDto;
        }

        public void Editar(ClienteEditDto clienteDto)
        {
            if (clienteDto == null)
                throw new ArgumentNullException(nameof(clienteDto), "El cliente no puede ser nulo");
            if (clienteDto.ClienteId == 0)
            {
                throw new ArgumentException(nameof(clienteDto.ClienteId), "El ID del cliente debe ser mayor a 0");
            }
            Cliente cliente = clienteDto.ToEntidad();
            if (_clienteRepositorio.ExisteCliente(cliente)) throw new InvalidOperationException($"Ya existe un cliente {cliente.Documento}");
            _clienteRepositorio.Editar(cliente);

        }

        public int ObtenerPaginaRegistro(string documento, int cantidadPorPagina,
            bool? filtroActivo = null, string? textoBuscar = null)
        {
            int posicion = _clienteRepositorio
                .ObtenerPosicionAlfabetica(documento, filtroActivo, textoBuscar);
            return (int)Math.Ceiling((double)posicion / cantidadPorPagina);
        }
    }

}
