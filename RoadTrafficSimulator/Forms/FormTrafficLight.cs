﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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
        private readonly Dictionary<CheckBox, CoordsConvertor.Direction> checkBoxDirections;
        private readonly Point origin;

        private List<(CoordsConvertor.Direction, CoordsConvertor.Direction)?> mainRoadOptions = new();
        private TrafficLight.Setting currentSetting;
        private GUI.IGRoad selectedRoad;
        private bool freezeCheckBoxes;
        private bool freezeDuration;

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
            checkBoxDirections = new Dictionary<CheckBox, CoordsConvertor.Direction>()
            {
                { checkBoxUp, CoordsConvertor.Direction.Up },
                { checkBoxDown, CoordsConvertor.Direction.Down },
                { checkBoxLeft, CoordsConvertor.Direction.Left },
                { checkBoxRight, CoordsConvertor.Direction.Right },
            };
            Point offset = CoordsConvertor.CalculatePoint(crossroad.crossroad.Id, new Point(0, 0), zoom);
            origin = new Point(panelMap.Width / 2 - offset.X, panelMap.Height / 2 - offset.Y);
            InitialiseComboBoxSetting();
            InitialiseComboBoxMainRoad();
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
            if (comboBoxSetting.SelectedIndex == 0)
                currentSetting = null;
            else
                currentSetting = trafficLight.Settings[comboBoxSetting.SelectedIndex - 1];
            ShowProperties();
        }

        private void imageComboBoxMainRoad_SelectedIndexChanged(object sender, EventArgs e)
        {
            crossroad.gCrossroad.MainRoadDirections = mainRoadOptions[imageComboBoxMainRoad.SelectedIndex];
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
            if (freezeCheckBoxes)
                return;
            CheckBox cb = sender as CheckBox;
            int from = selectedRoad.GetRoad().Id;
            int to = mapManager.GetRoad(crossroad.crossroad.Id, checkBoxDirections[cb]).GetRoad().Id;

            if (cb.Checked)
                currentSetting.AddDirection(from, to);
            else
                currentSetting.RemoveDirection(from, to);
        }

        private void buttonNewSetting_Click(object sender, EventArgs e)
        {
            TrafficLight.Setting setting = trafficLight.InsertSetting(comboBoxSetting.SelectedIndex);
            Debug.Assert(setting != null);
            InitialiseComboBoxSetting();
            comboBoxSetting.SelectedIndex++;
        }

        private void buttonDeleteSetting_Click(object sender, EventArgs e)
        {
            bool success = trafficLight.RemoveSetting(comboBoxSetting.SelectedIndex - 1);
            Debug.Assert(success);
            InitialiseComboBoxSetting();
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
            Vector vector = CoordsConvertor.CalculateVector(mouseLocation, origin, zoom);
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
            InitialiseDirectionCheckBoxes();
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
            SuspendLayout();
            if (currentSetting == null)
            {
                // Main road settings
                groupBoxAllowedDirections.Visible = false;
                groupBoxMainRoad.Visible = true;

                numericUpDownDuration.Enabled = false;
                buttonNewSetting.Enabled = false;
                buttonDeleteSetting.Enabled = false;
                freezeDuration = true;
                numericUpDownDuration.Value = 0;
                freezeDuration = false;
            }
            else
            {
                // Traffic light settings
                groupBoxAllowedDirections.Visible = true;
                groupBoxMainRoad.Visible = false;

                numericUpDownDuration.Enabled = trafficLight.Settings.Count > 1;
                buttonDeleteSetting.Enabled = trafficLight.Settings.Count > 1;
                buttonNewSetting.Enabled = trafficLight.Settings.Count < TrafficLight.maxSettingsCount;
                freezeDuration = true;
                numericUpDownDuration.Value = currentSetting.Duration.ToSeconds();
                freezeDuration = false;
                InitialiseDirectionCheckBoxes();
            }
            ResumeLayout();
        }

        private void InitialiseComboBoxSetting()
        {
            int index = comboBoxSetting.SelectedIndex;
            comboBoxSetting.Items.Clear();
            comboBoxSetting.Items.Add("Main road setting");
            for (int i = 1; i <= trafficLight.Settings.Count; i++)
                comboBoxSetting.Items.Add(string.Format("Setting {0}", i));
            if (index > comboBoxSetting.Items.Count - 1)
                index = comboBoxSetting.Items.Count - 1;
            if (comboBoxSetting.Items.Count > 0 && index < 0)
                index = 0;
            comboBoxSetting.SelectedIndex = index;
        }

        private void InitialiseComboBoxMainRoad()
        {
            bool CanBeMainRoad(CoordsConvertor.Direction dir1, CoordsConvertor.Direction dir2, out bool isSelected)
            {
                GUI.IGRoad road1 = mapManager.GetRoad(crossroad.crossroad.Id, dir1);
                GUI.IGRoad road2 = mapManager.GetRoad(crossroad.crossroad.Id, dir2);
                isSelected =
                    crossroad.gCrossroad.IsMainRoadDirection(dir1) &&
                    crossroad.gCrossroad.IsMainRoadDirection(dir2);

                if (road1 == null || road2 == null)
                    return false;
                if (road1.GetRoad(GUI.IGRoad.Direction.Backward) != null &&
                    road2.GetRoad(GUI.IGRoad.Direction.Forward) != null)
                    return true;
                if (road2.GetRoad(GUI.IGRoad.Direction.Backward) != null &&
                    road1.GetRoad(GUI.IGRoad.Direction.Forward) != null)
                    return true;
                return false;
            }

            var items = imageComboBoxMainRoad.Items;
            items.Clear();
            items.Add(new ImageComboBox.DropDownItem("none", null));
            mainRoadOptions.Clear();
            mainRoadOptions.Add(null);

            int selectedIndex = 0;
            var allDirections = Enum.GetValues<CoordsConvertor.Direction>();

            for (int i = 0; i < allDirections.Length; i++)
            {
                for (int j = i + 1; j < allDirections.Length; j++)
                {
                    CoordsConvertor.Direction dir1 = allDirections[i];
                    CoordsConvertor.Direction dir2 = allDirections[j];
                    if (CanBeMainRoad(dir1, dir2, out bool isSelected))
                    {
                        if (isSelected)
                            selectedIndex = items.Count;
                        items.Add(new ImageComboBox.DropDownItem($"{dir1} - {dir2}", null));
                        // TODO: add image Properties.Resources.main_road_{dir1}_{dir2}
                        mainRoadOptions.Add((dir1, dir2));
                    }
                }
            }
            imageComboBoxMainRoad.SelectedIndex = selectedIndex;
        }

        private void InitialiseDirectionCheckBoxes()
        {
            try
            {
                freezeCheckBoxes = true;
                bool enable = selectedRoad != null;
                foreach (var (cb, dir) in checkBoxDirections)
                {
                    Road road = mapManager.GetRoad(crossroad.crossroad.Id, dir)?.GetRoad();
                    if (enable && road != null)
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
                            Road backRoad = selectedRoad.GetRoad(GUI.IGRoad.Direction.Backward);
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
                freezeCheckBoxes = false;
            }
        }

        private float CalculateZoom()
        {
            float fullRoadSize = Math.Min(panelMap.Width, panelMap.Height) / 2;
            float fullZoom = fullRoadSize / MapManager.gridSize;
            return fullZoom / roadPercentageInView;
        }

        #endregion helper_methods
    }
}
