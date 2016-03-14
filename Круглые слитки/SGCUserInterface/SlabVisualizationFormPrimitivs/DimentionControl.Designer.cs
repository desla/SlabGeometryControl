namespace SGCUserInterface.SlabVisualizationFormPrimitivs
{
    partial class DimentionControl
    {
        /// <summary> 
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Обязательный метод для поддержки конструктора - не изменяйте 
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.nameLabel = new System.Windows.Forms.Label();
            this.valueLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.limitLabel = new System.Windows.Forms.Label();
            this.regulationLabel = new System.Windows.Forms.Label();
            this.showCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // nameLabel
            // 
            this.nameLabel.Location = new System.Drawing.Point(13, 12);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(132, 35);
            this.nameLabel.TabIndex = 0;
            this.nameLabel.Text = "Измерение";
            this.nameLabel.Click += new System.EventHandler(this.nameLabel_Click);
            // 
            // valueLabel
            // 
            this.valueLabel.AutoSize = true;
            this.valueLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.valueLabel.Location = new System.Drawing.Point(13, 44);
            this.valueLabel.Name = "valueLabel";
            this.valueLabel.Size = new System.Drawing.Size(39, 13);
            this.valueLabel.TabIndex = 1;
            this.valueLabel.Text = "0.000";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(119, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Допустимые пределы";
            // 
            // limitLabel
            // 
            this.limitLabel.AutoSize = true;
            this.limitLabel.Location = new System.Drawing.Point(13, 85);
            this.limitLabel.Name = "limitLabel";
            this.limitLabel.Size = new System.Drawing.Size(46, 13);
            this.limitLabel.TabIndex = 3;
            this.limitLabel.Text = "0.0 - 0.0";
            // 
            // regulationLabel
            // 
            this.regulationLabel.AutoSize = true;
            this.regulationLabel.Location = new System.Drawing.Point(13, 105);
            this.regulationLabel.Name = "regulationLabel";
            this.regulationLabel.Size = new System.Drawing.Size(77, 13);
            this.regulationLabel.TabIndex = 4;
            this.regulationLabel.Text = "Соответствие";
            // 
            // showCheckBox
            // 
            this.showCheckBox.AutoSize = true;
            this.showCheckBox.Checked = true;
            this.showCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showCheckBox.Location = new System.Drawing.Point(16, 125);
            this.showCheckBox.Name = "showCheckBox";
            this.showCheckBox.Size = new System.Drawing.Size(89, 17);
            this.showCheckBox.TabIndex = 5;
            this.showCheckBox.Text = "Показывать";
            this.showCheckBox.UseVisualStyleBackColor = true;
            this.showCheckBox.CheckedChanged += new System.EventHandler(this.showCheckBox_CheckedChanged);
            // 
            // DimentionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.showCheckBox);
            this.Controls.Add(this.regulationLabel);
            this.Controls.Add(this.limitLabel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.valueLabel);
            this.Controls.Add(this.nameLabel);
            this.Name = "DimentionControl";
            this.Size = new System.Drawing.Size(148, 151);
            this.Load += new System.EventHandler(this.DimentionControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.Label valueLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label limitLabel;
        private System.Windows.Forms.Label regulationLabel;
        private System.Windows.Forms.CheckBox showCheckBox;
    }
}
