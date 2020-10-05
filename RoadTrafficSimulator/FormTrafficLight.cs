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
        private readonly Point origin;

        internal FormTrafficLight(MapManager mapManager, CrossroadView crossroadView)
        {
            InitializeComponent();
            this.mapManager = mapManager;
            this.crossroadView = crossroadView;
            trafficLight = crossroadView.TrafficLight;
            InitializeComboBoxSetting();
            Point offset = MapManager.CalculatePoint(crossroadView.Coords, new Point(0, 0), zoom);
            origin = new Point(panelMap.Width / 2 - offset.X, panelMap.Height / 2 - offset.Y);
        }

        private void panelMap_Paint(object sender, PaintEventArgs e)
        {
            mapManager.Draw(e.Graphics, origin, zoom, panelMap.Width, panelMap.Height);
        }

        private void panelMap_MouseClick(object sender, MouseEventArgs e)
        {
            // TODO
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

        private void ShowProperties()
        {
            numericUpDownDuration.Value = currentSetting.Duration;
        }

        private void ShowInfo(string info)
        {
            // TODO
            Debug.Print(info);
        }
    }
}
