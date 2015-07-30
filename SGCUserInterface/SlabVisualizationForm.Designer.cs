namespace SGCUserInterface
{
    partial class SlabVisualizationForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.plotsPage = new System.Windows.Forms.TabPage();
            this.plotsView = new ZedGraph.ZedGraphControl();
            this.slabModelPage = new System.Windows.Forms.TabPage();
            this.panel3 = new System.Windows.Forms.Panel();
            this.button6 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lengthCheckBox = new System.Windows.Forms.CheckBox();
            this.widthCheckBox = new System.Windows.Forms.CheckBox();
            this.heightCheckBox = new System.Windows.Forms.CheckBox();
            this.allDimentionsCheckBox = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.smoothCheckedBox = new System.Windows.Forms.CheckBox();
            this.sensorValuesCheckBox = new System.Windows.Forms.CheckBox();
            this.dimensionsCheckBox = new System.Windows.Forms.CheckBox();
            this.gridSurfaceCheckBox = new System.Windows.Forms.CheckBox();
            this.modelPanel = new Tao.Platform.Windows.SimpleOpenGlControl();
            this.lateralRightCheckBox = new System.Windows.Forms.CheckBox();
            this.lateralLeftCheckBox = new System.Windows.Forms.CheckBox();
            this.longitudinalTopCheckBox = new System.Windows.Forms.CheckBox();
            this.tabControl1.SuspendLayout();
            this.plotsPage.SuspendLayout();
            this.slabModelPage.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.plotsPage);
            this.tabControl1.Controls.Add(this.slabModelPage);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(704, 425);
            this.tabControl1.TabIndex = 0;
            // 
            // plotsPage
            // 
            this.plotsPage.Controls.Add(this.plotsView);
            this.plotsPage.Location = new System.Drawing.Point(4, 22);
            this.plotsPage.Name = "plotsPage";
            this.plotsPage.Padding = new System.Windows.Forms.Padding(3);
            this.plotsPage.Size = new System.Drawing.Size(696, 399);
            this.plotsPage.TabIndex = 0;
            this.plotsPage.Text = "Показания датчиков";
            this.plotsPage.UseVisualStyleBackColor = true;
            // 
            // plotsView
            // 
            this.plotsView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plotsView.Location = new System.Drawing.Point(3, 3);
            this.plotsView.Name = "plotsView";
            this.plotsView.ScrollGrace = 0D;
            this.plotsView.ScrollMaxX = 0D;
            this.plotsView.ScrollMaxY = 0D;
            this.plotsView.ScrollMaxY2 = 0D;
            this.plotsView.ScrollMinX = 0D;
            this.plotsView.ScrollMinY = 0D;
            this.plotsView.ScrollMinY2 = 0D;
            this.plotsView.Size = new System.Drawing.Size(690, 393);
            this.plotsView.TabIndex = 0;
            this.plotsView.ContextMenuBuilder += new ZedGraph.ZedGraphControl.ContextMenuBuilderEventHandler(this.plotsView_ContextMenuBuilder);
            // 
            // slabModelPage
            // 
            this.slabModelPage.Controls.Add(this.panel3);
            this.slabModelPage.Controls.Add(this.panel1);
            this.slabModelPage.Controls.Add(this.modelPanel);
            this.slabModelPage.Location = new System.Drawing.Point(4, 22);
            this.slabModelPage.Name = "slabModelPage";
            this.slabModelPage.Padding = new System.Windows.Forms.Padding(3);
            this.slabModelPage.Size = new System.Drawing.Size(696, 399);
            this.slabModelPage.TabIndex = 1;
            this.slabModelPage.Text = "3-D модель слитка";
            this.slabModelPage.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.White;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.button6);
            this.panel3.Controls.Add(this.button5);
            this.panel3.Controls.Add(this.button4);
            this.panel3.Controls.Add(this.button3);
            this.panel3.Location = new System.Drawing.Point(8, 6);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(80, 80);
            this.panel3.TabIndex = 2;
            // 
            // button6
            // 
            this.button6.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button6.Location = new System.Drawing.Point(40, 40);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(40, 40);
            this.button6.TabIndex = 3;
            this.button6.Text = "У";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.angleSideButton_Click);
            // 
            // button5
            // 
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button5.Location = new System.Drawing.Point(0, 40);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(40, 40);
            this.button5.TabIndex = 2;
            this.button5.Text = "В";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.topSideButton_Click);
            // 
            // button4
            // 
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button4.Location = new System.Drawing.Point(0, 0);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(40, 40);
            this.button4.TabIndex = 1;
            this.button4.Text = "Ф";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.frontSideButton_Click);
            // 
            // button3
            // 
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button3.Location = new System.Drawing.Point(40, 0);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(40, 40);
            this.button3.TabIndex = 0;
            this.button3.Text = "Л";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.leftSideButton_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.longitudinalTopCheckBox);
            this.panel1.Controls.Add(this.lateralLeftCheckBox);
            this.panel1.Controls.Add(this.lateralRightCheckBox);
            this.panel1.Controls.Add(this.lengthCheckBox);
            this.panel1.Controls.Add(this.widthCheckBox);
            this.panel1.Controls.Add(this.heightCheckBox);
            this.panel1.Controls.Add(this.allDimentionsCheckBox);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.smoothCheckedBox);
            this.panel1.Controls.Add(this.sensorValuesCheckBox);
            this.panel1.Controls.Add(this.dimensionsCheckBox);
            this.panel1.Controls.Add(this.gridSurfaceCheckBox);
            this.panel1.Location = new System.Drawing.Point(500, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(188, 330);
            this.panel1.TabIndex = 1;
            // 
            // lengthCheckBox
            // 
            this.lengthCheckBox.AutoSize = true;
            this.lengthCheckBox.Checked = true;
            this.lengthCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.lengthCheckBox.Location = new System.Drawing.Point(7, 180);
            this.lengthCheckBox.Name = "lengthCheckBox";
            this.lengthCheckBox.Size = new System.Drawing.Size(97, 17);
            this.lengthCheckBox.TabIndex = 11;
            this.lengthCheckBox.Text = "Длина слитка";
            this.lengthCheckBox.UseVisualStyleBackColor = true;
            // 
            // widthCheckBox
            // 
            this.widthCheckBox.AutoSize = true;
            this.widthCheckBox.Checked = true;
            this.widthCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.widthCheckBox.Location = new System.Drawing.Point(7, 160);
            this.widthCheckBox.Name = "widthCheckBox";
            this.widthCheckBox.Size = new System.Drawing.Size(103, 17);
            this.widthCheckBox.TabIndex = 10;
            this.widthCheckBox.Text = "Ширина слитка";
            this.widthCheckBox.UseVisualStyleBackColor = true;
            // 
            // heightCheckBox
            // 
            this.heightCheckBox.AutoSize = true;
            this.heightCheckBox.Checked = true;
            this.heightCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.heightCheckBox.Location = new System.Drawing.Point(7, 140);
            this.heightCheckBox.Name = "heightCheckBox";
            this.heightCheckBox.Size = new System.Drawing.Size(102, 17);
            this.heightCheckBox.TabIndex = 9;
            this.heightCheckBox.Text = "Высота слитка";
            this.heightCheckBox.UseVisualStyleBackColor = true;
            // 
            // allDimentionsCheckBox
            // 
            this.allDimentionsCheckBox.AutoSize = true;
            this.allDimentionsCheckBox.Checked = true;
            this.allDimentionsCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.allDimentionsCheckBox.Location = new System.Drawing.Point(7, 120);
            this.allDimentionsCheckBox.Name = "allDimentionsCheckBox";
            this.allDimentionsCheckBox.Size = new System.Drawing.Size(104, 17);
            this.allDimentionsCheckBox.TabIndex = 8;
            this.allDimentionsCheckBox.Text = "Все измерения";
            this.allDimentionsCheckBox.UseVisualStyleBackColor = true;
            this.allDimentionsCheckBox.CheckedChanged += new System.EventHandler(this.allDimentionsCheckBox_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 105);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(127, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "____________________";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(127, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "____________________";
            // 
            // smoothCheckedBox
            // 
            this.smoothCheckedBox.AutoSize = true;
            this.smoothCheckedBox.Checked = true;
            this.smoothCheckedBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.smoothCheckedBox.Location = new System.Drawing.Point(7, 87);
            this.smoothCheckedBox.Name = "smoothCheckedBox";
            this.smoothCheckedBox.Size = new System.Drawing.Size(94, 17);
            this.smoothCheckedBox.TabIndex = 3;
            this.smoothCheckedBox.Text = "Сглаживание";
            this.smoothCheckedBox.UseVisualStyleBackColor = true;
            this.smoothCheckedBox.CheckedChanged += new System.EventHandler(this.smoothCheckedBox_CheckedChanged);
            // 
            // sensorValuesCheckBox
            // 
            this.sensorValuesCheckBox.AutoSize = true;
            this.sensorValuesCheckBox.Checked = true;
            this.sensorValuesCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.sensorValuesCheckBox.Location = new System.Drawing.Point(7, 50);
            this.sensorValuesCheckBox.Name = "sensorValuesCheckBox";
            this.sensorValuesCheckBox.Size = new System.Drawing.Size(131, 17);
            this.sensorValuesCheckBox.TabIndex = 2;
            this.sensorValuesCheckBox.Text = "Показания датчиков";
            this.sensorValuesCheckBox.UseVisualStyleBackColor = true;
            this.sensorValuesCheckBox.CheckedChanged += new System.EventHandler(this.sensorValuesCheckBox_CheckedChanged);
            // 
            // dimensionsCheckBox
            // 
            this.dimensionsCheckBox.AutoSize = true;
            this.dimensionsCheckBox.Checked = true;
            this.dimensionsCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.dimensionsCheckBox.Location = new System.Drawing.Point(7, 30);
            this.dimensionsCheckBox.Name = "dimensionsCheckBox";
            this.dimensionsCheckBox.Size = new System.Drawing.Size(113, 17);
            this.dimensionsCheckBox.TabIndex = 1;
            this.dimensionsCheckBox.Text = "Габариты слитка";
            this.dimensionsCheckBox.UseVisualStyleBackColor = true;
            this.dimensionsCheckBox.CheckedChanged += new System.EventHandler(this.dimensionsCheckBox_CheckedChanged);
            // 
            // gridSurfaceCheckBox
            // 
            this.gridSurfaceCheckBox.AutoSize = true;
            this.gridSurfaceCheckBox.Checked = true;
            this.gridSurfaceCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.gridSurfaceCheckBox.Location = new System.Drawing.Point(7, 10);
            this.gridSurfaceCheckBox.Name = "gridSurfaceCheckBox";
            this.gridSurfaceCheckBox.Size = new System.Drawing.Size(123, 17);
            this.gridSurfaceCheckBox.TabIndex = 0;
            this.gridSurfaceCheckBox.Text = "Сетка поверхности";
            this.gridSurfaceCheckBox.UseVisualStyleBackColor = true;
            this.gridSurfaceCheckBox.CheckedChanged += new System.EventHandler(this.gridSurfaceCheckBox_CheckedChanged);
            // 
            // modelPanel
            // 
            this.modelPanel.AccumBits = ((byte)(0));
            this.modelPanel.AutoCheckErrors = false;
            this.modelPanel.AutoFinish = false;
            this.modelPanel.AutoMakeCurrent = true;
            this.modelPanel.AutoSwapBuffers = true;
            this.modelPanel.BackColor = System.Drawing.Color.Black;
            this.modelPanel.ColorBits = ((byte)(32));
            this.modelPanel.DepthBits = ((byte)(16));
            this.modelPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.modelPanel.Location = new System.Drawing.Point(3, 3);
            this.modelPanel.Name = "modelPanel";
            this.modelPanel.Size = new System.Drawing.Size(690, 393);
            this.modelPanel.StencilBits = ((byte)(0));
            this.modelPanel.TabIndex = 0;
            this.modelPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.modelPanel_MouseDown);
            this.modelPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.modelPanel_MouseMove);
            // 
            // lateralRightCheckBox
            // 
            this.lateralRightCheckBox.AutoSize = true;
            this.lateralRightCheckBox.Checked = true;
            this.lateralRightCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.lateralRightCheckBox.Location = new System.Drawing.Point(7, 200);
            this.lateralRightCheckBox.Name = "lateralRightCheckBox";
            this.lateralRightCheckBox.Size = new System.Drawing.Size(179, 17);
            this.lateralRightCheckBox.TabIndex = 12;
            this.lateralRightCheckBox.Text = "Поперечная кривизна (Право)";
            this.lateralRightCheckBox.UseVisualStyleBackColor = true;
            // 
            // lateralLeftCheckBox
            // 
            this.lateralLeftCheckBox.AutoSize = true;
            this.lateralLeftCheckBox.Checked = true;
            this.lateralLeftCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.lateralLeftCheckBox.Location = new System.Drawing.Point(7, 220);
            this.lateralLeftCheckBox.Name = "lateralLeftCheckBox";
            this.lateralLeftCheckBox.Size = new System.Drawing.Size(173, 17);
            this.lateralLeftCheckBox.TabIndex = 13;
            this.lateralLeftCheckBox.Text = "Поперечная кривизна (Лево)";
            this.lateralLeftCheckBox.UseVisualStyleBackColor = true;
            // 
            // longitudinalTopCheckBox
            // 
            this.longitudinalTopCheckBox.AutoSize = true;
            this.longitudinalTopCheckBox.Checked = true;
            this.longitudinalTopCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.longitudinalTopCheckBox.Location = new System.Drawing.Point(7, 240);
            this.longitudinalTopCheckBox.Name = "longitudinalTopCheckBox";
            this.longitudinalTopCheckBox.Size = new System.Drawing.Size(172, 17);
            this.longitudinalTopCheckBox.TabIndex = 14;
            this.longitudinalTopCheckBox.Text = "Продольная кривизна (Верх)";
            this.longitudinalTopCheckBox.UseVisualStyleBackColor = true;
            // 
            // SlabVisualizationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.HighlightText;
            this.ClientSize = new System.Drawing.Size(704, 425);
            this.Controls.Add(this.tabControl1);
            this.Name = "SlabVisualizationForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Данные слитка";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SlabVisualizationForm_FormClosing);
            this.Load += new System.EventHandler(this.SlabVisualizationForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.plotsPage.ResumeLayout(false);
            this.slabModelPage.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage plotsPage;
        private ZedGraph.ZedGraphControl plotsView;
        private System.Windows.Forms.TabPage slabModelPage;
        private Tao.Platform.Windows.SimpleOpenGlControl modelPanel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox gridSurfaceCheckBox;
        private System.Windows.Forms.CheckBox dimensionsCheckBox;
        private System.Windows.Forms.CheckBox sensorValuesCheckBox;
        private System.Windows.Forms.CheckBox smoothCheckedBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox allDimentionsCheckBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox heightCheckBox;
        private System.Windows.Forms.CheckBox widthCheckBox;
        private System.Windows.Forms.CheckBox lengthCheckBox;
        private System.Windows.Forms.Button angleSideButton;
        private System.Windows.Forms.Button topSideButton;
        private System.Windows.Forms.Button leftSideButton;
        private System.Windows.Forms.Button frontSideButton;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button leftButtonSide;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button fronSideButton;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.CheckBox lateralRightCheckBox;
        private System.Windows.Forms.CheckBox lateralLeftCheckBox;
        private System.Windows.Forms.CheckBox longitudinalTopCheckBox;
    }
}