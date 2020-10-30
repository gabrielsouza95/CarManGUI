using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CarManGUI
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            //InitializeComponent();
            /*this.Build();

            this.Chart.LegendY = "Sells (in thousands)";
            this.Chart.LegendX = "Months";
            this.Chart.Values = new[] { 10, 20, 30, 40, 25, 21, 11, 2, 28, 33, 18, 45 };
            this.Chart.Draw();*/
        }

        public const int ChartCanvasSize = 512;

        /*private void Build()
        {
            this.Chart = new ChartClass(width: ChartCanvasSize,
                                    height: ChartCanvasSize)
            {
                Dock = DockStyle.Fill,
            };

            this.Controls.Add(this.Chart);
            this.MinimumSize = new Size(ChartCanvasSize, ChartCanvasSize);
            this.Text = this.GetType().Name;
        }

        /// <summary>
        /// Gets the <see cref="Chart"/>.
        /// </summary>
        /// <value>The chart.</value>
        public ChartClass Chart
        {
            get; private set;
        }*/

        private void Form2_Load(object sender, EventArgs e)
        {

        }
    }
}
