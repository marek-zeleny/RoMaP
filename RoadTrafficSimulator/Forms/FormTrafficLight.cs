using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

using RoadTrafficSimulator.Components;
using RoadTrafficSimulator.ValueTypes;

namespace RoadTrafficSimulator.Forms
{
    public partial class FormTrafficLight : Form
    {
        private const float roadPercentageInView = 0.8f;

        private readonly float zoom;
        private readonly MapManager mapManager;
        private readonly MapManager.CrossroadWrapper crossroad;
        private readonly TrafficLight trafficLight;
        private TrafficLight.Setting currentSetting;
        private readonly Dictionary<CheckBox, int?> checkBoxBinder;
        private readonly Point origin;
        private GUI.IGRoad selectedRoad;
        private bool freezeCheckBoxes;

        internal FormTrafficLight(MapManager mapManager, MapManager.CrossroadWrapper crossroad)
        {
            InitializeComponent();
            // Disable form resizing via maximizing the window, disable minimizing the window
            MaximizeBox = false;
            MinimizeBox = false;
            // Enable double-buffering for panelMap
            typeof(Panel).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.SetProperty |
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.NonPublic,
                null, panelMap, new object[] { true });
            zoom = CalculateZoom();

            this.mapManager = mapManager;
            this.crossroad = crossroad;
            trafficLight = crossroad.crossroad.TrafficLight;
            checkBoxBinder = GetCheckBoxBinder();
            Point offset = MapManager.CalculatePoint(crossroad.crossroad.Id, new Point(0, 0), zoom);
            origin = new Point(panelMap.Width / 2 - offset.X, panelMap.Height / 2 - offset.Y);
            InitializeComboBoxSetting();
        }

        #region form_events

        private void panelMap_Paint(object sender, PaintEventArgs e)
        {
            mapManager.Draw(e.Graphics, origin, zoom, panelMap.Width, panelMap.Height, false);
        }

        private void panelMap_MouseClick(object sender, MouseEventArgs e)
        {
            SelectRoad(e.Location);
            panelMap.Invalidate();
        }

