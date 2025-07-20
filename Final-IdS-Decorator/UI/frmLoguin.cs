using System;
using System.Drawing;
using System.Windows.Forms;
using BE;
using BLL;

public class frmLogin : Form
{
    private Label lblNombre;
    private TextBox txtNombre;
    private Label lblContraseña;
    private TextBox txtContraseña;
    private Button btnLogin;
    private Label lblError;

    private readonly ServicioLogin _servicioLogin;
    private readonly Func<Form> _crearFormularioPrincipal;

    public frmLogin(Func<Form> crearFormularioPrincipal, ServicioLogin servicioLogin)
    {
        _crearFormularioPrincipal = crearFormularioPrincipal;
        _servicioLogin = servicioLogin;
        InicializarComponentesCustom();
    }

    private void InicializarComponentesCustom()
    {
        this.Text = "Login de Jugador";
        this.Size = new Size(450, 320);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;

        int labelX = 40;
        int textboxX = 150;
        int anchoCampos = 220;
        int altoControl = 30;

        lblNombre = new Label
        {
            Text = "Nombre:",
            Location = new Point(labelX, 40),
            AutoSize = true
        };

        txtNombre = new TextBox
        {
            Location = new Point(textboxX, 35),
            Size = new Size(anchoCampos, altoControl)
        };

        lblContraseña = new Label
        {
            Text = "Contraseña:",
            Location = new Point(labelX, 90),
            AutoSize = true
        };

        txtContraseña = new TextBox
        {
            Location = new Point(textboxX, 85),
            Size = new Size(anchoCampos, altoControl),
            UseSystemPasswordChar = true
        };

        btnLogin = new Button
        {
            Text = "Iniciar sesión",
            Location = new Point(textboxX, 140),
            Size = new Size(anchoCampos, 40)
        };
        btnLogin.Click += btnLogin_Click;

        lblError = new Label
        {
            ForeColor = Color.Red,
            Location = new Point(labelX, 200),
            AutoSize = true,
            Text = ""
        };

        Controls.Add(lblNombre);
        Controls.Add(txtNombre);
        Controls.Add(lblContraseña);
        Controls.Add(txtContraseña);
        Controls.Add(btnLogin);
        Controls.Add(lblError);
    }

    private async void btnLogin_Click(object? sender, EventArgs e)
    {
        string nombre = txtNombre.Text.Trim();
        string contraseña = txtContraseña.Text;

        if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(contraseña))
        {
            lblError.Text = "Ingrese nombre y contraseña.";
            return;
        }

        var loguin = await _servicioLogin.Loguin(new Jugador(nombre, contraseña));

        if (loguin.Jugador != null)
        {
            MessageBox.Show($"¡Bienvenido, {loguin.Jugador.Nombre}!", "Login correcto");

            Form frmMain = _crearFormularioPrincipal.Invoke();
            frmMain.Show();
            this.Hide();
        }
        else
        {
            MessageBox.Show(loguin.Mensaje, "Error de Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // Método de arranque centralizado con formulario dinámico
    public static void Mostrar(Func<Form> crearFormularioPrincipal, ServicioLogin servicioLogin)
    {
        Application.Run(new frmLogin(crearFormularioPrincipal, servicioLogin));
    }
}
