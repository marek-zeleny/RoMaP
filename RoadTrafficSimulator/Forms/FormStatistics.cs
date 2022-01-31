using System;
using System.Collections.Generic;
using System.Windows.Forms;

using RoadTrafficSimulator.Statistics;
using RoadTrafficSimulator.ValueTypes;

namespace RoadTrafficSimulator.Forms
{
    public partial class FormStatistics : Form
    {
        private Chart<Simulation.StatsData, Simulation.IGlobalStatistics> chartActiveCars;
        private Chart<Simulation.StatsData, Simulation.IGlobalStatistics> chartCarsWithZeroSpeed;
        private Chart<Simulation.StatsData, Simulation.IGlobalStatistics> chartAverageSpeed;
        private Chart<Simulation.StatsData, Simulation.IGlobalStatistics> chartAverageDelay;

        internal FormStatistics()
        {
            static double GetActiveCars(Simulation.StatsData data) => data.carsActive;
            static double GetCarsWithZeroSpeed(Simulation.StatsData data) => data.carsWithZeroSpeed;
            static double GetAverageSpeed(Simulation.StatsData data) => data.averageSpeed.ToKilometresPerHour();
            static double GetAverageDelay(Simulation.StatsData data) => data.averageDelay.ToMinutes();

            InitializeComponent();

            InitialiseChart(ref chartActiveCars, 0, 0, nameof(chartActiveCars),
                "Active cars", "cars", GetActiveCars);
            InitialiseChart(ref chartCarsWithZeroSpeed, 0, 1, nameof(chartCarsWithZeroSpeed),
                "Cars in a traffic jam (speed 0 km/h)", "cars", GetCarsWithZeroSpeed);
            InitialiseChart(ref chartAverageSpeed, 1, 0, nameof(chartAverageSpeed),
                "Average speed", "km/h", GetAverageSpeed);
            InitialiseChart(ref chartAverageDelay, 1, 1, nameof(chartAverageDelay),
                "Average delay (among finished cars)", "minutes", GetAverageDelay);
        }

        internal void SetDataSource(Simulation.IGlobalStatistics statistics, IClock clock)
        {
            static IReadOnlyList<Timestamp<Simulation.StatsData>> GetData(Simulation.IGlobalStatistics stats) =>
                stats.DataLog;

            chartActiveCars.SetDataSource(statistics, GetData, clock);
            chartCarsWithZeroSpeed.SetDataSource(statistics, GetData, clock);
            chartAverageSpeed.SetDataSource(statistics, GetData, clock);
            chartAverageDelay.SetDataSource(statistics, GetData, clock);
        }

        private void InitialiseChart(ref Chart<Simulation.StatsData, Simulation.IGlobalStatistics> chart,
            int col, int row, string name, string caption, string unit, Func<Simulation.StatsData, double> dataToDouble)
        {
            chart = new Chart<Simulation.StatsData, Simulation.IGlobalStatistics>(dataToDouble)
            {
                Name = name,
                Caption = caption,
                TimeRepresentation = ChartTimeUnit.Minute,
                TimeSpan = 20.Minutes(),
                Mode = ChartRangeMode.FixedMin,
                MinValue = 0,
                ValueUnit = unit,
                TabIndex = 0,
                TabStop = false,
            };
            tableLayoutPanel.Controls.Add(chart, col, row);
            chart.Dock = DockStyle.Fill;
        }
    }
}
