using BombonesApp2026.Datos.Interfaces;
using BombonesApp2026.Entidades.Entidades;
using Microsoft.EntityFrameworkCore;

namespace BombonesApp2026.Datos.Repositorios
{
    public class TipoBombonRepositorio : ITipoBombonRepositorio
    {
        private readonly BombonesDbContext _context;
        public TipoBombonRepositorio(BombonesDbContext context)
        {
            _context = context;
        }
        public List<TipoBombon> ObtenerTodos()
        {
            return _context.TipoBombones
                .OrderBy(tb => tb.Nombre)
                .AsNoTracking()
                .ToList();
        }
        public (List<TipoBombon> lista, int cantidadRegistros) ObtenerPagina(int paginaActual,
            int cantidadPorPagina, bool? filtroActivo = null,
            string? textoBuscar = null)
        {
            IQueryable<TipoBombon> query = _context
                .TipoBombones.AsNoTracking();
            if (filtroActivo is not null)
            {
                query = query.Where(tb => tb.Activo == filtroActivo);
            }
            if (!string.IsNullOrWhiteSpace(textoBuscar))
            {
                query = query.Where(tb => tb.Nombre.Contains(textoBuscar));
            }
            var cantidad = query.Count();
            var lista = query
                .OrderBy(tb => tb.Nombre)
                .Skip(cantidadPorPagina * (paginaActual - 1))
                .Take(cantidadPorPagina)
                .ToList();
            return (lista, cantidad);
        }
        public void Agregar(TipoBombon tipoBombon)
        {
            _context.TipoBombones.Add(tipoBombon);

            _context.SaveChanges();

        }
        public void Editar(TipoBombon tipoBombon)
        {
            var tipoEnDb = _context.TipoBombones.Find(tipoBombon.TipoBombonId);

            if (tipoEnDb is null) throw new Exception("Tipo de Bombon no encontrado");
            tipoEnDb.Nombre = tipoBombon.Nombre;
            tipoEnDb.Descripcion = tipoBombon.Descripcion;
            tipoEnDb.Activo = tipoBombon.Activo;

            _context.SaveChanges();
        }
        public void Borrar(int id)
        {
            var tipoEnDb = _context.TipoBombones
                .Find(id);
            if (tipoEnDb is null) throw new Exception("Tipo de Bombón no encontrado");
            _context.TipoBombones.Remove(tipoEnDb);
            _context.SaveChanges();
        }
        public TipoBombon? ObtenerPorId(int id)
        {
            return _context.TipoBombones.AsNoTracking()
                .FirstOrDefault(tb => tb.TipoBombonId == id);
        }

        public bool ExisteTipoBombon(TipoBombon tipoBombon)
        {
            if (tipoBombon.TipoBombonId == 0)
            {
                return _context.TipoBombones.Any(tb => tb.Nombre == tipoBombon.Nombre);
            }
            else
            {
                return _context.TipoBombones.Any(tb => tb.Nombre == tipoBombon.Nombre &&
                        tb.TipoBombonId != tipoBombon.TipoBombonId);
            }
        }

        public bool TieneRegistrosRelacionados(int tipoId)
        {
            return false;
        }

        public int ObtenerPosicionAlfabetica(string nombre,
            bool? filtroActivo = null, string? textoBuscar = null)
        {
            IQueryable<TipoBombon> query = _context.TipoBombones.AsNoTracking();
            if (filtroActivo.HasValue)
            {
                query = query.Where(tb => tb.Activo == filtroActivo.Value);
            }
            if (!string.IsNullOrWhiteSpace(textoBuscar))
            {
                query = query.Where(tb => tb.Nombre.Contains(textoBuscar));
            }
            return query
                .Count(tb => string
                    .Compare(tb.Nombre, nombre) <= 0);
        }
    }
}
