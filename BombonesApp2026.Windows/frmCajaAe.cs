using BombonesApp2026.Servicios.DTOs.Caja;
using BombonesApp2026.Servicios.DTOs.DetalleCaja;
using BombonesApp2026.Servicios.Interfaces;
using System.ComponentModel;

namespace BombonesApp2026.Windows
{
    public partial class frmCajaAe : Form
    {
        private readonly IBombonServicio _bombonServicio;
        private BindingSource _bindingSource = new BindingSource();
        private BindingList<DetalleCajaListDto> _listaDetalles = new BindingList<DetalleCajaListDto>();
        private CajaEditDto? _cajaDto;
        public frmCajaAe(IBombonServicio bombonServicio)
        {
            InitializeComponent();
            _bombonServicio = bombonServicio;
            _bindingSource.DataSource = _listaDetalles;
            dgvDatos.DataSource = _bindingSource;
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
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
                _cajaDto.Activo = chkActivo.Checked;

                DialogResult = DialogResult.OK;
            }
        }

        private bool ValidarDatos()
        {
            bool valido = true;
            errorProvider1.Clear();
            if (string.IsNullOrWhiteSpace(txtNombreCaja.Text))
            {
                valido = false;
                errorProvider1.SetError(txtNombreCaja, "El nombre es requerido");
            }
            else if (txtNombreCaja.Text.Length > 50)
            {
                valido = false;
                errorProvider1.SetError(txtNombreCaja, "El nombre no puede tener más de 50 caracteres");
            }
            if (txtDescripcion.Text.Length > 250)
            {
                valido = false;
                errorProvider1.SetError(txtDescripcion, "La descripción no puede tener más de 250 caracteres");
            }
            if (!decimal.TryParse(txtPrecio.Text, out decimal precio) || precio <= 0)
            {
                valido = false;
                errorProvider1.SetError(txtPrecio, "Precio no válido o fuera de rango");
            }
            return valido;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnAgregarBombon_Click(object sender, EventArgs e)
        {
        }

    }
}
