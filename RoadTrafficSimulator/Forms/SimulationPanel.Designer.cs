﻿
namespace RoadTrafficSimulator.Forms
{
    partial class SimulationPanel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBoxRoad = new System.Windows.Forms.GroupBox();
            this.labelMaxSpeed = new System.Windows.Forms.Label();
            this.labelTo = new System.Windows.Forms.Label();
            this.labelFrom = new System.Windows.Forms.Label();
            this.labelTwoWayRoad = new System.Windows.Forms.Label();
            this.groupBoxCrossroad = new System.Windows.Forms.GroupBox();
            this.labelCarSpawnRate = new System.Windows.Forms.Label();
            this.labelOutIndex = new System.Windows.Forms.Label();
            this.labelInIndex = new System.Windows.Forms.Label();
            this.labelCoords = new System.Windows.Forms.Label();
            this.trackBarSimulationSpeed = new System.Windows.Forms.TrackBar();
            this.groupBoxSimulation = new System.Windows.Forms.GroupBox();
            this.labelSimulationTime = new System.Windows.Forms.Label();
            this.labelSimulationSpeed = new System.Windows.Forms.Label();
            this.groupBoxRoad.SuspendLayout();
            this.groupBoxCrossroad.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSimulationSpeed)).BeginInit();
            this.groupBoxSimulation.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxRoad
            // 
            this.groupBoxRoad.Controls.Add(this.labelMaxSpeed);
            this.groupBoxRoad.Controls.Add(this.labelTo);
            this.groupBoxRoad.Controls.Add(this.labelFrom);
            this.groupBoxRoad.Controls.Add(this.labelTwoWayRoad);
            this.groupBoxRoad.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxRoad.Location = new System.Drawing.Point(0, 142);
            this.groupBoxRoad.Name = "groupBoxRoad";
            this.groupBoxRoad.Size = new System.Drawing.Size(325, 262);
            this.groupBoxRoad.TabIndex = 0;
            this.groupBoxRoad.TabStop = false;
            this.groupBoxRoad.Text = "Road";
            // 
            // labelMaxSpeed
            // 
            this.labelMaxSpeed.AutoSize = true;
            this.labelMaxSpeed.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelMaxSpeed.Location = new System.Drawing.Point(3, 64);
            this.labelMaxSpeed.Name = "labelMaxSpeed";
            this.labelMaxSpeed.Size = new System.Drawing.Size(107, 15);
            this.labelMaxSpeed.TabIndex = 0;
            this.labelMaxSpeed.Text = "Max speed: - km/h";
            // 
            // labelTo
            // 
            this.labelTo.AutoSize = true;
            this.labelTo.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelTo.Location = new System.Drawing.Point(3, 49);
            this.labelTo.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.labelTo.Name = "labelTo";
            this.labelTo.Size = new System.Drawing.Size(38, 15);
            this.labelTo.TabIndex = 0;
            this.labelTo.Text = "To: -;-";
            // 
            // labelFrom
            // 
            this.labelFrom.AutoSize = true;
            this.labelFrom.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelFrom.Location = new System.Drawing.Point(3, 34);
            this.labelFrom.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.labelFrom.Name = "labelFrom";
            this.labelFrom.Size = new System.Drawing.Size(54, 15);
            this.labelFrom.TabIndex = 0;
            this.labelFrom.Text = "From: -;-";
            // 
            // labelTwoWayRoad
            // 
            this.labelTwoWayRoad.AutoSize = true;
            this.labelTwoWayRoad.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelTwoWayRoad.Location = new System.Drawing.Point(3, 19);
            this.labelTwoWayRoad.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.labelTwoWayRoad.Name = "labelTwoWayRoad";
            this.labelTwoWayRoad.Size = new System.Drawing.Size(12, 15);
            this.labelTwoWayRoad.TabIndex = 0;
            this.labelTwoWayRoad.Text = "-";
            // 
            // groupBoxCrossroad
            // 
            this.groupBoxCrossroad.AutoSize = true;
            this.groupBoxCrossroad.Controls.Add(this.labelCarSpawnRate);
            this.groupBoxCrossroad.Controls.Add(this.labelOutIndex);
            this.groupBoxCrossroad.Controls.Add(this.labelInIndex);
            this.groupBoxCrossroad.Controls.Add(this.labelCoords);
            this.groupBoxCrossroad.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxCrossroad.Location = new System.Drawing.Point(0, 404);
            this.groupBoxCrossroad.Name = "groupBoxCrossroad";
            this.groupBoxCrossroad.Size = new System.Drawing.Size(325, 82);
            this.groupBoxCrossroad.TabIndex = 0;
            this.groupBoxCrossroad.TabStop = false;
            this.groupBoxCrossroad.Text = "Crossroad";
            // 
            // labelCarSpawnRate
            // 
            this.labelCarSpawnRate.AutoSize = true;
            this.labelCarSpawnRate.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelCarSpawnRate.Location = new System.Drawing.Point(3, 64);
            this.labelCarSpawnRate.Name = "labelCarSpawnRate";
            this.labelCarSpawnRate.Size = new System.Drawing.Size(109, 15);
            this.labelCarSpawnRate.TabIndex = 0;
            this.labelCarSpawnRate.Text = "Car spawn rate: - %";
            // 
            // labelOutIndex
            // 
            this.labelOutIndex.AutoSize = true;
            this.labelOutIndex.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelOutIndex.Location = new System.Drawing.Point(3, 49);
            this.labelOutIndex.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelOutIndex.Name = "labelOutIndex";
            this.labelOutIndex.Size = new System.Drawing.Size(111, 15);
            this.labelOutIndex.TabIndex = 0;
            this.labelOutIndex.Text = "Outcoming roads: -";
            // 
            // labelInIndex
            // 
            this.labelInIndex.AutoSize = true;
            this.labelInIndex.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelInIndex.Location = new System.Drawing.Point(3, 34);
            this.labelInIndex.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelInIndex.Name = "labelInIndex";
            this.labelInIndex.Size = new System.Drawing.Size(101, 15);
            this.labelInIndex.TabIndex = 0;
            this.labelInIndex.Text = "Incoming roads: -";
            // 
            // labelCoords
            // 
            this.labelCoords.AutoSize = true;
            this.labelCoords.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelCoords.Location = new System.Drawing.Point(3, 19);
            this.labelCoords.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelCoords.Name = "labelCoords";
            this.labelCoords.Size = new System.Drawing.Size(64, 15);
            this.labelCoords.TabIndex = 0;
            this.labelCoords.Text = "Coords: -;-";
            // 
            // trackBarSimulationSpeed
            // 
            this.trackBarSimulationSpeed.Dock = System.Windows.Forms.DockStyle.Top;
            this.trackBarSimulationSpeed.LargeChange = 2;
            this.trackBarSimulationSpeed.Location = new System.Drawing.Point(3, 34);
            this.trackBarSimulationSpeed.Maximum = 9;
            this.trackBarSimulationSpeed.Name = "trackBarSimulationSpeed";
            this.trackBarSimulationSpeed.Size = new System.Drawing.Size(319, 45);
            this.trackBarSimulationSpeed.TabIndex = 1;
            this.trackBarSimulationSpeed.Scroll += new System.EventHandler(this.trackBarSimulationSpeed_Scroll);
            // 
            // groupBoxSimulation
            // 
            this.groupBoxSimulation.AutoSize = true;
            this.groupBoxSimulation.Controls.Add(this.labelSimulationTime);
            this.groupBoxSimulation.Controls.Add(this.trackBarSimulationSpeed);
            this.groupBoxSimulation.Controls.Add(this.labelSimulationSpeed);
            this.groupBoxSimulation.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxSimulation.Location = new System.Drawing.Point(0, 0);
            this.groupBoxSimulation.Name = "groupBoxSimulation";
            this.groupBoxSimulation.Size = new System.Drawing.Size(325, 142);
            this.groupBoxSimulation.TabIndex = 2;
            this.groupBoxSimulation.TabStop = false;
            this.groupBoxSimulation.Text = "Simulation";
            // 
            // labelSimulationTime
            // 
            this.labelSimulationTime.AutoSize = true;
            this.labelSimulationTime.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelSimulationTime.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.labelSimulationTime.Location = new System.Drawing.Point(3, 79);
            this.labelSimulationTime.Name = "labelSimulationTime";
            this.labelSimulationTime.Size = new System.Drawing.Size(176, 60);
            this.labelSimulationTime.TabIndex = 3;
            this.labelSimulationTime.Text = "Simulation time:\r\n0d 00:00:00";
            // 
            // labelSimulationSpeed
            // 
            this.labelSimulationSpeed.AutoSize = true;
            this.labelSimulationSpeed.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelSimulationSpeed.Location = new System.Drawing.Point(3, 19);
            this.labelSimulationSpeed.Name = "labelSimulationSpeed";
            this.labelSimulationSpeed.Size = new System.Drawing.Size(116, 15);
            this.labelSimulationSpeed.TabIndex = 2;
            this.labelSimulationSpeed.Text = "Simulation speed: 1x";
            // 
            // SimulationPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxCrossroad);
            this.Controls.Add(this.groupBoxRoad);
            this.Controls.Add(this.groupBoxSimulation);
            this.Name = "SimulationPanel";
            this.Size = new System.Drawing.Size(325, 547);
            this.groupBoxRoad.ResumeLayout(false);
            this.groupBoxRoad.PerformLayout();
            this.groupBoxCrossroad.ResumeLayout(false);
            this.groupBoxCrossroad.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSimulationSpeed)).EndInit();
            this.groupBoxSimulation.ResumeLayout(false);
            this.groupBoxSimulation.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxRoad;
        private System.Windows.Forms.Label labelTo;
        private System.Windows.Forms.Label labelFrom;
        private System.Windows.Forms.Label labelTwoWayRoad;
        private System.Windows.Forms.Label labelMaxSpeed;
        private System.Windows.Forms.GroupBox groupBoxCrossroad;
        private System.Windows.Forms.Label labelCarSpawnRate;
        private System.Windows.Forms.Label labelOutIndex;
        private System.Windows.Forms.Label labelInIndex;
        private System.Windows.Forms.Label labelCoords;
        private System.Windows.Forms.TrackBar trackBarSimulationSpeed;
        private System.Windows.Forms.GroupBox groupBoxSimulation;
        private System.Windows.Forms.Label labelSimulationTime;
        private System.Windows.Forms.Label labelSimulationSpeed;
    }
}
