﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Alvasoft.SlabGeometryControl;
using Tao.FreeGlut;
using Tao.OpenGl;
using ZedGraph;

namespace SGCUserInterface
{
    public partial class SlabVisualizationForm : Form
    {
        private static bool isGlutInited = false;

        private SGCClientImpl client = null;
        private int slabId = -1;                        
        private BackgroundWorker loader = new BackgroundWorker();
        private ProgressShower progressShower = null;        

        // Параметры для отрисовки графиков.
        private SensorInfo[] sensors = null;
        private Random rnd = new Random(14121989);
        private PointF[][] points = null;        

        // Дальше идут параметры для отображения 3д модели слитка.
        private SlabModel3D slabModel = null;
        private int translateX = 0;
        private int translateY = 0;
        private int translateZ = -10000;
        private int angleX = 0;
        private int angleY = 0;
        private int angleZ = 0;        
        private int deltaX = 0;
        private int deltaY = 0;
        private int lastPositionX = 0;
        private int lastPositionY = 0;
        private int lastAngleX = 0;
        private int lastAngleY = 0;

        private Dictionary<string, int> objectsList = new Dictionary<string, int>();
        private const string KEY_SURFACE = "surface";
        private const string KEY_SENSOR_VALUES = "sensorValues";
        private const string KEY_SLAB_DIMENTIONS = "slabDimentions";

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
            InitializeGlObjects();

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

        private void InitializeGlObjects()
        {
            InitGridSurface();
            InitSensorValues();
            InitSlabDimentions();
        }

        private void LoadSensorsValues(object sender, DoWorkEventArgs e)
        {
            if (client == null || !client.IsConnected) {
                return;
            }

            sensors = client.GetSensorInfos();
            if (sensors != null) {
                var sensorPercens = 90 / sensors.Length;
                points = new PointF[sensors.Length][];
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
                slabModel = client.GetSlabModel3DBySlabId(slabId);
                loader.ReportProgress(100);
            }
        }

        private void SlabVisualizationForm_Load(object sender, EventArgs e)
        {
            InitPlotsPanel();
            InitModelPanel();

            for (var i = 0; i < this.Controls.Count; ++i) {                
                Controls[i].Hide();
            }                       
            
            progressShower = new ProgressShower();
            progressShower.Parent = this;
            progressShower.Dock = DockStyle.Fill;            
            progressShower.Show();

            loader.RunWorkerAsync(sensors);
        }

        private void InitModelPanel()
        {
            modelPanel.MouseWheel += modelPanel_MouseWheel;
            modelPanel.InitializeContexts();
            // инициализация Glut             
            if (!isGlutInited) {
                Glut.glutInit();
                Glut.glutInitDisplayMode(Glut.GLUT_RGB | Glut.GLUT_DOUBLE | Glut.GLUT_DEPTH);
                isGlutInited = true;
            }            

            // отчитка окна 
            Gl.glClearColor(255, 255, 255, 1);

            // установка порта вывода в соответствии с размерами элемента anT 
            Gl.glViewport(0, 0, modelPanel.Width, modelPanel.Height);

            // настройка проекции 
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();
            Glu.gluPerspective(45, (float) modelPanel.Width / modelPanel.Height, 10, 100000);

            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();

            // настройка параметров OpenGL для визуализации 
            Gl.glEnable(Gl.GL_DEPTH_TEST);
            Gl.glEnable(Gl.GL_COLOR_MATERIAL);

            Gl.glEnable(Gl.GL_MULTISAMPLE_ARB);
            Gl.glEnable(Gl.GL_LINE_SMOOTH);
            Gl.glEnable(Gl.GL_BLEND);
            Gl.glHint(Gl.GL_MULTISAMPLE_FILTER_HINT_NV, Gl.GL_NEAREST);
            Gl.glHint(Gl.GL_LINE_SMOOTH_HINT, Gl.GL_NEAREST);
            Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);
        }        

        private void InitPlotsPanel()
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

