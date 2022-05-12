using System;
using System.Collections.Generic;
using System.Windows.Forms;

using RoadTrafficSimulator.Statistics;
using RoadTrafficSimulator.ValueTypes;

namespace RoadTrafficSimulator.Forms
{
    /// <summary>
    /// Represents a form for showing statistics of a currently running simulation.
    /// </summary>
    public partial class FormStatistics : Form
    {
        /// <summary>
        /// Defines available time spans for displayed charts
        /// </summary>
        private static readonly (Time time, string text)[] timeSpans =
        {
            (5.Minutes(), "5 minutes"),
            (20.Minutes(), "20 minutes"),
            (1.Hours(), "1 hour"),
            (6.Hours(), "6 hours"),
            (1.Days(), "1 day"),
            (7.Days(), "1 week"),
        };

        Simulation.IGlobalStatistics statistics;
        private Chart<Simulation.StatsData, Simulation.IGlobalStatistics> chartActiveCars;
        private Chart<Simulation.StatsData, Simulation.IGlobalStatistics> chartCarsWithZeroSpeed;
        private Chart<Simulation.StatsData, Simulation.IGlobalStatistics> chartAverageSpeed;
        private Chart<Simulation.StatsData, Simulation.IGlobalStatistics> chartAverageDelay;
        private readonly Chart<Simulation.StatsData, Simulation.IGlobalStatistics>[] charts;

        /// <summary>
        /// Creates a new form for displaying statistics.
        /// </summary>
        internal FormStatistics()
        {
            InitializeComponent();
            InitialiseCharts();
            charts = new Chart<Simulation.StatsData, Simulation.IGlobalStatistics>[] {
                chartActiveCars,
                chartCarsWithZeroSpeed,
                chartAverageSpeed,
                chartAverageDelay,
            };
            // Initialise comboBoxTimeSpan
            foreach (var (_, text) in timeSpans)
                comboBoxTimeSpan.Items.Add(text);
            comboBoxTimeSpan.SelectedIndex = 1; // Default 20 minutes; triggers comboBoxTimeSpan_SelectedIndexChanged()
        }

        /// <summary>
        /// Ensures that all statistical data and charts are updated.
        /// </summary>
        public void UpdateStatistics()
        {
            if (Visible)
            {
                UpdateStats();
                foreach (var chart in charts)
                    chart.UpdateChart();
            }
        }

        /// <summary>
        /// Sets a new data source for global statistics.
        /// </summary>
        /// <param name="clock">Global clock measuring the simulation time</param>
        internal void SetDataSource(Simulation.IGlobalStatistics statistics, IClock clock)
        {
            static IReadOnlyList<Timestamp<Simulation.StatsData>> GetData(Simulation.IGlobalStatistics stats) =>
                stats.DataLog;

            this.statistics = statistics;
            foreach (var chart in charts)
                chart.SetDataSource(statistics, GetData, clock);
        }

        /// <summary>
        /// Updates statistical data.
        /// </summary>
        private void UpdateStats()
        {
            string totalCars = "-";
            string activeCars = "-";
            string finishedCars = "-";
            string stationaryCars = "-";
            string averageSpeed = "-";
            string averageDelay = "-";

            if (statistics != null)
            {
                totalCars = statistics.CarsTotal.ToString();
                activeCars = statistics.CarsActive.ToString();
                finishedCars = statistics.CarsFinished.ToString();
                stationaryCars = statistics.CarsWithZeroSpeed.ToString();
                averageSpeed = statistics.AverageSpeed.ToKilometresPerHour().ToString();
                averageDelay = statistics.AverageDelay.ToMinutes().ToString();
            }

            labelTotalCars.Text = $"Total cars: {totalCars}";
            labelActiveCars.Text = $"Active cars: {activeCars}";
            labelFinishedCars.Text = $"Finished cars: {finishedCars}";
            labelStationaryCars.Text = $"Stationary cars: {stationaryCars}";
            labelAverageSpeed.Text = $"Average speed: {averageSpeed} km/h";
            labelAverageDelay.Text = $"Average delay: {averageDelay} min";
        }

        /// <summary>
        /// Initialises all charts.
        /// </summary>
        private void InitialiseCharts()
        {
            static double GetActiveCars(Simulation.StatsData data) => data.carsActive;
            static double GetCarsWithZeroSpeed(Simulation.StatsData data) => data.carsWithZeroSpeed;
            static double GetAverageSpeed(Simulation.StatsData data) => data.averageSpeed.ToKilometresPerHour();
            static double GetAverageDelay(Simulation.StatsData data) => data.averageDelay.ToMinutes();

            InitialiseChart(ref chartActiveCars, 0, 0, nameof(chartActiveCars),
                "Active cars", "cars", GetActiveCars);
            InitialiseChart(ref chartCarsWithZeroSpeed, 0, 1, nameof(chartCarsWithZeroSpeed),
                "Cars in a traffic jam (speed 0 km/h)", "cars", GetCarsWithZeroSpeed);
            InitialiseChart(ref chartAverageSpeed, 1, 0, nameof(chartAverageSpeed),
                "Average speed", "km/h", GetAverageSpeed);
            InitialiseChart(ref chartAverageDelay, 1, 1, nameof(chartAverageDelay),
                "Average delay (among finished cars)", "min", GetAverageDelay);
        }

        /// <summary>
        /// Initialises a given chart using given parameters.
        /// </summary>
        /// <param name="col">Column in the chart table where the chart will be placed</param>
        /// <param name="row">Row in the chart table where the chart will be placed</param>
        /// <param name="name">Name of the control (not displayed anywhere)</param>
        /// <param name="caption">Caption of the chart (displayed)</param>
        /// <param name="unit">Unit of values (Y axis) of the chart</param>
        /// <param name="dataToDouble">Function for extracting values from global statistical data</param>
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

        private void comboBoxTimeSpan_SelectedIndexChanged(object sender, EventArgs e)
        {
            Time timeSpan = timeSpans[comboBoxTimeSpan.SelectedIndex].time;
            foreach (var chart in charts)
                chart.TimeSpan = timeSpan;
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FormStatistics_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Prevents destroying the form when closing, only hides it instead to enable reopening it again
            e.Cancel = true;
            Hide();
        }

        private void FormStatistics_VisibleChanged(object sender, EventArgs e)
        {
            UpdateStatistics();
        }
    }
}
