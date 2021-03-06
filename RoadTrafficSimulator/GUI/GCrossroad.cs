using System;
using System.Drawing;

using RoadTrafficSimulator.ValueTypes;

namespace RoadTrafficSimulator.GUI
{
    /// <summary>
    /// Represents graphical properties of a crossroad.
    /// </summary>
    class GCrossroad : IGCrossroad
    {
        private const int size = 20;
        private static readonly Color color = Color.Brown;

        private Highlight highlight;

        public Coords CrossroadId { get; }
        public (CoordsConvertor.Direction, CoordsConvertor.Direction)? MainRoadDirections { get; set; }

        /// <summary>
        /// Creates a new crossroad at given coordinates.
        /// </summary>
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

        public void Draw(Graphics graphics, Point origin, float zoom, Func<Point, bool> isVisible)
        {
            Point point = CoordsConvertor.CalculatePoint(CrossroadId, origin, zoom);
            if (!isVisible(point))
                return;

            Color color = GCrossroad.color;
            int size = (int)(GCrossroad.size * zoom);
            if (highlight.HasFlag(Highlight.Transparent))
                color = Color.FromArgb(150, color);
            if (highlight.HasFlag(Highlight.Large))
                size = GetIncreasedSize(size);

            Brush brush = new SolidBrush(color);
            int halfSize = size / 2;
            point.Offset(-halfSize, -halfSize);
            Rectangle rect = new(point, new Size(size, size));
            graphics.FillEllipse(brush, rect);
        }

        /// <summary>
        /// Gets an increased size of the crossroad given its normal size.
        /// </summary>
        public static int GetIncreasedSize(int size) => size * 3 / 2;
    }
}