        private void ShowPlots()
        {
            DrawPlotsPanel();

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

        private void DrawPlotsPanel()
        {
            if (sensors == null) {
                return;
            }
            
            for (var i = 0; i < sensors.Length; ++i) {
                var pane = plotsView.GraphPane;
                pane.AddCurve(
                    sensors[i].Name,
                    new PointPairList(),
                    Color.FromArgb(rnd.Next(255), rnd.Next(255), rnd.Next(255)),
                    SymbolType.None);               
            }                                    
        }

        private void ShowModel()
        {
            Gl.glPushMatrix();

            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            Gl.glTranslated(translateX, translateY, translateZ);
            Gl.glRotated(angleX, 1, 0, 0);
            Gl.glRotated(angleY, 0, 1, 0);
            Gl.glRotated(angleZ, 0, 0, 1);
            {
                if (gridSurfaceCheckBox.Checked && objectsList.ContainsKey(KEY_SURFACE)) {
                    Gl.glCallList(objectsList[KEY_SURFACE]);
                }
                
                if (sensorValuesCheckBox.Checked && objectsList.ContainsKey(KEY_SENSOR_VALUES)) {
                    Gl.glCallList(objectsList[KEY_SENSOR_VALUES]);
                }

                if (dimensionsCheckBox.Checked && objectsList.ContainsKey(KEY_SLAB_DIMENTIONS)) {
                    Gl.glCallList(objectsList[KEY_SLAB_DIMENTIONS]);
                }
            }
            Gl.glPopMatrix();
            Gl.glFlush();
            modelPanel.Invalidate();
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

        private void modelPanel_MouseDown(object sender, MouseEventArgs e)
        {
            deltaX = e.X;
            deltaY = e.Y;
            lastPositionX = translateX;
            lastPositionY = translateY;
            lastAngleX = angleX;
            lastAngleY = angleY;
        }

        private void modelPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right) {
                translateX = lastPositionX + (e.X - deltaX) / 1;
                translateY = lastPositionY - (e.Y - deltaY) / 1;
                ShowModel();
            }
            else if (e.Button == MouseButtons.Left) {
                angleX = lastAngleX + (e.Y - deltaY) / 10;
                angleY = lastAngleY + (e.X - deltaX) / 10;
                ShowModel();
            }            
        }

        private void modelPanel_MouseWheel(object sender, MouseEventArgs e)
        {
            translateZ += e.Delta;
            ShowModel();
        }

