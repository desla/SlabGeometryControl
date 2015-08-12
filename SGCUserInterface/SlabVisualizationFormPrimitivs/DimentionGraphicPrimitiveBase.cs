using System;
using System.Drawing;
using System.Windows.Forms;
using Alvasoft.SlabGeometryControl;
using Tao.OpenGl;

namespace SGCUserInterface.SlabVisualizationFormPrimitivs
{
    public abstract class DimentionGraphicPrimitiveBase
    {
        private CheckBox checkBox;

        private static readonly Color calloutColor = System.Drawing.Color.LightGray;
        private static readonly Color selectedCalloutColor = System.Drawing.Color.DarkGray;

        protected const double SphereSize = 20;

        public DimentionResult Result { get; set; }

        public Dimention Dimention { get; set; }

        public bool IsVisible { get; set; }

        public bool IsSelected { get; set; }

        public Color Color { get; set; }

        public SlabModel3D SlabModel { get; set; }

        public CheckBox CheckBox
        {
            set
            {
                if (value != null) {
                    value.CheckedChanged += CheckBoxCheckChanged;
                    checkBox = value;
                }
            }
            get
            {
                return checkBox;
            }
        }        

        public abstract void DrawDimention(bool aIsSmoothEnable);

        public abstract string GetDimentionName();

        protected void SetCalloutLineType()
        {
            if (IsSelected) {
                Gl.glLineWidth(2f);
                Gl.glColor3d(Convert.ToDouble(selectedCalloutColor.R) / 255,
                    Convert.ToDouble(selectedCalloutColor.G) / 255,
                    Convert.ToDouble(selectedCalloutColor.B) / 255);
            }
            else {
                Gl.glLineWidth(1f);
                Gl.glColor3d(Convert.ToDouble(calloutColor.R) / 255,
                    Convert.ToDouble(calloutColor.G) / 255,
                    Convert.ToDouble(calloutColor.B) / 255);
            }
        }

        private void CheckBoxCheckChanged(object sender, EventArgs e)
        {
            IsVisible = checkBox.Checked;
        }
    }
}
