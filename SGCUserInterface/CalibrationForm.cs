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
    public partial class CalibrationForm : Form
    {
        private class InformationBag
        {
            public double FirstSensorValue { get; set; }
            public double SecondSensor { get; set; }
            public int FirstSensorId { get; set; }
            public int SecondSensorId { get; set; }
        }

        private SGCClientImpl client;
        private InformationBag bag = new InformationBag();
        private SensorInfo[] sensors;

        private BackgroundWorker loader;
        private Timer loaderActivator;

        public CalibrationForm(SGCClientImpl aClient)
        {
            InitializeComponent();

            client = aClient;
            loader = new BackgroundWorker();
            loader.DoWork += LoadCurrentValues;
            loader.RunWorkerCompleted += LoadCompleated;

            loaderActivator = new Timer();
            loaderActivator.Tick += StartLoading;
            loaderActivator.Interval = 1000;
        }

        private void StartLoading(object sender, EventArgs e)
        {
            if (client != null && client.IsConnected) {
                if (!loader.IsBusy) {
                    loader.RunWorkerAsync();
                }
            }
        }

        private void LoadCompleated(object sender, RunWorkerCompletedEventArgs e)
        {
            textBox1.Text = bag.FirstSensorValue.ToString();
            textBox2.Text = bag.SecondSensor.ToString();
        }

        private void LoadCurrentValues(object sender, DoWorkEventArgs e)
        {
            try {                                    
                var firstValue = client.GetSensorValueBySensorId(bag.FirstSensorId);
                var secondValue = client.GetSensorValueBySensorId(bag.SecondSensorId);
                bag.FirstSensorValue = firstValue != null ? firstValue.Value : double.NaN;
                bag.SecondSensor = secondValue != null ? secondValue.Value : double.NaN;                                
            }
            catch (Exception ex) {
                MessageBox.Show(@"Ошибка при обновлении информации: " + ex.Message);
                if (client != null && client.IsConnected) {
                    client.Disconnect();
                }
            }
        }

        private int GetSensorId(string aSensorName)
        {
            if (sensors != null && !string.IsNullOrEmpty(aSensorName)) {
                for (var i = 0; i < sensors.Length; ++i) {
                    if (sensors[i].Name == aSensorName) {
                        return sensors[i].Id;
                    }
                }
            }

            return -1;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void CalibrationForm_Load(object sender, EventArgs e)
        {
            if (client == null || !client.IsConnected) {
                return;
            }
            try {
                sensors = client.GetSensorInfos();
                if (sensors != null) {
                    for (var i = 0; i < sensors.Length; ++i) {
                        comboBox1.Items.Add(sensors[i].Name);
                        comboBox2.Items.Add(sensors[i].Name);
                    }

                    if (sensors.Length > 1) {
                        comboBox1.SelectedIndex = 0;
                        comboBox2.SelectedIndex = 1;
                    }                    
                }                

                loaderActivator.Start();
            }
            catch (Exception ex) {
                MessageBox.Show(@"Ошибка при загрузке данных сервера: " + ex.Message);
            }
        }

        private void CalibrationForm_FormClosing(object sender, FormClosingEventArgs e)
        {            
            loaderActivator.Stop();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (client == null || !client.IsConnected) {
                MessageBox.Show(@"Клиент не подключен к серверу.");
                return;
            }
            
            var calibratedValue = 
                Convert.ToDouble(numericUpDown1.Value)
                + bag.FirstSensorValue 
                + bag.SecondSensor;

            try {
                client.SetCalibratedValue(bag.FirstSensorId, calibratedValue);
                client.SetCalibratedValue(bag.SecondSensorId, calibratedValue);
                MessageBox.Show(@"Калибровка прошла успешно. Калибровочное значение: " + calibratedValue);
            }
            catch (Exception ex) {
                MessageBox.Show(@"Ошибка при калибровании: " + ex.Message);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            bag.FirstSensorId = GetSensorId(comboBox1.Text);            
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            bag.SecondSensorId = GetSensorId(comboBox2.Text);
        }
    }
}
