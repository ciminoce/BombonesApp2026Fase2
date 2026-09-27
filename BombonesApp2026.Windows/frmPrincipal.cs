using Bombones2026.Servicios.Servicios;
using BombonesApp2026.Servicios.Interfaces;
using BombonesApp2026.Servicios.Servicios;

namespace BombonesApp2026.Windows
{
    public partial class frmPrincipal : Form
    {
        private readonly ITipoBombonServicio _tipoBombonServicio;
        private readonly IFormaDePagoServicio _formaDePagoServicio;
        private readonly IRolServicio _rolServicio;
        private readonly IProvinciaServicio _provinciaServicio;
        private readonly ICiudadServicio _ciudadServicio;
        private readonly ITransporteServicio _transporteServicio;
        private readonly IBombonServicio _bombonServicio;
        private readonly IClienteServicio _clienteServicio;
        private readonly ICajaServicio _cajaServicio;
        public frmPrincipal(ITipoBombonServicio tipoBombonServicio,

            IFormaDePagoServicio formaDePagoServicio,
            IRolServicio rolServicio,
            IProvinciaServicio provinciaServicio,
            ICiudadServicio ciudadServicio,
            ITransporteServicio transporteServicio,
            IBombonServicio bombonServicio,
            IClienteServicio clienteServicio,
            ICajaServicio cajaServicio)
        {
            InitializeComponent();
            _tipoBombonServicio = tipoBombonServicio;
            _formaDePagoServicio = formaDePagoServicio;
            _rolServicio = rolServicio;
            _provinciaServicio = provinciaServicio;
            _ciudadServicio = ciudadServicio;
            _transporteServicio = transporteServicio;
            _bombonServicio = bombonServicio;
            _clienteServicio = clienteServicio;
            _cajaServicio = cajaServicio;
        }

        private void btnRoles_Click(object sender, EventArgs e)
        {
            using (frmRoles frm = new frmRoles(_rolServicio) { Text = "Listado de Roles" })
            {
                frm.ShowDialog();
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnTiposBombones_Click(object sender, EventArgs e)
        {
            using (frmTiposDeBombones frm = new frmTiposDeBombones(_tipoBombonServicio) { Text = "Listado de Tipos de Bombones" })
            {
                frm.ShowDialog();
            }

        }

        private void btnFormaDePago_Click(object sender, EventArgs e)
        {
            using (frmFormasDePago frm = new frmFormasDePago(_formaDePagoServicio) { Text = "Listado de Formas de Pago" })
            {
                frm.ShowDialog();
            }

        }

        private void btnProvincias_Click(object sender, EventArgs e)
        {
            using (frmProvincias frm = new frmProvincias(_provinciaServicio) { Text = "Listado de Provincias" })
            {
                frm.ShowDialog();
            }
        }

        private void btnCiudades_Click(object sender, EventArgs e)
        {
            using (frmCiudades frm = new frmCiudades(_ciudadServicio, _provinciaServicio) { Text = "Listado de Ciudades" })
            {
                frm.ShowDialog();
            }
        }

        private void btnTransportes_Click(object sender, EventArgs e)
        {
            using (frmTransportes frm = new frmTransportes(_transporteServicio, _provinciaServicio) { Text = "Listado de Transportes" })
            {
                frm.ShowDialog();
            }
        }

        private void btnBombones_Click(object sender, EventArgs e)
        {
            using (frmBombones frm = new frmBombones(_bombonServicio, _tipoBombonServicio) { Text = "Listado de Bombones" })
            {
                frm.ShowDialog();

            }
        }

        private void btnClientes_Click(object sender, EventArgs e)
        {
            using (frmClientes frm = new frmClientes(_clienteServicio, _provinciaServicio, _ciudadServicio) { Text = "Listado de Clientes" })
            {
                frm.ShowDialog();
            }
        }

        private void btnCajas_Click(object sender, EventArgs e)
        {
            using (frmCajas frm = new frmCajas(_cajaServicio) { Text = "Listado de Cajas" }) 
            {
                frm.ShowDialog();
            }
        }
    }
}
