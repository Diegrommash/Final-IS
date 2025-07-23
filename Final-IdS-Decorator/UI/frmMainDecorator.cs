using System;
using System.Windows.Forms;
using UI;

public class frmMainDecorator : Form
{
    private MenuStrip menuStrip;

    public frmMainDecorator()
    {
        InicializarComponentesCustom();
    }

    private void InicializarComponentesCustom()
    {
        this.Text = "RPG - Juego Principal";
        this.IsMdiContainer = true;
        this.WindowState = FormWindowState.Maximized;

        menuStrip = new MenuStrip();
        var menuJuego = new ToolStripMenuItem("Juego");
        var itemCrearPersonaje = new ToolStripMenuItem("Crear Personaje");
        var verPersonajes = new ToolStripMenuItem("Ver Personajes");
        var itemSalir = new ToolStripMenuItem("Salir");

        itemCrearPersonaje.Click += (s, e) => AbrirFormularioPersonaje();
        verPersonajes.Click += (s, e) => AbrirFormularioVerPersonajes();
        itemSalir.Click += (s, e) => this.Close();

        menuJuego.DropDownItems.Add(itemCrearPersonaje);
        menuJuego.DropDownItems.Add(verPersonajes);
        menuJuego.DropDownItems.Add(new ToolStripSeparator());
        menuJuego.DropDownItems.Add(itemSalir);

        menuStrip.Items.Add(menuJuego);
        this.MainMenuStrip = menuStrip;
        this.Controls.Add(menuStrip);
    }

    private void AbrirFormularioPersonaje()
    {
        foreach (Form f in this.MdiChildren)
        {
            if (f is frmCrearPersonaje)
            {
                f.BringToFront();
                return;
            }
        }

        var crearPersonajeForm = new frmCrearPersonaje();
        crearPersonajeForm.MdiParent = this;
        crearPersonajeForm.Show();
    }

    private void AbrirFormularioVerPersonajes()
    {
        var verForm = new frmVerPersonajes();
        verForm.MdiParent = this;
        verForm.Show();
    }

    public static void Mostrar()
    {
        Application.Run(new frmMainDecorator());
    }
}
