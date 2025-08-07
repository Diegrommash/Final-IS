using BE.Enums;
using BLL;
using BLL.Abstracciones;
using UI.Observable;

namespace UI
{
    public partial class ucCardPersonaje : UserControl, IOrdenObserver
    {
        private readonly ServicioOTF _servicioOTF;

        private IComponente _personaje;
        private readonly string _iconPath;

        private Image _iconNormal;
        private Image _iconHablando;
        private string _trabajoActual;
        public string TrabajoActual => _trabajoActual;

        private System.Windows.Forms.Timer animacionBoca;
        private bool bocaAbierta = false;

        public ucCardPersonaje()
        {
            _servicioOTF = new ServicioOTF();
            InitializeComponent();
            _iconPath = Path.Combine(Application.StartupPath, "Resources", "Iconos");
        }

        public void SetPersonaje(IComponente personaje)
        {
            _personaje = personaje;

            lblNombre.Text = ServicioPersonaje.ObtenerPersonajeBase(personaje).Nombre;
            lblStats.Text = $"Poder: {_personaje.ObtenerPoder()}  |  Defensa: {_personaje.ObtenerDefensa()}";

            _trabajoActual = ServicioPersonaje.ExtraerDecoradores(personaje)
                                              .FirstOrDefault(d => d.Item.Tipo == TipoDecoradorEnum.Trabajo)
                                              .Item?.Nombre ?? "Sin trabajo";

            lblTipo.Text = _trabajoActual;
            lblTipo.BackColor = ObtenerColorTrabajo(_trabajoActual);

            _iconNormal = CargarIcono(_trabajoActual, false);
            _iconHablando = CargarIcono(_trabajoActual, true);
            pictureTipo.Image = _iconNormal;
        }

        #region color e iconos
        private Color ObtenerColorTrabajo(string trabajo) => trabajo switch
        {
            "Guerrero" => Color.DarkRed,
            "Mago" => Color.MediumPurple,
            "Arquero" => Color.ForestGreen,
            "Ladron" => Color.DarkSlateGray,
            _ => Color.Gray
        };

        private Image CargarIcono(string trabajo, bool hablando)
        {
            string nombreArchivo = trabajo.ToLower() + (hablando ? "_hablando.png" : ".png");
            string filePath = Path.Combine(_iconPath, nombreArchivo);

            if (File.Exists(filePath))
                return Image.FromFile(filePath);

            string resName = hablando ? $"icon_{trabajo.ToLower()}_hablando" : $"icon_{trabajo.ToLower()}";
            object resObj = Properties.Resources.ResourceManager.GetObject(resName);

            Image icono = ConvertirAImagen(resObj);
            if (icono != null)
                return icono;

            object defaultObj = Properties.Resources.ResourceManager.GetObject("icon_default");
            return ConvertirAImagen(defaultObj) ?? new Bitmap(64, 64);
        }

        private Image ConvertirAImagen(object recurso)
        {
            if (recurso == null) return null;

            if (recurso is Image img) return img;

            if (recurso is byte[] bytes)
            {
                using var ms = new MemoryStream(bytes);
                return Image.FromStream(ms);
            }

            return null;
        }
        #endregion

        #region metodos para que el pj hable

        public async void Hablar(string mensaje)
        {
            int alturaOriginal = this.Height;
            this.Height += 30;

            Label burbuja = new Label
            {
                AutoSize = true,
                BackColor = Color.LightYellow,
                ForeColor = Color.Black,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(5),
                Font = new Font("Segoe UI", 9, FontStyle.Italic),
                Location = new Point(10, this.Height - 40),
                MaximumSize = new Size(this.Width - 20, 0)
            };

            this.Controls.Add(burbuja);
            burbuja.BringToFront();

            IniciarAnimacionBoca();
            await MostrarTextoProgresivo(burbuja, mensaje);
            DetenerAnimacionBoca();

            System.Windows.Forms.Timer t = new System.Windows.Forms.Timer { Interval = 2500 };
            t.Tick += (s, e) =>
            {
                this.Controls.Remove(burbuja);
                burbuja.Dispose();
                this.Height = alturaOriginal;
                t.Stop();
                t.Dispose();
            };
            t.Start();
        }

        private void IniciarAnimacionBoca()
        {
            if (animacionBoca != null && animacionBoca.Enabled) return;

            animacionBoca = new System.Windows.Forms.Timer { Interval = 150 };
            animacionBoca.Tick += (s, e) =>
            {
                pictureTipo.Image = bocaAbierta ? _iconNormal : _iconHablando;
                bocaAbierta = !bocaAbierta;
            };
            animacionBoca.Start();
        }

        private void DetenerAnimacionBoca()
        {
            if (animacionBoca != null)
            {
                animacionBoca.Stop();
                bocaAbierta = false;
                pictureTipo.Image = _iconNormal;
            }
        }

        private async Task MostrarTextoProgresivo(Label lbl, string mensaje)
        {
            lbl.Text = "";
            foreach (char c in mensaje)
            {
                lbl.Text += c;
                await Task.Delay(40);
            }
        }
        #endregion

        public async void OnOrdenRecibida(string orden)
        {
            string frase = await _servicioOTF.ObtenerFrasePorOrdenYTrabajoAsync(orden, TrabajoActual)
                            ?? $"{TrabajoActual} responde a {orden}!";
            Hablar(frase);
        }
    }
}
