using BombonesApp2026.Datos.Interfaces;
using BombonesApp2026.Entidades.Entidades;
using Microsoft.EntityFrameworkCore;

namespace BombonesApp2026.Datos.Repositorios
{
    public class RolRepositorio : IRolRepositorio
    {
        private readonly BombonesDbContext _context;
        public RolRepositorio(BombonesDbContext context)
        {
            _context = context;
        }
        public List<Rol> ObtenerTodos()
        {
            return _context.Roles
                .AsNoTracking()
                .ToList();
        }
        public List<Rol> FiltrarPorActivo(bool activo)
        {
            return _context.Roles
                .AsNoTracking()
                .Where(r => r.Activo == activo)
                .ToList();
        }
        public void Agregar(Rol rol)
        {
            _context.Roles.Add(rol);
            _context.SaveChanges();

        }
        public void Editar(Rol rol)
        {
            var rolEnDb = _context.Roles.Find(rol.RolId);

            if (rolEnDb is null) throw new Exception("Rol no encontrado");
            rolEnDb.Nombre = rol.Nombre;
            rolEnDb.Descripcion = rol.Descripcion;
            rolEnDb.Activo = rol.Activo;

            _context.SaveChanges();

        }
        public void Borrar(int id)
        {
            var rolEnDb = _context.Roles
                .Find(id);
            if (rolEnDb is null) throw new Exception("Rol no encontrado");
            _context.Roles.Remove(rolEnDb);
            _context.SaveChanges();
        }
        public Rol? ObtenerPorId(int id)
        {
            return _context.Roles.AsNoTracking()
                .FirstOrDefault(r => r.RolId == id);
        }

        public bool ExisteRol(Rol rol)
        {
            if (rol.RolId == 0)
            {
                return _context.Roles.Any(r => r.Nombre == rol.Nombre);
            }
            else
            {
                return _context.Roles.Any(r => r.Nombre == rol.Nombre &&
                        r.RolId != rol.RolId);
            }
        }

        public bool TieneRegistrosRelacionados(int rolId)
        {
            return false;
        }
    }
}
