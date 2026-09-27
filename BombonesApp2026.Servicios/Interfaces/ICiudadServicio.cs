using Bombones2026.Servicios.DTOs.Ciudad;
using Bombones2026.Servicios.DTOs.Paginacion;
using BombonesApp2026.Entidades.Enum;

namespace BombonesApp2026.Servicios.Interfaces
{
    public interface ICiudadServicio
    {
        int Agregar(CiudadCreateDto ciudadDto);
        void Borrar(int ciudadId);
        void Editar(CiudadEditDto ciudadDto);
        List<CiudadListDto> ObtenerDatosCombo(TipoCiudadDefault tipoDefault, int provinciaId);
        ResultadoPaginacionDto<CiudadListDto> ObtenerPagina(int paginaActual, int cantidadPorPagina, bool? filtroActivo = null, string? textoBuscar = null);
        int ObtenerPaginaRegistro(string nombre, int cantidadPorPagina, bool? filtroActivo = null, string? textoBuscar = null);
        CiudadEditDto? ObtenerParaEditar(int ciudadId);
        List<CiudadListDto> ObtenerTodos();
    }
}