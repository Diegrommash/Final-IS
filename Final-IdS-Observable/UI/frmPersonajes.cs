using BLL.Abstracciones;
using BLL;
using Servicios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI
{
    public partial class frmPersonajes : Form
    {
        private readonly ServicioPersonaje _servicioPersonaje;
        private List<IComponente> _personajes;

        public frmPersonajes()
        {
            InitializeComponent();
            _servicioPersonaje = new ServicioPersonaje();
            this.Load += FrmPersonajes_Load;
        }

        private async void FrmPersonajes_Load(object sender, EventArgs e)
        {
            await CargarPersonajes();
        }

        private async Task CargarPersonajes()
        {
            // 🔹 Trae los personajes del servicio
            _personajes = await _servicioPersonaje.BuscarPersonajes(SesionJugador.JugadorActual);

            // 🔹 Limpia cards existentes
            flpCards.Controls.Clear();

            // 🔹 Crea una card por cada personaje
            foreach (var pj in _personajes)
            {
                var card = new ucCardPersonaje
                {
                    Width = 220,
                    Height = 120,
                    Margin = new Padding(10)
                };

                // 🔹 Setea datos
                card.SetPersonaje(pj);

                // 🔹 (Opcional) Agregar evento click
                card.Click += (s, e) =>
                    MessageBox.Show($"Personaje: {pj.ObtenerDescripcion()}", "Detalle");

                flpCards.Controls.Add(card);
            }
        }
    }
}
