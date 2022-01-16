using System;
using System.Drawing;

using RoadTrafficSimulator.ValueTypes;

namespace RoadTrafficSimulator.GUI
{
    class GCrossroad : IGCrossroad
    {
        private static readonly Color color = Color.Brown;

        private Highlight highlight;

        public Coords CrossroadId { get; }
        public (Coords, Coords)? MainRoadDirections { get; set; }

        public GCrossroad(Coords crossroadId)
        {
            CrossroadId = crossroadId;
        }

        public void ResetHighlight(Highlight highlight)
        {
            this.highlight = highlight;
        }

        public void SetHighlight(Highlight highlight)
        {
            this.highlight |= highlight;
        }

        public void UnsetHighlight(Highlight highlight)
        {
            this.highlight &= ~highlight;
        }

        public void Draw(Graphics graphics, Point point, int size)
        {
            Color color = GCrossroad.color;
            if (highlight.HasFlag(Highlight.Transparent))
                color = Color.FromArgb(150, color);
            if (highlight.HasFlag(Highlight.Large))
                size += size / 2;

            int halfSize = size / 2;
            point.Offset(-halfSize, -halfSize);
            Rectangle rectangle = new Rectangle(point, new Size(size, size));
            graphics.FillEllipse(new SolidBrush(color), rectangle);
        }
    }
}
