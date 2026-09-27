using BombonesApp2026.Entidades.Entidades;

namespace BombonesApp2026.Datos.Interfaces
{
    public interface IFormaDePagoRepositorio
    {
        void Agregar(FormaDePago formaDePago);
        void Borrar(int id);
        void Editar(FormaDePago formaDePago);
        bool ExisteFormaDePago(FormaDePago formaDePago);
        List<FormaDePago> FiltrarPorActivo(bool activo);
        (List<FormaDePago> lista, int cantidadRegistros) ObtenerPagina(int paginaActual, int cantidadPorPagina, bool? filtroActivo = null, string? textoBuscar = null);
        FormaDePago? ObtenerPorId(int id);
        int ObtenerPosicionAlfabetica(string nombre, bool? filtroActivo = null, string? textoBuscar = null);
        List<FormaDePago> ObtenerTodos();
        bool TieneRegistrosRelacionados(int formaDePagoId);
    }
}