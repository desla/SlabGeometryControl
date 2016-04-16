using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Alvasoft.SlabGeometryControl;
using SGCUserInterface.SlabVisualizationFormPrimitivs;
using SGCUserInterface.SlabVisualizationFormPrimitivs.Panels;
using Tao.FreeGlut;
using Tao.OpenGl;
using ZedGraph;

namespace SGCUserInterface
{
    public partial class SlabVisualizationForm : Form
    {
        private SGCClientImpl client = null;
        private int slabId = -1;
        private int standartSizeId = -1;
        private BackgroundWorker loader = new BackgroundWorker();
        private ProgressShower progressShower = null;        

        // Параметры для отрисовки графиков.
        private SensorInfo[] sensors = null;        
        private PointF[][] points = null;        

        // Дальше идут параметры для отображения 3д модели слитка.
        private SlabModel3D slabModel = null;                
        private Dimention[] systemDimentions;
        private DimentionResult[] slabDimentionsResults;
        private Regulation[] regulations;

        private bool isErrorLoading = false;

        /// <summary>
        /// Для отображения графиков показаний датиков.
        /// </summary>
        private SensorsPlotsPanel sensorsPlots;
        /// <summary>
        /// Для отображения 3-д модели слитка.
        /// </summary>
        private SlabModelPanel slabModelPanel;
        /// <summary>
        /// Для отображения срезов слитка.
        /// </summary>
        private SectionsPlotsPanel sectionsPanel;
        /// <summary>
        /// Для отображения графиков отклонения от среднего.
        /// </summary>
        private DeviationsPlotsPanel deviationsPanel;

        private bool isUseFilters;

        public SlabVisualizationForm(int aSlabId, int aStandartSizeId, SGCClientImpl aClient, bool aIsUseFilter)
        {
            InitializeComponent();                        

            client = aClient;
            slabId = aSlabId;
            standartSizeId = aStandartSizeId;
            isUseFilters = aIsUseFilter;

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
            try {
                if (!isErrorLoading) {                                    
                    // графики показания датчиков.
                    sensorsPlots.DrawPlots(points, sensors);
                    sensorsPlots.ShowAllPlots(isAllPlotShowCheckedBox.Checked);
                    // срезы слитка.                    
                    sectionsPanel.SetSlabModel(slabModel);
                    sectionsPanel.Initialize();
                    sectionsPanel.ShowLeftSection();
                    // графики отклонения от среднего.
                    deviationsPanel.SetSlabModel(slabModel);
                    deviationsPanel.Initialize();
                    deviationsPanel.ShowCurrentPlot();
                    // 3д модель слитка.
                    InitializeSlabModelPanel();
                    slabModelPanel.ShowModel();           
                }                               
            }
            catch (Exception ex) {
                MessageBox.Show(@"Ошибка при построении модели: " + ex.Message);                
                tabControl1.TabPages["slabModelPage"].Visible = false;
                tabControl1.TabPages["sectionsPage"].Visible = false;
            } 
            finally {
                if (progressShower != null) {
                    progressShower.Dispose();
                    progressShower = null;
                }

                for (var i = 0; i < this.Controls.Count; ++i) {
                    Controls[i].Show();
                }
            }            
        }        

        private void InitializeSlabModelPanel()
        {
            slabModelPanel.SetSlabModel(slabModel);            
            slabModelPanel.SetDimentions(systemDimentions);
            slabModelPanel.SetDimentionResults(slabDimentionsResults);
            slabModelPanel.SetRegulations(regulations);
                        
            slabModelPanel.AddDimention(new LengthDimention(), lengthCheckBox);
            slabModelPanel.AddDimention(new FrontDiameterDimention(), frontDiameterCheckBox);
            slabModelPanel.AddDimention(new BackDiameterDimention(), backDiameterCheckBox);

            slabModelPanel.InitializeGlObjects();
        }

        private void LoadSensorsValues(object sender, DoWorkEventArgs e)
        {
            if (client == null || !client.IsConnected) {
                isErrorLoading = true;
                return;
            }

            try {
                sensors = client.GetSensorInfos();
                if (sensors != null) {
                    var sensorPercens = 90/sensors.Length;
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
                        loader.ReportProgress((i + 1)*sensorPercens);
                    }
                    slabModel = client.GetSlabModel3DBySlabId(slabId, isUseFilters);
                    loader.ReportProgress(95);
                    systemDimentions = client.GetDimentions();
                    slabDimentionsResults = client.GetDimentionResultsBySlabId(slabId);
                    regulations = client.GetRegulations();                    
                    loader.ReportProgress(100);
                }
                isErrorLoading = false;
            }
            catch (Exception ex) {
                MessageBox.Show(@"Ошибка при загрузке данных слитка: " + ex.Message);
                isErrorLoading = true;
            }
        }

