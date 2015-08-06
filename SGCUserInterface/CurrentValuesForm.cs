using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Alvasoft.SlabGeometryControl;
using ZedGraph;
using Timer = System.Windows.Forms.Timer;

namespace SGCUserInterface
{
    public partial class CurrentValuesForm : Form
    {
        private SGCClientImpl client = null;        
        private Random rnd = new Random(14121989);
        private long currentTime = 0;
        private BackgroundWorker loader = null;
        private CurrentInformation information = new CurrentInformation();
        private Timer loaderStarter = new Timer();

        public CurrentValuesForm(SGCClientImpl aClient)
        {
            InitializeComponent();

            client = aClient;            
        }

        private void CurrentValuesForm_Load(object sender, EventArgs e)
        {
            MakeGraphPane();

            if (client != null && client.IsConnected) {
                var sensors = client.GetSensorInfos();
                information.Sensors = sensors;
                information.Values = new double[sensors.Length];
                for (var i = 0; i < sensors.Length; ++i) {
                    var sensor = sensors[i];                    
                    var row = new object[] {
                        sensor.Id,
                        sensor.Name,
                        "-"
                    };

                    dataGridView1.Rows.Add(row);
                    var pane = plotsView.GraphPane;
                    pane.AddCurve(
                        sensor.Name, 
                        new PointPairList(), 
                        Color.FromArgb(rnd.Next(255), rnd.Next(255), rnd.Next(255)), 
                        SymbolType.None);
                }
                loader = new BackgroundWorker();
                loader.DoWork += InformationLoad;
                loader.RunWorkerCompleted += InformationLoadCompleat;
                loader.ProgressChanged += ProgressChanged;
                loader.WorkerReportsProgress = true;
                loader.WorkerSupportsCancellation = true;
                loader.RunWorkerAsync();

                loaderStarter.Tick += StartLoader;
                loaderStarter.Interval = 1;
                loaderStarter.Start();
            }            
        }

        private void MakeGraphPane()
        {
            plotsView.IsShowPointValues = true;
            plotsView.GraphPane = new GraphPane(
                new RectangleF(plotsView.Location.X,
                               plotsView.Location.Y,
                               plotsView.Size.Width,
                               plotsView.Size.Height),
                "Текущие показания датчиков", "Шаг мониторинга", "Показание датчика (мм)");
            var pane = plotsView.GraphPane;
            // Включаем отображение сетки напротив крупных рисок по оси X
            pane.XAxis.MajorGrid.IsVisible = true;
            // Задаем вид пунктирной линии для крупных рисок по оси X:
            // Длина штрихов равна 10 пикселям, ... 
            pane.XAxis.MajorGrid.DashOn = 1;
            // затем 5 пикселей - пропуск
            pane.XAxis.MajorGrid.DashOff = 2;
            // Включаем отображение сетки напротив крупных рисок по оси Y
            pane.YAxis.MajorGrid.IsVisible = true;
            // Аналогично задаем вид пунктирной линии для крупных рисок по оси Y
            pane.YAxis.MajorGrid.DashOn = 1;
            pane.YAxis.MajorGrid.DashOff = 2;
            // Включаем отображение сетки напротив мелких рисок по оси X
            pane.YAxis.MinorGrid.IsVisible = true;
            // Задаем вид пунктирной линии для крупных рисок по оси Y: 
            // Длина штрихов равна одному пикселю, ... 
            pane.YAxis.MinorGrid.DashOn = 1;
            // затем 2 пикселя - пропуск
            pane.YAxis.MinorGrid.DashOff = 2;
            // Включаем отображение сетки напротив мелких рисок по оси Y
            pane.XAxis.MinorGrid.IsVisible = true;
            // Аналогично задаем вид пунктирной линии для крупных рисок по оси Y
            pane.XAxis.MinorGrid.DashOn = 1;
            pane.XAxis.MinorGrid.DashOff = 2;
        }

        private void StartLoader(object sender, EventArgs e)
        {
            if (!loader.IsBusy) {
                if (client != null && client.IsConnected) {
                    loader.RunWorkerAsync();
                }
            }
        }

        private void ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (toolStripProgressBar1.ProgressBar != null) {
                toolStripProgressBar1.ProgressBar.Value = e.ProgressPercentage;
            }            
        }

        private void InformationLoadCompleat(object sender, RunWorkerCompletedEventArgs e)
        {
            try {
                UpdateSensorValues();
                UpdatePlots();                      
            }
            catch (Exception ex) {
                MessageBox.Show(@"Ошибка при обновлении инфомрации: " + ex.Message);
            }
        }

        private void InformationLoad(object sender, DoWorkEventArgs e)
        {
            try {
                if (client != null && client.IsConnected) {
                    var sensors = information.Sensors;
                    var sensorPercents = 100/sensors.Length;
                    for (var i = 0; i < sensors.Length; ++i) {
                        var sensor = sensors[i];
                        var sensorValue = client.GetSensorValueBySensorId(sensor.Id);
                        if (sensorValue != null) {
                            information.Values[i] = sensorValue.Value;
                            loader.ReportProgress(i*sensorPercents);
                        }
                    }
                }
            }
            catch (Exception ex) {
                information.Values = null;
                if (client != null) {
                    client.Disconnect();
                }
            }
        }

        private void UpdateSensorValues()
        {
            if (information.Values != null) {
                for (var i = 0; i < information.Sensors.Length; ++i) {
                    var row = dataGridView1.Rows[i];
                    row.Cells["Value"].Value = information.Values[i];
                }
            }
        }

        private void UpdatePlots()
        {            
            if (information.Values != null) {
                var pane = plotsView.GraphPane;
                for (var i = 0; i < information.Sensors.Length; ++i) {
                    pane.CurveList[i].AddPoint(currentTime, information.Values[i]);
                }
                currentTime++;
                plotsView.AxisChange();
                plotsView.Invalidate();            
            }                                    
        }

        private void plotsView_ContextMenuBuilder(ZedGraphControl sender, ContextMenuStrip menuStrip, Point mousePt, ZedGraphControl.ContextMenuObjectState objState)
        {
            menuStrip.Items[0].Text = @"Копировать";
            menuStrip.Items[1].Text = @"Сохранить как картинку";
            menuStrip.Items[2].Text = @"Параметры страницы";
            menuStrip.Items[3].Text = @"Печать";
            menuStrip.Items[4].Text = @"Показывать значения в точках";
            menuStrip.Items[7].Text = @"Установить масштаб по умолчаню";
            menuStrip.Items.RemoveAt(5);
            menuStrip.Items.RemoveAt(5);
        }

        private void CurrentValuesForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            loaderStarter.Stop();
        }
    }
}
