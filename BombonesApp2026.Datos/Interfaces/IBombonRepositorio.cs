using BombonesApp2026.Entidades.Entidades;

namespace BombonesApp2026.Datos.Interfaces
{
    public interface IBombonRepositorio
    {
        void Agregar(Bombon bombon);
        void Borrar(int productoId);
        void Editar(Bombon bombon);
        bool ExisteBombon(Bombon bombon);
        (List<Bombon> lista, int cantidadRegistros) ObtenerPagina(int paginaActual, int cantidadPorPagina, bool? filtroActivo = null, string? textoBuscar = null);
        Bombon? ObtenerPorId(int productoId);
        int ObtenerPosicionAlfabetica(string nombre, bool? filtroActivo = null, string? textoBuscar = null);
        List<Bombon> ObtenerTodos();
    }
}