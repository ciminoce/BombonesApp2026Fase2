using Bombones2026.Servicios.DTOs.Ciudad;
using Bombones2026.Servicios.DTOs.Provincia;
using Bombones2026.Servicios.Servicios;
using BombonesApp2026.Entidades.Enum;
using BombonesApp2026.Servicios.DTOs.Cliente;
using BombonesApp2026.Servicios.Interfaces;

namespace BombonesApp2026.Windows
{
    public partial class frmClienteAe : Form
    {
        private ClienteEditDto? _clienteDto;
        private readonly IProvinciaServicio _provinciaServicio;
        private readonly ICiudadServicio _ciudadServicio;
        private ProvinciaListDto? _provinciaSeleccionada;
        public frmClienteAe(IProvinciaServicio provinciaServicio, ICiudadServicio ciudadServicio)
        {
            InitializeComponent();
            _provinciaServicio = provinciaServicio;
            _ciudadServicio = ciudadServicio;
        }


        public ClienteEditDto? GetCliente()
        {
            return _clienteDto;
        }

        public void SetCliente(ClienteEditDto clienteEditDto)
        {
            _clienteDto = clienteEditDto;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            CargarDatosComboProvincia(cboProvincia);
            if (_clienteDto is null)
            {
                chkActivo.Checked = true;
                chkActivo.Enabled = false;

            }
            else
            {
                CargarDatosComboCiudades(cboCiudad, _clienteDto.ProvinciaId);
                txtNombre.Text = _clienteDto.Nombre;
                txtApellido.Text = _clienteDto.Apellido;
                txtDocumento.Text = _clienteDto.Documento;
                txtEmail.Text = _clienteDto.Email;
                txtCalle.Text = _clienteDto.Calle;
                txtNumero.Text = _clienteDto.Numero;
                cboProvincia.SelectedValue = _clienteDto.ProvinciaId;
                cboCiudad.SelectedValue = _clienteDto.CiudadId;
                chkActivo.Checked = _clienteDto.Activo;
                chkActivo.Enabled = true;

            }

        }

        private void CargarDatosComboCiudades(ComboBox cboCiudad, int provinciaId)
        {
            var listaCiudades = _ciudadServicio.ObtenerDatosCombo(TipoCiudadDefault.Seleccione, provinciaId);
            cboCiudad.DataSource = listaCiudades;
            cboCiudad.DisplayMember = "Ciudad";
            cboCiudad.ValueMember = "CiudadId";
            cboCiudad.SelectedIndex = 0;
        }

        private void CargarDatosComboProvincia(ComboBox cboProvincia)
        {
            var listaProvincias = _provinciaServicio.ObtenerDatosCombo(TipoProvinciaDefault.Seleccione);
            cboProvincia.DataSource = listaProvincias;
            cboProvincia.DisplayMember = "Nombre";
            cboProvincia.ValueMember = "ProvinciaId";
            cboProvincia.SelectedIndex = 0;
        }

        private void cboProvincia_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboProvincia.SelectedIndex == 0)
            {
                cboCiudad.DataSource = null;
                return;
            }
            _provinciaSeleccionada = (ProvinciaListDto)cboProvincia.SelectedItem!;
            CargarDatosComboCiudades(cboCiudad, _provinciaSeleccionada.ProvinciaId);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ValidarDatos())
            {
                if(_clienteDto is null)
                {
                    _clienteDto = new ClienteEditDto();
                }
                _clienteDto.Nombre = txtNombre.Text;
                _clienteDto.Apellido = txtApellido.Text;
                _clienteDto.Documento = txtDocumento.Text;
                _clienteDto.Calle = txtCalle.Text;
                _clienteDto.Numero = txtNumero.Text;
                _clienteDto.CiudadId = ((CiudadListDto)cboCiudad.SelectedItem!).CiudadId;
                _clienteDto.CodigoPostal = txtCodPostal.Text;
                _clienteDto.Activo = chkActivo.Checked;
                _clienteDto.Telefono = txtTelefono.Text;
                _clienteDto.Email = txtEmail.Text;

                DialogResult = DialogResult.OK;
            }
        }

        private bool ValidarDatos()
        {
            return true;
        }
    }
}