        private void gridSurfaceCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ShowModel();
        }

        private void dimensionsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ShowModel();
        }

        private void sensorValuesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ShowModel();
        }

        // Вспомогательные функции для отрисовки 3д модели.

        private void InitGridSurface()
        {
            var strongLinesCount = 15;
            var slimLinesCount = 2;
            var distanceBetwenStrongLines = 500;
            var depthY = -2000;

            var surfaceNumber = Gl.glGenLists(1);
            objectsList[KEY_SURFACE] = surfaceNumber;
            Gl.glNewList(surfaceNumber, Gl.GL_COMPILE);
            var color = Color.Gray;
            Gl.glLineWidth(1f);                        
            Gl.glColor3d(
                Convert.ToDouble(color.R) / 255,
                Convert.ToDouble(color.G) / 255,
                Convert.ToDouble(color.B) / 255);
            Gl.glBegin(Gl.GL_LINES);
            {
                var startPosition = distanceBetwenStrongLines * strongLinesCount / 2;
                for (var i = 0; i <= strongLinesCount; ++i) {
                    Gl.glVertex3f(-startPosition, depthY, -startPosition + distanceBetwenStrongLines * i);
                    Gl.glVertex3f(startPosition, depthY, -startPosition + distanceBetwenStrongLines * i);
                    Gl.glVertex3f(startPosition - distanceBetwenStrongLines * i, depthY, -startPosition);
                    Gl.glVertex3f(startPosition - distanceBetwenStrongLines * i, depthY, startPosition);
                }
            }
            Gl.glEnd();
            color = Color.LightGray;
            Gl.glColor3d(
                Convert.ToDouble(color.R) / 255,
                Convert.ToDouble(color.G) / 255,
                Convert.ToDouble(color.B) / 255);
            Gl.glBegin(Gl.GL_LINES);
            {
                var startPosition = distanceBetwenStrongLines * strongLinesCount / 2;
                var distance = distanceBetwenStrongLines / (slimLinesCount + 1);
                for (var i = 0; i < strongLinesCount; ++i) {
                    for (var j = 1; j <= slimLinesCount; ++j) {
                        Gl.glVertex3f(-startPosition, depthY, -startPosition + distanceBetwenStrongLines * i + distance * j);
                        Gl.glVertex3f(startPosition, depthY, -startPosition + distanceBetwenStrongLines * i + distance * j);
                        Gl.glVertex3f(-startPosition + distanceBetwenStrongLines * i + distance * j, depthY, -startPosition);
                        Gl.glVertex3f(-startPosition + distanceBetwenStrongLines * i + distance * j, depthY, startPosition);
                    }
                }
            }
            Gl.glEnd();
            Gl.glEndList();
        }

        private void InitSlabDimentions()
        {
            if (slabModel == null) {
                return;
            }

            var p1 = slabModel.TopLines[slabModel.TopLines.Length / 2].First();
            var p2 = slabModel.RightLines[slabModel.RightLines.Length / 2].First();
            var p3 = slabModel.BottomLines[slabModel.BottomLines.Length / 2].First();
            var p4 = slabModel.LeftLines[slabModel.LeftLines.Length / 2].First();
            var p5 = slabModel.TopLines[slabModel.TopLines.Length / 2].Last();
            var p6 = slabModel.RightLines[slabModel.RightLines.Length / 2].Last();
            var p7 = slabModel.BottomLines[slabModel.BottomLines.Length / 2].Last();
            var p8 = slabModel.LeftLines[slabModel.LeftLines.Length / 2].Last();
            var moveTo = p5.Z/2;

            var slabDimentionsNumber = Gl.glGenLists(1);
            objectsList[KEY_SLAB_DIMENTIONS] = slabDimentionsNumber;
            Gl.glNewList(slabDimentionsNumber, Gl.GL_COMPILE);
            var color = Color.Blue;
            Gl.glColor3d(
                Convert.ToDouble(color.R) / 255,
                Convert.ToDouble(color.G) / 255,
                Convert.ToDouble(color.B) / 255);
            Gl.glLineWidth(2f);
            Gl.glEnable(Gl.GL_LINE_STIPPLE);
            Gl.glLineStipple(1, 0x00FF);
            Gl.glBegin(Gl.GL_LINE_STRIP);
            {                
                Gl.glVertex3d(p4.X, p1.Y, p1.Z - moveTo);
                Gl.glVertex3d(p2.X, p1.Y, p1.Z - moveTo);
                Gl.glVertex3d(p2.X, p3.Y, p1.Z - moveTo);
                Gl.glVertex3d(p4.X, p3.Y, p1.Z - moveTo);
                Gl.glVertex3d(p4.X, p1.Y, p1.Z - moveTo);
                Gl.glVertex3d(p8.X, p5.Y, p5.Z - moveTo);
                Gl.glVertex3d(p6.X, p5.Y, p5.Z - moveTo);
                Gl.glVertex3d(p6.X, p7.Y, p5.Z - moveTo);
                Gl.glVertex3d(p8.X, p7.Y, p5.Z - moveTo);
                Gl.glVertex3d(p8.X, p5.Y, p5.Z - moveTo);
            }
            Gl.glEnd();
            Gl.glBegin(Gl.GL_LINES);
            {
                Gl.glVertex3d(p2.X, p1.Y, p1.Z - moveTo);
                Gl.glVertex3d(p6.X, p5.Y, p5.Z - moveTo);
                Gl.glVertex3d(p2.X, p3.Y, p1.Z - moveTo);
                Gl.glVertex3d(p6.X, p7.Y, p5.Z - moveTo);
                Gl.glVertex3d(p4.X, p3.Y, p1.Z - moveTo);
                Gl.glVertex3d(p8.X, p7.Y, p5.Z - moveTo);
            }
            Gl.glEnd();
            Gl.glDisable(Gl.GL_LINE_STIPPLE);
            Gl.glEndList();
        }

        private void InitSensorValues()
        {
            if (slabModel == null) {
                return;
            }

            var moveTo = slabModel.TopLines[slabModel.TopLines.Length/2].Last().Z / 2;

            var sensorValuesNumber = Gl.glGenLists(1);
            objectsList[KEY_SENSOR_VALUES] = sensorValuesNumber;
            Gl.glNewList(sensorValuesNumber, Gl.GL_COMPILE);
            var color = Color.Brown;
            Gl.glColor3d(
                Convert.ToDouble(color.R) / 255,
                Convert.ToDouble(color.G) / 255,
                Convert.ToDouble(color.B) / 255);
            Gl.glLineWidth(2f);
            Gl.glBegin(Gl.GL_LINE_STRIP);
            {
                for (var i = 0; i < slabModel.TopLines.Length; ++i) {
                    for (var j = 0; j < slabModel.TopLines[i].Length; ++j) {
                        var point = slabModel.TopLines[i][j];
                        Gl.glVertex3d(point.X, point.Y, point.Z - moveTo);
                    }
                }
            }
            Gl.glEnd();
            Gl.glBegin(Gl.GL_LINE_STRIP);
            {
                for (var i = 0; i < slabModel.BottomLines.Length; ++i) {
                    for (var j = 0; j < slabModel.BottomLines[i].Length; ++j) {
                        var point = slabModel.BottomLines[i][j];
                        Gl.glVertex3d(point.X, point.Y, point.Z - moveTo);
                    }
                }
            }
            Gl.glEnd();
            Gl.glBegin(Gl.GL_LINE_STRIP);
            {
                for (var i = 0; i < slabModel.LeftLines.Length; ++i) {
                    for (var j = 0; j < slabModel.LeftLines[i].Length; ++j) {
                        var point = slabModel.LeftLines[i][j];
                        Gl.glVertex3d(point.X, point.Y, point.Z - moveTo);
                    }
                }
            }
            Gl.glEnd();
            Gl.glBegin(Gl.GL_LINE_STRIP);
            {
                for (var i = 0; i < slabModel.RightLines.Length; ++i) {
                    for (var j = 0; j < slabModel.RightLines[i].Length; ++j) {
                        var point = slabModel.RightLines[i][j];
                        Gl.glVertex3d(point.X, point.Y, point.Z - moveTo);
                    }
                }
            }
            Gl.glEnd();
            Gl.glEndList();
        }

        private void smoothCheckedBox_CheckedChanged(object sender, EventArgs e)
        {
            if (smoothCheckedBox.Checked) {
                Gl.glEnable(Gl.GL_MULTISAMPLE_ARB);                
                Gl.glEnable(Gl.GL_LINE_SMOOTH);                                                
                Gl.glEnable(Gl.GL_BLEND);                                
            }
            else {
                Gl.glDisable(Gl.GL_MULTISAMPLE_ARB);
                Gl.glDisable(Gl.GL_LINE_SMOOTH);
                Gl.glDisable(Gl.GL_BLEND);
            }

            ShowModel();
        }

        private void SlabVisualizationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (objectsList.ContainsKey(KEY_SURFACE)) {
                Gl.glDeleteLists(objectsList[KEY_SURFACE], 1);
            }            
            if (objectsList.ContainsKey(KEY_SENSOR_VALUES)) {
                Gl.glDeleteLists(objectsList[KEY_SENSOR_VALUES], 1);
            }
            if (objectsList.ContainsKey(KEY_SLAB_DIMENTIONS)) {
                Gl.glDeleteLists(objectsList[KEY_SLAB_DIMENTIONS], 1);
            }
        }
    }
}
