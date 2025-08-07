using BLL.Abstracciones;
using BLL;
using Servicios;
using UI.Observable;
using BE;

namespace UI
{
    public partial class frmPersonajes : Form , IOrdenSubject
    {
        private readonly List<IOrdenObserver> _observadores = new();

        private readonly ServicioPersonaje _servicioPersonaje;
        private readonly ServicioOTF _servicioOTF;
        private readonly ServicioOrden _servicioOrden;
        private List<IComponente> _personajes;

        public frmPersonajes()
        {
            InitializeComponent();

            _servicioPersonaje = new ServicioPersonaje();
            _servicioOTF = new ServicioOTF();
            _servicioOrden = new ServicioOrden();

            this.Load += FrmPersonajes_Load;
        }

        private async void FrmPersonajes_Load(object sender, EventArgs e)
        {
            await CargarPersonajes();
            await CargarOrdenes();
        }

        private async Task CargarOrdenes()
        {
            try
            {
                var ordenes = await _servicioOrden.BuscarTodosAsync();

                cmbOrdenes.DataSource = ordenes;
                cmbOrdenes.DisplayMember = "Declaracion";
                cmbOrdenes.ValueMember = "Id"; 
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar las órdenes: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task CargarPersonajes()
        {
            _personajes = await _servicioPersonaje.BuscarPersonajes(SesionJugador.JugadorActual);

            flpCards.Controls.Clear();

            foreach (var pj in _personajes)
            {
                var card = new ucCardPersonaje
                {
                    Width = 220,
                    Height = 120,
                    Margin = new Padding(10)
                };

                card.SetPersonaje(pj);

                Suscribir(card);

                card.Click += (s, e) =>
                    MessageBox.Show($"Personaje: {pj.ObtenerDescripcion()}", "Detalle");

                flpCards.Controls.Add(card);
            }
        }

        private void btnDarOrden_Click(object sender, EventArgs e)
        {
            if (cmbOrdenes.SelectedItem is Orden ordenSeleccionada && !string.IsNullOrWhiteSpace(ordenSeleccionada.Declaracion))
            {
                string orden = ordenSeleccionada.Declaracion;
                Notificar(orden);
            }
            else
            {
                MessageBox.Show("Por favor, seleccione una orden válida.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void Suscribir(IOrdenObserver observador) => _observadores.Add(observador);

        public void Desuscribir(IOrdenObserver observador) => _observadores.Remove(observador);

        public async void Notificar(string orden)
        {
            foreach (var obs in _observadores)
            {
                if (obs is ucCardPersonaje card)
                {
                    obs.OnOrdenRecibida(orden);
                }
            }
        }
    }
}
