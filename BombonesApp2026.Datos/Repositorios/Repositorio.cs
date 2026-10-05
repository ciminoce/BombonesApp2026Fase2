using BombonesApp2026.Datos.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BombonesApp2026.Datos.Repositorios
{
    public class Repositorio<T> : IRepositorio<T> where T : class
    {
        protected readonly BombonesDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repositorio(BombonesDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public void Agregar(T entidad)
        {
            _dbSet.Add(entidad);
        }

        public void Borrar(int id)
        {
            var entidadEnDb=_dbSet.Find(id);
            if(entidadEnDb is null)
            {
                throw new KeyNotFoundException($"No se encontró ninguna entidad con el ID: {id}");
            }
            _dbSet.Remove(entidadEnDb);
        }

        public void Editar(T entidad, int id)
        {
            var entidadEnDb = _dbSet.Find(id);
            if (entidadEnDb is null)
            {
                throw new KeyNotFoundException($"No se encontró ninguna entidad con el ID: {id}");
            }
            _context.Entry(entidadEnDb).CurrentValues.SetValues(entidad);
        }

        public virtual T? ObtenerPorId(int id)
        {
            return _dbSet.Find(id);
        }

        public virtual List<T> ObtenerTodos()
        {
            return _dbSet.AsNoTracking()
                .ToList();
        }
    }
}
