using Bombones2026.Servicios.DTOs.FormaDePago;
using Bombones2026.Servicios.DTOs.Paginacion;
using BombonesApp2026.Servicios.Interfaces;

namespace BombonesApp2026.Windows
{
    public partial class frmFormasDePago : Form
    {
        private readonly IFormaDePagoServicio _formaDePagoServicio;
        private BindingSource _bindingSource = new BindingSource();
        //para paginar
        private int paginaActual = 1;
        private int cantidadPorPagina = 10;
        private int totalRegistros = 0;
        private int totalPaginas = 0;

        private bool? filtroActivo = null;
        private string? textoBuscar = null;
        public frmFormasDePago(IFormaDePagoServicio formaDePagoServicio)
        {
            InitializeComponent();
            _formaDePagoServicio = formaDePagoServicio;
        }
        private void tsbCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }


        private void MostrarDatosEnGrilla(ResultadoPaginacionDto<FormaDePagoListDto> resultado)
        {
            totalPaginas = resultado.TotalPaginas;
            totalRegistros = resultado.TotalRegistros;
            int desde = 1 + (paginaActual - 1) * cantidadPorPagina;
            int hasta = desde + cantidadPorPagina;
            if (hasta > totalRegistros)
            {
                hasta = totalRegistros;
            }
            _bindingSource.DataSource = resultado.Items;
            dgvDatos.DataSource = _bindingSource;

            lblCantidad.Text = $"{desde} a {hasta} de {totalRegistros}";
            lblPaginas.Text = $"{paginaActual} de {totalPaginas}";

            btnPrimero.Enabled = resultado.TieneRegistrosAnteriores;
            btnAnterior.Enabled = resultado.TieneRegistrosAnteriores;
            btnSiguiente.Enabled = resultado.TieneRegistrosSiguientes;
            btnUltimo.Enabled = resultado.TieneRegistrosSiguientes;

        }

