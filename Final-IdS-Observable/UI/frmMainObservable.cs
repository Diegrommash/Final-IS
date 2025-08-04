
using Servicios;

namespace UI
{
    public partial class frmMainObservable : Form
    {


        public frmMainObservable()
        {
            InitializeComponent();
        }

        private async void frmMostrarPersonajes_Load(object sender, EventArgs e)
        {
            await CargarPersonajesAsync();
        }

        private async Task CargarPersonajesAsync()
        {
            flpCards.Controls.Clear();

            //var personajes = await _servicioPersonaje.BuscarPersonajes(SesionJugador.JugadorActual);
            //foreach (var personaje in personajes)
            //{
            //    var card = new ucCardPersonaje();
            //    card.CargarPersonaje(personaje);
            //    card.OnModificarPersonaje += Card_OnModificarPersonaje;
            //    flpCards.Controls.Add(card);
            //}
        }

        
    }
}
