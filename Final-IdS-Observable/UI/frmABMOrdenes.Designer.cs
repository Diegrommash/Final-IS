namespace UI
{
    partial class frmABMOrdenes
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataGridView dgvOrdenes;
        private System.Windows.Forms.Button btnAgregar;
        private System.Windows.Forms.Button btnEditar;
        private System.Windows.Forms.Button btnEliminar;
        private System.Windows.Forms.Button btnCerrar;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.dgvOrdenes = new System.Windows.Forms.DataGridView();
            this.btnAgregar = new System.Windows.Forms.Button();
            this.btnEditar = new System.Windows.Forms.Button();
            this.btnEliminar = new System.Windows.Forms.Button();
            this.btnCerrar = new System.Windows.Forms.Button();

            ((System.ComponentModel.ISupportInitialize)(this.dgvOrdenes)).BeginInit();
            this.SuspendLayout();

            // dgvOrdenes
            this.dgvOrdenes.AllowUserToAddRows = false;
            this.dgvOrdenes.AllowUserToDeleteRows = false;
            this.dgvOrdenes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvOrdenes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvOrdenes.Location = new System.Drawing.Point(12, 12);
            this.dgvOrdenes.Name = "dgvOrdenes";
            this.dgvOrdenes.ReadOnly = true;
            this.dgvOrdenes.Size = new System.Drawing.Size(400, 250);

            // btnAgregar
            this.btnAgregar.Text = "Agregar";
            this.btnAgregar.Location = new System.Drawing.Point(430, 20);
            this.btnAgregar.Click += new System.EventHandler(this.btnAgregar_Click);

            // btnEditar
            this.btnEditar.Text = "Editar";
            this.btnEditar.Location = new System.Drawing.Point(430, 60);
            this.btnEditar.Click += new System.EventHandler(this.btnEditar_Click);

            // btnEliminar
            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.Location = new System.Drawing.Point(430, 100);
            this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);

            // btnCerrar
            this.btnCerrar.Text = "Cerrar";
            this.btnCerrar.Location = new System.Drawing.Point(430, 140);
//            this.btnCerrar.Click += (s, e) => this.Close();

            // frmOrden
            this.ClientSize = new System.Drawing.Size(550, 280);
            this.Controls.Add(this.dgvOrdenes);
            this.Controls.Add(this.btnAgregar);
            this.Controls.Add(this.btnEditar);
            this.Controls.Add(this.btnEliminar);
            this.Controls.Add(this.btnCerrar);
            this.Text = "Gestión de Órdenes";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.frmOrden_Load);

            ((System.ComponentModel.ISupportInitialize)(this.dgvOrdenes)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
