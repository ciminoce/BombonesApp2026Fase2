using BombonesApp2026.Servicios.DTOs.Caja;

namespace BombonesApp2026.Windows
{
    public partial class frmDetallesCaja : Form
    {
        private CajaDetailDto? _cajaDto;
        private BindingSource _bindingSource = new BindingSource();
        public frmDetallesCaja()
        {
            InitializeComponent();
            dgvDatos.DataSource = _bindingSource;
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (_cajaDto is null) return;
            txtNombreCaja.Text = _cajaDto.Nombre;
            txtDescripcion.Text = _cajaDto.Descripcion;
            txtPrecio.Text = _cajaDto.Precio.ToString();
            txtCantidadBombones.Text = _cajaDto.CantidadBombones.ToString();
            txtStock.Text = _cajaDto.Stock.ToString();
            chkActivo.Checked = _cajaDto.Activo;
            chkEsSurtida.Checked = _cajaDto.EsSurtida;
            _bindingSource.DataSource = _cajaDto.Detalles;
        }
        public void SetCaja(CajaDetailDto cajaDetailDto)
        {
            _cajaDto = cajaDetailDto;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
