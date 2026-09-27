using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BombonesApp2026.Windows
{
    public partial class frmDetalleBombon : Form
    {

        public frmDetalleBombon()
        {
            InitializeComponent();
        }

        public void SetDescripcion(string? descripcion)
        {
            txtDescripcion.Text = descripcion;
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
