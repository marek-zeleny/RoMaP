namespace RoadTrafficSimulator
{
    partial class FormMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelMap = new System.Windows.Forms.Panel();
            this.labelMode = new System.Windows.Forms.Label();
            this.comboBoxMode = new System.Windows.Forms.ComboBox();
            this.groupBoxBuild = new System.Windows.Forms.GroupBox();
            this.labelMps = new System.Windows.Forms.Label();
            this.labelBuildMaxSpeed = new System.Windows.Forms.Label();
            this.numericUpDownSpeed = new System.Windows.Forms.NumericUpDown();
            this.checkBoxTwoWayRoad = new System.Windows.Forms.CheckBox();
            this.groupBoxCrossroad = new System.Windows.Forms.GroupBox();
            this.buttonTrafficLight = new System.Windows.Forms.Button();
            this.labelOutIndex = new System.Windows.Forms.Label();
            this.labelInIndex = new System.Windows.Forms.Label();
            this.labelCoords = new System.Windows.Forms.Label();
            this.groupBoxRoad = new System.Windows.Forms.GroupBox();
            this.labelRoadMaxSpeed = new System.Windows.Forms.Label();
            this.labelTo = new System.Windows.Forms.Label();
            this.labelFrom = new System.Windows.Forms.Label();
            this.labelRoadId = new System.Windows.Forms.Label();
            this.buttonCenter = new System.Windows.Forms.Button();
            this.buttonZoom = new System.Windows.Forms.Button();
            this.groupBoxBuild.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSpeed)).BeginInit();
            this.groupBoxCrossroad.SuspendLayout();
            this.groupBoxRoad.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelMap
            // 
            this.panelMap.BackColor = System.Drawing.Color.White;
            this.panelMap.Location = new System.Drawing.Point(14, 15);
            this.panelMap.Margin = new System.Windows.Forms.Padding(4);
            this.panelMap.Name = "panelMap";
            this.panelMap.Size = new System.Drawing.Size(1019, 954);
            this.panelMap.TabIndex = 0;
            this.panelMap.Paint += new System.Windows.Forms.PaintEventHandler(this.panelMap_Paint);
            this.panelMap.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panelMap_MouseClick);
            this.panelMap.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.panelMap_MouseDoubleClick);
            this.panelMap.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelMap_MouseDown);
            this.panelMap.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelMap_MouseMove);
            this.panelMap.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panelMap_MouseUp);
            // 
            // labelMode
            // 
            this.labelMode.AutoSize = true;
            this.labelMode.Location = new System.Drawing.Point(1041, 15);
            this.labelMode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelMode.Name = "labelMode";
            this.labelMode.Size = new System.Drawing.Size(59, 25);
            this.labelMode.TabIndex = 1;
            this.labelMode.Text = "Mode";
            // 
            // comboBoxMode
            // 
            this.comboBoxMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMode.FormattingEnabled = true;
            this.comboBoxMode.Location = new System.Drawing.Point(1041, 45);
            this.comboBoxMode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.comboBoxMode.Name = "comboBoxMode";
            this.comboBoxMode.Size = new System.Drawing.Size(266, 33);
            this.comboBoxMode.TabIndex = 2;
            this.comboBoxMode.SelectedIndexChanged += new System.EventHandler(this.comboBoxMode_SelectedIndexChanged);
            // 
            // groupBoxBuild
            // 
            this.groupBoxBuild.Controls.Add(this.labelMps);
            this.groupBoxBuild.Controls.Add(this.labelBuildMaxSpeed);
            this.groupBoxBuild.Controls.Add(this.numericUpDownSpeed);
            this.groupBoxBuild.Controls.Add(this.checkBoxTwoWayRoad);
            this.groupBoxBuild.Location = new System.Drawing.Point(1041, 86);
            this.groupBoxBuild.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxBuild.Name = "groupBoxBuild";
            this.groupBoxBuild.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxBuild.Size = new System.Drawing.Size(266, 130);
            this.groupBoxBuild.TabIndex = 3;
            this.groupBoxBuild.TabStop = false;
            this.groupBoxBuild.Text = "Build Properties";
            this.groupBoxBuild.Visible = false;
            // 
            // labelMps
            // 
            this.labelMps.AutoSize = true;
            this.labelMps.Location = new System.Drawing.Point(188, 81);
            this.labelMps.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelMps.Name = "labelMps";
            this.labelMps.Size = new System.Drawing.Size(47, 25);
            this.labelMps.TabIndex = 3;
            this.labelMps.Text = "mps";
            // 
            // labelBuildMaxSpeed
            // 
            this.labelBuildMaxSpeed.AutoSize = true;
            this.labelBuildMaxSpeed.Location = new System.Drawing.Point(8, 81);
            this.labelBuildMaxSpeed.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelBuildMaxSpeed.Name = "labelBuildMaxSpeed";
            this.labelBuildMaxSpeed.Size = new System.Drawing.Size(98, 25);
            this.labelBuildMaxSpeed.TabIndex = 2;
            this.labelBuildMaxSpeed.Text = "Max speed";
            // 
            // numericUpDownSpeed
            // 
            this.numericUpDownSpeed.Location = new System.Drawing.Point(119, 76);
            this.numericUpDownSpeed.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.numericUpDownSpeed.Maximum = new decimal(new int[] {
            40,
            0,
            0,
            0});
            this.numericUpDownSpeed.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDownSpeed.Name = "numericUpDownSpeed";
            this.numericUpDownSpeed.Size = new System.Drawing.Size(61, 31);
            this.numericUpDownSpeed.TabIndex = 1;
            this.numericUpDownSpeed.Value = new decimal(new int[] {
            14,
            0,
            0,
            0});
            // 
            // checkBoxTwoWayRoad
            // 
            this.checkBoxTwoWayRoad.AutoSize = true;
            this.checkBoxTwoWayRoad.Checked = true;
            this.checkBoxTwoWayRoad.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxTwoWayRoad.Location = new System.Drawing.Point(9, 36);
            this.checkBoxTwoWayRoad.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.checkBoxTwoWayRoad.Name = "checkBoxTwoWayRoad";
            this.checkBoxTwoWayRoad.Size = new System.Drawing.Size(150, 29);
            this.checkBoxTwoWayRoad.TabIndex = 0;
            this.checkBoxTwoWayRoad.Text = "Two-way road";
            this.checkBoxTwoWayRoad.UseVisualStyleBackColor = true;
            // 
            // groupBoxCrossroad
            // 
            this.groupBoxCrossroad.Controls.Add(this.buttonTrafficLight);
            this.groupBoxCrossroad.Controls.Add(this.labelOutIndex);
            this.groupBoxCrossroad.Controls.Add(this.labelInIndex);
            this.groupBoxCrossroad.Controls.Add(this.labelCoords);
            this.groupBoxCrossroad.Location = new System.Drawing.Point(1041, 86);
            this.groupBoxCrossroad.Name = "groupBoxCrossroad";
            this.groupBoxCrossroad.Size = new System.Drawing.Size(266, 153);
            this.groupBoxCrossroad.TabIndex = 4;
            this.groupBoxCrossroad.TabStop = false;
            this.groupBoxCrossroad.Text = "Crossroad Properties";
            this.groupBoxCrossroad.Visible = false;
            // 
            // buttonTrafficLight
            // 
            this.buttonTrafficLight.Location = new System.Drawing.Point(6, 105);
            this.buttonTrafficLight.Name = "buttonTrafficLight";
            this.buttonTrafficLight.Size = new System.Drawing.Size(232, 42);
            this.buttonTrafficLight.TabIndex = 3;
            this.buttonTrafficLight.Text = "Customize Traffic Light";
            this.buttonTrafficLight.UseVisualStyleBackColor = true;
            this.buttonTrafficLight.Click += new System.EventHandler(this.buttonTrafficLight_Click);
            // 
            // labelOutIndex
            // 
            this.labelOutIndex.AutoSize = true;
            this.labelOutIndex.Location = new System.Drawing.Point(6, 77);
            this.labelOutIndex.Name = "labelOutIndex";
            this.labelOutIndex.Size = new System.Drawing.Size(168, 25);
            this.labelOutIndex.TabIndex = 2;
            this.labelOutIndex.Text = "Outcoming roads: -";
            // 
            // labelInIndex
            // 
            this.labelInIndex.AutoSize = true;
            this.labelInIndex.Location = new System.Drawing.Point(6, 52);
            this.labelInIndex.Name = "labelInIndex";
            this.labelInIndex.Size = new System.Drawing.Size(153, 25);
            this.labelInIndex.TabIndex = 1;
            this.labelInIndex.Text = "Incoming roads: -";
            // 
            // labelCoords
            // 
            this.labelCoords.AutoSize = true;
            this.labelCoords.Location = new System.Drawing.Point(6, 27);
            this.labelCoords.Name = "labelCoords";
            this.labelCoords.Size = new System.Drawing.Size(97, 25);
            this.labelCoords.TabIndex = 0;
            this.labelCoords.Text = "Coords: -;-";
            // 
            // groupBoxRoad
            // 
            this.groupBoxRoad.Controls.Add(this.labelRoadMaxSpeed);
            this.groupBoxRoad.Controls.Add(this.labelTo);
            this.groupBoxRoad.Controls.Add(this.labelFrom);
            this.groupBoxRoad.Controls.Add(this.labelRoadId);
            this.groupBoxRoad.Location = new System.Drawing.Point(1041, 86);
            this.groupBoxRoad.Name = "groupBoxRoad";
            this.groupBoxRoad.Size = new System.Drawing.Size(266, 130);
            this.groupBoxRoad.TabIndex = 5;
            this.groupBoxRoad.TabStop = false;
            this.groupBoxRoad.Text = "Road Properties";
            this.groupBoxRoad.Visible = false;
            // 
            // labelRoadMaxSpeed
            // 
            this.labelRoadMaxSpeed.AutoSize = true;
            this.labelRoadMaxSpeed.Location = new System.Drawing.Point(6, 102);
            this.labelRoadMaxSpeed.Name = "labelRoadMaxSpeed";
            this.labelRoadMaxSpeed.Size = new System.Drawing.Size(154, 25);
            this.labelRoadMaxSpeed.TabIndex = 0;
            this.labelRoadMaxSpeed.Text = "Max speed: - mps";
            // 
            // labelTo
            // 
            this.labelTo.AutoSize = true;
            this.labelTo.Location = new System.Drawing.Point(6, 77);
            this.labelTo.Name = "labelTo";
            this.labelTo.Size = new System.Drawing.Size(57, 25);
            this.labelTo.TabIndex = 0;
            this.labelTo.Text = "To: -;-";
            // 
            // labelFrom
            // 
            this.labelFrom.AutoSize = true;
            this.labelFrom.Location = new System.Drawing.Point(6, 52);
            this.labelFrom.Name = "labelFrom";
            this.labelFrom.Size = new System.Drawing.Size(81, 25);
            this.labelFrom.TabIndex = 0;
            this.labelFrom.Text = "From: -;-";
            // 
            // labelRoadId
            // 
            this.labelRoadId.AutoSize = true;
            this.labelRoadId.Location = new System.Drawing.Point(6, 27);
            this.labelRoadId.Name = "labelRoadId";
            this.labelRoadId.Size = new System.Drawing.Size(46, 25);
            this.labelRoadId.TabIndex = 0;
            this.labelRoadId.Text = "ID: -";
            // 
            // buttonCenter
            // 
            this.buttonCenter.Location = new System.Drawing.Point(1041, 927);
            this.buttonCenter.Name = "buttonCenter";
            this.buttonCenter.Size = new System.Drawing.Size(120, 42);
            this.buttonCenter.TabIndex = 3;
            this.buttonCenter.Text = "Center Map";
            this.buttonCenter.UseVisualStyleBackColor = true;
            this.buttonCenter.Click += new System.EventHandler(this.buttonCenter_Click);
            // 
            // buttonZoom
            // 
            this.buttonZoom.Location = new System.Drawing.Point(1167, 927);
            this.buttonZoom.Name = "buttonZoom";
            this.buttonZoom.Size = new System.Drawing.Size(120, 42);
            this.buttonZoom.TabIndex = 3;
            this.buttonZoom.Text = "Zoom: 1.0x";
            this.buttonZoom.UseVisualStyleBackColor = true;
            this.buttonZoom.Click += new System.EventHandler(this.buttonZoom_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1326, 1032);
            this.Controls.Add(this.groupBoxRoad);
            this.Controls.Add(this.buttonZoom);
            this.Controls.Add(this.buttonCenter);
            this.Controls.Add(this.groupBoxCrossroad);
            this.Controls.Add(this.comboBoxMode);
            this.Controls.Add(this.labelMode);
            this.Controls.Add(this.groupBoxBuild);
            this.Controls.Add(this.panelMap);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormMain";
            this.Text = "RoadTrafficSimulator";
            this.groupBoxBuild.ResumeLayout(false);
            this.groupBoxBuild.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSpeed)).EndInit();
            this.groupBoxCrossroad.ResumeLayout(false);
            this.groupBoxCrossroad.PerformLayout();
            this.groupBoxRoad.ResumeLayout(false);
            this.groupBoxRoad.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelMap;
        private System.Windows.Forms.Label labelMode;
        private System.Windows.Forms.ComboBox comboBoxMode;
        private System.Windows.Forms.GroupBox groupBoxBuild;
        private System.Windows.Forms.CheckBox checkBoxTwoWayRoad;
        private System.Windows.Forms.Label labelMps;
        private System.Windows.Forms.Label labelBuildMaxSpeed;
        private System.Windows.Forms.NumericUpDown numericUpDownSpeed;
        private System.Windows.Forms.GroupBox groupBoxCrossroad;
        private System.Windows.Forms.Button buttonTrafficLight;
        private System.Windows.Forms.Label labelOutIndex;
        private System.Windows.Forms.Label labelInIndex;
        private System.Windows.Forms.Label labelCoords;
        private System.Windows.Forms.Button buttonCenter;
        private System.Windows.Forms.Button buttonZoom;
        private System.Windows.Forms.GroupBox groupBoxRoad;
        private System.Windows.Forms.Label labelRoadMaxSpeed;
        private System.Windows.Forms.Label labelTo;
        private System.Windows.Forms.Label labelFrom;
        private System.Windows.Forms.Label labelRoadId;
    }
}

