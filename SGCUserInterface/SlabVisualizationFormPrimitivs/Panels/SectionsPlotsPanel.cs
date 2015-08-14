using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using Alvasoft.SlabGeometryControl;
using ZedGraph;

namespace SGCUserInterface.SlabVisualizationFormPrimitivs.Panels
{
    public class SectionsPlotsPanel
    {
        private ZedGraphControl control;
        private SlabModel3D slabModel;

        private List<LineItem> leftPlots = new List<LineItem>();
        private List<LineItem> topPlots = new List<LineItem>(); 

        public SectionsPlotsPanel(ZedGraphControl aGraphControl)
        {
            if (aGraphControl == null) {
                throw new ArgumentNullException("aGraphControl");
            }

            control = aGraphControl;
            InitSectionsPanel();
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
            
            BuildLeftSectionPlots();
            BuildTopSectionPlots();
        }

        public void ShowLeftSection()
        {
            foreach (var plot in topPlots) {
                plot.IsVisible = false;
                plot.Label.IsVisible = false;
            }

            foreach (var plot in leftPlots) {
                plot.IsVisible = true;
                plot.Label.IsVisible = true;
            }

            control.AxisChange();
            control.Invalidate();
        }

        public void ShowTopSection()
        {
            foreach (var plot in topPlots) {
                plot.IsVisible = true;
                plot.Label.IsVisible = true;
            }

            foreach (var plot in leftPlots) {
                plot.IsVisible = false;
                plot.Label.IsVisible = false;
            }

            control.AxisChange();
            control.Invalidate();
        }

        private void InitSectionsPanel()
        {
            control.IsShowPointValues = true;
            control.GraphPane = new GraphPane(
                new RectangleF(control.Location.X,
                               control.Location.Y,
                               control.Size.Width,
                               control.Size.Height),
                "Срезы слитка", "Расстояние от начала слитка (мм)", "Показание датчика (мм)");
        }

        private void BuildLeftSectionPlots()
        {
            if (slabModel == null || 
                slabModel.TopLines == null || 
                slabModel.BottomLines == null) {
                return;
            }
            var topPoints = GetLeftSectionPoints(slabModel.TopLines[slabModel.TopLines.Length / 2]);
            var bottomPoints = GetLeftSectionPoints(slabModel.BottomLines[slabModel.BottomLines.Length / 2]);
            var averageBottom = -bottomPoints.Y.Average();            
            for (var i = 0; i < topPoints.Y.Length; ++i) {
                topPoints.Y[i] += averageBottom;
            }
            var averageTop = topPoints.Y.Average();
            for (var i = 0; i < bottomPoints.Y.Length; ++i) {
                bottomPoints.Y[i] += averageBottom;
            }            

            var pane = control.GraphPane;
            var topCurve = pane.AddCurve("Поверхность сверху", 
                topPoints.X, 
                topPoints.Y, 
                Color.Blue, SymbolType.None);
            var topZero = pane.AddCurve("", 
                new[] {0, topPoints.X.Last()},
                new[] { averageTop, averageTop },
                Color.Green, SymbolType.None);
            topZero.Line.Style = DashStyle.Dash;
            topZero.Line.IsAntiAlias = true;            
            var bottomCurve = pane.AddCurve("Поверхность снизу", 
                bottomPoints.X, 
                bottomPoints.Y, 
                Color.Brown, SymbolType.None);
            topCurve.Line.IsAntiAlias = true;
            bottomCurve.Line.IsAntiAlias = true;
            leftPlots.Add(topCurve);
            leftPlots.Add(bottomCurve);
            leftPlots.Add(topZero);            
        }        

        private void BuildTopSectionPlots()
        {
            if (slabModel == null ||
                slabModel.LeftLines == null ||
                slabModel.RightLines == null) {
                return;
            }
            var leftPoints = GetTopSectionPoints(slabModel.LeftLines[slabModel.LeftLines.Length / 2]);
            var rightPoints = GetTopSectionPoints(slabModel.RightLines[slabModel.RightLines.Length / 2]);
            var averageLeft = -leftPoints.Y.Average();
            for (var i = 0; i < leftPoints.Y.Length; ++i) {
                leftPoints.Y[i] += averageLeft;
            }            
            for (var i = 0; i < rightPoints.Y.Length; ++i) {
                rightPoints.Y[i] += averageLeft;
            }
            var averageRight = rightPoints.Y.Average();            

            var pane = control.GraphPane;
            var rightZero = pane.AddCurve("",
                new[] { 0, rightPoints.X.Last() },
                new[] { averageRight, averageRight },
                Color.Green, SymbolType.None);
            rightZero.Line.Style = DashStyle.Dash;
            rightZero.Line.IsAntiAlias = true;            
            var leftCurve = pane.AddCurve("Поверхность слева", leftPoints.X, leftPoints.Y, Color.Blue, SymbolType.None);
            var rightCurve = pane.AddCurve("Поверхность справа", rightPoints.X, rightPoints.Y, Color.Brown, SymbolType.None);
            leftCurve.Line.IsAntiAlias = true;
            rightCurve.Line.IsAntiAlias = true;
            topPlots.Add(leftCurve);
            topPlots.Add(rightCurve);
            topPlots.Add(rightZero);            
        }

        private TuplePoints GetTopSectionPoints(SlabPoint[] aLine)
        {
            var xPoints = new List<double>();
            var yPoints = new List<double>();
            if (aLine != null) {
                for (var i = 0; i < aLine.Length; ++i) {
                    xPoints.Add(aLine[i].Z);
                    yPoints.Add(aLine[i].X);
                }
            }

            return new TuplePoints {
                X = xPoints.ToArray(),
                Y = yPoints.ToArray()
            };
        }

        private TuplePoints GetLeftSectionPoints(SlabPoint[] aLine)
        {
            var xPoints = new List<double>();
            var yPoints = new List<double>();
            if (aLine != null) {
                for (var i = 0; i < aLine.Length; ++i) {
                    xPoints.Add(aLine[i].Z);
                    yPoints.Add(aLine[i].Y);
                }
            }

            return new TuplePoints {
                X = xPoints.ToArray(),
                Y = yPoints.ToArray()
            };
        }        
    }
}
