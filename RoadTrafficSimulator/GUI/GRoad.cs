using System;
using System.Collections.Generic;
using System.Drawing;

using RoadTrafficSimulator.ValueTypes;
using RoadTrafficSimulator.Components;

namespace RoadTrafficSimulator.GUI
{
    class GRoad : IMutableRoad
    {
        private static readonly Color color = Color.Blue;
        private static readonly Color arrowColor = Color.Yellow;

        private Road fRoad;
        private Road bRoad;

        public Coords From { get => Route[0]; }
        public Coords To { get => Route[Route.Count - 1]; }
        public bool IsTwoWay { get => fRoad != null && bRoad != null; }
        public Highlight Highlight { private get; set; }
        public IList<Coords> Route { get; } = new List<Coords>();

        public IEnumerable<Road> GetRoads()
        {
            if (fRoad != null)
                yield return fRoad;
            if (bRoad != null)
                yield return bRoad;
        }

        public void SetRoad(Road road, IMutableRoad.Direction direction)
        {
            if (direction == IMutableRoad.Direction.Forward)
                fRoad = road;
            else
                bRoad = road;
        }

        public IEnumerable<Coords> GetRoute()
        {
            return Route;
        }

        public void Draw(Graphics graphics, Point from, Point to, int width)
        {
            if (!IsTwoWay)
            {
                DrawLane(graphics, from, to, width);
                return;
            }
            // two-way
            Point from1 = from;
            Point to1 = to;
            Point from2 = to;
            Point to2 = from;
            int distance = width * 3 / 4;
            if (Highlight == Highlight.High)
                distance = IncreaseWidth(width) * 3 / 4;
            int diffX = to.X - from.X;
            int diffY = to.Y - from.Y;
            if (diffX == 0)
            {
                if (diffY == 0)
                    throw new ArgumentException($"The given points cannot be identical.");
                else if (diffY > 0)
                    distance = -distance;
                from1.Offset(distance, 0);
                to1.Offset(distance, 0);
                from2.Offset(-distance, 0);
                to2.Offset(-distance, 0);
            }
            else
            {
                if (diffY != 0)
                    throw new ArgumentException($"The given points must be horizontally or vertically aligned.");
                else if (diffX < 0)
                    distance = -distance;
                from1.Offset(0, distance);
                to1.Offset(0, distance);
                from2.Offset(0, -distance);
                to2.Offset(0, -distance);
            }
            DrawLane(graphics, from1, to1, width);
            DrawLane(graphics, from2, to2, width);
        }

        private void DrawLane(Graphics graphics, Point from, Point to, int width)
        {
            // TODO: one-way
            Color color = GRoad.color;
            switch (Highlight)
            {
                case Highlight.Low:
                    color = Color.FromArgb(150, color);
                    break;
                case Highlight.High:
                    width = IncreaseWidth(width);
                    break;
            }
            Pen pen = new Pen(color, width);
            graphics.DrawLine(pen, from, to);

            float ratio = 0.6f;
            float invRatio = 1 - ratio;
            PointF arrowFrom = new PointF(from.X * ratio + to.X * invRatio, from.Y * ratio + to.Y * invRatio);
            PointF arrowTo = new PointF(to.X * ratio + from.X * invRatio, to.Y * ratio + from.Y * invRatio);
            Brush brush = new SolidBrush(arrowColor);
            graphics.FillArrow(brush, arrowFrom, arrowTo, width / 2);
        }

        private static int IncreaseWidth(int width) => width + width / 2;
    }

    static class GraphicsExtensions
    {
        public static void FillArrow(this Graphics graphics, Brush brush, PointF from, PointF to, int width)
        {
            PointF[] points = new PointF[8];
            points[0] = to;
            points[7] = to;

            PointF vector = new PointF(to.X - from.X, to.Y - from.Y);
            PointF orthVector = new PointF(-vector.Y, vector.X);
            float vectorSize = (float)Math.Sqrt(Math.Pow(orthVector.X, 2) + Math.Pow(orthVector.Y, 2));
            orthVector.X = orthVector.X * width / vectorSize;
            orthVector.Y = orthVector.Y * width / vectorSize;
            PointF center = new PointF((from.X + to.X) / 2, (from.Y + to.Y) / 2);
            
            points[1] = new PointF(center.X + orthVector.X, center.Y + orthVector.Y);
            points[6] = new PointF(center.X - orthVector.X, center.Y - orthVector.Y);
            points[2] = new PointF(center.X + orthVector.X / 2, center.Y + orthVector.Y / 2);
            points[5] = new PointF(center.X - orthVector.X / 2, center.Y - orthVector.Y / 2);
            points[3] = new PointF(from.X + orthVector.X / 2, from.Y + orthVector.Y / 2);
            points[4] = new PointF(from.X - orthVector.X / 2, from.Y - orthVector.Y / 2);

            graphics.FillPolygon(brush, points);
        }
    }
}
