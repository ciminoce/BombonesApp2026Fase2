using BombonesApp2026.Entidades.Entidades;

namespace BombonesApp2026.Datos.Interfaces
{
    public interface IClienteRepositorio
    {
        void Agregar(Cliente cliente);
        void Borrar(int clienteId);
        void Editar(Cliente cliente);
        bool ExisteBombon(Cliente cliente);
        bool ExisteCliente(Cliente cliente);
        (List<Cliente> lista, int cantidadRegistros) ObtenerPagina(int paginaActual, int cantidadPorPagina, bool? filtroActivo = null, string? textoBuscar = null);
        Cliente? ObtenerPorId(int clienteId);
        int ObtenerPosicionAlfabetica(string nombre, bool? filtroActivo = null, string? textoBuscar = null);
        List<Cliente> ObtenerTodos();
    }
}