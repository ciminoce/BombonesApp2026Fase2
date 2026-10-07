using BombonesApp2026.Entidades.Entidades;

namespace BombonesApp2026.Datos.Interfaces
{
    public interface ICiudadRepositorio:IRepositorio<Ciudad>
    {
        bool ExisteCiudad(Ciudad ciudad);
        (List<Ciudad> lista, int cantidadRegistros) ObtenerPagina(int paginaActual, int cantidadPorPagina, bool? filtroActivo = null, string? textoBuscar = null);
        int ObtenerPosicionAlfabetica(string nombre, bool? filtroActivo = null, string? textoBuscar = null);
        List<Ciudad> ObtenerTodos(int? provinciaId = null);
    }
}