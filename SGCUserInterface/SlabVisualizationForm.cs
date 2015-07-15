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

        public SlabVisualizationForm(int aSlabId, SGCClientImpl aClient)
        {
            InitializeComponent();

            client = aClient;
            slabId = aSlabId;
        }

        private void SlabVisualizationForm_Load(object sender, EventArgs e)
        {
            plotsView.GraphPane = new GraphPane(
                new RectangleF(plotsView.Location.X, 
                               plotsView.Location.Y, 
                               plotsView.Size.Width, 
                               plotsView.Size.Height), 
                "Показания датчиков при сканировании слитка", "Время (секунды)", "Показание датчика (миллиметры)");
            if (client != null && client.IsConnected) {
                ShowSensorPlots();
                //ShowSlabModel();
            }
        }

        private void ShowSensorPlots()
        {
            var sensors = client.GetSensorInfos();
            for (var i = 0; i < sensors.Length; ++i) {
                var pane = plotsView.GraphPane;
                pane.AddCurve(
                    sensors[i].Name,
                    new PointPairList(),
                    Color.FromArgb(rnd.Next(255), rnd.Next(255), rnd.Next(255)),
                    SymbolType.None);

                var sensorValues = client.GetSensorValuesBySlabId(slabId, sensors[i].Id);
                if (sensorValues != null) {
                    var startTime = DateTime.FromBinary(sensorValues[0].Time);
                    foreach (var sensorValue in sensorValues) {
                        var time = DateTime.FromBinary(sensorValue.Time).Subtract(startTime);                        
                        pane.CurveList[i].AddPoint(time.TotalMilliseconds / 1000, sensorValue.Value);
                    }
                }
            }

            plotsView.AxisChange();
            plotsView.Invalidate();
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
