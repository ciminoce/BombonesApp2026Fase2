using BombonesApp2026.Datos.Interfaces;
using BombonesApp2026.Entidades.Entidades;
using Microsoft.EntityFrameworkCore;

namespace BombonesApp2026.Datos.Repositorios
{
    public class BombonRepositorio :Repositorio<Bombon>, IBombonRepositorio
    {
        public BombonRepositorio(BombonesDbContext context) : base(context)
        {
        }

        public override List<Bombon> ObtenerTodos()
        {
            return _context.Bombones
                .Include(b => b.TipoBombon)
                .AsNoTracking()
                .ToList();
        }
        public (List<Bombon> lista, int cantidadRegistros) ObtenerPagina(int paginaActual,
    int cantidadPorPagina, bool? filtroActivo = null,
    string? textoBuscar = null)
        {
            IQueryable<Bombon> query = _context
                .Bombones
                .Include(b => b.TipoBombon)
                .AsNoTracking();
            if (!string.IsNullOrWhiteSpace(textoBuscar))
            {
                query = query.Where(b => b.Nombre.Contains(textoBuscar));
            }
            var cantidad = query.Count();
            var lista = query
                .OrderBy(b => b.Nombre)
                .Skip(cantidadPorPagina * (paginaActual - 1))
                .Take(cantidadPorPagina)
                .ToList();
            return (lista, cantidad);
        }
        public int ObtenerPosicionAlfabetica(string nombre,
                bool? filtroActivo = null, string? textoBuscar = null)
        {
            IQueryable<Bombon> query = _context.Bombones.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(textoBuscar))
            {
                query = query.Where(b => b.Nombre.Contains(textoBuscar));
            }
            return query
                .Count(c => string
                    .Compare(c.Nombre, nombre) <= 0);
        }


        public bool ExisteBombon(Bombon Bombon)
        {
            return _context.Bombones.Any(b => b.Nombre == Bombon.Nombre
                && b.ProductoId != Bombon.ProductoId);
        }

        public override Bombon? ObtenerPorId(int ProductoId)
        {
            return _context.Bombones
                .AsNoTracking()
                .FirstOrDefault(b => b.ProductoId == ProductoId);
        }


    }
}
