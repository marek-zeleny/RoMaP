using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

using RoadTrafficSimulator.ValueTypes;
using RoadTrafficSimulator.Components;
using RoadTrafficSimulator.Statistics;

namespace RoadTrafficSimulator.Forms
{
    public partial class SimulationPanel : UserControl
    {
        Chart<Road.Throughput, Road.IRoadStatistics> chartAverageSpeed;

        public SimulationPanel()
        {
            InitializeComponent();
            InitialiseChart();
            if (!DesignMode)
                Deselect();
        }

        [Browsable(true)]
        [Category("Action")]
        [Description("Occurs when the Build Map button is clicked.")]
        public event EventHandler BuildMapClicked
        {
            add => buttonBuild.Click += value;
            remove => buttonBuild.Click -= value;
        }

        internal void SelectCrossroad(CrossroadView crossroad)
        {
            SuspendLayout();
            labelCoords.Text = $"Coords: {crossroad.Coords}";
            labelInIndex.Text = $"Incoming roads: {crossroad.InIndex}";
            labelOutIndex.Text = $"Outcoming roads: {crossroad.OutIndex}";
            labelCarSpawnRate.Text = $"Car spawn rate: {crossroad.CarSpawnRate} %";
            groupBoxCrossroad.Visible = true;
            ResumeLayout();
        }

        internal void SelectRoad(RoadView road, IClock clock)
        {
            static IReadOnlyList<Timestamp<Road.Throughput>> GetThroughput(Road.IRoadStatistics stats) =>
                stats.ThroughputLog;

            SuspendLayout();
            labelTwoWayRoad.Text = road.TwoWayRoad ? "Two-way" : "One-way";
            labelFrom.Text = $"From: {road.From}";
            labelTo.Text = $"To: {road.To}";
            labelMaxSpeed.Text = $"Max speed: {road.MaxSpeed.ToKilometresPerHour()} km/h";
            chartAverageSpeed.SetDataSource(road.Statistics, GetThroughput, clock);
            chartAverageSpeed.MaxValue = road.MaxSpeed.ToKilometresPerHour();
            groupBoxRoad.Visible = true;
            ResumeLayout();
        }

        internal void Deselect()
        {
            SuspendLayout();
            groupBoxCrossroad.Visible = false;
            groupBoxRoad.Visible = false;
            ResumeLayout();
        }

        internal void UpdateChart()
        {
            if (chartAverageSpeed.Visible)
                chartAverageSpeed.UpdateChart();
        }

        private void InitialiseChart()
        {
            static double GetAverageSpeed(Road.Throughput throughput) => throughput.averageSpeed;

            chartAverageSpeed = new Chart<Road.Throughput, Road.IRoadStatistics>(GetAverageSpeed)
            {
                Name = nameof(chartAverageSpeed),
                Caption = "Average speed",
                TimeRepresentation = Chart<Road.Throughput, Road.IRoadStatistics>.TimeUnit.Minute,
                TimeSpan = 10.Hours(),
                Mode = Chart<Road.Throughput, Road.IRoadStatistics>.RangeMode.Fixed,
                MinValue = 0,
                ValueUnit = "km/h",
                TabIndex = 0,
                TabStop = false
            };
            groupBoxRoad.Controls.Add(chartAverageSpeed);
            chartAverageSpeed.Dock = DockStyle.Fill;
            chartAverageSpeed.BringToFront();
        }
    }
}
