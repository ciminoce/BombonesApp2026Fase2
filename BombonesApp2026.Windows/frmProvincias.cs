using Bombones2026.Servicios.DTOs.Paginacion;
using Bombones2026.Servicios.DTOs.Provincia;
using BombonesApp2026.Servicios.Interfaces;
using BombonesApp2026.Servicios.Servicios;
using BombonesApp2026.Windows.Helpers;

namespace BombonesApp2026.Windows
{
    //TODO: Buscar Provincia
    public partial class frmProvincias : Form
    {
        private readonly IProvinciaServicio _provinciaServicio;
        private BindingSource _bindingSource = new BindingSource();
        private ServicioMensajes _servicioMensajes = new();
        private EstadoNavegacion _estadoNavegacion = new(10);
        //para paginar
        public frmProvincias(IProvinciaServicio servicio)
        {
            InitializeComponent();
            _provinciaServicio = servicio;
        }

        private void tsbCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void MostrarDatosEnGrilla(ResultadoPaginacionDto<ProvinciaListDto> resultado)
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
            using (frmProvinciaAe frm = new frmProvinciaAe() { Text = "Nueva Provincia" })
            {
                DialogResult dr = frm.ShowDialog();
                if (dr == DialogResult.Cancel) return;
                ProvinciaEditDto? provinciaEditDto = frm.GetProvincia();
                if (provinciaEditDto == null) return;
                try
                {
                    ProvinciaCreateDto provinciaCreateDto = new ProvinciaCreateDto
                    {
                        Nombre = provinciaEditDto.Nombre,
                    };
                    int nuevoId = _provinciaServicio.Agregar(provinciaCreateDto);
                    bool sePuedeVer = (_estadoNavegacion.FiltroActivo is null || _estadoNavegacion.FiltroActivo == true) &&
                        (string.IsNullOrWhiteSpace(txtBuscar.Text) ||
                        provinciaCreateDto.Nombre.Contains(txtBuscar.Text));
                    if (sePuedeVer)
                    {
                        _estadoNavegacion.PaginaActual = _provinciaServicio
                            .ObtenerPaginaRegistro(provinciaCreateDto.Nombre, _estadoNavegacion.CantidadPorPagina,
                            _estadoNavegacion.FiltroActivo, _estadoNavegacion.TextoBuscar);

                    }
                    RecargarGrilla();
                    if (sePuedeVer)
                    {
                        var nuevoTipo = _bindingSource.List
                            .Cast<ProvinciaListDto>()
                            .FirstOrDefault(tb => tb.ProvinciaId == nuevoId);
                        if (nuevoTipo is null) return;
                        _bindingSource.Position = _bindingSource.IndexOf(nuevoTipo);
                        _servicioMensajes.Informacion("Registro agregado");

                    }
                    else
                    {
                        _servicioMensajes.Informacion($"Provincia {provinciaEditDto.Nombre} agregada." +
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
            ProvinciaListDto? provinciaDto = _bindingSource
                .ObtenerSeleccionado<ProvinciaListDto>(_servicioMensajes);
            if (provinciaDto is null) return;
            if(!_servicioMensajes.Confirmar($"¿Desea borrar la provincia {provinciaDto.Nombre}?"))
            {
                return;
            }
            try
            {
                _provinciaServicio.Borrar(provinciaDto.ProvinciaId);
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
            ProvinciaListDto? provinciaDto = _bindingSource
                .ObtenerSeleccionado<ProvinciaListDto>(_servicioMensajes);
            if (provinciaDto is null) return;
            ProvinciaEditDto? provinciaEditDto = _provinciaServicio.ObtenerParaEditar(provinciaDto.ProvinciaId);
            if (provinciaEditDto is null) return;
            using (frmProvinciaAe frm = new frmProvinciaAe() { Text = "Editar Provincia " })
            {
                frm.SetProvincia(provinciaEditDto);
                DialogResult dr = frm.ShowDialog();
                if (dr == DialogResult.Cancel) return;
                provinciaEditDto = frm.GetProvincia();
                if (provinciaEditDto is null) return;
                try
                {
                    _provinciaServicio.Editar(provinciaEditDto);
                    int editadoId = provinciaEditDto.ProvinciaId;
                    bool sePuedeVer = (_estadoNavegacion.FiltroActivo is null || _estadoNavegacion.FiltroActivo == true) &&
                        (string.IsNullOrWhiteSpace(txtBuscar.Text) ||
                        provinciaEditDto.Nombre.ToLower().Contains(txtBuscar.Text.ToLower()));

                    if (sePuedeVer)
                    {
                        _estadoNavegacion.PaginaActual = _provinciaServicio.ObtenerPaginaRegistro(provinciaEditDto.Nombre,
                            _estadoNavegacion.CantidadPorPagina, _estadoNavegacion.FiltroActivo, _estadoNavegacion.TextoBuscar);
                    }
                    RecargarGrilla();
                    if (sePuedeVer)
                    {

                        var editadoTipo = _bindingSource.List
                            .Cast<ProvinciaListDto>()
                            .FirstOrDefault(tb => tb.ProvinciaId == editadoId);

                        _bindingSource.Position = _bindingSource.IndexOf(editadoTipo);
                        _servicioMensajes.Informacion("Registro editado");
                    }
                    else
                    {
                        _servicioMensajes.Informacion($"Provincia {provinciaEditDto.Nombre} editada." +
                            $"\nNo se muestra por condición de filtrado o búsqueda");
                    }

                }
                catch (Exception ex)
                {

                    _servicioMensajes.Error(ex.Message);
                }
            }
        }

        private void frmProvincias_Load(object sender, EventArgs e)
        {
            RecargarGrilla();
        }

        private void RecargarGrilla()
        {
            try
            {
                var paginaSolicitada = _estadoNavegacion.PaginaActual;
                var resultado = _provinciaServicio
                    .ObtenerPagina(paginaSolicitada,
                    _estadoNavegacion.CantidadPorPagina,
                    _estadoNavegacion.FiltroActivo, _estadoNavegacion.TextoBuscar);
                _estadoNavegacion.Actualizar(resultado.TotalRegistros);
                if (paginaSolicitada != _estadoNavegacion.PaginaActual)
                {
                    resultado = _provinciaServicio
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

        private void tsbActualizar_Click(object sender, EventArgs e)
        {
            _estadoNavegacion.Inicializar();
            txtBuscar.Clear();
            tsbBuscar.BackColor = SystemColors.Control;
            RecargarGrilla();
        }
    }

}
