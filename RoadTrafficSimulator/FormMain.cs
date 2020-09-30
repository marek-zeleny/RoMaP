﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

using RoadTrafficSimulator.ValueTypes;

namespace RoadTrafficSimulator
{
    public partial class FormMain : Form
    {
        private enum Mode { Select_Crossroad, Select_Road, Build_Road };

        private MapManager mapManager;
        private Simulation simulation;
        private Mode mode;
        private IRoadBuilder roadBuilder;
        private Point previousMouseLocation;
        private bool drag;

        private Point PanelMapCenter { get => new Point(panelMap.Width / 2, panelMap.Height / 2); }
        private decimal Zoom
        {
            get => mapManager.Settings.Zoom;
            set
            {
                mapManager.Settings.Zoom = value;
                buttonZoom.Text = string.Format("Zoom: {0:0.0}x", mapManager.Settings.Zoom);
            }
        }

        public FormMain()
        {
            InitializeComponent();
            // Enable double-buffering for panelMap
            typeof(Panel).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic,
                null, panelMap, new object[] { true });
            // Add MouseWheel event for panelMap
            panelMap.MouseWheel += panelMap_MouseWheel;
            // Initialize comboBoxMode
            foreach (Mode m in Enum.GetValues(typeof(Mode)))
                comboBoxMode.Items.Add(m);
            comboBoxMode.SelectedIndex = 0;
            mode = 0;

            Components.Map map = new Components.Map();
            mapManager = new MapManager(map);
            mapManager.Settings.Origin = PanelMapCenter;
            mapManager.Settings.Zoom = 0.5m;
            simulation = new Simulation(map);
        }

        private void panelMap_Paint(object sender, PaintEventArgs e)
        {
            mapManager.Draw(e.Graphics, panelMap.Width, panelMap.Height);
        }

        private void panelMap_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                switch (mode)
                {
                    case Mode.Select_Crossroad:
                        SelectCrossroad(e.Location);
                        break;
                    case Mode.Select_Road:
                        SelectRoad(e.Location);
                        break;
                    case Mode.Build_Road:
                        Build(e.Location);
                        break;
                }
            else if (e.Button == MouseButtons.Right)
                switch (mode)
                {
                    case Mode.Select_Crossroad:
                        break;
                    case Mode.Select_Road:
                        break;
                    case Mode.Build_Road:
                        DestroyBuild();
                        break;
                }
            panelMap.Invalidate();
        }

        private void panelMap_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (mode == Mode.Build_Road)
            {
                FinishBuild();
                panelMap.Invalidate();
            }
        }

        private void panelMap_MouseDown(object sender, MouseEventArgs e)
        {
            if (mode == Mode.Select_Crossroad || mode == Mode.Select_Road)
                drag = true;
        }

        private void panelMap_MouseUp(object sender, MouseEventArgs e)
        {
            drag = false;
        }

        private void panelMap_MouseMove(object sender, MouseEventArgs e)
        {
            if (drag)
            {
                Drag(e.Location);
                panelMap.Invalidate();
            }
            previousMouseLocation = e.Location;
        }
        private void panelMap_MouseWheel(object sender, MouseEventArgs e)
        {
            ChangeZoom(e.Delta, e.Location);
            panelMap.Invalidate();
        }

        private void buttonCenter_Click(object sender, EventArgs e)
        {
            mapManager.Settings.Origin = PanelMapCenter;
            panelMap.Invalidate();
        }

        private void buttonZoom_Click(object sender, EventArgs e)
        {
            Zoom = 1m;
            panelMap.Invalidate();
        }

        private void buttonTrafficLight_Click(object sender, EventArgs e)
        {
            // TODO
        }

        private void comboBoxMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            mode = Enum.Parse<Mode>(comboBoxMode.Text);
            switch (mode)
            {
                case Mode.Select_Crossroad:
                    groupBoxCrossroad.Visible = true;
                    groupBoxRoad.Visible = false;
                    groupBoxBuild.Visible = false;
                    DestroyBuild();
                    break;
                case Mode.Select_Road:
                    groupBoxCrossroad.Visible = false;
                    groupBoxRoad.Visible = true;
                    groupBoxBuild.Visible = false;
                    DestroyBuild();
                    break;
                case Mode.Build_Road:
                    groupBoxCrossroad.Visible = false;
                    groupBoxRoad.Visible = false;
                    groupBoxBuild.Visible = true;
                    break;
            }
            panelMap.Invalidate();
        }

        private void Drag(Point mouseLocation)
        {
            Point delta = new Point(mouseLocation.X - previousMouseLocation.X, mouseLocation.Y - previousMouseLocation.Y);
            mapManager.Settings.MoveOrigin(delta);
        }

        private void ChangeZoom(int direction, Point mouseLocation)
        {
            const decimal coefficient = 1.2m;
            // For centering the zoom at coursor position
            decimal originalZoom = Zoom;
            Point newOrigin = mapManager.Settings.Origin;
            if (direction > 0)
            {
                Zoom *= coefficient;
                newOrigin.X = (int)(newOrigin.X * coefficient - mouseLocation.X * (coefficient - 1));
                newOrigin.Y = (int)(newOrigin.Y * coefficient - mouseLocation.Y * (coefficient - 1));
            }
            else if (direction < 0)
            {
                Zoom /= coefficient;
                newOrigin.X = (int)((newOrigin.X + mouseLocation.X * (coefficient - 1)) / coefficient);
                newOrigin.Y = (int)((newOrigin.Y + mouseLocation.Y * (coefficient - 1)) / coefficient);
            }
            if (Zoom != originalZoom)
                mapManager.Settings.Origin = newOrigin;
        }

        private void SelectCrossroad(Point mouseLocation)
        {
            // TODO
        }

        private void SelectRoad(Point mouseLocation)
        {
            // TODO
        }

        private void Build(Point mouseLocation)
        {
            Coords coords = MapManager.CalculateCoords(mouseLocation, mapManager.Settings.Origin, mapManager.Settings.Zoom);
            if (roadBuilder == null)
                roadBuilder = mapManager.GetRoadBuilder(coords, checkBoxTwoWayRoad.Checked);
            else
            {
                roadBuilder.AddSegment(coords);
                if (!roadBuilder.CanContinue)
                    FinishBuild();
            }
        }

        private void FinishBuild()
        {
            int maxSpeed = (int)numericUpDownSpeed.Value;
            if (roadBuilder.FinishRoad(maxSpeed.MetersPerSecond()))
                roadBuilder = null;
            else
                ShowInfo("The road cannot be built like this.");
        }

        private void DestroyBuild()
        {
            if (roadBuilder == null)
                return;
            roadBuilder.DestroyRoad();
            roadBuilder = null;
        }

        private void ShowInfo(string info)
        {
            // TODO
            Debug.Print(info);
        }
    }
}