        private void SlabVisualizationForm_Load(object sender, EventArgs e)
        {
            sensorsPlots = new SensorsPlotsPanel(plotsView);
            slabModelPanel = new SlabModelPanel(slabId, standartSizeId, modelView);                        
            sectionsPanel = new SectionsPlotsPanel(sectionsView);           
            deviationsPanel = new DeviationsPlotsPanel(deviationView);
            deviationView.MouseWheel += deviationView_MouseWheel;

            for (var i = 0; i < this.Controls.Count; ++i) {                
                Controls[i].Hide();
            }                       
            
            progressShower = new ProgressShower();
            progressShower.Parent = this;
            progressShower.Dock = DockStyle.Fill;            
            progressShower.Show();

            loader.RunWorkerAsync(sensors);
        }        

        private void modelPanel_MouseDown(object sender, MouseEventArgs e)
        {
            slabModelPanel.MouseDown(e);
        }

        private void modelPanel_MouseMove(object sender, MouseEventArgs e)
        {
            slabModelPanel.MouseMove(e);
        }                              

        private void gridSurfaceCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            slabModelPanel.ShowGridSurfaceChanged(gridSurfaceCheckBox.Checked);
            slabModelPanel.ShowModel();
        }

        private void dimensionsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            slabModelPanel.ShowDimentionsChanged(dimensionsCheckBox.Checked);
            slabModelPanel.ShowModel();
        }

        private void centersValuesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            slabModelPanel.ShowCentersValuesChanged(centersValuesCheckBox.Checked);
            slabModelPanel.ShowModel();
        }               

        private void smoothCheckedBox_CheckedChanged(object sender, EventArgs e)
        {
            slabModelPanel.ShowSmoothChanged(smoothCheckedBox.Checked);
            slabModelPanel.ShowModel();
        }

        private void SlabVisualizationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            slabModelPanel.Dispose();
        }

        private void allDimentionsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            var value = allDimentionsCheckBox.Checked;

            lengthCheckBox.Checked = value;
            frontDiameterCheckBox.Checked = value;            
        }

        private void deviationView_MouseWheel(object sender, MouseEventArgs e)
        {
            deviationsPanel.ChangeYAsix(e.Delta / 10);
        }

        private void frontSideButton_Click(object sender, EventArgs e)
        {
            slabModelPanel.MoveToFrontSide();
            slabModelPanel.ShowModel();            
        }

        private void topSideButton_Click(object sender, EventArgs e)
        {
            slabModelPanel.MoveToTopSide();
            slabModelPanel.ShowModel();
        }

        private void leftSideButton_Click(object sender, EventArgs e)
        {
            slabModelPanel.MoveToLeftSide();
            slabModelPanel.ShowModel();
        }

        private void angleSideButton_Click(object sender, EventArgs e)
        {
            slabModelPanel.MoveToAngleSide();
            slabModelPanel.ShowModel();
        }               

        private void leftButton_Click(object sender, EventArgs e)
        {
            sensorsPlots.ShowLeftPlots();
        }

        private void rightButton_Click(object sender, EventArgs e)
        {
            sensorsPlots.ShowRightPlots();
        }

        private void isAllPlotShowCheckedBox_CheckedChanged(object sender, EventArgs e)
        {
            sensorsPlots.ShowAllPlots(isAllPlotShowCheckedBox.Checked);
        }

        private void isShowNodesCheckedBox_CheckedChanged(object sender, EventArgs e)
        {
            sensorsPlots.ShowPlotNodes(isShowNodesCheckedBox.Checked);
        }

        private void sectionsView_ContextMenuBuilder(ZedGraphControl sender, ContextMenuStrip menuStrip, Point mousePt, ZedGraphControl.ContextMenuObjectState objState)
        {
            menuStrip.Items[0].Text = @"Копировать";
            menuStrip.Items[1].Text = @"Сохранить как картинку…";
            menuStrip.Items[2].Text = @"Параметры страницы";
            menuStrip.Items[3].Text = @"Печать…";
            menuStrip.Items[4].Text = @"Показывать значения в точках";
            menuStrip.Items[7].Text = @"Установить масштаб по умолчанию";
            menuStrip.Items.RemoveAt(5);
            menuStrip.Items.RemoveAt(5);
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

        private void button7_Click(object sender, EventArgs e)
        {
            sectionsPanel.ShowLeftSection();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            sectionsPanel.ShowTopSection();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            deviationsPanel.ShowLeftPlot();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            deviationsPanel.ShowRightPlot();
        }

        private void deviationView_ContextMenuBuilder(ZedGraphControl sender, ContextMenuStrip menuStrip, Point mousePt, ZedGraphControl.ContextMenuObjectState objState)
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

        private void sensorsValuesCheckedBox_CheckedChanged(object sender, EventArgs e) {
            slabModelPanel.ShowSensorValuesChanged(sensorsValuesCheckedBox.Checked);
            slabModelPanel.ShowModel();
        }

        private void lengthCheckBox_CheckedChanged(object sender, EventArgs e) {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) {

        }

        private void frontDiameterCheckBox_CheckedChanged(object sender, EventArgs e) {

        }
    }
}
