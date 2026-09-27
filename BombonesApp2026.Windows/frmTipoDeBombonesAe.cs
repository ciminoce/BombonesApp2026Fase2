using Bombones2026.Servicios.DTOs.TipoBombon;

namespace BombonesApp2026.Windows
{
    public partial class frmTipoDeBombonesAe : Form
    {
        private TipoBombonEditDto? tipoDto;
        public frmTipoDeBombonesAe()
        {
            InitializeComponent();
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (tipoDto is not null)
            {
                txtTipoBombon.Text = tipoDto.Nombre;
                txtDescripcion.Text = tipoDto.Descripcion;
                chkActivo.Checked = tipoDto.Activo;
            }
            else
            {
                chkActivo.Checked = true;
                chkActivo.Enabled = false;

            }
        }

        public TipoBombonEditDto? GetTipo()
        {
            return tipoDto;
        }
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ValidarDatos())
            {
                if (tipoDto is null)
                {
                    tipoDto = new TipoBombonEditDto();
                }
                tipoDto.Nombre = txtTipoBombon.Text;
                tipoDto.Descripcion = txtDescripcion.Text;
                tipoDto.Activo = chkActivo.Checked;
                DialogResult = DialogResult.OK;
            }
        }

        private bool ValidarDatos()
        {
            bool valido = true;
            errorProvider1.Clear();
            if (string.IsNullOrEmpty(txtTipoBombon.Text))
            {
                valido = false;
                errorProvider1.SetError(txtTipoBombon, "El tipo de bombón es obligatorio");
            }
            return valido;
        }


        public void SetTipo(TipoBombonEditDto tipoEditDto)
        {
            tipoDto = tipoEditDto;
        }

    }
}
