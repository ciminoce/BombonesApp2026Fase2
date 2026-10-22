using BombonesApp2026.Servicios.DTOs.Caja;
using BombonesApp2026.Servicios.DTOs.DetalleCaja;
using BombonesApp2026.Servicios.Interfaces;
using BombonesApp2026.Windows.Classes;
using BombonesApp2026.Windows.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;

namespace BombonesApp2026.Windows
{
    public partial class frmCajaAe : Form
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IBombonServicio _bombonServicio;
        private ServicioMensajes _servicioMensajes = new();
        private BindingSource _bindingSource = new BindingSource();
        private BindingList<DetalleCajaDto> _listaDetalles = new BindingList<DetalleCajaDto>();
        private CajaEditDto? _cajaDto;
        private EditorCaja _editorCaja = null!;
        public frmCajaAe(IServiceProvider serviceProvider,
            IBombonServicio bombonServicio)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            _bombonServicio = bombonServicio;
            _bindingSource.DataSource = _listaDetalles;
            dgvDatos.DataSource = _bindingSource;
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (_cajaDto is null)
            {
                InicializarEditor();
                chkActivo.Checked = true;
                chkActivo.Enabled = false;
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
                _cajaDto.Detalles = _editorCaja.ObtenerDetalles();
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
            if (dgvDatos.Rows.Count == 0)
            {
                valido = false;
                errorProvider1.SetError(txtCantidadBombones, "La caja debe tener al menos 1 detalle");
            }
            return valido;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnAgregarBombon_Click(object sender, EventArgs e)
        {
            using (frmManejarBombonCajaAe frm = _serviceProvider.GetRequiredService<frmManejarBombonCajaAe>())
            {
                frm.Text = "Nueve Detalle de Caja";
                if (frm.ShowDialog() == DialogResult.Cancel) return;
                var (bombonDto, cantidad) = frm.GetDatos();
                if (bombonDto is null) return;

                try
                {
                    _editorCaja.AgregarBombon(bombonDto, cantidad);
                    MostrarDatos(_editorCaja.ObtenerResumen());

                }
                catch (Exception ex)
                {

                    _servicioMensajes.Error($"No se pudo agregar el bombón {ex.Message}");
                }

            }

        }

        private void MostrarDatos((IReadOnlyCollection<DetalleCajaDto> Detalles,
            int Cantidad, decimal Precio, bool EsSurtida) resumen)
        {
            _listaDetalles.Clear();
            foreach (var item in resumen.Detalles)
            {
                _listaDetalles.Add(item);
            }
            txtCantidadBombones.Text = resumen.Cantidad.ToString();
            txtPrecio.Text = resumen.Precio.ToString();
            txtSurtida.Text = resumen.EsSurtida ? "Sí" : "No";
        }

        private void btnBorrarBombon_Click(object sender, EventArgs e)
        {
            DetalleCajaDto? detalleDto = _bindingSource
                .ObtenerSeleccionado<DetalleCajaDto>(_servicioMensajes);
            if (detalleDto is null) return;
            if (!_servicioMensajes.Confirmar($"¿Desea borrar el detalle" +
                $" del bombón {detalleDto.NombreBombon}?")) return;
            try
            {
                _editorCaja.EliminarBombon(detalleDto.BombonId);
                MostrarDatos(_editorCaja.ObtenerResumen());
            }
            catch (Exception ex)
            {

                _servicioMensajes.Error(ex.Message);
            }
        }

        private void btnEditarBombon_Click(object sender, EventArgs e)
        {
            DetalleCajaDto? detalleDto = _bindingSource
                .ObtenerSeleccionado<DetalleCajaDto>(_servicioMensajes);
            if (detalleDto is null) return;
            using (frmManejarBombonCajaAe frm=_serviceProvider
                .GetRequiredService<frmManejarBombonCajaAe>())
            {
                try
                {
                    frm.Text = "Modificar Cantidad del Bombón";
                    var bombonDto = _bombonServicio.ObtenerPorId(detalleDto.BombonId);
                    if (bombonDto is null) return;
                    frm.SetDatos(bombonDto, detalleDto.Cantidad);
                    if (frm.ShowDialog() == DialogResult.Cancel) return;
                    var (bombon, cantidad) = frm.GetDatos();
                    if (bombon is null) return;
                    try
                    {
                        _editorCaja.ModificarCantidadBombon(bombon, cantidad);
                        MostrarDatos(_editorCaja.ObtenerResumen());
                    }
                    catch (Exception ex)
                    {

                        _servicioMensajes.Error(ex.Message);
                    }
                }
                catch (Exception ex)
                {

                    _servicioMensajes.Error(ex.Message);
                }                
            }
        }
    }
}
