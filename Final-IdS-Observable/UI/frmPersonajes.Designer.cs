namespace UI
{
    partial class frmPersonajes
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.FlowLayoutPanel flpCards;
        private System.Windows.Forms.ComboBox cmbOrdenes;
        private System.Windows.Forms.Button btnDarOrden;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.flpCards = new System.Windows.Forms.FlowLayoutPanel();
            this.cmbOrdenes = new System.Windows.Forms.ComboBox();
            this.btnDarOrden = new System.Windows.Forms.Button();
            this.SuspendLayout();

            // 
            // cmbOrdenes
            // 
            this.cmbOrdenes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbOrdenes.Location = new System.Drawing.Point(12, 12);
            this.cmbOrdenes.Name = "cmbOrdenes";
            this.cmbOrdenes.Size = new System.Drawing.Size(300, 24);
            this.cmbOrdenes.TabIndex = 1;

            // 
            // btnDarOrden
            // 
            this.btnDarOrden.Location = new System.Drawing.Point(320, 12);
            this.btnDarOrden.Name = "btnDarOrden";
            this.btnDarOrden.Size = new System.Drawing.Size(100, 24);
            this.btnDarOrden.TabIndex = 2;
            this.btnDarOrden.Text = "Dar Orden";
            this.btnDarOrden.UseVisualStyleBackColor = true;
            this.btnDarOrden.Click += new System.EventHandler(this.btnDarOrden_Click);

            // 
            // flpCards
            // 
            this.flpCards.AutoScroll = true;
            this.flpCards.Location = new System.Drawing.Point(12, 50);
            this.flpCards.Name = "flpCards";
            this.flpCards.Padding = new System.Windows.Forms.Padding(10);
            this.flpCards.Size = new System.Drawing.Size(776, 388);
            this.flpCards.TabIndex = 0;
            this.flpCards.WrapContents = true;
            this.flpCards.BackColor = System.Drawing.Color.White;

            // 
            // frmPersonajes
            // 
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnDarOrden);
            this.Controls.Add(this.cmbOrdenes);
            this.Controls.Add(this.flpCards);
            this.Name = "frmPersonajes";
            this.Text = "Personajes";
            this.ResumeLayout(false);
        }
    }
}
