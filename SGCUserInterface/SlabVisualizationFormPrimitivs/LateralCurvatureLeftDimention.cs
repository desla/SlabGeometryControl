using System;
using System.Linq;
using Alvasoft.SlabGeometryControl;
using Tao.FreeGlut;
using Tao.OpenGl;

namespace SGCUserInterface.SlabVisualizationFormPrimitivs
{
    public class LateralCurvatureLeftDimention : DimentionGraphicPrimitiveBase
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
            return "left_side_leteral_curvature";
        }

        private void DrawSphere(bool aIsSmoothEnable)
        {
            Gl.glPushMatrix();            
            var translateX = SlabModel.LeftLines[0].First().X - 50;
            var translateY = (SlabModel.BottomLines[0].First().Y + SlabModel.TopLines[0].First().Y) / 2;
            var translateZ = (SlabModel.RightLines[0].Last().Z + SlabModel.RightLines[0].First().Z) / 2;
            Gl.glTranslated(translateX, translateY, translateZ);
            Gl.glColor3d(Convert.ToDouble(Color.R) / 255,
                         Convert.ToDouble(Color.G) / 255,
                         Convert.ToDouble(Color.B) / 255);
            if (aIsSmoothEnable) {
                Gl.glDisable(Gl.GL_MULTISAMPLE_ARB);
                Gl.glDisable(Gl.GL_LINE_SMOOTH);
                Gl.glDisable(Gl.GL_BLEND);
            }
            Glut.glutSolidSphere(40, 15, 15);
            if (aIsSmoothEnable) {
                Gl.glEnable(Gl.GL_MULTISAMPLE_ARB);
                Gl.glEnable(Gl.GL_LINE_SMOOTH);
                Gl.glEnable(Gl.GL_BLEND);
            }
            Gl.glPopMatrix();
        }

        private void DrawCallout()
        {
            var p0 = new SlabPoint();
            var p1 = new SlabPoint();            
            p0.X = SlabModel.LeftLines[0].First().X - 20;
            p0.Y = (SlabModel.TopLines[0].First().Y + SlabModel.BottomLines[0][0].Y)/2;
            p0.Z = SlabModel.LeftLines[0].First().Z;
            p1.X = p0.X;
            p1.Y = p0.Y;
            p1.Z = SlabModel.LeftLines[0].Last().Z;            
            SetCalloutLineType();
            Gl.glBegin(Gl.GL_LINES);
            {
                Gl.glVertex3d(p0.X, p0.Y, p0.Z);
                Gl.glVertex3d(p1.X, p1.Y, p1.Z);                
            }
            Gl.glEnd();
        }
    }
}
