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
        public frmMainObservable()
        {
            InitializeComponent();
            _servicioOrden = new ServicioOrden();
            _servicioItem = new ServicioItem();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmABMOrden formOrden = new frmABMOrden(_servicioOrden, _servicioItem);
            formOrden.Show();
        }
    }
}
