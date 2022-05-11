using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

using RoadTrafficSimulator.ValueTypes;

namespace RoadTrafficSimulator.Forms
{
    /// <summary>
    /// Provides controls for building and modifying a map.
    /// </summary>
    public partial class BuildPanel : UserControl
    {
        /// <summary>
        /// Mode of the panel
        /// </summary>
        public enum Mode
        {
            /// <summary>
            /// Building new roads
            /// </summary>
            Build,
            /// <summary>
            /// Selecting and modifying existing roads and crossroads
            /// </summary>
            Select
        };

        private Mode mode;
        private bool lockMode;
        private bool lockLanes;
        private bool lockLength;
        private bool lockMaxSpeed;

        /// <summary>
        /// Side of driving on the map
        /// </summary>
        public GUI.RoadSide CurrentRoadSide
        {
            get
            {
                if (radioButtonDriveRight.Checked)
                    return GUI.RoadSide.Right;
                else
                    return GUI.RoadSide.Left;
            }
            set
            {
                switch (value)
                {
                    case GUI.RoadSide.Right:
                        radioButtonDriveRight.Checked = true;
                        break;
                    case GUI.RoadSide.Left:
                        radioButtonDriveLeft.Checked = true;
                        break;
                }
                CurrentRoadSideChanged?.Invoke(this, new EventArgs());
            }
        }
        /// <summary>
        /// Current mode of the panel
        /// </summary>
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
        /// <summary>
        /// <c>true</c> if the a newly built road should be two-way, otherwise <c>false</c>
        /// </summary>
        public bool TwoWayRoad { get => checkBoxTwoWayRoad.Checked; }
        /// <summary>
        /// Number of lanes of a selected road
        /// </summary>
        public int Lanes
        {
            get => (int)numericUpDownLanes.Value;
            set
            {
                lockLanes = true;
                numericUpDownLanes.Value = value;
                lockLanes = false;
            }
        }
        /// <summary>
        /// Length of a selected road
        /// </summary>
        public int Length
        {
            get => (int)numericUpDownLength.Value;
            set
            {
                lockLength = true;
                numericUpDownLength.Value = value;
                lockLength = false;
            }
        }
        /// <summary>
        /// Maximum speed of a selected road
        /// </summary>
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
            DoubleBuffered = true;
            // Initialize comboBoxMode
            foreach (string name in Enum.GetNames<Mode>())
                comboBoxMode.Items.Add(name.Replace('_', ' '));
            comboBoxMode.SelectedIndex = 0; // Triggers comboBoxMode_SelectedIndexChanged()
            radioButtonDriveRight.CheckedChanged += radioButtonsRoadSide_CheckedChanged;
            radioButtonDriveLeft.CheckedChanged += radioButtonsRoadSide_CheckedChanged;
        }

        #region events

        [Browsable(true)]
        [Category("Action")]
        [Description("Occurs when the Close Road button is clicked.")]
        public event EventHandler CloseRoadClick
        {
            add => buttonCloseRoad.Click += value;
            remove => buttonCloseRoad.Click -= value;
        }

        [Browsable(true)]
        [Category("Action")]
        [Description("Occurs when the Open Road button is clicked.")]
        public event EventHandler OpenRoadClick
        {
            add => buttonOpenRoad.Click += value;
            remove => buttonOpenRoad.Click -= value;
        }

