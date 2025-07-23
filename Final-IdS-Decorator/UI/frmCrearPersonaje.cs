using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using BE;
using BE.Enums;
using BLL;
using BLL.Abstracciones;
using BLL.Decoradores;
using BLL.Mapper;
using Servicios;
using UI;

public class frmCrearPersonaje : Form
{
    private readonly bool _esEdicion;
    //private readonly IComponente? _personajeOriginal;

    private readonly ServicioItem _servicioItem;
    private readonly ServicioPersonaje _servicioPersonaje = new ServicioPersonaje();

    private bool _cargando = true;
    private Form? statsPopupActual;
    private int _orden = 0;

    private TextBox txtNombre;
    private ComboBox cmbTrabajo, cmbArma, cmbArmadura, cmbJoya, cmbPocion;
    private CheckBox chkMostrarDetalles;
    private ListBox lstResumen;
    private Label lblPoder, lblDefensa;
    private Button btnCrear;

    private IComponente personajeActual;

    public frmCrearPersonaje()
    {
        _servicioItem = new ServicioItem();
        _servicioPersonaje = new ServicioPersonaje();

        InicializarComponentes();
        _ = CargarOpciones();
    }

    public frmCrearPersonaje(IComponente personajeExistente)
    {
        _servicioItem = new ServicioItem();
        _servicioPersonaje = new ServicioPersonaje();

        _esEdicion = true;
        personajeActual = personajeExistente;

        InicializarComponentes();
        _ = CargarOpciones();
        RefrescarResumen();
        //CargarPersonajeExistente(personajeActual);
    }

    private void InicializarComponentes()
    {
        this.Text = "Crear Personaje";
        this.Size = new Size(600, 600);
        this.StartPosition = FormStartPosition.CenterScreen;

        var contenedor = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            RowCount = 11,
            ColumnCount = 4,
            Padding = new Padding(20),
            AutoSize = true
        };

