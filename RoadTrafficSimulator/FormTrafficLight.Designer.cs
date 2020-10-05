namespace RoadTrafficSimulator
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
            panelMap = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDuration)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBoxSetting
            // 
            this.comboBoxSetting.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSetting.FormattingEnabled = true;
            this.comboBoxSetting.Location = new System.Drawing.Point(338, 12);
            this.comboBoxSetting.Name = "comboBoxSetting";
            this.comboBoxSetting.Size = new System.Drawing.Size(184, 23);
            this.comboBoxSetting.TabIndex = 2;
            this.comboBoxSetting.SelectedIndexChanged += new System.EventHandler(this.comboBoxSetting_SelectedIndexChanged);
            // 
            // labelDuration
            // 
            this.labelDuration.AutoSize = true;
            this.labelDuration.Location = new System.Drawing.Point(338, 43);
            this.labelDuration.Name = "labelDuration";
            this.labelDuration.Size = new System.Drawing.Size(53, 15);
            this.labelDuration.TabIndex = 3;
            this.labelDuration.Text = "Duration";
            // 
            // numericUpDownDuration
            // 
            this.numericUpDownDuration.Location = new System.Drawing.Point(397, 41);
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
            this.numericUpDownDuration.Size = new System.Drawing.Size(43, 23);
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
            this.labelS.Location = new System.Drawing.Point(446, 43);
            this.labelS.Name = "labelS";
            this.labelS.Size = new System.Drawing.Size(12, 15);
            this.labelS.TabIndex = 5;
            this.labelS.Text = "s";
            // 
            // buttonNewSetting
            // 
            this.buttonNewSetting.Location = new System.Drawing.Point(338, 70);
            this.buttonNewSetting.Name = "buttonNewSetting";
            this.buttonNewSetting.Size = new System.Drawing.Size(163, 26);
            this.buttonNewSetting.TabIndex = 6;
            this.buttonNewSetting.Text = "New Setting";
            this.buttonNewSetting.UseVisualStyleBackColor = true;
            this.buttonNewSetting.Click += new System.EventHandler(this.buttonNewSetting_Click);
            // 
            // buttonFinish
            // 
            this.buttonFinish.Location = new System.Drawing.Point(338, 300);
            this.buttonFinish.Name = "buttonFinish";
            this.buttonFinish.Size = new System.Drawing.Size(163, 26);
            this.buttonFinish.TabIndex = 6;
            this.buttonFinish.Text = "Finish";
            this.buttonFinish.UseVisualStyleBackColor = true;
            this.buttonFinish.Click += new System.EventHandler(this.buttonFinish_Click);
            // 
            // panelMap
            // 
            panelMap.BackColor = System.Drawing.Color.White;
            panelMap.Location = new System.Drawing.Point(12, 11);
            panelMap.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            panelMap.Name = "panelMap";
            panelMap.Size = new System.Drawing.Size(320, 320);
            panelMap.TabIndex = 0;
            panelMap.Paint += new System.Windows.Forms.PaintEventHandler(this.panelMap_Paint);
            panelMap.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panelMap_MouseClick);
            // 
            // buttonDeleteSetting
            // 
            this.buttonDeleteSetting.Location = new System.Drawing.Point(338, 102);
            this.buttonDeleteSetting.Name = "buttonDeleteSetting";
            this.buttonDeleteSetting.Size = new System.Drawing.Size(163, 26);
            this.buttonDeleteSetting.TabIndex = 6;
            this.buttonDeleteSetting.Text = "Delete Setting";
            this.buttonDeleteSetting.UseVisualStyleBackColor = true;
            this.buttonDeleteSetting.Click += new System.EventHandler(this.buttonDeleteSetting_Click);
            // 
            // FormTrafficLight
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 338);
            this.Controls.Add(this.buttonDeleteSetting);
            this.Controls.Add(this.buttonFinish);
            this.Controls.Add(this.buttonNewSetting);
            this.Controls.Add(this.labelS);
            this.Controls.Add(this.numericUpDownDuration);
            this.Controls.Add(this.labelDuration);
            this.Controls.Add(this.comboBoxSetting);
            this.Controls.Add(panelMap);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FormTrafficLight";
            this.Text = "Road Traffic Simulator - Traffic Light Settings";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDuration)).EndInit();
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
    }
}