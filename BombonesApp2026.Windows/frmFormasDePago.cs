using Bombones2026.Servicios.DTOs.FormaDePago;
using Bombones2026.Servicios.DTOs.Paginacion;
using BombonesApp2026.Servicios.Interfaces;
using BombonesApp2026.Servicios.Servicios;
using BombonesApp2026.Windows.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace BombonesApp2026.Windows
{
    public partial class frmFormasDePago : Form
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IFormaDePagoServicio _formaDePagoServicio;
        private BindingSource _bindingSource = new BindingSource();
        private ServicioMensajes _servicioMensajes = new();
        private EstadoNavegacion _estadoNavegacion = new(10);
        //para paginar
        public frmFormasDePago(IServiceProvider serviceProvider,
            IFormaDePagoServicio formaDePagoServicio)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            _formaDePagoServicio = formaDePagoServicio;
        }
        private void tsbCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }


        private void MostrarDatosEnGrilla(ResultadoPaginacionDto<FormaDePagoListDto> resultado)
        {

            _bindingSource.DataSource = resultado.Items;
            dgvDatos.DataSource = _bindingSource;

            lblCantidad.Text = _estadoNavegacion.TextoRegistros();
            lblPaginas.Text = _estadoNavegacion.TextoPaginas();

            btnPrimero.Enabled = resultado.TieneRegistrosAnteriores;
            btnAnterior.Enabled = resultado.TieneRegistrosAnteriores;
            btnSiguiente.Enabled = resultado.TieneRegistrosSiguientes;
            btnUltimo.Enabled = resultado.TieneRegistrosSiguientes;

        }

        private void tsbNuevo_Click(object sender, EventArgs e)
        {
            using (frmFormaDePagoAe frm =_serviceProvider.GetRequiredService<frmFormaDePagoAe>())
            {
                frm.Text = "Nueva Forma de Pago";
                if (frm.ShowDialog() == DialogResult.Cancel) return;
                FormaDePagoEditDto? formaEditDto = frm.GetTipo();
                if (formaEditDto == null) return;
                try
                {
                    FormaDePagoCreateDto formaCreateDto = new FormaDePagoCreateDto
                    {
                        Nombre = formaEditDto.Nombre,
                    };
                    int nuevoId = _formaDePagoServicio.Agregar(formaCreateDto);
                    bool sePuedeVer = (_estadoNavegacion.FiltroActivo is null || _estadoNavegacion.FiltroActivo == true) &&
                        (string.IsNullOrWhiteSpace(txtBuscar.Text) ||
                        formaCreateDto.Nombre.ToLower().Contains(txtBuscar.Text.ToLower()));

                    if (sePuedeVer)
                    {
                        _estadoNavegacion.PaginaActual = _formaDePagoServicio
                            .ObtenerPaginaRegistro(formaCreateDto.Nombre, _estadoNavegacion.CantidadPorPagina,
                            _estadoNavegacion.FiltroActivo, _estadoNavegacion.TextoBuscar);

                    }
                    RecargarGrilla();
                    if (sePuedeVer)
                    {
                        var nuevoTipo = _bindingSource.List
                            .Cast<FormaDePagoListDto>()
                            .FirstOrDefault(tb => tb.FormaDePagoId == nuevoId);
                        if (nuevoTipo is null) return;
                        _bindingSource.Position = _bindingSource.IndexOf(nuevoTipo);
                        _servicioMensajes.Informacion("Registro agregado");

                    }
                    else
                    {
                        MessageBox.Show($"Forma de Pago {formaEditDto.Nombre} agregada.\nNo se muestra por condición de filtrado o búsqueda",
                            "Confirmación", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    _servicioMensajes.Error(ex.Message);
                }
            }

        }

        private void tsbBorrar_Click(object sender, EventArgs e)
        {
            FormaDePagoListDto? formaDePagoDto = _bindingSource
                .ObtenerSeleccionado<FormaDePagoListDto>(_servicioMensajes);
            if(formaDePagoDto is null) return;
            DialogResult dr = MessageBox.Show($"¿Desea borrar la forma de pago {formaDePagoDto.Nombre}?",
                "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);
            if (dr == DialogResult.No) return;
            try
            {
                _formaDePagoServicio.Borrar(formaDePagoDto.FormaDePagoId);
                RecargarGrilla();
                _servicioMensajes.Informacion("Registro eliminado");
            }
            catch (Exception ex)
            {

                _servicioMensajes.Error(ex.Message);

            }

        }

        private void tsbEditar_Click(object sender, EventArgs e)
        {
            FormaDePagoListDto? formaDePagoDto = _bindingSource
                .ObtenerSeleccionado<FormaDePagoListDto>(_servicioMensajes);
            if (formaDePagoDto is null) return;
            FormaDePagoEditDto? formaDePagoEditDto = _formaDePagoServicio.ObtenerParaEditar(formaDePagoDto.FormaDePagoId);
            if (formaDePagoEditDto is null) return;
            using (frmFormaDePagoAe frm = _serviceProvider.GetRequiredService<frmFormaDePagoAe>())
            {
                frm.Text = "Nueva Forma de Pago";
                frm.SetFormaDePago(formaDePagoEditDto);
                if (frm.ShowDialog() == DialogResult.Cancel) return;
                formaDePagoEditDto = frm.GetFormaDePago();
                if (formaDePagoEditDto is null) return;
                try
                {
                    _formaDePagoServicio.Editar(formaDePagoEditDto);
                    int editadoId = formaDePagoEditDto.FormaDePagoId;
                    bool sePuedeVer = (_estadoNavegacion.FiltroActivo is null || _estadoNavegacion.FiltroActivo == true) &&
                        (string.IsNullOrWhiteSpace(txtBuscar.Text) ||
                        formaDePagoEditDto.Nombre.ToLower().Contains(txtBuscar.Text.ToLower()));


                    if (sePuedeVer)
                    {
                        _estadoNavegacion.PaginaActual = _formaDePagoServicio.ObtenerPaginaRegistro(formaDePagoEditDto.Nombre,
                            _estadoNavegacion.CantidadPorPagina, _estadoNavegacion.FiltroActivo, _estadoNavegacion.TextoBuscar);
                    }
                    RecargarGrilla();
                    if (sePuedeVer)
                    {

                        var editadoTipo = _bindingSource.List
                            .Cast<FormaDePagoListDto>()
                            .FirstOrDefault(tb => tb.FormaDePagoId == editadoId);

                        _bindingSource.Position = _bindingSource.IndexOf(editadoTipo);
                        MessageBox.Show("Forma de Pago editada",
                            "Mensaje",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show($"Forma de Pago {formaDePagoEditDto.Nombre} editada.\nNo se muestra por condición de filtrado o búsqueda",
                            "Confirmación",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                }
                catch (Exception ex)
                {

                    _servicioMensajes.Error(ex.Message);
                }
            }
        }

        private void activosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _estadoNavegacion.IrPrimera();
            _estadoNavegacion.FiltroActivo = true;
            tsbFiltrar.BackColor = Color.Orange;
            RecargarGrilla();
        }


        private void noActivosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _estadoNavegacion.IrPrimera();
            _estadoNavegacion.FiltroActivo = false;
            tsbFiltrar.BackColor = Color.Orange;
            RecargarGrilla();

        }

        private void tsbActualizar_Click(object sender, EventArgs e)
        {
            _estadoNavegacion.Inicializar();
            txtBuscar.Clear();
            tsbBuscar.BackColor = SystemColors.Control;
            tsbFiltrar.BackColor = SystemColors.Control;
            RecargarGrilla();
        }

        private void frmFormasDePago_Load(object sender, EventArgs e)
        {
            RecargarGrilla();
        }

        private void RecargarGrilla()
        {
            try
            {
                var paginaSolicitada = _estadoNavegacion.PaginaActual;
                var resultado = _formaDePagoServicio
                    .ObtenerPagina(paginaSolicitada,
                    _estadoNavegacion.CantidadPorPagina,
                    _estadoNavegacion.FiltroActivo, _estadoNavegacion.TextoBuscar);
                _estadoNavegacion.Actualizar(resultado.TotalRegistros);
                if (paginaSolicitada != _estadoNavegacion.PaginaActual)
                {
                    resultado = _formaDePagoServicio
                        .ObtenerPagina(_estadoNavegacion.PaginaActual,
                        _estadoNavegacion.CantidadPorPagina,
                        _estadoNavegacion.FiltroActivo, _estadoNavegacion.TextoBuscar);
                    _estadoNavegacion.Actualizar(resultado.TotalRegistros);
                }
                MostrarDatosEnGrilla(resultado);
            }
            catch (Exception ex)
            {

                _servicioMensajes.Error(ex.Message);
            }
        }

        private void btnPrimero_Click(object sender, EventArgs e)
        {
            _estadoNavegacion.IrPrimera();
            RecargarGrilla();
        }

        private void btnAnterior_Click(object sender, EventArgs e)
        {
            _estadoNavegacion.IrAnterior();
            RecargarGrilla();
        }

        private void btnSiguiente_Click(object sender, EventArgs e)
        {
            _estadoNavegacion.IrSiguiente();
            RecargarGrilla();
        }

        private void btnUltimo_Click(object sender, EventArgs e)
        {
            _estadoNavegacion.IrUltima();
            RecargarGrilla();
        }

        private void tsbBuscar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBuscar.Text))
            {
                _servicioMensajes.Advertencia("Debe poner un texto para efectuar la búsqueda");
                return;
            }
            _estadoNavegacion.TextoBuscar = txtBuscar.Text;
            tsbBuscar.BackColor = Color.Orange;
            _estadoNavegacion.IrPrimera();
            RecargarGrilla();

        }
    }
}