        private void comboBoxSetting_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentSetting = trafficLight.Settings[comboBoxSetting.SelectedIndex];
            ShowProperties();
        }

        private void numericUpDownDuration_ValueChanged(object sender, EventArgs e)
        {
            currentSetting.Duration = ((int)numericUpDownDuration.Value).Seconds();
        }

        private void checkBoxDirection_CheckedChanged(object sender, EventArgs e)
        {
            if (freezeCheckBoxes)
                return;
            CheckBox cb = sender as CheckBox;
            int from = selectedRoad.GetRoad().Id;
            int to = checkBoxBinder[cb].Value;

            if (cb.Checked)
                currentSetting.AddDirection(from, to);
            else
                currentSetting.RemoveDirection(from, to);
        }

        private void buttonNewSetting_Click(object sender, EventArgs e)
        {
            TrafficLight.Setting setting = trafficLight.InsertSetting(comboBoxSetting.SelectedIndex + 1);
            if (setting == null)
            {
                ShowInfo("Cannot add a new setting, the maximum number of settings has been reached.");
                return;
            }
            InitializeComboBoxSetting();
            comboBoxSetting.SelectedIndex++;
        }

        private void buttonDeleteSetting_Click(object sender, EventArgs e)
        {
            if (trafficLight.RemoveSetting(comboBoxSetting.SelectedIndex))
                InitializeComboBoxSetting();
            else
                ShowInfo("Cannot delete this setting, the traffic light needs at least one active setting.");
        }

        private void buttonFinish_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FormTrafficLight_FormClosed(object sender, FormClosedEventArgs e)
        {
            UnselectRoad();
        }

        #endregion form_events

        #region helper_methods

        private void SelectRoad(Point mouseLocation)
        {
            UnselectRoad();
            Vector vector = MapManager.CalculateVector(mouseLocation, origin, zoom);
            if (vector.to != crossroad.crossroad.Id)
                vector = vector.Reverse();
            selectedRoad = mapManager.GetRoad(vector);
            // We only want to select open roads
            Road road = selectedRoad?.GetRoad();
            if (road == null || !road.IsConnected)
            {
                selectedRoad = null;
                return;
            }

            selectedRoad.SetHighlight(GUI.Highlight.Large, GUI.IGRoad.Direction.Forward);
            InitializeDirectionCheckBoxes();
        }

        private void UnselectRoad()
        {
            if (selectedRoad == null)
                return;
            selectedRoad.UnsetHighlight(GUI.Highlight.Large, GUI.IGRoad.Direction.Forward);
            selectedRoad = null;
        }

        private void ShowProperties()
        {
            numericUpDownDuration.Enabled = trafficLight.Settings.Count > 1;
            numericUpDownDuration.Value = currentSetting.Duration.ToSeconds();
            InitializeDirectionCheckBoxes();
        }

        private void InitializeComboBoxSetting()
        {
            int index = comboBoxSetting.SelectedIndex;
            comboBoxSetting.Items.Clear();
            for (int i = 0; i < trafficLight.Settings.Count; i++)
                comboBoxSetting.Items.Add(string.Format("Setting {0}", i + 1));
            if (index > comboBoxSetting.Items.Count - 1)
                index = comboBoxSetting.Items.Count - 1;
            if (comboBoxSetting.Items.Count > 0 && index < 0)
                index = 0;
            comboBoxSetting.SelectedIndex = index;
        }

        private void InitializeDirectionCheckBoxes()
        {
            try
            {
                freezeCheckBoxes = true;
                bool enable = selectedRoad != null;
                foreach (var (cb, id) in checkBoxBinder)
                {
                    if (enable && id.HasValue)
                    {
                        // If there's just one setting, all possible directions are allowed
                        if (trafficLight.Settings.Count <= 1)
                        {
                            cb.Enabled = false;
                            cb.Checked = true;
                        }
                        else
                        {
                            // If the selected road is two-way, disable the backward direction
                            cb.Enabled = selectedRoad.GetRoad(GUI.IGRoad.Direction.Backward)?.Id != id;
                            cb.Checked = currentSetting.ContainsDirection(selectedRoad.GetRoad().Id, id.Value);
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
                freezeCheckBoxes = false;
            }
        }

        private Dictionary<CheckBox, int?> GetCheckBoxBinder()
        {
            Coords from = crossroad.crossroad.Id;

            Vector leftDir = new(from, new Coords(from.x - 1, from.y));
            Vector rightDir = new(from, new Coords(from.x + 1, from.y));
            Vector upDir = new(from, new Coords(from.x, from.y - 1));
            Vector downDir = new(from, new Coords(from.x, from.y + 1));

            GUI.IGRoad leftGRoad = mapManager.GetRoad(leftDir);
            GUI.IGRoad rightGRoad = mapManager.GetRoad(rightDir);
            GUI.IGRoad upGRoad = mapManager.GetRoad(upDir);
            GUI.IGRoad downGRoad = mapManager.GetRoad(downDir);

            Road leftRoad = leftGRoad?.GetRoad();
            Road rightRoad = rightGRoad?.GetRoad();
            Road upRoad = upGRoad?.GetRoad();
            Road downRoad = downGRoad?.GetRoad();
            // We consider only open roads for traffic lights
            int? leftId = leftRoad != null && leftRoad.IsConnected ? leftRoad.Id : null;
            int? rightId = rightRoad != null && rightRoad.IsConnected ? rightRoad.Id : null;
            int? upId = upRoad != null && upRoad.IsConnected ? upRoad.Id : null;
            int? downId = downRoad != null && downRoad.IsConnected ? downRoad.Id : null;

            Dictionary<CheckBox, int?> binder = new(4);
            binder[checkBoxLeft] = leftId;
            binder[checkBoxRight] = rightId;
            binder[checkBoxUp] = upId;
            binder[checkBoxDown] = downId;
            return binder;
        }

        private float CalculateZoom()
        {
            float fullRoadSize = Math.Min(panelMap.Width, panelMap.Height) / 2;
            float fullZoom = fullRoadSize / MapManager.gridSize;
            return fullZoom / roadPercentageInView;
        }

        private void ShowInfo(string info)
        {
            labelInfo.Text = info;
            Debug.WriteLine("{0}: {1}", DateTime.Now, info);
            labelInfo.Refresh();
        }

        #endregion helper_methods
    }
}
