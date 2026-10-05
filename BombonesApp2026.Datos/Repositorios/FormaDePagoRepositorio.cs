using BombonesApp2026.Datos.Interfaces;
using BombonesApp2026.Entidades.Entidades;
using Microsoft.EntityFrameworkCore;

namespace BombonesApp2026.Datos.Repositorios
{
    public class FormaDePagoRepositorio :Repositorio<FormaDePago>, IFormaDePagoRepositorio
    {
        public FormaDePagoRepositorio(BombonesDbContext context) : base(context)
        {
        }

        public (List<FormaDePago> lista, int cantidadRegistros) ObtenerPagina(int paginaActual,
                int cantidadPorPagina, bool? filtroActivo = null,
                string? textoBuscar = null)
        {
            IQueryable<FormaDePago> query = _context
                .FormasDePago.AsNoTracking();
            if (filtroActivo is not null)
            {
                query = query.Where(f => f.Activo == filtroActivo);
            }
            if (!string.IsNullOrWhiteSpace(textoBuscar))
            {
                query = query.Where(f => f.Nombre.Contains(textoBuscar));
            }
            var cantidad = query.Count();
            var lista = query
                .OrderBy(f => f.Nombre)
                .Skip(cantidadPorPagina * (paginaActual - 1))
                .Take(cantidadPorPagina)
                .ToList();
            return (lista, cantidad);
        }
        public int ObtenerPosicionAlfabetica(string nombre,
                bool? filtroActivo = null, string? textoBuscar = null)
        {
            IQueryable<FormaDePago> query = _context.FormasDePago.AsNoTracking();
            if (filtroActivo.HasValue)
            {
                query = query.Where(f => f.Activo == filtroActivo.Value);
            }
            if (!string.IsNullOrWhiteSpace(textoBuscar))
            {
                query = query.Where(f => f.Nombre.Contains(textoBuscar));
            }
            return query
                .Count(f => string
                    .Compare(f.Nombre, nombre) <= 0);
        }

        public List<FormaDePago> FiltrarPorActivo(bool activo)
        {
            return _context.FormasDePago
                .AsNoTracking()
                .Where(f => f.Activo == activo)
                .ToList();
        }

        public bool ExisteFormaDePago(FormaDePago formaDePago)
        {
            return _context.FormasDePago.Any(f => f.Nombre == formaDePago.Nombre &&
                    f.FormaDePagoId != formaDePago.FormaDePagoId);
        }

        public bool TieneRegistrosRelacionados(int formaDePagoId)
        {
            return false;
        }

    }
}
