using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

using RoadTrafficSimulator.Components;
using RoadTrafficSimulator.GUI;
using RoadTrafficSimulator.ValueTypes;

namespace RoadTrafficSimulator.Forms
{
    /// <summary>
    /// Provides interface for setting up traffic lights and main roads.
    /// </summary>
    public partial class TrafficLightPanel : UserControl
    {
        private const float roadPercentageInView = 0.8f;

        private readonly Dictionary<CheckBox, CoordsConvertor.Direction> checkBoxDirections;
        private readonly ImageList mainRoadDirectionImages;

        private Point origin;
        private float zoom;
        private MapManager mapManager;
        private MapManager.CrossroadWrapper crossroad;
        private TrafficLight trafficLight;
        private List<(CoordsConvertor.Direction, CoordsConvertor.Direction)?> mainRoadOptions = new();
        private TrafficLight.Setting currentSetting;
        private IGRoad selectedRoad;
        private bool freezeMainRoads;
        private bool freezeDirectionCheckBoxes;
        private bool freezeCheckBoxTrafficLight;
        private bool freezeDuration;

        /// <summary>
        /// Creates a new traffic light panel.
        /// </summary>
        public TrafficLightPanel()
        {
            InitializeComponent();
            // Enable double-buffering for panelMap
            typeof(Panel).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.SetProperty |
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.NonPublic,
                null, panelMap, new object[] { true });

            checkBoxDirections = new Dictionary<CheckBox, CoordsConvertor.Direction>()
            {
                { checkBoxUp, CoordsConvertor.Direction.Up },
                { checkBoxDown, CoordsConvertor.Direction.Down },
                { checkBoxLeft, CoordsConvertor.Direction.Left },
                { checkBoxRight, CoordsConvertor.Direction.Right },
            };
            mainRoadDirectionImages = new ImageList();
            InitialiseMainRoadSelection();
        }

        [Browsable(true)]
        [Category("Action")]
        [Description("Occurs when the map is clicked.")]
        public event EventHandler MapClicked;

        /// <summary>
        /// Initialises the traffic light panel and connects it to a given map manager.
        /// </summary>
        internal void Initialise(MapManager mapManager)
        {
            this.mapManager = mapManager;
        }

        /// <summary>
        /// Activates the panel and connects it to a given crossroad.
        /// </summary>
        internal void Activate(MapManager.CrossroadWrapper crossroad)
        {
            this.crossroad = crossroad;
            trafficLight = crossroad.crossroad.TrafficLight;
            freezeCheckBoxTrafficLight = true;
            checkBoxActivateTrafficLight.Checked = crossroad.crossroad.ActiveCrossingAlgorithm == trafficLight;
            freezeCheckBoxTrafficLight = false;
            UpdateMapPositioning();
            InitialiseComboBoxSetting();
            FillViableMainRoadDirections();
        }

        /// <summary>
        /// Deactivates the panel and disconnects it from a currently connected crossroad.
        /// </summary>
        internal void Deactivate()
        {
            crossroad = default;
            trafficLight = null;
            UnselectRoad();
        }

        #region form_events

        private void panelMap_Paint(object sender, PaintEventArgs e)
        {
            mapManager?.Draw(e.Graphics, origin, zoom, panelMap.Width, panelMap.Height, false);
        }

        private void panelMap_MouseClick(object sender, MouseEventArgs e)
        {
            SelectRoad(e.Location);
            panelMap.Invalidate();
            MapClicked?.Invoke(sender, e);
        }

        private void panelMap_Resize(object sender, EventArgs e)
        {
            if (panelMap.Height != panelMap.Width)
                panelMap.Height = panelMap.Width;
            UpdateMapPositioning();
        }

        private void checkBoxActivateTrafficLight_CheckedChanged(object sender, EventArgs e)
        {
            if (freezeCheckBoxTrafficLight)
                return;
            if (checkBoxActivateTrafficLight.Checked)
                crossroad.crossroad.ActivateTrafficLight();
            else
                crossroad.crossroad.DeactivateTrafficLight();
        }

