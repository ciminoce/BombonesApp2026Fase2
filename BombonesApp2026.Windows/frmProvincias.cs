using Bombones2026.Servicios.DTOs.Paginacion;
using Bombones2026.Servicios.DTOs.Provincia;
using BombonesApp2026.Servicios.Interfaces;

namespace BombonesApp2026.Windows
{
    //TODO: Buscar Provincia
    public partial class frmProvincias : Form
    {
        private readonly IProvinciaServicio _provinciaServicio;
        private BindingSource _bindingSource = new BindingSource();
        //para paginar
        private int paginaActual = 1;
        private int cantidadPorPagina = 10;
        private int totalRegistros = 0;
        private int totalPaginas = 0;
        //para filtrar
        private bool? filtroActivo = null;
        private string? textoBuscar = null;
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
                    bool sePuedeVer = (filtroActivo is null || filtroActivo == true) &&
                        (string.IsNullOrWhiteSpace(txtBuscar.Text) ||
                        provinciaCreateDto.Nombre.Contains(txtBuscar.Text));
                    if (sePuedeVer)
                    {
                        paginaActual = _provinciaServicio
                            .ObtenerPaginaRegistro(provinciaCreateDto.Nombre, cantidadPorPagina,
                            filtroActivo, textoBuscar);

                    }
                    RecargarGrilla();
                    if (sePuedeVer)
                    {
                        var nuevoTipo = _bindingSource.List
                            .Cast<ProvinciaListDto>()
                            .FirstOrDefault(tb => tb.ProvinciaId == nuevoId);
                        if (nuevoTipo is null) return;
                        _bindingSource.Position = _bindingSource.IndexOf(nuevoTipo);
                        MessageBox.Show("Provincia Agregada",
                            "Mensaje", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                    }
                    else
                    {
                        MessageBox.Show($"Provincia {provinciaEditDto.Nombre} agregada.\nNo se muestra por condición de filtrado o búsqueda",
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
            ProvinciaListDto provinciaDto = (ProvinciaListDto)_bindingSource.Current!;
            DialogResult dr = MessageBox.Show($"¿Desea borrar la provincia {provinciaDto.Nombre}?",
                "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);
            if (dr == DialogResult.No) return;
            try
            {
                _provinciaServicio.Borrar(provinciaDto.ProvinciaId);
                RecargarGrilla();
                if (paginaActual > totalPaginas && totalPaginas > 0)
                {
                    paginaActual = totalPaginas;
                    RecargarGrilla();
                }
                MessageBox.Show("Provincia eliminada",
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
            ProvinciaListDto provinciaDto = (ProvinciaListDto)_bindingSource.Current!;
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
                    bool sePuedeVer = (filtroActivo is null || filtroActivo == true) &&
                        (string.IsNullOrWhiteSpace(txtBuscar.Text) ||
                        provinciaEditDto.Nombre.ToLower().Contains(txtBuscar.Text.ToLower()));

                    if (sePuedeVer)
                    {
                        paginaActual = _provinciaServicio.ObtenerPaginaRegistro(provinciaEditDto.Nombre,
                            cantidadPorPagina, filtroActivo, textoBuscar);
                    }
                    RecargarGrilla();
                    if (sePuedeVer)
                    {

                        var editadoTipo = _bindingSource.List
                            .Cast<ProvinciaListDto>()
                            .FirstOrDefault(tb => tb.ProvinciaId == editadoId);

                        _bindingSource.Position = _bindingSource.IndexOf(editadoTipo);
                        MessageBox.Show("Provincia editada",
                            "Mensaje",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show($"Provincia {provinciaEditDto.Nombre} editada.\nNo se muestra por condición de filtrado o búsqueda",
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

        private void frmProvincias_Load(object sender, EventArgs e)
        {
            RecargarGrilla();
        }

        private void RecargarGrilla()
        {
            try
            {
                var resultado = _provinciaServicio.ObtenerPagina(paginaActual,
                    cantidadPorPagina, filtroActivo, textoBuscar);
                MostrarDatosEnGrilla(resultado);

            }
            catch (Exception ex)
            {


                MessageBox.Show(ex.Message,
                 "Error",
                 MessageBoxButtons.OK,
                 MessageBoxIcon.Error);
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
                return;
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
