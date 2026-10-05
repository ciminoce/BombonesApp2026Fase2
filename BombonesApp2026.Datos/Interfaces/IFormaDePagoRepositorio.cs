using BombonesApp2026.Entidades.Entidades;

namespace BombonesApp2026.Datos.Interfaces
{
    public interface IFormaDePagoRepositorio:IRepositorio<FormaDePago>
    {
        bool ExisteFormaDePago(FormaDePago formaDePago);
        List<FormaDePago> FiltrarPorActivo(bool activo);
        (List<FormaDePago> lista, int cantidadRegistros) ObtenerPagina(int paginaActual, int cantidadPorPagina, bool? filtroActivo = null, string? textoBuscar = null);
        int ObtenerPosicionAlfabetica(string nombre, bool? filtroActivo = null, string? textoBuscar = null);
        bool TieneRegistrosRelacionados(int formaDePagoId);
    }
}