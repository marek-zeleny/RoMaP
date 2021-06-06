﻿using System;
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
            DoubleBuffered = true;
            InitialiseChart();
            if (!DesignMode)
                Deselect();
        }

        internal void SelectCrossroad(Crossroad crossroad)
        {
            SuspendLayout();
            labelCoords.Text = $"Coords: {crossroad.Id}";
            labelInIndex.Text = $"Incoming roads: {crossroad.InDegree}";
            labelOutIndex.Text = $"Outcoming roads: {crossroad.OutDegree}";
            labelCarSpawnRate.Text = $"Car spawn rate: {crossroad.CarSpawnRate} %";
            groupBoxCrossroad.Visible = true;
            ResumeLayout();
        }

        internal void SelectRoad(GUI.IGRoad gRoad, IClock clock)
        {
            static IReadOnlyList<Timestamp<Road.Throughput>> GetThroughput(Road.IRoadStatistics stats) =>
                stats.ThroughputLog;

            Road road = gRoad.GetRoad();
            SuspendLayout();
            labelTwoWayRoad.Text = gRoad.IsTwoWay ? "Two-way" : "One-way";
            labelFrom.Text = $"From: {gRoad.From}";
            labelTo.Text = $"To: {gRoad.To}";
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
            static double GetAverageSpeed(Road.Throughput throughput) => throughput.averageSpeed.ToKilometresPerHour();

            chartAverageSpeed = new Chart<Road.Throughput, Road.IRoadStatistics>(GetAverageSpeed)
            {
                Name = nameof(chartAverageSpeed),
                Caption = "Average speed",
                TimeRepresentation = Chart<Road.Throughput, Road.IRoadStatistics>.TimeUnit.Minute,
                TimeSpan = 20.Minutes(),
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
