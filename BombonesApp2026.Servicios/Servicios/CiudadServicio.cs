using Bombones2026.Servicios.DTOs.Ciudad;
using Bombones2026.Servicios.DTOs.Paginacion;
using BombonesApp2026.Datos.Interfaces;
using BombonesApp2026.Entidades.Entidades;
using BombonesApp2026.Entidades.Enum;
using BombonesApp2026.Servicios.Interfaces;

namespace Bombones2026.Servicios.Servicios
{
    public class CiudadServicio : ICiudadServicio
    {
        private readonly ICiudadRepositorio _ciudadRepositorio;
        public CiudadServicio(ICiudadRepositorio ciudadRepositorio)
        {
            _ciudadRepositorio = ciudadRepositorio;
        }

        public ResultadoPaginacionDto<CiudadListDto> ObtenerPagina(int paginaActual,
        int cantidadPorPagina, bool? filtroActivo = null, string? textoBuscar = null)
        {
            try
            {
                var resultado = _ciudadRepositorio.ObtenerPagina(paginaActual,
                    cantidadPorPagina, filtroActivo, textoBuscar);
                var listaDto = resultado.lista
                        .Select(c => new CiudadListDto
                        {
                            CiudadId = c.CiudadId,
                            Ciudad = c.Nombre,
                            Provincia = c.Provincia!.NombreProvincia
                        }).ToList();
                return new ResultadoPaginacionDto<CiudadListDto>
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

        public List<CiudadListDto> ObtenerTodos()
        {
            return _ciudadRepositorio.ObtenerTodos()
                .Select(c => new CiudadListDto
                {
                    CiudadId = c.CiudadId,
                    Ciudad = c.Nombre,
                    Provincia = c.Provincia!.NombreProvincia
                }).ToList();
        }
        public int Agregar(CiudadCreateDto ciudadDto)
        {
            Ciudad ciudad = new Ciudad
            {
                Nombre = ciudadDto.Nombre,
                ProvinciaId = ciudadDto.ProvinciaId,
            };
            if (_ciudadRepositorio.ExisteCiudad(ciudad))
            {
                throw new ArgumentException(nameof(ciudad), $"Ya existe una ciudad con el nombre {ciudad.Nombre}\n en esa provincia");
            }
            try
            {
                _ciudadRepositorio.Agregar(ciudad);
                return ciudad.CiudadId;
            }
            catch (Exception ex)
            {

                throw new Exception($"Error al intentar guardar una ciudad {ex.Message}");
            }
        }

        public void Borrar(int ciudadId)
        {
            if (ciudadId <= 0)
            {
                throw new ArgumentException(nameof(ciudadId),
                    "El ID debe ser positivo");
            }
            var ciudad = _ciudadRepositorio.ObtenerPorId(ciudadId);
            if (ciudad is null)
            {
                throw new KeyNotFoundException($"No se encontró una ciudad con el ID {ciudadId}");
            }
            //OJO falta ver si el registro está relacionado
            try
            {
                _ciudadRepositorio.Borrar(ciudadId);
            }
            catch (Exception ex)
            {

                throw new Exception($"Error al intentar borrar una ciudad: {ex.Message}");
            }
        }
        public CiudadEditDto? ObtenerParaEditar(int ciudadId)
        {
            // AJUSTE: Validación defensiva del ID antes de operar
            if (ciudadId <= 0)
                throw new ArgumentException("El ID ciudad debe ser entero mayor a cero.", nameof(ciudadId));


            Ciudad? ciudad = _ciudadRepositorio.ObtenerPorId(ciudadId);
            if (ciudad is null) throw new ArgumentException(nameof(ciudadId), $"Id {ciudadId} no encontrado");
            CiudadEditDto ciudadDto = new CiudadEditDto
            {
                CiudadId = ciudad.CiudadId,
                Nombre = ciudad.Nombre,
                ProvinciaId = ciudad.ProvinciaId
            };
            return ciudadDto;
        }

        public void Editar(CiudadEditDto ciudadDto)
        {
            if (ciudadDto == null)
                throw new ArgumentNullException(nameof(ciudadDto), "La forma de pago no puede ser nula");
            if (string.IsNullOrWhiteSpace(ciudadDto.Nombre))
                throw new ArgumentException(nameof(ciudadDto.Nombre), "El nombre de la forma de pago es requerido");
            if (ciudadDto.ProvinciaId == 0)
            {
                throw new ArgumentException(nameof(ciudadDto.ProvinciaId), "El ID de la provincia debe ser mayor a 0");
            }
            Ciudad ciudad = new Ciudad
            {
                CiudadId = ciudadDto.CiudadId,
                Nombre = ciudadDto.Nombre,
                ProvinciaId = ciudadDto.ProvinciaId
            };
            if (_ciudadRepositorio.ExisteCiudad(ciudad)) throw new InvalidOperationException($"Ya existe una ciudad {ciudad.Nombre}");
            _ciudadRepositorio.Editar(ciudad);

        }

        public int ObtenerPaginaRegistro(string nombre, int cantidadPorPagina,
            bool? filtroActivo = null, string? textoBuscar = null)
        {
            int posicion = _ciudadRepositorio
                .ObtenerPosicionAlfabetica(nombre, filtroActivo, textoBuscar);
            return (int)Math.Ceiling((double)posicion / cantidadPorPagina);
        }

        public List<CiudadListDto> ObtenerDatosCombo(TipoCiudadDefault tipoDefault, int provinciaId)
        {
            var lista = _ciudadRepositorio.ObtenerTodos(provinciaId)
                .Select(c => new CiudadListDto
                {
                    CiudadId = c.CiudadId,
                    Ciudad = c.Nombre,
                }).ToList();
            if (tipoDefault == TipoCiudadDefault.Todas)
            {
                var defaultCiudad = new CiudadListDto
                {
                    CiudadId = 0,
                    Ciudad = "Todas"
                };
                lista.Insert(0, defaultCiudad);

            }
            else
            {
                var defaultCiudad = new CiudadListDto
                {
                    CiudadId = 0,
                    Ciudad = "Seleccione"
                };
                lista.Insert(0, defaultCiudad);
            }
            return lista;
        }
    }
}
