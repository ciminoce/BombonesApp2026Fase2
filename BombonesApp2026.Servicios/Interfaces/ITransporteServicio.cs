using Bombones2026.Servicios.DTOs.Paginacion;
using Bombones2026.Servicios.DTOs.Transporte;

namespace BombonesApp2026.Servicios.Interfaces
{
    public interface ITransporteServicio
    {
        int Agregar(TransporteCreateDto? transporteDto);
        void Borrar(int transporteId);
        void Editar(TransporteEditDto? transporteDto);
        ResultadoPaginacionDto<TransporteListDto> ObtenerPagina(int paginaActual, int cantidadPorPagina, bool? filtroActivo = null, int? provinciaIdFiltro = null, string? textoBuscar = null);
        int ObtenerPaginaRegistro(string nombre, int cantidadPorPagina, bool? filtroActivo = null, int? provinciaIdFiltro = null, string? textoBuscar = null);
        TransporteEditDto? ObtenerParaEditar(int transporteId);
        List<TransporteListDto> ObtenerTodos();
    }
}