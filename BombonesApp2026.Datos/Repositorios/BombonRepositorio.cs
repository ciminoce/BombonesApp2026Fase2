using BombonesApp2026.Datos.Interfaces;
using BombonesApp2026.Entidades.Entidades;
using Microsoft.EntityFrameworkCore;

namespace BombonesApp2026.Datos.Repositorios
{
    public class BombonRepositorio : IBombonRepositorio
    {
        private readonly BombonesDbContext _context;

        public BombonRepositorio(BombonesDbContext context)
        {
            _context = context;
        }
        public List<Bombon> ObtenerTodos()
        {
            return _context.Bombones
                .Include(b => b.TipoBombon)
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

        public void Agregar(Bombon Bombon)
        {
            _context.Bombones.Add(Bombon);
            _context.SaveChanges();
        }

        public bool ExisteBombon(Bombon Bombon)
        {
            return _context.Bombones.Any(b => b.Nombre == Bombon.Nombre
                && b.ProductoId != Bombon.ProductoId);
        }
        public void Borrar(int ProductoId)
        {
            var BombonEnDb = _context.Bombones.Find(ProductoId);
            if (BombonEnDb is null) throw new KeyNotFoundException($"No se encuentra un Bombon con ID: {ProductoId}");
            _context.Bombones.Remove(BombonEnDb);
            _context.SaveChanges();
        }

        public Bombon? ObtenerPorId(int ProductoId)
        {
            return _context.Bombones
                .FirstOrDefault(b => b.ProductoId == ProductoId);
        }

        public void Editar(Bombon bombon)
        {
            var bombonEnDb = _context.Bombones.Find(bombon.ProductoId);

            if (bombonEnDb is null) throw new Exception("Bombon no encontrado");
            bombonEnDb.Nombre = bombon.Nombre;
            bombonEnDb.TipoBombonId = bombon.TipoBombonId;
            bombonEnDb.Descripcion = bombon.Descripcion;
            bombonEnDb.Precio = bombon.Precio;
            bombonEnDb.Stock = bombon.Stock;
            bombonEnDb.Activo = bombon.Activo;
            bombonEnDb.TieneAzucar = bombon.TieneAzucar;
            bombonEnDb.PesoEnGramos = bombon.PesoEnGramos;


            _context.SaveChanges();

        }

    }
}
