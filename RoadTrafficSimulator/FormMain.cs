using System;
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
        private const decimal minZoom = 0.2m;
        private const decimal maxZoom = 5m;

        private enum Mode { Select_Crossroad, Select_Road, Build_Road };

        private MapManager mapManager;
        private Point origin;
        private decimal zoom; // Do not access directly, use property Zoom
        private Simulation simulation;
        private Mode mode;
        private IRoadBuilder currentRoadBuilder;
        private CrossroadView selectedCrossroad;
        private RoadView selectedRoad;
        private Point previousMouseLocation;
        private bool drag;
        private bool dragOccured;

        public decimal Zoom
        {
            get => zoom;
            set
            {
                if (value < minZoom)
                    zoom = minZoom;
                else if (value > maxZoom)
                    zoom = maxZoom;
                else
                    zoom = value;
                buttonZoom.Text = string.Format("Zoom: {0:0.0}x", zoom);
            }
        }

        private Point PanelMapCenter { get => new Point(panelMap.Width / 2, panelMap.Height / 2); }

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
            origin = PanelMapCenter;
            Zoom = 1m;
            simulation = new Simulation(map);
        }

        #region form_events

        private void panelMap_Paint(object sender, PaintEventArgs e)
        {
            mapManager.Draw(e.Graphics, origin, Zoom, panelMap.Width, panelMap.Height);
        }

        private void panelMap_MouseClick(object sender, MouseEventArgs e)
        {
            if (dragOccured)
                return;
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
            if (e.Button == MouseButtons.Left)
                drag = true;
        }

        private void panelMap_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                drag = false;
                dragOccured = false;
            }
        }

        private void panelMap_MouseMove(object sender, MouseEventArgs e)
        {
            if (drag)
            {
                Drag(e.Location);
                dragOccured = true;
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
            origin = PanelMapCenter;
            panelMap.Invalidate();
        }

        private void buttonZoom_Click(object sender, EventArgs e)
        {
            Zoom = 1m;
            panelMap.Invalidate();
        }

        private void buttonTrafficLight_Click(object sender, EventArgs e)
        {
            FormTrafficLight form = new FormTrafficLight(mapManager, selectedCrossroad);
            form.FormClosed += (object sender, FormClosedEventArgs e) => Enabled = true;
            Enabled = false;
            form.Show();
        }

        private void buttonDestroyCrossroad_Click(object sender, EventArgs e)
        {
            if (selectedCrossroad != null)
            {
                mapManager.DestroyCrossroad(selectedCrossroad);
                UnselectCrossroad();
                panelMap.Invalidate();
            }
        }

        private void buttonDestroyRoad_Click(object sender, EventArgs e)
        {
            if (selectedRoad != null)
            {
                mapManager.DestroyRoad(selectedRoad);
                UnselectRoad();
                panelMap.Invalidate();
            }
        }

        private void buttonStartSimulation_Click(object sender, EventArgs e)
        {
            StartSimulation();
        }

        private void trackBarDuration_Scroll(object sender, EventArgs e)
        {
            labelDuration.Text = string.Format("Duration: {0}h", trackBarDuration.Value);
        }

        private void trackBarCarFrequency_Scroll(object sender, EventArgs e)
        {
            labelCarFrequency.Text = string.Format("Car frequency: {0:0.00}", (float)trackBarCarFrequency.Value / 100);
        }

        private void comboBoxMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeMode(Enum.Parse<Mode>(comboBoxMode.Text));
            panelMap.Invalidate();
        }

        #endregion // form_events

        #region helper_methods

        private void StartSimulation()
        {
            if (simulation.Initialize())
            {
                string message = string.Format(
                    "Map check complete: the created map is consistent.\n" +
                    "Do you want to start the simulation of {0} hours with {1:0.00} car frequency?"
                    , trackBarDuration.Value, (float)trackBarCarFrequency.Value / 100);
                DialogResult result = MessageBox.Show(message, "Start Simulation", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                switch (result)
                {
                    case DialogResult.Yes:
                        ShowInfo("Starting simulation...");
                        simulation.Simulate((trackBarDuration.Value * 3600).Seconds(), trackBarCarFrequency.Value);
                        ShowInfo("The simulation has ended.");
                        UpdateStatistics();
                        break;
                    default:
                        ShowInfo("The simulation did not start.");
                        break;
                }
            }
            else
            {
                string message =
                    "Map check complete: the created map is inconsistent.\n" +
                    "Please make sure that every crossroad has a correctly set-up traffic light.";
                MessageBox.Show(message, "Map Inconsistent", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void UpdateStatistics()
        {
            labelCars.Text = string.Format("Finished cars: {0}", simulation.Statistics.RecordCount);
            labelAvgDistance.Text = string.Format("Average distance: {0}", simulation.Statistics.GetAverageDistance());
            labelAvgDelay.Text = string.Format("Average delay: {0}", simulation.Statistics.GetAverageDelay());
        }

        private void Drag(Point mouseLocation)
        {
            Point delta = new Point(mouseLocation.X - previousMouseLocation.X, mouseLocation.Y - previousMouseLocation.Y);
            origin.Offset(delta);
        }

        private void ChangeZoom(int direction, Point mouseLocation)
        {
            const decimal coefficient = 1.2m;
            // For centering the zoom at coursor position
            decimal originalZoom = Zoom;
            Point newOrigin = origin;
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
                origin = newOrigin;
        }

        private void SelectCrossroad(Point mouseLocation)
        {
            UnselectCrossroad();
            Coords coords = MapManager.CalculateCoords(mouseLocation, origin, Zoom);
            selectedCrossroad = mapManager.GetCrossroad(coords);
            if (selectedCrossroad != null)
                selectedCrossroad.GuiCrossroad.Highlight = GUI.Highlight.High;
            ShowProperties();
        }

        private void UnselectCrossroad()
        {
            if (selectedCrossroad == null)
                return;
            selectedCrossroad.GuiCrossroad.Highlight = GUI.Highlight.Normal;
            selectedCrossroad = null;
        }

        private void SelectRoad(Point mouseLocation)
        {
            UnselectRoad();
            Vector vector = MapManager.CalculateVector(mouseLocation, origin, Zoom);
            selectedRoad = mapManager.GetRoad(vector);
            if (selectedRoad != null)
                selectedRoad.GuiRoad.Highlight = GUI.Highlight.High;
            ShowProperties();
        }

        private void UnselectRoad()
        {
            if (selectedRoad == null)
                return;
            selectedRoad.GuiRoad.Highlight = GUI.Highlight.Normal;
            selectedRoad = null;
        }

        private void Build(Point mouseLocation)
        {
            Coords coords = MapManager.CalculateCoords(mouseLocation, origin, Zoom);
            if (currentRoadBuilder == null)
                currentRoadBuilder = mapManager.GetRoadBuilder(coords, checkBoxTwoWayRoad.Checked);
            else
            {
                currentRoadBuilder.AddSegment(coords);
                if (!currentRoadBuilder.CanContinue)
                    FinishBuild();
            }
        }

        private void FinishBuild()
        {
            if (currentRoadBuilder == null)
                return;
            int maxSpeed = (int)numericUpDownSpeed.Value;
            if (currentRoadBuilder.FinishRoad(maxSpeed.MetersPerSecond()))
                currentRoadBuilder = null;
            else
                ShowInfo("The road cannot be built like this.");
        }

        private void DestroyBuild()
        {
            if (currentRoadBuilder == null)
                return;
            currentRoadBuilder.DestroyRoad();
            currentRoadBuilder = null;
        }

        private void ChangeMode(Mode newMode)
        {
            mode = newMode;
            switch (mode)
            {
                case Mode.Select_Crossroad:
                    groupBoxCrossroad.Visible = true;
                    groupBoxRoad.Visible = false;
                    groupBoxBuild.Visible = false;
                    UnselectRoad();
                    DestroyBuild();
                    break;
                case Mode.Select_Road:
                    groupBoxCrossroad.Visible = false;
                    groupBoxRoad.Visible = true;
                    groupBoxBuild.Visible = false;
                    UnselectCrossroad();
                    DestroyBuild();
                    break;
                case Mode.Build_Road:
                    groupBoxCrossroad.Visible = false;
                    groupBoxRoad.Visible = false;
                    groupBoxBuild.Visible = true;
                    UnselectCrossroad();
                    UnselectRoad();
                    break;
            }
            ShowProperties();
        }

        private void ShowProperties()
        {
            switch (mode)
            {
                case Mode.Select_Crossroad:
                    labelCoords.Text = string.Format("Coords: {0}", selectedCrossroad?.Coords.ToString() ?? "(-;-)");
                    labelInIndex.Text = string.Format("Incoming roads: {0}", selectedCrossroad?.InIndex.ToString() ?? "-");
                    labelOutIndex.Text = string.Format("Outcoming roads: {0}", selectedCrossroad?.OutIndex.ToString() ?? "-");
                    buttonDestroyCrossroad.Enabled = selectedCrossroad != null;
                    buttonTrafficLight.Enabled = selectedCrossroad != null;
                    break;
                case Mode.Select_Road:
                    labelRoadId.Text = string.Format("ID: {0}", selectedRoad?.Id.ToString() ?? "-");
                    labelFrom.Text = string.Format("From: {0}", selectedRoad?.From.ToString() ?? "(-;-)");
                    labelTo.Text = string.Format("To: {0}", selectedRoad?.To.ToString() ?? "(-;-)");
                    labelRoadMaxSpeed.Text = string.Format("Max speed: {0}", selectedRoad?.MaxSpeed.ToString() ?? "-mps");
                    buttonDestroyRoad.Enabled = selectedRoad != null;
                    break;
            }
        }

        private void ShowInfo(string info)
        {
            // TODO
            Debug.Print(info);
        }

        #endregion // helper_methods
    }
}
