using System;
using System.Drawing;

using RoadTrafficSimulator.ValueTypes;

namespace RoadTrafficSimulator.GUI
{
    class GCrossroad : IGCrossroad
    {
        private static readonly Color color = Color.Brown;

        public Highlight Highlight { private get; set; }

        public Coords CrossroadId { get; }
        public (Coords, Coords)? MainRoadDirections { get; set; }

        public GCrossroad(Coords crossroadId)
        {
            CrossroadId = crossroadId;
        }

        public void Draw(Graphics graphics, Point point, int size)
        {
            Color color = GCrossroad.color;
            switch (Highlight)
            {
                case Highlight.Low:
                    color = Color.FromArgb(150, color);
                    break;
                case Highlight.High:
                    size += size / 2;
                    break;
            }
            int halfSize = size / 2;
            point.Offset(-halfSize, -halfSize);
            Rectangle rectangle = new Rectangle(point, new Size(size, size));
            graphics.FillEllipse(new SolidBrush(color), rectangle);
        }
    }
}
