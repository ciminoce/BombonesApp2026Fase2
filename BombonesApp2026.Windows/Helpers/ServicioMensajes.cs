namespace BombonesApp2026.Windows.Helpers
{
    public class ServicioMensajes
    {
        public void Informacion(string mensaje, string titulo = "Informar")
        {
            MessageBox.Show(mensaje, titulo, MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
        public void Advertencia(string mensaje, string titulo = "Advertencia")
        {
            MessageBox.Show(mensaje, titulo, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        public void Error(string mensaje, string titulo = "Error")
        {
            MessageBox.Show(mensaje, titulo, MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        public void Errores(IEnumerable<string> errores, string titulo = "Errores")
        {
            if (errores is null || !errores.Any()) return;
            string mensajeCompleto = string.Join(Environment.NewLine,
                errores);
            MessageBox.Show(mensajeCompleto, titulo, MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        public bool Confirmar(string mensaje, string titulo = "Confirmar")
        {
            DialogResult dr=MessageBox.Show(mensaje, titulo,
                MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);
            return dr==DialogResult.Yes;
        }
    }
}
