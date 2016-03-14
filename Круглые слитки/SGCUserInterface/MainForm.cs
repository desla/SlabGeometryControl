using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
        private bool isInformationProcessingCompleat = true;
        private object isInformationProcessingCompleatLocker = new object();

        private Timer slabListLoaderActivator = null;
        private BackgroundWorker slabsListLoader = null;
        private SlabsList slabsList = new SlabsList();
        private SGCSystemState lastState = SGCSystemState.WAITING;
        private bool isSlabListProcessingCompleat = true;
        private object isSlabListProcessingCompleatLocker = new object();

        private BackgroundWorker connectorWorker = null;

        private BackgroundWorker dimentionLoader = null;
        private DimentionResult[] dimentions = null;
        private int currentStandartSizeId = -1;

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

            connectorWorker = new BackgroundWorker();
            connectorWorker.DoWork += ConnectOrDisconnect;
            connectorWorker.RunWorkerCompleted += ConnectionCompleat;

            dimentionLoader = new BackgroundWorker();
            dimentionLoader.DoWork += LoadSlabDimentionResults;
            dimentionLoader.RunWorkerCompleted += LoadSlabDimentionResultsCompleat;

            AddLogInfo("GUI", "Успешная инициализация.");
            MakeClientInstance();

            if (Convert.ToBoolean(Connection.Default["autoConnect"])) {
                подключитьсяToolStripMenuItem_Click(sender, e);
            }
        }

        private void LoadSlabDimentionResultsCompleat(object sender, RunWorkerCompletedEventArgs e)
        {
            ShowDimentionResults(dimentions);
        }

        private void LoadSlabDimentionResults(object sender, DoWorkEventArgs e)
        {
            var slabId = (int) e.Argument;
            dimentions = client.GetDimentionResultsBySlabId(slabId);
        }

        private void ConnectOrDisconnect(object sender, DoWorkEventArgs e)
        {
            var isConnect = (bool)e.Argument;
            if (isConnect) {
                ConnectToService();
            }
            else {
                DisconnectoFromService();
            }
        }

        private void ConnectionCompleat(object sender, RunWorkerCompletedEventArgs e)
        {
            if (client != null && client.IsConnected) {
                отключитьсяToolStripMenuItem.Enabled = true;
                подключитьсяToolStripMenuItem.Enabled = false;
                toolStripStatusLabel1.Text = @"Подключено. Сессия: " + client.GetSessionId();
                AddLogInfo("Server", "Успешное подключение к серверу. Сессия: " + client.GetSessionId() + ".");
                informationLoaderActivator.Start();
                slabListLoaderActivator.Start();
            }
            else {
                informationLoaderActivator.Stop();
                slabListLoaderActivator.Stop();
                отключитьсяToolStripMenuItem.Enabled = false;
                подключитьсяToolStripMenuItem.Enabled = true;
                toolStripStatusLabel1.Text = @"Отключено от сервиса.";
                AddLogInfo("Server", "Отключение от сервера выполнено.");
                ClearFormInformation();
            }
        }

        private void ActivateInformationLoader(object sender, EventArgs e)
        {
            if (!informationLoader.IsBusy && isInformationProcessingCompleat) {
                if (client != null && client.IsConnected) {
                    informationLoader.RunWorkerAsync();
                }
            }
        }

        private void ActivateSlabsListLoader(object sender, EventArgs e)
        {
            if (!slabsListLoader.IsBusy && isSlabListProcessingCompleat) {
                if (client != null && client.IsConnected) {
                    slabsListLoader.RunWorkerAsync();
                }
            }
        }

        private void SlabsListLoadingCompleat(object sender, RunWorkerCompletedEventArgs e)
        {
            lock (isSlabListProcessingCompleatLocker) {
                if (!isSlabListProcessingCompleat) {
                    return;
                }

                isSlabListProcessingCompleat = false;
            }            
            
            lock (slabsList) {                
                if (slabsList.Slabs != null && !IsNeedToClearDataGridView()) {
                    var slabs = slabsList.Slabs;                    
                    for (var i = 0; i < slabs.Length; ++i) {
                        if (client != null) {
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
                                        isAccepted ? "Соответствует" : "Не соответствует",
                                        slabInfo.StandartSizeId.ToString()
                                    };
                                    try {
                                        var newRowIndex = dataGridView1.Rows.Add(row);
                                        UpdateRegulationAccepted(newRowIndex, slabInfo);
                                    }
                                    catch {
                                    }
                                }
                                else {
                                    CheckStandartSizeUpdated(rowIndex, slabInfo);
                                }
                            }
                        }
                        else {
                            ClearFormInformation();
                        }
                    }
                }
                else {                    
                    ClearFormInformation();
                }
            }

            lock (isSlabListProcessingCompleatLocker) {
                isSlabListProcessingCompleat = true;
            }
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
            if (slabsList.Regulations != null && aSlabInfo != null && client != null) {
                var dimentionResults = client.GetDimentionResultsBySlabId(aSlabInfo.Id);
                if (dimentionResults != null) {
                    for (var i = 0; i < slabsList.Regulations.Length; ++i) {
                        var regulation = slabsList.Regulations[i];
                        if (regulation.StandartSizeId == aSlabInfo.StandartSizeId) {
                            for (var j = 0; j < dimentionResults.Length; ++j) {
                                var dimentionResult = dimentionResults[j];
                                if (dimentionResult.DimentionId == regulation.DimentionId) {
                                    if (dimentionResult.Value < regulation.MinValue ||
                                        dimentionResult.Value > regulation.MaxValue) {
                                        return false;
                                    }
                                } // if
                            } // for
                        }                        
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
                row.Cells["standartSizeId"].Value = aSlabInfo.StandartSizeId;
                UpdateRegulationAccepted(aRowIndex, aSlabInfo);
            }
        }

        private void UpdateRegulationAccepted(int aRowIndex, SlabInfo aSlabInfo)
        {
            var row = dataGridView1.Rows[aRowIndex];
            if (CheckRegulationsAccepted(aSlabInfo)) {                
                row.Cells["Accepted"].Value = "Соответствует";
                row.DefaultCellStyle.BackColor = Color.White;
            }
            else {
                row.Cells["Accepted"].Value = "Не соответствует";
                row.DefaultCellStyle.BackColor = Color.LightGray;                
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
                lock (slabsList) {
                    slabsList.Slabs = null;
                    slabsList.Regulations = null;
                }
                if (client != null && client.IsConnected) {
                    client.Disconnect();
                }                
            }
        }

        private void InformationLoadingCompleat(object sender, RunWorkerCompletedEventArgs e)
        {
            lock (isInformationProcessingCompleatLocker) {
                if (!isInformationProcessingCompleat) {
                    return;
                }

                isInformationProcessingCompleat = false;
            }

            ControllerConnectionUpdate();
            SensorsCountUpdate();
            SGCStateUpdate();

            lock (isInformationProcessingCompleatLocker) {
                isInformationProcessingCompleat = true;
            }
        }

        private void InformationLoading(object sender, DoWorkEventArgs e)
        {
            try {
                information.ControllerConnectionState = client.GetConnectionState();
                information.SGCState = client.GetSGCSystemState();
                information.SensorsCount = client.GetSensorsCount();
            }
            catch {
            }
        }

        private void ClearFormInformation()
        {
            label2.Text = "-";
            label4.Text = "-";
            label10.Text = "-";
            label8.Text = "-";
            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();
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
            connectorWorker.RunWorkerAsync(true);
            AddLogInfo("GUI", "Подключение к сервису...");
            подключитьсяToolStripMenuItem.Enabled = false;            
        }

        private void отключитьсяToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            AddLogInfo("GUI", "Отключение от сервиса...");
            отключитьсяToolStripMenuItem.Enabled = false;            
            connectorWorker.RunWorkerAsync(false);            
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
        }

        private void ConnectToService()
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
            }
            catch (Exception ex) {
                MessageBox.Show(@"Ошибка при подключении к сервису: " + ex.Message);                
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
                var standartSizeValue = Convert.ToString(row.Cells["standartSizeId"].Value);
                currentStandartSizeId = -1;
                int.TryParse(standartSizeValue, out currentStandartSizeId);

                if (!dimentionLoader.IsBusy) {
                    dimentionLoader.RunWorkerAsync(slabId);
                }                
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
                    var rowIndex = dataGridView2.Rows.Add(row);
                    if (currentStandartSizeId != -1 && slabsList.Regulations != null) {
                        var gridRow = dataGridView2.Rows[rowIndex];
                        foreach (var regulation in slabsList.Regulations) {
                            if (regulation.StandartSizeId == currentStandartSizeId) {
                                if (dimentionResult.DimentionId == regulation.DimentionId) {
                                    if (dimentionResult.Value < regulation.MinValue ||
                                        dimentionResult.Value > regulation.MaxValue) {
                                        //gridRow.DefaultCellStyle.BackColor = Color.LightGray;
                                        gridRow.DefaultCellStyle.ForeColor = Color.Red;
                                        break;
                                    }
                                }
                            }
                        }
                    }                    
                }
            }            
        }

        private void типоразмерыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new StandartSizeForm(client).ShowDialog();
            lock (slabsList) {
                slabsList.StandartSizes = null;
            }
        }

        private void датчикиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new SensorsForm(client).ShowDialog();
        }

        private void правилаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new RegulationsForm(client).ShowDialog();
            lock (slabsList) {
                slabsList.Regulations = null;
            }            
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
                var standartSizeId = Convert.ToInt32(row.Cells["standartSizeId"].Value);
                ThreadPool.QueueUserWorkItem(state => 
                    new SlabVisualizationForm(slabId, standartSizeId, client, false).ShowDialog());                
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

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.ScrollToCaret();
        }

        public void OnConnected(IClient aClient, long aSessionId)
        {           
        }

        public void OnDisconnected(IClient aClient, long aSessionId)
        {
        }

        private void подключениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ConnectionSettingsForm().ShowDialog();
        }

        private void калибровкаДатчиковToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new CalibrationForm(client).ShowDialog();
        }

        private void отчетПоСлиткуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("report_.pdf");
        }

        private void измеренияСлиткаToolStripMenuItem1_Click(object sender, EventArgs e) {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) {             
        }
    }
}
