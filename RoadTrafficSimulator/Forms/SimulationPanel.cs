using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

using RoadTrafficSimulator.ValueTypes;
using RoadTrafficSimulator.Components;
using RoadTrafficSimulator.Statistics;

namespace RoadTrafficSimulator.Forms
{
    public partial class SimulationPanel : UserControl
    {
        private static readonly int[] simulationSpeedTable = new int[]
        {
            1,
            2,
            5,
            10,
            20,
            50,
            100,
            200,
            500,
            1000,
        };

        private static string TimeToString(Time time)
        {
            int days = time.ToDays();
            time -= days.Days();
            int hours = time.ToHours();
            Debug.Assert(hours < 24);
            time -= hours.Hours();
            int minutes = time.ToMinutes();
            Debug.Assert(minutes < 60);
            time -= minutes.Minutes();
            int seconds = time.ToSeconds();
            Debug.Assert(seconds < 60);
            return $"{days}d {hours:00}:{minutes:00}:{seconds:00}";
        }

        private Chart<Road.Throughput, Road.IRoadStatistics> chartAverageSpeed;
        private Time simulationTime;

        public int SimulationSpeed { get => simulationSpeedTable[trackBarSimulationSpeed.Value]; }

        internal Time SimulationTime
        {
            get => simulationTime;
            set
            {
                simulationTime = value;
                labelSimulationTime.Text = $"Simulation time:\n{TimeToString(simulationTime)}";
            }
        }

        public SimulationPanel()
        {
            InitializeComponent();
            DoubleBuffered = true;
            trackBarSimulationSpeed.Maximum = simulationSpeedTable.Length - 1;
            InitialiseChart();
            if (!DesignMode)
                Deselect();
        }

        #region events

        [Browsable(true)]
        [Category("Action")]
        [Description("Occurs when the Statistics button is clicked.")]
        public event EventHandler StatisticsClick
        {
            add => buttonStatistics.Click += value;
            remove => buttonStatistics.Click -= value;
        }

        [Browsable(true)]
        [Category("Property Changed")]
        [Description("Occurs when the SimulationSpeed property is changed.")]
        public event EventHandler SimulationSpeedChanged;

        #endregion events

        private void trackBarSimulationSpeed_Scroll(object sender, EventArgs e)
        {
            labelSimulationSpeed.Text = $"Simulation speed: {SimulationSpeed}x";
            SimulationSpeedChanged?.Invoke(this, new EventArgs());
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
            labelLength.Text = $"Length: {road.Length.ToMetres()} m";
            labelMaxSpeed.Text = $"Max speed: {road.MaxSpeed.ToKilometresPerHour()} km/h";
            if (road.IsConnected)
            {
                chartAverageSpeed.SetDataSource(road.Statistics, GetThroughput, clock);
                chartAverageSpeed.MaxValue = road.MaxSpeed.ToKilometresPerHour();
            }
            else
            {
                chartAverageSpeed.ClearDataSource();
            }
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
                TimeRepresentation = ChartTimeUnit.Minute,
                TimeSpan = 20.Minutes(),
                Mode = ChartRangeMode.Fixed,
                MinValue = 0,
                ValueUnit = "km/h",
                TabIndex = 0,
                TabStop = false,
            };
            groupBoxRoad.Controls.Add(chartAverageSpeed);
            chartAverageSpeed.Dock = DockStyle.Fill;
            chartAverageSpeed.BringToFront();
        }
    }
}
