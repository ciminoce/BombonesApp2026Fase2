namespace BombonesApp2026.Windows.Helpers
{
    public class EstadoNavegacion
    {
        public int PaginaActual { get; set; } = 1;
        public int CantidadPorPagina { get; }
        public int TotalRegistros { get;  private set; }
        public int TotalPaginas { get; private set; }
        public bool? FiltroActivo { get; set; } = null;
        public string? TextoBuscar { get; set; } = null;
        public EstadoNavegacion(int cantidadPorPagina)
        {
            CantidadPorPagina=cantidadPorPagina;
        }
        public void Actualizar(int cantidadRegistros)
        {
            TotalRegistros = cantidadRegistros;
            if (TotalRegistros == 0)
            {
                PaginaActual = 1;
                TotalRegistros = 1;
                return;
            }
            TotalPaginas = (int) Math
                .Ceiling(TotalRegistros / (double)CantidadPorPagina);
            PaginaActual = Math.Clamp(PaginaActual, 1, TotalPaginas);

        }
        private (int desde, int hasta) CalcularRango()
        {
            int desde = 1 + (PaginaActual - 1) * CantidadPorPagina;
            int hasta =Math.Min(desde + CantidadPorPagina - 1,
                TotalRegistros);
            return (desde, hasta);
        }
        public string TextoRegistros()
        {
            var (desde, hasta) = CalcularRango();
            return $"Del {desde} al {hasta} de {TotalRegistros}";
        }
        public string TextoPaginas()
        {
            return $"{PaginaActual} de {TotalPaginas}";
        }
        public void IrPrimera()
        {
            PaginaActual = 1;
        }
        public void IrUltima()
        {
            PaginaActual = TotalPaginas;
        }
        public void IrAnterior()
        {
            PaginaActual--;
            if (PaginaActual < 1) PaginaActual = 1;
        }
        public void IrSiguiente()
        {
            PaginaActual++;
            if(PaginaActual>TotalPaginas)PaginaActual=TotalPaginas;
        }

        public void Inicializar()
        {
            PaginaActual = 1;
            FiltroActivo = null;
            TextoBuscar = null;
        }
    }
}
