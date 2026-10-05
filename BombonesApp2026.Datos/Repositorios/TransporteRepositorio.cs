using BombonesApp2026.Datos.Interfaces;
using BombonesApp2026.Entidades.Entidades;
using Microsoft.EntityFrameworkCore;

namespace BombonesApp2026.Datos.Repositorios
{
    public class TransporteRepositorio :Repositorio<Transporte>, ITransporteRepositorio
    {
        public TransporteRepositorio(BombonesDbContext context) : base(context)
        {
        }

        public (List<Transporte> lista, int cantidadRegistros) ObtenerPagina(int paginaActual,
            int cantidadPorPagina, bool? filtroActivo = null,
            int? provinciaIdFiltro = null,
            string? textoBuscar = null)
        {
            IQueryable<Transporte> query = _context
                .Transportes
                .Include(t => t.Provincia)
                .AsNoTracking();
            if (filtroActivo is not null)
            {
                query = query.Where(t => t.Activo == filtroActivo);
            }
            if (provinciaIdFiltro is not null)
            {
                query = query.Where(t => t.ProvinciaId == provinciaIdFiltro);
            }
            if (!string.IsNullOrWhiteSpace(textoBuscar))
            {
                query = query.Where(t => t.NombreEmpresa.Contains(textoBuscar));
            }
            var cantidad = query.Count();
            var lista = query
                .OrderBy(t => t.NombreEmpresa)
                .Skip(cantidadPorPagina * (paginaActual - 1))
                .Take(cantidadPorPagina)
                .ToList();
            return (lista, cantidad);
        }
        public int ObtenerPosicionAlfabetica(string nombre,
                bool? filtroActivo = null, int? provinciaIdFiltro = null, string? textoBuscar = null)
        {
            IQueryable<Transporte> query = _context.Transportes.AsNoTracking();
            if (filtroActivo.HasValue)
            {
                query = query.Where(t => t.Activo == filtroActivo.Value);
            }
            if (provinciaIdFiltro is not null)
            {
                query = query.Where(t => t.ProvinciaId == provinciaIdFiltro);
            }
            if (!string.IsNullOrWhiteSpace(textoBuscar))
            {
                query = query.Where(t => t.NombreEmpresa.Contains(textoBuscar));
            }
            return query
                .Count(t => string
                    .Compare(t.NombreEmpresa, nombre) <= 0);
        }

        public bool ExisteTransporte(Transporte transporte)
        {
            return _context.Transportes
                .Any(t => t.NombreEmpresa == transporte.NombreEmpresa &&
                t.ProvinciaId == transporte.ProvinciaId &&
                t.TransporteId != transporte.TransporteId);

        }

        public override Transporte? ObtenerPorId(int transporteId)
        {
            return _context.Transportes
                .Include(t=>t.Provincia)
                .AsNoTracking()
                .FirstOrDefault(t => t.TransporteId == transporteId);
        }

        public override List<Transporte> ObtenerTodos()
        {
            return _context.Transportes
                .Include(t => t.Provincia)
                .OrderBy(t => t.NombreEmpresa)
                .ToList();
        }

        public bool TieneRegistrosRelacionados(int transporteId)
        {
            return false;
        }
    }
}