        [Browsable(true)]
        [Category("Action")]
        [Description("Occurs when the Destroy Road button is clicked.")]
        public event EventHandler DestroyRoadClick
        {
            add => buttonDestroyRoad.Click += value;
            remove => buttonDestroyRoad.Click -= value;
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
        [Category("Action")]
        [Description("Occurs when the map on the traffic light panel is clicked.")]
        public event EventHandler TrafficLightMapClick
        {
            add => trafficLightPanel.MapClicked += value;
            remove => trafficLightPanel.MapClicked -= value;
        }

        [Browsable(true)]
        [Category("Property Changed")]
        [Description("Occurs when the RoadSide property is changed.")]
        public event EventHandler CurrentRoadSideChanged;

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
        [Description("Occurs when the Lanes property is changed.")]
        public event EventHandler LanesChanged;

        [Browsable(true)]
        [Category("Property Changed")]
        [Description("Occurs when the Length property is changed.")]
        public event EventHandler LengthChanged;

        [Browsable(true)]
        [Category("Property Changed")]
        [Description("Occurs when the MaxSpeed property is changed.")]
        public event EventHandler MaxSpeedChanged;

        [Browsable(true)]
        [Category("Property Changed")]
        [Description("Occurs when the SpawnRate property is changed.")]
        public event EventHandler SpawnRateChanged;

        #endregion events

        internal void Initialise(MapManager mapManager)
        {
            trafficLightPanel.Initialise(mapManager);
        }

        internal void SelectCrossroad(MapManager.CrossroadWrapper crossroad)
        {
            Debug.Assert(CurrentMode == Mode.Select);
            SuspendLayout();
            labelCoords.Text = $"Coords: {crossroad.crossroad.Id}";
            labelInIndex.Text = $"Incoming roads: {crossroad.crossroad.InDegree}";
            labelOutIndex.Text = $"Outcoming roads: {crossroad.crossroad.OutDegree}";
            labelCarSpawnRate.Text = $"Car spawn rate: {crossroad.crossroad.CarSpawnRate} %";
            trackBarCarSpawnRate.Value = crossroad.crossroad.CarSpawnRate;

            trafficLightPanel.Activate(crossroad);
            groupBoxCrossroad.Visible = true;
            ResumeLayout();
        }

        internal void SelectRoad(GUI.IGRoad gRoad)
        {
            Debug.Assert(CurrentMode == Mode.Select);
            SuspendLayout();
            labelTwoWayRoad.Text = gRoad.IsTwoWay ? "Two-way" : "One-way";
            labelFrom.Text = $"From: {gRoad.From}";
            labelTo.Text = $"To: {gRoad.To}";
            lockLanes = true;
            numericUpDownLanes.Value = gRoad.GetRoad().LaneCount;
            lockLanes = false;
            lockLength = true;
            numericUpDownLength.Value = gRoad.GetRoad().Length.ToMetres();
            lockLength = false;
            lockMaxSpeed = true;
            numericUpDownMaxSpeed.Value = gRoad.GetRoad().MaxSpeed.ToKilometresPerHour();
            lockMaxSpeed = false;
            groupBoxRoad.Visible = true;
            if (gRoad.GetRoad().IsConnected)
            {
                numericUpDownLanes.Enabled = true;
                numericUpDownLength.Enabled = true;
                numericUpDownMaxSpeed.Enabled = true;
                buttonCloseRoad.Visible = true;
                buttonOpenRoad.Visible = false;
            }
            else
            {
                numericUpDownLanes.Enabled = false;
                numericUpDownLength.Enabled = false;
                numericUpDownMaxSpeed.Enabled = false;
                buttonCloseRoad.Visible = false;
                buttonOpenRoad.Visible = true;
            }
            ResumeLayout();
        }

        internal void Deselect()
        {
            SuspendLayout();
            groupBoxCrossroad.Visible = false;
            groupBoxRoad.Visible = false;
            trafficLightPanel.Deactivate();
            ResumeLayout();
        }

        private void radioButtonsRoadSide_CheckedChanged(object sender, EventArgs e)
        {
            CurrentRoadSideChanged?.Invoke(sender, e);
        }

        private void numericUpDownLanes_ValueChanged(object sender, EventArgs e)
        {
            if (!lockLanes)
                LanesChanged?.Invoke(sender, e);
        }

        private void numericUpDownLength_ValueChanged(object sender, EventArgs e)
        {
            if (!lockLength)
                LengthChanged?.Invoke(sender, e);
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
            Deselect();
            Mode newMode = Enum.Parse<Mode>(comboBoxMode.Text.Replace(' ', '_'));
            switch (newMode)
            {
                case Mode.Build:
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
