using BombonesApp2026.Datos;
using BombonesApp2026.Datos.Repositorios;
using BombonesApp2026.Entidades.Entidades;
using CajaesApp2026.Datos.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CajaesApp2026.Datos.Repositorios
{
    public class CajaRepositorio :Repositorio<Caja>, ICajaRepositorio
    {
        public CajaRepositorio(BombonesDbContext context) : base(context)
        {
        }

        public (List<Caja> lista, int cantidadRegistros) ObtenerPagina(int paginaActual,
            int cantidadPorPagina, bool? filtroActivo = null,
            string? textoBuscar = null)
        {
            IQueryable<Caja> query = _context
                .Cajas
                .Include(c=>c.Detalles)
                .ThenInclude(d=>d.Bombon)
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
            IQueryable<Caja> query = _context.Cajas.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(textoBuscar))
            {
                query = query.Where(b => b.Nombre.Contains(textoBuscar));
            }
            return query
                .Count(c => string
                    .Compare(c.Nombre, nombre) <= 0);
        }

        public bool ExisteCaja(Caja Caja)
        {
            return _context.Cajas.Any(b => b.Nombre == Caja.Nombre
                && b.ProductoId != Caja.ProductoId);
        }

        public override Caja? ObtenerPorId(int ProductoId)
        {
            return _context.Cajas
                .Include(c=>c.Detalles)
                .ThenInclude(d=>d.Bombon)
                .FirstOrDefault(b => b.ProductoId == ProductoId);
        }


        public override List<Caja> ObtenerTodos()
        {
            return _context.Cajas
                .OrderBy(c => c.Nombre).ToList();
        }
    }
}
