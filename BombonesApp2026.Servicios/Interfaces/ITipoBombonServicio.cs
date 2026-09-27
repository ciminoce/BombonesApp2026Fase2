using Bombones2026.Servicios.DTOs.Paginacion;
using Bombones2026.Servicios.DTOs.TipoBombon;
using BombonesApp2026.Entidades.Enum;

namespace BombonesApp2026.Servicios.Interfaces
{
    public interface ITipoBombonServicio
    {
        int Agregar(TipoBombonCreateDto? tipoDto);
        void Borrar(int tipoBombonId);
        void Editar(TipoBombonEditDto? tipoDto);
        List<TipoBombonListDto> ObtenerDatosCombo(TipoBombonDefault tipoDefault);
        ResultadoPaginacionDto<TipoBombonListDto> ObtenerPagina(int paginaActual, int cantidadPorPagina, bool? filtroActivo = null, string? textoBuscar = null);
        int ObtenerPaginaRegistro(string nombre, int cantidadPorPagina, bool? filtroActivo = null, string? textoBuscar = null);
        TipoBombonEditDto ObtenerParaEditar(int id);
        List<TipoBombonListDto> ObtenerTodos();
    }
}