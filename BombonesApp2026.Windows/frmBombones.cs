using Bombones2026.Servicios.DTOs.Paginacion;
using BombonesApp2026.Servicios.DTOs.Bombon;
using BombonesApp2026.Servicios.Interfaces;
using BombonesApp2026.Servicios.Mapeadores;
using BombonesApp2026.Windows.Helpers;

namespace BombonesApp2026.Windows
{
    public partial class frmBombones : Form
    {
        private readonly IBombonServicio _bombonServicio;
        private readonly ITipoBombonServicio _tipoServicio;
        private BindingSource _bindingSource = new BindingSource();
        private ServicioMensajes _servicioMensajes = new();
        //para paginar
        private EstadoNavegacion _estadoNavegacion = new(10);
        public frmBombones(IBombonServicio bombonServicio,
            ITipoBombonServicio tipoServicio)
        {
            InitializeComponent();
            _bombonServicio = bombonServicio;
            _tipoServicio = tipoServicio;
        }

        private void frmBombones_Load(object sender, EventArgs e)
        {
            RecargarGrilla();
        }

        private void RecargarGrilla()
        {
            try
            {
                var paginaSolicitada = _estadoNavegacion.PaginaActual;
                var resultado = _bombonServicio
                    .ObtenerPagina(paginaSolicitada,
                    _estadoNavegacion.CantidadPorPagina,
                    _estadoNavegacion.FiltroActivo, _estadoNavegacion.TextoBuscar);
                _estadoNavegacion.Actualizar(resultado.TotalRegistros);
                if (paginaSolicitada != _estadoNavegacion.PaginaActual)
                {
                    resultado = _bombonServicio
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

        private void MostrarDatosEnGrilla(ResultadoPaginacionDto<BombonListDto> resultado)
        {
            //_estadoNavegacion.Actualizar(resultado.TotalRegistros);
            
            _bindingSource.DataSource = resultado.Items;
            dgvDatos.DataSource = _bindingSource;

            lblCantidad.Text = _estadoNavegacion.TextoRegistros();
            lblPaginas.Text =_estadoNavegacion.TextoPaginas();

            btnPrimero.Enabled = resultado.TieneRegistrosAnteriores;
            btnAnterior.Enabled = resultado.TieneRegistrosAnteriores;
            btnSiguiente.Enabled = resultado.TieneRegistrosSiguientes;
            btnUltimo.Enabled = resultado.TieneRegistrosSiguientes;
        }

        private void tsbNuevo_Click(object sender, EventArgs e)
        {
            using (frmBombonAe frm = new frmBombonAe(_tipoServicio) { Text = "Nueva Bombon" })
            {
                DialogResult dr = frm.ShowDialog();
                if (dr == DialogResult.Cancel) return;
                BombonEditDto? bombonEditDto = frm.GetBombon();
                if (bombonEditDto == null) return;
                try
                {
                    BombonCreateDto bombonCreateDto = bombonEditDto.ToCreateDto();
                    int nuevoId = _bombonServicio.Agregar(bombonCreateDto);
                    bool sePuedeVer = string.IsNullOrWhiteSpace(txtBuscar.Text) ||
                        bombonCreateDto.Nombre.Contains(txtBuscar.Text);
                    if (sePuedeVer)
                    {
                        _estadoNavegacion.PaginaActual = _bombonServicio
                            .ObtenerPaginaRegistro(bombonCreateDto.Nombre, _estadoNavegacion.CantidadPorPagina,
                            _estadoNavegacion.FiltroActivo, _estadoNavegacion.TextoBuscar);

                    }
                    RecargarGrilla();
                    if (sePuedeVer)
                    {
                        var nuevoTipo = _bindingSource.List
                            .Cast<BombonListDto>()
                            .FirstOrDefault(tb => tb.ProductoId == nuevoId);
                        if (nuevoTipo is null) return;
                        _bindingSource.Position = _bindingSource.IndexOf(nuevoTipo);
                        _servicioMensajes.Informacion("Bombon Agregado");

                    }
                    else
                    {
                        _servicioMensajes.Informacion($"Bombon {bombonCreateDto.Nombre} agregada." +
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
            BombonListDto? bombonDto = _bindingSource
                .ObtenerSeleccionado<BombonListDto>(_servicioMensajes);
            if (bombonDto is null) return;
            if(!_servicioMensajes.Confirmar($"¿Desea borrar el bombon {bombonDto.Nombre}?"))
            {
                return;
            }
            try
            {
                _bombonServicio.Borrar(bombonDto.ProductoId);
                RecargarGrilla();
                _servicioMensajes.Informacion("Bombon eliminado");
            }
            catch (Exception ex)
            {

                _servicioMensajes.Error(ex.Message);

            }

        }

        private void tsbEditar_Click(object sender, EventArgs e)
        {
            BombonListDto? bombonDto = _bindingSource
                .ObtenerSeleccionado<BombonListDto>(_servicioMensajes);
            if (bombonDto is null) return;
            BombonEditDto? bombonEditDto = _bombonServicio.ObtenerParaEditar(bombonDto.ProductoId);
            if (bombonEditDto is null) return;
            using (frmBombonAe frm = new frmBombonAe(_tipoServicio) { Text = "Editar Bombon " })
            {
                frm.SetBombon(bombonEditDto);
                DialogResult dr = frm.ShowDialog();
                if (dr == DialogResult.Cancel) return;
                bombonEditDto = frm.GetBombon();
                if (bombonEditDto is null) return;
                try
                {
                    _bombonServicio.Editar(bombonEditDto);
                    int editadoId = bombonEditDto.ProductoId;
                    bool sePuedeVer = string.IsNullOrWhiteSpace(txtBuscar.Text) ||
                        bombonEditDto.Nombre.ToLower().Contains(txtBuscar.Text.ToLower());

                    if (sePuedeVer)
                    {
                        _estadoNavegacion.PaginaActual = _bombonServicio.ObtenerPaginaRegistro(bombonEditDto.Nombre,
                            _estadoNavegacion.CantidadPorPagina, _estadoNavegacion.FiltroActivo, _estadoNavegacion.TextoBuscar);
                    }
                    RecargarGrilla();
                    if (sePuedeVer)
                    {

                        var editadoTipo = _bindingSource.List
                            .Cast<BombonListDto>()
                            .FirstOrDefault(tb => tb.ProductoId == editadoId);

                        _bindingSource.Position = _bindingSource.IndexOf(editadoTipo);
                        _servicioMensajes.Informacion("Bombon editado");
                    }
                    else
                    {
                        _servicioMensajes.Informacion($"Bombon {bombonEditDto.Nombre} editada." +
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

        private void tsbDetalle_Click(object sender, EventArgs e)
        {
            BombonListDto? bombonDto = _bindingSource
                .ObtenerSeleccionado<BombonListDto>(_servicioMensajes);
            if (bombonDto is null) return;
            BombonEditDto? bombonEditDto = _bombonServicio.ObtenerParaEditar(bombonDto.ProductoId);
            if (bombonEditDto is null) return;
            using (frmDetalleBombon frm = new frmDetalleBombon() { Text = $"Descripción del Bombón: {bombonEditDto.Nombre}" })
            {
                frm.SetDescripcion(bombonEditDto.Descripcion);
                frm.ShowDialog(this);
            }
        }
    }
}
