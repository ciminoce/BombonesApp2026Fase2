using BombonesApp2026.Entidades.Enum;
using BombonesApp2026.Servicios.DTOs.Bombon;
using BombonesApp2026.Servicios.Interfaces;

namespace BombonesApp2026.Windows
{
    public partial class frmManejarBombonCajaAe : Form
    {
        private readonly IBombonServicio _bombonServicio;
        private BombonListDto _bombonSeleccionado = null!;
        private int _cantidadBombones;
        public frmManejarBombonCajaAe(IBombonServicio bombonServicio)
        {
            InitializeComponent();
            _bombonServicio = bombonServicio;
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            CargarDatosComboBombones(cboBombones);
        }

        private void CargarDatosComboBombones(ComboBox cboBombones)
        {
            var lista = _bombonServicio
                .ObtenerDatosCombo(BombonDefault.Seleccione);
            cboBombones.DataSource = lista;
            cboBombones.DisplayMember = "Nombre";
            cboBombones.ValueMember = "ProductoId";
            cboBombones.SelectedIndex = 0;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (ValidarDatos())
            {
                _cantidadBombones =(int) nudCantidad.Value;

                DialogResult=DialogResult.OK;
            }
        }
        public (BombonListDto bombonDto, int cantidad) GetDatos()
        {
            return (_bombonSeleccionado, _cantidadBombones);
        }
        private bool ValidarDatos()
        {
            bool valido = true;
            errorProvider1.Clear();
            if (cboBombones.SelectedIndex == 0)
            {
                valido = false;
                errorProvider1.SetError(cboBombones, "Debe seleccionar un bombón");
            }
            return valido;
        }

        private void cboBombones_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboBombones.SelectedIndex == 0) return;
            _bombonSeleccionado = (BombonListDto)cboBombones.SelectedItem!;
        }
    }
}
