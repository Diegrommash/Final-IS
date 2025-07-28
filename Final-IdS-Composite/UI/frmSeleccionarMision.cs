using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using BE;
using BLL;

namespace UI
{
    public partial class frmSeleccionarMision : Form
    {
        private readonly ServicioMision _servicio;
        private List<IMision> _misionesDisponibles;
        private readonly ImageList _imageList;

        public IMision? MisionSeleccionada { get; private set; }

        public frmSeleccionarMision(ServicioMision servicio)
        {
            InitializeComponent();
            _servicio = servicio;

            _imageList = new ImageList { ImageSize = new Size(16, 16), ColorDepth = ColorDepth.Depth32Bit };

            string basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Imagenes");
            _imageList.Images.Add("simple", Image.FromFile(Path.Combine(basePath, "Mision_simple.png")));
            _imageList.Images.Add("compuesta", Image.FromFile(Path.Combine(basePath, "Mision_compuesta.png")));

            lstMisiones.View = View.List;
            lstMisiones.SmallImageList = _imageList;
        }

        private async void frmSeleccionarMision_Load(object sender, EventArgs e)
        {
            _misionesDisponibles = await _servicio.ObtenerArbol();
            lstMisiones.Items.Clear();

            foreach (var mision in _misionesDisponibles)
            {
                string key = mision.EsCompuesta ? "compuesta" : "simple";
                var item = new ListViewItem(mision.Nombre) { ImageKey = key, Tag = mision };
                lstMisiones.Items.Add(item);
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (lstMisiones.SelectedItems.Count > 0)
            {
                MisionSeleccionada = lstMisiones.SelectedItems[0].Tag as IMision;
                DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("Seleccione una misión.");
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e) => DialogResult = DialogResult.Cancel;
    }
}
