using System;
using System.Windows.Forms;

using RoadTrafficSimulator.ValueTypes;
using RoadTrafficSimulator.Components;

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

        internal void Select(CrossroadView crossroad)
        {
            labelCoords.Text = $"Coords: {crossroad.Coords}";
            labelInIndex.Text = $"Incoming roads: {crossroad.InIndex}";
            labelOutIndex.Text = $"Outcoming roads: {crossroad.OutIndex}";
            labelCarSpawnRate.Text = $"Car spawn rate: {crossroad.CarSpawnRate} %";
            groupBoxCrossroad.Visible = true;
        }

        internal void Select(RoadView road)
        {
            labelTwoWayRoad.Text = road.TwoWayRoad ? "Two-way" : "One-way";
            labelFrom.Text = $"From: {road.From}";
            labelTo.Text = $"To: {road.To}";
            labelMaxSpeed.Text = $"Max speed: {road.MaxSpeed.ToKilometresPerHour()} km/h";
            // TODO: Set chart data source
            chartAverageSpeed.MaxValue = road.MaxSpeed.ToKilometresPerHour();
            groupBoxRoad.Visible = true;
        }

        internal void Deselect()
        {
            groupBoxCrossroad.Visible = false;
            groupBoxRoad.Visible = false;
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
                Name = "chartAverageSpeed",
                Caption = "Average speed",
                TimeRepresentation = Chart<Road.Throughput, Road.IRoadStatistics>.TimeUnit.Minute,
                TimeSpan = 10.Hours(),
                ValueUnit = "km/h",
                Dock = DockStyle.Fill,
                TabIndex = 0,
                TabStop = false
            };
            groupBoxRoad.Controls.Add(chartAverageSpeed);
        }
    }
}
