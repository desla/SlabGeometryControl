using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Alvasoft.SlabGeometryControl;
using ZedGraph;

namespace SGCUserInterface
{
    public partial class SlabVisualizationForm : Form
    {
        private SGCClientImpl client = null;
        private int slabId = -1;
        private Random rnd = new Random(14121989);
        private PointF[][] points = null;
        private BackgroundWorker loader = new BackgroundWorker();
        private ProgressShower progressShower = null;

        public SlabVisualizationForm(int aSlabId, SGCClientImpl aClient)
        {
            InitializeComponent();

            client = aClient;
            slabId = aSlabId;

            loader.DoWork += LoadSensorsValues;
            loader.RunWorkerCompleted += LoadCompleted;
            loader.ProgressChanged += LoadProgressChanged;
            loader.WorkerReportsProgress = true;
        }

        private void LoadProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (progressShower != null) {
                progressShower.UpdatePercents(e.ProgressPercentage);
            }
        }

        private void LoadCompleted(object sender, RunWorkerCompletedEventArgs e)
        {            
            var pane = plotsView.GraphPane;
            for (var i = 0; i < points.Length; ++i) {
                if (points[i] != null) {
                    for (var j = 0; j < points[i].Length; ++j) {
                        pane.CurveList[i].AddPoint(points[i][j].X / 1000, points[i][j].Y);
                    }
                }                
            }

            plotsView.AxisChange();
            plotsView.Invalidate();

            if (progressShower != null) {
                progressShower.Dispose();
                progressShower = null;
            }

            for (var i = 0; i < this.Controls.Count; ++i) {                
                Controls[i].Show();
            }
        }

        private void LoadSensorsValues(object sender, DoWorkEventArgs e)
        {
            var sensors = e.Argument as SensorInfo[];
            if (sensors != null) {
                var sensorPercens = 100 / sensors.Length;
                for (var i = 0; i < sensors.Length; ++i) {
                    var sensorValues = client.GetSensorValuesBySlabId(slabId, sensors[i].Id);                    
                    if (sensorValues != null && sensorValues.Length > 0) {
                        points[i] = new PointF[sensorValues.Length];
                        var startTime = DateTime.FromBinary(sensorValues[0].Time);
                        var piePercents = 10*sensorPercens/sensorValues.Length;
                        for (var j = 0; j < sensorValues.Length; ++j) {
                            var time = DateTime.FromBinary(sensorValues[j].Time).Subtract(startTime);
                            points[i][j] = new PointF((float) time.TotalMilliseconds, (float) sensorValues[j].Value);                            
                        }                        
                    }
                    loader.ReportProgress((i+1) * sensorPercens);
                }
            }
        }

        private void SlabVisualizationForm_Load(object sender, EventArgs e)
        {
            MakeGraphPane();

            for (var i = 0; i < this.Controls.Count; ++i) {                
                Controls[i].Hide();
            }
            
            if (client != null && client.IsConnected) {
                ShowSensorPlots();
                //ShowSlabModel();
            }
            
            progressShower = new ProgressShower();
            progressShower.Parent = this;
            progressShower.Dock = DockStyle.Fill;            
            progressShower.Show();
        }

        private void MakeGraphPane()
        {
            plotsView.IsShowPointValues = true;            
            plotsView.GraphPane = new GraphPane(
                new RectangleF(plotsView.Location.X,
                               plotsView.Location.Y,
                               plotsView.Size.Width,
                               plotsView.Size.Height),
                "Показания датчиков при сканировании слитка", "Время (секунды)", "Показание датчика (мм)");

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

        private void ShowSensorPlots()
        {
            var sensors = client.GetSensorInfos();
            points = new PointF[sensors.Length][];
            for (var i = 0; i < sensors.Length; ++i) {
                var pane = plotsView.GraphPane;
                pane.AddCurve(
                    sensors[i].Name,
                    new PointPairList(),
                    Color.FromArgb(rnd.Next(255), rnd.Next(255), rnd.Next(255)),
                    SymbolType.None);               
            }

            loader.RunWorkerAsync(sensors);                        
        }

        private void plotsView_ContextMenuBuilder(ZedGraphControl sender, ContextMenuStrip menuStrip, Point mousePt, ZedGraphControl.ContextMenuObjectState objState)
        {
            menuStrip.Items[0].Text = @"Копировать";
            menuStrip.Items[1].Text = @"Сохранить как картинку…";
            menuStrip.Items[2].Text = @"Параметры страницы…";
            menuStrip.Items[3].Text = @"Печать…";
            menuStrip.Items[4].Text = @"Показывать значения в точках…";
            menuStrip.Items[7].Text = @"Установить масштаб по умолчанию…";
            menuStrip.Items.RemoveAt(5);
            menuStrip.Items.RemoveAt(5);
        }        
    }
}
