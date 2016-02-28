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
            
        }        

        private void BuildTopSectionPlots()
        {
               
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
