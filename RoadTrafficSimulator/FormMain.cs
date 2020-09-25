using RoadTrafficSimulator.ValueTypes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RoadTrafficSimulator
{
    public partial class FormMain : Form
    {
        private MapManager manager;
        private Simulation simulation;
        private IRoadBuilder roadBuilder;

        public FormMain()
        {
            InitializeComponent();
            Components.Map map = new Components.Map();
            manager = new MapManager(map);
            simulation = new Simulation(map);
            manager.Origin = new Point(panelMap.Width / 2, panelMap.Height / 2);
            manager.Zoom = 0.5m;
        }

        private void panelMap_Paint(object sender, PaintEventArgs e)
        {
            manager.Draw(e.Graphics, panelMap.Width, panelMap.Height);
        }

        private void panelMap_Click(object sender, EventArgs e)
        {
            Point point = panelMap.PointToClient(Cursor.Position);
            Coords coords = MapManager.CalculateCoords(point, manager.Origin, manager.Zoom);
            if (roadBuilder == null)
                roadBuilder = manager.GetRoadBuilder(coords, false);
            else
                roadBuilder.AddSegment(coords);
            panelMap.Invalidate();
        }
    }
}
