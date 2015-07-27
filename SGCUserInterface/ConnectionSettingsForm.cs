using System;
using System.Diagnostics;
using System.Windows.Forms;
using Alvasoft.SlabGeometryControl;
using Alvasoft.Wcf.NetConfiguration;

namespace SGCUserInterface
{
    public partial class ConnectionSettingsForm : Form
    {
        public ConnectionSettingsForm()
        {
            InitializeComponent();

            textBox1.Text = Connection.Default["host"].ToString();
            numericUpDown1.Value = Convert.ToInt32(Connection.Default["port"]);
        }        

        private void UpdateLinkText()
        {            
            var serviceName = SGCClientImpl.CurrentServiceName;

            var addres = "http://{0}:8080/{1}";
            linkLabel1.Text = string.Format(addres, textBox1.Text, serviceName);            
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

        private void ConnectionSettingsForm_Load(object sender, EventArgs e)
        {

        }        
    }
}
