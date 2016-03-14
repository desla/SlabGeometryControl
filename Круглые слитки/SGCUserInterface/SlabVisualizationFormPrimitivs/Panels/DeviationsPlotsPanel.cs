using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Alvasoft.SlabGeometryControl;
using ZedGraph;

namespace SGCUserInterface.SlabVisualizationFormPrimitivs.Panels
{
    public class DeviationsPlotsPanel
    {
        private ZedGraphControl control;
        private SlabModel3D slabModel;
        private List<LineItem> deviationsPlots = new List<LineItem>();
        private List<LineItem> convexPlots = new List<LineItem>(); 
        private int currentCurveIndex = 0;

        public DeviationsPlotsPanel(ZedGraphControl aGraphControl)
        {
            if (aGraphControl == null) {
                throw new ArgumentNullException("aGraphControl");
            }

            control = aGraphControl;
            InitDeviationsPlotsPanel();
        }

        public void SetSlabModel(SlabModel3D aSlabModel)
        {
            slabModel = aSlabModel;
        }

        public void Initialize()
        {
            if (slabModel == null) {
                return;
            }

            BuildLeftViewPlots();
            BuildTopViewPlots();
        }

        public void ShowLeftPlot()
        {
            if (deviationsPlots.Count == 0) {
                return;
            }

            deviationsPlots[currentCurveIndex].IsVisible = false;
            deviationsPlots[currentCurveIndex].Label.IsVisible = false;
            convexPlots[currentCurveIndex].IsVisible = false;
            convexPlots[currentCurveIndex].Label.IsVisible = false;
            if (currentCurveIndex == 0) {
                currentCurveIndex = deviationsPlots.Count - 1;
            }
            else {
                currentCurveIndex--;
            }

            deviationsPlots[currentCurveIndex].IsVisible = true;
            deviationsPlots[currentCurveIndex].Label.IsVisible = true;
            convexPlots[currentCurveIndex].IsVisible = true;
            convexPlots[currentCurveIndex].Label.IsVisible = true;

            control.AxisChange();
            control.Invalidate();
        }

        public void ShowRightPlot()
        {
            if (deviationsPlots.Count == 0) {
                return;
            }

            deviationsPlots[currentCurveIndex].IsVisible = false;
            deviationsPlots[currentCurveIndex].Label.IsVisible = false;
            convexPlots[currentCurveIndex].IsVisible = false;
            convexPlots[currentCurveIndex].Label.IsVisible = false;
            if (currentCurveIndex == deviationsPlots.Count - 1) {
                currentCurveIndex = 0;
            }
            else {
                currentCurveIndex++;
            }

            deviationsPlots[currentCurveIndex].IsVisible = true;
            deviationsPlots[currentCurveIndex].Label.IsVisible = true;
            convexPlots[currentCurveIndex].IsVisible = true;
            convexPlots[currentCurveIndex].Label.IsVisible = true;

            control.AxisChange();
            control.Invalidate();
        }

        public void ShowCurrentPlot()
        {
            deviationsPlots[currentCurveIndex].IsVisible = true;
            deviationsPlots[currentCurveIndex].Label.IsVisible = true;
            convexPlots[currentCurveIndex].IsVisible = true;
            convexPlots[currentCurveIndex].Label.IsVisible = true;
            control.AxisChange();
            control.Invalidate();
        }

        public void ChangeYAsix(int aValue)
        {
            var pane = control.GraphPane;
            if (pane.YAxis.Scale.Min + aValue >= pane.YAxis.Scale.Max - aValue) {
                return;
            }
            pane.YAxis.Scale.Min += aValue;
            pane.YAxis.Scale.Max -= aValue;
            control.AxisChange();
            control.Invalidate();
        }

