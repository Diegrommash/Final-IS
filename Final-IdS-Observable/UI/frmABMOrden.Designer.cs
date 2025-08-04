namespace UI
{
    partial class frmABMOrden
    {
        private System.ComponentModel.IContainer components = null;
        private ComboBox cmbOrden;
        private ComboBox cmbItem;
        private TextBox txtFrase;
        private Label lblOrden;
        private Label lblItem;
        private Label lblFrase;
        private Button btnAgregar;
        private Button btnEliminar;
        private Button btnCerrar;
        private DataGridView dgvRelaciones;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.cmbOrden = new ComboBox();
            this.cmbItem = new ComboBox();
            this.txtFrase = new TextBox();
            this.lblOrden = new Label();
            this.lblItem = new Label();
            this.lblFrase = new Label();
            this.btnAgregar = new Button();
            this.btnEliminar = new Button();
            this.btnCerrar = new Button();
            this.dgvRelaciones = new DataGridView();

            ((System.ComponentModel.ISupportInitialize)(this.dgvRelaciones)).BeginInit();
            this.SuspendLayout();

            // lblOrden
            this.lblOrden.AutoSize = true;
            this.lblOrden.Location = new Point(12, 15);
            this.lblOrden.Text = "Orden:";
            this.lblOrden.Name = "lblOrden";

            // cmbOrden
            this.cmbOrden.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbOrden.Location = new Point(70, 12);
            this.cmbOrden.Name = "cmbOrden";
            this.cmbOrden.Size = new Size(200, 23);

            // lblItem
            this.lblItem.AutoSize = true;
            this.lblItem.Location = new Point(290, 15);
            this.lblItem.Text = "Tipo trabajo:";
            this.lblItem.Name = "lblItem";

            // cmbItem
            this.cmbItem.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbItem.Location = new Point(370, 12);
            this.cmbItem.Name = "cmbItem";
            this.cmbItem.Size = new Size(200, 23);

            // lblFrase
            this.lblFrase.AutoSize = true;
            this.lblFrase.Location = new Point(12, 50);
            this.lblFrase.Text = "Frase:";
            this.lblFrase.Name = "lblFrase";

            // txtFrase
            this.txtFrase.Location = new Point(70, 47);
            this.txtFrase.Name = "txtFrase";
            this.txtFrase.Size = new Size(500, 23);

            // btnAgregar
            this.btnAgregar.Location = new Point(580, 47);
            this.btnAgregar.Name = "btnAgregar";
            this.btnAgregar.Size = new Size(75, 23);
            this.btnAgregar.Text = "Agregar";
            this.btnAgregar.UseVisualStyleBackColor = true;
            this.btnAgregar.Click += new EventHandler(this.btnAgregar_Click);

            // dgvRelaciones
            this.dgvRelaciones.AllowUserToAddRows = false;
            this.dgvRelaciones.AllowUserToDeleteRows = false;
            this.dgvRelaciones.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRelaciones.Location = new Point(12, 85);
            this.dgvRelaciones.Name = "dgvRelaciones";
            this.dgvRelaciones.ReadOnly = true;
            this.dgvRelaciones.RowHeadersVisible = false;
            this.dgvRelaciones.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvRelaciones.Size = new Size(643, 220);
            this.dgvRelaciones.CellClick += new DataGridViewCellEventHandler(this.dgvRelaciones_CellClick);

            // btnEliminar
            this.btnEliminar.Location = new Point(12, 320);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new Size(75, 23);
            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.UseVisualStyleBackColor = true;
            //this.btnEliminar.Click += new EventHandler(this.btnEliminar_Click);

            // btnCerrar
            this.btnCerrar.Location = new Point(580, 320);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new Size(75, 23);
            this.btnCerrar.Text = "Cerrar";
            this.btnCerrar.UseVisualStyleBackColor = true;
            this.btnCerrar.Click += (s, e) => this.Close();

            // frmOrdenRelacion
            this.ClientSize = new Size(667, 360);
            this.Controls.Add(this.lblOrden);
            this.Controls.Add(this.cmbOrden);
            this.Controls.Add(this.lblItem);
            this.Controls.Add(this.cmbItem);
            this.Controls.Add(this.lblFrase);
            this.Controls.Add(this.txtFrase);
            this.Controls.Add(this.btnAgregar);
            this.Controls.Add(this.dgvRelaciones);
            this.Controls.Add(this.btnEliminar);
            this.Controls.Add(this.btnCerrar);
            this.Name = "frmOrdenRelacion";
            this.Text = "Relaciones Orden - Tipo de trabajo";

            ((System.ComponentModel.ISupportInitialize)(this.dgvRelaciones)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }


}