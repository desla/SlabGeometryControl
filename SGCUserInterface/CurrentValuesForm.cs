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
    public partial class CurrentValuesForm : Form
    {
        private SGCClientImpl client = null;
        private Timer updateTimer = new Timer();
        private Random rnd = new Random(14121989);
        private long currentTime = 0;

        public CurrentValuesForm(SGCClientImpl aClient)
        {
            InitializeComponent();

            client = aClient;            
        }

        private void CurrentValuesForm_Load(object sender, EventArgs e)
        {            
            plotsView.GraphPane = new GraphPane(
                new RectangleF(plotsView.Location.X,
                               plotsView.Location.Y,
                               plotsView.Size.Width,
                               plotsView.Size.Height),
                "Текущие показания датчиков", "Шаг мониторинга", "Показание датчика (миллиметры)");

            if (client != null && client.IsConnected) {
                var sensors = client.GetSensorInfos();
                foreach (var sensor in sensors) {
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
            }            
            
            updateTimer.Tick += UpdateInformation;
            updateTimer.Interval = 100;
            updateTimer.Start();
        }

        private void UpdateInformation(object sender, EventArgs e)
        {
            UpdateSensorValues();           
            UpdatePlots();
        }

        private void UpdateSensorValues()
        {
            try {
                for (var i = 0; i < dataGridView1.RowCount; ++i) {
                    var row = dataGridView1.Rows[i];
                    var sensorId = Convert.ToInt32(row.Cells["Id"].Value);
                    row.Cells["Value"].Value = client.GetSensorValueBySensorId(sensorId).Value;
                }
            }
            catch (Exception ex) {
                MessageBox.Show(@"Ошибка обновления данных: " + ex.Message);
                updateTimer.Stop();                
            }
        }

        private void UpdatePlots()
        {
            var pane = plotsView.GraphPane;
            for (var i = 0; i < dataGridView1.RowCount; ++i) {
                var row = dataGridView1.Rows[i];                
                pane.CurveList[i].AddPoint(currentTime, Convert.ToDouble(row.Cells["Value"].Value));                
            }            

            currentTime++;

            if (currentTime%5 == 0) {
                plotsView.AxisChange();
                plotsView.Invalidate();
            }
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