        private void BuildLeftViewPlots()
        {
            if (slabModel.CenterLine == null) {
                return;
            }

            var pane = control.GraphPane;
            var centersLine = slabModel.CenterLine;
            var diameters = slabModel.Diameters;
            var xPoints = new double[centersLine.Length];
            var yPoints = new double[centersLine.Length];
            for (var i = 0; i < centersLine.Length; ++i) {
                xPoints[i] = centersLine[i].Z;
                yPoints[i] = centersLine[i].Y + diameters[i] / 2.0;
            }
            var average = -yPoints.Average();
            for (var i = 0; i < yPoints.Length; ++i) {
                yPoints[i] += average;
            }
            var curve = pane.AddCurve("Снизу", xPoints, yPoints, Color.Blue, SymbolType.None);
            curve.Line.IsAntiAlias = true;
            curve.IsVisible = false;
            curve.Label.IsVisible = false;
            deviationsPlots.Add(curve);
            AddConvexHullPlot(xPoints, yPoints, -1);

            xPoints = new double[centersLine.Length];
            yPoints = new double[centersLine.Length];
            for (var i = 0; i < centersLine.Length; ++i) {
                xPoints[i] = centersLine[i].Z;
                yPoints[i] = centersLine[i].Y - diameters[i] / 2.0;
            }
            average = -yPoints.Average();
            for (var i = 0; i < yPoints.Length; ++i) {
                yPoints[i] += average;
            }
            curve = pane.AddCurve("Сверху", xPoints, yPoints, Color.Blue, SymbolType.None);
            curve.Line.IsAntiAlias = true;
            curve.IsVisible = false;
            curve.Label.IsVisible = false;
            deviationsPlots.Add(curve);
            AddConvexHullPlot(xPoints, yPoints, 1);
        }

        private void AddConvexHullPlot(double[] aXPoints, double[] aYPoints, int aValue)
        {
            var saddlePoints = ConvexHull.Build(aXPoints, aYPoints, aValue);
            var xPoints = new double[saddlePoints.Length];
            var yPoints = new double[saddlePoints.Length];
            for (var i = 0; i < saddlePoints.Length; ++i) {
                xPoints[i] = aXPoints[saddlePoints[i]];
                yPoints[i] = aYPoints[saddlePoints[i]];
            }

            var pane = control.GraphPane;
            var curve = pane.AddCurve("Леска", xPoints, yPoints, Color.DarkGreen, SymbolType.None);
            curve.Line.IsAntiAlias = true;
            curve.IsVisible = false;
            curve.Label.IsVisible = false;
            convexPlots.Add(curve);
        }        

        private void BuildTopViewPlots()
        {
            if (slabModel.CenterLine == null) {
                return;
            }

            var pane = control.GraphPane;
            var centersLine = slabModel.CenterLine;
            var diameters = slabModel.Diameters;
            var xPoints = new double[centersLine.Length];
            var yPoints = new double[centersLine.Length];
            for (var i = 0; i < centersLine.Length; ++i) {
                xPoints[i] = centersLine[i].Z;
                yPoints[i] = centersLine[i].X + diameters[i] / 2.0;
            }
            var average = -yPoints.Average();
            for (var i = 0; i < yPoints.Length; ++i) {
                yPoints[i] += average;
            }
            var curve = pane.AddCurve("Слева", xPoints, yPoints, Color.Blue, SymbolType.None);
            curve.Line.IsAntiAlias = true;
            curve.IsVisible = false;
            curve.Label.IsVisible = false;
            deviationsPlots.Add(curve);
            AddConvexHullPlot(xPoints, yPoints, -1);

            xPoints = new double[centersLine.Length];
            yPoints = new double[centersLine.Length];
            for (var i = 0; i < centersLine.Length; ++i) {
                xPoints[i] = centersLine[i].Z;
                yPoints[i] = centersLine[i].X - diameters[i] / 2.0;
            }
            average = -yPoints.Average();
            for (var i = 0; i < yPoints.Length; ++i) {
                yPoints[i] += average;
            }
            curve = pane.AddCurve("Справа", xPoints, yPoints, Color.Blue, SymbolType.None);
            curve.Line.IsAntiAlias = true;
            curve.IsVisible = false;
            curve.Label.IsVisible = false;
            deviationsPlots.Add(curve);
            AddConvexHullPlot(xPoints, yPoints, 1);
        }

        private void InitDeviationsPlotsPanel()
        {
            control.IsShowPointValues = true;
            control.GraphPane = new GraphPane(
                new RectangleF(control.Location.X,
                               control.Location.Y,
                               control.Size.Width,
                               control.Size.Height),
                "График отклонений поверхностей слитка от среднего",
                "Расстояние от начала слитка (мм)",
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
        }        
    }
}
