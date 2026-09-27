using Bombones2026.Servicios.DTOs.Paginacion;
using Bombones2026.Servicios.DTOs.Transporte;
using BombonesApp2026.Entidades.Enum;
using BombonesApp2026.Servicios.Interfaces;

namespace BombonesApp2026.Windows
{
    public partial class frmTransportes : Form
    {
        private readonly ITransporteServicio _transporteServicio;
        private readonly IProvinciaServicio _provinciaServicio;
        private BindingSource _bindingSource = new BindingSource();
        //para paginar
        private int paginaActual = 1;
        private int cantidadPorPagina = 10;
        private int totalRegistros = 0;
        private int totalPaginas = 0;
        //para filtrar
        private bool estaCargado = false;
        private bool? filtroActivo = null;
        private int? provinciaIdFiltro = null;
        private string? textoBuscar = null;
        public frmTransportes(ITransporteServicio servicio, IProvinciaServicio provinciaServicio)
        {
            InitializeComponent();
            _transporteServicio = servicio;
            _provinciaServicio = provinciaServicio;
        }

        private void tsbCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmTransportes_Load(object sender, EventArgs e)
        {
            estaCargado = false;
            CargarComboProvincias(tsCboProvincias.ComboBox);
            estaCargado = false;
            CargarComboEstados(tsCboActivo.ComboBox);
            RecargarGrilla();
        }

        private void CargarComboEstados(ComboBox comboBox)
        {
            var lista = Enum.GetValues(typeof(TipoFiltroEstado))
                .Cast<TipoFiltroEstado>()
                .Select(e => new
                {
                    Valor = (int)e,
                    Texto = e
                })
                .ToList();
            comboBox.DataSource = lista;
            comboBox.DisplayMember = "Texto";
            comboBox.ValueMember = "Valor";
            comboBox.SelectedIndex = 0;
            estaCargado = true;

        }

