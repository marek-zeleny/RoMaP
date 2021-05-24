
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
            this.groupBoxRoad.SuspendLayout();
            this.groupBoxCrossroad.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxRoad
            // 
            this.groupBoxRoad.Controls.Add(this.chartAverageSpeed);
            this.groupBoxRoad.Controls.Add(this.labelMaxSpeed);
            this.groupBoxRoad.Controls.Add(this.labelTo);
            this.groupBoxRoad.Controls.Add(this.labelFrom);
            this.groupBoxRoad.Controls.Add(this.labelTwoWayRoad);
            this.groupBoxRoad.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxRoad.Location = new System.Drawing.Point(0, 0);
            this.groupBoxRoad.Name = "groupBoxRoad";
            this.groupBoxRoad.Size = new System.Drawing.Size(373, 246);
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
            this.groupBoxCrossroad.Location = new System.Drawing.Point(0, 246);
            this.groupBoxCrossroad.Name = "groupBoxCrossroad";
            this.groupBoxCrossroad.Size = new System.Drawing.Size(373, 82);
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
            // chartAverageSpeed
            // 
            this.chartAverageSpeed.Caption = "Average speed";
            this.chartAverageSpeed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartAverageSpeed.Location = new System.Drawing.Point(3, 79);
            this.chartAverageSpeed.MaxValue = 130D;
            this.chartAverageSpeed.MinValue = 0D;
            this.chartAverageSpeed.Mode = RoadTrafficSimulator.Forms.Chart<RoadTrafficSimulator.Components.Road.Statistics.Throughput>.RangeMode.Fixed;
            this.chartAverageSpeed.Name = "chartAverageSpeed";
            this.chartAverageSpeed.Size = new System.Drawing.Size(367, 164);
            this.chartAverageSpeed.TabIndex = 1;
            this.chartAverageSpeed.TimeRepresentation = RoadTrafficSimulator.Forms.Chart<RoadTrafficSimulator.Components.Road.Statistics.Throughput>.TimeUnit.Minute;
            this.chartAverageSpeed.ValueUnit = "km/h";
            // 
            // SimulationPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxCrossroad);
            this.Controls.Add(this.groupBoxRoad);
            this.Name = "SimulationPanel";
            this.Size = new System.Drawing.Size(373, 375);
            this.groupBoxRoad.ResumeLayout(false);
            this.groupBoxRoad.PerformLayout();
            this.groupBoxCrossroad.ResumeLayout(false);
            this.groupBoxCrossroad.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxRoad;
        private System.Windows.Forms.Label labelTo;
        private System.Windows.Forms.Label labelFrom;
        private System.Windows.Forms.Label labelTwoWayRoad;
        private System.Windows.Forms.Label labelMaxSpeed;
        private ChartThroughput chartThroughput1;
        private ChartThroughput chartThroughput2;
        private ChartThroughput chartThroughput3;
        private ChartThroughput chartThroughput4;
        private ChartThroughput chartThroughput5;
        private ChartThroughput chartThroughput6;
        private ChartThroughput chartAverageSpeed;
        private System.Windows.Forms.GroupBox groupBoxCrossroad;
        private System.Windows.Forms.Label labelCarSpawnRate;
        private System.Windows.Forms.Label labelOutIndex;
        private System.Windows.Forms.Label labelInIndex;
        private System.Windows.Forms.Label labelCoords;
        private ChartThroughput chartAverageSpe;
    }
}