        private void comboBoxSetting_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentSetting = trafficLight.Settings[comboBoxSetting.SelectedIndex];
            ShowTrafficLightProperties();
        }

        private void dataGridViewMainRoad_SelectionChanged(object sender, EventArgs e)
        {
            if (!freezeMainRoads && dataGridViewMainRoad.SelectedRows.Count > 0)
                crossroad.gCrossroad.MainRoadDirections = mainRoadOptions[dataGridViewMainRoad.SelectedRows[0].Index];
        }

        private void numericUpDownDuration_ValueChanged(object sender, EventArgs e)
        {
            if (freezeDuration)
                return;
            currentSetting.Duration = ((int)numericUpDownDuration.Value).Seconds();
            // Check if the new value was accepted
            freezeDuration = true;
            numericUpDownDuration.Value = currentSetting.Duration.ToSeconds();
            freezeDuration = false;
        }

        private void checkBoxDirection_CheckedChanged(object sender, EventArgs e)
        {
            if (freezeDirectionCheckBoxes)
                return;
            CheckBox cb = sender as CheckBox;
            int from = selectedRoad.GetRoad().Id;
            int to = mapManager.GetRoad(crossroad.crossroad.Id, checkBoxDirections[cb]).GetRoad().Id;

            if (cb.Checked)
                currentSetting.AddDirection(from, to);
            else
                currentSetting.RemoveDirection(from, to);
        }

        private void buttonAddSetting_Click(object sender, EventArgs e)
        {
            TrafficLight.Setting setting = trafficLight.InsertSetting(comboBoxSetting.SelectedIndex + 1);
            Debug.Assert(setting != null);
            InitialiseComboBoxSetting();
            comboBoxSetting.SelectedIndex++;
        }

        private void buttonDeleteSetting_Click(object sender, EventArgs e)
        {
            bool success = trafficLight.RemoveSetting(comboBoxSetting.SelectedIndex);
            Debug.Assert(success);
            InitialiseComboBoxSetting();
        }

        #endregion form_events

        #region helper_methods

        /// <summary>
        /// Select a road nearest to the given mouse location.
        /// </summary>
        /// <remarks>
        /// Always selects the direction going towards the crossroad being set up.
        /// </remarks>
        private void SelectRoad(Point mouseLocation)
        {
            UnselectRoad();
            Vector vector = CoordsConvertor.CalculateVector(mouseLocation, origin, zoom);
            if (vector.to != crossroad.crossroad.Id)
                vector = vector.Reverse();
            selectedRoad = mapManager.GetRoad(vector);
            // We only want to select open roads
            if (selectedRoad?.GetRoad()?.IsConnected != true)
            {
                selectedRoad = null;
                InitialiseDirectionCheckBoxes();
                return;
            }

            selectedRoad.SetHighlight(Highlight.Large, IGRoad.Direction.Forward);
            InitialiseDirectionCheckBoxes();
        }

        /// <summary>
        /// Cancels selection of a road.
        /// </summary>
        private void UnselectRoad()
        {
            if (selectedRoad == null)
                return;
            selectedRoad.UnsetHighlight(Highlight.Large, IGRoad.Direction.Forward);
            selectedRoad = null;
        }

        /// <summary>
        /// Shows properties in the tab page for traffic lights based on available and selected traffic light settings.
        /// </summary>
        private void ShowTrafficLightProperties()
        {
            SuspendLayout();
            buttonDeleteSetting.Enabled = trafficLight.Settings.Count > 2;
            buttonAddSetting.Enabled = trafficLight.Settings.Count < TrafficLight.maxSettingsCount;
            freezeDuration = true;
            numericUpDownDuration.Value = currentSetting.Duration.ToSeconds();
            freezeDuration = false;
            InitialiseDirectionCheckBoxes();
            ResumeLayout();
        }

        /// <summary>
        /// Initialises values in combo box for selecting traffic light settings based on the crossroad being set up.
        /// </summary>
        private void InitialiseComboBoxSetting()
        {
            int index = comboBoxSetting.SelectedIndex;
            comboBoxSetting.Items.Clear();
            for (int i = 1; i <= trafficLight.Settings.Count; i++)
                comboBoxSetting.Items.Add($"Phase {i}");
            if (index > comboBoxSetting.Items.Count - 1)
                index = comboBoxSetting.Items.Count - 1;
            if (comboBoxSetting.Items.Count > 0 && index < 0)
                index = 0;
            comboBoxSetting.SelectedIndex = index;
        }

        /// <summary>
        /// Initialises controls for main road selection.
        /// </summary>
        private void InitialiseMainRoadSelection()
        {
            const int imageSize = 48;

            var allDirections = Enum.GetValues<CoordsConvertor.Direction>();
            Image defaultImg = new Bitmap(imageSize, imageSize);
            mainRoadDirectionImages.Images.Add(defaultImg);

            for (int i = 0; i < allDirections.Length; i++)
            {
                for (int j = i + 1; j < allDirections.Length; j++)
                {
                    string dir1 = allDirections[i].ToString().ToLower();
                    string dir2 = allDirections[j].ToString().ToLower();
                    Image img = (Image)Properties.Resources.ResourceManager.GetObject($"main_road_{dir1}_{dir2}");
                    if (img == null)
                        img = (Image)Properties.Resources.ResourceManager.GetObject($"main_road_{dir2}_{dir1}");
                    Debug.Assert(img != null);
                    mainRoadDirectionImages.Images.Add(img);
                }
            }
            mainRoadDirectionImages.ImageSize = new Size(imageSize, imageSize);

            DataGridViewImageColumn imageCol = new()
            {
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                Width = 70,
            };
            DataGridViewTextBoxColumn textCol = new()
            {
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            };
            dataGridViewMainRoad.Columns.AddRange(imageCol, textCol);
        }

        /// <summary>
        /// Fills in viable directions for selecting main roads based on the connected crossroad.
        /// </summary>
        private void FillViableMainRoadDirections()
        {
            freezeMainRoads = true;
            dataGridViewMainRoad.ClearSelection();
            var items = dataGridViewMainRoad.Rows;
            items.Clear();
            items.Add(mainRoadDirectionImages.Images[0], "None");
            mainRoadOptions.Clear();
            mainRoadOptions.Add(null);

            int selectedIndex = 0;
            var allDirections = Enum.GetValues<CoordsConvertor.Direction>();

            int imgIndex = 1;
            for (int i = 0; i < allDirections.Length; i++)
            {
                for (int j = i + 1; j < allDirections.Length; j++)
                {
                    CoordsConvertor.Direction dir1 = allDirections[i];
                    CoordsConvertor.Direction dir2 = allDirections[j];
                    if (mapManager.CanBeMainRoad(crossroad.gCrossroad, dir1, dir2))
                    {
                        if (crossroad.gCrossroad.IsMainRoadDirection(dir1) &&
                            crossroad.gCrossroad.IsMainRoadDirection(dir2))
                            selectedIndex = items.Count;
                        items.Add(mainRoadDirectionImages.Images[imgIndex], $"{dir1} - {dir2}");
                        mainRoadOptions.Add((dir1, dir2));
                    }
                    imgIndex++;
                }
            }
            dataGridViewMainRoad.CurrentCell = dataGridViewMainRoad.Rows[selectedIndex].Cells[0];
            dataGridViewMainRoad.Rows[selectedIndex].Selected = true;
            freezeMainRoads = false;
        }

        /// <summary>
        /// Initialises check boxes for allowed directions based on the currently selected road.
        /// </summary>
        private void InitialiseDirectionCheckBoxes()
        {
            try
            {
                freezeDirectionCheckBoxes = true;
                bool enable = selectedRoad != null;
                foreach (var (cb, dir) in checkBoxDirections)
                {
                    Road road = mapManager.GetRoad(crossroad.crossroad.Id, dir)?.GetRoad();
                    if (enable && road?.IsConnected == true)
                    {
                        // If main roads are selected or there's just one setting, all possible directions are allowed
                        if (currentSetting == null || trafficLight.Settings.Count <= 1)
                        {
                            cb.Enabled = false;
                            cb.Checked = true;
                        }
                        else
                        {
                            // If the selected road is two-way, disable the backward direction
                            Road backRoad = selectedRoad.GetRoad(IGRoad.Direction.Backward);
                            cb.Enabled = backRoad == null || backRoad.Id != road.Id;
                            cb.Checked = currentSetting.ContainsDirection(selectedRoad.GetRoad().Id, road.Id);
                        }
                    }
                    else
                    {
                        cb.Enabled = false;
                        cb.Checked = false;
                    }
                }
            }
            finally
            {
                freezeDirectionCheckBoxes = false;
            }
        }

        /// <summary>
        /// Updates the position and zoom of the displayed map to only contain the connected crossroad.
        /// </summary>
        private void UpdateMapPositioning()
        {
            // Calculate zoom for the map to only show the crossroad being set up.
            float fullRoadSize = Math.Min(panelMap.Width, panelMap.Height) / 2;
            float fullZoom = fullRoadSize / CoordsConvertor.gridSize;
            zoom = fullZoom / roadPercentageInView;
            // Centre on the crossroad
            if (crossroad.crossroad != null)
            {
                Point offset = CoordsConvertor.CalculatePoint(crossroad.crossroad.Id, new Point(0, 0), zoom);
                origin = new Point(panelMap.Width / 2 - offset.X, panelMap.Height / 2 - offset.Y);
            }
        }

        #endregion helper_methods
    }
}
