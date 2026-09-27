namespace Bombones2026.Servicios.DTOs.Paginacion
{
    public class ResultadoPaginacionDto<T> where T : class
    {
        public List<T> Items { get; set; } = new List<T>();
        public int TotalRegistros { get; set; }
        public int CantidadPorPagina { get; set; }
        public int PaginaActual { get; set; }
        public int TotalPaginas => (int)Math.Ceiling((double)TotalRegistros / CantidadPorPagina);
        public bool TieneRegistrosAnteriores => PaginaActual > 1;
        public bool TieneRegistrosSiguientes => PaginaActual < TotalPaginas;
    }
}
