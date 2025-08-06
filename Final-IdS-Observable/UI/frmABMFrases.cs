
using BE;
using BE.Excepciones;
using BLL;
using Microsoft.Data.SqlClient;

namespace UI
{
    public partial class frmABMFrases : Form
    {
        private readonly ServicioOrden _servicioOrden;
        private readonly ServicioItem _servicioItem;
        private readonly ServicioOTF _servicioOTF;

        private bool _modoEdicion = false;
        private OrdenTrabajoFrase? _relacionEnEdicion = null;

        public frmABMFrases(
            ServicioOrden servicioOrden,
            ServicioItem servicioItem,
            ServicioOTF servicioOTF)
        {
            InitializeComponent();
            _servicioOrden = servicioOrden;
            _servicioItem = servicioItem;
            _servicioOTF = servicioOTF;
        }

        private async void frmABMOrden_Load(object sender, EventArgs e)
        {
            await CargarComboOrden();
            await CargarComboItem();
            await CargarRelaciones();
        }

        private async Task CargarComboOrden()
        {
            var ordenes = await _servicioOrden.BuscarTodosAsync();
            cmbOrden.DataSource = ordenes;
            cmbOrden.DisplayMember = "Declaracion";
            cmbOrden.ValueMember = "Id";
            cmbOrden.SelectedValueChanged += (s, e) =>
            {
                _modoEdicion = false;
                _relacionEnEdicion = null;
                btnAgregar.Text = "Agregar";
            };
        }

        private async Task CargarComboItem()
        {
            var items = await _servicioItem.BuscarTodos();
            cmbItem.DataSource = items.Where(i => i.Tipo == BE.Enums.TipoDecoradorEnum.Trabajo).ToList();
            cmbItem.DisplayMember = "Nombre";
            cmbItem.ValueMember = "Id";
            cmbItem.SelectedValueChanged += (s, e) =>
            {
                _modoEdicion = false;
                _relacionEnEdicion = null;
                btnAgregar.Text = "Agregar";
            };
        }

        private async Task CargarRelaciones()
        {
            var relaciones = await _servicioOTF.BuscarTodosAsync();
            dgvRelaciones.DataSource = relaciones;
            dgvRelaciones.Columns.Clear();
            dgvRelaciones.AutoGenerateColumns = false;

            dgvRelaciones.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ObtenerOrden",
                HeaderText = "Orden",
                Width = 200
            });

            dgvRelaciones.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ObtenerTrabajo",
                HeaderText = "Tipo de trabajo",
                Width = 200
            });

            dgvRelaciones.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Frase",
                HeaderText = "Frase",
                Width = 220
            });

            var colEliminar = new DataGridViewButtonColumn
            {
                Name = "Acciones",
                HeaderText = "Acciones",
                Text = "Eliminar",
                UseColumnTextForButtonValue = true
            };
            dgvRelaciones.Columns.Add(colEliminar);
        }

        private async void btnAgregar_Click(object sender, EventArgs e)
        {
            if (cmbOrden.SelectedItem == null || cmbItem.SelectedItem == null || string.IsNullOrWhiteSpace(txtFrase.Text))
            {
                MessageBox.Show("Completá todos los campos.");
                return;
            }

            try
            {
                var orden = (Orden)cmbOrden.SelectedItem;
                var tipoTrabajo = (Item)cmbItem.SelectedItem;
                var frase = txtFrase.Text.Trim();

                if (_modoEdicion && _relacionEnEdicion != null)
                {
                    _relacionEnEdicion.Orden = orden;
                    _relacionEnEdicion.Trabajo = tipoTrabajo;
                    _relacionEnEdicion.Frase = frase;

                    await _servicioOTF.ModificarOTFAsync(_relacionEnEdicion);
                    MessageBox.Show("Relación modificada con éxito.");
                }
                else
                {
                    var nueva = new OrdenTrabajoFrase
                    {
                        Orden = orden,
                        Trabajo = tipoTrabajo,
                        Frase = frase
                    };

                    await _servicioOTF.AgregarOTFAsync(nueva);
                    MessageBox.Show("Relación agregada con éxito.");
                }
                await CargarRelaciones();
                LimpiarFormulario();
            }
            catch (ServicioExcepcion ex)
            {
                MessageBox.Show(ex.Message, "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void dgvRelaciones_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvRelaciones.Columns[e.ColumnIndex].Name == "Acciones")
            {
                if (dgvRelaciones.Rows[e.RowIndex].DataBoundItem is OrdenTrabajoFrase filaDGV)
                {
                    var confirm = MessageBox.Show(
                        $"¿Eliminar la relación '{filaDGV.ObtenerOrden}' - '{filaDGV.ObtenerTrabajo}'?",
                        "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (confirm == DialogResult.Yes)
                    {
                        await _servicioOTF.EliminarOTFAsync(filaDGV); 
                        await CargarRelaciones();
                    }
                }
            }
        }

        private void dgvRelaciones_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var fila = dgvRelaciones.Rows[e.RowIndex].DataBoundItem as OrdenTrabajoFrase;
            if (fila == null) return;

            cmbOrden.SelectedValue = fila.Orden.Id;
            cmbItem.SelectedValue = fila.Trabajo.Id;
            txtFrase.Text = fila.Frase;

            _modoEdicion = true;
            _relacionEnEdicion = fila;

            btnAgregar.Text = "Modificar";
        }

        private void LimpiarFormulario()
        {
            cmbOrden.SelectedIndex = -1;
            cmbItem.SelectedIndex = -1;
            txtFrase.Clear();

            _modoEdicion = false;
            _relacionEnEdicion = null;
            btnAgregar.Text = "Agregar";
        }
    }
}

