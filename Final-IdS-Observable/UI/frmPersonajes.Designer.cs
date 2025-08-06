namespace UI
{
    partial class frmPersonajes
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.FlowLayoutPanel flpCards;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.flpCards = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // flpCards
            // 
            this.flpCards.AutoScroll = true;
            this.flpCards.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpCards.Location = new System.Drawing.Point(0, 0);
            this.flpCards.Name = "flpCards";
            this.flpCards.Padding = new System.Windows.Forms.Padding(10);
            this.flpCards.Size = new System.Drawing.Size(800, 450);
            this.flpCards.TabIndex = 0;
            this.flpCards.WrapContents = true;
            this.flpCards.BackColor = System.Drawing.Color.White;
            // 
            // frmMainObservable
            // 
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.flpCards);
            this.IsMdiContainer = true;
            this.Name = "frmMainObservable";
            this.Text = "Personajes";
            this.ResumeLayout(false);
        }
    }
}