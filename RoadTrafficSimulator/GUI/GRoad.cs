using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

using RoadTrafficSimulator.ValueTypes;
using RoadTrafficSimulator.Components;
using DataStructures.Miscellaneous;

namespace RoadTrafficSimulator.GUI
{
    class GRoad : IMutableGRoad
    {
        private static readonly Color defaultColor = Color.Blue;
        private static readonly Color arrowColor = Color.Yellow;

        private Road fRoad;
        private Road bRoad;
        private Highlight fHighlight;
        private Highlight bHighlight;
        private readonly LinkedList<Coords> route = new();

        public Coords From { get => route.First.Value; }
        public Coords To { get => route.Last.Value; }
        public bool IsTwoWay { get => fRoad != null && bRoad != null; }
        public ICollection<Coords> Route { get => route; }

        public Road GetRoad(IGRoad.Direction direction)
        {
            return direction switch
            {
                IGRoad.Direction.Forward => fRoad,
                IGRoad.Direction.Backward => bRoad,
                _ => throw new NotImplementedException(),
            };
        }

        public IEnumerable<Road> GetRoads()
        {
            if (fRoad != null)
                yield return fRoad;
            if (bRoad != null)
                yield return bRoad;
        }

        public void SetRoad(Road road, IGRoad.Direction direction)
        {
            switch (direction)
            {
                case IGRoad.Direction.Forward:
                    fRoad = road;
                    break;
                case IGRoad.Direction.Backward:
                    bRoad = road;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public IEnumerable<Coords> GetRoute(IGRoad.Direction direction)
        {
            // Both directions return even if only one road exists, because the reverse might still be practical
            return direction switch
            {
                IGRoad.Direction.Forward => route,
                IGRoad.Direction.Backward => route?.Reverse(),
                _ => throw new NotImplementedException(),
            };
        }

        public IGRoad GetReversedGRoad()
        {
            return new ReversedGRoad(this);
        }

        public void ResetHighlight(Highlight highlight)
        {
            fHighlight = highlight;
            bHighlight = highlight;
        }

        public void ResetHighlight(Highlight highlight, IGRoad.Direction direction)
        {
            switch (direction)
            {
                case IGRoad.Direction.Forward:
                    Debug.Assert(fRoad != null);
                    fHighlight = highlight;
                    break;
                case IGRoad.Direction.Backward:
                    Debug.Assert(bRoad != null);
                    bHighlight = highlight;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public void SetHighlight(Highlight highlight, IGRoad.Direction direction)
        {
            switch (direction)
            {
                case IGRoad.Direction.Forward:
                    Debug.Assert(fRoad != null);
                    fHighlight |= highlight;
                    break;
                case IGRoad.Direction.Backward:
                    Debug.Assert(bRoad != null);
                    bHighlight |= highlight;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public void UnsetHighlight(Highlight highlight, IGRoad.Direction direction)
        {
            switch (direction)
            {
                case IGRoad.Direction.Forward:
                    Debug.Assert(fRoad != null);
                    fHighlight &= ~highlight;
                    break;
                case IGRoad.Direction.Backward:
                    Debug.Assert(bRoad != null);
                    bHighlight &= ~highlight;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        #region graphics

        public void Draw(Graphics graphics, Point from, Point to, int width, bool simulationMode)
        {
            Color GetRoadColor(Road road)
            {
                if (!simulationMode || !road.IsConnected)
                    return defaultColor;
                float speedRatio = (float)(road as Road).AverageSpeed / road.MaxSpeed;
                int red = (int)(255 * (1 - speedRatio));
                int green = (int)(255 * speedRatio);
                return Color.FromArgb(red, green, 0);
            }

            // one-way
            if (bRoad == null)
            {
                // Might also be during the building phase - both roads are null
                DrawLane(graphics, from, to, width, fHighlight, GetRoadColor(fRoad));
                return;
            }
            else if (fRoad == null)
            {
                DrawLane(graphics, to, from, width, bHighlight, GetRoadColor(bRoad));
                return;
            }
            // two-way
            Point from1 = from;
            Point to1 = to;
            Point from2 = to;
            Point to2 = from;
            int distance = width * 3 / 4;
            if (fHighlight.HasFlag(Highlight.Large) || bHighlight.HasFlag(Highlight.Large))
                distance = IncreaseWidth(width) * 3 / 4;
            int diffX = to.X - from.X;
            int diffY = to.Y - from.Y;
            if (diffX == 0)
            {
                if (diffY == 0)
                    throw new ArgumentException($"The given points cannot be identical.");
                else if ((diffY > 0) == (MapManager.roadSide == MapManager.RoadSide.Right))
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
                else if ((diffX < 0) == (MapManager.roadSide == MapManager.RoadSide.Right))
                    distance = -distance;
                from1.Offset(0, distance);
                to1.Offset(0, distance);
                from2.Offset(0, -distance);
                to2.Offset(0, -distance);
            }
            DrawLane(graphics, from1, to1, width, fHighlight, GetRoadColor(fRoad));
            DrawLane(graphics, from2, to2, width, bHighlight, GetRoadColor(bRoad));
        }

        private static void DrawLane(Graphics graphics, Point from, Point to, int width,
            Highlight highlight, Color color)
        {
            if (highlight.HasFlag(Highlight.Transparent))
                color = Color.FromArgb(150, color);
            if (highlight.HasFlag(Highlight.Large))
                width = IncreaseWidth(width);
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

        #endregion graphics

        private class ReversedGRoad : IGRoad
        {
            private static IGRoad.Direction Reverse(IGRoad.Direction direction)
            {
                return direction switch
                {
                    IGRoad.Direction.Forward => IGRoad.Direction.Backward,
                    IGRoad.Direction.Backward => IGRoad.Direction.Forward,
                    _ => throw new NotImplementedException(),
                };
            }

            private readonly IGRoad gRoad;

            public Coords From => gRoad.To;
            public Coords To => gRoad.From;
            public bool IsTwoWay => gRoad.IsTwoWay;

            public ReversedGRoad(IGRoad gRoad)
            {
                this.gRoad = gRoad;
            }

            public Road GetRoad(IGRoad.Direction direction) => gRoad.GetRoad(Reverse(direction));
            public IEnumerable<Road> GetRoads() => gRoad.GetRoads();
            public IEnumerable<Coords> GetRoute(IGRoad.Direction direction) => gRoad.GetRoute(Reverse(direction));
            public IGRoad GetReversedGRoad() => gRoad;
            public void ResetHighlight(Highlight highlight) => gRoad.ResetHighlight(highlight);
            public void ResetHighlight(Highlight highlight, IGRoad.Direction direction) =>
                gRoad.ResetHighlight(highlight, Reverse(direction));
            public void SetHighlight(Highlight highlight, IGRoad.Direction direction) =>
                gRoad.SetHighlight(highlight, Reverse(direction));
            public void UnsetHighlight(Highlight highlight, IGRoad.Direction direction) =>
                gRoad.UnsetHighlight(highlight, Reverse(direction));
            public void Draw(Graphics graphics, Point from, Point to, int width, bool simulationMode) =>
                gRoad.Draw(graphics, from, to, width, simulationMode);
        }
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
