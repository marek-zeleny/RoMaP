using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using RoadTrafficSimulator.Components;
using RoadTrafficSimulator.ValueTypes;

namespace RoadTrafficSimulator
{
    public partial class FormTrafficLight : Form
    {
        private const float zoom = 2f;

        private MapManager mapManager;
        private CrossroadView crossroadView;
        private TrafficLight trafficLight;
        private TrafficLight.Setting currentSetting;
        private readonly CheckBoxBinder checkBoxBinder;
        private readonly Point origin;
        private RoadView selectedRoad;
        private bool freezeCheckBoxes;

        internal FormTrafficLight(MapManager mapManager, CrossroadView crossroadView)
        {
            InitializeComponent();
            // Disable form resizing via maximizing the window, disable minimizing the window
            MaximizeBox = false;
            MinimizeBox = false;
            // Enable double-buffering for panelMap
            typeof(Panel).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic,
                null, panelMap, new object[] { true });
            this.mapManager = mapManager;
            this.crossroadView = crossroadView;
            trafficLight = crossroadView.TrafficLight;
            checkBoxBinder = GetCheckBoxBinder();
            Point offset = MapManager.CalculatePoint(crossroadView.Coords, new Point(0, 0), zoom);
            origin = new Point(panelMap.Width / 2 - offset.X, panelMap.Height / 2 - offset.Y);
            InitializeComboBoxSetting();
        }

        #region form_events

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

        private void checkBoxDirection_CheckedChanged(object sender, EventArgs e)
        {
            if (freezeCheckBoxes)
                return;
            CheckBox cb = sender as CheckBox;
            int from = selectedRoad.Id;
            int to = checkBoxBinder.GetId(cb).Value;
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

        #endregion form_events

        #region helper_methods

        private void FormTrafficLight_FormClosed(object sender, FormClosedEventArgs e)
        {
            UnselectRoad();
        }

        private void SelectRoad(Point mouseLocation)
        {
            UnselectRoad();
            Vector vector = MapManager.CalculateVector(mouseLocation, origin, zoom);
            selectedRoad = mapManager.GetRoad(vector);
            if (selectedRoad != null && selectedRoad.To != crossroadView.Coords)
                selectedRoad = mapManager.GetOppositeRoad(selectedRoad);
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
                foreach (var (cb, id) in checkBoxBinder)
                {
                    if (enable && id.HasValue)
                    {
                        // If the selected road is two-way, disable the backward direction
                        cb.Enabled = !selectedRoad.GuiRoad.GetRoadIds().Contains((int)id);
                        cb.Checked = currentSetting.ContainsDirection(selectedRoad.Id, id.Value);
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

        private CheckBoxBinder GetCheckBoxBinder()
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
            int? left = leftRoad?.Id;
            int? right = rightRoad?.Id;
            int? up = upRoad?.Id;
            int? down = downRoad?.Id;
            return new CheckBoxBinder(left, right, up, down, this);
        }

        private void ShowInfo(string info)
        {
            labelInfo.Text = info;
            Debug.WriteLine("{0}: {1}", DateTime.Now, info);
        }

        #endregion helper_methods

        private struct CheckBoxBinder : IEnumerable<(CheckBox, int?)>
        {
            private readonly (CheckBox, int?)[] pairs;

            public CheckBoxBinder(int? leftId, int? rightId, int? upId, int? downId, FormTrafficLight form)
            {
                pairs = new (CheckBox, int?)[4] {
                    (form.checkBoxLeft, leftId),
                    (form.checkBoxRight, rightId),
                    (form.checkBoxUp, upId),
                    (form.checkBoxDown, downId) };
            }

            public int? GetId(CheckBox checkBox) => Array.Find(pairs, pair => pair.Item1 == checkBox).Item2;

            public IEnumerator<(CheckBox, int?)> GetEnumerator() => pairs.AsEnumerable().GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => pairs.GetEnumerator();
        }
    }
}
