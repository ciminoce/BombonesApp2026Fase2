using Bombones2026.Servicios.DTOs.Paginacion;
using BombonesApp2026.Datos;
using BombonesApp2026.Datos.Interfaces;
using BombonesApp2026.Entidades.Entidades;
using BombonesApp2026.Entidades.Enum;
using BombonesApp2026.Servicios.DTOs.Bombon;
using BombonesApp2026.Servicios.Interfaces;
using BombonesApp2026.Servicios.Mapeadores;

namespace BombonesApp2026.Servicios.Servicios
{
    public class BombonServicio : IBombonServicio
    {
        private readonly IBombonRepositorio _bombonRepositorio;
        private readonly IUnitOfWork _unitOfWork;
        public BombonServicio(IBombonRepositorio bombonRepositorio, IUnitOfWork unitOfWork)
        {
            _bombonRepositorio = bombonRepositorio ?? throw new ArgumentNullException(nameof(bombonRepositorio));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
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
            catch (Exception ex)
            {

                throw new Exception("Error al obtener la página de bombones.", ex);
            }
        }

        public List<BombonListDto> ObtenerTodos()
        {
            try
            {
                return _bombonRepositorio.ObtenerTodos()
            .Select(b => b.ToListDto()).ToList();

            }
            catch (Exception ex)
            {

                throw new Exception("Error al obtener el listado completo de bombones.", ex);
            }
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
                _unitOfWork.Guardar();
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
                _unitOfWork.Guardar();
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
            try
            {
                BombonEditDto bombonDto = bombon.ToEditDto();
                return bombonDto;

            }
            catch (Exception ex)
            {

                throw new Exception("Error al obtener el bombón para editar.", ex);
            }
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
            try
            {
                _bombonRepositorio.Editar(bombon, bombonDto.ProductoId);
                _unitOfWork.Guardar();

            }
            catch (Exception ex)
            {
                throw new Exception($"Error al intentar actualizar el bombón con ID {bombon.ProductoId}.", ex);
            }
        }

        public int ObtenerPaginaRegistro(string nombre, int cantidadPorPagina,
            bool? filtroActivo = null, string? textoBuscar = null)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre no puede estar vacío.", nameof(nombre));

            if (cantidadPorPagina <= 0) cantidadPorPagina = 10;

            int posicion = _bombonRepositorio
                .ObtenerPosicionAlfabetica(nombre, filtroActivo, textoBuscar);

            if (posicion <= 0) return 1;

            try
            {
                return (int)Math.Ceiling((double)posicion / cantidadPorPagina);

            }
            catch (Exception ex)
            {

                throw new Exception($"Error al obtener la página de registro para el bombón con nombre {nombre}: {ex.Message}", ex);
            }
        }

        public List<BombonListDto> ObtenerDatosCombo(BombonDefault bombonDefault)
        {
            try
            {
                var lista = _bombonRepositorio.ObtenerTodos()
                .Select(b => b.ToListDto()).ToList();
                if (bombonDefault == BombonDefault.Todos)
                {
                    var defaultBombon = new BombonListDto
                    {
                        ProductoId = 0,
                        Nombre = "Todos"
                    };
                    lista.Insert(0, defaultBombon);

                }
                else
                {
                    var defaultBombon = new BombonListDto
                    {
                        ProductoId = 0,
                        Nombre = "Seleccione"
                    };
                    lista.Insert(0, defaultBombon);
                }
                return lista;

            }
            catch (Exception ex)
            {
                throw new Exception($"Error al poblar los datos del combo de bombones: {ex.Message}", ex);
            }
        }
    }

}
