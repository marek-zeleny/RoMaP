using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Security;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RoadTrafficSimulator.Forms
{
    public partial class FormMain : Form
    {
        private const float carFrequencyQuotient = 0.03f;
        private static readonly string savePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "RoadTrafficSimulator");

        private MapManager mapManager;
        private Simulation simulation;
        private CrossroadView selectedCrossroad;
        private RoadView selectedRoad;

        public FormMain()
        {
            InitializeComponent();
            Components.Map map = new Components.Map();
            mapManager = new MapManager(map);
            simulation = new Simulation(map);
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
            FormBuild form = new FormBuild(mapManager);
            form.FormClosed += (object sender, FormClosedEventArgs e) => Visible = true;
            Visible = false;
            form.Show();
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
            /*/
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
                    //simulation.Simulate(durationSeconds, newCarsPerHundredSecondsPerCrossroad);
                    ShowInfo("The simulation has ended.");
                    UpdateStatistics();
                    break;
                default:
                    ShowInfo("The simulation did not start.");
                    break;
            }
            /**/
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