        private void tsbNuevo_Click(object sender, EventArgs e)
        {
            using (frmFormaDePagoAe frm = new frmFormaDePagoAe() { Text = "Nueva Forma de Pago" })
            {
                DialogResult dr = frm.ShowDialog();
                if (dr == DialogResult.Cancel) return;
                FormaDePagoEditDto? formaEditDto = frm.GetTipo();
                if (formaEditDto == null) return;
                try
                {
                    FormaDePagoCreateDto formaCreateDto = new FormaDePagoCreateDto
                    {
                        Nombre = formaEditDto.Nombre,
                    };
                    int nuevoId = _formaDePagoServicio.Agregar(formaCreateDto);
                    bool sePuedeVer = (filtroActivo is null || filtroActivo == true) &&
                        (string.IsNullOrWhiteSpace(txtBuscar.Text) ||
                        formaCreateDto.Nombre.ToLower().Contains(txtBuscar.Text.ToLower()));

                    if (sePuedeVer)
                    {
                        paginaActual = _formaDePagoServicio
                            .ObtenerPaginaRegistro(formaCreateDto.Nombre, cantidadPorPagina,
                            filtroActivo, textoBuscar);

                    }
                    RecargarGrilla();
                    if (sePuedeVer)
                    {
                        var nuevoTipo = _bindingSource.List
                            .Cast<FormaDePagoListDto>()
                            .FirstOrDefault(tb => tb.FormaDePagoId == nuevoId);
                        if (nuevoTipo is null) return;
                        _bindingSource.Position = _bindingSource.IndexOf(nuevoTipo);
                        MessageBox.Show("Forma de Pago Agregada",
                            "Mensaje", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

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
                    MessageBox.Show(ex.Message,
                                "Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                }
            }

        }

        private void tsbBorrar_Click(object sender, EventArgs e)
        {
            if (_bindingSource.Current == null)
            {
                MessageBox.Show("Debe seleccionar una fila de la grilla",
                    "Advertencia",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }
            FormaDePagoListDto formaDePagoDto = (FormaDePagoListDto)_bindingSource.Current!;
            DialogResult dr = MessageBox.Show($"¿Desea borrar la forma de pago {formaDePagoDto.Nombre}?",
                "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);
            if (dr == DialogResult.No) return;
            try
            {
                _formaDePagoServicio.Borrar(formaDePagoDto.FormaDePagoId);
                RecargarGrilla();
                if (paginaActual > totalPaginas && totalPaginas > 0)
                {
                    paginaActual = totalPaginas;
                    RecargarGrilla();
                }
                MessageBox.Show("Forma de Pago eliminada",
                    "Mensaje",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

            }

        }

        private void tsbEditar_Click(object sender, EventArgs e)
        {
            if (_bindingSource.Current == null)
            {
                MessageBox.Show("Debe seleccionar una fila de la grilla",
                    "Advertencia",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }
            FormaDePagoListDto formaDePagoDto = (FormaDePagoListDto)_bindingSource.Current!;
            FormaDePagoEditDto? formaDePagoEditDto = _formaDePagoServicio.ObtenerParaEditar(formaDePagoDto.FormaDePagoId);
            if (formaDePagoEditDto is null) return;
            using (frmFormaDePagoAe frm = new frmFormaDePagoAe() { Text = "Editar Forma de Pago " })
            {
                frm.SetFormaDePago(formaDePagoEditDto);
                DialogResult dr = frm.ShowDialog();
                if (dr == DialogResult.Cancel) return;
                formaDePagoEditDto = frm.GetFormaDePago();
                if (formaDePagoEditDto is null) return;
                try
                {
                    _formaDePagoServicio.Editar(formaDePagoEditDto);
                    int editadoId = formaDePagoEditDto.FormaDePagoId;
                    bool sePuedeVer = (filtroActivo is null || filtroActivo == true) &&
                        (string.IsNullOrWhiteSpace(txtBuscar.Text) ||
                        formaDePagoEditDto.Nombre.ToLower().Contains(txtBuscar.Text.ToLower()));


                    if (sePuedeVer)
                    {
                        paginaActual = _formaDePagoServicio.ObtenerPaginaRegistro(formaDePagoEditDto.Nombre,
                            cantidadPorPagina, filtroActivo, textoBuscar);
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

                    MessageBox.Show(ex.Message,
                         "Error",
                         MessageBoxButtons.OK,
                         MessageBoxIcon.Error);

                }
            }
        }

        private void activosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            paginaActual = 1;
            filtroActivo = true;
            tsbFiltrar.BackColor = Color.Orange;
            RecargarGrilla();
        }


        private void noActivosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            paginaActual = 1;
            filtroActivo = false;
            tsbFiltrar.BackColor = Color.Orange;
            RecargarGrilla();

        }

        private void tsbActualizar_Click(object sender, EventArgs e)
        {
            paginaActual = 1;
            filtroActivo = null;
            textoBuscar = null;
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
                var resultado = _formaDePagoServicio.ObtenerPagina(paginaActual,
                    cantidadPorPagina,
                    filtroActivo, textoBuscar);
                MostrarDatosEnGrilla(resultado);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void btnPrimero_Click(object sender, EventArgs e)
        {
            paginaActual = 1;
            RecargarGrilla();
        }

        private void btnAnterior_Click(object sender, EventArgs e)
        {
            paginaActual--;
            if (paginaActual == 0)
            {
                paginaActual = 1;

            }
            RecargarGrilla();
        }

        private void btnSiguiente_Click(object sender, EventArgs e)
        {
            paginaActual++;
            if (paginaActual > totalPaginas)
            {
                paginaActual = totalPaginas;
            }
            RecargarGrilla();
        }

        private void btnUltimo_Click(object sender, EventArgs e)
        {
            paginaActual = totalPaginas;
            RecargarGrilla();
        }


        private void tsbBuscar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBuscar.Text))
            {
                MessageBox.Show("Debe poner un texto para efectuar la búsqueda",
                    "Advertencia",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            textoBuscar = txtBuscar.Text;
            tsbBuscar.BackColor = Color.Orange;
            paginaActual = 1;
            RecargarGrilla();

        }
    }
}
