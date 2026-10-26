using Bombones2026.Servicios.DTOs.Paginacion;
using BombonesApp2026.Datos;
using BombonesApp2026.Datos.Interfaces;
using BombonesApp2026.Entidades.Entidades;
using BombonesApp2026.Servicios.DTOs.Caja;
using BombonesApp2026.Servicios.DTOs.DetalleCaja;
using BombonesApp2026.Servicios.Excepciones;
using BombonesApp2026.Servicios.Interfaces;
using BombonesApp2026.Servicios.Mapeadores;

namespace BombonesApp2026.Servicios.Servicios
{
    public class CajaServicio : ICajaServicio
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICajaRepositorio _cajaRepositorio;
        private readonly IBombonRepositorio _bombonRepositorio;

        public CajaServicio(IUnitOfWork unitOfWork,
            ICajaRepositorio cajaRepositorio,
            IBombonRepositorio bombonRepositorio)
        {
            _unitOfWork = unitOfWork;
            _cajaRepositorio = cajaRepositorio;
            _bombonRepositorio=bombonRepositorio;
        }

        public int Agregar(CajaCreateDto cajaDto)
        {
            var errores = new List<string>();
            errores.AddRange(ValidarCaja(cajaDto));
            errores.AddRange(ValidarDetalles(cajaDto.Detalles));
            errores.AddRange(ValidarBombones(cajaDto.Detalles));
            if (errores.Any())
            {
                throw new ValidarCajaExcepcion(errores);
            }
            Caja caja = new Caja
            {
                Nombre = cajaDto.Nombre,
                Descripcion = cajaDto.Descripcion,
                Stock = cajaDto.Stock,
                Activo = true
            };
            try
            {
                foreach (var item in cajaDto.Detalles)
                {
                    var bombon = _bombonRepositorio.ObtenerPorId(item.BombonId);
                    if (bombon is null) throw new ArgumentNullException("El bombón no existe");
                    caja.AgregarBombon(bombon, item.Cantidad);
                }
            }
            catch (Exception ex)
            {

                throw new ArgumentException(ex.Message);
            }
            if (_cajaRepositorio.ExisteCaja(caja))
            {
                throw new ArgumentException($"Ya existe un caja con el nombre {caja.Nombre}");
            }
            try
            {
                _cajaRepositorio.Agregar(caja);
                _unitOfWork.Guardar();
                return caja.ProductoId;
            }
            catch (Exception ex)
            {

                throw new Exception($"Error al intentar guardar un caja {ex.Message}");
            }
        }
        private List<string> ValidarCaja(CajaCreateDto cajaDto)
        {
            var errores = new List<string>();
            if (string.IsNullOrEmpty(cajaDto.Nombre))
            {
                errores.Add("El nombre de la caja es requerido");
            }else if (cajaDto.Nombre.Length > 100)
            {
                errores.Add("El nombre de la caja no puede superar los 100 caracteres");
            }
            if(!string.IsNullOrEmpty(cajaDto.Descripcion)&&
                cajaDto.Descripcion.Length > 250)
            {
                errores.Add("La descripción no puede superar los 250 caracteres");
            }
            if (cajaDto.Stock <= 0)
            {
                errores.Add("El stock debe ser positivo");
            }
            if (cajaDto.Detalles.Count == 0)
            {
                errores.Add("La caja no tiene detalles");
            }
            return errores;
        }
        private List<string> ValidarDetalles(List<DetalleCajaDto> detalles)
        {
            var errores = new List<string>();
            foreach (var item in detalles)
            {
                if (item.Cantidad <= 0)
                {
                    errores.Add($"La cantidad del bombon ID {item.BombonId}" +
                        $" no es válida {item.Cantidad}");
                }
            }
            return errores;
        }
        private List<string> ValidarBombones(List<DetalleCajaDto> detalles)
        {
            var errores = new List<string>();
            foreach (var item in detalles)
            {
                var bombon = _bombonRepositorio.ObtenerPorId(item.BombonId);
                if(bombon is null)
                {
                    errores.Add($"El bombón ID {item.BombonId} no existe");
                }else if (!bombon.Activo)
                {
                    errores.Add($"El bombón ID {item.BombonId} no está activo");
                }
                
            }
            return errores;
        }
        public CajaEditDto? ObtenerParaEditar(int cajaId)
        {
            // AJUSTE: Validación defensiva del ID antes de operar
            if (cajaId <= 0)
                throw new ArgumentException("El ID caja debe ser entero mayor a cero.", nameof(cajaId));


            Caja? caja = _cajaRepositorio.ObtenerPorId(cajaId);
            if (caja is null) throw new ArgumentException(nameof(cajaId), $"Id {cajaId} no encontrado");
            CajaEditDto cajaDto = caja.ToEditDto();
            return cajaDto;
        }

        public void Editar(CajaEditDto cajaDto)
        {
            try
            {
                var errores = new List<string>();
                errores.AddRange(ValidarCajaEditDto(cajaDto));
                errores.AddRange(ValidarDetalles(cajaDto.Detalles));
                errores.AddRange(ValidarBombones(cajaDto.Detalles));
                if (errores.Any())
                {
                    throw new ValidarCajaExcepcion(errores);
                }
                Caja? caja = _cajaRepositorio
                    .ObtenerCajaConDetalles(cajaDto.ProductoId);
                if (caja is null)
                {
                    throw new ArgumentException(nameof(cajaDto.ProductoId),
                        $"La caja ID {cajaDto.ProductoId} no existe");
                }
                caja.Nombre = cajaDto.Nombre;
                caja.Descripcion = cajaDto.Descripcion;
                caja.Stock = cajaDto.Stock;
                caja.Activo = cajaDto.Activo;

                if (_cajaRepositorio.ExisteCaja(caja))
                    throw new InvalidOperationException($"Ya existe una caja" +
                        $" {caja.Nombre}");
                ActualizarDetalles(caja, cajaDto.Detalles);

                _unitOfWork.Guardar();


            }
            catch (Exception ex)
            {

                throw new Exception($"Error al intentar guardar la caja {ex.Message}");
            }
        }

