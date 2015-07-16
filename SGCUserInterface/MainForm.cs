using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Alvasoft.Wcf.Client;
using Alvasoft.Wcf.NetConfiguration;
using Alvasoft.SlabGeometryControl;
using Timer = System.Windows.Forms.Timer;

namespace SGCUserInterface
{
    public partial class MainForm : Form, IClientActionsListener
    {
        private SGCClientImpl client = null;
        private DateTime lastScanTime = DateTime.Now;        
        private Random rnd = new Random(14121989);        
        private Dictionary<int, string> dimentionTitles = null;

        private BackgroundWorker informationLoader = null;
        private SystemInformation information = new SystemInformation();

        private BackgroundWorker slabsListLoader = null;
        private SlabsList slabsList = new SlabsList();
        //private Timer updateInformationTimer = null;

        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dateTimeFrom.Value = DateTime.Now.Date;
            dateTimeTo.Value = DateTime.Now.Date.AddDays(1);

            informationLoader = new BackgroundWorker();
            informationLoader.DoWork += InformationLoading;
            informationLoader.RunWorkerCompleted += InformationLoadingCompleat;            

            slabsListLoader = new BackgroundWorker();
            slabsListLoader.DoWork += SlabsListLoading;
            slabsListLoader.RunWorkerCompleted += SlabsListLoadingCompleat;            

            ConnectToService();
        }

        private void SlabsListLoadingCompleat(object sender, RunWorkerCompletedEventArgs e)
        {
            if (slabsList.Slabs != null) {
                var slabs = slabsList.Slabs;
                for (var i = 0; i < slabs.Length; ++i) {
                    var slabInfo = slabs[i];
                    var slabFilter = slabNumberFilter.Text.Trim();
                    if (string.IsNullOrEmpty(slabFilter) || slabFilter.Equals(slabInfo.Number)) {
                        if (!IsContainSlabInfo(slabInfo.Id)) {
                            string[] row = {
                                slabInfo.Id.ToString(),
                                "#######", //slabInfo.Number,
                                slabInfo.StandartSizeId.ToString(),
                                DateTime.FromBinary(slabInfo.StartScanTime).ToString(),
                                "Соответствие"
                            };
                            try {
                                dataGridView1.Rows.Add(row);
                            }
                            catch {
                            }
                        }
                    }
                }
            }
            else {
                try {
                    dataGridView1.Rows.Clear();
                    dataGridView2.Rows.Clear();
                }
                catch {
                }
            }

            if (client != null && client.IsConnected) {
                slabsListLoader.RunWorkerAsync();
            }            
        }

        private void SlabsListLoading(object sender, DoWorkEventArgs e)
        {
            try {
                var from = dateTimeFrom.Value.ToLocalTime().ToBinary();
                var to = dateTimeTo.Value.ToLocalTime().ToBinary();
                slabsList.Slabs = client.GetSlabInfosByTimeInterval(from, to);
            }
            catch (Exception ex) {
                MessageBox.Show(@"Ошибка при обновлении информации: " + ex.Message);
                client.Disconnect();
            }
        }

        private void InformationLoadingCompleat(object sender, RunWorkerCompletedEventArgs e)
        {
            ControllerConnectionUpdate();
            SensorsCountUpdate();
            SGCStateUpdate();

            if (client != null && client.IsConnected) {
                informationLoader.RunWorkerAsync();
            }            
        }

        private void InformationLoading(object sender, DoWorkEventArgs e)
        {
            try {
                information.ControllerConnectionState = client.GetConnectionState();
                information.SGCState = client.GetSGCSystemState();
                information.SensorsCount = client.GetSensorsCount();
            }
            catch (Exception ex) {                
            }
        }

        public void OnConnected(IClient aClient, long aSessionId)
        {
            toolStripStatusLabel1.Text = @"Подключено. Сессия: " + aSessionId;
            slabsListLoader.RunWorkerAsync();
            informationLoader.RunWorkerAsync();
        }

        public void OnDisconnected(IClient aClient, long aSessionId)
        {            
            information.ControllerConnectionState = ControllerConnectionState.DISCONNECTED;
            information.SGCState = SGCSystemState.WAITING;
            information.SensorsCount = 0;
            slabsList.Slabs = null;            
        }        

