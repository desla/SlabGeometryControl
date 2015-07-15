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
    public partial class StandartSizeForm : Form
    {
        private SGCClientImpl client = null;

        public StandartSizeForm(SGCClientImpl aClient)
        {
            InitializeComponent();

            client = aClient;
        }

        private void StandartSizeForm_Load(object sender, EventArgs e)
        {
            if (client != null && client.IsConnected) {
                dataGridView1.Rows.Clear();
                var standartSizes = client.GetStandartSizes();
                foreach (var standartSize in standartSizes) {
                    var row = new[] {
                        standartSize.Id.ToString(),
                        standartSize.Length.ToString(),
                        standartSize.Width.ToString(),
                        standartSize.Height.ToString(),
                        "0"
                    };
                    dataGridView1.Rows.Add(row);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var rowIndex = dataGridView1.Rows.Add(1);
            dataGridView1.Rows[rowIndex].Cells["Mask"].Value = ModifiedMask.ADDED;
        }

        private void button2_Click(object sender, EventArgs e)
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

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (client == null || !client.IsConnected) {
                MessageBox.Show(@"Не удалось выполнить операцию: нет подключения к сервису.");
                return;
            }

            for (var i = 0; i < dataGridView1.Rows.Count; ++i) {
                var row = dataGridView1.Rows[i];
                var mask = Convert.ToInt32(row.Cells["Mask"].Value);
                if ((mask & (int) ModifiedMask.DELETED) != 0) {
                    var sId = row.Cells["Id"].Value.ToString();
                    try {
                        if (!string.IsNullOrEmpty(sId)) {
                            var id = Convert.ToInt32(sId);
                            client.RemoveStandartSize(id);
                        }
                    }
                    catch (Exception ex) {
                        MessageBox.Show(@"Не удалось удалить типо-размер. Исключение: " + ex.Message);
                    }
                }
                else if ((mask & (int) ModifiedMask.ADDED) != 0) {
                    try {
                        var standartSize = new StandartSize();
                        standartSize.Length = Convert.ToDouble(row.Cells["Length"].Value);
                        standartSize.Width = Convert.ToDouble(row.Cells["Width"].Value);
                        standartSize.Height = Convert.ToDouble(row.Cells["Height"].Value);
                        client.AddStandartSize(standartSize);
                    }
                    catch (Exception ex) {
                        MessageBox.Show(@"Не удалось добавить типо-размер. Исключение: " + ex.Message);
                    }
                }
                else if ((mask & (int)ModifiedMask.CHANGED) != 0) {
                    try {
                        var standartSize = new StandartSize();
                        standartSize.Id = Convert.ToInt32(row.Cells["Id"].Value);
                        standartSize.Length = Convert.ToDouble(row.Cells["Length"].Value);
                        standartSize.Width = Convert.ToDouble(row.Cells["Width"].Value);
                        standartSize.Height = Convert.ToDouble(row.Cells["Height"].Value);
                        client.EditStandartSize(standartSize);
                    }
                    catch (Exception ex) {
                        MessageBox.Show(@"Не удалось изменить типо-размер. Исключение: " + ex.Message);
                    }
                }
            }

            this.Close();
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var mask = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["Mask"].Value);
            dataGridView1.Rows[e.RowIndex].Cells["Mask"].Value = mask | (int) ModifiedMask.CHANGED;
        }      
    }
}
