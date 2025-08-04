namespace UI
{
    partial class frmMainObservable
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            flpCards = new FlowLayoutPanel();
            SuspendLayout();
            // 
            // flpCards
            // 
            flpCards.AutoScroll = true;
            flpCards.Dock = DockStyle.Fill;
            flpCards.Location = new Point(0, 0);
            flpCards.Margin = new Padding(3, 2, 3, 2);
            flpCards.Name = "flpCards";
            flpCards.Padding = new Padding(9, 8, 9, 8);
            flpCards.Size = new Size(700, 338);
            flpCards.TabIndex = 0;
            // 
            // frmMainObservable
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(700, 338);
            Controls.Add(flpCards);
            Margin = new Padding(3, 2, 3, 2);
            Name = "frmMainObservable";
            Text = "Mostrar Personajes";
            Load += frmMostrarPersonajes_Load;
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flpCards;
    }
}
