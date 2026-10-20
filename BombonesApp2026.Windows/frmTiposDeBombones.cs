using Bombones2026.Servicios.DTOs.Paginacion;
using Bombones2026.Servicios.DTOs.TipoBombon;
using BombonesApp2026.Servicios.Interfaces;
using BombonesApp2026.Servicios.Servicios;
using BombonesApp2026.Windows.Helpers;

namespace BombonesApp2026.Windows
{
    //TODO: OJO revisar cuando se borra!!

    public partial class frmTiposDeBombones : Form
    {
        private readonly ITipoBombonServicio _tipoServicio;
        private BindingSource _bindingSource = new BindingSource();
        private ServicioMensajes _servicioMensajes = new();
        private EstadoNavegacion _estadoNavegacion = new(10);
        //para filtrar
        public frmTiposDeBombones(ITipoBombonServicio tipoServicio)
        {
            InitializeComponent();
            _tipoServicio = tipoServicio;
        }

        private void tsbCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmTiposDeBombones_Load(object sender, EventArgs e)
        {
            RecargarGrilla();
        }
        private void RecargarGrilla()
        {
            try
            {
                var paginaSolicitada = _estadoNavegacion.PaginaActual;
                var resultado = _tipoServicio
                    .ObtenerPagina(paginaSolicitada,
                    _estadoNavegacion.CantidadPorPagina,
                    _estadoNavegacion.FiltroActivo, _estadoNavegacion.TextoBuscar);
                _estadoNavegacion.Actualizar(resultado.TotalRegistros);
                if (paginaSolicitada != _estadoNavegacion.PaginaActual)
                {
                    resultado = _tipoServicio
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

        private void MostrarDatosEnGrilla(ResultadoPaginacionDto<TipoBombonListDto> resultado)
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


        private void activosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _estadoNavegacion.FiltroActivo = true;
            tsbFiltrar.BackColor = Color.Orange;
            _estadoNavegacion.IrPrimera();
            RecargarGrilla();
        }


        private void noActivosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _estadoNavegacion.FiltroActivo = false;
            tsbFiltrar.BackColor = Color.Orange;
            _estadoNavegacion.IrPrimera();
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

        private void tsbNuevo_Click(object sender, EventArgs e)
        {
            using (frmTipoDeBombonesAe frm = new frmTipoDeBombonesAe() { Text = "Nuevo Tipo de Bombón" })
            {
                DialogResult dr = frm.ShowDialog();
                if (dr == DialogResult.Cancel) return;
                TipoBombonEditDto? tipoEditDto = frm.GetTipo();
                if (tipoEditDto == null) return;
                try
                {
                    TipoBombonCreateDto tipoCreateDto = new TipoBombonCreateDto
                    {
                        Nombre = tipoEditDto.Nombre,
                        Descripcion = tipoEditDto.Descripcion,
                    };
                    int nuevoId = _tipoServicio.Agregar(tipoCreateDto);
                    bool sePuedeVer = (_estadoNavegacion.FiltroActivo is null || _estadoNavegacion.FiltroActivo == true) &&
                        (string.IsNullOrWhiteSpace(txtBuscar.Text) ||
                        tipoCreateDto.Nombre.Contains(txtBuscar.Text));
                    if (sePuedeVer)
                    {
                        _estadoNavegacion.PaginaActual = _tipoServicio
                            .ObtenerPaginaRegistro(tipoCreateDto.Nombre, _estadoNavegacion.CantidadPorPagina,
                            _estadoNavegacion.FiltroActivo, _estadoNavegacion.TextoBuscar);

                    }
                    RecargarGrilla();
                    if (sePuedeVer)
                    {
                        var nuevoTipo = _bindingSource.List
                            .Cast<TipoBombonListDto>()
                            .FirstOrDefault(tb => tb.TipoBombonId == nuevoId);
                        if (nuevoTipo is null) return;
                        _bindingSource.Position = _bindingSource.IndexOf(nuevoTipo);
                        _servicioMensajes.Informacion("Registro agregado");

                    }
                    else
                    {
                        _servicioMensajes.Informacion($"Tipo de Bombón {tipoEditDto.Nombre} agregado." +
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
            TipoBombonListDto? tipoBombonDto = _bindingSource
                .ObtenerSeleccionado<TipoBombonListDto>(_servicioMensajes);
            if (tipoBombonDto is null) return;
            if(!_servicioMensajes.Confirmar($"¿Desea borrar el tipo de bombón {tipoBombonDto.Nombre}?"))
            {
                return;
            }
            try
            {
                _tipoServicio.Borrar(tipoBombonDto.TipoBombonId);

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
            TipoBombonListDto? tipoBombonDto = _bindingSource
                .ObtenerSeleccionado<TipoBombonListDto>(_servicioMensajes);
            if (tipoBombonDto is null) return;
            TipoBombonEditDto? tipoBombonEditDto = _tipoServicio.ObtenerParaEditar(tipoBombonDto.TipoBombonId);
            if (tipoBombonEditDto is null) return;
            using (frmTipoDeBombonesAe frm = new frmTipoDeBombonesAe() { Text = "Editar Tipo de Bombón " })
            {
                frm.SetTipo(tipoBombonEditDto);
                DialogResult dr = frm.ShowDialog();
                if (dr == DialogResult.Cancel) return;
                tipoBombonEditDto = frm.GetTipo();
                if (tipoBombonEditDto is null) return;
                try
                {
                    _tipoServicio.Editar(tipoBombonEditDto);
                    int editadoId = tipoBombonEditDto.TipoBombonId;
                    bool sePuedeVer = (_estadoNavegacion.FiltroActivo is null || _estadoNavegacion.FiltroActivo == true) &&
                        string.IsNullOrWhiteSpace(txtBuscar.Text) ||
                        tipoBombonEditDto.Nombre.ToLower().Contains(txtBuscar.Text.ToLower());

                    if (sePuedeVer)
                    {
                        _estadoNavegacion.PaginaActual = _tipoServicio.ObtenerPaginaRegistro(tipoBombonEditDto.Nombre,
                            _estadoNavegacion.CantidadPorPagina, _estadoNavegacion.FiltroActivo, _estadoNavegacion.TextoBuscar);
                    }
                    RecargarGrilla();
                    if (sePuedeVer)
                    {

                        var editadoTipo = _bindingSource.List
                            .Cast<TipoBombonListDto>()
                            .FirstOrDefault(tb => tb.TipoBombonId == editadoId);

                        _bindingSource.Position = _bindingSource.IndexOf(editadoTipo);
                        _servicioMensajes.Informacion("Registro editado");
                    }
                    else
                    {
                        _servicioMensajes.Informacion($"Tipo de Bombón {tipoBombonEditDto.Nombre} editado." +
                            $"\nNo se muestra por condición de filtrado o búsqueda");
                    }

                }
                catch (Exception ex)
                {

                    _servicioMensajes.Error(ex.Message);

                }
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
