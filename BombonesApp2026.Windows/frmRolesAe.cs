using Bombones2026.Servicios.DTOs.Rol;

namespace BombonesApp2026.Windows
{
    public partial class frmRolesAe : Form
    {
        private RolEditDto? rolDto;
        public frmRolesAe()
        {
            InitializeComponent();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ValidarDatos())
            {
                if(rolDto is null)
                {
                    rolDto = new RolEditDto();
                }
                rolDto.Nombre = txtRol.Text;
                rolDto.Descripcion = txtDescripcion.Text;
                rolDto.Activo=chkActivo.Checked;
                DialogResult = DialogResult.OK;
            }
        }

        private bool ValidarDatos()
        {
            bool valido = true;
            errorProvider1.Clear();
            if (string.IsNullOrEmpty(txtRol.Text))
            {
                valido = false;
                errorProvider1.SetError(txtRol, "El rol es obligatorio");
            }
            return valido;
        }

        private void frmRolesAe_Load(object sender, EventArgs e)
        {
        }
        public RolEditDto? GetRol()
        {
            return rolDto;
        }

        public void SetRol(RolEditDto rolEditDto)
        {
            rolDto = rolEditDto;
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (rolDto is not null)
            {
                txtRol.Text = rolDto.Nombre;
                txtDescripcion.Text = rolDto.Descripcion;
                chkActivo.Checked=rolDto.Activo;
            }
            else
            {
                chkActivo.Checked = true;
                chkActivo.Enabled = false;

            }
        }
    }
}
