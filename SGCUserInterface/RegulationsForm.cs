using System;
using System.Linq;
using System.Windows.Forms;
using Alvasoft.SlabGeometryControl;

namespace SGCUserInterface
{
    public partial class RegulationsForm : Form
    {
        private SGCClientImpl client = null;
        private Dimention[] dimentions = null;
        private StandartSize[] standartSizes = null;

        public RegulationsForm(SGCClientImpl aClient)
        {
            InitializeComponent();

            client = aClient;

            if (client != null && client.IsConnected) {
                dimentions = client.GetDimentions();
                standartSizes = client.GetStandartSizes();
                var dimentionColumn = dataGridView1.Columns["Dimention"] as DataGridViewComboBoxColumn;
                var standartSizesColumn = dataGridView1.Columns["StandartSize"] as DataGridViewComboBoxColumn;
                foreach (var dimention in dimentions) {                    
                    dimentionColumn.Items.Add(dimention.Description);
                }
                foreach (var standartSize in standartSizes) {
                    standartSizesColumn.Items.Add(standartSize.ToString());
                }
            }
        }

        private void RegulationsForm_Load(object sender, EventArgs e)
        {
            if (client != null && client.IsConnected) {
                var regulations = client.GetRegulations();                
                foreach (var regulation in regulations) {
                    var dimention = dimentions.FirstOrDefault(d => d.Id == regulation.DimentionId);
                    var standartSize = standartSizes.FirstOrDefault(s => s.Id == regulation.StandartSizeId);
                    var row = new[] {
                        regulation.Id.ToString(),
                        "0",
                        dimention == null ? "-" : dimention.Description ,
                        standartSize == null ? "-" : standartSize.ToString(),
                        regulation.MinValue.ToString(),
                        regulation.MaxValue.ToString()
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
                if ((mask & (int) ModifiedMask.DELETED) != 0) {
                    var sId = row.Cells["Id"].Value.ToString();
                    try {
                        if (!string.IsNullOrEmpty(sId)) {
                            var id = Convert.ToInt32(sId);
                            client.RemoveRegulation(id);
                        }
                    }
                    catch (Exception ex) {
                        MessageBox.Show(@"Не удалось удалить правило. Исключение: " + ex.Message);
                    }
                }
                else if ((mask & (int) ModifiedMask.ADDED) != 0) {
                    try {
                        var standartSize =
                            standartSizes.FirstOrDefault(s => s.ToString() == row.Cells["StandartSize"].Value.ToString());
                        var dimention = dimentions.FirstOrDefault(d => d.Description == row.Cells["Dimention"].Value.ToString());

                        var regulation = new Regulation();
                        regulation.DimentionId = dimention.Id;
                        regulation.StandartSizeId = standartSize.Id;
                        regulation.MinValue = Convert.ToDouble(row.Cells["Min"].Value);
                        regulation.MaxValue = Convert.ToDouble(row.Cells["Max"].Value);
                        client.AddRegulation(regulation);
                    }
                    catch (Exception ex) {
                        MessageBox.Show(@"Не удалось добавить правило. Исключение: " + ex.Message);
                    }
                }
                else if ((mask & (int) ModifiedMask.CHANGED) != 0) {
                    try {
                        var standartSize =
                            standartSizes.FirstOrDefault(s => s.ToString() == row.Cells["StandartSize"].Value.ToString());
                        var dimention = dimentions.FirstOrDefault(d => d.Description == row.Cells["Dimention"].Value.ToString());                        

                        var regulation = new Regulation();
                        regulation.Id = Convert.ToInt32(row.Cells["Id"].Value);
                        regulation.DimentionId = dimention.Id;
                        regulation.StandartSizeId = standartSize.Id;
                        regulation.MinValue = Convert.ToDouble(row.Cells["Min"].Value);
                        regulation.MaxValue = Convert.ToDouble(row.Cells["Max"].Value);
                        client.EditRegulation(regulation);
                    }
                    catch (Exception ex) {
                        MessageBox.Show(@"Не удалось изменить правило. Исключение: " + ex.Message);
                    }
                }                
            }

            Close();
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var mask = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["Mask"].Value);
            dataGridView1.Rows[e.RowIndex].Cells["Mask"].Value = mask | (int)ModifiedMask.CHANGED;
        }
    }
}
