using Bombones2026.Servicios.DTOs.Paginacion;
using Bombones2026.Servicios.DTOs.Transporte;
using BombonesApp2026.Entidades.Enum;
using BombonesApp2026.Servicios.Interfaces;
using BombonesApp2026.Windows.Helpers;

namespace BombonesApp2026.Windows
{
    public partial class frmTransportes : Form
    {
        private readonly ITransporteServicio _transporteServicio;
        private readonly IProvinciaServicio _provinciaServicio;
        private BindingSource _bindingSource = new BindingSource();
        private ServicioMensajes _servicioMensajes = new();
        private EstadoNavegacion _estadoNavegacion = new(10);
        //para paginar
        private bool estaCargado = false;
        private int? provinciaIdFiltro = null;
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
                var paginaSolicitada = _estadoNavegacion.PaginaActual;
                var resultado = _transporteServicio
                    .ObtenerPagina(paginaSolicitada,
                    _estadoNavegacion.CantidadPorPagina,
                    _estadoNavegacion.FiltroActivo, provinciaIdFiltro,
                    _estadoNavegacion.TextoBuscar);
                _estadoNavegacion.Actualizar(resultado.TotalRegistros);
                if (paginaSolicitada != _estadoNavegacion.PaginaActual)
                {
                    resultado = _transporteServicio
                        .ObtenerPagina(_estadoNavegacion.PaginaActual,
                        _estadoNavegacion.CantidadPorPagina,
                        _estadoNavegacion.FiltroActivo,provinciaIdFiltro,
                        _estadoNavegacion.TextoBuscar);
                    _estadoNavegacion.Actualizar(resultado.TotalRegistros);
                }
                MostrarDatosEnGrilla(resultado);
            }
            catch (Exception ex)
            {

                _servicioMensajes.Error(ex.Message);
            }
        }

        private void MostrarDatosEnGrilla(ResultadoPaginacionDto<TransporteListDto> resultado)
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
                    bool sePuedeVer = (_estadoNavegacion.FiltroActivo is null || _estadoNavegacion.FiltroActivo == true) &&
                        (provinciaIdFiltro is null || transporteCreateDto.ProvinciaId == provinciaIdFiltro) &&
                        (string.IsNullOrWhiteSpace(txtBuscar.Text) ||
                        transporteCreateDto.NombreEmpresa.Contains(txtBuscar.Text));
                    if (sePuedeVer)
                    {
                        _estadoNavegacion.PaginaActual = _transporteServicio
                            .ObtenerPaginaRegistro(transporteCreateDto.NombreEmpresa, _estadoNavegacion.CantidadPorPagina,
                            _estadoNavegacion.FiltroActivo, provinciaIdFiltro, _estadoNavegacion.TextoBuscar);

                    }
                    RecargarGrilla();
                    if (sePuedeVer)
                    {
                        var nuevoTransporte = _bindingSource.List
                            .Cast<TransporteListDto>()
                            .FirstOrDefault(t => t.TransporteId == nuevoId);
                        if (nuevoTransporte is null) return;
                        _bindingSource.Position = _bindingSource.IndexOf(nuevoTransporte);
                        _servicioMensajes.Informacion("Registro agregado");

                    }
                    else
                    {
                        _servicioMensajes.Informacion($"Transporte {transporteCreateDto.NombreEmpresa} agregado." +
                            $"\nNo se muestra por condición de filtrado o búsqueda");
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
            TransporteListDto? transporteDto = _bindingSource
                .ObtenerSeleccionado<TransporteListDto>(_servicioMensajes);
            if (transporteDto is null) return;
            if (!_servicioMensajes.Confirmar($"¿Desea borrar el transporte {transporteDto.NombreEmpresa}?"))
            {
                return;
            }
            try
            {
                _transporteServicio.Borrar(transporteDto.TransporteId);
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
            TransporteListDto? transporteDto = _bindingSource
                .ObtenerSeleccionado<TransporteListDto>(_servicioMensajes);
            if (transporteDto is null) return;
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
                    bool sePuedeVer = (_estadoNavegacion.FiltroActivo is null || _estadoNavegacion.FiltroActivo == true) &&
                        (provinciaIdFiltro is null || transporteEditDto.ProvinciaId == provinciaIdFiltro) &&
                        (string.IsNullOrWhiteSpace(txtBuscar.Text) ||
                        transporteEditDto.NombreEmpresa.Contains(txtBuscar.Text));

                    if (sePuedeVer)
                    {
                        _estadoNavegacion.PaginaActual = _transporteServicio.ObtenerPaginaRegistro(transporteEditDto.NombreEmpresa,
                            _estadoNavegacion.CantidadPorPagina, _estadoNavegacion.FiltroActivo, provinciaIdFiltro, _estadoNavegacion.TextoBuscar);
                    }
                    RecargarGrilla();
                    if (sePuedeVer)
                    {

                        var editadoTipo = _bindingSource.List
                            .Cast<TransporteListDto>()
                            .FirstOrDefault(tb => tb.TransporteId == editadoId);

                        _bindingSource.Position = _bindingSource.IndexOf(editadoTipo);
                        _servicioMensajes.Informacion("Registro editado");
                    }
                    else
                    {
                        _servicioMensajes.Informacion($"Transporte {transporteEditDto.NombreEmpresa} editado." +
                            $"\nNo se muestra por condición de filtrado o búsqueda");
                    }

                }
                catch (Exception ex)
                {

                    _servicioMensajes.Error(ex.Message);

                }
            }


        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

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

        private void tsbActualizar_Click(object sender, EventArgs e)
        {
            _estadoNavegacion.Inicializar();
            provinciaIdFiltro = null;
            txtBuscar.Clear();
            tsbBuscar.BackColor = SystemColors.Control;
            tsCboProvincias.ComboBox.SelectedIndex = 0;
            tsCboActivo.ComboBox.SelectedIndex = 0;
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
            _estadoNavegacion.IrPrimera();
            RecargarGrilla();
        }

        private void tsCboActivo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!estaCargado) return;
            if (tsCboActivo.ComboBox.ValueMember == null) return;
            switch (tsCboActivo.ComboBox.Text)
            {
                case "Todos":
                    _estadoNavegacion.FiltroActivo = null;
                    break;
                case "Activos":
                    _estadoNavegacion.FiltroActivo = true;
                    break;
                case "Inactivos":
                    _estadoNavegacion.FiltroActivo = false;
                    break;
            }
            _estadoNavegacion.IrPrimera();
            RecargarGrilla();
        }
    }
}
