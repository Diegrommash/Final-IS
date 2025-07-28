namespace UI
{
    partial class frmSeleccionarMision
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.ListView lstMisiones;
        private System.Windows.Forms.Button btnAceptar;
        private System.Windows.Forms.Button btnCancelar;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lstMisiones = new System.Windows.Forms.ListView();
            this.btnAceptar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.SuspendLayout();

            // 
            // lstMisiones
            // 
            this.lstMisiones.Location = new System.Drawing.Point(12, 12);
            this.lstMisiones.Name = "lstMisiones";
            this.lstMisiones.Size = new System.Drawing.Size(360, 214);
            this.lstMisiones.TabIndex = 0;
            this.lstMisiones.UseCompatibleStateImageBehavior = false;
            this.lstMisiones.View = System.Windows.Forms.View.List; // Vista de lista para iconos pequeños
            this.lstMisiones.FullRowSelect = true;
            this.lstMisiones.HideSelection = false;

            // 
            // btnAceptar
            // 
            this.btnAceptar.Location = new System.Drawing.Point(80, 240);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(100, 30);
            this.btnAceptar.TabIndex = 1;
            this.btnAceptar.Text = "Aceptar";
            this.btnAceptar.UseVisualStyleBackColor = true;
            this.btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);

            // 
            // btnCancelar
            // 
            this.btnCancelar.Location = new System.Drawing.Point(200, 240);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(100, 30);
            this.btnCancelar.TabIndex = 2;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);

            // 
            // frmSeleccionarMision
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 281);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnAceptar);
            this.Controls.Add(this.lstMisiones);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSeleccionarMision";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Seleccionar Misión";
            this.Load += new System.EventHandler(this.frmSeleccionarMision_Load);
            this.ResumeLayout(false);
        }
    }
}
