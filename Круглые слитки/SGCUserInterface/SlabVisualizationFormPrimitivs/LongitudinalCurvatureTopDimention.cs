using System;
using System.Linq;
using Alvasoft.SlabGeometryControl;
using Tao.FreeGlut;
using Tao.OpenGl;

namespace SGCUserInterface.SlabVisualizationFormPrimitivs
{
    public class LongitudinalCurvatureTopDimention : DimentionGraphicPrimitiveBase
    {
        public override void DrawDimention(bool aIsSmoothEnable)
        {
            if (!IsVisible) {
                return;
            }

            DrawCallout();

            DrawSphere(aIsSmoothEnable);
        }

        public override string GetDimentionName()
        {
            return "top_side_longitudinal_curvature";
        }

        private void DrawSphere(bool aIsSmoothEnable)
        {
            
        }

        private void DrawCallout()
        {
            
        }
    }
}
