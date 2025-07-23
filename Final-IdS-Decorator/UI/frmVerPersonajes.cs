using BE;
using BLL;
using BLL.Abstracciones;
using Servicios;
using UI.Controles;

namespace UI
{
    public partial class frmVerPersonajes : Form
    {
        private readonly ServicioPersonaje _servicioPersonaje;

        public frmVerPersonajes()
        {
            InitializeComponent();
            _servicioPersonaje = new ServicioPersonaje();
        }

        private async void frmMostrarPersonajes_Load(object sender, EventArgs e)
        {
            await CargarPersonajesAsync();
        }

        private async Task CargarPersonajesAsync()
        {
            flpCards.Controls.Clear();

            var personajes = await _servicioPersonaje.BuscarPersonajes(SesionJugador.JugadorActual);
            foreach (var personaje in personajes)
            {
                var card = new ucCardPersonaje();
                card.CargarPersonaje(personaje);
                card.OnModificarPersonaje += Card_OnModificarPersonaje;
                flpCards.Controls.Add(card);
            }
        }

        private async void Card_OnModificarPersonaje(object? sender, IComponente personaje)
        {
            using var frm = new frmCrearPersonaje(personaje);
            var resultado = frm.ShowDialog();

            if (resultado == DialogResult.OK)
                await CargarPersonajesAsync();
        }
    }
}
