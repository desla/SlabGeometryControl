using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Alvasoft.SlabGeometryControl;

namespace SGCUserInterface
{
    public partial class DimentionForm : Form
    {
        private SGCClientImpl client = null;

        public DimentionForm(SGCClientImpl aClient)
        {
            InitializeComponent();

            client = aClient;            
        }

        private void DimentionForm_Load(object sender, EventArgs e)
        {
            if (client != null && client.IsConnected) {
                var dimentions = client.GetDimentions();
                foreach (var dimention in dimentions) {
                    var row = new[] {
                        dimention.Id.ToString(),
                        dimention.Description,
                        "true"
                    };
                    dataGridView1.Rows.Add(row);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