        private IEnumerable<string> ValidarCajaEditDto(CajaEditDto cajaDto)
        {
            var errores = new List<string>();
            if (string.IsNullOrEmpty(cajaDto.Nombre))
            {
                errores.Add("El nombre de la caja es requerido");
            }
            else if (cajaDto.Nombre.Length > 100)
            {
                errores.Add("El nombre de la caja no puede superar los 100 caracteres");
            }
            if (!string.IsNullOrEmpty(cajaDto.Descripcion) &&
                cajaDto.Descripcion.Length > 250)
            {
                errores.Add("La descripción no puede superar los 250 caracteres");
            }
            if (cajaDto.Stock <= 0)
            {
                errores.Add("El stock debe ser positivo");
            }
            if (cajaDto.Detalles.Count == 0)
            {
                errores.Add("La caja no tiene detalles");
            }
            return errores;

        }

        private void ActualizarDetalles(Caja caja, 
            List<DetalleCajaDto> detalleDto)
        {
            //1.Borrar detalles que no están
            foreach (var itemEnCaja in caja.Detalles.ToList())
            {
                var existe = detalleDto
                    .Any(d => d.BombonId == itemEnCaja.BombonId);
                if (!existe)
                {
                    caja.EliminarBombon(itemEnCaja.BombonId);
                    _cajaRepositorio.EliminarDetalle(itemEnCaja);
                }
            }
            //2.Modificar lo existente
            foreach (var itemDetalleDto in detalleDto)
            {
                var detalleEnCaja = caja.Detalles
                    .FirstOrDefault(d => d.BombonId == itemDetalleDto.BombonId);
                if (detalleEnCaja is null) continue;
                detalleEnCaja.Cantidad = itemDetalleDto.Cantidad;
            }
            //3.Agregar lo nuevo
            foreach (var itemDetalleDto in detalleDto)
            {
                var detalleEnCaja = caja.Detalles
                    .FirstOrDefault(d => d.BombonId == itemDetalleDto.BombonId);
                if (detalleEnCaja is not null) continue;
                var bombon = _bombonRepositorio.ObtenerPorId(itemDetalleDto.BombonId);
                if (bombon is null)
                    throw new ArgumentNullException(
                        $"Bombon ID {itemDetalleDto.BombonId} no encontrado");
                caja.AgregarBombon(bombon, itemDetalleDto.Cantidad);

            }
        }

        public int ObtenerPaginaRegistro(string nombre, int cantidadPorPagina,
            bool? filtroActivo = null, string? textoBuscar = null)
        {
            int posicion = _cajaRepositorio
                .ObtenerPosicionAlfabetica(nombre, filtroActivo, textoBuscar);
            return (int)Math.Ceiling((double)posicion / cantidadPorPagina);
        }
        public ResultadoPaginacionDto<CajaListDto> ObtenerPagina(int paginaActual,
                    int cantidadPorPagina, bool? filtroActivo = null, string? textoBuscar = null)
        {
            try
            {
                var resultado = _cajaRepositorio.ObtenerPagina(paginaActual,
                    cantidadPorPagina, filtroActivo, textoBuscar);
                var listaDto = resultado.lista
                        .Select(b => b.ToListDto()).ToList();
                return new ResultadoPaginacionDto<CajaListDto>
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



        public List<CajaListDto> ObtenerTodas()
        {
            var listaCajas = _cajaRepositorio.ObtenerTodos()
                .Select(c => c.ToListDto()).ToList();
            return listaCajas;
        }

        public void Borrar(int productoId)
        {
            if (productoId <= 0)
            {
                throw new ArgumentException(nameof(productoId),
                    "El ID debe ser positivo");
            }
            var caja = _cajaRepositorio.ObtenerCajaConDetalles(productoId);
            if (caja is null)
            {
                throw new KeyNotFoundException($"No se encontró una caja con el ID {productoId}");
            }
            //OJO falta ver si el registro está relacionado
            try
            {
                _cajaRepositorio.Borrar(productoId);
                _unitOfWork.Guardar();
            }
            catch (Exception ex)
            {

                throw new Exception($"Error al intentar borrar una caja: {ex.Message}");
            }
        }

        public CajaDetailDto? ObtenerCajaConDetalles(int productoId)
        {
            if (productoId <= 0)
            {
                throw new ArgumentException(nameof(productoId),
                    "El ID debe ser positivo");
            }
            var caja = _cajaRepositorio.ObtenerPorId(productoId);
            if (caja is null)
            {
                throw new KeyNotFoundException($"No se encontró una caja con el ID {productoId}");
            }
            return caja.ToDetailDto();
        }

        public CajaEditDto? ObtenerCajaConDetallesParaEditar(int cajaId)
        {
            // AJUSTE: Validación defensiva del ID antes de operar
            if (cajaId <= 0)
                throw new ArgumentException("El ID caja debe ser entero mayor a cero.", nameof(cajaId));


            try
            {
                Caja? caja = _cajaRepositorio.ObtenerCajaConDetalles(cajaId);
                if (caja is null) throw new ArgumentException(nameof(cajaId), $"Id {cajaId} no encontrado");
                CajaEditDto cajaDto = caja.ToEditDto();
                return cajaDto;

            }
            catch (Exception ex)
            {

                throw new Exception($"Error al intentar obtener la caja con sus detalles {ex.Message}");
            } 
        }
    }
}