        private bool IsContainSlabInfo(int aSlabId)
        {
            for (var i = 0; i < dataGridView1.RowCount; ++i) {
                if (Convert.ToInt32(dataGridView1.Rows[i].Cells["Id"].Value) == aSlabId) {
                    return true;
                }
            }

            return false;
        }

        private void ControllerConnectionUpdate()
        {
            if (information.ControllerConnectionState == ControllerConnectionState.CONNECTED) {
                label2.Text = @"Подерживается";
                label2.ForeColor = Color.Green;
            }
            else {
                label2.Text = @"Отключено";
                label2.ForeColor = Color.Red;
            }
        }

        private void SensorsCountUpdate()
        {
            label8.Text = information.SensorsCount.ToString();
        }

        private void SGCStateUpdate()
        {
            if (information.SGCState == SGCSystemState.WAITING) {
                label4.Text = @"Ожидание слитка";
                label4.ForeColor = Color.Brown;
                lastScanTime = DateTime.Now;
                label10.Text = @"0:0 (мм:сс)";
            }
            else {
                label4.Text = @"Идет сканирование";
                label4.ForeColor = Color.Blue;
                var timeDifference = DateTime.Now - lastScanTime;
                label10.Text = timeDifference.Minutes + @":" + timeDifference.Seconds + @" (мм:сс)";
            }
        }

        private void подключитьсяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ConnectToService()) {
                подключитьсяToolStripMenuItem.Enabled = false;
                отключитьсяToolStripMenuItem.Enabled = true;
            }            
        }

        private void отключитьсяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DisconnectoFromService();            
        }

        private void DisconnectoFromService()
        {
            try {
                if (client != null && client.IsConnected) {
                    client.Disconnect();                    
                }
            }
            finally {
                client = null;                
            }

            подключитьсяToolStripMenuItem.Enabled = true;
            отключитьсяToolStripMenuItem.Enabled = false;
        }

        private bool ConnectToService()
        {
            var configuration = new NetConfigurationImpl {
                ServerHost = "192.168.1.66",
                ServerPort = 9876
            };

            try {
                client = new SGCClientImpl(configuration);
                client.ConnectionActionsListener = this;
                client.Connect();

                dimentionTitles = new Dictionary<int, string>();
                var dimentions = client.GetDimentions();
                if (dimentions != null) {
                    foreach (var dimention in dimentions) {
                        dimentionTitles[dimention.Id] = dimention.Description;
                    }
                }
                return true;
            }
            catch (Exception ex) {
                MessageBox.Show(@"Ошибка при подключении к сервису: " + ex.Message);                
                return false;
            }
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {            
            if (e.RowIndex >= 0 && client != null && client.IsConnected) {
                var row = dataGridView1.Rows[e.RowIndex];
                var slabId = Convert.ToInt32(row.Cells["Id"].Value);
                var dimentions = client.GetDimentionResultsBySlabId(slabId);
                ShowDimentionResults(dimentions);
            }            
        }

        private void ShowDimentionResults(DimentionResult[] aDimentions)
        {            
            dataGridView2.Rows.Clear();
            if (aDimentions != null) {
                foreach (var dimentionResult in aDimentions) {
                    var row = new[] {
                        dimentionTitles[dimentionResult.DimentionId], 
                        dimentionResult.Value.ToString()
                    };
                    dataGridView2.Rows.Add(row);
                }
            }            
        }

        private void типоразмерыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new StandartSizeForm(client).ShowDialog();
        }

        private void измеренияСлиткаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new DimentionForm(client).ShowDialog();
        }

        private void датчикиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new SensorsForm(client).ShowDialog();
        }

        private void правилаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new RegulationsForm(client).ShowDialog();
        }

        private void теукщиеПоказанияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new CurrentValuesForm(client).ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new CurrentValuesForm(client).ShowDialog();
        }

        private void просмотрToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var rowIndex = -1;
            if (dataGridView1.SelectedCells.Count > 0) {
                rowIndex = dataGridView1.SelectedCells[0].RowIndex;
            }

            if (rowIndex != -1) {
                var row = dataGridView1.Rows[rowIndex];
                var slabId = Convert.ToInt32(row.Cells["Id"].Value);
                new Thread(() => new SlabVisualizationForm(slabId, client).ShowDialog()).Start();
            }
        }

        private void dateTimeTo_ValueChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
        }
    }
}
