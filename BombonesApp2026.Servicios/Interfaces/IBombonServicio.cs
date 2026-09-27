using Bombones2026.Servicios.DTOs.Paginacion;
using BombonesApp2026.Servicios.DTOs.Bombon;

namespace BombonesApp2026.Servicios.Interfaces
{
    public interface IBombonServicio
    {
        int Agregar(BombonCreateDto bombonDto);
        void Borrar(int bombonId);
        void Editar(BombonEditDto bombonDto);
        ResultadoPaginacionDto<BombonListDto> ObtenerPagina(int paginaActual, int cantidadPorPagina, bool? filtroActivo = null, string? textoBuscar = null);
        int ObtenerPaginaRegistro(string nombre, int cantidadPorPagina, bool? filtroActivo = null, string? textoBuscar = null);
        BombonEditDto? ObtenerParaEditar(int bombonId);
        List<BombonListDto> ObtenerTodos();
    }
}