using Microsoft.Extensions.DependencyInjection;

namespace BombonesApp2026.Windows
{
    public partial class frmPrincipal : Form
    {
        private readonly IServiceScopeFactory _scopeFactory;
        public frmPrincipal(IServiceScopeFactory scopeFactory)
        {
            InitializeComponent();
            _scopeFactory = scopeFactory;
        }
        public void AbrirFormulario<TForm>(string titulo) where TForm : Form
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                using (var frm = scope.ServiceProvider
                    .GetRequiredService<TForm>())
                {
                    frm.Text = titulo;
                    frm.ShowDialog();
                }
            }
        }
        private void btnRoles_Click(object sender, EventArgs e)
        {
            AbrirFormulario<frmRoles>("Listado de Roles");
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnTiposBombones_Click(object sender, EventArgs e)
        {
            AbrirFormulario<frmTiposDeBombones>("Listado de Tipos de Bombones");

        }

        private void btnFormaDePago_Click(object sender, EventArgs e)
        {
            AbrirFormulario<frmFormasDePago>("Listado de Formas de Pago");
        }

        private void btnProvincias_Click(object sender, EventArgs e)
        {
            AbrirFormulario<frmProvincias>("Listado de Provincias");
        }

        private void btnCiudades_Click(object sender, EventArgs e)
        {
            AbrirFormulario<frmCiudades>("Listado de Ciudades");
        }

        private void btnTransportes_Click(object sender, EventArgs e)
        {
            AbrirFormulario<frmTransportes>("Listado de Transportes");
        }

        private void btnBombones_Click(object sender, EventArgs e)
        {
            AbrirFormulario<frmBombones>("Listado de Bombones");
        }

        private void btnClientes_Click(object sender, EventArgs e)
        {
            AbrirFormulario<frmClientes>("Listado de Clientes");
        }

        private void btnCajas_Click(object sender, EventArgs e)
        {
            AbrirFormulario<frmCajas>("Listado de Cajas");
        }
    }
}
