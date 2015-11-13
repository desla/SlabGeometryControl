namespace Emulator
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using Configuration;

    public partial class Form1 : Form
    {
        private EmulatorConfiguration configuration;

        public Form1()
        {
            //var c = new EmulatorConfiguration();
            //c.Length = 3000;
            //c.Speed = 150;
            //c.Bumps = new BumpConfiguration[] {
            //    new BumpConfiguration {
            //        Position = 700,
            //        Direction = new PointF(0, 20)
            //    }
            //};
            //c.RattleLimit = 1.5;            
            //c.Flexs = new FlexConfiguration[] {
            //    new FlexConfiguration {
            //        Position = 1500,
            //        Maxumum = 5
            //    }
            //};
            //c.Frame = new FrameConfiguration {
            //    Error = 1,
            //    ScanSpeed = 150,
            //    Sensors = new SensorConfiguration[] {
            //        new SensorConfiguration {
            //            Position = new PointF(0, 70),
            //            ScanVertor = new PointF(0, -1)
            //        },
            //        new SensorConfiguration {
            //            Position = new PointF(70, 0),
            //            ScanVertor = new PointF(-1, 0)
            //        },
            //        new SensorConfiguration {
            //            Position = new PointF(0, -70),
            //            ScanVertor = new PointF(0, 1)
            //        },
            //        new SensorConfiguration {
            //            Position = new PointF(-70, 0),
            //            ScanVertor = new PointF(1, 0)
            //        },
            //    }
            //};

            //c.Serialize("../../Settings/Settings.xml");

            InitializeComponent();
        }

        private void загрузитьКонфигурациюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog()) {
                dialog.InitialDirectory = Application.ExecutablePath;
                dialog.Filter = "xml-файлы (*.xml)|*.xml";
                dialog.FilterIndex = 0;
                dialog.RestoreDirectory = true;
                if (dialog.ShowDialog() == DialogResult.OK) {
                    configuration = EmulatorConfiguration.Deserialize(dialog.FileName);
                    MessageBox.Show("Конфигурация загружена. " + dialog.FileName);
                }
            }            
        }
    }
}
