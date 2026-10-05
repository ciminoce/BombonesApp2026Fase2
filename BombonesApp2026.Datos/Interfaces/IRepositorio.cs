namespace BombonesApp2026.Datos.Interfaces
{
    public interface IRepositorio<T> where T:class
    {
        void Agregar(T entidad);
        void Borrar(int id);
        void Editar(T entidad, int id);
        T? ObtenerPorId(int id);
        List<T> ObtenerTodos();
    }
}
