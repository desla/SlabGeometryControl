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
    public partial class EditOpcTagsForm : Form
    {
        private SGCClientImpl client = null;
        private int sensorId = -1;

        public EditOpcTagsForm(SGCClientImpl aClient, int aSensorId)
        {
            InitializeComponent();

            client = aClient;
            sensorId = aSensorId;
        }

        private void EditOpcTagsForm_Load(object sender, EventArgs e)
        {

        }
    }
}
