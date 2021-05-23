using System;
using System.Windows.Forms;

using RoadTrafficSimulator.ValueTypes;

namespace RoadTrafficSimulator.Forms
{
    public partial class FormSimulationSettings : Form
    {
        private enum SpawnRateDetail
        {
            Low = 6,
            Medium = 12,
            High = 24
        }

        private static readonly int[] durationTable = new int[]
        {
            1, 2, 3, 4, 5, 6,
            8, 10, 12,
            16, 20, 24,
            36, 48,
            72, 96, 120, 144, 168,
            336
        };

        private TrackBar[] spawnRateBars;

        internal SimulationSettings Settings { get; private set; }

        public FormSimulationSettings()
        {
            InitializeComponent();
            // Initialize comboBoxMode
            foreach (string name in Enum.GetNames<SpawnRateDetail>())
                comboBoxSpawnRateDetail.Items.Add(name.Replace('_', ' '));
            comboBoxSpawnRateDetail.SelectedIndex = 0; // Triggers comboBoxSpawnRateDetail_SelectedIndexChanged()
        }

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

        private void comboBoxSpawnRateDetail_SelectedIndexChanged(object sender, EventArgs e)
        {
            SpawnRateDetail detail = Enum.Parse<SpawnRateDetail>(comboBoxSpawnRateDetail.Text.Replace(' ', '_'));
            ShowSpawnRateCombos(detail);
        }

        private void ShowSpawnRateCombos(SpawnRateDetail detail)
        {
            const int maxValue = 10;
            spawnRateBars = new TrackBar[(int)detail];
            for (int i = 0; i < spawnRateBars.Length; i++)
            {
                TrackBar bar = new()
                {
                    Orientation = Orientation.Vertical,
                    Dock = DockStyle.Left,
                    Minimum = 1,
                    Maximum = maxValue,
                    SmallChange = 1,
                    LargeChange = 2,
                    Value = maxValue / 2
                };
                spawnRateBars[i] = bar;
            }

            SuspendLayout();
            panelSpawnRate.Controls.Clear();
            panelSpawnRate.Controls.AddRange(spawnRateBars);
            ResumeLayout();
        }

        private void FillSettings()
        {
            Time duration = durationTable[trackBarDuration.Value].Hours();
            float navigationRate = trackBarNavigation.Value / 100f;
            float[] spawnRateDistribution = new float[spawnRateBars.Length];
            for (int i = 0; i < spawnRateDistribution.Length; i++)
            {
                // Reverse order because of reversed docking order of spawnRateBars in the form
                TrackBar bar = spawnRateBars[^(i + 1)];
                spawnRateDistribution[i] = (float)bar.Value / bar.Maximum;
            }
            Settings = new SimulationSettings(duration, navigationRate, spawnRateDistribution);
        }
    }
}
