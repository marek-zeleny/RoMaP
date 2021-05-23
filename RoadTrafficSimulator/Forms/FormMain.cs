using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Security;
using System.Threading.Tasks;
using System.Windows.Forms;

using RoadTrafficSimulator.ValueTypes;

namespace RoadTrafficSimulator.Forms
{
    public partial class FormMain : Form
    {
        private const float carFrequencyQuotient = 0.03f;
        private static readonly string savePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "RoadTrafficSimulator");

        private MapManager mapManager;
        private Simulation simulation;
        private FormBuild buildForm;
        private FormSimulationSettings settingsForm;
        private CrossroadView selectedCrossroad;
        private RoadView selectedRoad;

        public FormMain()
        {
            InitializeComponent();
            mapManager = new MapManager();
            simulation = new Simulation();
            buildForm = new FormBuild(mapManager);
            buildForm.VisibleChanged += (_, _) => Visible = !buildForm.Visible;
            settingsForm = new FormSimulationSettings();
        }

        #region form_events

        private void mapPanel_Paint(object sender, PaintEventArgs e)
        {
            mapManager.Draw(e.Graphics, mapPanel.Origin, mapPanel.Zoom, mapPanel.Width, mapPanel.Height);
        }

        private void mapPanel_MouseClick(object sender, MouseEventArgs e)
        {
            // TODO
        }

        private void buttonCenter_Click(object sender, EventArgs e)
        {
            mapPanel.ResetOrigin();
        }

        private void buttonZoom_Click(object sender, EventArgs e)
        {
            mapPanel.Zoom = 1f;
        }

        private void buttonBuildMap_Click(object sender, EventArgs e)
        {
            buildForm.Show();
        }

        private void buttonSimulate_Click(object sender, EventArgs e)
        {
            InitializeSimulation();
        }

        #endregion form_events

        #region helper_methods

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

        private void ShowInfo(string info)
        {
            labelInfo.Text = info;
            Debug.WriteLine($"{ DateTime.Now}: {info}");
            labelInfo.Refresh();
        }

        #endregion helper_methods
    }
}
