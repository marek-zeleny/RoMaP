using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

using RoadTrafficSimulator.ValueTypes;

namespace RoadTrafficSimulator.Forms
{
    public partial class BuildPanel : UserControl
    {
        public enum Mode { Build, Select };

        private Mode mode;
        private bool lockMode;
        private bool lockMaxSpeed;

        public Mode CurrentMode
        {
            get => mode;
            set
            {
                mode = value;
                if (!lockMode)
                {
                    lockMode = true;
                    comboBoxMode.SelectedIndex = comboBoxMode.FindStringExact(value.ToString().Replace('_', ' '));
                    lockMode = false;
                }
                CurrentModeChanged?.Invoke(this, new EventArgs());
            }
        }
        public bool TwoWayRoad { get => checkBoxTwoWayRoad.Checked; }
        public int MaxSpeed
        {
            get => (int)numericUpDownMaxSpeed.Value;
            set
            {
                lockMaxSpeed = true;
                numericUpDownMaxSpeed.Value = value;
                lockMaxSpeed = false;
            }
        }
        public int SpawnRate { get => trackBarCarSpawnRate.Value; }

        public BuildPanel()
        {
            InitializeComponent();
            // Initialize comboBoxMode
            foreach (string name in Enum.GetNames<Mode>())
                comboBoxMode.Items.Add(name.Replace('_', ' '));
            comboBoxMode.SelectedIndex = 0; // Triggers comboBoxMode_SelectedIndexChanged()
        }

        #region events

        [Browsable(true)]
        [Category("Action")]
        [Description("Occurs when the Destroy Crossroad button is clicked.")]
        public event EventHandler DestroyRoadClick
        {
            add => buttonDestroyRoad.Click += value;
            remove => buttonDestroyRoad.Click -= value;
        }

        [Browsable(true)]
        [Category("Action")]
        [Description("Occurs when the Traffic Light button is clicked.")]
        public event EventHandler TrafficLightClick
        {
            add => buttonTrafficLight.Click += value;
            remove => buttonTrafficLight.Click -= value;
        }

        [Browsable(true)]
        [Category("Action")]
        [Description("Occurs when the Destroy Crossroad button is clicked.")]
        public event EventHandler DestroyCrossroadClick
        {
            add => buttonDestroyCrossroad.Click += value;
            remove => buttonDestroyCrossroad.Click -= value;
        }

        [Browsable(true)]
        [Category("Action")]
        [Description("Occurs when the Save Map button is clicked.")]
        public event EventHandler SaveMapClick
        {
            add => buttonSaveMap.Click += value;
            remove => buttonSaveMap.Click -= value;
        }

        [Browsable(true)]
        [Category("Action")]
        [Description("Occurs when the Load Map button is clicked.")]
        public event EventHandler LoadMapClick
        {
            add => buttonLoadMap.Click += value;
            remove => buttonLoadMap.Click -= value;
        }

        [Browsable(true)]
        [Category("Property Changed")]
        [Description("Occurs when the CurrentMode property is changed.")]
        public event EventHandler CurrentModeChanged;

        [Browsable(true)]
        [Category("Property Changed")]
        [Description("Occurs when the TwoWayRoad property is changed.")]
        public event EventHandler TwoWayRoadChanged
        {
            add => checkBoxTwoWayRoad.CheckedChanged += value;
            remove => checkBoxTwoWayRoad.CheckedChanged -= value;
        }

        [Browsable(true)]
        [Category("Property Changed")]
        [Description("Occurs when the MaxSpeed property is changed.")]
        public event EventHandler MaxSpeedChanged;

        [Browsable(true)]
        [Category("Property Changed")]
        [Description("Occurs when the SpawnRate property is changed.")]
        public event EventHandler SpawnRateChanged;

        #endregion events

        internal void SelectCrossroad(CrossroadView crossroad)
        {
            Debug.Assert(CurrentMode == Mode.Select);
            SuspendLayout();
            labelCoords.Text = $"Coords: {crossroad.Coords}";
            labelInIndex.Text = $"Incoming roads: {crossroad.InIndex}";
            labelOutIndex.Text = $"Outcoming roads: {crossroad.OutIndex}";
            labelCarSpawnRate.Text = $"Car spawn rate: {crossroad.CarSpawnRate} %";
            trackBarCarSpawnRate.Value = crossroad.CarSpawnRate;
            groupBoxCrossroad.Visible = true;
            ResumeLayout();
        }

        internal void SelectRoad(RoadView road)
        {
            Debug.Assert(CurrentMode == Mode.Select);
            SuspendLayout();
            labelTwoWayRoad.Text = road.TwoWayRoad ? "Two-way" : "One-way";
            labelFrom.Text = $"From: {road.From}";
            labelTo.Text = $"To: {road.To}";
            lockMaxSpeed = true;
            numericUpDownMaxSpeed.Value = road.MaxSpeed.ToKilometresPerHour();
            lockMaxSpeed = false;
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

        private void numericUpDownMaxSpeed_ValueChanged(object sender, EventArgs e)
        {
            if (!lockMaxSpeed)
                MaxSpeedChanged?.Invoke(sender, e);
        }

        private void trackBarCarSpawnRate_Scroll(object sender, EventArgs e)
        {
            labelCarSpawnRate.Text = $"Car spawn rate: {trackBarCarSpawnRate.Value} %";
            SpawnRateChanged?.Invoke(sender, e);
        }

        private void comboBoxMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            Mode newMode = Enum.Parse<Mode>(comboBoxMode.Text.Replace(' ', '_'));
            switch (newMode)
            {
                case Mode.Build:
                    Deselect();
                    groupBoxBuild.Visible = true;
                    break;
                case Mode.Select:
                    groupBoxBuild.Visible = false;
                    break;
                default:
                    break;
            }
            if (!lockMode)
            {
                lockMode = true;
                CurrentMode = newMode;
                lockMode = false;
            }
        }
    }
}
