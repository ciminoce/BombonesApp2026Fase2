using Bombones2026.Servicios.DTOs.Paginacion;
using BombonesApp2026.Servicios.DTOs.Caja;

namespace BombonesApp2026.Servicios.Interfaces
{
    public interface ICajaServicio
    {
        int Agregar(CajaCreateDto cajaCreateDto);
        void Borrar(int productoId);
        void Editar(CajaEditDto cajaEditDto);
        ResultadoPaginacionDto<CajaListDto> ObtenerPagina(int paginaActual, int cantidadPorPagina, bool? filtroActivo = null, string? textoBuscar = null);
        int ObtenerPaginaRegistro(string nombre, int cantidadPorPagina, bool? filtroActivo, string? textoBuscar);
        CajaEditDto? ObtenerParaEditar(int productoId);
        List<CajaListDto> ObtenerTodas();
    }
}
