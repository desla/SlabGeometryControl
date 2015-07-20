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

        private SlabPoint[] slabPoints = null;
        private double zoom;
        private double digreeX = 0;
        private double digreeY = 0;
        private double digreeZ = 0;
        private PointF center;
        private PointF mouceDown;

        public SlabVisualizationForm(int aSlabId, SGCClientImpl aClient)
        {
            InitializeComponent();
            tabControl1.MouseWheel += pictureBox1_MouseWheel;
            button1.Text = "\u2190";
            button3.Text = "\u2192";
            button4.Text = "\u2193";
            button2.Text = "\u2191";

            zoom = pictureBox1.Width / 1.5;            

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
            ShowPlots();
            ShowModel();

            if (progressShower != null) {
                progressShower.Dispose();
                progressShower = null;
            }

            for (var i = 0; i < this.Controls.Count; ++i) {                
                Controls[i].Show();
            }
        }

        private void ShowModel()
        {
            if (slabPoints == null) {
                return;
            }
                        
            var bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);                
            var camera = new Math3D.Camera {
                Position = new Math3D.Point3D(0, 0, 2000)
            };
            var pointsF = new PointF[slabPoints.Length];
            var points = new Math3D.Point3D[slabPoints.Length];
            var neightbours = new bool[slabPoints.Length];
            for (var i = 0; i < pointsF.Length; ++i) {
                points[i] = new Math3D.Point3D {
                    X = slabPoints[i].X, Y = slabPoints[i].Y, Z = slabPoints[i].Z
                };
                points[i] = Math3D.RotateX(points[i], digreeX);
                points[i] = Math3D.RotateY(points[i], digreeY);
                points[i] = Math3D.RotateZ(points[i], digreeZ);
                pointsF[i] = Math3D.Convert3DTo2D(points[i], camera, zoom, center);
                if (i > 0) {
                    if (GetDistance(points[i], points[i - 1]) < 100) {
                        neightbours[i] = true;
                    }
                }
            }
            var g = Graphics.FromImage(bitmap);
            DrawSystem(g, camera, center);
            for (var i = 1; i < pointsF.Length; ++i) {
                if (neightbours[i]) {
                    g.DrawLine(
                        new Pen(Color.Red), 
                        pointsF[i].X, 
                        pointsF[i].Y, 
                        pointsF[i - 1].X, 
                        pointsF[i - 1].Y);
                }                
                //g.FillEllipse(
                //    new SolidBrush(Color.Red),
                //    pointsF[i].X,
                //    pointsF[i].Y,
                //    (float)1.1,
                //    (float)1.1);
            }
            g.Dispose();
            pictureBox1.Image = bitmap;        
        }

        private double GetDistance(Math3D.Point3D aA, Math3D.Point3D aB)
        {
            return Math.Sqrt(
                (aA.X - aB.X) * (aA.X - aB.X) +
                (aA.Y - aB.Y) * (aA.Y - aB.Y) +
                (aA.Z - aB.Z) * (aA.Z - aB.Z));
        }

        private void DrawSystem(Graphics aGraphic, Math3D.Camera aCamera, PointF aCenter)
        {
            var points3D = new Math3D.Point3D[] {
                new Math3D.Point3D(0, 0, 0),
                new Math3D.Point3D(200, 0, 0),
                new Math3D.Point3D(0, 200, 0),
                new Math3D.Point3D(0, 0, 200)
            };

            var points2D = new PointF[points3D.Length];
            for (var i = 0; i < points3D.Length; ++i) {
                points3D[i] = Math3D.RotateX(points3D[i], digreeX);
                points3D[i] = Math3D.RotateY(points3D[i], digreeY);
                points3D[i] = Math3D.RotateZ(points3D[i], digreeZ);

                points2D[i] = Math3D.Convert3DTo2D(points3D[i], aCamera, zoom, aCenter);
            }

            for (var i = 1; i <= 3; ++i) {
                aGraphic.DrawLine(new Pen(Color.Blue),
                    points2D[0].X, points2D[0].Y,
                    points2D[i].X, points2D[i].Y);
            }
        }

        private void ShowPlots()
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
        }

        private void LoadSensorsValues(object sender, DoWorkEventArgs e)
        {
            var sensors = e.Argument as SensorInfo[];
            if (sensors != null) {
                var sensorPercens = 90 / sensors.Length;
                for (var i = 0; i < sensors.Length; ++i) {
                    var sensorValues = client.GetSensorValuesBySlabId(slabId, sensors[i].Id);                    
                    if (sensorValues != null && sensorValues.Length > 0) {
                        points[i] = new PointF[sensorValues.Length];
                        var startTime = DateTime.FromBinary(sensorValues[0].Time);                        
                        for (var j = 0; j < sensorValues.Length; ++j) {
                            var time = DateTime.FromBinary(sensorValues[j].Time).Subtract(startTime);
                            points[i][j] = new PointF((float) time.TotalMilliseconds, (float) sensorValues[j].Value);                            
                        }                        
                    }
                    loader.ReportProgress((i+1) * sensorPercens);
                }
                slabPoints = client.GetSlabPointsBySlabId(slabId);
                loader.ReportProgress(100);
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
        
        private void pictureBox1_MouseWheel(object sender, MouseEventArgs e)
        {            
            if (tabControl1.SelectedTab == slabModelPage) {
                zoom += e.Delta;
                ShowModel();                
            }            
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            digreeX += 5;
            ShowModel();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            digreeX -= 5;
            ShowModel();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            digreeY += 5;
            ShowModel();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            digreeY -= 5;
            ShowModel();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {            
            mouceDown = new PointF(e.X, e.Y);
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {            
            center.X += e.X - mouceDown.X;
            center.Y += e.Y - mouceDown.Y;
            ShowModel();
        }

        private void pictureBox1_SizeChanged(object sender, EventArgs e)
        {
            center = new PointF((float)(this.pictureBox1.Width / 2.0),
                (float)(pictureBox1.Height / 2.0));
        }
    }
}
