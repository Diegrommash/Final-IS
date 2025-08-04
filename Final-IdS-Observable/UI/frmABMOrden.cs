
using BE;
using BLL;

namespace UI
{
    public partial class frmABMOrden : Form
    {
        private readonly ServicioOrden _servicioOrden;
        private readonly ServicioItem _servicioItem;


        public frmABMOrden(
            ServicioOrden servicioOrden,
            ServicioItem servicioItem
)
        {
            InitializeComponent();
            _servicioOrden = servicioOrden;
            _servicioItem = servicioItem;
        }

        private async void frmOrdenRelacion_Load(object sender, EventArgs e)
        {
            await CargarComboOrden();
            await CargarComboItem();
            await CargarRelaciones();
        }

        private async Task CargarComboOrden()
        {
            //var ordenes = await _servicioOrden.ObtenerTodosAsync();
            //cmbOrden.DataSource = ordenes;
            //cmbOrden.DisplayMember = "Declaracion";
            //cmbOrden.ValueMember = "Id";
        }

        private async Task CargarComboItem()
        {
            var items = await _servicioItem.BuscarTodos();
            cmbItem.DataSource = items;
            cmbItem.DisplayMember = "Nombre";
            cmbItem.ValueMember = "Id";
        }

        private async Task CargarRelaciones()
        {
            //var relaciones = await _relacionService.ObtenerTodasConNombresAsync();
            //dgvRelaciones.DataSource = relaciones;
            dgvRelaciones.Columns.Clear();
            dgvRelaciones.AutoGenerateColumns = false;

            dgvRelaciones.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Orden",
                HeaderText = "Orden",
                Width = 200
            });

            dgvRelaciones.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Item",
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

            var ordenId = ((Orden)cmbOrden.SelectedItem).Id;
            var tipoTrabajoId = ((Item)cmbItem.SelectedItem).Id;
            var frase = txtFrase.Text.Trim();

            var relacion = new OrdenTrabajoFrase
            {
                //OrdenId = ordenId,
                //TipoTrabajoId = tipoTrabajoId,
                //Frase = frase
            };

            //await _relacionService.AgregarAsync(relacion);
            await CargarRelaciones();
            txtFrase.Clear();
        }

        private async void dgvRelaciones_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dgvRelaciones.Columns["Acciones"].Index)
            {
                var fila = dgvRelaciones.Rows[e.RowIndex].DataBoundItem as OrdenTrabajoFrase;

                if (fila != null)
                {
                    var confirm = MessageBox.Show("¿Eliminar esta relación?", "Confirmar", MessageBoxButtons.YesNo);
                    if (confirm == DialogResult.Yes)
                    {
                       // await _relacionService.EliminarAsync(fila.OrdenId, fila.TipoTrabajoId);
                        await CargarRelaciones();
                    }
                }
            }
        }
    }

}

