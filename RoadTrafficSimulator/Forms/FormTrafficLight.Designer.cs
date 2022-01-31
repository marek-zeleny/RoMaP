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
            System.Windows.Forms.Panel panelMap;
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
            this.groupBoxInfo = new System.Windows.Forms.GroupBox();
            this.labelInfo = new System.Windows.Forms.Label();
            panelMap = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDuration)).BeginInit();
            this.groupBoxAllowedDirections.SuspendLayout();
            this.groupBoxInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBoxSetting
            // 
            this.comboBoxSetting.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSetting.FormattingEnabled = true;
            this.comboBoxSetting.Location = new System.Drawing.Point(472, 15);
            this.comboBoxSetting.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.comboBoxSetting.Name = "comboBoxSetting";
            this.comboBoxSetting.Size = new System.Drawing.Size(235, 33);
            this.comboBoxSetting.TabIndex = 2;
            this.comboBoxSetting.SelectedIndexChanged += new System.EventHandler(this.comboBoxSetting_SelectedIndexChanged);
            // 
            // labelDuration
            // 
            this.labelDuration.AutoSize = true;
            this.labelDuration.Location = new System.Drawing.Point(478, 73);
            this.labelDuration.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelDuration.Name = "labelDuration";
            this.labelDuration.Size = new System.Drawing.Size(81, 25);
            this.labelDuration.TabIndex = 3;
            this.labelDuration.Text = "Duration";
            // 
            // numericUpDownDuration
            // 
            this.numericUpDownDuration.Location = new System.Drawing.Point(559, 61);
            this.numericUpDownDuration.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.numericUpDownDuration.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numericUpDownDuration.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownDuration.Name = "numericUpDownDuration";
            this.numericUpDownDuration.Size = new System.Drawing.Size(61, 31);
            this.numericUpDownDuration.TabIndex = 4;
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
            this.labelS.Location = new System.Drawing.Point(634, 73);
            this.labelS.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelS.Name = "labelS";
            this.labelS.Size = new System.Drawing.Size(20, 25);
            this.labelS.TabIndex = 5;
            this.labelS.Text = "s";
            // 
            // buttonNewSetting
            // 
            this.buttonNewSetting.Location = new System.Drawing.Point(472, 256);
            this.buttonNewSetting.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonNewSetting.Name = "buttonNewSetting";
            this.buttonNewSetting.Size = new System.Drawing.Size(114, 44);
            this.buttonNewSetting.TabIndex = 6;
            this.buttonNewSetting.Text = "New Setting";
            this.buttonNewSetting.UseVisualStyleBackColor = true;
            this.buttonNewSetting.Click += new System.EventHandler(this.buttonNewSetting_Click);
            // 
            // buttonFinish
            // 
            this.buttonFinish.Location = new System.Drawing.Point(472, 446);
            this.buttonFinish.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonFinish.Name = "buttonFinish";
            this.buttonFinish.Size = new System.Drawing.Size(238, 44);
            this.buttonFinish.TabIndex = 6;
            this.buttonFinish.Text = "Finish";
            this.buttonFinish.UseVisualStyleBackColor = true;
            this.buttonFinish.Click += new System.EventHandler(this.buttonFinish_Click);
            // 
            // buttonDeleteSetting
            // 
            this.buttonDeleteSetting.Location = new System.Drawing.Point(596, 256);
            this.buttonDeleteSetting.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonDeleteSetting.Name = "buttonDeleteSetting";
            this.buttonDeleteSetting.Size = new System.Drawing.Size(114, 44);
            this.buttonDeleteSetting.TabIndex = 6;
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
            this.groupBoxAllowedDirections.Location = new System.Drawing.Point(477, 113);
            this.groupBoxAllowedDirections.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBoxAllowedDirections.Name = "groupBoxAllowedDirections";
            this.groupBoxAllowedDirections.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBoxAllowedDirections.Size = new System.Drawing.Size(190, 141);
            this.groupBoxAllowedDirections.TabIndex = 7;
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
            this.checkBoxDown.Location = new System.Drawing.Point(59, 90);
            this.checkBoxDown.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkBoxDown.Name = "checkBoxDown";
            this.checkBoxDown.Size = new System.Drawing.Size(72, 47);
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
            this.checkBoxUp.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkBoxUp.Name = "checkBoxUp";
            this.checkBoxUp.Size = new System.Drawing.Size(72, 47);
            this.checkBoxUp.TabIndex = 2;
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
            this.checkBoxRight.Location = new System.Drawing.Point(131, 28);
            this.checkBoxRight.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkBoxRight.Name = "checkBoxRight";
            this.checkBoxRight.Size = new System.Drawing.Size(55, 109);
            this.checkBoxRight.TabIndex = 1;
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
            this.checkBoxLeft.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkBoxLeft.Name = "checkBoxLeft";
            this.checkBoxLeft.Size = new System.Drawing.Size(55, 109);
            this.checkBoxLeft.TabIndex = 0;
            this.checkBoxLeft.Text = "◄";
            this.checkBoxLeft.UseVisualStyleBackColor = true;
            this.checkBoxLeft.CheckedChanged += new System.EventHandler(this.checkBoxDirection_CheckedChanged);
            // 
            // groupBoxInfo
            // 
            this.groupBoxInfo.Controls.Add(this.labelInfo);
            this.groupBoxInfo.Location = new System.Drawing.Point(472, 310);
            this.groupBoxInfo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxInfo.Name = "groupBoxInfo";
            this.groupBoxInfo.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxInfo.Size = new System.Drawing.Size(238, 126);
            this.groupBoxInfo.TabIndex = 8;
            this.groupBoxInfo.TabStop = false;
            this.groupBoxInfo.Text = "Info";
            // 
            // labelInfo
            // 
            this.labelInfo.Location = new System.Drawing.Point(9, 31);
            this.labelInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(220, 90);
            this.labelInfo.TabIndex = 0;
            // 
            // panelMap
            // 
            panelMap.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(237)))), ((int)(((byte)(242)))));
            panelMap.Cursor = System.Windows.Forms.Cursors.Cross;
            panelMap.Location = new System.Drawing.Point(14, 15);
            panelMap.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            panelMap.Name = "panelMap";
            panelMap.Size = new System.Drawing.Size(450, 474);
            panelMap.TabIndex = 0;
            panelMap.Paint += new System.Windows.Forms.PaintEventHandler(this.panelMap_Paint);
            panelMap.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panelMap_MouseClick);
            // 
            // FormTrafficLight
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(724, 505);
            this.Controls.Add(this.groupBoxInfo);
            this.Controls.Add(this.groupBoxAllowedDirections);
            this.Controls.Add(this.buttonDeleteSetting);
            this.Controls.Add(this.buttonFinish);
            this.Controls.Add(this.labelS);
            this.Controls.Add(this.numericUpDownDuration);
            this.Controls.Add(this.buttonNewSetting);
            this.Controls.Add(this.labelDuration);
            this.Controls.Add(this.comboBoxSetting);
            this.Controls.Add(panelMap);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FormTrafficLight";
            this.Padding = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Road Traffic Simulator - Traffic Light Settings";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormTrafficLight_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDuration)).EndInit();
            this.groupBoxAllowedDirections.ResumeLayout(false);
            this.groupBoxAllowedDirections.PerformLayout();
            this.groupBoxInfo.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

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
        private System.Windows.Forms.GroupBox groupBoxInfo;
        private System.Windows.Forms.Label labelInfo;
    }
}