using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
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

        private Timer informationLoaderActivator = null;
        private BackgroundWorker informationLoader = null;
        private SystemInformation information = new SystemInformation();

        private Timer slabListLoaderActivator = null;
        private BackgroundWorker slabsListLoader = null;
        private SlabsList slabsList = new SlabsList();
        private SGCSystemState lastState = SGCSystemState.WAITING;        

        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dateTimeFrom.Value = DateTime.Now.Date;            
            informationLoader = new BackgroundWorker();
            informationLoader.DoWork += InformationLoading;
            informationLoader.RunWorkerCompleted += InformationLoadingCompleat;            

            slabsListLoader = new BackgroundWorker();
            slabsListLoader.DoWork += SlabsListLoading;
            slabsListLoader.RunWorkerCompleted += SlabsListLoadingCompleat;  
          
            informationLoaderActivator = new Timer();
            informationLoaderActivator.Tick += ActivateInformationLoader;
            informationLoaderActivator.Interval = 1000;

            slabListLoaderActivator = new Timer();
            slabListLoaderActivator.Tick += ActivateSlabsListLoader;
            slabListLoaderActivator.Interval = 2000;            


            AddLogInfo("GUI", "Успешная инициализация.");
            MakeClientInstance();
            //ConnectToService();
        }

        private void ActivateInformationLoader(object sender, EventArgs e)
        {
            if (!informationLoader.IsBusy) {
                if (client != null && client.IsConnected) {
                    informationLoader.RunWorkerAsync();
                }
            }            
            //informationLoaderActivator.Stop();
        }

        private void ActivateSlabsListLoader(object sender, EventArgs e)
        {
            if (!slabsListLoader.IsBusy) {
                if (client != null && client.IsConnected) {
                    slabsListLoader.RunWorkerAsync();
                }
            }            
            //slabListLoaderActivator.Stop();
        }

        private void SlabsListLoadingCompleat(object sender, RunWorkerCompletedEventArgs e)
        {
            if (slabsList.Slabs != null && !IsNeedToClearDataGridView()) {
                var slabs = slabsList.Slabs;
                for (var i = 0; i < slabs.Length; ++i) {
                    Application.DoEvents();
                    var slabInfo = slabs[i];
                    var slabFilter = slabNumberFilter.Text.Trim();
                    if (string.IsNullOrEmpty(slabFilter) || 
                        slabFilter.Equals(slabInfo.Number)) {
                        var rowIndex = GetRowIndex(slabInfo.Id);                        
                        if (rowIndex == -1) {
                            var isAccepted = CheckRegulationsAccepted(slabInfo);
                            string[] row = {
                                slabInfo.Id.ToString(),
                                "########", //slabInfo.Number,
                                GetStandartSizeById(slabInfo.StandartSizeId),
                                DateTime.FromBinary(slabInfo.StartScanTime).ToString(),
                                isAccepted ? "Соответствует" : "Не соответствует"
                            };
                            try {
                                var newRowIndex = dataGridView1.Rows.Add(row);
                                if (!isAccepted) {
                                    dataGridView1.Rows[newRowIndex].DefaultCellStyle.BackColor = Color.Khaki;
                                }
                            }
                            catch {
                            }
                        }
                        else {
                            CheckStandartSizeUpdated(rowIndex, slabInfo);
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

            slabListLoaderActivator.Start();
        }

        private bool IsNeedToClearDataGridView()
        {
            if (dataGridView1.RowCount > 0) {
                var from = dateTimeFrom.Value;
                var to = dateTimeFrom.Value.AddDays(1);
                var value = DateTime.Parse(dataGridView1.Rows[0].Cells["ScanTime"].Value.ToString());
                if (value < from || value > to) {                    
                    return true;
                }
            }

            return false;
        }

        private bool CheckRegulationsAccepted(SlabInfo aSlabInfo)
        {            
            if (slabsList.Regulations != null && aSlabInfo != null) {
                var dimentionResults = client.GetDimentionResultsBySlabId(aSlabInfo.Id);
                if (dimentionResults != null) {
                    for (var i = 0; i < slabsList.Regulations.Length; ++i) {
                        var regulation = slabsList.Regulations[i];
                        for (var j = 0; j < dimentionResults.Length; ++j) {
                            var dimentionResult = dimentionResults[j];
                            if (dimentionResult.DimentionId == regulation.DimentionId) {
                                if (dimentionResult.Value < regulation.MinValue ||
                                    dimentionResult.Value > regulation.MaxValue) {
                                    return false;
                                }
                            } // if
                        } // for
                    } // for
                } // if                
            } // if

            return true;
        }

        private void CheckStandartSizeUpdated(int aRowIndex, SlabInfo aSlabInfo)
        {
            if (aRowIndex < 0 || aSlabInfo == null) {
                return;
            }

            var row = dataGridView1.Rows[aRowIndex];
            var standartSizeText = GetStandartSizeById(aSlabInfo.StandartSizeId);
            if (!Equals(row.Cells["StandartSize"].Value, standartSizeText)) {
                row.Cells["StandartSize"].Value = standartSizeText;                
            }
        }

        private string GetStandartSizeById(int aStandartSizeId)
        {
            if (slabsList.StandartSizes != null) {
                for (var i = 0; i < slabsList.StandartSizes.Length; ++i) {
                    if (aStandartSizeId == slabsList.StandartSizes[i].Id) {
                        return slabsList.StandartSizes[i].ToString();
                    }
                }
            }

            return "не определен";
        }

        private void SlabsListLoading(object sender, DoWorkEventArgs e)
        {
            try {
                var from = dateTimeFrom.Value.ToLocalTime().ToBinary();
                var to = dateTimeFrom.Value.AddDays(1).ToLocalTime().ToBinary();
                slabsList.Slabs = client.GetSlabInfosByTimeInterval(from, to);
                if (slabsList.StandartSizes == null) {
                    slabsList.StandartSizes = client.GetStandartSizes();
                }
                if (slabsList.Regulations == null) {
                    slabsList.Regulations = client.GetRegulations();
                }
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
            
            informationLoaderActivator.Start();
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
            AddLogInfo("Server", "Успешное подключение к серверу. Сессия: " + aSessionId + ".");
            //slabsListLoader.RunWorkerAsync();
            //informationLoader.RunWorkerAsync();
            slabListLoaderActivator.Start();
            informationLoaderActivator.Start();
        }

        public void OnDisconnected(IClient aClient, long aSessionId)
        {            
            information.ControllerConnectionState = ControllerConnectionState.DISCONNECTED;
            information.SGCState = SGCSystemState.WAITING;
            information.SensorsCount = 0;
            slabsList.Slabs = null;            
        }        

        private int GetRowIndex(int aSlabId)
        {
            for (var i = 0; i < dataGridView1.RowCount; ++i) {
                if (Convert.ToInt32(dataGridView1.Rows[i].Cells["Id"].Value) == aSlabId) {
                    return i;
                }
            }

            return -1;
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
                if (lastState == SGCSystemState.SCANNING) {
                    AddLogInfo("Server", "Сканирование закончено.");
                }
                label4.Text = @"Ожидание слитка";
                label4.ForeColor = Color.Brown;
                lastScanTime = DateTime.Now;
                label10.Text = @"0:0 (мм:сс)";
            }
            else {
                if (lastState == SGCSystemState.WAITING) {
                    AddLogInfo("Server", "Новое сканирование начато.");
                }
                label4.Text = @"Идет сканирование";
                label4.ForeColor = Color.Blue;
                var timeDifference = DateTime.Now - lastScanTime;
                label10.Text = timeDifference.Minutes + @":" + timeDifference.Seconds + @" (мм:сс)";                
            }

            lastState = information.SGCState;
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

            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();
            подключитьсяToolStripMenuItem.Enabled = true;
            отключитьсяToolStripMenuItem.Enabled = false;
            AddLogInfo("GUI", "Отключение от сервера.");
        }

        private bool ConnectToService()
        {
            try {
                MakeClientInstance();
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
                AddLogInfo("GUI", "Ошибка подключения к серверу.");
                return false;
            }
        }

        private void MakeClientInstance()
        {
            var configuration = new NetConfigurationImpl {
                ServerHost = Connection.Default["host"].ToString(),
                ServerPort = Convert.ToInt32(Connection.Default["port"])
            };

            client = new SGCClientImpl(configuration);
            client.ConnectionActionsListener = this;
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
            slabsList.StandartSizes = null;
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
            slabsList.Regulations = null;
        }

        private void теукщиеПоказанияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddLogInfo("GUI", "Просмотр текущих показаний датчиков.");
            new CurrentValuesForm(client).ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AddLogInfo("GUI", "Просмотр текущих показаний датчиков.");
            new CurrentValuesForm(client).ShowDialog();
        }

        private void просмотрToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddLogInfo("GUI", "Просмотр данных слитка.");
            var rowIndex = -1;
            if (dataGridView1.SelectedCells.Count > 0) {
                rowIndex = dataGridView1.SelectedCells[0].RowIndex;
            }

            if (rowIndex != -1) {
                var row = dataGridView1.Rows[rowIndex];
                var slabId = Convert.ToInt32(row.Cells["Id"].Value);
                ThreadPool.QueueUserWorkItem(state => 
                    new SlabVisualizationForm(slabId, client).ShowDialog());                
            }
        }        

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (client != null && client.IsConnected) {
                client.Disconnect();
            }

            this.Close();
        }

        private void оПрограммеToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            new AboutForm().ShowDialog();
        }

        private void AddLogInfo(string aHandle, string aMessage)
        {
            var time = DateTime.Now;
            richTextBox1.AppendText(time + " [" + aHandle + "] " + aMessage + "\n");
        }

        private void просмотрВыбранногоСлиткаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            просмотрToolStripMenuItem_Click(sender, e);
        }

        private void настройкиПодключенияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ConnectionSettingsForm(client).ShowDialog();
        }        
    }
}
