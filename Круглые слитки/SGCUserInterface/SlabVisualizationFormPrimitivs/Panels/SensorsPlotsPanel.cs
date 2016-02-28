using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Alvasoft.SlabGeometryControl;
using ZedGraph;

namespace SGCUserInterface.SlabVisualizationFormPrimitivs.Panels
{
    /// <summary>
    /// Вспомогательный класс для того, чтобы вынесни код, 
    /// отвечающий за показ графиков показаний датчиков, 
    /// из кода формы.
    /// </summary>
    public class SensorsPlotsPanel
    {
        private ZedGraphControl control;
        private Random rnd = new Random(14121989);
        private List<LineItem> plotLines = new List<LineItem>();        
        private int plotIndex = 0;
        private bool isShowAllPlots;
        
        public SensorsPlotsPanel(ZedGraphControl aGraphControl)
        {
            if (aGraphControl == null) {
                throw new ArgumentNullException("aGraphControl");
            }

            control = aGraphControl;
            Initialize();
        }

        public void DrawPlots(PointF[][] aPoints, SensorInfo[] aSensors)
        {
            AddCurvesToPanel(aSensors);

            if (aPoints == null) {
                MessageBox.Show(@"Нет точек для отображения");
                return;
            }

            var pane = control.GraphPane;
            for (var i = 0; i < aPoints.Length; ++i) {
                if (aPoints[i] != null) {
                    for (var j = 0; j < aPoints[i].Length; ++j) {
                        pane.CurveList[i].AddPoint(aPoints[i][j].X / 1000, aPoints[i][j].Y);
                    }
                }

                HidePlotLine(i);
            }

            if (aPoints.Length > 0) {
                ShowPlotLine(0);
            }

            control.AxisChange();
            control.Invalidate();
        }

        public void ShowLeftPlots()
        {
            if (isShowAllPlots) {
                return;
            }

            HidePlotLine(plotIndex);
            plotIndex = (plotIndex == 0 ? plotLines.Count - 1 : plotIndex - 1);
            ShowPlotLine(plotIndex);

            control.AxisChange();
            control.Invalidate();
        }

        public void ShowRightPlots()
        {
            if (isShowAllPlots) {
                return;
            }

            HidePlotLine(plotIndex);
            plotIndex = (plotIndex == plotLines.Count - 1 ? 0 : plotIndex + 1);
            ShowPlotLine(plotIndex);

            control.AxisChange();
            control.Invalidate();
        }

        public void ShowAllPlots(bool aIsShowAllPlots)
        {
            isShowAllPlots = aIsShowAllPlots;
            if (aIsShowAllPlots) {                
                for (var i = 0; i < plotLines.Count; ++i) {
                    ShowPlotLine(i);
                }
            }
            else {                
                for (var i = 0; i < plotLines.Count; ++i) {
                    HidePlotLine(i);
                }

                ShowPlotLine(plotIndex);
            }

            control.AxisChange();
            control.Invalidate();
        }

        private void AddCurvesToPanel(SensorInfo[] aSensors)
        {
            if (aSensors == null) {
                return;
            }

            var pane = control.GraphPane;
            for (var i = 0; i < aSensors.Length; ++i) {
                var curve = pane.AddCurve(
                    aSensors[i].Name,
                    new PointPairList(),
                    Color.FromArgb(rnd.Next(255), rnd.Next(255), rnd.Next(255)),
                    SymbolType.None);
                curve.Line.IsSmooth = true;
                curve.Line.SmoothTension = 0;
                curve.Line.IsAntiAlias = true;
                plotLines.Add(curve);
            }
        }

        private void Initialize()
        {
            control.IsShowPointValues = true;
            control.GraphPane = new GraphPane(
                new RectangleF(control.Location.X,
                               control.Location.Y,
                               control.Size.Width,
                               control.Size.Height),
                "Показания датчиков при сканировании слитка",
                "Время (секунды)",
                "Показание датчика (мм)");

            var pane = control.GraphPane;
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

        private void ShowPlotLine(int aPlotIndex)
        {
            plotLines[aPlotIndex].IsVisible = true;
            plotLines[aPlotIndex].Label.IsVisible = true;            
        }

        private void HidePlotLine(int aPlotIndex)
        {
            plotLines[aPlotIndex].IsVisible = false;
            plotLines[aPlotIndex].Label.IsVisible = false;            
        }

        public void ShowPlotNodes(bool aIsShowNodes)
        {
            foreach (var plotLine in plotLines) {
                var color = plotLine.Color;
                if (aIsShowNodes) {
                    plotLine.Symbol = new Symbol(SymbolType.Triangle, color);
                }
                else {
                    plotLine.Symbol = new Symbol(SymbolType.None, color);
                }
            }

            control.AxisChange();
            control.Invalidate();
        }
    }
}
