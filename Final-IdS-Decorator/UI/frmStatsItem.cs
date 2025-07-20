using BE;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace UI
{
    public class frmStatsItem : Form
    {
        public frmStatsItem(Item item)
        {
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.StartPosition = FormStartPosition.Manual;
            this.Size = new Size(180, 150);
            this.BackColor = Color.Beige;

            var contenedor = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 1
            };

            contenedor.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            contenedor.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            contenedor.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            var lblNombre = new Label
            {
                Text = item.Nombre,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Padding = new Padding(5),
                AutoSize = false,
                Height = 30
            };

            var lblStats = new Label
            {
                Text = $"Poder: {item.Poder}\nDefensa: {item.Defensa}\nAtributo: {item.AtributoPpl}",
                Font = new Font("Segoe UI", 9),
                TextAlign = ContentAlignment.TopLeft,
                Dock = DockStyle.Fill,
                Padding = new Padding(10)
            };

            contenedor.Controls.Add(lblNombre, 0, 0);
            contenedor.Controls.Add(lblStats, 0, 1);

            this.Controls.Add(contenedor);
        }
    }
}
