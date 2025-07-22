using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using BE;
using BE.Enums;
using BLL;
using BLL.Abstracciones;
using DAL;

namespace UI
{
    public class frmEditarPersonaje : Form
    {
        private readonly ServicioPersonaje _servicioPersonaje;
        private readonly List<Item> _itemsDisponibles;
        private IComponente _personajeActual;
        private readonly Dictionary<TipoDecoradorEnum, ComboBox> _combosPorTipo;

        public frmEditarPersonaje(IComponente personaje)
        {
            _servicioPersonaje = new ServicioPersonaje();
            _personajeActual = personaje;
            _combosPorTipo = new Dictionary<TipoDecoradorEnum, ComboBox>();
            _itemsDisponibles = new List<Item>();

            this.Text = "Editar Personaje";
            this.Width = 550;
            this.Height = 500;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Load += frmEditarPersonaje_Load;
        }

        private async void frmEditarPersonaje_Load(object sender, EventArgs e)
        {
            await CargarItems();
            InicializarCombos();
            CargarValoresIniciales();
            RefrescarResumen();
        }

        private async Task CargarItems()
        {
            var repo = new RepoItem(new Acceso());
            _itemsDisponibles.Clear();
            _itemsDisponibles.AddRange(await repo.BuscarTodos());
        }

        private void InicializarCombos()
        {
            int y = 10;
            foreach (TipoDecoradorEnum tipo in Enum.GetValues(typeof(TipoDecoradorEnum)))
            {
                var lbl = new Label { Text = tipo.ToString(), Top = y, Left = 10, Width = 100 };
                var cmb = new ComboBox
                {
                    Top = y,
                    Left = 120,
                    Width = 300,
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    DisplayMember = "Nombre",
                    ValueMember = "Id"
                };

                // Copia local para usar en evento
                var tipoLocal = tipo;
                cmb.SelectedIndexChanged += (s, e) =>
                {
                    if (cmb.SelectedItem is Item nuevoItem)
                        CambiarDecorador(tipoLocal, nuevoItem);
                };

                var itemsFiltrados = _itemsDisponibles.Where(i => i.Tipo == tipo).ToList();
                cmb.DataSource = itemsFiltrados;

                Controls.Add(lbl);
                Controls.Add(cmb);
                _combosPorTipo[tipo] = cmb;

                y += 35;
            }
        }

        private void CargarValoresIniciales()
        {
            var decoradores = ServicioPersonaje.ExtraerDecoradores(_personajeActual);
            foreach (var (item, _) in decoradores)
            {
                if (_combosPorTipo.ContainsKey(item.Tipo))
                {
                    var combo = _combosPorTipo[item.Tipo];
                    combo.SelectedItem = _itemsDisponibles.FirstOrDefault(i => i.Id == item.Id);
                }
            }
        }

        private void CambiarDecorador(TipoDecoradorEnum tipo, Item nuevoItem)
        {
            var decoradores = ServicioPersonaje.ExtraerDecoradores(_personajeActual);
            var decorador = decoradores.FirstOrDefault(d => d.Item.Tipo == tipo);
            if (decorador.Item != null)
            {
                _personajeActual = _servicioPersonaje.ReemplazarDecoradorPorOrden(_personajeActual, decorador.Orden, nuevoItem);
                RefrescarResumen();
            }
        }

        private void RefrescarResumen()
        {
            var resumen = Controls.OfType<ListBox>().FirstOrDefault(l => l.Name == "lstResumen");
            if (resumen == null)
            {
                resumen = new ListBox
                {
                    Name = "lstResumen",
                    Top = 200,
                    Left = 10,
                    Width = 510,
                    Height = 150
                };
                Controls.Add(resumen);
            }

            resumen.Items.Clear();
            string[] lineas = _personajeActual.ObtenerDescripcion().Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var linea in lineas)
                resumen.Items.Add(linea.Trim());

            var lblPoder = Controls.OfType<Label>().FirstOrDefault(l => l.Name == "lblPoder");
            var lblDefensa = Controls.OfType<Label>().FirstOrDefault(l => l.Name == "lblDefensa");

            if (lblPoder == null)
            {
                lblPoder = new Label { Name = "lblPoder", Top = 360, Left = 10, Width = 300 };
                Controls.Add(lblPoder);
            }

            if (lblDefensa == null)
            {
                lblDefensa = new Label { Name = "lblDefensa", Top = 380, Left = 10, Width = 300 };
                Controls.Add(lblDefensa);
            }

            lblPoder.Text = $"Poder total: {_personajeActual.ObtenerPoder()}";
            lblDefensa.Text = $"Defensa total: {_personajeActual.ObtenerDefensa()}";
        }
    }
}
