using Bombones2026.Servicios.DTOs.Paginacion;
using BombonesApp2026.Servicios.DTOs.Cliente;
using BombonesApp2026.Servicios.Interfaces;
using BombonesApp2026.Servicios.Mapeadores;
using BombonesApp2026.Servicios.Servicios;
using BombonesApp2026.Windows.Helpers;

namespace BombonesApp2026.Windows
{
    public partial class frmClientes : Form
    {
        private readonly IClienteServicio _clienteServicio;
        private readonly IProvinciaServicio _provinciaServicio;
        private readonly ICiudadServicio _ciudadServicio;
        private BindingSource _bindingSource = new BindingSource();
        private ServicioMensajes _servicioMensajes = new();
        private EstadoNavegacion _estadoNavegacion = new(10);
        //para paginar
        public frmClientes(IClienteServicio clienteServicio, IProvinciaServicio provinciaServicio,
            ICiudadServicio ciudadServicio)
        {
            InitializeComponent();
            _clienteServicio = clienteServicio;
            _provinciaServicio = provinciaServicio;
            _ciudadServicio = ciudadServicio;

        }


        private void frmClientes_Load(object sender, EventArgs e)
        {
            RecargarGrilla();
        }

        private void RecargarGrilla()
        {
            try
            {
                var paginaSolicitada = _estadoNavegacion.PaginaActual;
                var resultado = _clienteServicio
                    .ObtenerPagina(paginaSolicitada,
                    _estadoNavegacion.CantidadPorPagina,
                    _estadoNavegacion.FiltroActivo, _estadoNavegacion.TextoBuscar);
                _estadoNavegacion.Actualizar(resultado.TotalRegistros);
                if (paginaSolicitada != _estadoNavegacion.PaginaActual)
                {
                    resultado = _clienteServicio
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

        private void MostrarDatosEnGrilla(ResultadoPaginacionDto<ClienteListDto> resultado)
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
            using (frmClienteAe frm = new frmClienteAe(_provinciaServicio,
                _ciudadServicio) { Text = "Nuevo Cliente" })
            {
                DialogResult dr = frm.ShowDialog();
                if (dr == DialogResult.Cancel) return;
                ClienteEditDto? clienteEditDto = frm.GetCliente();
                if (clienteEditDto == null) return;
                try
                {
                    ClienteCreateDto clienteCreateDto = clienteEditDto.ToCreateDto();
                    int nuevoId = _clienteServicio.Agregar(clienteCreateDto);
                    bool sePuedeVer = string.IsNullOrWhiteSpace(txtBuscar.Text) ||
                        clienteCreateDto.Nombre.Contains(txtBuscar.Text);
                    if (sePuedeVer)
                    {
                        _estadoNavegacion.PaginaActual = _clienteServicio
                            .ObtenerPaginaRegistro(clienteCreateDto.Nombre, _estadoNavegacion.CantidadPorPagina,
                            _estadoNavegacion.FiltroActivo, _estadoNavegacion.TextoBuscar);

                    }
                    RecargarGrilla();
                    if (sePuedeVer)
                    {
                        var nuevoTipo = _bindingSource.List
                            .Cast<ClienteListDto>()
                            .FirstOrDefault(tb => tb.ClienteId == nuevoId);
                        if (nuevoTipo is null) return;
                        _bindingSource.Position = _bindingSource.IndexOf(nuevoTipo);
                        _servicioMensajes.Informacion("Registro agregado");

                    }
                    else
                    {
                        _servicioMensajes.Informacion($" {clienteCreateDto.Nombre} agregada." +
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
            ClienteListDto? clienteDto = _bindingSource.
                ObtenerSeleccionado<ClienteListDto>(_servicioMensajes);
            if (clienteDto is null) return;
            if(!_servicioMensajes.Confirmar($"¿Desea borrar el cliente {clienteDto.NombreCompleto}?"))
            {
                return;
            }
            try
            {
                _clienteServicio.Borrar(clienteDto.ClienteId);
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
            ClienteListDto? clienteDto = _bindingSource.
                ObtenerSeleccionado<ClienteListDto>(_servicioMensajes);
            if (clienteDto is null) return;
            ClienteEditDto? clienteEditDto = _clienteServicio.ObtenerParaEditar(clienteDto.ClienteId);
            if (clienteEditDto is null) return;
            using (frmClienteAe frm = new frmClienteAe(_provinciaServicio,
                _ciudadServicio) { Text = "Editar  " })
            {
                frm.SetCliente(clienteEditDto);
                DialogResult dr = frm.ShowDialog();
                if (dr == DialogResult.Cancel) return;
                clienteEditDto = frm.GetCliente();
                if (clienteEditDto is null) return;
                try
                {
                    _clienteServicio.Editar(clienteEditDto);
                    int editadoId = clienteEditDto.ClienteId;
                    bool sePuedeVer = string.IsNullOrWhiteSpace(txtBuscar.Text) ||
                        clienteEditDto.Nombre.ToLower().Contains(txtBuscar.Text.ToLower());

                    if (sePuedeVer)
                    {
                        _estadoNavegacion.PaginaActual = _clienteServicio.ObtenerPaginaRegistro(clienteEditDto.Nombre,
                            _estadoNavegacion.CantidadPorPagina, _estadoNavegacion.FiltroActivo, _estadoNavegacion.TextoBuscar);
                    }
                    RecargarGrilla();
                    if (sePuedeVer)
                    {

                        var editadoTipo = _bindingSource.List
                            .Cast<ClienteListDto>()
                            .FirstOrDefault(tb => tb.ClienteId == editadoId);

                        _bindingSource.Position = _bindingSource.IndexOf(editadoTipo);
                        _servicioMensajes.Informacion("Registro editado");
                    }
                    else
                    {
                        _servicioMensajes.Informacion($" {clienteEditDto.Nombre} editada." +
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