        contenedor.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
        contenedor.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45));
        contenedor.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15));
        contenedor.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15));

        for (int i = 0; i < 11; i++)
            contenedor.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        txtNombre = new TextBox { Width = 200 };
        txtNombre.TextChanged += ActualizarPersonaje;

        cmbTrabajo = CrearComboBox();
        cmbArma = CrearComboBox();
        cmbArmadura = CrearComboBox();
        cmbJoya = CrearComboBox();
        cmbPocion = CrearComboBox();

        chkMostrarDetalles = new CheckBox
        {
            Text = "Mostrar detalles del ítem",
            Checked = true,
            AutoSize = true
        };

        lstResumen = new ListBox
        {
            Width = 400,
            Height = 150,
            Font = new Font("Segoe UI Emoji", 9),
            DrawMode = DrawMode.OwnerDrawFixed
        };
        lstResumen.DrawItem += (s, e) =>
        {
            if (e.Index < 0) return;
            e.DrawBackground();
            string text = lstResumen.Items[e.Index]?.ToString() ?? "";
            using var brush = new SolidBrush(e.ForeColor);
            e.Graphics.DrawString(text, e.Font, brush, e.Bounds);
            e.DrawFocusRectangle();
        };

        lblPoder = new Label { AutoSize = true };
        lblDefensa = new Label { AutoSize = true };

        btnCrear = new Button { Text = _esEdicion == true ? "Modificar" : "Crear" , Width = 100 };
        btnCrear.Click += BtnCrear_Click;

        AgregarFila(contenedor, "Nombre:", txtNombre, null, null, 0);
        AgregarFila(contenedor, "Trabajo:", cmbTrabajo, () => AplicarDecorador(cmbTrabajo), () => QuitarDecorador(TipoDecoradorEnum.Trabajo), 1);
        AgregarFila(contenedor, "Arma:", cmbArma, () => AplicarDecorador(cmbArma), () => QuitarDecorador(TipoDecoradorEnum.Arma), 2);
        AgregarFila(contenedor, "Armadura:", cmbArmadura, () => AplicarDecorador(cmbArmadura), () => QuitarDecorador(TipoDecoradorEnum.Armadura), 3);
        AgregarFila(contenedor, "Joya:", cmbJoya, () => AplicarDecorador(cmbJoya), () => QuitarDecorador(TipoDecoradorEnum.Joya), 4);
        AgregarFila(contenedor, "Poción:", cmbPocion, () => AplicarDecorador(cmbPocion), () => QuitarDecorador(TipoDecoradorEnum.Pocion), 5);

        contenedor.Controls.Add(chkMostrarDetalles, 1, 6);
        contenedor.SetColumnSpan(chkMostrarDetalles, 3);

        contenedor.Controls.Add(new Label { Text = "Resumen:", AutoSize = true }, 0, 7);
        contenedor.Controls.Add(lstResumen, 1, 7);
        contenedor.SetColumnSpan(lstResumen, 3);

        contenedor.Controls.Add(lblPoder, 1, 8);
        contenedor.Controls.Add(lblDefensa, 1, 9);
        contenedor.Controls.Add(btnCrear, 1, 10);

        this.Controls.Add(contenedor);
    }

    private ComboBox CrearComboBox()
    {
        var combo = new ComboBox
        {
            Width = 200,
            DropDownStyle = ComboBoxStyle.DropDownList,
            DisplayMember = "Nombre"
        };

        combo.SelectedIndexChanged += (s, e) =>
        {
            if (_cargando) return;

            if (combo.SelectedItem is Item item && chkMostrarDetalles.Checked)
            {
                statsPopupActual?.Close();
                statsPopupActual?.Dispose();

                var popup = new frmStatsItem(item);
                var control = (Control)s;
                var locationOnScreen = control.PointToScreen(Point.Empty);
                popup.Location = new Point(locationOnScreen.X + control.Width + 5, locationOnScreen.Y);

                statsPopupActual = popup;
                popup.Show();
            }
        };

        return combo;
    }

    private void AgregarFila(TableLayoutPanel contenedor, string etiqueta, Control control, Action? onAdd, Action? onRemove, int row)
    {
        var lbl = new Label { Text = etiqueta, AutoSize = true };
        contenedor.Controls.Add(lbl, 0, row);
        contenedor.Controls.Add(control, 1, row);

        if (onAdd != null)
        {
            var btnAdd = new Button { Text = "+", Width = 30 };
            btnAdd.Click += (s, e) => onAdd();
            contenedor.Controls.Add(btnAdd, 2, row);
        }

        if (onRemove != null)
        {
            var btnRemove = new Button { Text = "–", Width = 30 };
            btnRemove.Click += (s, e) => onRemove();
            contenedor.Controls.Add(btnRemove, 3, row);
        }
    }

    private async Task CargarOpciones()
    {
        //var personajes = await _servicioPersonaje.BuscarPersonajes(SesionJugador.JugadorActual);

        var items = await _servicioItem.BuscarTodos();

        cmbTrabajo.DataSource = items.Where(i => i.Tipo == TipoDecoradorEnum.Trabajo).ToList();
        cmbTrabajo.SelectedIndex = -1;

        cmbArma.DataSource = items.Where(i => i.Tipo == TipoDecoradorEnum.Arma).ToList();
        cmbArma.SelectedIndex = -1;

        cmbArmadura.DataSource = items.Where(i => i.Tipo == TipoDecoradorEnum.Armadura).ToList();
        cmbArmadura.SelectedIndex = -1;

        cmbJoya.DataSource = items.Where(i => i.Tipo == TipoDecoradorEnum.Joya).ToList();
        cmbJoya.SelectedIndex = -1;

        cmbPocion.DataSource = items.Where(i => i.Tipo == TipoDecoradorEnum.Pocion).ToList();
        cmbPocion.SelectedIndex = -1;

        _cargando = false;
    }

    private void ActualizarPersonaje(object? sender, EventArgs e)
    {
        string nombre = txtNombre.Text.Trim();
        if (string.IsNullOrEmpty(nombre))
        {
            personajeActual = null;
            lstResumen.Items.Clear();
            lblPoder.Text = "";
            lblDefensa.Text = "";
            return;
        }

        personajeActual = new PersonajeBase(nombre);
        _orden = 0;
        RefrescarResumen();
    }

    private void CargarPersonajeExistente(IComponente personaje)
    {
        //personajeActual = personaje.Clonar();

        txtNombre.Text = ServicioPersonaje.ObtenerPersonajeBase(personaje).Nombre;

        var decoradores = ServicioPersonaje.ExtraerDecoradores(personaje);

        foreach (var deco in decoradores)
        {
            switch (deco.Item.Tipo)
            {
                case TipoDecoradorEnum.Trabajo:
                    cmbTrabajo.SelectedItem = deco.Item.Id;
                    break;
                case TipoDecoradorEnum.Arma:
                    cmbArma.SelectedItem = deco.Item;
                    break;
                case TipoDecoradorEnum.Armadura:
                    cmbArmadura.SelectedItem = deco.Item;
                    break;
                case TipoDecoradorEnum.Joya:
                    cmbJoya.SelectedItem = deco.Item;
                    break;
                case TipoDecoradorEnum.Pocion:
                    cmbPocion.SelectedItem = deco.Item;
                    break;
            }
        }

        RefrescarResumen();
    }


    private void AplicarDecorador(ComboBox combo)
    {
        if (combo.SelectedItem is not Item item || personajeActual == null)
            return;

        try
        {
            personajeActual = item.AplicarItem(personajeActual);
            if (personajeActual is Decorador deco)
                deco.Orden = ++_orden;
            RefrescarResumen();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al aplicar decorador:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void QuitarDecorador(TipoDecoradorEnum tipo)
    {
        if (personajeActual == null) return;

        IComponente actual = personajeActual;
        IComponente? anterior = null;

        while (actual is Decorador deco)
        {
            if (deco.Tipo == tipo)
            {
                if (anterior == null)
                {
                    personajeActual = deco.ObtenerPersonajeInterno();
                }
                else if (anterior is Decorador decoradorAnterior)
                {
                    decoradorAnterior.ReemplazarPersonajeInterno(deco.ObtenerPersonajeInterno());
                }
                break;
            }
            anterior = actual;
            actual = deco.ObtenerPersonajeInterno();
        }

        RefrescarResumen();
    }

    private void RefrescarResumen()
    {
        lstResumen.Items.Clear();
        if (personajeActual == null) return;

        string[] lineas = personajeActual.ObtenerDescripcion().Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var linea in lineas)
            lstResumen.Items.Add(linea.Trim());

        lblPoder.Text = $"Poder total: {personajeActual.ObtenerPoder()}";
        lblDefensa.Text = $"Defensa total: {personajeActual.ObtenerDefensa()}";
    }

    private async void BtnCrear_Click(object? sender, EventArgs e)
    {
        if (personajeActual == null)
        {
            MessageBox.Show("Complete todos los campos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (_esEdicion)
        {
           // await _servicioPersonaje.ActualizarPersonajeAsync(personajeActual, SesionJugador.JugadorActual);
            MessageBox.Show("Personaje actualizado con éxito.", "Éxito");
        }
        else
        {
            await _servicioPersonaje.GuardarPersonajeAsync(personajeActual, SesionJugador.JugadorActual);
            MessageBox.Show("Personaje creado con éxito:\n" + personajeActual.ObtenerDescripcion(), "Éxito");
        }

        this.DialogResult = DialogResult.OK;
        this.Close();
    }
}
