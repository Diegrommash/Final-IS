using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;

namespace UI
{
    public partial class frmMainObservable : Form
    {
        private readonly ServicioOrden _servicioOrden;
        private readonly ServicioItem _servicioItem;
        private readonly ServicioOTF _servicioOTF;
        public frmMainObservable()
        {
            InitializeComponent();
            _servicioOrden = new ServicioOrden();
            _servicioItem = new ServicioItem();
            _servicioOTF = new ServicioOTF();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmABMFrases frmFrases = new frmABMFrases(_servicioOrden, _servicioItem, _servicioOTF);
            frmFrases.Show();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            frmABMOrdenes frmFrases = new frmABMOrdenes(_servicioOrden);
            frmFrases.Show();
        }

        private void personajesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmPersonajes frmPersonajes = new frmPersonajes();
            frmPersonajes.Show();
        }
    }
}
