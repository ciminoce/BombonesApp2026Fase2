using BombonesApp2026.Servicios.DTOs.Caja;

namespace BombonesApp2026.Windows
{
    public partial class frmCajaAe : Form
    {
        private CajaEditDto? _cajaDto;
        public frmCajaAe()
        {
            InitializeComponent();
        }

        public CajaEditDto? GetCaja()
        {
            return _cajaDto;
        }

        public void SetCaja(CajaEditDto cajaEditDto)
        {
            _cajaDto= cajaEditDto;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ValidarDatos())
            {
                if (_cajaDto is null)
                {
                    _cajaDto = new CajaEditDto();
                }
                _cajaDto.Nombre = txtNombreCaja.Text;
                _cajaDto.Descripcion = txtDescripcion.Text;
                _cajaDto.Stock = (int)nudStock.Value;
                _cajaDto.CantidadBombones = (int)nudCantidadBombones.Value;
                _cajaDto.Precio = decimal.Parse(txtPrecio.Text);
                _cajaDto.EsSurtida = chkEsSurtida.Checked;
                _cajaDto.Activo = chkActivo.Checked;

                DialogResult = DialogResult.OK;
            }
        }

        private bool ValidarDatos()
        {
            bool valido = true;
            errorProvider1.Clear();
            if (string.IsNullOrWhiteSpace(txtNombreCaja.Text)) {
                valido = false;
                errorProvider1.SetError(txtNombreCaja, "El nombre es requerido");
            }else if (txtNombreCaja.Text.Length > 50)
            {
                valido = false;
                errorProvider1.SetError(txtNombreCaja, "El nombre no puede tener más de 50 caracteres");
            }
            if (txtDescripcion.Text.Length>250)
            {
                valido = false;
                errorProvider1.SetError(txtDescripcion, "La descripción no puede tener más de 250 caracteres");
            }
            if(!decimal.TryParse(txtPrecio.Text, out decimal precio) || precio <= 0)
            {
                valido = false;
                errorProvider1.SetError(txtPrecio, "Precio no válido o fuera de rango");
            }
            return valido;
        }
    }
}
