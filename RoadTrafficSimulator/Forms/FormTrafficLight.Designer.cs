namespace RoadTrafficSimulator.Forms
{
    partial class FormTrafficLight
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
            if (disposing && (components != null))
            {
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
            this.panelMap = new System.Windows.Forms.Panel();
            this.comboBoxSetting = new System.Windows.Forms.ComboBox();
            this.labelDuration = new System.Windows.Forms.Label();
            this.numericUpDownDuration = new System.Windows.Forms.NumericUpDown();
            this.labelS = new System.Windows.Forms.Label();
            this.buttonNewSetting = new System.Windows.Forms.Button();
            this.buttonFinish = new System.Windows.Forms.Button();
            this.buttonDeleteSetting = new System.Windows.Forms.Button();
            this.groupBoxAllowedDirections = new System.Windows.Forms.GroupBox();
            this.checkBoxDown = new System.Windows.Forms.CheckBox();
            this.checkBoxUp = new System.Windows.Forms.CheckBox();
            this.checkBoxRight = new System.Windows.Forms.CheckBox();
            this.checkBoxLeft = new System.Windows.Forms.CheckBox();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanelButtons = new System.Windows.Forms.TableLayoutPanel();
            this.groupBoxMainRoad = new System.Windows.Forms.GroupBox();
            this.imageComboBoxMainRoad = new RoadTrafficSimulator.Forms.ImageComboBox();
            this.flowLayoutPanelDuration = new System.Windows.Forms.FlowLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDuration)).BeginInit();
            this.groupBoxAllowedDirections.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.tableLayoutPanelButtons.SuspendLayout();
            this.groupBoxMainRoad.SuspendLayout();
            this.flowLayoutPanelDuration.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelMap
            // 
            this.panelMap.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(237)))), ((int)(((byte)(242)))));
            this.panelMap.Cursor = System.Windows.Forms.Cursors.Cross;
            this.panelMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMap.Location = new System.Drawing.Point(0, 0);
            this.panelMap.Margin = new System.Windows.Forms.Padding(4);
            this.panelMap.Name = "panelMap";
            this.panelMap.Size = new System.Drawing.Size(446, 446);
            this.panelMap.TabIndex = 0;
            this.panelMap.Paint += new System.Windows.Forms.PaintEventHandler(this.panelMap_Paint);
            this.panelMap.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panelMap_MouseClick);
            // 
            // comboBoxSetting
            // 
            this.comboBoxSetting.Dock = System.Windows.Forms.DockStyle.Top;
            this.comboBoxSetting.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSetting.FormattingEnabled = true;
            this.comboBoxSetting.Location = new System.Drawing.Point(6, 8);
            this.comboBoxSetting.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.comboBoxSetting.Name = "comboBoxSetting";
            this.comboBoxSetting.Size = new System.Drawing.Size(242, 33);
            this.comboBoxSetting.TabIndex = 1;
            this.comboBoxSetting.SelectedIndexChanged += new System.EventHandler(this.comboBoxSetting_SelectedIndexChanged);
            // 
            // labelDuration
            // 
            this.labelDuration.AutoSize = true;
            this.labelDuration.Location = new System.Drawing.Point(5, 1);
            this.labelDuration.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelDuration.Name = "labelDuration";
            this.labelDuration.Size = new System.Drawing.Size(81, 25);
            this.labelDuration.TabIndex = 2;
            this.labelDuration.Text = "Duration";
            // 
            // numericUpDownDuration
            // 
            this.numericUpDownDuration.Location = new System.Drawing.Point(94, 6);
            this.numericUpDownDuration.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.numericUpDownDuration.Maximum = new decimal(new int[] {
            120,
            0,
            0,
            0});
            this.numericUpDownDuration.Name = "numericUpDownDuration";
            this.numericUpDownDuration.Size = new System.Drawing.Size(61, 31);
            this.numericUpDownDuration.TabIndex = 3;
            this.numericUpDownDuration.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownDuration.ValueChanged += new System.EventHandler(this.numericUpDownDuration_ValueChanged);
            // 
            // labelS
            // 
            this.labelS.AutoSize = true;
            this.labelS.Location = new System.Drawing.Point(163, 1);
            this.labelS.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelS.Name = "labelS";
            this.labelS.Size = new System.Drawing.Size(20, 25);
            this.labelS.TabIndex = 4;
            this.labelS.Text = "s";
            // 
            // buttonNewSetting
            // 
            this.buttonNewSetting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonNewSetting.Location = new System.Drawing.Point(4, 5);
            this.buttonNewSetting.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonNewSetting.Name = "buttonNewSetting";
            this.buttonNewSetting.Size = new System.Drawing.Size(113, 36);
            this.buttonNewSetting.TabIndex = 6;
            this.buttonNewSetting.Text = "New Setting";
            this.buttonNewSetting.UseVisualStyleBackColor = true;
            this.buttonNewSetting.Click += new System.EventHandler(this.buttonNewSetting_Click);
            // 
            // buttonFinish
            // 
            this.buttonFinish.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonFinish.Location = new System.Drawing.Point(6, 394);
            this.buttonFinish.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonFinish.Name = "buttonFinish";
            this.buttonFinish.Size = new System.Drawing.Size(242, 44);
            this.buttonFinish.TabIndex = 9;
            this.buttonFinish.Text = "Finish";
            this.buttonFinish.UseVisualStyleBackColor = true;
            this.buttonFinish.Click += new System.EventHandler(this.buttonFinish_Click);
            // 
            // buttonDeleteSetting
            // 
            this.buttonDeleteSetting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonDeleteSetting.Location = new System.Drawing.Point(125, 5);
            this.buttonDeleteSetting.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonDeleteSetting.Name = "buttonDeleteSetting";
            this.buttonDeleteSetting.Size = new System.Drawing.Size(113, 36);
            this.buttonDeleteSetting.TabIndex = 7;
            this.buttonDeleteSetting.Text = "Delete Setting";
            this.buttonDeleteSetting.UseVisualStyleBackColor = true;
            this.buttonDeleteSetting.Click += new System.EventHandler(this.buttonDeleteSetting_Click);
            // 
            // groupBoxAllowedDirections
            // 
            this.groupBoxAllowedDirections.AutoSize = true;
            this.groupBoxAllowedDirections.Controls.Add(this.checkBoxDown);
            this.groupBoxAllowedDirections.Controls.Add(this.checkBoxUp);
            this.groupBoxAllowedDirections.Controls.Add(this.checkBoxRight);
            this.groupBoxAllowedDirections.Controls.Add(this.checkBoxLeft);
            this.groupBoxAllowedDirections.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxAllowedDirections.Location = new System.Drawing.Point(6, 84);
            this.groupBoxAllowedDirections.Margin = new System.Windows.Forms.Padding(4);
            this.groupBoxAllowedDirections.Name = "groupBoxAllowedDirections";
            this.groupBoxAllowedDirections.Padding = new System.Windows.Forms.Padding(4);
            this.groupBoxAllowedDirections.Size = new System.Drawing.Size(242, 126);
            this.groupBoxAllowedDirections.TabIndex = 5;
            this.groupBoxAllowedDirections.TabStop = false;
            this.groupBoxAllowedDirections.Text = "Allowed Directions";
            // 
            // checkBoxDown
            // 
            this.checkBoxDown.AutoSize = true;
            this.checkBoxDown.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.checkBoxDown.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.checkBoxDown.Enabled = false;
            this.checkBoxDown.Font = new System.Drawing.Font("Arial", 9.163636F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.checkBoxDown.Location = new System.Drawing.Point(59, 75);
            this.checkBoxDown.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxDown.Name = "checkBoxDown";
            this.checkBoxDown.Size = new System.Drawing.Size(124, 47);
            this.checkBoxDown.TabIndex = 3;
            this.checkBoxDown.Text = "▼";
            this.checkBoxDown.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.checkBoxDown.UseVisualStyleBackColor = true;
            this.checkBoxDown.CheckedChanged += new System.EventHandler(this.checkBoxDirection_CheckedChanged);
            // 
            // checkBoxUp
            // 
            this.checkBoxUp.AutoSize = true;
            this.checkBoxUp.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.checkBoxUp.Dock = System.Windows.Forms.DockStyle.Top;
            this.checkBoxUp.Enabled = false;
            this.checkBoxUp.Font = new System.Drawing.Font("Arial", 9.163636F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.checkBoxUp.Location = new System.Drawing.Point(59, 28);
            this.checkBoxUp.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxUp.Name = "checkBoxUp";
            this.checkBoxUp.Size = new System.Drawing.Size(124, 47);
            this.checkBoxUp.TabIndex = 1;
            this.checkBoxUp.Text = "▲";
            this.checkBoxUp.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.checkBoxUp.UseVisualStyleBackColor = true;
            this.checkBoxUp.CheckedChanged += new System.EventHandler(this.checkBoxDirection_CheckedChanged);
            // 
            // checkBoxRight
            // 
            this.checkBoxRight.AutoSize = true;
            this.checkBoxRight.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.checkBoxRight.Enabled = false;
            this.checkBoxRight.Font = new System.Drawing.Font("Arial", 9.163636F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.checkBoxRight.Location = new System.Drawing.Point(183, 28);
            this.checkBoxRight.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxRight.Name = "checkBoxRight";
            this.checkBoxRight.Size = new System.Drawing.Size(55, 94);
            this.checkBoxRight.TabIndex = 2;
            this.checkBoxRight.Text = "►";
            this.checkBoxRight.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxRight.UseVisualStyleBackColor = true;
            this.checkBoxRight.CheckedChanged += new System.EventHandler(this.checkBoxDirection_CheckedChanged);
            // 
            // checkBoxLeft
            // 
            this.checkBoxLeft.AutoSize = true;
            this.checkBoxLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.checkBoxLeft.Enabled = false;
            this.checkBoxLeft.Font = new System.Drawing.Font("Arial", 9.163636F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.checkBoxLeft.Location = new System.Drawing.Point(4, 28);
            this.checkBoxLeft.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxLeft.Name = "checkBoxLeft";
            this.checkBoxLeft.Size = new System.Drawing.Size(55, 94);
            this.checkBoxLeft.TabIndex = 0;
            this.checkBoxLeft.Text = "◄";
            this.checkBoxLeft.UseVisualStyleBackColor = true;
            this.checkBoxLeft.CheckedChanged += new System.EventHandler(this.checkBoxDirection_CheckedChanged);
            // 
            // splitContainer
            // 
            this.splitContainer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer.Cursor = System.Windows.Forms.Cursors.VSplit;
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer.IsSplitterFixed = true;
            this.splitContainer.Location = new System.Drawing.Point(6, 8);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.panelMap);
            this.splitContainer.Panel1.Cursor = System.Windows.Forms.Cursors.Default;
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.tableLayoutPanelButtons);
            this.splitContainer.Panel2.Controls.Add(this.groupBoxMainRoad);
            this.splitContainer.Panel2.Controls.Add(this.groupBoxAllowedDirections);
            this.splitContainer.Panel2.Controls.Add(this.buttonFinish);
            this.splitContainer.Panel2.Controls.Add(this.flowLayoutPanelDuration);
            this.splitContainer.Panel2.Controls.Add(this.comboBoxSetting);
            this.splitContainer.Panel2.Cursor = System.Windows.Forms.Cursors.Default;
            this.splitContainer.Panel2.Padding = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.splitContainer.Size = new System.Drawing.Size(712, 450);
            this.splitContainer.SplitterDistance = 450;
            this.splitContainer.TabIndex = 10;
            // 
            // tableLayoutPanelButtons
            // 
            this.tableLayoutPanelButtons.ColumnCount = 2;
            this.tableLayoutPanelButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelButtons.Controls.Add(this.buttonNewSetting, 0, 0);
            this.tableLayoutPanelButtons.Controls.Add(this.buttonDeleteSetting, 1, 0);
            this.tableLayoutPanelButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanelButtons.Location = new System.Drawing.Point(6, 273);
            this.tableLayoutPanelButtons.Name = "tableLayoutPanelButtons";
            this.tableLayoutPanelButtons.RowCount = 1;
            this.tableLayoutPanelButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelButtons.Size = new System.Drawing.Size(242, 46);
            this.tableLayoutPanelButtons.TabIndex = 11;
            // 
            // groupBoxMainRoad
            // 
            this.groupBoxMainRoad.AutoSize = true;
            this.groupBoxMainRoad.Controls.Add(this.imageComboBoxMainRoad);
            this.groupBoxMainRoad.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxMainRoad.Location = new System.Drawing.Point(6, 210);
            this.groupBoxMainRoad.Name = "groupBoxMainRoad";
            this.groupBoxMainRoad.Size = new System.Drawing.Size(242, 63);
            this.groupBoxMainRoad.TabIndex = 12;
            this.groupBoxMainRoad.TabStop = false;
            this.groupBoxMainRoad.Text = "Main Road";
            // 
            // imageComboBoxMainRoad
            // 
            this.imageComboBoxMainRoad.Dock = System.Windows.Forms.DockStyle.Top;
            this.imageComboBoxMainRoad.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.imageComboBoxMainRoad.FormattingEnabled = true;
            this.imageComboBoxMainRoad.Location = new System.Drawing.Point(3, 27);
            this.imageComboBoxMainRoad.Name = "imageComboBoxMainRoad";
            this.imageComboBoxMainRoad.Size = new System.Drawing.Size(236, 33);
            this.imageComboBoxMainRoad.TabIndex = 0;
            this.imageComboBoxMainRoad.SelectedIndexChanged += new System.EventHandler(this.imageComboBoxMainRoad_SelectedIndexChanged);
            // 
            // flowLayoutPanelDuration
            // 
            this.flowLayoutPanelDuration.AutoSize = true;
            this.flowLayoutPanelDuration.Controls.Add(this.labelDuration);
            this.flowLayoutPanelDuration.Controls.Add(this.numericUpDownDuration);
            this.flowLayoutPanelDuration.Controls.Add(this.labelS);
            this.flowLayoutPanelDuration.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanelDuration.Location = new System.Drawing.Point(6, 41);
            this.flowLayoutPanelDuration.Name = "flowLayoutPanelDuration";
            this.flowLayoutPanelDuration.Padding = new System.Windows.Forms.Padding(1);
            this.flowLayoutPanelDuration.Size = new System.Drawing.Size(242, 43);
            this.flowLayoutPanelDuration.TabIndex = 10;
            // 
            // FormTrafficLight
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(724, 466);
            this.Controls.Add(this.splitContainer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormTrafficLight";
            this.Padding = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Road Traffic Simulator - Traffic Light Settings";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormTrafficLight_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDuration)).EndInit();
            this.groupBoxAllowedDirections.ResumeLayout(false);
            this.groupBoxAllowedDirections.PerformLayout();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.tableLayoutPanelButtons.ResumeLayout(false);
            this.groupBoxMainRoad.ResumeLayout(false);
            this.flowLayoutPanelDuration.ResumeLayout(false);
            this.flowLayoutPanelDuration.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelMap;
        private System.Windows.Forms.ComboBox comboBoxSetting;
        private System.Windows.Forms.Label labelDuration;
        private System.Windows.Forms.NumericUpDown numericUpDownDuration;
        private System.Windows.Forms.Label labelS;
        private System.Windows.Forms.Button buttonNewSetting;
        private System.Windows.Forms.Button buttonFinish;
        private System.Windows.Forms.Button buttonDeleteSetting;
        private System.Windows.Forms.GroupBox groupBoxAllowedDirections;
        private System.Windows.Forms.CheckBox checkBoxDown;
        private System.Windows.Forms.CheckBox checkBoxUp;
        private System.Windows.Forms.CheckBox checkBoxRight;
        private System.Windows.Forms.CheckBox checkBoxLeft;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelDuration;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelButtons;
        private System.Windows.Forms.GroupBox groupBoxMainRoad;
        private ImageComboBox imageComboBoxMainRoad;
    }
}