using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using BE;
using BE.Enums;
using BLL;
using BLL.Abstracciones;

namespace UI
{
    public partial class ucCardPersonaje : UserControl
    {
        private IComponente _personaje;
        private readonly string _iconPath;

        public ucCardPersonaje()
        {
            InitializeComponent();

            // Ruta donde están los íconos
            _iconPath = Path.Combine(Application.StartupPath, "Recursos", "Iconos");
        }

        public void SetPersonaje(IComponente personaje)
        {
            _personaje = personaje;

            // ✅ Nombre y stats
            lblNombre.Text = ServicioPersonaje.ObtenerPersonajeBase(personaje).Nombre;
            lblStats.Text = $"Poder: {_personaje.ObtenerPoder()}  |  Defensa: {_personaje.ObtenerDefensa()}";

            // ✅ Obtener trabajo (decorador del personaje)
            var trabajo = ServicioPersonaje.ExtraerDecoradores(personaje)
                                           .FirstOrDefault(d => d.Item.Tipo == TipoDecoradorEnum.Trabajo)
                                           .Item?.Nombre ?? "Sin trabajo";

            // ✅ Mostrar badge de trabajo
            lblTipo.Text = trabajo;
            lblTipo.BackColor = ObtenerColorTrabajo(trabajo);

            // ✅ Asignar ícono
            pictureTipo.Image = ObtenerIconoTrabajo(trabajo);
        }

        private Color ObtenerColorTrabajo(string trabajo) => trabajo switch
        {
            "Guerrero" => Color.DarkRed,
            "Mago" => Color.MediumPurple,
            "Arquero" => Color.ForestGreen,
            "Ladron" => Color.DarkSlateGray,
            _ => Color.Gray
        };

        /// <summary>
        /// Obtiene el icono desde carpeta o resources
        /// </summary>
        private Image ObtenerIconoTrabajo(string trabajo)
        {
            // 🔹 1. Intentar cargar desde carpeta
            string fileName = trabajo.ToLower() + ".png";
            string filePath = Path.Combine(_iconPath, fileName);

            if (File.Exists(filePath))
                return Image.FromFile(filePath);

            // 🔹 2. Si no está en carpeta, usar resources
            using (var ms = new MemoryStream(Properties.Resources.ResourceManager.GetObject($"icon_{trabajo.ToLower()}") as byte[]))
            {
                return Image.FromStream(ms);
            }
        }
    }
}
