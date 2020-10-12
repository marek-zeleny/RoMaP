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
            this.buttonDestroyCrossroad = new System.Windows.Forms.Button();
            this.buttonTrafficLight = new System.Windows.Forms.Button();
            this.labelOutIndex = new System.Windows.Forms.Label();
            this.labelInIndex = new System.Windows.Forms.Label();
            this.labelCoords = new System.Windows.Forms.Label();
            this.groupBoxRoad = new System.Windows.Forms.GroupBox();
            this.buttonDestroyRoad = new System.Windows.Forms.Button();
            this.labelRoadMaxSpeed = new System.Windows.Forms.Label();
            this.labelTo = new System.Windows.Forms.Label();
            this.labelFrom = new System.Windows.Forms.Label();
            this.labelRoadId = new System.Windows.Forms.Label();
            this.buttonCenter = new System.Windows.Forms.Button();
            this.buttonZoom = new System.Windows.Forms.Button();
            this.groupBoxSimulation = new System.Windows.Forms.GroupBox();
            this.buttonStartSimulation = new System.Windows.Forms.Button();
            this.labelCarFrequency = new System.Windows.Forms.Label();
            this.trackBarCarFrequency = new System.Windows.Forms.TrackBar();
            this.labelDuration = new System.Windows.Forms.Label();
            this.trackBarDuration = new System.Windows.Forms.TrackBar();
            this.groupBoxStatistics = new System.Windows.Forms.GroupBox();
            this.labelCars = new System.Windows.Forms.Label();
            this.labelAvgDistance = new System.Windows.Forms.Label();
            this.labelAvgDelay = new System.Windows.Forms.Label();
            this.groupBoxBuild.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSpeed)).BeginInit();
            this.groupBoxCrossroad.SuspendLayout();
            this.groupBoxRoad.SuspendLayout();
            this.groupBoxSimulation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarCarFrequency)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarDuration)).BeginInit();
            this.groupBoxStatistics.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelMap
            // 
            this.panelMap.BackColor = System.Drawing.Color.White;
            this.panelMap.Location = new System.Drawing.Point(11, 11);
            this.panelMap.Name = "panelMap";
            this.panelMap.Size = new System.Drawing.Size(815, 725);
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
            this.labelMode.Location = new System.Drawing.Point(833, 11);
            this.labelMode.Name = "labelMode";
            this.labelMode.Size = new System.Drawing.Size(45, 19);
            this.labelMode.TabIndex = 1;
            this.labelMode.Text = "Mode";
            // 
            // comboBoxMode
            // 
            this.comboBoxMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMode.FormattingEnabled = true;
            this.comboBoxMode.Location = new System.Drawing.Point(833, 34);
            this.comboBoxMode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.comboBoxMode.Name = "comboBoxMode";
            this.comboBoxMode.Size = new System.Drawing.Size(214, 27);
            this.comboBoxMode.TabIndex = 2;
            this.comboBoxMode.SelectedIndexChanged += new System.EventHandler(this.comboBoxMode_SelectedIndexChanged);
            // 
            // groupBoxBuild
            // 
            this.groupBoxBuild.Controls.Add(this.labelMps);
            this.groupBoxBuild.Controls.Add(this.labelBuildMaxSpeed);
            this.groupBoxBuild.Controls.Add(this.numericUpDownSpeed);
            this.groupBoxBuild.Controls.Add(this.checkBoxTwoWayRoad);
            this.groupBoxBuild.Location = new System.Drawing.Point(833, 66);
            this.groupBoxBuild.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBoxBuild.Name = "groupBoxBuild";
            this.groupBoxBuild.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBoxBuild.Size = new System.Drawing.Size(213, 99);
            this.groupBoxBuild.TabIndex = 3;
            this.groupBoxBuild.TabStop = false;
            this.groupBoxBuild.Text = "Build Properties";
            this.groupBoxBuild.Visible = false;
            // 
            // labelMps
            // 
            this.labelMps.AutoSize = true;
            this.labelMps.Location = new System.Drawing.Point(150, 62);
            this.labelMps.Name = "labelMps";
            this.labelMps.Size = new System.Drawing.Size(35, 19);
            this.labelMps.TabIndex = 3;
            this.labelMps.Text = "mps";
            // 
            // labelBuildMaxSpeed
            // 
            this.labelBuildMaxSpeed.AutoSize = true;
            this.labelBuildMaxSpeed.Location = new System.Drawing.Point(6, 62);
            this.labelBuildMaxSpeed.Name = "labelBuildMaxSpeed";
            this.labelBuildMaxSpeed.Size = new System.Drawing.Size(75, 19);
            this.labelBuildMaxSpeed.TabIndex = 2;
            this.labelBuildMaxSpeed.Text = "Max speed";
            // 
            // numericUpDownSpeed
            // 
            this.numericUpDownSpeed.Location = new System.Drawing.Point(95, 58);
            this.numericUpDownSpeed.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
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
            this.numericUpDownSpeed.Size = new System.Drawing.Size(49, 26);
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
            this.checkBoxTwoWayRoad.Location = new System.Drawing.Point(7, 28);
            this.checkBoxTwoWayRoad.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.checkBoxTwoWayRoad.Name = "checkBoxTwoWayRoad";
            this.checkBoxTwoWayRoad.Size = new System.Drawing.Size(114, 23);
            this.checkBoxTwoWayRoad.TabIndex = 0;
            this.checkBoxTwoWayRoad.Text = "Two-way road";
            this.checkBoxTwoWayRoad.UseVisualStyleBackColor = true;
            // 
            // groupBoxCrossroad
            // 
            this.groupBoxCrossroad.Controls.Add(this.buttonDestroyCrossroad);
            this.groupBoxCrossroad.Controls.Add(this.buttonTrafficLight);
            this.groupBoxCrossroad.Controls.Add(this.labelOutIndex);
            this.groupBoxCrossroad.Controls.Add(this.labelInIndex);
            this.groupBoxCrossroad.Controls.Add(this.labelCoords);
            this.groupBoxCrossroad.Location = new System.Drawing.Point(833, 66);
            this.groupBoxCrossroad.Margin = new System.Windows.Forms.Padding(2);
            this.groupBoxCrossroad.Name = "groupBoxCrossroad";
            this.groupBoxCrossroad.Padding = new System.Windows.Forms.Padding(2);
            this.groupBoxCrossroad.Size = new System.Drawing.Size(213, 154);
            this.groupBoxCrossroad.TabIndex = 4;
            this.groupBoxCrossroad.TabStop = false;
            this.groupBoxCrossroad.Text = "Crossroad Properties";
            this.groupBoxCrossroad.Visible = false;
            // 
            // buttonDestroyCrossroad
            // 
            this.buttonDestroyCrossroad.Location = new System.Drawing.Point(4, 116);
            this.buttonDestroyCrossroad.Margin = new System.Windows.Forms.Padding(2);
            this.buttonDestroyCrossroad.Name = "buttonDestroyCrossroad";
            this.buttonDestroyCrossroad.Size = new System.Drawing.Size(186, 32);
            this.buttonDestroyCrossroad.TabIndex = 3;
            this.buttonDestroyCrossroad.Text = "Destroy Crossroad";
            this.buttonDestroyCrossroad.UseVisualStyleBackColor = true;
            this.buttonDestroyCrossroad.Click += new System.EventHandler(this.buttonDestroyCrossroad_Click);
            // 
            // buttonTrafficLight
            // 
            this.buttonTrafficLight.Location = new System.Drawing.Point(4, 83);
            this.buttonTrafficLight.Margin = new System.Windows.Forms.Padding(2);
            this.buttonTrafficLight.Name = "buttonTrafficLight";
            this.buttonTrafficLight.Size = new System.Drawing.Size(186, 32);
            this.buttonTrafficLight.TabIndex = 3;
            this.buttonTrafficLight.Text = "Customize Traffic Light";
            this.buttonTrafficLight.UseVisualStyleBackColor = true;
            this.buttonTrafficLight.Click += new System.EventHandler(this.buttonTrafficLight_Click);
            // 
            // labelOutIndex
            // 
            this.labelOutIndex.AutoSize = true;
            this.labelOutIndex.Location = new System.Drawing.Point(5, 59);
            this.labelOutIndex.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelOutIndex.Name = "labelOutIndex";
            this.labelOutIndex.Size = new System.Drawing.Size(129, 19);
            this.labelOutIndex.TabIndex = 2;
            this.labelOutIndex.Text = "Outcoming roads: -";
            // 
            // labelInIndex
            // 
            this.labelInIndex.AutoSize = true;
            this.labelInIndex.Location = new System.Drawing.Point(5, 40);
            this.labelInIndex.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelInIndex.Name = "labelInIndex";
            this.labelInIndex.Size = new System.Drawing.Size(117, 19);
            this.labelInIndex.TabIndex = 1;
            this.labelInIndex.Text = "Incoming roads: -";
            // 
            // labelCoords
            // 
            this.labelCoords.AutoSize = true;
            this.labelCoords.Location = new System.Drawing.Point(5, 21);
            this.labelCoords.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelCoords.Name = "labelCoords";
            this.labelCoords.Size = new System.Drawing.Size(75, 19);
            this.labelCoords.TabIndex = 0;
            this.labelCoords.Text = "Coords: -;-";
            // 
            // groupBoxRoad
            // 
            this.groupBoxRoad.Controls.Add(this.buttonDestroyRoad);
            this.groupBoxRoad.Controls.Add(this.labelRoadMaxSpeed);
            this.groupBoxRoad.Controls.Add(this.labelTo);
            this.groupBoxRoad.Controls.Add(this.labelFrom);
            this.groupBoxRoad.Controls.Add(this.labelRoadId);
            this.groupBoxRoad.Location = new System.Drawing.Point(833, 66);
            this.groupBoxRoad.Margin = new System.Windows.Forms.Padding(2);
            this.groupBoxRoad.Name = "groupBoxRoad";
            this.groupBoxRoad.Padding = new System.Windows.Forms.Padding(2);
            this.groupBoxRoad.Size = new System.Drawing.Size(213, 138);
            this.groupBoxRoad.TabIndex = 5;
            this.groupBoxRoad.TabStop = false;
            this.groupBoxRoad.Text = "Road Properties";
            this.groupBoxRoad.Visible = false;
            // 
            // buttonDestroyRoad
            // 
            this.buttonDestroyRoad.Location = new System.Drawing.Point(4, 99);
            this.buttonDestroyRoad.Margin = new System.Windows.Forms.Padding(2);
            this.buttonDestroyRoad.Name = "buttonDestroyRoad";
            this.buttonDestroyRoad.Size = new System.Drawing.Size(186, 32);
            this.buttonDestroyRoad.TabIndex = 3;
            this.buttonDestroyRoad.Text = "Destroy Road";
            this.buttonDestroyRoad.UseVisualStyleBackColor = true;
            this.buttonDestroyRoad.Click += new System.EventHandler(this.buttonDestroyRoad_Click);
            // 
            // labelRoadMaxSpeed
            // 
            this.labelRoadMaxSpeed.AutoSize = true;
            this.labelRoadMaxSpeed.Location = new System.Drawing.Point(4, 78);
            this.labelRoadMaxSpeed.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelRoadMaxSpeed.Name = "labelRoadMaxSpeed";
            this.labelRoadMaxSpeed.Size = new System.Drawing.Size(118, 19);
            this.labelRoadMaxSpeed.TabIndex = 0;
            this.labelRoadMaxSpeed.Text = "Max speed: - mps";
            // 
            // labelTo
            // 
            this.labelTo.AutoSize = true;
            this.labelTo.Location = new System.Drawing.Point(4, 59);
            this.labelTo.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelTo.Name = "labelTo";
            this.labelTo.Size = new System.Drawing.Size(45, 19);
            this.labelTo.TabIndex = 0;
            this.labelTo.Text = "To: -;-";
            // 
            // labelFrom
            // 
            this.labelFrom.AutoSize = true;
            this.labelFrom.Location = new System.Drawing.Point(4, 40);
            this.labelFrom.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelFrom.Name = "labelFrom";
            this.labelFrom.Size = new System.Drawing.Size(63, 19);
            this.labelFrom.TabIndex = 0;
            this.labelFrom.Text = "From: -;-";
            // 
            // labelRoadId
            // 
            this.labelRoadId.AutoSize = true;
            this.labelRoadId.Location = new System.Drawing.Point(4, 21);
            this.labelRoadId.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelRoadId.Name = "labelRoadId";
            this.labelRoadId.Size = new System.Drawing.Size(36, 19);
            this.labelRoadId.TabIndex = 0;
            this.labelRoadId.Text = "ID: -";
            // 
            // buttonCenter
            // 
            this.buttonCenter.Location = new System.Drawing.Point(833, 705);
            this.buttonCenter.Margin = new System.Windows.Forms.Padding(2);
            this.buttonCenter.Name = "buttonCenter";
            this.buttonCenter.Size = new System.Drawing.Size(96, 32);
            this.buttonCenter.TabIndex = 3;
            this.buttonCenter.Text = "Center Map";
            this.buttonCenter.UseVisualStyleBackColor = true;
            this.buttonCenter.Click += new System.EventHandler(this.buttonCenter_Click);
            // 
            // buttonZoom
            // 
            this.buttonZoom.Location = new System.Drawing.Point(934, 705);
            this.buttonZoom.Margin = new System.Windows.Forms.Padding(2);
            this.buttonZoom.Name = "buttonZoom";
            this.buttonZoom.Size = new System.Drawing.Size(96, 32);
            this.buttonZoom.TabIndex = 3;
            this.buttonZoom.Text = "Zoom: 1.0x";
            this.buttonZoom.UseVisualStyleBackColor = true;
            this.buttonZoom.Click += new System.EventHandler(this.buttonZoom_Click);
            // 
            // groupBoxSimulation
            // 
            this.groupBoxSimulation.Controls.Add(this.buttonStartSimulation);
            this.groupBoxSimulation.Controls.Add(this.labelCarFrequency);
            this.groupBoxSimulation.Controls.Add(this.trackBarCarFrequency);
            this.groupBoxSimulation.Controls.Add(this.labelDuration);
            this.groupBoxSimulation.Controls.Add(this.trackBarDuration);
            this.groupBoxSimulation.Location = new System.Drawing.Point(834, 453);
            this.groupBoxSimulation.Name = "groupBoxSimulation";
            this.groupBoxSimulation.Size = new System.Drawing.Size(213, 247);
            this.groupBoxSimulation.TabIndex = 6;
            this.groupBoxSimulation.TabStop = false;
            this.groupBoxSimulation.Text = "Simulation";
            // 
            // buttonStartSimulation
            // 
            this.buttonStartSimulation.Location = new System.Drawing.Point(5, 174);
            this.buttonStartSimulation.Margin = new System.Windows.Forms.Padding(2);
            this.buttonStartSimulation.Name = "buttonStartSimulation";
            this.buttonStartSimulation.Size = new System.Drawing.Size(185, 32);
            this.buttonStartSimulation.TabIndex = 3;
            this.buttonStartSimulation.Text = "Start Simulation";
            this.buttonStartSimulation.UseVisualStyleBackColor = true;
            this.buttonStartSimulation.Click += new System.EventHandler(this.buttonStartSimulation_Click);
            // 
            // labelCarFrequency
            // 
            this.labelCarFrequency.AutoSize = true;
            this.labelCarFrequency.Location = new System.Drawing.Point(6, 97);
            this.labelCarFrequency.Name = "labelCarFrequency";
            this.labelCarFrequency.Size = new System.Drawing.Size(128, 19);
            this.labelCarFrequency.TabIndex = 1;
            this.labelCarFrequency.Text = "Car frequency: 0.50";
            // 
            // trackBarCarFrequency
            // 
            this.trackBarCarFrequency.LargeChange = 10;
            this.trackBarCarFrequency.Location = new System.Drawing.Point(6, 119);
            this.trackBarCarFrequency.Maximum = 100;
            this.trackBarCarFrequency.Minimum = 1;
            this.trackBarCarFrequency.Name = "trackBarCarFrequency";
            this.trackBarCarFrequency.Size = new System.Drawing.Size(201, 50);
            this.trackBarCarFrequency.TabIndex = 0;
            this.trackBarCarFrequency.Value = 50;
            this.trackBarCarFrequency.Scroll += new System.EventHandler(this.trackBarCarFrequency_Scroll);
            // 
            // labelDuration
            // 
            this.labelDuration.AutoSize = true;
            this.labelDuration.Location = new System.Drawing.Point(6, 22);
            this.labelDuration.Name = "labelDuration";
            this.labelDuration.Size = new System.Drawing.Size(94, 19);
            this.labelDuration.TabIndex = 1;
            this.labelDuration.Text = "Duration: 10h";
            // 
            // trackBarDuration
            // 
            this.trackBarDuration.LargeChange = 4;
            this.trackBarDuration.Location = new System.Drawing.Point(6, 44);
            this.trackBarDuration.Maximum = 24;
            this.trackBarDuration.Minimum = 1;
            this.trackBarDuration.Name = "trackBarDuration";
            this.trackBarDuration.Size = new System.Drawing.Size(201, 50);
            this.trackBarDuration.TabIndex = 0;
            this.trackBarDuration.Value = 10;
            this.trackBarDuration.Scroll += new System.EventHandler(this.trackBarDuration_Scroll);
            // 
            // groupBoxStatistics
            // 
            this.groupBoxStatistics.Controls.Add(this.labelAvgDelay);
            this.groupBoxStatistics.Controls.Add(this.labelAvgDistance);
            this.groupBoxStatistics.Controls.Add(this.labelCars);
            this.groupBoxStatistics.Location = new System.Drawing.Point(834, 358);
            this.groupBoxStatistics.Name = "groupBoxStatistics";
            this.groupBoxStatistics.Size = new System.Drawing.Size(213, 89);
            this.groupBoxStatistics.TabIndex = 7;
            this.groupBoxStatistics.TabStop = false;
            this.groupBoxStatistics.Text = "Statistics";
            // 
            // labelCars
            // 
            this.labelCars.AutoSize = true;
            this.labelCars.Location = new System.Drawing.Point(6, 22);
            this.labelCars.Name = "labelCars";
            this.labelCars.Size = new System.Drawing.Size(100, 19);
            this.labelCars.TabIndex = 0;
            this.labelCars.Text = "Finished cars: -";
            // 
            // labelAvgDistance
            // 
            this.labelAvgDistance.AutoSize = true;
            this.labelAvgDistance.Location = new System.Drawing.Point(6, 41);
            this.labelAvgDistance.Name = "labelAvgDistance";
            this.labelAvgDistance.Size = new System.Drawing.Size(138, 19);
            this.labelAvgDistance.TabIndex = 0;
            this.labelAvgDistance.Text = "Average distance: -m";
            // 
            // labelAvgDelay
            // 
            this.labelAvgDelay.AutoSize = true;
            this.labelAvgDelay.Location = new System.Drawing.Point(6, 60);
            this.labelAvgDelay.Name = "labelAvgDelay";
            this.labelAvgDelay.Size = new System.Drawing.Size(114, 19);
            this.labelAvgDelay.TabIndex = 0;
            this.labelAvgDelay.Text = "Average delay: -s";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1063, 792);
            this.Controls.Add(this.groupBoxStatistics);
            this.Controls.Add(this.groupBoxSimulation);
            this.Controls.Add(this.groupBoxRoad);
            this.Controls.Add(this.buttonZoom);
            this.Controls.Add(this.buttonCenter);
            this.Controls.Add(this.groupBoxCrossroad);
            this.Controls.Add(this.comboBoxMode);
            this.Controls.Add(this.labelMode);
            this.Controls.Add(this.groupBoxBuild);
            this.Controls.Add(this.panelMap);
            this.Name = "FormMain";
            this.Text = "Road Traffic Simulator";
            this.groupBoxBuild.ResumeLayout(false);
            this.groupBoxBuild.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSpeed)).EndInit();
            this.groupBoxCrossroad.ResumeLayout(false);
            this.groupBoxCrossroad.PerformLayout();
            this.groupBoxRoad.ResumeLayout(false);
            this.groupBoxRoad.PerformLayout();
            this.groupBoxSimulation.ResumeLayout(false);
            this.groupBoxSimulation.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarCarFrequency)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarDuration)).EndInit();
            this.groupBoxStatistics.ResumeLayout(false);
            this.groupBoxStatistics.PerformLayout();
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
        private System.Windows.Forms.Button buttonDestroyCrossroad;
        private System.Windows.Forms.Button buttonDestroyRoad;
        private System.Windows.Forms.GroupBox groupBoxSimulation;
        private System.Windows.Forms.Button buttonStartSimulation;
        private System.Windows.Forms.Label labelCarFrequency;
        private System.Windows.Forms.TrackBar trackBarCarFrequency;
        private System.Windows.Forms.Label labelDuration;
        private System.Windows.Forms.TrackBar trackBarDuration;
        private System.Windows.Forms.GroupBox groupBoxStatistics;
        private System.Windows.Forms.Label labelAvgDelay;
        private System.Windows.Forms.Label labelAvgDistance;
        private System.Windows.Forms.Label labelCars;
    }
}

