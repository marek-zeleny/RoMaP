using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using System.Windows.Forms;

using RoadTrafficSimulator.ValueTypes;

namespace RoadTrafficSimulator.Forms
{
    public partial class FormMain : Form
    {
        private enum Mode { Simulate, Build };

        private static readonly string savePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "RoadTrafficSimulator");

        private MapManager mapManager;
        private Simulation simulation;
        //private FormBuild buildForm;
        private FormSimulationSettings settingsForm;
        private Mode mode;
        private CrossroadView selectedCrossroad;
        private RoadView selectedRoad;
        private IRoadBuilder currentRoadBuilder;

        public FormMain()
        {
            InitializeComponent();
            mapManager = new MapManager();
            simulation = new Simulation();
            //buildForm = new FormBuild(mapManager);
            //buildForm.VisibleChanged += (_, _) => Visible = !buildForm.Visible;
            settingsForm = new FormSimulationSettings();
            mode = Mode.Build;
            ChangeMode();
        }

        #region form_events

        private void mapPanel_Paint(object sender, PaintEventArgs e)
        {
            mapManager.Draw(e.Graphics, mapPanel.Origin, mapPanel.Zoom, mapPanel.Width, mapPanel.Height);
        }

        private void mapPanel_MouseClick(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    if (mode == Mode.Build && buildPanel.CurrentMode == BuildPanel.Mode.Build)
                        Build(e.Location);
                    else
                        Select(e.Location);
                    break;
                case MouseButtons.Right:
                    if (mode == Mode.Build && buildPanel.CurrentMode == BuildPanel.Mode.Build)
                        DestroyBuild();
                    break;
                default:
                    break;
            }
            mapPanel.Redraw();
        }

        private void mapPanel_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (mode == Mode.Build && buildPanel.CurrentMode == BuildPanel.Mode.Build)
            {
                FinishBuild();
                mapPanel.Redraw();
            }
        }

        private void mapPanel_ZoomChanged(object sender, EventArgs e)
        {
            buttonZoom.Text = $"Zoom: {mapPanel.Zoom:0.0}x";
        }

        private void buttonCenter_Click(object sender, EventArgs e)
        {
            mapPanel.ResetOrigin();
        }

        private void buttonZoom_Click(object sender, EventArgs e)
        {
            mapPanel.Zoom = 1f;
        }

        private void buttonMode_Click(object sender, EventArgs e)
        {
            ChangeMode();
        }

        private void buttonSimulate_Click(object sender, EventArgs e)
        {
            InitializeSimulation();
        }

        private void buildPanel_TrafficLightClick(object sender, EventArgs e)
        {
            Debug.Assert(selectedCrossroad != null);
            FormTrafficLight form = new(mapManager, selectedCrossroad);
            form.ShowDialog();
        }

        private void buildPanel_DestroyCrossroadClick(object sender, EventArgs e)
        {
            Debug.Assert(selectedCrossroad != null);
            mapManager.DestroyCrossroad(selectedCrossroad.GuiCrossroad);
            Deselect();
            mapPanel.Redraw();
        }

        private void buildPanel_DestroyRoadClick(object sender, EventArgs e)
        {
            Debug.Assert(selectedRoad != null);
            mapManager.DestroyRoad(selectedRoad.GuiRoad);
            Deselect();
            mapPanel.Redraw();
        }

        private void buildPanel_SaveMapClick(object sender, EventArgs e)
        {
            InitializeSaveMap();
        }

        private void buildPanel_LoadMapClick(object sender, EventArgs e)
        {
            InitializeLoadMap();
            mapPanel.Redraw();
        }

        private void buildPanel_MaxSpeedChanged(object sender, EventArgs e)
        {
            Debug.Assert(selectedRoad != null);
            selectedRoad.MaxSpeed = buildPanel.MaxSpeed.KilometresPerHour();
            // Must check whether the road accepted this max speed
            buildPanel.MaxSpeed = selectedRoad.MaxSpeed.ToKilometresPerHour();
        }

        private void buildPanel_SpawnRateChanged(object sender, EventArgs e)
        {
            Debug.Assert(selectedCrossroad != null);
            selectedCrossroad.CarSpawnRate = (byte)buildPanel.SpawnRate;
        }

        private void buildPanel_CurrentModeChanged(object sender, EventArgs e)
        {
            switch (buildPanel.CurrentMode)
            {
                case BuildPanel.Mode.Build:
                    Deselect();
                    break;
                case BuildPanel.Mode.Select:
                    DestroyBuild();
                    break;
                default:
                    break;
            }
        }

        #endregion form_events

        #region helper_methods

        private void Select(Point mouseLocation)
        {
            const double proximityCoeff = 0.2;
            static double CalculateDistance(Point p1, Point p2)
            {
                int dx = p1.X - p2.X;
                int dy = p1.Y - p2.Y;
                return Math.Sqrt(dx * dx + dy * dy);
            }

            Deselect();
            Coords coords = MapManager.CalculateCoords(mouseLocation, mapPanel.Origin, mapPanel.Zoom);
            CrossroadView nearestCrossroad = mapManager.GetCrossroad(coords);
            // No crossroad in the area - select nearby road (if exists)
            if (nearestCrossroad == null)
            {
                var roads = mapManager.GetAllRoads(coords);
                if (roads.Any())
                    SelectRoad(roads.First());
                return;
            }
            // Crossroad nearby - decide based on the distance from mouseLocation
            Vector vector = MapManager.CalculateVector(mouseLocation, mapPanel.Origin, mapPanel.Zoom);
            Debug.Assert(vector.from == coords);
            Point from = MapManager.CalculatePoint(vector.from, mapPanel.Origin, mapPanel.Zoom);
            Point to = MapManager.CalculatePoint(vector.to, mapPanel.Origin, mapPanel.Zoom);
            double length = CalculateDistance(from, to);
            double distance = CalculateDistance(from, mouseLocation);
            if (distance < length * proximityCoeff)
                SelectCrossroad(nearestCrossroad);
            else
            {
                RoadView nearestRoad = mapManager.GetRoad(vector);
                if (nearestRoad != null)
                    SelectRoad(nearestRoad);
            }
        }

        private void Deselect()
        {
            if (selectedCrossroad != null)
            {
                selectedCrossroad.GuiCrossroad.Highlight = GUI.Highlight.Normal;
                selectedCrossroad = null;
            }
            if (selectedRoad != null)
            {
                selectedRoad.GuiRoad.Highlight = GUI.Highlight.Normal;
                selectedRoad = null;
            }
            switch (mode)
            {
                case Mode.Simulate:
                    simulationPanel.Deselect();
                    break;
                case Mode.Build:
                    buildPanel.Deselect();
                    break;
                default:
                    break;
            }
        }

        private void SelectCrossroad(CrossroadView view)
        {
            selectedCrossroad = view;
            selectedCrossroad.GuiCrossroad.Highlight = GUI.Highlight.High;
            switch (mode)
            {
                case Mode.Simulate:
                    simulationPanel.SelectCrossroad(view);
                    break;
                case Mode.Build:
                    buildPanel.SelectCrossroad(view);
                    break;
                default:
                    break;
            }
        }

        private void SelectRoad(RoadView view)
        {
            selectedRoad = view;
            selectedRoad.GuiRoad.Highlight = GUI.Highlight.High;
            switch (mode)
            {
                case Mode.Simulate:
                    simulationPanel.SelectRoad(view, simulation.Clock);
                    break;
                case Mode.Build:
                    buildPanel.SelectRoad(view);
                    break;
                default:
                    break;
            }
        }

        private void Build(Point mouseLocation)
        {
            Coords coords = MapManager.CalculateCoords(mouseLocation, mapPanel.Origin, mapPanel.Zoom);
            if (currentRoadBuilder == null)
                currentRoadBuilder = mapManager.GetRoadBuilder(coords, buildPanel.TwoWayRoad);
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

        private void ChangeMode()
        {
            SuspendLayout();
            switch (mode)
            {
                case Mode.Simulate:
                    simulationPanel.Deselect();
                    simulationPanel.Visible = false;
                    buildPanel.Visible = true;
                    buttonSimulate.Enabled = false;
                    switch (buildPanel.CurrentMode)
                    {
                        case BuildPanel.Mode.Build:
                            Deselect();
                            break;
                        case BuildPanel.Mode.Select:
                            if (selectedRoad != null)
                                buildPanel.SelectRoad(selectedRoad);
                            if (selectedCrossroad != null)
                                buildPanel.SelectCrossroad(selectedCrossroad);
                            break;
                        default:
                            break;
                    }
                    buttonMode.Text = "Finish Build";
                    mode = Mode.Build;
                    break;
                case Mode.Build:
                    DestroyBuild();
                    buildPanel.Deselect();
                    buildPanel.Visible = false;
                    simulationPanel.Visible = true;
                    buttonSimulate.Enabled = true;
                    if (selectedRoad != null)
                        simulationPanel.SelectRoad(selectedRoad, simulation.Clock);
                    if (selectedCrossroad != null)
                        simulationPanel.SelectCrossroad(selectedCrossroad);
                    buttonMode.Text = "Build Map";
                    mode = Mode.Simulate;
                    break;
                default:
                    throw new InvalidOperationException();
            }
            ResumeLayout();
            mapPanel.Redraw();
        }

        private void InitializeSimulation()
        {
            var initResult = simulation.Initialise(mapManager.Map, out Components.Crossroad invalidCrossroad);
            if (initResult == Simulation.InitialisationResult.Ok)
            {
                var settingsResult = settingsForm.ShowDialog();
                Debug.WriteLine($"Settings dialog result: {settingsResult}");
                if (settingsResult == DialogResult.OK)
                    StartSimulation(settingsForm.Settings);
                else
                    ShowInfo("The simulation was cancelled.");
            }
            else
            {
                string message;
                switch (initResult)
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

        private void StartSimulation(SimulationSettings settings)
        {
            ShowInfo($"Starting simulation of {settings.Duration.ToHours()} hours...");
            simulation.Simulate(settings);
            ShowInfo("The simulation has ended.");
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
            SaveFileDialog fileDialog = new()
            {
                Title = "Export CSV",
                Filter = "CSV files (*.csv)|*.csv",
                InitialDirectory = savePath
            };
            if (fileDialog.ShowDialog() == DialogResult.OK)
                ExportStats(fileDialog.FileName);
        }

        private void InitializeSaveMap()
        {
            if (!Directory.Exists(savePath))
                Directory.CreateDirectory(savePath);
            SaveFileDialog fileDialog = new()
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
            catch (Exception e) when (
                e is IOException ||
                e is UnauthorizedAccessException ||
                e is SecurityException)
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
                OpenFileDialog fileDialog = new()
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
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(path);
                successful = mapManager.LoadMap(sr);
            }
            catch (Exception e) when (
                e is IOException ||
                e is UnauthorizedAccessException ||
                e is SecurityException)
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
                ShowInfo("Map successfully loaded.");
            }
            else
                ShowInfo("Chosen map couldn't be loaded due to wrong format.");
        }

        private void ExportStats(string path)
        {
            bool successful = true;
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(path);
                //simulation.Statistics.ExportCSV(sw);
            }
            catch (Exception e) when (
                e is IOException ||
                e is UnauthorizedAccessException ||
                e is SecurityException)
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

        private void ShowInfo(string info)
        {
            labelInfo.Text = info;
            Debug.WriteLine($"{ DateTime.Now}: {info}");
            labelInfo.Refresh();
        }

        #endregion helper_methods
    }
}
