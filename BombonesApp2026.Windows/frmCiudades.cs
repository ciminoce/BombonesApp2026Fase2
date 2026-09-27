using Bombones2026.Servicios.DTOs.Ciudad;
using Bombones2026.Servicios.DTOs.Paginacion;
using BombonesApp2026.Servicios.Interfaces;

namespace BombonesApp2026.Windows
{
    //TODO: Modificar el filtrado de ciduidades
    //TODO: Método para cargar las provincias
    //TODO: ver cuando se carga el combo de provincias en el load
    //TODO: Corregir la condición del sePuedeVer!!!
    public partial class frmCiudades : Form
    {
        private readonly ICiudadServicio _ciudadServicio;
        private readonly IProvinciaServicio _provinciaServicio;
        private BindingSource _bindingSource = new BindingSource();
        //para paginar
        private int paginaActual = 1;
        private int cantidadPorPagina = 10;
        private int totalRegistros = 0;
        private int totalPaginas = 0;

        private bool? filtroActivo = null;
        private string? textoBuscar = null;
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
                var resultado = _ciudadServicio.ObtenerPagina(paginaActual, cantidadPorPagina, filtroActivo, textoBuscar);
                MostrarDatosEnGrilla(resultado);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void MostrarDatosEnGrilla(ResultadoPaginacionDto<CiudadListDto> resultado)
        {
            totalPaginas = resultado.TotalPaginas;
            totalRegistros = resultado.TotalRegistros;
            int desde = 1 + (paginaActual - 1) * cantidadPorPagina;
            int hasta = desde + cantidadPorPagina - 1;//OJO acá!!
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
                        paginaActual = _ciudadServicio
                            .ObtenerPaginaRegistro(ciudadCreateDto.Nombre, cantidadPorPagina,
                            filtroActivo, textoBuscar);

                    }
                    RecargarGrilla();
                    if (sePuedeVer)
                    {
                        var nuevoTipo = _bindingSource.List
                            .Cast<CiudadListDto>()
                            .FirstOrDefault(tb => tb.CiudadId == nuevoId);
                        if (nuevoTipo is null) return;
                        _bindingSource.Position = _bindingSource.IndexOf(nuevoTipo);
                        MessageBox.Show("Ciudad Agregada",
                            "Mensaje", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                    }
                    else
                    {
                        MessageBox.Show($"Ciudad {ciudadCreateDto.Nombre} agregada.\nNo se muestra por condición de filtrado o búsqueda",
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

        private void tsbCerrar_Click(object sender, EventArgs e)
        {
            Close();
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

            CiudadListDto ciudadDto = (CiudadListDto)_bindingSource.Current!;
            DialogResult dr = MessageBox.Show($"¿Desea borrar la ciudad {ciudadDto.Ciudad}?",
                "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);
            if (dr == DialogResult.No) return;
            try
            {
                _ciudadServicio.Borrar(ciudadDto.CiudadId);
                RecargarGrilla();
                if (paginaActual > totalPaginas && totalPaginas > 0)
                {
                    paginaActual = totalPaginas;
                    RecargarGrilla();
                }
                MessageBox.Show("Ciudad eliminada",
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

            CiudadListDto ciudadDto = (CiudadListDto)_bindingSource.Current!;
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
                        paginaActual = _ciudadServicio.ObtenerPaginaRegistro(ciudadEditDto.Nombre,
                            cantidadPorPagina, filtroActivo, textoBuscar);
                    }
                    RecargarGrilla();
                    if (sePuedeVer)
                    {

                        var editadoTipo = _bindingSource.List
                            .Cast<CiudadListDto>()
                            .FirstOrDefault(tb => tb.CiudadId == editadoId);

                        _bindingSource.Position = _bindingSource.IndexOf(editadoTipo);
                        MessageBox.Show("Ciudad editada",
                            "Mensaje",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show($"Ciudad {ciudadEditDto.Nombre} editada.\nNo se muestra por condición de filtrado o búsqueda",
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

        private void tsbActualizar_Click(object sender, EventArgs e)
        {
            paginaActual = 1;
            filtroActivo = null;
            textoBuscar = null;
            txtBuscar.Clear();
            tsbBuscar.BackColor = SystemColors.Control;
            RecargarGrilla();
        }
    }
}
