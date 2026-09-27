using Bombones2026.Servicios.DTOs.Paginacion;
using Bombones2026.Servicios.DTOs.Provincia;
using BombonesApp2026.Entidades.Enum;

namespace BombonesApp2026.Servicios.Interfaces
{
    public interface IProvinciaServicio
    {
        int Agregar(ProvinciaCreateDto? provinciaDto);
        void Borrar(int provinciaId);
        void Editar(ProvinciaEditDto? provinciaDto);
        List<ProvinciaListDto> ObtenerDatosCombo(TipoProvinciaDefault tipoDefault);
        ResultadoPaginacionDto<ProvinciaListDto> ObtenerPagina(int paginaActual, int cantidadPorPagina, bool? filtroActivo = null, string? textoBuscar = null);
        int ObtenerPaginaRegistro(string nombre, int cantidadPorPagina, bool? filtroActivo = null, string? textoBuscar = null);
        ProvinciaEditDto ObtenerParaEditar(int id);
        List<ProvinciaListDto> ObtenerTodos();
    }
}