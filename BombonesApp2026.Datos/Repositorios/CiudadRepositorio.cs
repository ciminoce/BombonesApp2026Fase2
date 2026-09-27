using BombonesApp2026.Datos.Interfaces;
using BombonesApp2026.Entidades.Entidades;
using Microsoft.EntityFrameworkCore;

namespace BombonesApp2026.Datos.Repositorios
{
    public class CiudadRepositorio : ICiudadRepositorio
    {
        private readonly BombonesDbContext _context;
        public CiudadRepositorio(BombonesDbContext context)
        {
            _context = context;
        }
        public List<Ciudad> ObtenerTodos(int? provinciaId = null)
        {
            return _context.Ciudades
                .OrderBy(c => c.Nombre)
                .Where(c => c.ProvinciaId == provinciaId)
                .ToList();
        }
        public (List<Ciudad> lista, int cantidadRegistros) ObtenerPagina(int paginaActual,
    int cantidadPorPagina, bool? filtroActivo = null,
    string? textoBuscar = null)
        {
            IQueryable<Ciudad> query = _context
                .Ciudades
                .Include(c => c.Provincia)
                .AsNoTracking();
            if (!string.IsNullOrWhiteSpace(textoBuscar))
            {
                query = query.Where(c => c.Nombre.Contains(textoBuscar));
            }
            var cantidad = query.Count();
            var lista = query
                .OrderBy(c => c.Nombre)
                .Skip(cantidadPorPagina * (paginaActual - 1))
                .Take(cantidadPorPagina)
                .ToList();
            return (lista, cantidad);
        }
        public int ObtenerPosicionAlfabetica(string nombre,
                bool? filtroActivo = null, string? textoBuscar = null)
        {
            IQueryable<Ciudad> query = _context.Ciudades.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(textoBuscar))
            {
                query = query.Where(c => c.Nombre.Contains(textoBuscar));
            }
            return query
                .Count(c => string
                    .Compare(c.Nombre, nombre) <= 0);
        }

        public void Agregar(Ciudad ciudad)
        {
            _context.Ciudades.Add(ciudad);
            _context.SaveChanges();
        }

        public bool ExisteCiudad(Ciudad ciudad)
        {
            if (ciudad.CiudadId == 0)
            {
                return _context.Ciudades.Any(c => c.Nombre == ciudad.Nombre
                    && c.ProvinciaId == ciudad.ProvinciaId);
            }
            else
            {
                return _context.Ciudades.Any(c => c.Nombre == ciudad.Nombre
                    && c.ProvinciaId == ciudad.ProvinciaId
                    && c.CiudadId != ciudad.CiudadId);

            }
        }
        public void Borrar(int ciudadId)
        {
            var ciudadEnDb = _context.Ciudades.Find(ciudadId);
            if (ciudadEnDb is null) throw new KeyNotFoundException($"No se encuentra una ciudad con ID: {ciudadId}");
            _context.Ciudades.Remove(ciudadEnDb);
            _context.SaveChanges();
        }

        public Ciudad? ObtenerPorId(int ciudadId)
        {
            return _context.Ciudades
                .FirstOrDefault(c => c.CiudadId == ciudadId);
        }

        public void Editar(Ciudad ciudad)
        {
            var ciudadEnDb = _context.Ciudades.Find(ciudad.CiudadId);

            if (ciudadEnDb is null) throw new Exception("Ciudad no encontrada");
            ciudadEnDb.Nombre = ciudad.Nombre;
            ciudadEnDb.ProvinciaId = ciudad.ProvinciaId;

            _context.SaveChanges();

        }
    }
}
