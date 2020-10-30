//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.ComponentModel;

namespace CarManGUI
{
    public class Chart1 : Component 
    {
        //private System.Windows.Forms.Form _Form;

        public Chart1(System.Windows.Forms.Form pForm)
        {
            //_Form = pForm;

            this.Build();

            this.Chart.LegendY = "Sells (in thousands)";
            this.Chart.LegendX = "Months";
            this.Chart.Values = new[] { 10, 20, 30, 40, 25, 21, 11, 2, 28, 33, 18, 45 };
            this.Chart.Draw();
        }

        private void Build()
        {
            this.Chart = new ChartClass(width: 353,
                                    height: 191)
            {
                Dock = System.Windows.Forms.DockStyle.Fill
            };

            //_Form.Controls.Add(this.Chart);
            //_Form.MinimumSize = new Size(ChartCanvasSize, ChartCanvasSize);
            //_Form.Text = this.GetType().Name;
        }

        /// <summary>
        /// Gets the <see cref="Chart"/>.
        /// </summary>
        /// <value>The chart.</value>
        public ChartClass Chart
        {
            get; private set;
        }
    }
}