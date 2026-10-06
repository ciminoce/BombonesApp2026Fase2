using BombonesApp2026.Entidades.Entidades;

namespace BombonesApp2026.Datos.Interfaces
{
    public interface IClienteRepositorio:IRepositorio<Cliente>
    {
        bool EstaRelacionado(int clienteId);
        bool ExisteCliente(Cliente cliente);
        (List<Cliente> lista, int cantidadRegistros) ObtenerPagina(int paginaActual, int cantidadPorPagina, bool? filtroActivo = null, string? textoBuscar = null);
        int ObtenerPosicionAlfabetica(string nombre, bool? filtroActivo = null, string? textoBuscar = null);
    }
}