using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using BE;
using BLL;

namespace UI
{
    public partial class frmABMOrdenes : Form
    {
        private readonly ServicioOrden _servicioOrden;
        private List<Orden> _ordenes;

        public frmABMOrdenes(ServicioOrden servicioOrden)
        {
            InitializeComponent();
            _servicioOrden = servicioOrden;
        }

        private async void frmOrden_Load(object sender, EventArgs e)
        {
            await CargarOrdenes();
        }

        private async Task CargarOrdenes()
        {
            _ordenes = await _servicioOrden.BuscarTodosAsync();
            dgvOrdenes.DataSource = null;
            dgvOrdenes.DataSource = _ordenes;
        }

        private async void btnAgregar_Click(object sender, EventArgs e)
        {
            string nombre = Microsoft.VisualBasic.Interaction.InputBox("Ingrese el nombre de la nueva orden:", "Nueva Orden");

            if (string.IsNullOrWhiteSpace(nombre))
                return;

            await _servicioOrden.AgregarOrdenAsync(new Orden { Declaracion = nombre });
            await CargarOrdenes();
        }

        private async void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvOrdenes.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione una orden para editar.");
                return;
            }

            var orden = dgvOrdenes.SelectedRows[0].DataBoundItem as Orden;
            if (orden == null) return;

            string nuevoNombre = Microsoft.VisualBasic.Interaction.InputBox("Editar nombre de la orden:", "Editar Orden", orden.Declaracion);
            if (string.IsNullOrWhiteSpace(nuevoNombre))
                return;

            orden.Declaracion = nuevoNombre;
            await _servicioOrden.ModificarOrdenAsync(orden);
            await CargarOrdenes();
        }

        private async void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvOrdenes.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione una orden para eliminar.");
                return;
            }

            var orden = dgvOrdenes.SelectedRows[0].DataBoundItem as Orden;
            if (orden == null) return;

            if (MessageBox.Show($"¿Eliminar orden '{orden.Declaracion}'?", "Confirmar", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                await _servicioOrden.EliminarOrdenAsync(orden);
                await CargarOrdenes();
            }
        }
    }
}
