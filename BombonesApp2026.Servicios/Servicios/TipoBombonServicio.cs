using Bombones2026.Servicios.DTOs.Paginacion;
using Bombones2026.Servicios.DTOs.TipoBombon;
using BombonesApp2026.Datos.Interfaces;
using BombonesApp2026.Entidades.Entidades;
using BombonesApp2026.Entidades.Enum;
using BombonesApp2026.Servicios.Interfaces;
using BombonesApp2026.Servicios.Mapeadores;

namespace Bombones2026.Servicios.Servicios
{
    public class TipoBombonServicio : ITipoBombonServicio
    {
        private readonly ITipoBombonRepositorio _tipoBombonRepositorio;
        public TipoBombonServicio(ITipoBombonRepositorio tipoBombonRepositorio)
        {
            _tipoBombonRepositorio = tipoBombonRepositorio;
        }
        public ResultadoPaginacionDto<TipoBombonListDto> ObtenerPagina(int paginaActual,
            int cantidadPorPagina, bool? filtroActivo = null, string? textoBuscar = null)
        {
            try
            {
                var resultado = _tipoBombonRepositorio.ObtenerPagina(paginaActual,
                    cantidadPorPagina, filtroActivo, textoBuscar);
                var listaDto = resultado.lista
                    .Select(tb => new TipoBombonListDto
                    {
                        TipoBombonId = tb.TipoBombonId,
                        Nombre = tb.Nombre,
                        Descripcion = tb.Descripcion,
                        Activo = tb.Activo
                    }).ToList();
                return new ResultadoPaginacionDto<TipoBombonListDto>
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
        public List<TipoBombonListDto> ObtenerTodos()
        {
            return _tipoBombonRepositorio.ObtenerTodos()
                .Select(tb => new TipoBombonListDto
                {
                    TipoBombonId = tb.TipoBombonId,
                    Nombre = tb.Nombre,
                    Descripcion = tb.Descripcion,
                    Activo = tb.Activo
                }).ToList();
        }

        public int Agregar(TipoBombonCreateDto? tipoDto)
        {

            if (tipoDto is null)
            {
                throw new ArgumentNullException(nameof(tipoDto), "El tipo de bombón no puede ser nulo");
            }
            if (string.IsNullOrWhiteSpace(tipoDto.Nombre))
            {
                throw new ArgumentNullException(nameof(tipoDto.Nombre), "El nombre es requerido");
            }
            TipoBombon tipo = new TipoBombon();
            tipo.Nombre = tipoDto.Nombre;
            tipo.Descripcion = tipoDto.Descripcion;
            tipo.Activo = true;
            if (_tipoBombonRepositorio.ExisteTipoBombon(tipo))
            {
                throw new InvalidOperationException($"Ya existe un tipo de bombón {tipo.Nombre}");
            }
            try
            {
                _tipoBombonRepositorio.Agregar(tipo);
                return tipo.TipoBombonId;
            }
            catch (Exception ex)
            {

                throw new Exception($"Error al intentar guardar un rol {ex.Message}");
            }
        }

        public void Borrar(int tipoBombonId)
        {
            // AJUSTE: Validación defensiva del ID antes de operar
            if (tipoBombonId <= 0)
                throw new ArgumentException("El ID del tipo de bombón debe ser un entero mayor a cero.", nameof(tipoBombonId));
            // 2. NUEVO AJUSTE: Verificar existencia real en el sistema antes que cualquier otra cosa
            // Nota: Asumiendo que tu repositorio tiene un método Existe(id) o similar
            var tipoBombonInDb = _tipoBombonRepositorio.ObtenerPorId(tipoBombonId);
            if (tipoBombonInDb is null)
            {
                throw new KeyNotFoundException($"No se puede borrar. No existe ningún tipo de bombón con el ID {tipoBombonId}.");
            }

            // AJUSTE: Se cambia Exception genérica por InvalidOperationException
            if (_tipoBombonRepositorio.TieneRegistrosRelacionados(tipoBombonId))
            {
                throw new InvalidOperationException($"No se puede eliminar el tipo de bombón (ID: {tipoBombonId}) porque tiene registros relacionados en el sistema.");
            }
            _tipoBombonRepositorio.Borrar(tipoBombonId);
        }

        public void Editar(TipoBombonEditDto? tipoDto)
        {
            if (tipoDto == null)
                throw new ArgumentNullException(nameof(tipoDto), "El tipo de bombón no puede ser nulo");
            if (string.IsNullOrWhiteSpace(tipoDto.Nombre))
                throw new ArgumentException(nameof(tipoDto.Nombre), "El nombre del tipo de bombón es requerido");
            TipoBombon tipo = new TipoBombon
            {
                TipoBombonId = tipoDto.TipoBombonId,
                Nombre = tipoDto.Nombre,
                Descripcion = tipoDto.Descripcion,
                Activo = tipoDto.Activo
            };
            if (_tipoBombonRepositorio.ExisteTipoBombon(tipo)) throw new InvalidOperationException($"Ya existe un tipo de bombón {tipo.Nombre}");
            _tipoBombonRepositorio.Editar(tipo);
        }
        public TipoBombonEditDto ObtenerParaEditar(int id)
        {
            // AJUSTE: Validación defensiva del ID antes de operar
            if (id <= 0)
                throw new ArgumentException("El ID del tipo de bombón debe ser un entero mayor a cero.", nameof(id));


            TipoBombon? tipo = _tipoBombonRepositorio.ObtenerPorId(id);
            if (tipo is null) throw new ArgumentException(nameof(id), $"Id {id} no encontrado");
            TipoBombonEditDto tipoDto = new TipoBombonEditDto
            {
                TipoBombonId = tipo.TipoBombonId,
                Nombre = tipo.Nombre,
                Descripcion = tipo.Descripcion,
                Activo = tipo.Activo
            };
            return tipoDto;
        }

        public int ObtenerPaginaRegistro(string nombre, int cantidadPorPagina,
            bool? filtroActivo = null, string? textoBuscar = null)
        {
            int posicion = _tipoBombonRepositorio
                .ObtenerPosicionAlfabetica(nombre, filtroActivo, textoBuscar);
            return (int)Math.Ceiling((double)posicion / cantidadPorPagina);
        }
        public List<TipoBombonListDto> ObtenerDatosCombo(TipoBombonDefault tipoDefault)
        {
            var lista = _tipoBombonRepositorio.ObtenerTodos()
                .Select(tp => tp.ToListDto()).ToList();
            if (tipoDefault == TipoBombonDefault.Todos)
            {
                var defaultTipo = new TipoBombonListDto
                {
                    TipoBombonId = 0,
                    Nombre = "Todos"
                };
                lista.Insert(0, defaultTipo);

            }
            else
            {
                var defaultTipo = new TipoBombonListDto
                {
                    TipoBombonId = 0,
                    Nombre = "Seleccione"
                };
                lista.Insert(0, defaultTipo);
            }
            return lista;
        }

    }
}
