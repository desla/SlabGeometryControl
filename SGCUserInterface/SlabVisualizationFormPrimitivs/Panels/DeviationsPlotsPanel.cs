﻿using System;
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
        private List<LineItem> plots = new List<LineItem>();
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
            if (plots.Count == 0) {
                return;
            }

            plots[currentCurveIndex].IsVisible = false;
            plots[currentCurveIndex].Label.IsVisible = false;
            if (currentCurveIndex == 0) {
                currentCurveIndex = plots.Count - 1;
            }
            else {
                currentCurveIndex--;
            }

            plots[currentCurveIndex].IsVisible = true;
            plots[currentCurveIndex].Label.IsVisible = true;

            control.AxisChange();
            control.Invalidate();
        }

        public void ShowRightPlot()
        {
            if (plots.Count == 0) {
                return;
            }

            plots[currentCurveIndex].IsVisible = false;
            plots[currentCurveIndex].Label.IsVisible = false;
            if (currentCurveIndex == plots.Count - 1) {
                currentCurveIndex = 0;
            }
            else {
                currentCurveIndex++;
            }

            plots[currentCurveIndex].IsVisible = true;
            plots[currentCurveIndex].Label.IsVisible = true;

            control.AxisChange();
            control.Invalidate();
        }

        public void ShowCurrentPlot()
        {
            plots[currentCurveIndex].IsVisible = true;
            plots[currentCurveIndex].Label.IsVisible = true;
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
            if (slabModel.TopLines == null) {
                return;
            }

            var pane = control.GraphPane;            
            var topLine = slabModel.TopLines[slabModel.TopLines.Length / 2];
            var xPoints = new double[topLine.Length];
            var yPoints = new double[topLine.Length];
            for (var i = 0; i < topLine.Length; ++i) {
                xPoints[i] = topLine[i].Z;
                yPoints[i] = topLine[i].Y;
            }
            var average = -yPoints.Average();
            for (var i = 0; i < yPoints.Length; ++i) {
                yPoints[i] += average;
            }

            var curve = pane.AddCurve("Сверху", xPoints, yPoints, Color.Blue, SymbolType.None);
            curve.Line.IsAntiAlias = true;
            curve.IsVisible = false;
            curve.Label.IsVisible = false;
            plots.Add(curve);

            var bottomLine = slabModel.BottomLines[slabModel.BottomLines.Length / 2];
            xPoints = new double[bottomLine.Length];
            yPoints = new double[bottomLine.Length];            
            for (var j = 0; j < bottomLine.Length; ++j) {
                xPoints[j] = bottomLine[j].Z;
                yPoints[j] = bottomLine[j].Y;
            }
            average = -yPoints.Average();
            for (var i = 0; i < yPoints.Length; ++i) {
                yPoints[i] += average;
            }
            curve = pane.AddCurve("Снизу", xPoints, yPoints, Color.Blue, SymbolType.None);
            curve.Line.IsAntiAlias = true;
            curve.IsVisible = false;
            curve.Label.IsVisible = false;
            plots.Add(curve);
        }

        private void BuildTopViewPlots()
        {
            if (slabModel.TopLines == null) {
                return;
            }

            var pane = control.GraphPane;
            var leftLine = slabModel.LeftLines[slabModel.LeftLines.Length / 2];
            var xPoints = new double[leftLine.Length];
            var yPoints = new double[leftLine.Length];
            for (var j = 0; j < leftLine.Length; ++j) {
                xPoints[j] = leftLine[j].Z;
                yPoints[j] = leftLine[j].X;
            }
            var average = -yPoints.Average();
            for (var i = 0; i < yPoints.Length; ++i) {
                yPoints[i] += average;
            }
            var curve = pane.AddCurve("Слева", xPoints, yPoints, Color.Blue, SymbolType.None);
            curve.Line.IsAntiAlias = true;
            curve.IsVisible = false;
            curve.Label.IsVisible = false;
            plots.Add(curve);

            var rightLine = slabModel.RightLines[slabModel.RightLines.Length / 2];
            xPoints = new double[rightLine.Length];
            yPoints = new double[rightLine.Length];
            for (var j = 0; j < rightLine.Length; ++j) {
                xPoints[j] = rightLine[j].Z;
                yPoints[j] = rightLine[j].X;
            }
            average = -yPoints.Average();
            for (var i = 0; i < yPoints.Length; ++i) {
                yPoints[i] += average;
            }
            curve = pane.AddCurve("Справа", xPoints, yPoints, Color.Blue, SymbolType.None);
            curve.Line.IsAntiAlias = true;
            curve.IsVisible = false;
            curve.Label.IsVisible = false;
            plots.Add(curve);
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