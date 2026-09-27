using Bombones2026.Servicios.DTOs.FormaDePago;
using Bombones2026.Servicios.DTOs.Paginacion;

namespace BombonesApp2026.Servicios.Interfaces
{
    public interface IFormaDePagoServicio
    {
        int Agregar(FormaDePagoCreateDto? formaDePagoDto);
        void Borrar(int formaDePagoId);
        void Editar(FormaDePagoEditDto? formaDePagoDto);
        List<FormaDePagoListDto> FiltrarPorActivo(bool activo);
        ResultadoPaginacionDto<FormaDePagoListDto> ObtenerPagina(int paginaActual, int cantidadPorPagina, bool? filtroActivo = null, string? textoBuscar = null);
        int ObtenerPaginaRegistro(string nombre, int cantidadPorPagina, bool? filtroActivo = null, string? textoBuscar = null);
        FormaDePagoEditDto ObtenerParaEditar(int id);
        List<FormaDePagoListDto> ObtenerTodos();
    }
}