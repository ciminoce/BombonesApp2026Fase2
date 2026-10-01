using BombonesApp2026.Servicios.DTOs.Caja;
using BombonesApp2026.Servicios.DTOs.DetalleCaja;
using BombonesApp2026.Servicios.Interfaces;
using BombonesApp2026.Windows.Classes;
using System.ComponentModel;

namespace BombonesApp2026.Windows
{
    public partial class frmCajaAe : Form
    {
        private readonly IBombonServicio _bombonServicio;
        private BindingSource _bindingSource = new BindingSource();
        private BindingList<DetalleCajaListDto> _listaDetalles = new BindingList<DetalleCajaListDto>();
        private EditorCaja _editorCaja = null!;
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
            if(_cajaDto is null)
            {
                InicializarEditor();
            }
        }
        public CajaEditDto? GetCaja()
        {
            return _cajaDto;
        }

        public void SetCaja(CajaEditDto cajaEditDto)
        {
            _cajaDto = cajaEditDto;
            _editorCaja = new EditorCaja(_cajaDto);
        }
        public void InicializarEditor()
        {
            _cajaDto = new CajaEditDto();
            _editorCaja = new EditorCaja(_cajaDto);
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
            using (frmManejarBombonCajaAe frm = new frmManejarBombonCajaAe(_bombonServicio) { Text = "Nuevo Detalle" })
            {
                DialogResult dr = frm.ShowDialog();
                if (dr == DialogResult.Cancel) return;
                var (bombonDto, cantidad) = frm.GetDatos();
                if (bombonDto is null) return;

                try
                {
                    _editorCaja.AgregarBombon(bombonDto, cantidad);
                    MostrarDatos(_editorCaja.ObtenerResumen());

                }
                catch (Exception ex)
                {

                    MessageBox.Show($"No se pudo agregar el bombón {ex.Message}",
                        "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        private void MostrarDatos((IReadOnlyCollection<DetalleCajaListDto> Detalles,
            int Cantidad, decimal Precio, bool EsSurtida) resumen)
        {
            _listaDetalles.Clear();
            foreach (var item in resumen.Detalles)
            {
                _listaDetalles.Add(item);
            }
            txtCantidadBombones.Text = resumen.Cantidad.ToString();
            txtPrecio.Text = resumen.Precio.ToString();
            chkEsSurtida.Checked = resumen.EsSurtida;
        }
    }
}
