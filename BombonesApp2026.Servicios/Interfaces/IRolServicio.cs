using Bombones2026.Servicios.DTOs.Rol;

namespace BombonesApp2026.Servicios.Interfaces
{
    public interface IRolServicio
    {
        int Agregar(RolCreateDto? rolDto);
        void Borrar(int rolId);
        void Editar(RolEditDto? rolDto);
        List<RolListDto> FiltrarPorActivo(bool activo);
        RolEditDto ObtenerParaEditar(int id);
        List<RolListDto> ObtenerTodos();
    }
}