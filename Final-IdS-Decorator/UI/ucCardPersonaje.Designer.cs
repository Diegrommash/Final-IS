namespace UI.Controles
{
    partial class ucCardPersonaje
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblNombrePersonaje;
        private System.Windows.Forms.ListBox lstItems;
        private System.Windows.Forms.Label lblPoder;
        private System.Windows.Forms.Label lblDefensa;
        private System.Windows.Forms.Button btnModificar;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblNombrePersonaje = new Label();
            lstItems = new ListBox();
            lblPoder = new Label();
            lblDefensa = new Label();
            btnModificar = new Button();
            SuspendLayout();
            // 
            // lblNombrePersonaje
            // 
            lblNombrePersonaje.AutoSize = true;
            lblNombrePersonaje.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblNombrePersonaje.Location = new Point(101, 9);
            lblNombrePersonaje.Name = "lblNombrePersonaje";
            lblNombrePersonaje.Size = new Size(65, 19);
            lblNombrePersonaje.TabIndex = 0;
            lblNombrePersonaje.Text = "Nombre";
            // 
            // lstItems
            // 
            lstItems.FormattingEnabled = true;
            lstItems.ItemHeight = 15;
            lstItems.Location = new Point(10, 40);
            lstItems.Name = "lstItems";
            lstItems.Size = new Size(384, 94);
            lstItems.TabIndex = 1;
            // 
            // lblPoder
            // 
            lblPoder.AutoSize = true;
            lblPoder.Location = new Point(10, 145);
            lblPoder.Name = "lblPoder";
            lblPoder.Size = new Size(77, 15);
            lblPoder.TabIndex = 2;
            lblPoder.Text = "Poder total: 0";
            // 
            // lblDefensa
            // 
            lblDefensa.AutoSize = true;
            lblDefensa.Location = new Point(10, 165);
            lblDefensa.Name = "lblDefensa";
            lblDefensa.Size = new Size(88, 15);
            lblDefensa.TabIndex = 3;
            lblDefensa.Text = "Defensa total: 0";
            // 
            // btnModificar
            // 
            btnModificar.Location = new Point(76, 193);
            btnModificar.Name = "btnModificar";
            btnModificar.Size = new Size(220, 25);
            btnModificar.TabIndex = 4;
            btnModificar.Text = "Modificar";
            btnModificar.UseVisualStyleBackColor = true;
            btnModificar.Click += btnModificar_Click;
            // 
            // ucCardPersonaje
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(lblNombrePersonaje);
            Controls.Add(lstItems);
            Controls.Add(lblPoder);
            Controls.Add(lblDefensa);
            Controls.Add(btnModificar);
            Name = "ucCardPersonaje";
            Size = new Size(409, 230);
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
