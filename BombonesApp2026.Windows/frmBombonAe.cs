using Bombones2026.Servicios.DTOs.TipoBombon;
using Bombones2026.Servicios.Servicios;
using BombonesApp2026.Entidades.Enum;
using BombonesApp2026.Servicios.DTOs.Bombon;
using BombonesApp2026.Servicios.Interfaces;

namespace BombonesApp2026.Windows
{
    public partial class frmBombonAe : Form
    {
        private readonly ITipoBombonServicio _tipoServicio;
        private TipoBombonListDto? _tipoSeleccionado = null;
        private BombonEditDto? _bombonDto;
        public frmBombonAe(ITipoBombonServicio tipoServicio)
        {
            InitializeComponent();
            _tipoServicio = tipoServicio;
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            CargarDatosComboTipoBombones(cboTipoBombon);
            if (_bombonDto is null)
            {
                chkActivo.Checked = true;
                chkActivo.Enabled = false;
            }
            else
            {
                txtNombreBombon.Text = _bombonDto.Nombre;
                cboTipoBombon.SelectedValue = _bombonDto.TipoBombonId;
                txtDescripcion.Text = _bombonDto.Descripcion;
                txtPrecio.Text = _bombonDto.Precio.ToString();
                nudPesoEnGramos.Value = _bombonDto.PesoEnGramos;
                nudStock.Value = _bombonDto.Stock;
                chkTieneAzucar.Checked = _bombonDto.TieneAzucar;
                chkActivo.Checked=_bombonDto.Activo;
            }
        }

        private void CargarDatosComboTipoBombones(ComboBox cboTipoBombon)
        {
            var listaTipos = _tipoServicio
                .ObtenerDatosCombo(TipoBombonDefault.Seleccione);
            cboTipoBombon.DataSource = listaTipos;
            cboTipoBombon.DisplayMember = "Nombre";
            cboTipoBombon.ValueMember = "TipoBombonId";
            cboTipoBombon.SelectedIndex = 0;
        }

        public BombonEditDto? GetBombon()
        {
            return _bombonDto;
        }

        public void SetBombon(BombonEditDto bombonEditDto)
        {
            _bombonDto = bombonEditDto;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void frmBombonAe_Load(object sender, EventArgs e)
        {

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ValidarDatos())
            {
                if (_bombonDto is null)
                {
                    _bombonDto = new BombonEditDto();
                }
                _bombonDto.Nombre = txtNombreBombon.Text;
                // _bombonDto.TipoBombonId = ((TipoBombonListDto)cboTipoBombon.SelectedItem!).TipoBombonId;
                _bombonDto.TipoBombonId = _tipoSeleccionado!.TipoBombonId;
                _bombonDto.Descripcion = txtDescripcion.Text;
                _bombonDto.Precio = decimal.Parse(txtPrecio.Text);
                _bombonDto.Stock = (int)nudStock.Value;
                _bombonDto.PesoEnGramos = (int)nudPesoEnGramos.Value;
                _bombonDto.TieneAzucar = chkTieneAzucar.Checked;
                _bombonDto.Activo = chkActivo.Checked;

                DialogResult = DialogResult.OK;
            }
        }

        private bool ValidarDatos()
        {
            bool valido = true;
            errorProvider1.Clear();
            if (string.IsNullOrWhiteSpace(txtNombreBombon.Text))
            {
                valido = false;
                errorProvider1.SetError(txtNombreBombon, "El nombre es requerido");
            }
            else if (txtNombreBombon.Text.Length > 100)
            {
                valido = false;
                errorProvider1.SetError(txtNombreBombon, "El nombre debe tener no más de 100 caracteres");
            }
            if (txtDescripcion.Text.Length > 250)
            {
                valido = false;
                errorProvider1.SetError(txtDescripcion, "La descripción no puede tener más de 250 caracteres");
            }
            if (cboTipoBombon.SelectedIndex == 0)
            {
                valido = false;
                errorProvider1.SetError(cboTipoBombon, "Debe seleccionar un tipo de bombón");
            }
            if (!decimal.TryParse(txtPrecio.Text, out decimal precio) || precio <= 0)
            {
                valido = false;
                errorProvider1.SetError(txtPrecio, "Precio no válido o fuera de rango");
            }
            return valido;
        }

        private void cboTipoBombon_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTipoBombon.SelectedIndex == 0)
            {
                _tipoSeleccionado = null;
            }
            _tipoSeleccionado = (TipoBombonListDto)cboTipoBombon.SelectedItem!;
        }
    }
}
