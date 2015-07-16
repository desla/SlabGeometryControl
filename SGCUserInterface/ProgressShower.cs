using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SGCUserInterface
{
    public partial class ProgressShower : UserControl
    {
        public ProgressShower()
        {
            InitializeComponent();
        }

        private void ProgressShower_Load(object sender, EventArgs e)
        {
        }

        public void UpdatePercents(int aPercents)
        {
            this.label1.Text = aPercents + @" %";
        }

        private void ProgressShower_SizeChanged(object sender, EventArgs e)
        {
            panel1.Top = this.Height/2 - panel1.Height/2;
            panel1.Left = this.Width/2 - panel1.Width/2;
        }
    }
}
