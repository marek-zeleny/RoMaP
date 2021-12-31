using System;
using System.Windows.Forms;

using RoadTrafficSimulator.ValueTypes;

namespace RoadTrafficSimulator.Forms
{
    public partial class FormSimulationSettings : Form
    {
        private enum SpawnFrequencyDetail
        {
            Low = 6,
            Medium = 12,
            High = 24
        }

        private const float carFrequencyQuotient = 0.03f;
        private static readonly int[] durationTable = new int[]
        {
            1, 2, 3, 4, 5, 6,
            8, 10, 12,
            16, 20, 24,
            36, 48,
            72, 96, 120, 144, 168,
            336
        };

        private TrackBar[] spawnFrequencyBars;

        internal SimulationSettings Settings { get; private set; }

        public FormSimulationSettings()
        {
            InitializeComponent();
            // Initialize comboBoxMode
            foreach (string name in Enum.GetNames<SpawnFrequencyDetail>())
                comboBoxSpawnFrequencyDetail.Items.Add(name.Replace('_', ' '));
            comboBoxSpawnFrequencyDetail.SelectedIndex = 0; // Triggers comboBoxSpawnFrequencyDetail_SelectedIndexChanged()
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

        private void comboBoxSpawnFrequencyDetail_SelectedIndexChanged(object sender, EventArgs e)
        {
            SpawnFrequencyDetail detail = Enum.Parse<SpawnFrequencyDetail>(comboBoxSpawnFrequencyDetail.Text.Replace(' ', '_'));
            ShowSpawnFrequencyCombos(detail);
        }

        private void ShowSpawnFrequencyCombos(SpawnFrequencyDetail detail)
        {
            const int maxValue = 10;
            spawnFrequencyBars = new TrackBar[(int)detail];
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
                    Value = maxValue / 2
                };
                spawnFrequencyBars[i] = bar;
            }

            SuspendLayout();
            panelSpawnFrequency.Controls.Clear();
            panelSpawnFrequency.Controls.AddRange(spawnFrequencyBars);
            ResumeLayout();
        }

        private void FillSettings()
        {
            Time duration = durationTable[trackBarDuration.Value].Hours();
            float navigationRate = trackBarNavigation.Value / 100f;
            float[] spawnFrequencyDistribution = new float[spawnFrequencyBars.Length];
            for (int i = 0; i < spawnFrequencyDistribution.Length; i++)
            {
                // Reverse order because of reversed docking order of spawnFrequencyBars in the form
                TrackBar bar = spawnFrequencyBars[^(i + 1)];
                spawnFrequencyDistribution[i] = ((float)bar.Value / bar.Maximum) * carFrequencyQuotient;
            }
            Settings = new SimulationSettings(duration, navigationRate, spawnFrequencyDistribution);
        }
    }
}
