using Bombones2026.Servicios.DTOs.Rol;
using BombonesApp2026.Servicios.Interfaces;
using System.ComponentModel;

namespace BombonesApp2026.Windows
{
    //TODO:Ver detalles de los roles
    public partial class frmRoles : Form
    {
        private readonly IRolServicio _rolServicio;
        private List<RolListDto>? _listaRoles;
        private BindingSource _bindingSource = new BindingSource();
        public frmRoles(IRolServicio rolServicio)
        {
            InitializeComponent();
            _rolServicio = rolServicio;
        }

        private void tsbCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmRoles_Load(object sender, EventArgs e)
        {
            RecargarGrilla();
        }
        private void RecargarGrilla()
        {
            try
            {
                _listaRoles = _rolServicio.ObtenerTodos();
                MostrarDatosEnGrilla(_listaRoles);

            }
            catch (Exception ex)
            {


                MessageBox.Show(ex.Message,
                 "Error",
                 MessageBoxButtons.OK,
                 MessageBoxIcon.Error);
            }
        }

        private void MostrarDatosEnGrilla(List<RolListDto> listaRoles)
        {
            var bindingList = new BindingList<RolListDto>(listaRoles);
            _bindingSource.DataSource = bindingList;
            dgvDatos.DataSource = _bindingSource;

            lblCantidad.Text = listaRoles.Count.ToString();
        }


        private void tsbNuevo_Click(object sender, EventArgs e)
        {
            using (frmRolesAe frm = new frmRolesAe() { Text = "Nuevo Rol" })
            {
                DialogResult dr = frm.ShowDialog();
                if (dr == DialogResult.Cancel) return;
                RolEditDto? rolEditDto = frm.GetRol();
                if (rolEditDto == null) return;
                try
                {
                    RolCreateDto rolCreateDto = new RolCreateDto
                    {
                        Nombre = rolEditDto.Nombre,
                        Descripcion = rolEditDto.Descripcion,
                    };
                    var nuevoId = _rolServicio.Agregar(rolCreateDto);
                    _listaRoles = _rolServicio.ObtenerTodos();
                    MostrarDatosEnGrilla(_listaRoles);
                    var nuevoRol = _listaRoles.FirstOrDefault(r => r.RolId == nuevoId);
                    if (nuevoRol is null) return;
                    _bindingSource.Position = _bindingSource.IndexOf(nuevoRol);
                    MessageBox.Show("Rol Agregado",
                        "Mensaje", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
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
            if (_bindingSource.Current! == null)
            {
                MessageBox.Show("Debe seleccionar una fila de la grilla",
                    "Advertencia",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }
            RolListDto rolDto = (RolListDto)_bindingSource.Current!;
            DialogResult dr = MessageBox.Show($"¿Desea borrar el rol {rolDto.Nombre}?",
                "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);
            if (dr == DialogResult.No) return;
            try
            {
                _rolServicio.Borrar(rolDto.RolId);
                _listaRoles = _rolServicio.ObtenerTodos();
                MostrarDatosEnGrilla(_listaRoles);
                MessageBox.Show("Rol eliminado",
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
            //Este es el control que no me anduvo
            if (_bindingSource.Current! == null)
            {
                MessageBox.Show("Debe seleccionar una fila de la grilla",
                    "Advertencia",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            RolListDto rolDto = (RolListDto)_bindingSource.Current!;

            int posicion = _bindingSource.Position;//reservo la posición

            RolEditDto? rolEditDto = _rolServicio.ObtenerParaEditar(rolDto.RolId);
            if (rolEditDto is null) return;
            using (frmRolesAe frm = new frmRolesAe() { Text = "Editar Rol" })
            {
                frm.SetRol(rolEditDto);
                DialogResult dr = frm.ShowDialog();
                if (dr == DialogResult.Cancel) return;
                rolEditDto = frm.GetRol();
                if (rolEditDto is null) return;
                try
                {
                    _rolServicio.Editar(rolEditDto);
                    _listaRoles = _rolServicio.ObtenerTodos();
                    MostrarDatosEnGrilla(_listaRoles);
                    //var rolEditado = _listaRoles
                    //    .FirstOrDefault(r => r.RolId == rolEditDto.RolId);
                    _bindingSource.Position = posicion;
                    MessageBox.Show("Rol editado",
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
        }

        private void activosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _listaRoles = _rolServicio.FiltrarPorActivo(true);
            MostrarDatosEnGrilla(_listaRoles);
            ManejarBotones(true);
        }

        private void ManejarBotones(bool filtrado)
        {
            tsbNuevo.Enabled = !filtrado;
            tsbBorrar.Enabled = !filtrado;
            tsbEditar.Enabled = !filtrado;

            tsbFiltrar.BackColor = filtrado ? Color.Orange : SystemColors.Control;
        }

        private void noActivosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _listaRoles = _rolServicio.FiltrarPorActivo(false);
            MostrarDatosEnGrilla(_listaRoles);
            ManejarBotones(true);

        }

        private void tsbActualizar_Click(object sender, EventArgs e)
        {
            RecargarGrilla();
            ManejarBotones(false);
        }
    }
}
