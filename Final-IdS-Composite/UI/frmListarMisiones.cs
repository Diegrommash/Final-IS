using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;
using BE;
using System.Drawing;
using BE.Excepciones;

namespace UI
{
    public partial class frmListarMisiones : Form
    {
        private readonly ServicioMision _servicioMision;
        private readonly ServicioRecompensa _servicioRecompensa;

        public frmListarMisiones(ServicioMision servicioMision, ServicioRecompensa servicioRecompensa)
        {
            InitializeComponent();
            _servicioMision = servicioMision;
            _servicioRecompensa = servicioRecompensa;

            string basePath = Application.StartupPath;
            imageListIcons.ColorDepth = ColorDepth.Depth32Bit;
            imageListIcons.ImageSize = new Size(16, 16);

            imageListIcons.Images.Add("simple", Image.FromFile(Path.Combine(basePath, "Imagenes", "Mision_simple.png")));
            imageListIcons.Images.Add("compuesta", Image.FromFile(Path.Combine(basePath, "Imagenes", "Mision_compuesta.png")));
        }

        private async void frmListarMisiones_Load(object sender, EventArgs e)
        {
            await CargarArbol();
        }

        private async Task CargarArbol()
        {
            treeMisiones.Nodes.Clear();
            var arbol = await _servicioMision.ObtenerArbol();
            foreach (var m in arbol)
                treeMisiones.Nodes.Add(CrearNodo(m));
            treeMisiones.ExpandAll();
        }

        private TreeNode CrearNodo(IMision m)
        {
            var dificultad = m.Dificultad > 0 ? $" (Dif: {m.Dificultad})" : "";
            var nodo = new TreeNode($"{m.Nombre}{dificultad} {(m.EstaCompleta ? "[✔]" : "")}")
            {
                Tag = m,
                ImageKey = m.EsCompuesta ? "compuesta" : "simple",
                SelectedImageKey = m.EsCompuesta ? "compuesta" : "simple"
            };

            foreach (var hija in m.Hijas)
                nodo.Nodes.Add(CrearNodo(hija));

            return nodo;
        }


        private async void btnCrear_Click(object sender, EventArgs e)
        {
            using var frm = new frmCrearMision(_servicioMision, _servicioRecompensa);
            if (frm.ShowDialog() == DialogResult.OK)
                await CargarArbol();
        }

        //asignar mision hija
        private async void btnAsignar_Click(object sender, EventArgs e)
        {
            if (treeMisiones.SelectedNode == null)
            {
                MessageBox.Show("Selecciona una misión compuesta como padre.");
                return;
            }

            var padre = treeMisiones.SelectedNode.Tag as IMision;
            
            using var selector = new frmSeleccionarMision(_servicioMision);
            if (selector.ShowDialog() == DialogResult.OK && selector.MisionSeleccionada != null)
            {
                try
                {
                    await _servicioMision.AsignarMision(padre, selector.MisionSeleccionada);
                    MessageBox.Show($"Misión '{selector.MisionSeleccionada.Nombre}' asignada a '{padre.Nombre}'.");
                    await CargarArbol();
                }
                catch (ServicioExcepcion ex)
                {
                    MessageBox.Show(ex.Message, "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        // Quitar misión hija
        private async void btnQuitar_Click(object sender, EventArgs e)
        {
            if (treeMisiones.SelectedNode == null || treeMisiones.SelectedNode.Parent == null)
            {
                MessageBox.Show("Selecciona una misión hija dentro de una misión compuesta.");
                return;
            }

            var nodoHijo = treeMisiones.SelectedNode.Tag as IMision;
            var nodoPadre = treeMisiones.SelectedNode.Parent.Tag as IMision;

            if (nodoHijo == null || nodoPadre == null)
            {
                MessageBox.Show("Error: no se pudo obtener la misión seleccionada.");
                return;
            }

            if (MessageBox.Show($"¿Quitar la misión '{nodoHijo.Nombre}' de '{nodoPadre.Nombre}'?",
                                "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    await _servicioMision.QuitarMision(nodoPadre, nodoHijo);
                    MessageBox.Show("Misión hija quitada correctamente.");
                    await CargarArbol();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al quitar misión hija: " + ex.Message);
                }
            }
        }



        private async void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (treeMisiones.SelectedNode == null) return;

                var mision = treeMisiones.SelectedNode.Tag as IMision;
                if (mision == null) return;

                if (MessageBox.Show($"¿Eliminar misión '{mision.Nombre}'?", "Confirmar", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    await _servicioMision.Eliminar(mision);
                    await CargarArbol();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar misión: " + ex.Message);
            }
        }

        private async void btnCompletar_Click(object sender, EventArgs e)
        {
            try
            {
                if (treeMisiones.SelectedNode == null) return;

                var mision = treeMisiones.SelectedNode.Tag as IMision;
                if (mision == null) return;

                await _servicioMision.CompletarMision(mision);
                MessageBox.Show("Misión marcada como completada.");
                await CargarArbol();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al completar misión: " + ex.Message);
            }
        }

        private async void btnRefrescar_Click(object sender, EventArgs e)
        {
            await CargarArbol();
        }

        private void treeMisiones_MouseMove(object sender, MouseEventArgs e)
        {
            TreeNode nodoActual = treeMisiones.GetNodeAt(e.Location);

            if (nodoActual == null || nodoActual.Tag is not IMision mision)
            {
                toolTipMision.Hide(treeMisiones);
                ultimoNodoConTooltip = null;
                return;
            }

            if (nodoActual != ultimoNodoConTooltip)
            {
                var recompensas = mision.ObtenerRecompensas();
                string texto = recompensas.Count > 0
                    ? "Recompensas: " + string.Join(", ", recompensas.ConvertAll(r => r.Nombre))
                    : "Sin recompensas";

                toolTipMision.SetToolTip(treeMisiones, texto);
                ultimoNodoConTooltip = nodoActual;
            }
        }

    }
}
