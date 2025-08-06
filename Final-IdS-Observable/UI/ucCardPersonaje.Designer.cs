namespace UI
{
    partial class ucCardPersonaje
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.PictureBox pictureTipo;
        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.Label lblTipo;
        private System.Windows.Forms.Label lblStats;
        private System.Windows.Forms.Panel panelCard;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panelCard = new System.Windows.Forms.Panel();
            this.pictureTipo = new System.Windows.Forms.PictureBox();
            this.lblNombre = new System.Windows.Forms.Label();
            this.lblTipo = new System.Windows.Forms.Label();
            this.lblStats = new System.Windows.Forms.Label();
            this.panelCard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureTipo)).BeginInit();
            this.SuspendLayout();

            // 
            // panelCard
            // 
            this.panelCard.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelCard.Controls.Add(this.lblStats);
            this.panelCard.Controls.Add(this.lblTipo);
            this.panelCard.Controls.Add(this.lblNombre);
            this.panelCard.Controls.Add(this.pictureTipo);
            this.panelCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCard.Location = new System.Drawing.Point(0, 0);
            this.panelCard.Name = "panelCard";
            this.panelCard.Padding = new System.Windows.Forms.Padding(8);
            this.panelCard.Size = new System.Drawing.Size(220, 120);
            this.panelCard.TabIndex = 0;

            // 
            // pictureTipo
            // 
            this.pictureTipo.Location = new System.Drawing.Point(10, 10);
            this.pictureTipo.Name = "pictureTipo";
            this.pictureTipo.Size = new System.Drawing.Size(48, 48);
            this.pictureTipo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureTipo.TabStop = false;

            // 
            // lblNombre
            // 
            this.lblNombre.AutoSize = true;
            this.lblNombre.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblNombre.Location = new System.Drawing.Point(65, 12);
            this.lblNombre.Name = "lblNombre";
            this.lblNombre.Size = new System.Drawing.Size(95, 19);
            this.lblNombre.TabIndex = 1;
            this.lblNombre.Text = "Nombre PJ";

            // 
            // lblTipo
            // 
            this.lblTipo.AutoSize = true;
            this.lblTipo.BackColor = System.Drawing.Color.Gray;
            this.lblTipo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            this.lblTipo.ForeColor = System.Drawing.Color.White;
            this.lblTipo.Location = new System.Drawing.Point(65, 34);
            this.lblTipo.Name = "lblTipo";
            this.lblTipo.Padding = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.lblTipo.Size = new System.Drawing.Size(78, 17);
            this.lblTipo.TabIndex = 2;
            this.lblTipo.Text = "Tipo trabajo";

            // 
            // lblStats
            // 
            this.lblStats.AutoSize = true;
            this.lblStats.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblStats.Location = new System.Drawing.Point(10, 70);
            this.lblStats.Name = "lblStats";
            this.lblStats.Size = new System.Drawing.Size(120, 15);
            this.lblStats.TabIndex = 3;
            this.lblStats.Text = "Poder: 0 | Defensa: 0";

            // 
            // ucCardPersonaje
            // 
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.panelCard);
            this.Name = "ucCardPersonaje";
            this.Size = new System.Drawing.Size(220, 120);
            this.panelCard.ResumeLayout(false);
            this.panelCard.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureTipo)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
