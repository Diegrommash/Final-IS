using System;
using System.Windows.Forms;

public class frmMain : Form
{
    private MenuStrip menuStrip;

    public frmMain()
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
        var itemIniciarHistoria = new ToolStripMenuItem("Iniciar Historia");
        var itemSalir = new ToolStripMenuItem("Salir");

        itemCrearPersonaje.Click += (s, e) => AbrirFormularioPersonaje();
        itemIniciarHistoria.Click += (s, e) => AbrirFormularioHistoria();
        itemSalir.Click += (s, e) => this.Close();

        menuJuego.DropDownItems.Add(itemCrearPersonaje);
        menuJuego.DropDownItems.Add(itemIniciarHistoria);
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

    private void AbrirFormularioHistoria()
    {
        //var historiaForm = new frmHistoria(); // o frmMision
        //historiaForm.MdiParent = this;
        //historiaForm.Show();
    }

    public static void Mostrar()
    {
        Application.Run(new frmMain());
    }
}
