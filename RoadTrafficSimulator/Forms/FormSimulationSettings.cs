using System;
using System.Windows.Forms;

using RoadTrafficSimulator.ValueTypes;
using RoadTrafficSimulator.Statistics;

namespace RoadTrafficSimulator.Forms
{
    /// <summary>
    /// Represents a form for filling in simulation settings.
    /// </summary>
    public partial class FormSimulationSettings : Form
    {
        /// <summary>
        /// Level of detail (granularity) for spawn frequency distribution
        /// </summary>
        private enum SpawnFrequencyDetail
        {
            Low = 6,
            Medium = 12,
            High = 24,
        }

        /// <summary>
        /// Quotient for converting a value from the frequency track bar to car spawn frequency in cars per crossroad
        /// per second
        /// </summary>
        private const float carFrequencyQuotient = 0.3f;
        /// <summary>
        /// Table for converting a track bar value to simulation duration (in hours)
        /// </summary>
        private static readonly int[] durationTable = new int[]
        {
            1, 2, 3, 4, 5, 6,
            8, 10, 12,
            16, 20, 24,
            36, 48,
            72, 96, 120, 144, 168,
            336,
        };

        private TrackBar[] spawnFrequencyBars;

        /// <summary>
        /// Simulation settings collected by the form
        /// </summary>
        internal SimulationSettings Settings { get; private set; }

        /// <summary>
        /// Creates a new form for simulation settings.
        /// </summary>
        public FormSimulationSettings()
        {
            InitializeComponent();
            // Initialise comboBoxDetailLevel
            foreach (string name in Enum.GetNames<StatisticsBase.DetailLevel>())
                comboBoxStatisticsDetail.Items.Add(name.Replace('_', ' '));
            // Triggers comboBoxStatisticsDetail_SelectedIndexChanged()
            comboBoxStatisticsDetail.SelectedIndex = (int)StatisticsBase.detailSetting;
            // Initialise comboBoxMode
            foreach (string name in Enum.GetNames<SpawnFrequencyDetail>())
                comboBoxSpawnFrequencyDetail.Items.Add(name.Replace('_', ' '));
            // Triggers comboBoxSpawnFrequencyDetail_SelectedIndexChanged()
            comboBoxSpawnFrequencyDetail.SelectedIndex = 0;
        }

        #region form_events

        private void buttonSimulate_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            FillSettings();
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void trackBarDuration_Scroll(object sender, EventArgs e)
        {
            string textRepr;
            Time duration = durationTable[trackBarDuration.Value].Hours();
            if (duration < 24.Hours())
                textRepr = $"{duration.ToHours()}h";
            else
                textRepr = $"{duration.ToDays()}d";
            labelDuration.Text = $"Duration: {textRepr}";
        }

        private void trackBarNavigation_Scroll(object sender, EventArgs e)
        {
            labelNavigation.Text = $"Navigation rate: {trackBarNavigation.Value} %";
        }

        private void trackBarSpawnFrequency_Scroll(object sender, EventArgs e)
        {
            labelSpawnFrequency.Text = $"Car spawn frequency: {trackBarSpawnFrequency.Value} %";
        }

        private void comboBoxSpawnFrequencyDetail_SelectedIndexChanged(object sender, EventArgs e)
        {
            string text = comboBoxSpawnFrequencyDetail.Text.Replace(' ', '_');
            SpawnFrequencyDetail detail = Enum.Parse<SpawnFrequencyDetail>(text);
            ShowSpawnFrequencyTrackBars((int)detail);
        }

        #endregion form_events

        /// <summary>
        /// Creates spawn frequency track bars and shows them in the spawn frequency panel.
        /// </summary>
        /// <param name="count">Number of track bars created</param>
        private void ShowSpawnFrequencyTrackBars(int count)
        {
            const int maxValue = 10;
            spawnFrequencyBars = new TrackBar[count];
            for (int i = 0; i < spawnFrequencyBars.Length; i++)
            {
                TrackBar bar = new()
                {
                    Orientation = Orientation.Vertical,
                    Dock = DockStyle.Left,
                    Minimum = 1,
                    Maximum = maxValue,
                    SmallChange = 1,
                    LargeChange = 2,
                    Value = maxValue / 2,
                };
                spawnFrequencyBars[i] = bar;
            }

            SuspendLayout();
            panelSpawnFrequency.Controls.Clear();
            panelSpawnFrequency.Controls.AddRange(spawnFrequencyBars);
            ResumeLayout();
        }

        /// <summary>
        /// Fills the simulation settings based on currently selected values in the form.
        /// </summary>
        private void FillSettings()
        {
            string statsDetailText = comboBoxStatisticsDetail.Text.Replace(' ', '_');
            StatisticsBase.DetailLevel statsDetail = Enum.Parse<StatisticsBase.DetailLevel>(statsDetailText);
            Time duration = durationTable[trackBarDuration.Value].Hours();
            float navigationRate = (float)trackBarNavigation.Value / trackBarNavigation.Maximum;
            float spawnFrequency = carFrequencyQuotient * trackBarSpawnFrequency.Value / trackBarSpawnFrequency.Maximum;
            float[] spawnFrequencyDistribution = new float[spawnFrequencyBars.Length];
            for (int i = 0; i < spawnFrequencyDistribution.Length; i++)
            {
                // Reverse order because of reversed docking order of spawnFrequencyBars in the form
                TrackBar bar = spawnFrequencyBars[^(i + 1)];
                spawnFrequencyDistribution[i] = ((float)bar.Value / bar.Maximum);
            }
            Settings = new SimulationSettings(duration, navigationRate, statsDetail, spawnFrequency,
                spawnFrequencyDistribution);
        }
    }
}
