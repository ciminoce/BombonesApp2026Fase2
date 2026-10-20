using Bombones2026.Servicios.DTOs.Ciudad;
using Bombones2026.Servicios.DTOs.Paginacion;
using BombonesApp2026.Servicios.Interfaces;
using BombonesApp2026.Windows.Helpers;

namespace BombonesApp2026.Windows
{
    public partial class frmCiudades : Form
    {
        private readonly ICiudadServicio _ciudadServicio;
        private readonly IProvinciaServicio _provinciaServicio;
        private BindingSource _bindingSource = new BindingSource();
        private ServicioMensajes _servicioMensajes = new();
        private EstadoNavegacion _estadoNavegacion = new(10);
        //para paginar
        public frmCiudades(ICiudadServicio ciuidadServicio, IProvinciaServicio provinciaServicio)
        {
            InitializeComponent();
            _ciudadServicio = ciuidadServicio;
            _provinciaServicio = provinciaServicio;
        }

        private void frmCiudades_Load(object sender, EventArgs e)
        {
            RecargarGrilla();
        }

        private void RecargarGrilla()
        {
            try
            {
                var paginaSolicitada = _estadoNavegacion.PaginaActual;
                var resultado = _ciudadServicio
                    .ObtenerPagina(paginaSolicitada,
                    _estadoNavegacion.CantidadPorPagina,
                    _estadoNavegacion.FiltroActivo, _estadoNavegacion.TextoBuscar);
                _estadoNavegacion.Actualizar(resultado.TotalRegistros);
                if (paginaSolicitada != _estadoNavegacion.PaginaActual)
                {
                    resultado = _ciudadServicio
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

        private void MostrarDatosEnGrilla(ResultadoPaginacionDto<CiudadListDto> resultado)
        {
            //_estadoNavegacion.Actualizar(resultado.TotalRegistros);

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
            using (frmCiudadAe frm = new frmCiudadAe(_provinciaServicio) { Text = "Nueva Ciudad" })
            {
                DialogResult dr = frm.ShowDialog();
                if (dr == DialogResult.Cancel) return;
                CiudadEditDto? ciudadEditDto = frm.GetCiudad();
                if (ciudadEditDto == null) return;
                try
                {
                    CiudadCreateDto ciudadCreateDto = new CiudadCreateDto
                    {
                        Nombre = ciudadEditDto.Nombre,
                        ProvinciaId = ciudadEditDto.ProvinciaId
                    };
                    int nuevoId = _ciudadServicio.Agregar(ciudadCreateDto);
                    bool sePuedeVer = string.IsNullOrWhiteSpace(txtBuscar.Text) ||
                        ciudadCreateDto.Nombre.Contains(txtBuscar.Text);
                    if (sePuedeVer)
                    {
                        _estadoNavegacion.PaginaActual = _ciudadServicio
                            .ObtenerPaginaRegistro(ciudadCreateDto.Nombre, _estadoNavegacion.CantidadPorPagina,
                            _estadoNavegacion.FiltroActivo, _estadoNavegacion.TextoBuscar);

                    }
                    RecargarGrilla();
                    if (sePuedeVer)
                    {
                        var nuevoTipo = _bindingSource.List
                            .Cast<CiudadListDto>()
                            .FirstOrDefault(tb => tb.CiudadId == nuevoId);
                        if (nuevoTipo is null) return;
                        _bindingSource.Position = _bindingSource.IndexOf(nuevoTipo);
                        _servicioMensajes.Informacion("Registro agregado");

                    }
                    else
                    {
                        _servicioMensajes.Informacion($"Ciudad {ciudadCreateDto.Nombre} agregada." +
                            $"\nNo se muestra por condición de filtrado o búsqueda");
                    }
                }
                catch (Exception ex)
                {
                    _servicioMensajes.Error(ex.Message);
                }
            }
        }

        private void tsbCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void tsbBorrar_Click(object sender, EventArgs e)
        {

            CiudadListDto? ciudadDto = _bindingSource
                .ObtenerSeleccionado<CiudadListDto>(_servicioMensajes);
            if (ciudadDto is null) return;
            if (!_servicioMensajes.Confirmar($"¿Desea borrar la ciudad {ciudadDto.Ciudad}?"))
            {
                return;
            }
            try
            {
                _ciudadServicio.Borrar(ciudadDto.CiudadId);
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
            CiudadListDto? ciudadDto = _bindingSource
                .ObtenerSeleccionado<CiudadListDto>(_servicioMensajes);
            if (ciudadDto is null) return;
            CiudadEditDto? ciudadEditDto = _ciudadServicio.ObtenerParaEditar(ciudadDto.CiudadId);
            if (ciudadEditDto is null) return;
            using (frmCiudadAe frm = new frmCiudadAe(_provinciaServicio) { Text = "Editar Ciudad " })
            {
                frm.SetCiudad(ciudadEditDto);
                DialogResult dr = frm.ShowDialog();
                if (dr == DialogResult.Cancel) return;
                ciudadEditDto = frm.GetCiudad();
                if (ciudadEditDto is null) return;
                try
                {
                    _ciudadServicio.Editar(ciudadEditDto);
                    int editadoId = ciudadEditDto.CiudadId;
                    bool sePuedeVer = string.IsNullOrWhiteSpace(txtBuscar.Text) ||
                        ciudadEditDto.Nombre.ToLower().Contains(txtBuscar.Text.ToLower());

                    if (sePuedeVer)
                    {
                        _estadoNavegacion.PaginaActual = _ciudadServicio.ObtenerPaginaRegistro(ciudadEditDto.Nombre,
                            _estadoNavegacion.CantidadPorPagina, _estadoNavegacion.FiltroActivo, _estadoNavegacion.TextoBuscar);
                    }
                    RecargarGrilla();
                    if (sePuedeVer)
                    {

                        var editadoTipo = _bindingSource.List
                            .Cast<CiudadListDto>()
                            .FirstOrDefault(tb => tb.CiudadId == editadoId);

                        _bindingSource.Position = _bindingSource.IndexOf(editadoTipo);
                        _servicioMensajes.Informacion("Registro editado");
                    }
                    else
                    {
                        _servicioMensajes.Informacion($"Ciudad {ciudadEditDto.Nombre} editada." +
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

        private void tsbActualizar_Click(object sender, EventArgs e)
        {
            _estadoNavegacion.Inicializar();
            txtBuscar.Clear();
            tsbBuscar.BackColor = SystemColors.Control;
            RecargarGrilla();
        }
    }
}
