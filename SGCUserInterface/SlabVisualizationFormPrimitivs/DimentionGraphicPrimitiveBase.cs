using System;
using System.Drawing;
using System.Windows.Forms;
using Alvasoft.SlabGeometryControl;

namespace SGCUserInterface.SlabVisualizationFormPrimitivs
{
    public abstract class DimentionGraphicPrimitiveBase
    {
        private CheckBox checkBox;

        protected static readonly Color calloutColor = System.Drawing.Color.LightGray;

        public DimentionResult Result { get; set; }

        public Dimention Dimention { get; set; }

        public bool IsVisible { get; set; }

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

        private void CheckBoxCheckChanged(object sender, EventArgs e)
        {
            IsVisible = checkBox.Checked;
        }
    }
}
