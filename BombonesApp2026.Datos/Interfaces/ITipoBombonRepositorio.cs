using BombonesApp2026.Entidades.Entidades;

namespace BombonesApp2026.Datos.Interfaces
{
    public interface ITipoBombonRepositorio
    {
        void Agregar(TipoBombon tipoBombon);
        void Borrar(int id);
        void Editar(TipoBombon tipoBombon);
        bool ExisteTipoBombon(TipoBombon tipoBombon);
        (List<TipoBombon> lista, int cantidadRegistros) ObtenerPagina(int paginaActual, int cantidadPorPagina, bool? filtroActivo = null, string? textoBuscar = null);
        TipoBombon? ObtenerPorId(int id);
        int ObtenerPosicionAlfabetica(string nombre, bool? filtroActivo = null, string? textoBuscar = null);
        List<TipoBombon> ObtenerTodos();
        bool TieneRegistrosRelacionados(int tipoId);
    }
}