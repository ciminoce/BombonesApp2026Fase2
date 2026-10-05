using BombonesApp2026.Datos.Interfaces;
using BombonesApp2026.Entidades.Entidades;
using Microsoft.EntityFrameworkCore;

namespace BombonesApp2026.Datos.Repositorios
{
    public class TipoBombonRepositorio :Repositorio<TipoBombon>, ITipoBombonRepositorio
    {
        public TipoBombonRepositorio(BombonesDbContext context) : base(context)
        {
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

        public bool ExisteTipoBombon(TipoBombon tipoBombon)
        { 
            return _context.TipoBombones.Any(tb => tb.Nombre == tipoBombon.Nombre &&
                    tb.TipoBombonId != tipoBombon.TipoBombonId);
        }

        public bool TieneRegistrosRelacionados(int tipoId)
        {
            return _context.Bombones.Any(b=>b.TipoBombonId == tipoId);
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
