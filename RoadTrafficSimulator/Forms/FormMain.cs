using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Security;
using System.Threading.Tasks;
using System.Windows.Forms;

using RoadTrafficSimulator.GUI;
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
        private Func<Time, bool> continueSimulation;
        private FormSimulationSettings settingsForm;
        private Mode mode;
        private MapManager.CrossroadWrapper? selectedCrossroad;
        private IGRoad selectedRoad;
        private IRoadBuilder currentRoadBuilder;

        public FormMain()
        {
            InitializeComponent();
            mapManager = new MapManager();
            simulation = new Simulation();
            settingsForm = new FormSimulationSettings();
            mapPanel.ResetOrigin();
            timerSimulation.Interval = Simulation.MinTimeStep.ToMilliseconds();
            mode = Mode.Build;
            ChangeMode();
        }

        #region form_events

        private void mapPanel_Paint(object sender, PaintEventArgs e)
        {
            mapManager.Draw(e.Graphics, mapPanel.Origin, mapPanel.Zoom, mapPanel.Width, mapPanel.Height,
                mode == Mode.Simulate);
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

        private void buttonPause_Click(object sender, EventArgs e)
        {
            PauseSimulation();
        }

        private void buttonContinue_Click(object sender, EventArgs e)
        {
            UnpauseSimulation();
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            EndSimulation();
        }

        private void buildPanel_TrafficLightClick(object sender, EventArgs e)
        {
            Debug.Assert(selectedCrossroad.HasValue);
            FormTrafficLight form = new(mapManager, selectedCrossroad.Value);
            form.ShowDialog();
        }

        private void buildPanel_DestroyCrossroadClick(object sender, EventArgs e)
        {
            Debug.Assert(selectedCrossroad.HasValue);
            // need to save selected crossroad temporarily before deselection
            IGCrossroad gCrossroad = selectedCrossroad.Value.gCrossroad;
            Deselect();
            mapManager.DestroyCrossroad(gCrossroad);
            mapPanel.Redraw();
        }

        private void buildPanel_CloseRoadClick(object sender, EventArgs e)
        {
            Debug.Assert(selectedRoad != null);
            buildPanel.Deselect();
            mapManager.CloseRoad(selectedRoad);
            buildPanel.SelectRoad(selectedRoad);
            mapPanel.Redraw();
        }

        private void buildPanel_OpenRoadClick(object sender, EventArgs e)
        {
            Debug.Assert(selectedRoad != null);
            buildPanel.Deselect();
            mapManager.OpenRoad(selectedRoad);
            buildPanel.SelectRoad(selectedRoad);
            mapPanel.Redraw();
        }

        private void buildPanel_DestroyRoadClick(object sender, EventArgs e)
        {
            Debug.Assert(selectedRoad != null);
            // need to save selected road temporarily before deselection
            IGRoad gRoad = selectedRoad;
            Deselect();
            mapManager.DestroyRoad(gRoad);
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

        private void buildPanel_CurrentRoadSideChanged(object sender, EventArgs e)
        {
            MapManager.roadSide = buildPanel.CurrentRoadSide switch
            {
                BuildPanel.RoadSide.Right => MapManager.RoadSide.Right,
                BuildPanel.RoadSide.Left => MapManager.RoadSide.Left,
                _ => throw new NotImplementedException(),
            };
            mapPanel.Redraw();
        }

        private void buildPanel_LengthChanged(object sender, EventArgs e)
        {
            Debug.Assert(selectedRoad != null);
            Components.Road road = selectedRoad.GetRoad();
            Distance length = buildPanel.Length.Metres();
            road.Length = length;
            // Must check whether the road accepted this length
            buildPanel.Length = road.Length.ToMetres();
            // If it's a two-way road, we need to adjust the opposite direction as well
            road = selectedRoad.GetRoad(IGRoad.Direction.Backward);
            if (road != null)
                road.Length = length;
        }

        private void buildPanel_MaxSpeedChanged(object sender, EventArgs e)
        {
            Debug.Assert(selectedRoad != null);
            Components.Road road = selectedRoad.GetRoad();
            road.MaxSpeed = buildPanel.MaxSpeed.KilometresPerHour();
            // Must check whether the road accepted this max speed
            buildPanel.MaxSpeed = road.MaxSpeed.ToKilometresPerHour();
        }

        private void buildPanel_SpawnRateChanged(object sender, EventArgs e)
        {
            Debug.Assert(selectedCrossroad.HasValue);
            selectedCrossroad.Value.crossroad.CarSpawnRate = (byte)buildPanel.SpawnRate;
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

        private void timerSimulation_Tick(object sender, EventArgs e)
        {
            Time timestep = (timerSimulation.Interval * simulationPanel.SimulationSpeed).Milliseconds();
            if (!continueSimulation(timestep))
                EndSimulation();
            simulationPanel.SimulationTime = simulation.Clock.Time;
            mapPanel.Redraw();
            if (selectedRoad != null)
                simulationPanel.UpdateChart();
        }

        #endregion form_events

        #region control_methods

        private void Select(Point mouseLocation)
        {
            const double crossroadMaxProximity = 0.2;

            Deselect();
            var crossroad = mapManager.GetNearestCrossroad(mouseLocation, mapPanel.Origin, mapPanel.Zoom,
                out double proximity);
            var road = mapManager.GetNearestRoad(mouseLocation, mapPanel.Origin, mapPanel.Zoom);
            if (crossroad != null && proximity <= crossroadMaxProximity)
                SelectCrossroad(crossroad.Value);
            else if (road != null)
                SelectRoad(road);
        }

        private void Deselect()
        {
            if (selectedCrossroad.HasValue)
            {
                selectedCrossroad.Value.gCrossroad.UnsetHighlight(Highlight.Large);
                selectedCrossroad = null;
            }
            if (selectedRoad != null)
            {
                selectedRoad.UnsetHighlight(Highlight.Large, IGRoad.Direction.Forward);
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

        private void SelectCrossroad(MapManager.CrossroadWrapper crossroad)
        {
            selectedCrossroad = crossroad;
            selectedCrossroad.Value.gCrossroad.SetHighlight(Highlight.Large);
            if (crossroad.crossroad == null)
                return;
            switch (mode)
            {
                case Mode.Simulate:
                    simulationPanel.SelectCrossroad(crossroad.crossroad);
                    break;
                case Mode.Build:
                    buildPanel.SelectCrossroad(crossroad.crossroad);
                    break;
                default:
                    break;
            }
        }

        private void SelectRoad(IGRoad road)
        {
            if (road.GetRoad() != null)
                selectedRoad = road;
            else
            {
                Debug.Assert(road.GetRoad(IGRoad.Direction.Backward) != null);
                selectedRoad = road.GetReversedGRoad();
            }
            selectedRoad.SetHighlight(Highlight.Large, IGRoad.Direction.Forward);
            switch (mode)
            {
                case Mode.Simulate:
                    simulationPanel.SelectRoad(selectedRoad, simulation.Clock);
                    break;
                case Mode.Build:
                    buildPanel.SelectRoad(selectedRoad);
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
                            if (selectedCrossroad.HasValue)
                                buildPanel.SelectCrossroad(selectedCrossroad.Value.crossroad);
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
                    if (selectedCrossroad.HasValue)
                        simulationPanel.SelectCrossroad(selectedCrossroad.Value.crossroad);
                    buttonMode.Text = "Build Map";
                    mode = Mode.Simulate;
                    break;
                default:
                    throw new InvalidOperationException();
            }
            ResumeLayout();
            mapPanel.Redraw();
        }

        #endregion control_methods

        #region action_methods

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
            SuspendLayout();
            buttonSimulate.Visible = false;
            buttonMode.Visible = false;
            buttonPause.Visible = true;
            buttonStop.Visible = true;
            buttonContinue.Visible = false;
            ResumeLayout();
            ShowInfo($"Starting simulation of {settings.Duration.ToHours()} hours...");
            simulation.StartSimulation(settings, out continueSimulation);
            timerSimulation.Start();
        }

        private void PauseSimulation()
        {
            Debug.Assert(simulation.IsRunning);
            timerSimulation.Stop();
            SuspendLayout();
            buttonPause.Visible = false;
            buttonContinue.Visible = true;
            ResumeLayout();
            ShowInfo("Simulation paused.");
        }

        private void UnpauseSimulation()
        {
            Debug.Assert(simulation.IsRunning);
            SuspendLayout();
            buttonPause.Visible = true;
            buttonContinue.Visible = false;
            ResumeLayout();
            ShowInfo("Continuing simulation...");
            timerSimulation.Start();
        }

        private void EndSimulation()
        {
            timerSimulation.Stop();
            SuspendLayout();
            buttonSimulate.Visible = true;
            buttonMode.Visible = true;
            buttonPause.Visible = false;
            buttonStop.Visible = false;
            buttonContinue.Visible = false;
            ResumeLayout();
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
                Filter = "JSON files (*.json)|*.json",
                InitialDirectory = savePath
            };
            if (fileDialog.ShowDialog() == DialogResult.OK)
                SaveMap(fileDialog.FileName);
        }

        private void SaveMap(string path)
        {
            bool successful = true;
            Stream stream = null;
            try
            {
                stream = new FileStream(path, FileMode.Create, FileAccess.Write);
                mapManager.SaveMap(stream);
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
                stream?.Dispose();
            }
            if (successful)
                ShowInfo("Map successfully saved.");
            else
                ShowInfo("Saving map failed due to unknown error.");
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
                    Filter = "JSON files (*.json)|*.json",
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
            Stream stream = null;
            try
            {
                stream = File.OpenRead(path);
                successful = mapManager.LoadMap(stream);
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
                stream?.Dispose();
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
            Debug.WriteLine($"{ DateTime.Now}: {info}");
            labelInfo.Text = info;
            labelInfo.Refresh();
        }

        #endregion action_methods
    }
}
