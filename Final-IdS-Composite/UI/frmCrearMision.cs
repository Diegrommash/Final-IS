using System;
using System.Windows.Forms;
using BLL;
using BE;
using BE.Excepciones;

namespace UI
{
    public partial class frmCrearMision : Form
    {
        private readonly ServicioMision _servicioMision;
        private readonly ServicioRecompensa _servicioRecompensa;

        private IList<Item> _itemsDisponibles;
        private List<Item> _recompensasSeleccionadas = new();

        public frmCrearMision(ServicioMision servicioMision, ServicioRecompensa servicioRecompensa)
        {
            InitializeComponent();
            _servicioMision = servicioMision;
            _servicioRecompensa = servicioRecompensa;
        }

        private async void frmCrearMision_Load(object sender, EventArgs e)
        {
            nudDificultad.Minimum = 1;
            nudDificultad.Maximum = 10;

            chkEsCompuesta.CheckedChanged += (s, ev) =>
            {
                lblDificultad.Visible = nudDificultad.Visible = !chkEsCompuesta.Checked;
            };

            _itemsDisponibles = await _servicioRecompensa.BuscarRecompensas(); // este método deberías tenerlo en tu servicio

            cboRecompensas.DataSource = _itemsDisponibles;
            cboRecompensas.DisplayMember = "Nombre"; // suponiendo que la clase Item tiene esta propiedad
            cboRecompensas.ValueMember = "Id";
        }

        private async void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                string nombre = txtNombre.Text.Trim();
                string descripcion = txtDescripcion.Text.Trim();

                if (chkEsCompuesta.Checked)
                {
                    int id = await _servicioMision.CrearMisionCompuesta(nombre, descripcion, _recompensasSeleccionadas);
                    MessageBox.Show($"✅ Misión compuesta creada con exito", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    int dificultad = (int)nudDificultad.Value;
                    int id = await _servicioMision.CrearMisionSimple(nombre, descripcion, dificultad, _recompensasSeleccionadas);
                    MessageBox.Show($"✅ Misión simple creada con exito", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Error de Negocio", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (ServicioExcepcion ex)
            {
                MessageBox.Show(ex.Message, "Error en Servicio", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error inesperado: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnAgregarRecompensa_Click(object sender, EventArgs e)
        {
            if (cboRecompensas.SelectedItem is Item itemSeleccionado &&
                !_recompensasSeleccionadas.Any(i => i.Id == itemSeleccionado.Id))
            {
                _recompensasSeleccionadas.Add(itemSeleccionado);
                lstRecompensas.DataSource = null;
                lstRecompensas.DataSource = _recompensasSeleccionadas;
                lstRecompensas.DisplayMember = "Nombre";
            }
            else
            {
                MessageBox.Show("El ítem ya fue agregado como recompensa.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

    }
}
