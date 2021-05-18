using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Security;
using System.Threading.Tasks;
using System.Windows.Forms;

using RoadTrafficSimulator.ValueTypes;

namespace RoadTrafficSimulator
{
    public partial class FormMain : Form
    {
        private const float minZoom = 0.2f;
        private const float maxZoom = 5f;
        private const float carFrequencyQuotient = 0.03f;
        private static readonly string savePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "RoadTrafficSimulator");

        private enum Mode { Build_Road, Select_Crossroad, Select_Road };

        private MapManager mapManager;
        private Point origin;
        private float zoom; // Do not access directly, use property Zoom
        private Simulation simulation;
        private Mode mode;
        private IRoadBuilder currentRoadBuilder;
        private CrossroadView selectedCrossroad;
        private RoadView selectedRoad;
        private Point prevMouseLocation;
        private bool drag;
        private bool dragOccured;
        private bool freezeMaxSpeed;

        public float Zoom
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
                buttonZoom.Text = $"Zoom: {zoom:0.0}x";
            }
        }

        private Point PanelMapCenter { get => new Point(panelMap.Width / 2, panelMap.Height / 2); }

        public FormMain()
        {
            InitializeComponent();
            // Disable form resizing via maximizing the window
            MaximizeBox = false;
            // Enable double-buffering for panelMap
            typeof(Panel).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.SetProperty
                | System.Reflection.BindingFlags.Instance
                | System.Reflection.BindingFlags.NonPublic,
                null,
                panelMap,
                new object[] { true });
            // Add MouseWheel event for panelMap
            panelMap.MouseWheel += panelMap_MouseWheel;
            // Initialize comboBoxMode
            foreach (Mode m in Enum.GetValues(typeof(Mode)))
                comboBoxMode.Items.Add(m.ToString().Replace('_', ' '));
            comboBoxMode.SelectedIndex = 0;
            mode = 0;

            Components.Map map = new Components.Map();
            mapManager = new MapManager(map);
            origin = PanelMapCenter;
            Zoom = 1f;
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
            prevMouseLocation = e.Location;
        }
        private void panelMap_MouseWheel(object sender, MouseEventArgs e)
        {
            ChangeZoom(e.Delta, e.Location);
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
                mapManager.DestroyCrossroad(selectedCrossroad.GuiCrossroad);
                UnselectCrossroad();
                panelMap.Invalidate();
            }
        }

        private void buttonDestroyRoad_Click(object sender, EventArgs e)
        {
            if (selectedRoad != null)
            {
                mapManager.DestroyRoad(selectedRoad.GuiRoad);
                UnselectRoad();
                panelMap.Invalidate();
            }
        }

        private void buttonSaveMap_Click(object sender, EventArgs e)
        {
            InitializeSaveMap();
        }

        private void buttonLoadMap_Click(object sender, EventArgs e)
        {
            InitializeLoadMap();
            panelMap.Invalidate();
        }

        private void buttonStartSimulation_Click(object sender, EventArgs e)
        {
            InitializeSimulation();
        }

        private void buttonExportStats_Click(object sender, EventArgs e)
        {
            InitializeExportStats();
        }

        private void buttonCenter_Click(object sender, EventArgs e)
        {
            origin = PanelMapCenter;
            panelMap.Invalidate();
        }

        private void buttonZoom_Click(object sender, EventArgs e)
        {
            Zoom = 1f;
            panelMap.Invalidate();
        }

        private void numericUpDownMaxSpeed_ValueChanged(object sender, EventArgs e)
        {
            if (freezeMaxSpeed || selectedRoad == null)
                return;
            selectedRoad.MaxSpeed = ((int)numericUpDownMaxSpeed.Value).MetresPerSecond();
            // Must check whether the road accepted this max speed
            freezeMaxSpeed = true;
            numericUpDownMaxSpeed.Value = selectedRoad.MaxSpeed;
            freezeMaxSpeed = false;
        }

        private void trackBarCarSpawnRate_Scroll(object sender, EventArgs e)
        {
            if (selectedCrossroad != null)
                selectedCrossroad.CarSpawnRate = (byte)trackBarCarSpawnRate.Value;
            labelCarSpawnRate.Text = $"Car spawn rate: {trackBarCarSpawnRate.Value} %";
        }

        private void trackBarDuration_Scroll(object sender, EventArgs e)
        {
            labelDuration.Text = $"Duration: {trackBarDuration.Value}h";
        }

        private void trackBarCarFrequency_Scroll(object sender, EventArgs e)
        {
            labelCarFrequency.Text = $"Car frequency: {(float)trackBarCarFrequency.Value / 100:0.00}";
        }

        private void comboBoxMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeMode(Enum.Parse<Mode>(comboBoxMode.Text.Replace(' ', '_')));
            panelMap.Invalidate();
        }

        #endregion form_events

        #region helper_methods

        private void InitializeSimulation()
        {
            Simulation.InitialisationResult result = simulation.Initialise(out Components.Crossroad invalidCrossroad);
            if (result == Simulation.InitialisationResult.Ok)
                StartSimulation();
            else
            {
                string message;
                switch (result)
                {
                    case Simulation.InitialisationResult.Error_NoMap:
                        message =
                            "Map check complete: the map is empty.\n" +
                            "You have to create a map before starting the simulation.";
                        break;
                    case Simulation.InitialisationResult.Error_InvalidCrossroad:
                        message =
                            $"Map check complete: the traffic light at {invalidCrossroad.Id} is inconsistent.\n" +
                            "Please make sure that every possible direction is allowed at that crossroad.";
                        break;
                    default:
                        message =
                            "Map check complete: cannot start the simulation for an unknown reason.\n" +
                            "If the problem occurs repeatedly, please restart the application.";
                        break;
                }
                MessageBox.Show(message, "Map Inconsistent", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void StartSimulation()
        {
            int duration = trackBarDuration.Value;
            float carFrequency = trackBarCarFrequency.Value / 100f;
            string message = 
                $"Map check complete: all traffic lights are set up correctly.\n" +
                $"Do you want to start the simulation of {duration} hours with {carFrequency:0.00} car frequency?";
            DialogResult result = MessageBox.Show(message, "Start Simulation",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            switch (result)
            {
                case DialogResult.Yes:
                    ShowInfo($"Starting simulation of {duration} hours with {carFrequency:0.00} car frequency...");
                    Time durationSeconds = (duration * 3600).Seconds();
                    float newCarsPerHundredSecondsPerCrossroad = trackBarCarFrequency.Value * carFrequencyQuotient;
                    simulation.Simulate(durationSeconds, newCarsPerHundredSecondsPerCrossroad);
                    ShowInfo("The simulation has ended.");
                    UpdateStatistics();
                    break;
                default:
                    ShowInfo("The simulation did not start.");
                    break;
            }
        }

        private void InitializeSaveMap()
        {
            if (!Directory.Exists(savePath))
                Directory.CreateDirectory(savePath);
            SaveFileDialog fileDialog = new SaveFileDialog()
            {
                Title = "Save map",
                Filter = "Map files (*.map)|*.map",
                InitialDirectory = savePath
            };
            if (fileDialog.ShowDialog() == DialogResult.OK)
                SaveMap(fileDialog.FileName);
        }

        private void SaveMap(string path)
        {
            bool successful = true;
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(path);
                mapManager.SaveMap(sw);
            }
            catch (Exception e) when (e is IOException || e is UnauthorizedAccessException || e is SecurityException)
            {
                string message = $"An error occurred while saving the map: {e.Message}";
                MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                successful = false;
            }
            finally
            {
                sw?.Dispose();
            }
            if (successful)
                ShowInfo("Map successfully saved.");
        }

        private void InitializeLoadMap()
        {
            string message =
                "Are you sure you want to load a new map?\n" +
                "The currently built map will be lost unless it's already saved.";
            DialogResult result = MessageBox.Show(message, "Load Map",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (result == DialogResult.Yes)
            {
                OpenFileDialog fileDialog = new OpenFileDialog()
                {
                    Title = "Load map",
                    Filter = "Map files (*.map)|*.map",
                    InitialDirectory = Directory.Exists(savePath)
                        ? savePath : Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
                };
                if (fileDialog.ShowDialog() == DialogResult.OK)
                    LoadMap(fileDialog.FileName);
            }
        }

        private void LoadMap(string path)
        {
            bool successful = false;
            Components.Map newMap = null;
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(path);
                successful = mapManager.LoadMap(sr, out newMap);
            }
            catch (Exception e) when (e is IOException || e is UnauthorizedAccessException || e is SecurityException)
            {
                string message = $"An error occurred while loading the map: {e.Message}";
                MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                sr?.Dispose();
            }
            if (successful)
            {
                simulation.Map = newMap;
                ShowInfo("Map successfully loaded.");
            }
            else
                ShowInfo("Chosen map couldn't be loaded due to wrong format.");
        }

        private void InitializeExportStats()
        {
            string message = "There are no statistics to export. You need to run the simulation first.";
            if (simulation.Statistics == null)
            {
                MessageBox.Show(message, "No Statistics", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (!Directory.Exists(savePath))
                Directory.CreateDirectory(savePath);
            SaveFileDialog fileDialog = new SaveFileDialog()
            {
                Title = "Export CSV",
                Filter = "CSV files (*.csv)|*.csv",
                InitialDirectory = savePath
            };
            if (fileDialog.ShowDialog() == DialogResult.OK)
                ExportStats(fileDialog.FileName);
        }

        private void ExportStats(string path)
        {
            bool successful = true;
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(path);
                simulation.Statistics.ExportCSV(sw);
            }
            catch (Exception e) when (e is IOException || e is UnauthorizedAccessException || e is SecurityException)
            {
                string message = $"An error occurred while exporting statistics: {e.Message}";
                MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                successful = false;
            }
            finally
            {
                sw?.Dispose();
            }
            if (successful)
                ShowInfo("Statistics successfully exported.");
        }

        private void UpdateStatistics()
        {
            labelCars.Text = string.Format("Finished cars: {0:N0}/{1:N0}", simulation.Statistics.CarsFinished, simulation.Statistics.CarsTotal);
            labelAvgDistance.Text = string.Format("Average distance: {0}", simulation.Statistics.GetAverageDistance());
            labelAvgDuration.Text = string.Format("Average duration: {0}", simulation.Statistics.GetAverageDuration());
            labelAvgDelay.Text = string.Format("Average delay: {0}", simulation.Statistics.GetAverageDelay());
        }

        private void Drag(Point mouseLocation)
        {
            Point delta = new Point(mouseLocation.X - prevMouseLocation.X, mouseLocation.Y - prevMouseLocation.Y);
            origin.Offset(delta);
        }

        private void ChangeZoom(int direction, Point mouseLocation)
        {
            const float coefficient = 1.2f;
            // For centring the zoom at cursor position
            float originalZoom = Zoom;
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
            UnselectCrossroad(false);
            Coords coords = MapManager.CalculateCoords(mouseLocation, origin, Zoom);
            selectedCrossroad = mapManager.GetCrossroad(coords);
            if (selectedCrossroad != null)
                selectedCrossroad.GuiCrossroad.Highlight = GUI.Highlight.High;
            ShowProperties();
        }

        private void UnselectCrossroad(bool updateProperties = true)
        {
            if (selectedCrossroad == null)
                return;
            selectedCrossroad.GuiCrossroad.Highlight = GUI.Highlight.Normal;
            selectedCrossroad = null;
            if (updateProperties)
                ShowProperties();
        }

        private void SelectRoad(Point mouseLocation)
        {
            UnselectRoad(false);
            Vector vector = MapManager.CalculateVector(mouseLocation, origin, Zoom);
            selectedRoad = mapManager.GetRoad(vector);
            if (selectedRoad != null)
                selectedRoad.GuiRoad.Highlight = GUI.Highlight.High;
            ShowProperties();
        }

        private void UnselectRoad(bool updateProperties = true)
        {
            if (selectedRoad == null)
                return;
            selectedRoad.GuiRoad.Highlight = GUI.Highlight.Normal;
            selectedRoad = null;
            if (updateProperties)
                ShowProperties();
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
            if (currentRoadBuilder.FinishRoad())
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
                    UnselectRoad(false);
                    DestroyBuild();
                    break;
                case Mode.Select_Road:
                    groupBoxCrossroad.Visible = false;
                    groupBoxRoad.Visible = true;
                    groupBoxBuild.Visible = false;
                    UnselectCrossroad(false);
                    DestroyBuild();
                    break;
                case Mode.Build_Road:
                    groupBoxCrossroad.Visible = false;
                    groupBoxRoad.Visible = false;
                    groupBoxBuild.Visible = true;
                    UnselectCrossroad(false);
                    UnselectRoad(false);
                    break;
            }
            ShowProperties();
        }

        private void ShowProperties()
        {
            switch (mode)
            {
                case Mode.Select_Crossroad:
                    labelCoords.Text = $"Coords: {selectedCrossroad?.Coords.ToString() ?? "(-;-)"}";
                    labelInIndex.Text = $"Incoming roads: {selectedCrossroad?.InIndex.ToString() ?? "-"}";
                    labelOutIndex.Text = $"Outcoming roads: {selectedCrossroad?.OutIndex.ToString() ?? "-"}";
                    labelCarSpawnRate.Text = $"Car spawn rate: {selectedCrossroad?.CarSpawnRate.ToString() ?? "-"} %";
                    trackBarCarSpawnRate.Value = selectedCrossroad?.CarSpawnRate ?? 10;
                    trackBarCarSpawnRate.Enabled = selectedCrossroad != null;
                    buttonDestroyCrossroad.Enabled = selectedCrossroad != null;
                    buttonTrafficLight.Enabled = selectedCrossroad != null;
                    break;
                case Mode.Select_Road:
                    if (selectedRoad == null)
                        labelTwoWayRoad.Text = "-";
                    else
                        labelTwoWayRoad.Text = selectedRoad.TwoWayRoad ? "Two-way" : "One-way";
                    labelFrom.Text = $"From: {selectedRoad?.From.ToString() ?? "(-;-)"}";
                    labelTo.Text = $"To: {selectedRoad?.To.ToString() ?? "(-;-)"}";
                    freezeMaxSpeed = true;
                    numericUpDownMaxSpeed.Value = selectedRoad?.MaxSpeed ?? numericUpDownMaxSpeed.Minimum;
                    freezeMaxSpeed = false;
                    numericUpDownMaxSpeed.Enabled = selectedRoad != null;
                    buttonDestroyRoad.Enabled = selectedRoad != null;
                    break;
            }
        }

        private void ShowInfo(string info)
        {
            labelInfo.Text = info;
            Debug.WriteLine($"{ DateTime.Now}: {info}");
            labelInfo.Refresh();
        }

        #endregion helper_methods
    }
}
