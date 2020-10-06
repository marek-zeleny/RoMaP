using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

using RoadTrafficSimulator.Components;
using RoadTrafficSimulator.ValueTypes;

namespace RoadTrafficSimulator
{
    public partial class FormTrafficLight : Form
    {
        private const decimal zoom = 10m;

        private MapManager mapManager;
        private CrossroadView crossroadView;
        private TrafficLight trafficLight;
        private TrafficLight.Setting currentSetting;
        private readonly OutRoadIds outRoads;
        private readonly Point origin;
        private RoadView selectedRoad;
        private bool freezeCheckBoxes;

        internal FormTrafficLight(MapManager mapManager, CrossroadView crossroadView)
        {
            InitializeComponent();
            this.mapManager = mapManager;
            this.crossroadView = crossroadView;
            trafficLight = crossroadView.TrafficLight;
            outRoads = GetOutRoadIds();
            Point offset = MapManager.CalculatePoint(crossroadView.Coords, new Point(0, 0), zoom);
            origin = new Point(panelMap.Width / 2 - offset.X, panelMap.Height / 2 - offset.Y);
            InitializeComboBoxSetting();
        }

        private void panelMap_Paint(object sender, PaintEventArgs e)
        {
            mapManager.Draw(e.Graphics, origin, zoom, panelMap.Width, panelMap.Height);
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

        private void checkBoxLeft_CheckedChanged(object sender, EventArgs e)
        {
            if (freezeCheckBoxes)
                return;
            if (checkBoxLeft.Checked)
                currentSetting.AddDirection(selectedRoad.Id, outRoads.left.Value);
            else
                currentSetting.RemoveDirection(selectedRoad.Id, outRoads.left.Value);
        }

        private void checkBoxRight_CheckedChanged(object sender, EventArgs e)
        {
            if (freezeCheckBoxes)
                return;
            if (checkBoxRight.Checked)
                currentSetting.AddDirection(selectedRoad.Id, outRoads.right.Value);
            else
                currentSetting.RemoveDirection(selectedRoad.Id, outRoads.right.Value);
        }

        private void checkBoxUp_CheckedChanged(object sender, EventArgs e)
        {
            if (freezeCheckBoxes)
                return;
            if (checkBoxUp.Checked)
                currentSetting.AddDirection(selectedRoad.Id, outRoads.up.Value);
            else
                currentSetting.RemoveDirection(selectedRoad.Id, outRoads.up.Value);
        }

        private void checkBoxDown_CheckedChanged(object sender, EventArgs e)
        {
            if (freezeCheckBoxes)
                return;
            if (checkBoxDown.Checked)
                currentSetting.AddDirection(selectedRoad.Id, outRoads.down.Value);
            else
                currentSetting.RemoveDirection(selectedRoad.Id, outRoads.down.Value);
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

        private void SelectRoad(Point mouseLocation)
        {
            UnselectRoad();
            Vector vector = MapManager.CalculateVector(mouseLocation, origin, zoom);
            selectedRoad = mapManager.GetRoad(vector);
            if (selectedRoad != null && !selectedRoad.TwoWayRoad && selectedRoad.To != crossroadView.Coords)
                selectedRoad = null;
            if (selectedRoad != null)
                selectedRoad.GuiRoad.Highlight = GUI.Highlight.High;
            InitializeDirectionCheckBoxes();
        }

        private void UnselectRoad()
        {
            if (selectedRoad == null)
                return;
            selectedRoad.GuiRoad.Highlight = GUI.Highlight.Normal;
            selectedRoad = null;
        }

        private void ShowProperties()
        {
            numericUpDownDuration.Value = currentSetting.Duration;
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
                checkBoxLeft.Enabled = enable && outRoads.left.HasValue;
                checkBoxRight.Enabled = enable && outRoads.right.HasValue;
                checkBoxUp.Enabled = enable && outRoads.up.HasValue;
                checkBoxDown.Enabled = enable && outRoads.down.HasValue;
                checkBoxLeft.Checked = checkBoxLeft.Enabled && currentSetting.ContainsDirection(selectedRoad.Id, outRoads.left.Value);
                checkBoxRight.Checked = checkBoxRight.Enabled && currentSetting.ContainsDirection(selectedRoad.Id, outRoads.right.Value);
                checkBoxUp.Checked = checkBoxUp.Enabled && currentSetting.ContainsDirection(selectedRoad.Id, outRoads.up.Value);
                checkBoxDown.Checked = checkBoxDown.Enabled && currentSetting.ContainsDirection(selectedRoad.Id, outRoads.down.Value);
            }
            finally
            {
                freezeCheckBoxes = false;
            }
        }

        private OutRoadIds GetOutRoadIds()
        {
            RoadView leftRoad = mapManager.GetRoad(new Vector(crossroadView.Coords, new Coords(crossroadView.Coords.x - 1, crossroadView.Coords.y)));
            RoadView rightRoad = mapManager.GetRoad(new Vector(crossroadView.Coords, new Coords(crossroadView.Coords.x + 1, crossroadView.Coords.y)));
            RoadView upRoad = mapManager.GetRoad(new Vector(crossroadView.Coords, new Coords(crossroadView.Coords.x, crossroadView.Coords.y - 1)));
            RoadView downRoad = mapManager.GetRoad(new Vector(crossroadView.Coords, new Coords(crossroadView.Coords.x, crossroadView.Coords.y + 1)));
            if (leftRoad != null && leftRoad.From != crossroadView.Coords)
                leftRoad = mapManager.GetOppositeRoad(leftRoad);
            if (rightRoad != null && rightRoad.From != crossroadView.Coords)
                rightRoad = mapManager.GetOppositeRoad(rightRoad);
            if (upRoad != null && upRoad.From != crossroadView.Coords)
                upRoad = mapManager.GetOppositeRoad(upRoad);
            if (downRoad != null && downRoad.From != crossroadView.Coords)
                downRoad = mapManager.GetOppositeRoad(downRoad);
            int? left = leftRoad?.Id ?? null;
            int? right = rightRoad?.Id ?? null;
            int? up = upRoad?.Id ?? null;
            int? down = downRoad?.Id ?? null;
            return new OutRoadIds(left, right, up, down);
        }

        private void ShowInfo(string info)
        {
            // TODO
            Debug.Print(info);
        }

        private struct OutRoadIds
        {
            public readonly int? left, right, up, down;

            public OutRoadIds(int? left, int? right, int? up, int? down)
            {
                this.left = left;
                this.right = right;
                this.up = up;
                this.down = down;
            }
        }
    }
}
