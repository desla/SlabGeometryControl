using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Alvasoft.SlabGeometryControl;

namespace SGCUserInterface
{
    public partial class SensorsForm : Form
    {
        private SGCClientImpl client = null;

        public SensorsForm(SGCClientImpl aClient)
        {
            InitializeComponent();

            client = aClient;
        }

        private void SensorsForm_Load(object sender, EventArgs e)
        {
            if (client != null && client.IsConnected) {
                var sensorsInfos = client.GetSensorInfos();
                foreach (var sensorInfo in sensorsInfos) {
                    var row = new[] {
                        "0", // Mask
                        sensorInfo.Id.ToString(),
                        sensorInfo.Name,
                        ConvertSensorTypeToStr(sensorInfo.SensorType),
                        ConvertSensorSideToSrt(sensorInfo.Side),
                        sensorInfo.Shift.ToString(),
                        sensorInfo.State.ToString()
                    };

                    dataGridView1.Rows.Add(row);                    
                }
            }
        }

        private string ConvertSensorTypeToStr(SensorType aSensorType)
        {
            return aSensorType == SensorType.POSITION ? "Положения" : "Расстояния";
        }

        private SensorType ConvertStrToSensorType(string aStr)
        {
            if (ConvertSensorTypeToStr(SensorType.POSITION) == aStr) {
                return SensorType.POSITION;
            }
            else {
                return SensorType.PROXIMITY;
            }
        }

        private string ConvertSensorSideToSrt(SensorSide aSize)
        {
            switch (aSize) {
                case SensorSide.BOTTOM:
                    return "Нижняя";
                case SensorSide.LEFT:
                    return "Левая";
                case SensorSide.RIGHT:
                    return "Правая";
                default:
                    return "Верхняя";
            }
        }

        private SensorSide ConvertStrToSensorSide(string aStr)
        {
            if (ConvertSensorSideToSrt(SensorSide.BOTTOM) == aStr) {
                return SensorSide.BOTTOM;
            } 
            else if (ConvertSensorSideToSrt(SensorSide.LEFT) == aStr) {
                return SensorSide.LEFT;
            } 
            else if (ConvertSensorSideToSrt(SensorSide.RIGHT) == aStr) {
                return SensorSide.RIGHT;
            }
            else {
                return SensorSide.TOP;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var rowIndex = dataGridView1.Rows.Add(1);
            dataGridView1.Rows[rowIndex].Cells["Mask"].Value = ModifiedMask.ADDED;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var rowIndex = -1;
            if (dataGridView1.SelectedCells.Count > 0) {
                rowIndex = dataGridView1.SelectedCells[0].RowIndex;
            }

            if (rowIndex >= 0) {
                dataGridView1.Rows[rowIndex].Visible = false;
                var mask = Convert.ToInt32(dataGridView1.Rows[rowIndex].Cells["Mask"].Value);
                dataGridView1.Rows[rowIndex].Cells["Mask"].Value = (mask | (int)ModifiedMask.DELETED).ToString();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (client == null || !client.IsConnected) {
                MessageBox.Show(@"Не удалось выполнить операцию: нет подключения к сервису.");
                return;
            }

            for (var i = 0; i < dataGridView1.Rows.Count; ++i) {
                var row = dataGridView1.Rows[i];
                var mask = Convert.ToInt32(row.Cells["Mask"].Value);
                if ((mask & (int)ModifiedMask.DELETED) != 0) {
                    var sId = row.Cells["Id"].Value.ToString();
                    try {
                        if (!string.IsNullOrEmpty(sId)) {
                            var id = Convert.ToInt32(sId);
                            client.RemoveSensorInfo(id);
                        }
                    }
                    catch (Exception ex) {
                        MessageBox.Show(@"Не удалось удалить датчик. Исключение: " + ex.Message);
                    }
                }
                else if ((mask & (int)ModifiedMask.ADDED) != 0) {
                    try {
                        var sensorInfo = new SensorInfo();
                        sensorInfo.Name = row.Cells["Name"].Value.ToString();
                        sensorInfo.SensorType = ConvertStrToSensorType(row.Cells["Type"].Value.ToString());
                        sensorInfo.Side = ConvertStrToSensorSide(row.Cells["Side"].Value.ToString());
                        sensorInfo.Shift = Convert.ToDouble(row.Cells["Shift"].Value);
                        sensorInfo.State = SensorState.INACTIVE;
                        client.AddSensorInfo(sensorInfo);
                    }
                    catch (Exception ex) {
                        MessageBox.Show(@"Не удалось добавить описание датчика. Исключение: " + ex.Message);
                    }
                }
                else if ((mask & (int)ModifiedMask.CHANGED) != 0) {
                    try {
                        var sensorInfo = new SensorInfo();
                        sensorInfo.Id = Convert.ToInt32(row.Cells["Id"].Value);
                        sensorInfo.Name = row.Cells["Name"].ToString();
                        sensorInfo.SensorType = ConvertStrToSensorType(row.Cells["Type"].Value.ToString());
                        sensorInfo.Side = ConvertStrToSensorSide(row.Cells["Side"].Value.ToString());
                        sensorInfo.Shift = Convert.ToDouble(row.Cells["Shift"].Value);                                 
                        client.EditSensorInfo(sensorInfo);
                    }
                    catch (Exception ex) {
                        MessageBox.Show(@"Не удалось изменить описание датчика. Исключение: " + ex.Message);
                    }
                }
            }

            this.Close();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["EditButton"].Index) {
                var row = dataGridView1.Rows[e.RowIndex];
                var sensorId = Convert.ToInt32(row.Cells["Id"].Value);
                new EditOpcTagsForm(client, sensorId).ShowDialog();
            }
        }
    }
}
