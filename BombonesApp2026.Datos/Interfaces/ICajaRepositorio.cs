using BombonesApp2026.Entidades.Entidades;

namespace BombonesApp2026.Datos.Interfaces
{
    public interface ICajaRepositorio : IRepositorio<Caja>
    {
        bool ExisteCaja(Caja caja);
        Caja? ObtenerCajaConDetalles(int productoId);
        (List<Caja> lista, int cantidadRegistros) ObtenerPagina(int paginaActual,
            int cantidadPorPagina, bool? filtroActivo = null,
            string? textoBuscar = null);
        int ObtenerPosicionAlfabetica(string nombre, bool? filtroActivo = null, string? textoBuscar = null);

    }
}
