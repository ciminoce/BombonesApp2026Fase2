using Bombones2026.Servicios.DTOs.Provincia;

namespace BombonesApp2026.Windows
{
    public partial class frmProvinciaAe : Form
    {
        private ProvinciaEditDto? _provinciaEditDto;
        public frmProvinciaAe()
        {
            InitializeComponent();
        }

        public ProvinciaEditDto? GetProvincia()
        {
            return _provinciaEditDto;
        }

        public void SetProvincia(ProvinciaEditDto provinciaEditDto)
        {
            _provinciaEditDto = provinciaEditDto;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ValidarDatos())
            {
                if (_provinciaEditDto is null)
                {
                    _provinciaEditDto = new ProvinciaEditDto();
                }
                _provinciaEditDto.Nombre = txtProvincia.Text;
                DialogResult = DialogResult.OK;
            }
        }

        private bool ValidarDatos()
        {
            bool valido = true;
            errorProvider1.Clear();
            if (string.IsNullOrEmpty(txtProvincia.Text))
            {
                valido = false;
                errorProvider1.SetError(txtProvincia, "La provincia es obligatoria");
            }
            return valido;
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (_provinciaEditDto is not null)
            {
                txtProvincia.Text = _provinciaEditDto.Nombre;
            }
        }

    }
}
