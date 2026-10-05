using BombonesApp2026.Entidades.Entidades;

namespace BombonesApp2026.Datos.Interfaces
{
    public interface ITransporteRepositorio:IRepositorio<Transporte>
    {
        bool ExisteTransporte(Transporte transporte);
        (List<Transporte> lista, int cantidadRegistros) ObtenerPagina(int paginaActual, int cantidadPorPagina, bool? filtroActivo = null, int? provinciaIdFiltro = null, string? textoBuscar = null);
        int ObtenerPosicionAlfabetica(string nombre, bool? filtroActivo = null, int? provinciaIdFiltro = null, string? textoBuscar = null);
        bool TieneRegistrosRelacionados(int transporteId);
    }
}