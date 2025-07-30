namespace UI
{
    partial class frmCrearMision
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblNombre;
        private TextBox txtNombre;
        private Label lblDescripcion;
        private TextBox txtDescripcion;
        private Label lblDificultad;
        private NumericUpDown nudDificultad;
        private CheckBox chkEsCompuesta;
        private Button btnGuardar;
        private Button btnCancelar;
        private ComboBox cboRecompensas;
        private Button btnAgregarRecompensa;
        private ListBox lstRecompensas;

        /// <summary>
        /// Limpia los recursos.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Inicializa los controles del formulario.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblNombre = new Label();
            this.txtNombre = new TextBox();
            this.lblDescripcion = new Label();
            this.txtDescripcion = new TextBox();
            this.lblDificultad = new Label();
            this.nudDificultad = new NumericUpDown();
            this.chkEsCompuesta = new CheckBox();
            this.cboRecompensas = new ComboBox();
            this.btnAgregarRecompensa = new Button();
            this.lstRecompensas = new ListBox();
            this.btnGuardar = new Button();
            this.btnCancelar = new Button();

            ((System.ComponentModel.ISupportInitialize)(this.nudDificultad)).BeginInit();
            this.SuspendLayout();

            // lblNombre
            this.lblNombre.AutoSize = true;
            this.lblNombre.Location = new System.Drawing.Point(20, 20);
            this.lblNombre.Name = "lblNombre";
            this.lblNombre.Size = new System.Drawing.Size(60, 15);
            this.lblNombre.Text = "Nombre:";

            // txtNombre
            this.txtNombre.Location = new System.Drawing.Point(100, 18);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(250, 23);

            // lblDescripcion
            this.lblDescripcion.AutoSize = true;
            this.lblDescripcion.Location = new System.Drawing.Point(20, 60);
            this.lblDescripcion.Name = "lblDescripcion";
            this.lblDescripcion.Size = new System.Drawing.Size(72, 15);
            this.lblDescripcion.Text = "Descripción:";

            // txtDescripcion
            this.txtDescripcion.Location = new System.Drawing.Point(100, 58);
            this.txtDescripcion.Multiline = true;
            this.txtDescripcion.Name = "txtDescripcion";
            this.txtDescripcion.Size = new System.Drawing.Size(250, 60);

            // lblDificultad
            this.lblDificultad.AutoSize = true;
            this.lblDificultad.Location = new System.Drawing.Point(20, 130);
            this.lblDificultad.Name = "lblDificultad";
            this.lblDificultad.Size = new System.Drawing.Size(62, 15);
            this.lblDificultad.Text = "Dificultad:";

            // nudDificultad
            this.nudDificultad.Location = new System.Drawing.Point(100, 128);
            this.nudDificultad.Name = "nudDificultad";
            this.nudDificultad.Size = new System.Drawing.Size(80, 23);

            // chkEsCompuesta
            this.chkEsCompuesta.AutoSize = true;
            this.chkEsCompuesta.Location = new System.Drawing.Point(200, 130);
            this.chkEsCompuesta.Name = "chkEsCompuesta";
            this.chkEsCompuesta.Size = new System.Drawing.Size(150, 19);
            this.chkEsCompuesta.Text = "¿Es misión compuesta?";

            // cboRecompensas
            this.cboRecompensas.Location = new System.Drawing.Point(20, 160);
            this.cboRecompensas.Name = "cboRecompensas";
            this.cboRecompensas.Size = new System.Drawing.Size(250, 23);

            // btnAgregarRecompensa
            this.btnAgregarRecompensa.Location = new System.Drawing.Point(280, 160);
            this.btnAgregarRecompensa.Name = "btnAgregarRecompensa";
            this.btnAgregarRecompensa.Size = new System.Drawing.Size(70, 23);
            this.btnAgregarRecompensa.Text = "Agregar";
            this.btnAgregarRecompensa.UseVisualStyleBackColor = true;
            this.btnAgregarRecompensa.Click += new EventHandler(this.btnAgregarRecompensa_Click);

            // lstRecompensas
            this.lstRecompensas.Location = new System.Drawing.Point(20, 190);
            this.lstRecompensas.Name = "lstRecompensas";
            this.lstRecompensas.Size = new System.Drawing.Size(330, 80);

            // btnGuardar
            this.btnGuardar.Location = new System.Drawing.Point(80, 285);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(100, 30);
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.UseVisualStyleBackColor = true;
            this.btnGuardar.Click += new EventHandler(this.btnGuardar_Click);

            // btnCancelar
            this.btnCancelar.Location = new System.Drawing.Point(200, 285);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(100, 30);
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new EventHandler(this.btnCancelar_Click);

            // frmCrearMision
            this.ClientSize = new System.Drawing.Size(380, 340);
            this.Controls.Add(this.lblNombre);
            this.Controls.Add(this.txtNombre);
            this.Controls.Add(this.lblDescripcion);
            this.Controls.Add(this.txtDescripcion);
            this.Controls.Add(this.lblDificultad);
            this.Controls.Add(this.nudDificultad);
            this.Controls.Add(this.chkEsCompuesta);
            this.Controls.Add(this.cboRecompensas);
            this.Controls.Add(this.btnAgregarRecompensa);
            this.Controls.Add(this.lstRecompensas);
            this.Controls.Add(this.btnGuardar);
            this.Controls.Add(this.btnCancelar);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Name = "frmCrearMision";
            this.Text = "Crear Misión";
            this.Load += new EventHandler(this.frmCrearMision_Load);

            ((System.ComponentModel.ISupportInitialize)(this.nudDificultad)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
