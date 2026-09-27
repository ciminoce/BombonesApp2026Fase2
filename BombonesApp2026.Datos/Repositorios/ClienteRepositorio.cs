using BombonesApp2026.Datos.Interfaces;
using BombonesApp2026.Entidades.Entidades;
using Microsoft.EntityFrameworkCore;

namespace BombonesApp2026.Datos.Repositorios
{
    public class ClienteRepositorio : IClienteRepositorio
    {
        private readonly BombonesDbContext _context;

        public ClienteRepositorio(BombonesDbContext context)
        {
            _context = context;
        }
        public List<Cliente> ObtenerTodos()
        {
            return _context.Clientes
                .Include(c => c.Ciudad)
                .ThenInclude(ci => ci.Provincia)
                .ToList();
        }
        public (List<Cliente> lista, int cantidadRegistros) ObtenerPagina(int paginaActual,
    int cantidadPorPagina, bool? filtroActivo = null,
    string? textoBuscar = null)
        {
            IQueryable<Cliente> query = _context
                .Clientes
                .Include(c => c.Ciudad)
                .ThenInclude(ci => ci.Provincia)
                .AsNoTracking();
            if (!string.IsNullOrWhiteSpace(textoBuscar))
            {
                query = query.Where(c => c.Nombre.Contains(textoBuscar));
            }
            var cantidad = query.Count();
            var lista = query
                .OrderBy(c => c.Apellido).ThenBy(c => c.Nombre)
                .Skip(cantidadPorPagina * (paginaActual - 1))
                .Take(cantidadPorPagina)
                .ToList();
            return (lista, cantidad);
        }
        public int ObtenerPosicionAlfabetica(string nombre,
                bool? filtroActivo = null, string? textoBuscar = null)
        {
            IQueryable<Cliente> query = _context.Clientes.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(textoBuscar))
            {
                query = query.Where(c => c.Nombre.Contains(textoBuscar));
            }
            return query
                .Count(c => string
                    .Compare(c.Nombre, nombre) <= 0);
        }

        public void Agregar(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
            _context.SaveChanges();
        }

        public bool ExisteBombon(Cliente cliente)
        {
            return _context.Clientes.Any(c => c.Documento == cliente.Documento
                && c.ClienteId != cliente.ClienteId);

        }
        public void Borrar(int clienteId)
        {
            var clienteEnDb = _context.Clientes.Find(clienteId);
            if (clienteEnDb is null) throw new KeyNotFoundException($"No se encuentra un Cliente con ID: {clienteId}");
            _context.Clientes.Remove(clienteEnDb);
            _context.SaveChanges();
        }

        public Cliente? ObtenerPorId(int clienteId)
        {
            return _context.Clientes
                .FirstOrDefault(c => c.ClienteId == clienteId);
        }

        public void Editar(Cliente cliente)
        {
            var clienteEnDb = _context.Clientes.Find(cliente.ClienteId);

            if (clienteEnDb is null) throw new Exception("Cliente no encontrado");
            clienteEnDb.Nombre = cliente.Nombre;
            clienteEnDb.Apellido = cliente.Apellido;
            clienteEnDb.Telefono = cliente.Telefono;
            clienteEnDb.Email = cliente.Email;
            clienteEnDb.Calle = cliente.Calle;
            clienteEnDb.Numero = cliente.Numero;
            clienteEnDb.CodigoPostal = cliente.CodigoPostal;
            clienteEnDb.CiudadId = cliente.CiudadId;
            clienteEnDb.Activo = cliente.Activo;



            _context.SaveChanges();

        }

        public bool ExisteCliente(Cliente cliente)
        {
            return _context.Clientes.Any(c => c.Documento == cliente.Documento);
        }
    }
}
