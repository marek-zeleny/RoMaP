﻿namespace RoadTrafficSimulator.Forms
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
            this.labelMode = new System.Windows.Forms.Label();
            this.comboBoxMode = new System.Windows.Forms.ComboBox();
            this.groupBoxBuild = new System.Windows.Forms.GroupBox();
            this.checkBoxTwoWayRoad = new System.Windows.Forms.CheckBox();
            this.groupBoxCrossroad = new System.Windows.Forms.GroupBox();
            this.labelCarSpawnRate = new System.Windows.Forms.Label();
            this.trackBarCarSpawnRate = new System.Windows.Forms.TrackBar();
            this.buttonDestroyCrossroad = new System.Windows.Forms.Button();
            this.buttonTrafficLight = new System.Windows.Forms.Button();
            this.labelOutIndex = new System.Windows.Forms.Label();
            this.labelInIndex = new System.Windows.Forms.Label();
            this.labelCoords = new System.Windows.Forms.Label();
            this.groupBoxRoad = new System.Windows.Forms.GroupBox();
            this.numericUpDownMaxSpeed = new System.Windows.Forms.NumericUpDown();
            this.labelMps = new System.Windows.Forms.Label();
            this.buttonDestroyRoad = new System.Windows.Forms.Button();
            this.labelMaxSpeed = new System.Windows.Forms.Label();
            this.labelTo = new System.Windows.Forms.Label();
            this.labelFrom = new System.Windows.Forms.Label();
            this.labelTwoWayRoad = new System.Windows.Forms.Label();
            this.buttonCenter = new System.Windows.Forms.Button();
            this.buttonZoom = new System.Windows.Forms.Button();
            this.groupBoxSimulation = new System.Windows.Forms.GroupBox();
            this.buttonStartSimulation = new System.Windows.Forms.Button();
            this.labelCarFrequency = new System.Windows.Forms.Label();
            this.trackBarCarFrequency = new System.Windows.Forms.TrackBar();
            this.labelDuration = new System.Windows.Forms.Label();
            this.trackBarDuration = new System.Windows.Forms.TrackBar();
            this.groupBoxStatistics = new System.Windows.Forms.GroupBox();
            this.buttonExportStats = new System.Windows.Forms.Button();
            this.labelAvgDuration = new System.Windows.Forms.Label();
            this.labelAvgDelay = new System.Windows.Forms.Label();
            this.labelAvgDistance = new System.Windows.Forms.Label();
            this.labelCars = new System.Windows.Forms.Label();
            this.buttonLoadMap = new System.Windows.Forms.Button();
            this.buttonSaveMap = new System.Windows.Forms.Button();
            this.groupBoxInfo = new System.Windows.Forms.GroupBox();
            this.labelInfo = new System.Windows.Forms.Label();
            this.mapPanel = new RoadTrafficSimulator.Forms.MapPanel();
            this.groupBoxBuild.SuspendLayout();
            this.groupBoxCrossroad.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarCarSpawnRate)).BeginInit();
            this.groupBoxRoad.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaxSpeed)).BeginInit();
            this.groupBoxSimulation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarCarFrequency)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarDuration)).BeginInit();
            this.groupBoxStatistics.SuspendLayout();
            this.groupBoxInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelMode
            // 
            this.labelMode.AutoSize = true;
            this.labelMode.Location = new System.Drawing.Point(712, 7);
            this.labelMode.Name = "labelMode";
            this.labelMode.Size = new System.Drawing.Size(38, 15);
            this.labelMode.TabIndex = 1;
            this.labelMode.Text = "Mode";
            // 
            // comboBoxMode
            // 
            this.comboBoxMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMode.FormattingEnabled = true;
            this.comboBoxMode.Location = new System.Drawing.Point(712, 25);
            this.comboBoxMode.Name = "comboBoxMode";
            this.comboBoxMode.Size = new System.Drawing.Size(207, 23);
            this.comboBoxMode.TabIndex = 2;
            this.comboBoxMode.SelectedIndexChanged += new System.EventHandler(this.comboBoxMode_SelectedIndexChanged);
            // 
            // groupBoxBuild
            // 
            this.groupBoxBuild.Controls.Add(this.checkBoxTwoWayRoad);
            this.groupBoxBuild.Location = new System.Drawing.Point(712, 85);
            this.groupBoxBuild.Name = "groupBoxBuild";
            this.groupBoxBuild.Size = new System.Drawing.Size(207, 45);
            this.groupBoxBuild.TabIndex = 3;
            this.groupBoxBuild.TabStop = false;
            this.groupBoxBuild.Text = "Build Properties";
            this.groupBoxBuild.Visible = false;
            // 
            // checkBoxTwoWayRoad
            // 
            this.checkBoxTwoWayRoad.AutoSize = true;
            this.checkBoxTwoWayRoad.Checked = true;
            this.checkBoxTwoWayRoad.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxTwoWayRoad.Location = new System.Drawing.Point(5, 21);
            this.checkBoxTwoWayRoad.Name = "checkBoxTwoWayRoad";
            this.checkBoxTwoWayRoad.Size = new System.Drawing.Size(100, 19);
            this.checkBoxTwoWayRoad.TabIndex = 0;
            this.checkBoxTwoWayRoad.Text = "Two-way road";
            this.checkBoxTwoWayRoad.UseVisualStyleBackColor = true;
            // 
            // groupBoxCrossroad
            // 
            this.groupBoxCrossroad.Controls.Add(this.labelCarSpawnRate);
            this.groupBoxCrossroad.Controls.Add(this.trackBarCarSpawnRate);
            this.groupBoxCrossroad.Controls.Add(this.buttonDestroyCrossroad);
            this.groupBoxCrossroad.Controls.Add(this.buttonTrafficLight);
            this.groupBoxCrossroad.Controls.Add(this.labelOutIndex);
            this.groupBoxCrossroad.Controls.Add(this.labelInIndex);
            this.groupBoxCrossroad.Controls.Add(this.labelCoords);
            this.groupBoxCrossroad.Location = new System.Drawing.Point(712, 85);
            this.groupBoxCrossroad.Margin = new System.Windows.Forms.Padding(2);
            this.groupBoxCrossroad.Name = "groupBoxCrossroad";
            this.groupBoxCrossroad.Padding = new System.Windows.Forms.Padding(2);
            this.groupBoxCrossroad.Size = new System.Drawing.Size(207, 161);
            this.groupBoxCrossroad.TabIndex = 4;
            this.groupBoxCrossroad.TabStop = false;
            this.groupBoxCrossroad.Text = "Crossroad Properties";
            this.groupBoxCrossroad.Visible = false;
            // 
            // labelCarSpawnRate
            // 
            this.labelCarSpawnRate.AutoSize = true;
            this.labelCarSpawnRate.Location = new System.Drawing.Point(6, 64);
            this.labelCarSpawnRate.Name = "labelCarSpawnRate";
            this.labelCarSpawnRate.Size = new System.Drawing.Size(109, 15);
            this.labelCarSpawnRate.TabIndex = 5;
            this.labelCarSpawnRate.Text = "Car spawn rate: - %";
            // 
            // trackBarCarSpawnRate
            // 
            this.trackBarCarSpawnRate.Location = new System.Drawing.Point(5, 82);
            this.trackBarCarSpawnRate.Maximum = 100;
            this.trackBarCarSpawnRate.Minimum = 1;
            this.trackBarCarSpawnRate.Name = "trackBarCarSpawnRate";
            this.trackBarCarSpawnRate.Size = new System.Drawing.Size(198, 45);
            this.trackBarCarSpawnRate.TabIndex = 4;
            this.trackBarCarSpawnRate.Value = 10;
            this.trackBarCarSpawnRate.Scroll += new System.EventHandler(this.trackBarCarSpawnRate_Scroll);
            // 
            // buttonDestroyCrossroad
            // 
            this.buttonDestroyCrossroad.Location = new System.Drawing.Point(106, 132);
            this.buttonDestroyCrossroad.Margin = new System.Windows.Forms.Padding(2);
            this.buttonDestroyCrossroad.Name = "buttonDestroyCrossroad";
            this.buttonDestroyCrossroad.Size = new System.Drawing.Size(97, 25);
            this.buttonDestroyCrossroad.TabIndex = 3;
            this.buttonDestroyCrossroad.Text = "Destroy Crossroad";
            this.buttonDestroyCrossroad.UseVisualStyleBackColor = true;
            this.buttonDestroyCrossroad.Click += new System.EventHandler(this.buttonDestroyCrossroad_Click);
            // 
            // buttonTrafficLight
            // 
            this.buttonTrafficLight.Location = new System.Drawing.Point(4, 132);
            this.buttonTrafficLight.Margin = new System.Windows.Forms.Padding(2);
            this.buttonTrafficLight.Name = "buttonTrafficLight";
            this.buttonTrafficLight.Size = new System.Drawing.Size(97, 25);
            this.buttonTrafficLight.TabIndex = 3;
            this.buttonTrafficLight.Text = "Customize Traffic Light";
            this.buttonTrafficLight.UseVisualStyleBackColor = true;
            this.buttonTrafficLight.Click += new System.EventHandler(this.buttonTrafficLight_Click);
            // 
            // labelOutIndex
            // 
            this.labelOutIndex.AutoSize = true;
            this.labelOutIndex.Location = new System.Drawing.Point(4, 47);
            this.labelOutIndex.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelOutIndex.Name = "labelOutIndex";
            this.labelOutIndex.Size = new System.Drawing.Size(111, 15);
            this.labelOutIndex.TabIndex = 2;
            this.labelOutIndex.Text = "Outcoming roads: -";
            // 
            // labelInIndex
            // 
            this.labelInIndex.AutoSize = true;
            this.labelInIndex.Location = new System.Drawing.Point(4, 32);
            this.labelInIndex.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelInIndex.Name = "labelInIndex";
            this.labelInIndex.Size = new System.Drawing.Size(101, 15);
            this.labelInIndex.TabIndex = 1;
            this.labelInIndex.Text = "Incoming roads: -";
            // 
            // labelCoords
            // 
            this.labelCoords.AutoSize = true;
            this.labelCoords.Location = new System.Drawing.Point(4, 17);
            this.labelCoords.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelCoords.Name = "labelCoords";
            this.labelCoords.Size = new System.Drawing.Size(64, 15);
            this.labelCoords.TabIndex = 0;
            this.labelCoords.Text = "Coords: -;-";
            // 
            // groupBoxRoad
            // 
            this.groupBoxRoad.Controls.Add(this.numericUpDownMaxSpeed);
            this.groupBoxRoad.Controls.Add(this.labelMps);
            this.groupBoxRoad.Controls.Add(this.buttonDestroyRoad);
            this.groupBoxRoad.Controls.Add(this.labelMaxSpeed);
            this.groupBoxRoad.Controls.Add(this.labelTo);
            this.groupBoxRoad.Controls.Add(this.labelFrom);
            this.groupBoxRoad.Controls.Add(this.labelTwoWayRoad);
            this.groupBoxRoad.Location = new System.Drawing.Point(712, 85);
            this.groupBoxRoad.Margin = new System.Windows.Forms.Padding(2);
            this.groupBoxRoad.Name = "groupBoxRoad";
            this.groupBoxRoad.Padding = new System.Windows.Forms.Padding(2);
            this.groupBoxRoad.Size = new System.Drawing.Size(207, 120);
            this.groupBoxRoad.TabIndex = 5;
            this.groupBoxRoad.TabStop = false;
            this.groupBoxRoad.Text = "Road Properties";
            this.groupBoxRoad.Visible = false;
            // 
            // numericUpDownMaxSpeed
            // 
            this.numericUpDownMaxSpeed.Location = new System.Drawing.Point(81, 65);
            this.numericUpDownMaxSpeed.Maximum = new decimal(new int[] {
            40,
            0,
            0,
            0});
            this.numericUpDownMaxSpeed.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDownMaxSpeed.Name = "numericUpDownMaxSpeed";
            this.numericUpDownMaxSpeed.Size = new System.Drawing.Size(43, 23);
            this.numericUpDownMaxSpeed.TabIndex = 1;
            this.numericUpDownMaxSpeed.Value = new decimal(new int[] {
            14,
            0,
            0,
            0});
            this.numericUpDownMaxSpeed.ValueChanged += new System.EventHandler(this.numericUpDownMaxSpeed_ValueChanged);
            // 
            // labelMps
            // 
            this.labelMps.AutoSize = true;
            this.labelMps.Location = new System.Drawing.Point(130, 67);
            this.labelMps.Name = "labelMps";
            this.labelMps.Size = new System.Drawing.Size(30, 15);
            this.labelMps.TabIndex = 3;
            this.labelMps.Text = "mps";
            // 
            // buttonDestroyRoad
            // 
            this.buttonDestroyRoad.Location = new System.Drawing.Point(4, 91);
            this.buttonDestroyRoad.Margin = new System.Windows.Forms.Padding(2);
            this.buttonDestroyRoad.Name = "buttonDestroyRoad";
            this.buttonDestroyRoad.Size = new System.Drawing.Size(199, 25);
            this.buttonDestroyRoad.TabIndex = 3;
            this.buttonDestroyRoad.Text = "Destroy Road";
            this.buttonDestroyRoad.UseVisualStyleBackColor = true;
            this.buttonDestroyRoad.Click += new System.EventHandler(this.buttonDestroyRoad_Click);
            // 
            // labelMaxSpeed
            // 
            this.labelMaxSpeed.AutoSize = true;
            this.labelMaxSpeed.Location = new System.Drawing.Point(4, 67);
            this.labelMaxSpeed.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelMaxSpeed.Name = "labelMaxSpeed";
            this.labelMaxSpeed.Size = new System.Drawing.Size(67, 15);
            this.labelMaxSpeed.TabIndex = 0;
            this.labelMaxSpeed.Text = "Max speed:";
            // 
            // labelTo
            // 
            this.labelTo.AutoSize = true;
            this.labelTo.Location = new System.Drawing.Point(4, 47);
            this.labelTo.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelTo.Name = "labelTo";
            this.labelTo.Size = new System.Drawing.Size(38, 15);
            this.labelTo.TabIndex = 0;
            this.labelTo.Text = "To: -;-";
            // 
            // labelFrom
            // 
            this.labelFrom.AutoSize = true;
            this.labelFrom.Location = new System.Drawing.Point(4, 32);
            this.labelFrom.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelFrom.Name = "labelFrom";
            this.labelFrom.Size = new System.Drawing.Size(54, 15);
            this.labelFrom.TabIndex = 0;
            this.labelFrom.Text = "From: -;-";
            // 
            // labelTwoWayRoad
            // 
            this.labelTwoWayRoad.AutoSize = true;
            this.labelTwoWayRoad.Location = new System.Drawing.Point(4, 17);
            this.labelTwoWayRoad.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelTwoWayRoad.Name = "labelTwoWayRoad";
            this.labelTwoWayRoad.Size = new System.Drawing.Size(12, 15);
            this.labelTwoWayRoad.TabIndex = 0;
            this.labelTwoWayRoad.Text = "-";
            // 
            // buttonCenter
            // 
            this.buttonCenter.Location = new System.Drawing.Point(712, 613);
            this.buttonCenter.Margin = new System.Windows.Forms.Padding(2);
            this.buttonCenter.Name = "buttonCenter";
            this.buttonCenter.Size = new System.Drawing.Size(102, 25);
            this.buttonCenter.TabIndex = 3;
            this.buttonCenter.Text = "Center Map";
            this.buttonCenter.UseVisualStyleBackColor = true;
            this.buttonCenter.Click += new System.EventHandler(this.buttonCenter_Click);
            // 
            // buttonZoom
            // 
            this.buttonZoom.Location = new System.Drawing.Point(817, 613);
            this.buttonZoom.Margin = new System.Windows.Forms.Padding(2);
            this.buttonZoom.Name = "buttonZoom";
            this.buttonZoom.Size = new System.Drawing.Size(102, 25);
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
            this.groupBoxSimulation.Location = new System.Drawing.Point(712, 250);
            this.groupBoxSimulation.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBoxSimulation.Name = "groupBoxSimulation";
            this.groupBoxSimulation.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBoxSimulation.Size = new System.Drawing.Size(207, 167);
            this.groupBoxSimulation.TabIndex = 6;
            this.groupBoxSimulation.TabStop = false;
            this.groupBoxSimulation.Text = "Simulation";
            // 
            // buttonStartSimulation
            // 
            this.buttonStartSimulation.Location = new System.Drawing.Point(4, 137);
            this.buttonStartSimulation.Margin = new System.Windows.Forms.Padding(2);
            this.buttonStartSimulation.Name = "buttonStartSimulation";
            this.buttonStartSimulation.Size = new System.Drawing.Size(199, 25);
            this.buttonStartSimulation.TabIndex = 3;
            this.buttonStartSimulation.Text = "Start Simulation";
            this.buttonStartSimulation.UseVisualStyleBackColor = true;
            this.buttonStartSimulation.Click += new System.EventHandler(this.buttonStartSimulation_Click);
            // 
            // labelCarFrequency
            // 
            this.labelCarFrequency.AutoSize = true;
            this.labelCarFrequency.Location = new System.Drawing.Point(5, 77);
            this.labelCarFrequency.Name = "labelCarFrequency";
            this.labelCarFrequency.Size = new System.Drawing.Size(108, 15);
            this.labelCarFrequency.TabIndex = 1;
            this.labelCarFrequency.Text = "Car frequency: 0.50";
            // 
            // trackBarCarFrequency
            // 
            this.trackBarCarFrequency.LargeChange = 10;
            this.trackBarCarFrequency.Location = new System.Drawing.Point(5, 94);
            this.trackBarCarFrequency.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.trackBarCarFrequency.Maximum = 100;
            this.trackBarCarFrequency.Minimum = 1;
            this.trackBarCarFrequency.Name = "trackBarCarFrequency";
            this.trackBarCarFrequency.Size = new System.Drawing.Size(196, 45);
            this.trackBarCarFrequency.TabIndex = 0;
            this.trackBarCarFrequency.Value = 50;
            this.trackBarCarFrequency.Scroll += new System.EventHandler(this.trackBarCarFrequency_Scroll);
            // 
            // labelDuration
            // 
            this.labelDuration.AutoSize = true;
            this.labelDuration.Location = new System.Drawing.Point(5, 17);
            this.labelDuration.Name = "labelDuration";
            this.labelDuration.Size = new System.Drawing.Size(78, 15);
            this.labelDuration.TabIndex = 1;
            this.labelDuration.Text = "Duration: 10h";
            // 
            // trackBarDuration
            // 
            this.trackBarDuration.LargeChange = 4;
            this.trackBarDuration.Location = new System.Drawing.Point(5, 35);
            this.trackBarDuration.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.trackBarDuration.Maximum = 24;
            this.trackBarDuration.Minimum = 1;
            this.trackBarDuration.Name = "trackBarDuration";
            this.trackBarDuration.Size = new System.Drawing.Size(196, 45);
            this.trackBarDuration.TabIndex = 0;
            this.trackBarDuration.Value = 10;
            this.trackBarDuration.Scroll += new System.EventHandler(this.trackBarDuration_Scroll);
            // 
            // groupBoxStatistics
            // 
            this.groupBoxStatistics.Controls.Add(this.buttonExportStats);
            this.groupBoxStatistics.Controls.Add(this.labelAvgDuration);
            this.groupBoxStatistics.Controls.Add(this.labelAvgDelay);
            this.groupBoxStatistics.Controls.Add(this.labelAvgDistance);
            this.groupBoxStatistics.Controls.Add(this.labelCars);
            this.groupBoxStatistics.Location = new System.Drawing.Point(712, 422);
            this.groupBoxStatistics.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBoxStatistics.Name = "groupBoxStatistics";
            this.groupBoxStatistics.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBoxStatistics.Size = new System.Drawing.Size(207, 109);
            this.groupBoxStatistics.TabIndex = 7;
            this.groupBoxStatistics.TabStop = false;
            this.groupBoxStatistics.Text = "Statistics";
            // 
            // buttonExportStats
            // 
            this.buttonExportStats.Location = new System.Drawing.Point(4, 80);
            this.buttonExportStats.Margin = new System.Windows.Forms.Padding(2);
            this.buttonExportStats.Name = "buttonExportStats";
            this.buttonExportStats.Size = new System.Drawing.Size(199, 25);
            this.buttonExportStats.TabIndex = 4;
            this.buttonExportStats.Text = "Export CSV";
            this.buttonExportStats.UseVisualStyleBackColor = true;
            this.buttonExportStats.Click += new System.EventHandler(this.buttonExportStats_Click);
            // 
            // labelAvgDuration
            // 
            this.labelAvgDuration.AutoSize = true;
            this.labelAvgDuration.Location = new System.Drawing.Point(5, 47);
            this.labelAvgDuration.Name = "labelAvgDuration";
            this.labelAvgDuration.Size = new System.Drawing.Size(114, 15);
            this.labelAvgDuration.TabIndex = 0;
            this.labelAvgDuration.Text = "Average duration: -s";
            // 
            // labelAvgDelay
            // 
            this.labelAvgDelay.AutoSize = true;
            this.labelAvgDelay.Location = new System.Drawing.Point(5, 62);
            this.labelAvgDelay.Name = "labelAvgDelay";
            this.labelAvgDelay.Size = new System.Drawing.Size(97, 15);
            this.labelAvgDelay.TabIndex = 0;
            this.labelAvgDelay.Text = "Average delay: -s";
            // 
            // labelAvgDistance
            // 
            this.labelAvgDistance.AutoSize = true;
            this.labelAvgDistance.Location = new System.Drawing.Point(5, 32);
            this.labelAvgDistance.Name = "labelAvgDistance";
            this.labelAvgDistance.Size = new System.Drawing.Size(119, 15);
            this.labelAvgDistance.TabIndex = 0;
            this.labelAvgDistance.Text = "Average distance: -m";
            // 
            // labelCars
            // 
            this.labelCars.AutoSize = true;
            this.labelCars.Location = new System.Drawing.Point(5, 17);
            this.labelCars.Name = "labelCars";
            this.labelCars.Size = new System.Drawing.Size(96, 15);
            this.labelCars.TabIndex = 0;
            this.labelCars.Text = "Finished cars: -/-";
            // 
            // buttonLoadMap
            // 
            this.buttonLoadMap.Location = new System.Drawing.Point(816, 53);
            this.buttonLoadMap.Margin = new System.Windows.Forms.Padding(2);
            this.buttonLoadMap.Name = "buttonLoadMap";
            this.buttonLoadMap.Size = new System.Drawing.Size(102, 25);
            this.buttonLoadMap.TabIndex = 3;
            this.buttonLoadMap.Text = "Load Map";
            this.buttonLoadMap.UseVisualStyleBackColor = true;
            this.buttonLoadMap.Click += new System.EventHandler(this.buttonLoadMap_Click);
            // 
            // buttonSaveMap
            // 
            this.buttonSaveMap.Location = new System.Drawing.Point(711, 53);
            this.buttonSaveMap.Margin = new System.Windows.Forms.Padding(2);
            this.buttonSaveMap.Name = "buttonSaveMap";
            this.buttonSaveMap.Size = new System.Drawing.Size(102, 25);
            this.buttonSaveMap.TabIndex = 3;
            this.buttonSaveMap.Text = "Save Map";
            this.buttonSaveMap.UseVisualStyleBackColor = true;
            this.buttonSaveMap.Click += new System.EventHandler(this.buttonSaveMap_Click);
            // 
            // groupBoxInfo
            // 
            this.groupBoxInfo.Controls.Add(this.labelInfo);
            this.groupBoxInfo.Location = new System.Drawing.Point(712, 536);
            this.groupBoxInfo.Name = "groupBoxInfo";
            this.groupBoxInfo.Size = new System.Drawing.Size(207, 72);
            this.groupBoxInfo.TabIndex = 8;
            this.groupBoxInfo.TabStop = false;
            this.groupBoxInfo.Text = "Info";
            // 
            // labelInfo
            // 
            this.labelInfo.Location = new System.Drawing.Point(5, 18);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(197, 51);
            this.labelInfo.TabIndex = 0;
            // 
            // mapPanel
            // 
            this.mapPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(237)))), ((int)(((byte)(242)))));
            this.mapPanel.Cursor = System.Windows.Forms.Cursors.Cross;
            this.mapPanel.Location = new System.Drawing.Point(12, 12);
            this.mapPanel.Name = "mapPanel";
            this.mapPanel.Size = new System.Drawing.Size(694, 626);
            this.mapPanel.TabIndex = 0;
            this.mapPanel.Zoom = 1F;
            this.mapPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.mapPanel_Paint);
            this.mapPanel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mapPanel_MouseClick);
            this.mapPanel.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.mapPanel_MouseDoubleClick);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(931, 653);
            this.Controls.Add(this.mapPanel);
            this.Controls.Add(this.groupBoxRoad);
            this.Controls.Add(this.groupBoxBuild);
            this.Controls.Add(this.groupBoxInfo);
            this.Controls.Add(this.buttonSaveMap);
            this.Controls.Add(this.buttonLoadMap);
            this.Controls.Add(this.groupBoxCrossroad);
            this.Controls.Add(this.groupBoxStatistics);
            this.Controls.Add(this.groupBoxSimulation);
            this.Controls.Add(this.buttonZoom);
            this.Controls.Add(this.buttonCenter);
            this.Controls.Add(this.comboBoxMode);
            this.Controls.Add(this.labelMode);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Road Traffic Simulator";
            this.groupBoxBuild.ResumeLayout(false);
            this.groupBoxBuild.PerformLayout();
            this.groupBoxCrossroad.ResumeLayout(false);
            this.groupBoxCrossroad.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarCarSpawnRate)).EndInit();
            this.groupBoxRoad.ResumeLayout(false);
            this.groupBoxRoad.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaxSpeed)).EndInit();
            this.groupBoxSimulation.ResumeLayout(false);
            this.groupBoxSimulation.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarCarFrequency)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarDuration)).EndInit();
            this.groupBoxStatistics.ResumeLayout(false);
            this.groupBoxStatistics.PerformLayout();
            this.groupBoxInfo.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label labelMode;
        private System.Windows.Forms.ComboBox comboBoxMode;
        private System.Windows.Forms.GroupBox groupBoxBuild;
        private System.Windows.Forms.CheckBox checkBoxTwoWayRoad;
        private System.Windows.Forms.GroupBox groupBoxCrossroad;
        private System.Windows.Forms.Button buttonTrafficLight;
        private System.Windows.Forms.Label labelOutIndex;
        private System.Windows.Forms.Label labelInIndex;
        private System.Windows.Forms.Label labelCoords;
        private System.Windows.Forms.Button buttonCenter;
        private System.Windows.Forms.Button buttonZoom;
        private System.Windows.Forms.GroupBox groupBoxRoad;
        private System.Windows.Forms.Label labelMaxSpeed;
        private System.Windows.Forms.Label labelTo;
        private System.Windows.Forms.Label labelFrom;
        private System.Windows.Forms.Label labelTwoWayRoad;
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
        private System.Windows.Forms.Label labelAvgDuration;
        private System.Windows.Forms.Button buttonLoadMap;
        private System.Windows.Forms.Button buttonSaveMap;
        private System.Windows.Forms.GroupBox groupBoxInfo;
        private System.Windows.Forms.Label labelInfo;
        private System.Windows.Forms.Label labelCarSpawnRate;
        private System.Windows.Forms.TrackBar trackBarCarSpawnRate;
        private System.Windows.Forms.NumericUpDown numericUpDownMaxSpeed;
        private System.Windows.Forms.Label labelMps;
        private System.Windows.Forms.Button buttonExportStats;
        private MapPanel mapPanel;
    }
}
