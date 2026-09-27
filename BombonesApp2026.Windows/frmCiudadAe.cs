using Bombones2026.Servicios.DTOs.Ciudad;
using Bombones2026.Servicios.Servicios;
using BombonesApp2026.Entidades.Enum;
using BombonesApp2026.Servicios.Interfaces;

namespace BombonesApp2026.Windows
{
    public partial class frmCiudadAe : Form
    {
        private readonly IProvinciaServicio _provinciaServicio;
        private CiudadEditDto? _ciudadDto;
        public frmCiudadAe(IProvinciaServicio servicio)
        {
            InitializeComponent();
            _provinciaServicio = servicio;
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            CargarComboProvincias(cboProvincias);
            if (_ciudadDto is not null)
            {
                txtCiudad.Text = _ciudadDto.Nombre;
                cboProvincias.SelectedValue = _ciudadDto.ProvinciaId;
            }
        }

        private void CargarComboProvincias(ComboBox cboProvincias)
        {
            var listaProvincias = _provinciaServicio
                .ObtenerDatosCombo(TipoProvinciaDefault.Seleccione);
            cboProvincias.DataSource = listaProvincias;
            cboProvincias.DisplayMember = "Nombre";
            cboProvincias.ValueMember = "ProvinciaId";
            cboProvincias.SelectedIndex = 0;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ValidarDatos())
            {
                if (_ciudadDto is null)
                {
                    _ciudadDto = new CiudadEditDto();
                }
                _ciudadDto.Nombre = txtCiudad.Text;
                _ciudadDto.ProvinciaId = (int)cboProvincias.SelectedValue!;
                DialogResult = DialogResult.OK;

            }
        }

        private bool ValidarDatos()
        {
            bool valido = true;
            errorProvider1.Clear();
            if (cboProvincias.SelectedIndex == 0)
            {
                valido = false;
                errorProvider1.SetError(cboProvincias, "Debe seleccionar una provincia");
            }
            if (string.IsNullOrWhiteSpace(txtCiudad.Text))
            {
                valido = false;
                errorProvider1.SetError(txtCiudad, "El nombre es requerido");
            }
            return valido;
        }

        public CiudadEditDto? GetCiudad()
        {
            return _ciudadDto;
        }

        public void SetCiudad(CiudadEditDto ciudadEditDto)
        {
            _ciudadDto = ciudadEditDto;
        }

    }
}
