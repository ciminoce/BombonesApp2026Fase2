using Bombones2026.Servicios.DTOs.Paginacion;
using BombonesApp2026.Servicios.DTOs.Caja;
using BombonesApp2026.Servicios.Excepciones;
using BombonesApp2026.Servicios.Interfaces;
using BombonesApp2026.Servicios.Mapeadores;
using BombonesApp2026.Windows.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace BombonesApp2026.Windows
{
    public partial class frmCajas : Form
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ICajaServicio _cajaServicio;
        private readonly IBombonServicio _bombonServicio;
        private BindingSource _bindingSource = new BindingSource();
        private ServicioMensajes _servicioMensajes = new();
        private EstadoNavegacion _estadoNavegacion = new(10);
        public frmCajas(IServiceProvider serviceProvider,
            ICajaServicio cajaServicio, IBombonServicio bombonServicio)
        {
            InitializeComponent();
            _serviceProvider=serviceProvider;
            _cajaServicio = cajaServicio;
            _bombonServicio = bombonServicio;
            dgvDatos.DataSource = _bindingSource;
        }

        private void frmCajas_Load(object sender, EventArgs e)
        {
            RecargarGrilla();
        }

        private void RecargarGrilla()
        {
            try
            {
                var paginaSolicitada = _estadoNavegacion.PaginaActual;
                var resultado = _cajaServicio
                    .ObtenerPagina(paginaSolicitada,
                    _estadoNavegacion.CantidadPorPagina,
                    _estadoNavegacion.FiltroActivo, _estadoNavegacion.TextoBuscar);
                _estadoNavegacion.Actualizar(resultado.TotalRegistros);
                if (paginaSolicitada != _estadoNavegacion.PaginaActual)
                {
                    resultado = _cajaServicio
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

        private void MostrarDatosEnGrilla(ResultadoPaginacionDto<CajaListDto> resultado)
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

        private void tsbCerrar_Click(object sender, EventArgs e)
        {
            Close();
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
        private void tsbNuevo_Click(object sender, EventArgs e)
        {
            using (frmCajaAe frm = _serviceProvider.GetRequiredService<frmCajaAe>())
            {
                frm.Text = "Nueva Caja";
                if (frm.ShowDialog() == DialogResult.Cancel) return;

                CajaEditDto? cajaEditDto = frm.GetCaja();
                if (cajaEditDto == null) return;
                try
                {
                    CajaCreateDto cajaCreateDto = cajaEditDto.ToCreateDto();
                    int nuevoId = _cajaServicio.Agregar(cajaCreateDto);
                    bool sePuedeVer = string.IsNullOrWhiteSpace(txtBuscar.Text) ||
                        cajaCreateDto.Nombre.Contains(txtBuscar.Text);
                    if (sePuedeVer)
                    {
                        _estadoNavegacion.PaginaActual = _cajaServicio
                            .ObtenerPaginaRegistro(cajaCreateDto.Nombre, _estadoNavegacion.CantidadPorPagina,
                            _estadoNavegacion.FiltroActivo, _estadoNavegacion.TextoBuscar);

                    }
                    RecargarGrilla();
                    if (sePuedeVer)
                    {
                        var nuevoTipo = _bindingSource.List
                            .Cast<CajaListDto>()
                            .FirstOrDefault(tb => tb.ProductoId == nuevoId);
                        if (nuevoTipo is null) return;
                        _bindingSource.Position = _bindingSource.IndexOf(nuevoTipo);
                        _servicioMensajes.Informacion("Registro agregado");

                    }
                    else
                    {
                        _servicioMensajes.Informacion($"Caja {cajaCreateDto.Nombre} agregada." +
                            $"\nNo se muestra por condición de filtrado o búsqueda");
                    }
                }
                catch(ValidarCajaExcepcion ex)
                {
                    _servicioMensajes.Errores(ex.Errores, "Lista de Errores");
                }
                catch(ArgumentException ex)
                {
                    _servicioMensajes.Error(ex.Message);
                }
                catch (Exception ex)
                {
                    _servicioMensajes.Error(ex.Message);
                }
            }
        }


        private void tsbBorrar_Click(object sender, EventArgs e)
        {

            CajaListDto? cajaDto = _bindingSource
                .ObtenerSeleccionado<CajaListDto>(_servicioMensajes);
            if (cajaDto is null) return;
            if (!_servicioMensajes.Confirmar($"¿Desea borrar la caja {cajaDto.Nombre}?")) return;
            try
            {
                _cajaServicio.Borrar(cajaDto.ProductoId);
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

            CajaListDto? cajaDto = _bindingSource
                .ObtenerSeleccionado<CajaListDto>(_servicioMensajes);
            if (cajaDto is null) return;
            CajaEditDto? cajaEditDto = _cajaServicio.ObtenerCajaConDetallesParaEditar(cajaDto.ProductoId);
            if (cajaEditDto is null) return;
            using (frmCajaAe frm = _serviceProvider.GetRequiredService<frmCajaAe>())
            {
                frm.Text = "Editar Caja";
                frm.SetCaja(cajaEditDto);
                if (frm.ShowDialog() == DialogResult.Cancel) return;
                cajaEditDto = frm.GetCaja();
                if (cajaEditDto is null) return;
                try
                {
                    _cajaServicio.Editar(cajaEditDto);
                    int editadoId = cajaEditDto.ProductoId;
                    bool sePuedeVer = string.IsNullOrWhiteSpace(txtBuscar.Text) ||
                        cajaEditDto.Nombre.ToLower().Contains(txtBuscar.Text.ToLower());

                    if (sePuedeVer)
                    {
                        _estadoNavegacion.PaginaActual = _cajaServicio.ObtenerPaginaRegistro(cajaEditDto.Nombre,
                            _estadoNavegacion.CantidadPorPagina, _estadoNavegacion.FiltroActivo, _estadoNavegacion.TextoBuscar);
                    }
                    RecargarGrilla();
                    if (sePuedeVer)
                    {

                        var editadoTipo = _bindingSource.List
                            .Cast<CajaListDto>()
                            .FirstOrDefault(tb => tb.ProductoId == editadoId);

                        _bindingSource.Position = _bindingSource.IndexOf(editadoTipo);
                        _servicioMensajes.Informacion("Registro editado");
                    }
                    else
                    {
                        _servicioMensajes.Informacion($"Caja {cajaEditDto.Nombre} editada." +
                            $"\nNo se muestra por condición de filtrado o búsqueda");
                    }

                }
                catch (ValidarCajaExcepcion ex)
                {
                    _servicioMensajes.Errores(ex.Errores);
                }
                catch (ArgumentException ex)
                {
                    _servicioMensajes.Error(ex.Message);
                }
                catch (KeyNotFoundException ex)
                {
                    _servicioMensajes.Error(ex.Message);
                }
                catch (Exception ex)
                {

                    _servicioMensajes.Error(ex.Message);

                }
            }

        }

        private void tsbDetalle_Click(object sender, EventArgs e)
        {
            CajaListDto? cajaDto = _bindingSource.ObtenerSeleccionado<CajaListDto>(_servicioMensajes);
            if (cajaDto is null) return;
            CajaDetailDto? cajaDetailDto = _cajaServicio.ObtenerCajaConDetalles(cajaDto.ProductoId);
            if (cajaDetailDto is null) return;
            using (frmDetallesCaja frm = _serviceProvider.GetRequiredService<frmDetallesCaja>())
            {
                frm.Text = "Detalles de Caja";
                frm.SetCaja(cajaDetailDto);
                frm.ShowDialog();
            }
        }
    }
}
