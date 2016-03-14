using System;
using System.Drawing;
using System.Windows.Forms;
using Alvasoft.SlabGeometryControl;

namespace SGCUserInterface.SlabVisualizationFormPrimitivs
{
    public partial class DimentionControl : UserControl
    {
        private DimentionGraphicPrimitiveBase dimentionPrimitive;

        public DimentionControl()
        {
            InitializeComponent();
        }

        private void DimentionControl_Load(object sender, EventArgs e)
        {
        }

        public void SetDimentionPrimitive(DimentionGraphicPrimitiveBase aDimentionPrimitive)
        {
            dimentionPrimitive = null;
            showCheckBox.Checked = true;
            dimentionPrimitive = aDimentionPrimitive;
            if (dimentionPrimitive != null) {
                nameLabel.Text = dimentionPrimitive.Dimention.Description;
                valueLabel.Text = dimentionPrimitive.Result.Value.ToString();
            }
        }

        public void SetRegulation(Regulation aRegulation)
        {
            if (aRegulation != null) {
                limitLabel.Text = aRegulation.MinValue + @" - " + aRegulation.MaxValue;
                if (dimentionPrimitive != null) {
                    if (dimentionPrimitive.Result != null) {
                        var value = dimentionPrimitive.Result.Value;
                        if (value >= aRegulation.MinValue && value <= aRegulation.MaxValue) {
                            regulationLabel.Text = @"Соответствует";
                            regulationLabel.ForeColor = Color.Green;
                        }
                        else {
                            regulationLabel.Text = @"Не соответствует";
                            regulationLabel.ForeColor = Color.Red;
                        }
                    }
                }
            }
            else {                
                regulationLabel.Text = @"";
                limitLabel.Text = @"Отсутствуют";
            }
        }

        private void showCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (dimentionPrimitive != null) {
                if (dimentionPrimitive.CheckBox != null) {
                    dimentionPrimitive.CheckBox.Checked = showCheckBox.Checked;
                }
            }
        }

        private void nameLabel_Click(object sender, EventArgs e) {

        }
    }
}
