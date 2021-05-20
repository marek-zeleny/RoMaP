using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using RoadTrafficSimulator.ValueTypes;

namespace RoadTrafficSimulator.Forms
{
    public partial class FormBuild : Form
    {
        private enum Mode { Build, Select };

        private static readonly string savePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "RoadTrafficSimulator");

        private Mode mode;
        private MapManager mapManager;
        private IRoadBuilder currentRoadBuilder;
        private CrossroadView selectedCrossroad;
        private RoadView selectedRoad;
        private bool freezeMaxSpeed;

        internal FormBuild(MapManager mapManager)
        {
            InitializeComponent();
            // Initialize comboBoxMode
            foreach (Mode m in Enum.GetValues(typeof(Mode)))
                comboBoxMode.Items.Add(m.ToString().Replace('_', ' '));
            comboBoxMode.SelectedIndex = 0;
            mode = 0;

            this.mapManager = mapManager;
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
                    switch (mode)
                    {
                        case Mode.Build:
                            Build(e.Location);
                            break;
                        case Mode.Select:
                            Select(e.Location);
                            break;
                        default:
                            break;
                    }
                    break;
                case MouseButtons.Right:
                    switch (mode)
                    {
                        case Mode.Build:
                            DestroyBuild();
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }

        private void mapPanel_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            switch (mode)
            {
                case Mode.Build:
                    FinishBuild();
                    mapPanel.Invalidate();
                    break;
                default:
                    break;
            }
        }

        private void mapPanel_ZoomChanged(object sender, EventArgs e)
        {
            buttonZoom.Text = $"Zoom: {mapPanel.Zoom:0.0}x";
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
            Debug.Assert(selectedCrossroad != null);
            mapManager.DestroyCrossroad(selectedCrossroad.GuiCrossroad);
            Deselect();
            mapPanel.Invalidate();
        }

        private void buttonDestroyRoad_Click(object sender, EventArgs e)
        {
            Debug.Assert(selectedRoad != null);
            mapManager.DestroyRoad(selectedRoad.GuiRoad);
            Deselect();
            mapPanel.Invalidate();
        }

        private void buttonSaveMap_Click(object sender, EventArgs e)
        {
            InitializeSaveMap();
        }

        private void buttonLoadMap_Click(object sender, EventArgs e)
        {
            InitializeLoadMap();
            mapPanel.Invalidate();
        }

        private void buttonCenter_Click(object sender, EventArgs e)
        {
            mapPanel.ResetOrigin();
        }

        private void buttonZoom_Click(object sender, EventArgs e)
        {
            mapPanel.Zoom = 1f;
        }

        private void buttonFinish_Click(object sender, EventArgs e)
        {
            Close();
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

        private void comboBoxMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeMode(Enum.Parse<Mode>(comboBoxMode.Text.Replace(' ', '_')));
            mapPanel.Invalidate();
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
                groupBoxCrossroad.Visible = false;
            }
            if (selectedRoad != null)
            {
                selectedRoad.GuiRoad.Highlight = GUI.Highlight.Normal;
                selectedRoad = null;
                groupBoxRoad.Visible = false;
            }
        }

        private void SelectCrossroad(CrossroadView view)
        {
            selectedCrossroad = view;
            selectedCrossroad.GuiCrossroad.Highlight = GUI.Highlight.High;
            groupBoxCrossroad.Visible = true;
            ShowProperties();
        }
        
        private void SelectRoad(RoadView view)
        {
            selectedRoad = view;
            selectedRoad.GuiRoad.Highlight = GUI.Highlight.High;
            groupBoxRoad.Visible = true;
            ShowProperties();
        }

        private void Build(Point mouseLocation)
        {
            Coords coords = MapManager.CalculateCoords(mouseLocation, mapPanel.Origin, mapPanel.Zoom);
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
                case Mode.Build:
                    Deselect();
                    groupBoxBuild.Visible = true;
                    break;
                case Mode.Select:
                    DestroyBuild();
                    groupBoxBuild.Visible = false;
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        private void ShowProperties()
        {
            if (selectedCrossroad != null)
            {
                labelCoords.Text = $"Coords: {selectedCrossroad.Coords}";
                labelInIndex.Text = $"Incoming roads: {selectedCrossroad.InIndex}";
                labelOutIndex.Text = $"Outcoming roads: {selectedCrossroad.OutIndex}";
                labelCarSpawnRate.Text = $"Car spawn rate: {selectedCrossroad.CarSpawnRate} %";
                trackBarCarSpawnRate.Value = selectedCrossroad.CarSpawnRate;
            }
            else if (selectedRoad != null)
            {
                labelTwoWayRoad.Text = selectedRoad.TwoWayRoad ? "Two-way" : "One-way";
                labelFrom.Text = $"From: {selectedRoad.From}";
                labelTo.Text = $"To: {selectedRoad.To}";
                freezeMaxSpeed = true;
                numericUpDownMaxSpeed.Value = selectedRoad.MaxSpeed;
                freezeMaxSpeed = false;
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
            catch (Exception e) when (
                e is IOException ||
                e is UnauthorizedAccessException ||
                e is System.Security.SecurityException)
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
            catch (Exception e) when (
                e is IOException ||
                e is UnauthorizedAccessException ||
                e is System.Security.SecurityException)
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

        private void ShowInfo(string info)
        {
            labelInfo.Text = info;
            Debug.WriteLine($"{ DateTime.Now}: {info}");
            labelInfo.Refresh();
        }

        #endregion helper_methods
    }
}
