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
            this.panel4 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.isShowNodesCheckedBox = new System.Windows.Forms.CheckBox();
            this.isAllPlotShowCheckedBox = new System.Windows.Forms.CheckBox();
            this.rightButton = new System.Windows.Forms.Button();
            this.leftButton = new System.Windows.Forms.Button();
            this.plotsView = new ZedGraph.ZedGraphControl();
            this.sectionsPage = new System.Windows.Forms.TabPage();
            this.panel5 = new System.Windows.Forms.Panel();
            this.button8 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.sectionsView = new ZedGraph.ZedGraphControl();
            this.deviationPage = new System.Windows.Forms.TabPage();
            this.panel6 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.button9 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.deviationView = new ZedGraph.ZedGraphControl();
            this.slabModelPage = new System.Windows.Forms.TabPage();
            this.panel3 = new System.Windows.Forms.Panel();
            this.button6 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.sensorsValuesCheckedBox = new System.Windows.Forms.CheckBox();
            this.frontDiameterCheckBox = new System.Windows.Forms.CheckBox();
            this.lengthCheckBox = new System.Windows.Forms.CheckBox();
            this.allDimentionsCheckBox = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.smoothCheckedBox = new System.Windows.Forms.CheckBox();
            this.centersValuesCheckBox = new System.Windows.Forms.CheckBox();
            this.dimensionsCheckBox = new System.Windows.Forms.CheckBox();
            this.gridSurfaceCheckBox = new System.Windows.Forms.CheckBox();
            this.modelView = new Tao.Platform.Windows.SimpleOpenGlControl();
            this.backDiameterCheckBox = new System.Windows.Forms.CheckBox();
            this.tabControl1.SuspendLayout();
            this.plotsPage.SuspendLayout();
            this.panel4.SuspendLayout();
            this.sectionsPage.SuspendLayout();
            this.panel5.SuspendLayout();
            this.deviationPage.SuspendLayout();
            this.panel6.SuspendLayout();
            this.slabModelPage.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.plotsPage);
            this.tabControl1.Controls.Add(this.sectionsPage);
            this.tabControl1.Controls.Add(this.deviationPage);
            this.tabControl1.Controls.Add(this.slabModelPage);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(723, 506);
            this.tabControl1.TabIndex = 0;
            // 
            // plotsPage
            // 
            this.plotsPage.Controls.Add(this.panel4);
            this.plotsPage.Controls.Add(this.plotsView);
            this.plotsPage.Location = new System.Drawing.Point(4, 22);
            this.plotsPage.Name = "plotsPage";
            this.plotsPage.Padding = new System.Windows.Forms.Padding(3);
            this.plotsPage.Size = new System.Drawing.Size(715, 480);
            this.plotsPage.TabIndex = 0;
            this.plotsPage.Text = "Показания датчиков";
            this.plotsPage.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            this.panel4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel4.BackColor = System.Drawing.Color.White;
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.label4);
            this.panel4.Controls.Add(this.isShowNodesCheckedBox);
            this.panel4.Controls.Add(this.isAllPlotShowCheckedBox);
            this.panel4.Controls.Add(this.rightButton);
            this.panel4.Controls.Add(this.leftButton);
            this.panel4.Location = new System.Drawing.Point(547, 6);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(165, 101);
            this.panel4.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(36, 6);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Выбор датчика";
            // 
            // isShowNodesCheckedBox
            // 
            this.isShowNodesCheckedBox.AutoSize = true;
            this.isShowNodesCheckedBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.isShowNodesCheckedBox.Location = new System.Drawing.Point(3, 74);
            this.isShowNodesCheckedBox.Name = "isShowNodesCheckedBox";
            this.isShowNodesCheckedBox.Size = new System.Drawing.Size(145, 17);
            this.isShowNodesCheckedBox.TabIndex = 4;
            this.isShowNodesCheckedBox.Text = "Показывать узлы точек";
            this.isShowNodesCheckedBox.UseVisualStyleBackColor = true;
            this.isShowNodesCheckedBox.CheckedChanged += new System.EventHandler(this.isShowNodesCheckedBox_CheckedChanged);
            // 
            // isAllPlotShowCheckedBox
            // 
            this.isAllPlotShowCheckedBox.AutoSize = true;
            this.isAllPlotShowCheckedBox.Checked = true;
            this.isAllPlotShowCheckedBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.isAllPlotShowCheckedBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.isAllPlotShowCheckedBox.Location = new System.Drawing.Point(3, 51);
            this.isAllPlotShowCheckedBox.Name = "isAllPlotShowCheckedBox";
            this.isAllPlotShowCheckedBox.Size = new System.Drawing.Size(153, 17);
            this.isAllPlotShowCheckedBox.TabIndex = 3;
            this.isAllPlotShowCheckedBox.Text = "Показывать все графики";
            this.isAllPlotShowCheckedBox.UseVisualStyleBackColor = true;
            this.isAllPlotShowCheckedBox.CheckedChanged += new System.EventHandler(this.isAllPlotShowCheckedBox_CheckedChanged);
            // 
            // rightButton
            // 
            this.rightButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.rightButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rightButton.Location = new System.Drawing.Point(84, 22);
            this.rightButton.Name = "rightButton";
            this.rightButton.Size = new System.Drawing.Size(74, 23);
            this.rightButton.TabIndex = 2;
            this.rightButton.Text = ">";
            this.rightButton.UseVisualStyleBackColor = false;
            this.rightButton.Click += new System.EventHandler(this.rightButton_Click);
            // 
            // leftButton
            // 
            this.leftButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.leftButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.leftButton.Location = new System.Drawing.Point(4, 22);
            this.leftButton.Name = "leftButton";
            this.leftButton.Size = new System.Drawing.Size(74, 23);
            this.leftButton.TabIndex = 1;
            this.leftButton.Text = "<";
            this.leftButton.UseVisualStyleBackColor = false;
            this.leftButton.Click += new System.EventHandler(this.leftButton_Click);
            // 
            // plotsView
            // 
            this.plotsView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.plotsView.Location = new System.Drawing.Point(-4, -10);
            this.plotsView.Name = "plotsView";
            this.plotsView.ScrollGrace = 0D;
            this.plotsView.ScrollMaxX = 0D;
            this.plotsView.ScrollMaxY = 0D;
            this.plotsView.ScrollMaxY2 = 0D;
            this.plotsView.ScrollMinX = 0D;
            this.plotsView.ScrollMinY = 0D;
            this.plotsView.ScrollMinY2 = 0D;
            this.plotsView.Size = new System.Drawing.Size(723, 500);
            this.plotsView.TabIndex = 0;
            this.plotsView.ContextMenuBuilder += new ZedGraph.ZedGraphControl.ContextMenuBuilderEventHandler(this.plotsView_ContextMenuBuilder);
            // 
            // sectionsPage
            // 
            this.sectionsPage.Controls.Add(this.panel5);
            this.sectionsPage.Controls.Add(this.sectionsView);
            this.sectionsPage.Location = new System.Drawing.Point(4, 22);
            this.sectionsPage.Name = "sectionsPage";
            this.sectionsPage.Padding = new System.Windows.Forms.Padding(3);
            this.sectionsPage.Size = new System.Drawing.Size(715, 480);
            this.sectionsPage.TabIndex = 2;
            this.sectionsPage.Text = "Срезы слитка";
            this.sectionsPage.UseVisualStyleBackColor = true;
            // 
            // panel5
            // 
            this.panel5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel5.BackColor = System.Drawing.Color.White;
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Controls.Add(this.button8);
            this.panel5.Controls.Add(this.button7);
            this.panel5.Location = new System.Drawing.Point(589, 6);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(123, 125);
            this.panel5.TabIndex = 4;
            // 
            // button8
            // 
            this.button8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button8.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button8.FlatAppearance.CheckedBackColor = System.Drawing.Color.White;
            this.button8.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            this.button8.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button8.Location = new System.Drawing.Point(12, 65);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(98, 48);
            this.button8.TabIndex = 3;
            this.button8.Text = "СВЕРХУ";
            this.button8.UseVisualStyleBackColor = false;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button7
            // 
            this.button7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button7.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button7.FlatAppearance.CheckedBackColor = System.Drawing.Color.White;
            this.button7.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            this.button7.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button7.Location = new System.Drawing.Point(12, 11);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(98, 48);
            this.button7.TabIndex = 2;
            this.button7.Text = "СЛЕВА";
            this.button7.UseVisualStyleBackColor = false;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // sectionsView
            // 
            this.sectionsView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionsView.Location = new System.Drawing.Point(-2, -10);
            this.sectionsView.Name = "sectionsView";
            this.sectionsView.ScrollGrace = 0D;
            this.sectionsView.ScrollMaxX = 0D;
            this.sectionsView.ScrollMaxY = 0D;
            this.sectionsView.ScrollMaxY2 = 0D;
            this.sectionsView.ScrollMinX = 0D;
            this.sectionsView.ScrollMinY = 0D;
            this.sectionsView.ScrollMinY2 = 0D;
            this.sectionsView.Size = new System.Drawing.Size(719, 504);
            this.sectionsView.TabIndex = 1;
            this.sectionsView.ContextMenuBuilder += new ZedGraph.ZedGraphControl.ContextMenuBuilderEventHandler(this.sectionsView_ContextMenuBuilder);
            // 
            // deviationPage
            // 
            this.deviationPage.Controls.Add(this.panel6);
            this.deviationPage.Controls.Add(this.deviationView);
            this.deviationPage.Location = new System.Drawing.Point(4, 22);
            this.deviationPage.Name = "deviationPage";
            this.deviationPage.Padding = new System.Windows.Forms.Padding(3);
            this.deviationPage.Size = new System.Drawing.Size(715, 480);
            this.deviationPage.TabIndex = 3;
            this.deviationPage.Text = "Графики отклонения от среднего";
            this.deviationPage.UseVisualStyleBackColor = true;
            // 
            // panel6
            // 
            this.panel6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel6.BackColor = System.Drawing.Color.White;
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel6.Controls.Add(this.label3);
            this.panel6.Controls.Add(this.button9);
            this.panel6.Controls.Add(this.button10);
            this.panel6.Location = new System.Drawing.Point(562, 6);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(147, 73);
            this.panel6.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(30, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Выбор стороны";
            // 
            // button9
            // 
            this.button9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button9.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button9.FlatAppearance.CheckedBackColor = System.Drawing.Color.White;
            this.button9.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            this.button9.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button9.Location = new System.Drawing.Point(82, 25);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(45, 29);
            this.button9.TabIndex = 3;
            this.button9.Text = ">>";
            this.button9.UseVisualStyleBackColor = false;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // button10
            // 
            this.button10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button10.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button10.FlatAppearance.CheckedBackColor = System.Drawing.Color.White;
            this.button10.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            this.button10.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button10.Location = new System.Drawing.Point(19, 25);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(45, 29);
            this.button10.TabIndex = 2;
            this.button10.Text = "<<";
            this.button10.UseVisualStyleBackColor = false;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // deviationView
            // 
            this.deviationView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.deviationView.IsEnableHZoom = false;
            this.deviationView.Location = new System.Drawing.Point(-2, -7);
            this.deviationView.Name = "deviationView";
            this.deviationView.ScrollGrace = 0D;
            this.deviationView.ScrollMaxX = 0D;
            this.deviationView.ScrollMaxY = 0D;
            this.deviationView.ScrollMaxY2 = 0D;
            this.deviationView.ScrollMinX = 0D;
            this.deviationView.ScrollMinY = 0D;
            this.deviationView.ScrollMinY2 = 0D;
            this.deviationView.Size = new System.Drawing.Size(719, 503);
            this.deviationView.TabIndex = 2;
            this.deviationView.ContextMenuBuilder += new ZedGraph.ZedGraphControl.ContextMenuBuilderEventHandler(this.deviationView_ContextMenuBuilder);
            // 
            // slabModelPage
            // 
            this.slabModelPage.Controls.Add(this.panel3);
            this.slabModelPage.Controls.Add(this.panel1);
            this.slabModelPage.Controls.Add(this.modelView);
            this.slabModelPage.Location = new System.Drawing.Point(4, 22);
            this.slabModelPage.Name = "slabModelPage";
            this.slabModelPage.Padding = new System.Windows.Forms.Padding(3);
            this.slabModelPage.Size = new System.Drawing.Size(715, 480);
            this.slabModelPage.TabIndex = 1;
            this.slabModelPage.Text = "3-D модель слитка";
            this.slabModelPage.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.BackColor = System.Drawing.Color.White;
            this.panel3.Controls.Add(this.button6);
            this.panel3.Controls.Add(this.button5);
            this.panel3.Controls.Add(this.button4);
            this.panel3.Controls.Add(this.button3);
            this.panel3.Location = new System.Drawing.Point(509, 290);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(200, 100);
            this.panel3.TabIndex = 2;
            // 
            // button6
            // 
            this.button6.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button6.FlatAppearance.CheckedBackColor = System.Drawing.Color.White;
            this.button6.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            this.button6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button6.Location = new System.Drawing.Point(100, 50);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(98, 48);
            this.button6.TabIndex = 3;
            this.button6.Text = "УГОЛ";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.angleSideButton_Click);
            // 
            // button5
            // 
            this.button5.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button5.FlatAppearance.CheckedBackColor = System.Drawing.Color.White;
            this.button5.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button5.Location = new System.Drawing.Point(0, 50);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(98, 48);
            this.button5.TabIndex = 2;
            this.button5.Text = "СВЕРХУ";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.topSideButton_Click);
            // 
            // button4
            // 
            this.button4.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button4.FlatAppearance.CheckedBackColor = System.Drawing.Color.White;
            this.button4.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Location = new System.Drawing.Point(0, 0);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(98, 48);
            this.button4.TabIndex = 1;
            this.button4.Text = "СПЕРЕДИ";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.frontSideButton_Click);
            // 
            // button3
            // 
            this.button3.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button3.FlatAppearance.CheckedBackColor = System.Drawing.Color.White;
            this.button3.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Location = new System.Drawing.Point(100, 0);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(98, 48);
            this.button3.TabIndex = 0;
            this.button3.Text = "СЛЕВА";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.leftSideButton_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.backDiameterCheckBox);
            this.panel1.Controls.Add(this.sensorsValuesCheckedBox);
            this.panel1.Controls.Add(this.frontDiameterCheckBox);
            this.panel1.Controls.Add(this.lengthCheckBox);
            this.panel1.Controls.Add(this.allDimentionsCheckBox);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.smoothCheckedBox);
            this.panel1.Controls.Add(this.centersValuesCheckBox);
            this.panel1.Controls.Add(this.dimensionsCheckBox);
            this.panel1.Controls.Add(this.gridSurfaceCheckBox);
            this.panel1.Location = new System.Drawing.Point(509, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 278);
            this.panel1.TabIndex = 1;
            // 
            // sensorsValuesCheckedBox
            // 
            this.sensorsValuesCheckedBox.AutoSize = true;
            this.sensorsValuesCheckedBox.Checked = true;
            this.sensorsValuesCheckedBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.sensorsValuesCheckedBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.sensorsValuesCheckedBox.Location = new System.Drawing.Point(7, 70);
            this.sensorsValuesCheckedBox.Name = "sensorsValuesCheckedBox";
            this.sensorsValuesCheckedBox.Size = new System.Drawing.Size(128, 17);
            this.sensorsValuesCheckedBox.TabIndex = 15;
            this.sensorsValuesCheckedBox.Text = "Показания датчиков";
            this.sensorsValuesCheckedBox.UseVisualStyleBackColor = true;
            this.sensorsValuesCheckedBox.CheckedChanged += new System.EventHandler(this.sensorsValuesCheckedBox_CheckedChanged);
            // 
            // frontDiameterCheckBox
            // 
            this.frontDiameterCheckBox.AutoSize = true;
            this.frontDiameterCheckBox.Checked = true;
            this.frontDiameterCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.frontDiameterCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.frontDiameterCheckBox.Location = new System.Drawing.Point(7, 183);
            this.frontDiameterCheckBox.Name = "frontDiameterCheckBox";
            this.frontDiameterCheckBox.Size = new System.Drawing.Size(169, 17);
            this.frontDiameterCheckBox.TabIndex = 12;
            this.frontDiameterCheckBox.Text = "Передний торцевой диаметр";
            this.frontDiameterCheckBox.UseVisualStyleBackColor = true;
            this.frontDiameterCheckBox.CheckedChanged += new System.EventHandler(this.frontDiameterCheckBox_CheckedChanged);
            // 
            // lengthCheckBox
            // 
            this.lengthCheckBox.AutoSize = true;
            this.lengthCheckBox.Checked = true;
            this.lengthCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.lengthCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lengthCheckBox.Location = new System.Drawing.Point(7, 161);
            this.lengthCheckBox.Name = "lengthCheckBox";
            this.lengthCheckBox.Size = new System.Drawing.Size(94, 17);
            this.lengthCheckBox.TabIndex = 11;
            this.lengthCheckBox.Text = "Длина слитка";
            this.lengthCheckBox.UseVisualStyleBackColor = true;
            this.lengthCheckBox.CheckedChanged += new System.EventHandler(this.lengthCheckBox_CheckedChanged);
            // 
            // allDimentionsCheckBox
            // 
            this.allDimentionsCheckBox.AutoSize = true;
            this.allDimentionsCheckBox.Checked = true;
            this.allDimentionsCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.allDimentionsCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.allDimentionsCheckBox.Location = new System.Drawing.Point(7, 134);
            this.allDimentionsCheckBox.Name = "allDimentionsCheckBox";
            this.allDimentionsCheckBox.Size = new System.Drawing.Size(101, 17);
            this.allDimentionsCheckBox.TabIndex = 8;
            this.allDimentionsCheckBox.Text = "Все измерения";
            this.allDimentionsCheckBox.UseVisualStyleBackColor = true;
            this.allDimentionsCheckBox.CheckedChanged += new System.EventHandler(this.allDimentionsCheckBox_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 119);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(127, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "____________________";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 79);
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
            this.smoothCheckedBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.smoothCheckedBox.Location = new System.Drawing.Point(7, 101);
            this.smoothCheckedBox.Name = "smoothCheckedBox";
            this.smoothCheckedBox.Size = new System.Drawing.Size(91, 17);
            this.smoothCheckedBox.TabIndex = 3;
            this.smoothCheckedBox.Text = "Сглаживание";
            this.smoothCheckedBox.UseVisualStyleBackColor = true;
            this.smoothCheckedBox.CheckedChanged += new System.EventHandler(this.smoothCheckedBox_CheckedChanged);
            // 
            // centersValuesCheckBox
            // 
            this.centersValuesCheckBox.AutoSize = true;
            this.centersValuesCheckBox.Checked = true;
            this.centersValuesCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.centersValuesCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.centersValuesCheckBox.Location = new System.Drawing.Point(7, 50);
            this.centersValuesCheckBox.Name = "centersValuesCheckBox";
            this.centersValuesCheckBox.Size = new System.Drawing.Size(101, 17);
            this.centersValuesCheckBox.TabIndex = 2;
            this.centersValuesCheckBox.Text = "Центры срезов";
            this.centersValuesCheckBox.UseVisualStyleBackColor = true;
            this.centersValuesCheckBox.CheckedChanged += new System.EventHandler(this.centersValuesCheckBox_CheckedChanged);
            // 
            // dimensionsCheckBox
            // 
            this.dimensionsCheckBox.AutoSize = true;
            this.dimensionsCheckBox.Checked = true;
            this.dimensionsCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.dimensionsCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dimensionsCheckBox.Location = new System.Drawing.Point(7, 30);
            this.dimensionsCheckBox.Name = "dimensionsCheckBox";
            this.dimensionsCheckBox.Size = new System.Drawing.Size(110, 17);
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
            this.gridSurfaceCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gridSurfaceCheckBox.Location = new System.Drawing.Point(7, 10);
            this.gridSurfaceCheckBox.Name = "gridSurfaceCheckBox";
            this.gridSurfaceCheckBox.Size = new System.Drawing.Size(127, 17);
            this.gridSurfaceCheckBox.TabIndex = 0;
            this.gridSurfaceCheckBox.Text = "Координатная сетка";
            this.gridSurfaceCheckBox.UseVisualStyleBackColor = true;
            this.gridSurfaceCheckBox.CheckedChanged += new System.EventHandler(this.gridSurfaceCheckBox_CheckedChanged);
            // 
            // modelView
            // 
            this.modelView.AccumBits = ((byte)(0));
            this.modelView.AutoCheckErrors = false;
            this.modelView.AutoFinish = false;
            this.modelView.AutoMakeCurrent = true;
            this.modelView.AutoSwapBuffers = true;
            this.modelView.BackColor = System.Drawing.Color.Black;
            this.modelView.ColorBits = ((byte)(32));
            this.modelView.DepthBits = ((byte)(16));
            this.modelView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.modelView.Location = new System.Drawing.Point(3, 3);
            this.modelView.Name = "modelView";
            this.modelView.Size = new System.Drawing.Size(709, 474);
            this.modelView.StencilBits = ((byte)(0));
            this.modelView.TabIndex = 0;
            this.modelView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.modelPanel_MouseDown);
            this.modelView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.modelPanel_MouseMove);
            // 
            // backDiameterCheckBox
            // 
            this.backDiameterCheckBox.AutoSize = true;
            this.backDiameterCheckBox.Checked = true;
            this.backDiameterCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.backDiameterCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.backDiameterCheckBox.Location = new System.Drawing.Point(7, 206);
            this.backDiameterCheckBox.Name = "backDiameterCheckBox";
            this.backDiameterCheckBox.Size = new System.Drawing.Size(156, 17);
            this.backDiameterCheckBox.TabIndex = 16;
            this.backDiameterCheckBox.Text = "Задний торцевой диаметр";
            this.backDiameterCheckBox.UseVisualStyleBackColor = true;
            this.backDiameterCheckBox.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // SlabVisualizationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.HighlightText;
            this.ClientSize = new System.Drawing.Size(723, 506);
            this.Controls.Add(this.tabControl1);
            this.Name = "SlabVisualizationForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Данные слитка";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SlabVisualizationForm_FormClosing);
            this.Load += new System.EventHandler(this.SlabVisualizationForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.plotsPage.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.sectionsPage.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.deviationPage.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.slabModelPage.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage plotsPage;
        private ZedGraph.ZedGraphControl plotsView;
        private System.Windows.Forms.TabPage slabModelPage;
        private Tao.Platform.Windows.SimpleOpenGlControl modelView;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox gridSurfaceCheckBox;
        private System.Windows.Forms.CheckBox dimensionsCheckBox;
        private System.Windows.Forms.CheckBox centersValuesCheckBox;
        private System.Windows.Forms.CheckBox smoothCheckedBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox allDimentionsCheckBox;
        private System.Windows.Forms.Label label2;
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
        private System.Windows.Forms.CheckBox frontDiameterCheckBox;
        private System.Windows.Forms.Button rightButton;
        private System.Windows.Forms.Button leftButton;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.CheckBox isShowNodesCheckedBox;
        private System.Windows.Forms.CheckBox isAllPlotShowCheckedBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TabPage sectionsPage;
        private ZedGraph.ZedGraphControl sectionsView;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage deviationPage;
        private ZedGraph.ZedGraphControl deviationView;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox sensorsValuesCheckedBox;
        private System.Windows.Forms.CheckBox backDiameterCheckBox;
    }
}