using BombonesApp2026.Datos;
using BombonesApp2026.Entidades.Entidades;
using CajaesApp2026.Datos.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CajaesApp2026.Datos.Repositorios
{
    public class CajaRepositorio : ICajaRepositorio
    {
        private readonly BombonesDbContext _context;

        public CajaRepositorio(BombonesDbContext context)
        {
            _context = context;
        }


        public (List<Caja> lista, int cantidadRegistros) ObtenerPagina(int paginaActual,
    int cantidadPorPagina, bool? filtroActivo = null,
    string? textoBuscar = null)
        {
            IQueryable<Caja> query = _context
                .Cajas
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

        public void Agregar(Caja Caja)
        {
            _context.Cajas.Add(Caja);
            _context.SaveChanges();
        }

        public bool ExisteCaja(Caja Caja)
        {
            return _context.Cajas.Any(b => b.Nombre == Caja.Nombre
                && b.ProductoId != Caja.ProductoId);
        }
        public void Borrar(int ProductoId)
        {
            var CajaEnDb = _context.Cajas.Find(ProductoId);
            if (CajaEnDb is null) throw new KeyNotFoundException($"No se encuentra un Caja con ID: {ProductoId}");
            _context.Cajas.Remove(CajaEnDb);
            _context.SaveChanges();
        }

        public Caja? ObtenerPorId(int ProductoId)
        {
            return _context.Cajas
                .FirstOrDefault(b => b.ProductoId == ProductoId);
        }

        public void Editar(Caja caja)
        {
            var cajaEnDb = _context.Cajas.Find(caja.ProductoId);

            if (cajaEnDb is null) throw new Exception("Caja no encontrado");
            cajaEnDb.Nombre = caja.Nombre;
            cajaEnDb.Descripcion = caja.Descripcion;
            cajaEnDb.Precio = caja.Precio;
            cajaEnDb.Stock = caja.Stock;
            cajaEnDb.Activo = caja.Activo;
            cajaEnDb.EsSurtida = caja.EsSurtida;
            cajaEnDb.CantidadBombones = caja.CantidadBombones;


            _context.SaveChanges();

        }

        public List<Caja> ObtenerTodos()
        {
            return _context.Cajas
                .OrderBy(c => c.Nombre).ToList();
        }
    }
}
