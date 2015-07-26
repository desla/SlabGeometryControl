using System;
using System.Diagnostics;
using System.Windows.Forms;
using Alvasoft.SlabGeometryControl;

namespace SGCUserInterface
{
    public partial class ConnectionSettingsForm : Form
    {
        private SGCClientImpl client;

        public ConnectionSettingsForm(SGCClientImpl aClient)
        {
            InitializeComponent();

            client = aClient;
            textBox1.Text = Connection.Default["host"].ToString();
            numericUpDown1.Value = Convert.ToInt32(Connection.Default["port"]);
        }        

        private void UpdateLinkText()
        {
            if (client == null) {
                return;
            }

            var addres = "http://{0}:{1}/{2}";
            linkLabel1.Text = string.Format(addres, textBox1.Text, numericUpDown1.Value, client.ServiceName);            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            UpdateLinkText();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            UpdateLinkText();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Connection.Default["host"] = textBox1.Text;
            Connection.Default["port"] = Convert.ToInt32(numericUpDown1.Value);
            Connection.Default.Save();
            this.Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(linkLabel1.Text);
        }        
    }
}
