using BombonesApp2026.Entidades.Entidades;

namespace CajaesApp2026.Datos.Interfaces
{
    public interface ICajaRepositorio
    {
        void Agregar(Caja caja);
        void Borrar(int productoId);
        void Editar(Caja caja);
        bool ExisteCaja(Caja caja);
        (List<Caja> lista, int cantidadRegistros) ObtenerPagina(int paginaActual,
            int cantidadPorPagina, bool? filtroActivo = null,
            string? textoBuscar = null);
        Caja? ObtenerPorId(int productoId);
        int ObtenerPosicionAlfabetica(string nombre, bool? filtroActivo = null, string? textoBuscar = null);
        List<Caja> ObtenerTodos();

    }
}
