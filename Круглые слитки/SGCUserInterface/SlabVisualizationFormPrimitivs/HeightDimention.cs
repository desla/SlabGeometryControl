using System;
using System.Linq;
using Alvasoft.SlabGeometryControl;
using Tao.FreeGlut;
using Tao.OpenGl;

namespace SGCUserInterface.SlabVisualizationFormPrimitivs
{
    public class HeightDimention : DimentionGraphicPrimitiveBase
    {
        public override void DrawDimention(bool aIsSmoothEnable)
        {
            if (!IsVisible) {
                return;
            }
          
        }

        public override string GetDimentionName()
        {
            return "height";
        }

        private void DrawSphere(bool aIsSmoothEnable)
        {
            
        }

        private void DrawCallout()
        {
           
        }        
    }
}
