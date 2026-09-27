using BombonesApp2026.Entidades.Entidades;

namespace BombonesApp2026.Datos.Interfaces
{
    public interface ITransporteRepositorio
    {
        void Agregar(Transporte transporte);
        void Borrar(int transporteId);
        void Editar(Transporte transporte);
        bool ExisteTransporte(Transporte transporte);
        (List<Transporte> lista, int cantidadRegistros) ObtenerPagina(int paginaActual, int cantidadPorPagina, bool? filtroActivo = null, int? provinciaIdFiltro = null, string? textoBuscar = null);
        Transporte? ObtenerPorId(int transporteId);
        int ObtenerPosicionAlfabetica(string nombre, bool? filtroActivo = null, int? provinciaIdFiltro = null, string? textoBuscar = null);
        List<Transporte> ObtenerTodos();
        bool TieneRegistrosRelacionados(int transporteId);
    }
}