using Bombones2026.Servicios.DTOs.Paginacion;
using BombonesApp2026.Datos.Interfaces;
using BombonesApp2026.Entidades.Entidades;
using BombonesApp2026.Servicios.DTOs.Bombon;
using BombonesApp2026.Servicios.Interfaces;
using BombonesApp2026.Servicios.Mapeadores;

namespace BombonesApp2026.Servicios.Servicios
{
    public class BombonServicio : IBombonServicio
    {
        private readonly IBombonRepositorio _bombonRepositorio;
        public BombonServicio(IBombonRepositorio bombonRepositorio)
        {
            _bombonRepositorio = bombonRepositorio;
        }
        public ResultadoPaginacionDto<BombonListDto> ObtenerPagina(int paginaActual,
            int cantidadPorPagina, bool? filtroActivo = null, string? textoBuscar = null)
        {
            try
            {
                var resultado = _bombonRepositorio.ObtenerPagina(paginaActual,
                    cantidadPorPagina, filtroActivo, textoBuscar);
                var listaDto = resultado.lista
                        .Select(b => b.ToListDto()).ToList();
                return new ResultadoPaginacionDto<BombonListDto>
                {
                    Items = listaDto,
                    TotalRegistros = resultado.cantidadRegistros,
                    CantidadPorPagina = cantidadPorPagina,
                    PaginaActual = paginaActual
                };
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<BombonListDto> ObtenerTodos()
        {
            return _bombonRepositorio.ObtenerTodos()
                .Select(b => b.ToListDto()).ToList();
        }
        public int Agregar(BombonCreateDto bombonDto)
        {
            Bombon bombon = bombonDto.ToEntidad();
            if (_bombonRepositorio.ExisteBombon(bombon))
            {
                throw new ArgumentException(nameof(bombon), $"Ya existe un bombon con el nombre {bombon.Nombre}\n en esa provincia");
            }
            try
            {
                _bombonRepositorio.Agregar(bombon);
                return bombon.ProductoId;
            }
            catch (Exception ex)
            {

                throw new Exception($"Error al intentar guardar un bombon {ex.Message}");
            }
        }

        public void Borrar(int bombonId)
        {
            if (bombonId <= 0)
            {
                throw new ArgumentException(nameof(bombonId),
                    "El ID debe ser positivo");
            }
            var bombon = _bombonRepositorio.ObtenerPorId(bombonId);
            if (bombon is null)
            {
                throw new KeyNotFoundException($"No se encontró un bombon con el ID {bombonId}");
            }
            //OJO falta ver si el registro está relacionado
            try
            {
                _bombonRepositorio.Borrar(bombonId);
            }
            catch (Exception ex)
            {

                throw new Exception($"Error al intentar borrar un bombon: {ex.Message}");
            }
        }
        public BombonEditDto? ObtenerParaEditar(int bombonId)
        {
            // AJUSTE: Validación defensiva del ID antes de operar
            if (bombonId <= 0)
                throw new ArgumentException("El ID bombon debe ser entero mayor a cero.", nameof(bombonId));


            Bombon? bombon = _bombonRepositorio.ObtenerPorId(bombonId);
            if (bombon is null) throw new ArgumentException(nameof(bombonId), $"Id {bombonId} no encontrado");
            BombonEditDto bombonDto = bombon.ToEditDto();
            return bombonDto;
        }

        public void Editar(BombonEditDto bombonDto)
        {
            if (bombonDto == null)
                throw new ArgumentNullException(nameof(bombonDto), "La forma de pago no puede ser nula");
            if (bombonDto.ProductoId == 0)
            {
                throw new ArgumentException(nameof(bombonDto.ProductoId), "El ID del bombón debe ser mayor a 0");
            }
            Bombon bombon = bombonDto.ToEntidad();
            if (_bombonRepositorio.ExisteBombon(bombon)) throw new InvalidOperationException($"Ya existe una bombon {bombon.Nombre}");
            _bombonRepositorio.Editar(bombon);

        }

        public int ObtenerPaginaRegistro(string nombre, int cantidadPorPagina,
            bool? filtroActivo = null, string? textoBuscar = null)
        {
            int posicion = _bombonRepositorio
                .ObtenerPosicionAlfabetica(nombre, filtroActivo, textoBuscar);
            return (int)Math.Ceiling((double)posicion / cantidadPorPagina);
        }
    }

}
