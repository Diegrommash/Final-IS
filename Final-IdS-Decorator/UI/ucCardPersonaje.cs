using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BE;
using BLL;
using BLL.Abstracciones;
using BLL.Decoradores;

namespace UI.Controles
{
    public partial class ucCardPersonaje : UserControl
    {
        public IComponente Personaje { get; private set; }

        public event EventHandler<IComponente>? OnModificarPersonaje;
        public event EventHandler<IComponente>? OnEliminarPersonaje;

        public ucCardPersonaje()
        {
            InitializeComponent();
        }

        public void CargarPersonaje(IComponente personaje)
        {
            Personaje = personaje;

            lblNombrePersonaje.Text = ServicioPersonaje.ObtenerPersonajeBase(personaje).Nombre;

            lstItems.Items.Clear();
            if (personaje == null) return;

            string[] lineas = personaje.ObtenerDescripcion().Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var linea in lineas)
                lstItems.Items.Add(linea.Trim());

            lblPoder.Text = $"Poder total: {personaje.ObtenerPoder()}";
            lblDefensa.Text = $"Defensa total: {personaje.ObtenerDefensa()}";
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            OnModificarPersonaje?.Invoke(this, Personaje);
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            OnEliminarPersonaje?.Invoke(this, Personaje);
        }
    }
}
