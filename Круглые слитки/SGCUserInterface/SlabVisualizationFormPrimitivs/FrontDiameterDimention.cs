using System;
using System.Linq;
using Alvasoft.SlabGeometryControl;
using Tao.FreeGlut;
using Tao.OpenGl;

namespace SGCUserInterface.SlabVisualizationFormPrimitivs
{
    public class FrontDiameterDimention : DimentionGraphicPrimitiveBase
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
            return "front_diameter";
        }

        private void DrawSphere(bool aIsSmoothEnable)
        {
            Gl.glPushMatrix();
            var p0 = new SlabPoint();
            var frontIndent = 150; // отступ.
            var pointIndex = -1;
            for (var i = 0; i < SlabModel.CenterLine.Length; ++i) {
                if (SlabModel.CenterLine[i].Z - SlabModel.CenterLine[0].Z >= frontIndent) {
                    pointIndex = i;
                    p0.X = SlabModel.CenterLine[i].X;
                    p0.Y = SlabModel.CenterLine[i].Y;
                    p0.Z = SlabModel.CenterLine[i].Z;
                    break;
                }
            }
            var translateX = p0.X + SlabModel.Diameters[pointIndex]/2.0 + 520;
            var translateY = p0.Y;
            var translateZ = p0.Z;
            Gl.glTranslated(translateX, translateY, translateZ);
            Gl.glColor3d(Convert.ToDouble(Color.R) / 255,
                         Convert.ToDouble(Color.G) / 255,
                         Convert.ToDouble(Color.B) / 255);
            if (aIsSmoothEnable) {
                Gl.glDisable(Gl.GL_MULTISAMPLE_ARB);
                Gl.glDisable(Gl.GL_LINE_SMOOTH);
                Gl.glDisable(Gl.GL_BLEND);
            }
            Glut.glutSolidSphere(SphereSize, 15, 15);
            if (aIsSmoothEnable) {
                Gl.glEnable(Gl.GL_MULTISAMPLE_ARB);
                Gl.glEnable(Gl.GL_LINE_SMOOTH);
                Gl.glEnable(Gl.GL_BLEND);
            }
            Gl.glPopMatrix();
        }

        private void DrawCallout() {
            var p0 = new SlabPoint();
            var frontIndent = 150; // отступ.
            var pointIndex = -1;
            for (var i = 0; i < SlabModel.CenterLine.Length; ++i) {
                if (SlabModel.CenterLine[i].Z - SlabModel.CenterLine[0].Z >= frontIndent) {
                    pointIndex = i;
                    p0.X = SlabModel.CenterLine[i].X;
                    p0.Y = SlabModel.CenterLine[i].Y;
                    p0.Z = SlabModel.CenterLine[i].Z;
                    break;
                }
            }

            p0.Y += SlabModel.Diameters[pointIndex] / 2.0;

            var p1 = new SlabPoint();
            p1.X = p0.X + SlabModel.Diameters[pointIndex] / 2.0 + 500;
            p1.Y = p0.Y;
            p1.Z = p0.Z;
            
            SetCalloutLineType();
            Gl.glBegin(Gl.GL_LINES);
            {
                Gl.glVertex3d(p0.X, p0.Y, p0.Z);
                Gl.glVertex3d(p1.X, p1.Y, p1.Z);
                Gl.glVertex3d(p0.X, p0.Y - SlabModel.Diameters[pointIndex], p0.Z);
                Gl.glVertex3d(p1.X, p1.Y - SlabModel.Diameters[pointIndex], p1.Z);
                Gl.glVertex3d(p1.X - 70, p1.Y, p1.Z);
                Gl.glVertex3d(p1.X - 70, p1.Y - SlabModel.Diameters[pointIndex], p1.Z);
            }
            Gl.glEnd();

        }
    }
}
