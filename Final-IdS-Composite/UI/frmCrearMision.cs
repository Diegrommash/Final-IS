using System;
using System.Windows.Forms;
using BLL;
using BE;
using BE.Excepciones;

namespace UI
{
    public partial class frmCrearMision : Form
    {
        private readonly ServicioMision _servicio;

        public frmCrearMision(ServicioMision servicio)
        {
            InitializeComponent();
            _servicio = servicio;
        }

        private void frmCrearMision_Load(object sender, EventArgs e)
        {
            nudDificultad.Minimum = 1;
            nudDificultad.Maximum = 10;

            chkEsCompuesta.CheckedChanged += (s, ev) =>
            {
                lblDificultad.Visible = nudDificultad.Visible = !chkEsCompuesta.Checked;
            };
        }

        private async void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                string nombre = txtNombre.Text.Trim();
                string descripcion = txtDescripcion.Text.Trim();

                if (chkEsCompuesta.Checked)
                {
                    int id = await _servicio.CrearMisionCompuesta(nombre, descripcion);
                    MessageBox.Show($"✅ Misión compuesta creada con exito", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    int dificultad = (int)nudDificultad.Value;
                    int id = await _servicio.CrearMisionSimple(nombre, descripcion, dificultad);
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
    }
}
