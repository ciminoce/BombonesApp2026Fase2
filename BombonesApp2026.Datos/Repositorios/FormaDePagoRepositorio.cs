using BombonesApp2026.Datos.Interfaces;
using BombonesApp2026.Entidades.Entidades;
using Microsoft.EntityFrameworkCore;

namespace BombonesApp2026.Datos.Repositorios
{
    public class FormaDePagoRepositorio : IFormaDePagoRepositorio
    {
        private readonly BombonesDbContext _context;
        public FormaDePagoRepositorio(BombonesDbContext context)
        {
            _context = context;
        }
        public List<FormaDePago> ObtenerTodos()
        {
            return _context.FormasDePago
                .AsNoTracking()
                .ToList();
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
        public void Agregar(FormaDePago formaDePago)
        {
            _context.FormasDePago.Add(formaDePago);

            _context.SaveChanges();

        }
        public void Editar(FormaDePago formaDePago)
        {
            var formaDePagoEnDb = _context.FormasDePago.Find(formaDePago.FormaDePagoId);

            if (formaDePagoEnDb is null) throw new Exception("Forma de pago no encontrada");
            formaDePagoEnDb.Nombre = formaDePago.Nombre;
            formaDePagoEnDb.Activo = formaDePago.Activo;

            _context.SaveChanges();

        }
        public void Borrar(int id)
        {
            var formaDePagoEnDb = _context.FormasDePago
                .Find(id);
            if (formaDePagoEnDb is null) throw new Exception("Forma de pago no encontrada");
            _context.FormasDePago.Remove(formaDePagoEnDb);
            _context.SaveChanges();
        }
        public FormaDePago? ObtenerPorId(int id)
        {
            return _context.FormasDePago.AsNoTracking()
                .FirstOrDefault(f => f.FormaDePagoId == id);
        }

        public bool ExisteFormaDePago(FormaDePago formaDePago)
        {
            if (formaDePago.FormaDePagoId == 0)
            {
                return _context.FormasDePago.Any(f => f.Nombre == formaDePago.Nombre);
            }
            else
            {
                return _context.FormasDePago.Any(f => f.Nombre == formaDePago.Nombre &&
                        f.FormaDePagoId != formaDePago.FormaDePagoId);
            }
        }

        public bool TieneRegistrosRelacionados(int formaDePagoId)
        {
            return false;
        }

    }
}
