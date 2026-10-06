using BombonesApp2026.Entidades.Entidades;

namespace BombonesApp2026.Datos.Interfaces
{
    public interface IBombonRepositorio:IRepositorio<Bombon>
    {
        bool ExisteBombon(Bombon bombon);
        (List<Bombon> lista, int cantidadRegistros) ObtenerPagina(int paginaActual, int cantidadPorPagina, bool? filtroActivo = null, string? textoBuscar = null);
        int ObtenerPosicionAlfabetica(string nombre, bool? filtroActivo = null, string? textoBuscar = null);
    }
}