        public void CargarComboProvincias(ComboBox combo)
        {
            var lista = _provinciaServicio.ObtenerDatosCombo(TipoProvinciaDefault.Todas);
            combo.DataSource = lista;
            combo.DisplayMember = "Nombre";
            combo.ValueMember = "ProvinciaId";
            combo.SelectedIndex = 0;
            estaCargado = true;
        }
        private void RecargarGrilla()
        {
            try
            {
                var resultado = _transporteServicio.ObtenerPagina(paginaActual, cantidadPorPagina,
                    filtroActivo, provinciaIdFiltro, textoBuscar);
                MostrarDatosEnGrilla(resultado);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MostrarDatosEnGrilla(ResultadoPaginacionDto<TransporteListDto> resultado)
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
            using (frmTransporteAe frm = new frmTransporteAe(_provinciaServicio) { Text = "Nuevo Transporte" })
            {
                DialogResult dr = frm.ShowDialog();
                if (dr == DialogResult.Cancel) return;
                TransporteEditDto? transporteEditDto = frm.GetTransporte();
                if (transporteEditDto == null) return;
                try
                {
                    TransporteCreateDto transporteCreateDto = new TransporteCreateDto
                    {
                        NombreEmpresa = transporteEditDto.NombreEmpresa,
                        Telefono = transporteEditDto.Telefono,
                        Email = transporteEditDto.Email,
                        ProvinciaId = transporteEditDto.ProvinciaId,
                    };
                    int nuevoId = _transporteServicio.Agregar(transporteCreateDto);
                    bool sePuedeVer = (filtroActivo is null || filtroActivo == true) &&
                        (provinciaIdFiltro is null || transporteCreateDto.ProvinciaId == provinciaIdFiltro) &&
                        (string.IsNullOrWhiteSpace(txtBuscar.Text) ||
                        transporteCreateDto.NombreEmpresa.Contains(txtBuscar.Text));
                    if (sePuedeVer)
                    {
                        paginaActual = _transporteServicio
                            .ObtenerPaginaRegistro(transporteCreateDto.NombreEmpresa, cantidadPorPagina,
                            filtroActivo, provinciaIdFiltro, textoBuscar);

                    }
                    RecargarGrilla();
                    if (sePuedeVer)
                    {
                        var nuevoTransporte = _bindingSource.List
                            .Cast<TransporteListDto>()
                            .FirstOrDefault(t => t.TransporteId == nuevoId);
                        if (nuevoTransporte is null) return;
                        _bindingSource.Position = _bindingSource.IndexOf(nuevoTransporte);
                        MessageBox.Show("Transporte Agregado",
                            "Mensaje", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                    }
                    else
                    {
                        MessageBox.Show($"Transporte {transporteCreateDto.NombreEmpresa} agregado.\nNo se muestra por condición de filtrado o búsqueda",
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
            TransporteListDto transporteDto = (TransporteListDto)_bindingSource.Current!;
            DialogResult dr = MessageBox.Show($"¿Desea borrar el transporte {transporteDto.NombreEmpresa}?",
                "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);
            if (dr == DialogResult.No) return;
            try
            {
                _transporteServicio.Borrar(transporteDto.TransporteId);
                RecargarGrilla();
                if (paginaActual > totalPaginas && totalPaginas > 0)
                {
                    paginaActual = totalPaginas;
                    RecargarGrilla();
                }
                MessageBox.Show("Transporte eliminado",
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

            TransporteListDto transporteDto = (TransporteListDto)_bindingSource.Current!;
            int posicion = _bindingSource.Position;
            TransporteEditDto? transporteEditDto = _transporteServicio.ObtenerParaEditar(transporteDto.TransporteId);
            if (transporteEditDto is null) return;
            using (frmTransporteAe frm = new frmTransporteAe(_provinciaServicio) { Text = "Editar Transporte " })
            {
                frm.SetTransporte(transporteEditDto);
                DialogResult dr = frm.ShowDialog();
                if (dr == DialogResult.Cancel) return;
                transporteEditDto = frm.GetTransporte();
                if (transporteEditDto is null) return;
                try
                {
                    _transporteServicio.Editar(transporteEditDto);
                    int editadoId = transporteEditDto.TransporteId;
                    bool sePuedeVer = (filtroActivo is null || filtroActivo == true) &&
                        (provinciaIdFiltro is null || transporteEditDto.ProvinciaId == provinciaIdFiltro) &&
                        (string.IsNullOrWhiteSpace(txtBuscar.Text) ||
                        transporteEditDto.NombreEmpresa.Contains(txtBuscar.Text));

                    if (sePuedeVer)
                    {
                        paginaActual = _transporteServicio.ObtenerPaginaRegistro(transporteEditDto.NombreEmpresa,
                            cantidadPorPagina, filtroActivo, provinciaIdFiltro, textoBuscar);
                    }
                    RecargarGrilla();
                    if (sePuedeVer)
                    {

                        var editadoTipo = _bindingSource.List
                            .Cast<TransporteListDto>()
                            .FirstOrDefault(tb => tb.TransporteId == editadoId);

                        _bindingSource.Position = _bindingSource.IndexOf(editadoTipo);
                        MessageBox.Show("Transporte editado",
                            "Mensaje",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show($"Transporte {transporteEditDto.NombreEmpresa} editado.\nNo se muestra por condición de filtrado o búsqueda",
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

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void btnPrimero_Click(object sender, EventArgs e)
        {
            paginaActual = 1;
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

        private void btnAnterior_Click(object sender, EventArgs e)
        {
            paginaActual--;
            if (paginaActual <= 0)
            {
                paginaActual = 1;
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

        private void tsbActualizar_Click(object sender, EventArgs e)
        {
            filtroActivo = null;
            provinciaIdFiltro = null;
            textoBuscar = null;
            txtBuscar.Clear();
            tsbBuscar.BackColor = SystemColors.Control;
            tsCboProvincias.ComboBox.SelectedIndex = 0;
            tsCboActivo.ComboBox.SelectedIndex = 0;
            paginaActual = 1;
            RecargarGrilla();

        }

        private void tsCboProvincias_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!estaCargado) return;
            if (tsCboProvincias.ComboBox.ValueMember == null) return;
            if (tsCboProvincias.ComboBox.SelectedIndex == 0)
            {
                provinciaIdFiltro = null;
            }
            else
            {
                provinciaIdFiltro = (int)tsCboProvincias.ComboBox.SelectedValue!;
            }
            paginaActual = 1;
            RecargarGrilla();
        }

        private void tsCboActivo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!estaCargado) return;
            if (tsCboActivo.ComboBox.ValueMember == null) return;
            switch (tsCboActivo.ComboBox.Text)
            {
                case "Todos":
                    filtroActivo = null;
                    break;
                case "Activos":
                    filtroActivo = true;
                    break;
                case "Inactivos":
                    filtroActivo = false;
                    break;
            }
            paginaActual = 1;
            RecargarGrilla();
        }
    }
}
