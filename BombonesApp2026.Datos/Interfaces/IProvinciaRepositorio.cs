using BombonesApp2026.Entidades.Entidades;

namespace BombonesApp2026.Datos.Interfaces
{
    public interface IProvinciaRepositorio
    {
        void Agregar(Provincia provincia);
        void Borrar(int id);
        void Editar(Provincia provincia);
        bool ExisteProvincia(Provincia provincia);
        (List<Provincia> lista, int cantidadRegistros) ObtenerPagina(int paginaActual, int cantidadPorPagina, bool? filtroActivo = null, string? textoBuscar = null);
        Provincia? ObtenerPorId(int id);
        int ObtenerPosicionAlfabetica(string nombre, bool? filtroActivo = null, string? textoBuscar = null);
        List<Provincia> ObtenerTodos();
        bool TieneRegistrosRelacionados(int provinciaId);
    }
}