namespace BombonesApp2026.Servicios.Excepciones
{
    public class ValidarCajaExcepcion : Exception
    {
        public List<string> Errores { get; }
        public ValidarCajaExcepcion(List<string> errores)
        {
            Errores = errores;
        }
    }
}
