using System;
using System.Windows.Forms;

using RoadTrafficSimulator.ValueTypes;

namespace RoadTrafficSimulator.Forms
{
    public partial class SimulationPanel : UserControl
    {
        public SimulationPanel()
        {
            InitializeComponent();
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
    }
}
