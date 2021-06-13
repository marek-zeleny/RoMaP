﻿namespace RoadTrafficSimulator.Forms
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
            this.panelMap = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDuration)).BeginInit();
            this.groupBoxAllowedDirections.SuspendLayout();
            this.groupBoxInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelMap
            // 
            panelMap.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(237)))), ((int)(((byte)(242)))));
            panelMap.Cursor = System.Windows.Forms.Cursors.Cross;
            panelMap.Location = new System.Drawing.Point(11, 12);
            panelMap.Name = "panelMap";
            panelMap.Size = new System.Drawing.Size(360, 379);
            panelMap.TabIndex = 0;
            panelMap.Paint += new System.Windows.Forms.PaintEventHandler(this.panelMap_Paint);
            panelMap.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panelMap_MouseClick);
            // 
            // comboBoxSetting
            // 
            this.comboBoxSetting.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSetting.FormattingEnabled = true;
            this.comboBoxSetting.Location = new System.Drawing.Point(378, 12);
            this.comboBoxSetting.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.comboBoxSetting.Name = "comboBoxSetting";
            this.comboBoxSetting.Size = new System.Drawing.Size(189, 28);
            this.comboBoxSetting.TabIndex = 2;
            this.comboBoxSetting.SelectedIndexChanged += new System.EventHandler(this.comboBoxSetting_SelectedIndexChanged);
            // 
            // labelDuration
            // 
            this.labelDuration.AutoSize = true;
            this.labelDuration.Location = new System.Drawing.Point(378, 52);
            this.labelDuration.Name = "labelDuration";
            this.labelDuration.Size = new System.Drawing.Size(67, 20);
            this.labelDuration.TabIndex = 3;
            this.labelDuration.Text = "Duration";
            // 
            // numericUpDownDuration
            // 
            this.numericUpDownDuration.Location = new System.Drawing.Point(447, 49);
            this.numericUpDownDuration.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
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
            this.numericUpDownDuration.Size = new System.Drawing.Size(49, 27);
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
            this.labelS.Location = new System.Drawing.Point(502, 52);
            this.labelS.Name = "labelS";
            this.labelS.Size = new System.Drawing.Size(15, 20);
            this.labelS.TabIndex = 5;
            this.labelS.Text = "s";
            // 
            // buttonNewSetting
            // 
            this.buttonNewSetting.Location = new System.Drawing.Point(378, 205);
            this.buttonNewSetting.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonNewSetting.Name = "buttonNewSetting";
            this.buttonNewSetting.Size = new System.Drawing.Size(91, 35);
            this.buttonNewSetting.TabIndex = 6;
            this.buttonNewSetting.Text = "New Setting";
            this.buttonNewSetting.UseVisualStyleBackColor = true;
            this.buttonNewSetting.Click += new System.EventHandler(this.buttonNewSetting_Click);
            // 
            // buttonFinish
            // 
            this.buttonFinish.Location = new System.Drawing.Point(378, 357);
            this.buttonFinish.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonFinish.Name = "buttonFinish";
            this.buttonFinish.Size = new System.Drawing.Size(190, 35);
            this.buttonFinish.TabIndex = 6;
            this.buttonFinish.Text = "Finish";
            this.buttonFinish.UseVisualStyleBackColor = true;
            this.buttonFinish.Click += new System.EventHandler(this.buttonFinish_Click);
            // 
            // buttonDeleteSetting
            // 
            this.buttonDeleteSetting.Location = new System.Drawing.Point(477, 205);
            this.buttonDeleteSetting.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonDeleteSetting.Name = "buttonDeleteSetting";
            this.buttonDeleteSetting.Size = new System.Drawing.Size(91, 35);
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
            this.groupBoxAllowedDirections.Location = new System.Drawing.Point(377, 84);
            this.groupBoxAllowedDirections.Name = "groupBoxAllowedDirections";
            this.groupBoxAllowedDirections.Size = new System.Drawing.Size(152, 113);
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
            this.checkBoxDown.Location = new System.Drawing.Point(49, 71);
            this.checkBoxDown.Name = "checkBoxDown";
            this.checkBoxDown.Size = new System.Drawing.Size(54, 39);
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
            this.checkBoxUp.Location = new System.Drawing.Point(49, 23);
            this.checkBoxUp.Name = "checkBoxUp";
            this.checkBoxUp.Size = new System.Drawing.Size(54, 39);
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
            this.checkBoxRight.Location = new System.Drawing.Point(103, 23);
            this.checkBoxRight.Name = "checkBoxRight";
            this.checkBoxRight.Size = new System.Drawing.Size(46, 87);
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
            this.checkBoxLeft.Location = new System.Drawing.Point(3, 23);
            this.checkBoxLeft.Name = "checkBoxLeft";
            this.checkBoxLeft.Size = new System.Drawing.Size(46, 87);
            this.checkBoxLeft.TabIndex = 0;
            this.checkBoxLeft.Text = "◄";
            this.checkBoxLeft.UseVisualStyleBackColor = true;
            this.checkBoxLeft.CheckedChanged += new System.EventHandler(this.checkBoxDirection_CheckedChanged);
            // 
            // groupBoxInfo
            // 
            this.groupBoxInfo.Controls.Add(this.labelInfo);
            this.groupBoxInfo.Location = new System.Drawing.Point(378, 248);
            this.groupBoxInfo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBoxInfo.Name = "groupBoxInfo";
            this.groupBoxInfo.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBoxInfo.Size = new System.Drawing.Size(190, 101);
            this.groupBoxInfo.TabIndex = 8;
            this.groupBoxInfo.TabStop = false;
            this.groupBoxInfo.Text = "Info";
            // 
            // labelInfo
            // 
            this.labelInfo.Location = new System.Drawing.Point(7, 25);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(176, 72);
            this.labelInfo.TabIndex = 0;
            // 
            // FormTrafficLight
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(579, 404);
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
            this.Name = "FormTrafficLight";
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