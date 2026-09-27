using BombonesApp2026.Entidades.Entidades;

namespace BombonesApp2026.Datos.Interfaces
{
    public interface IRolRepositorio
    {
        void Agregar(Rol rol);
        void Borrar(int id);
        void Editar(Rol rol);
        bool ExisteRol(Rol rol);
        List<Rol> FiltrarPorActivo(bool activo);
        Rol? ObtenerPorId(int id);
        List<Rol> ObtenerTodos();
        bool TieneRegistrosRelacionados(int rolId);
    }
}