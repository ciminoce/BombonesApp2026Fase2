using BombonesApp2026.Datos.Interfaces;
using BombonesApp2026.Entidades.Entidades;
using Microsoft.EntityFrameworkCore;

namespace BombonesApp2026.Datos.Repositorios
{
    //TODO:Ver registros relacionadados
    public class ProvinciaRepositorio : IProvinciaRepositorio
    {
        private readonly BombonesDbContext _context;
        public ProvinciaRepositorio(BombonesDbContext context)
        {
            _context = context;
        }
        public List<Provincia> ObtenerTodos()
        {
            return _context.Provincias
                .OrderBy(p => p.NombreProvincia)
                .AsNoTracking()
                .ToList();
        }
        public (List<Provincia> lista, int cantidadRegistros) ObtenerPagina(int paginaActual,
            int cantidadPorPagina, bool? filtroActivo = null,
            string? textoBuscar = null)
        {
            IQueryable<Provincia> query = _context
                .Provincias.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(textoBuscar))
            {
                query = query.Where(p => p.NombreProvincia.Contains(textoBuscar));
            }
            var cantidad = query.Count();
            var lista = query
                .OrderBy(p => p.NombreProvincia)
                .Skip(cantidadPorPagina * (paginaActual - 1))
                .Take(cantidadPorPagina)
                .ToList();
            return (lista, cantidad);
        }
        public int ObtenerPosicionAlfabetica(string nombre,
                bool? filtroActivo = null, string? textoBuscar = null)
        {
            IQueryable<Provincia> query = _context.Provincias.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(textoBuscar))
            {
                query = query.Where(p => p.NombreProvincia.Contains(textoBuscar));
            }
            return query
                .Count(p => string
                    .Compare(p.NombreProvincia, nombre) <= 0);
        }

        public void Agregar(Provincia provincia)
        {
            _context.Provincias.Add(provincia);

            _context.SaveChanges();
        }
        public void Editar(Provincia provincia)
        {
            var provinciaEnDb = _context.Provincias.Find(provincia.ProvinciaId);

            if (provinciaEnDb is null) throw new Exception("Provincia no encontrada");
            provinciaEnDb.NombreProvincia = provincia.NombreProvincia;

            _context.SaveChanges();
        }
        public void Borrar(int id)
        {
            var provinciaEnDb = _context.Provincias
                .Find(id);
            if (provinciaEnDb is null) throw new Exception("Provincia no encontrada");
            _context.Provincias.Remove(provinciaEnDb);
            _context.SaveChanges();
        }
        public Provincia? ObtenerPorId(int id)
        {
            return _context.Provincias.AsNoTracking()
                .FirstOrDefault(p => p.ProvinciaId == id);
        }

        public bool ExisteProvincia(Provincia provincia)
        {
            if (provincia.ProvinciaId == 0)
            {
                return _context.Provincias.Any(p => p.NombreProvincia == provincia.NombreProvincia);
            }
            else
            {
                return _context.Provincias.Any(p => p.NombreProvincia == provincia.NombreProvincia &&
                        p.ProvinciaId != provincia.ProvinciaId);
            }
        }

        public bool TieneRegistrosRelacionados(int provinciaId)
        {
            return false;
        }

    }
}
