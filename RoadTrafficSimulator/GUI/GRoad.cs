﻿using System;
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
        private const int laneWidth = 7;
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

        public void Draw(Graphics graphics, Point origin, float zoom, bool simulationMode, Func<Point, bool> isVisible)
        {
            Debug.Assert(Route.Count >= 2);

            int width = (int)(laneWidth * zoom);
            int fWidth = fHighlight.HasFlag(Highlight.Large) ? GetIncreasedWidth(width) : width;
            int bWidth = bHighlight.HasFlag(Highlight.Large) ? GetIncreasedWidth(width) : width;
            Color fColor = GetRoadColor(fRoad, fHighlight, simulationMode);
            Color bColor = GetRoadColor(bRoad, bHighlight, simulationMode);
            IEnumerator<Coords> routeEnum = Route.GetEnumerator();
            routeEnum.MoveNext();
            Point from;
            Point to = CoordsConvertor.CalculatePoint(routeEnum.Current, origin, zoom);

            while (routeEnum.MoveNext())
            {
                from = to;
                to = CoordsConvertor.CalculatePoint(routeEnum.Current, origin, zoom);
                if (isVisible(from) || isVisible(to))
                {
                    DrawSegment(graphics, from, to, width, fWidth, bWidth, fColor, bColor);
                }
            }
        }

        private void DrawSegment(Graphics graphics, Point from, Point to, int width, int fWidth, int bWidth,
            Color fColor, Color bColor)
        {
            if (bRoad == null)
            {
                if (fRoad == null)
                    // Building phase - both roads are null
                    DrawLane(graphics, from, to, width, defaultColor);
                else
                    // One-way forward road
                    DrawOneWayRoad(graphics, from, to, fWidth, fColor, fRoad.LaneCount);
            }
            else if (fRoad == null)
            {
                // One-way backward road
                DrawOneWayRoad(graphics, to, from, bWidth, bColor, bRoad.LaneCount);
            }
            else
            {
                // Two-way
                DrawTwoWayRoad(graphics, from, to, fWidth, bWidth, fColor, bColor);
            }
        }

        private void DrawOneWayRoad(Graphics graphics, Point from, Point to, int width, Color color, int lanes)
        {
            Point from2 = from;
            Point to2 = to;
            var offsetDirs = GetLaneOffsetDirections(from, to);
            var (dx, dy) = ScaleLaneOffsets(offsetDirs, GetLaneOffset(width));
            if (lanes % 2 == 0)
            {
                // Even number of lanes
                from.Offset(dx / 2, dy / 2);
                to.Offset(dx / 2, dy / 2);
                from2.Offset(-dx / 2, -dy / 2);
                to2.Offset(-dx / 2, -dy / 2);
                for (int i = 0; i < lanes; i += 2)
                {
                    DrawLane(graphics, from, to, width, color);
                    DrawLane(graphics, from2, to2, width, color);
                    from.Offset(dx, dy);
                    to.Offset(dy, dy);
                    from2.Offset(-dx, -dy);
                    to2.Offset(-dx, -dy);
                }
            }
            else
            {
                // Odd number of lanes
                DrawLane(graphics, from, to, width, color);
                for (int i = 1; i < lanes; i += 2)
                {
                    from.Offset(dx, dy);
                    to.Offset(dx, dy);
                    from2.Offset(-dx, -dy);
                    to2.Offset(-dx, -dy);
                    DrawLane(graphics, from, to, width, color);
                    DrawLane(graphics, from2, to2, width, color);
                }
            }
        }

        private void DrawTwoWayRoad(Graphics graphics, Point from, Point to, int fWidth, int bWidth,
            Color fColor, Color bColor)
        {
            void DrawOneWay(Point from, Point to, int dx, int dy, int width, int lanes, Color color)
            {
                from.Offset(dx / 2, dy / 2);
                to.Offset(dx / 2, dy / 2);
                for (int i = 0; i < lanes; i++)
                {
                    DrawLane(graphics, from, to, width, color);
                    from.Offset(dx, dy);
                    to.Offset(dx, dy);
                }
            }

            var offsetDirs = GetLaneOffsetDirections(from, to);
            var o1 = ScaleLaneOffsets(offsetDirs, GetLaneOffset(fWidth));
            var o2 = ScaleLaneOffsets(offsetDirs, GetLaneOffset(bWidth));
            o2 = (-o2.dx, -o2.dy);

            DrawOneWay(from, to, o1.dx, o1.dy, fWidth, fRoad.LaneCount, fColor);
            DrawOneWay(to, from, o2.dx, o2.dy, bWidth, bRoad.LaneCount, bColor);
        }

        private static void DrawLane(Graphics graphics, Point from, Point to, int width, Color color)
        {
            Pen pen = new(color, width);
            graphics.DrawLine(pen, from, to);

            const float ratio = 0.6f;
            const float invRatio = 1 - ratio;
            PointF arrowFrom = new(from.X * ratio + to.X * invRatio, from.Y * ratio + to.Y * invRatio);
            PointF arrowTo = new(to.X * ratio + from.X * invRatio, to.Y * ratio + from.Y * invRatio);
            Brush brush = new SolidBrush(arrowColor);
            graphics.FillArrow(brush, arrowFrom, arrowTo, width / 2);
        }

        private static Color GetRoadColor(Road road, Highlight highlight, bool simulationMode)
        {
            Color ApplyHighlight(Color color)
            {
                if (highlight.HasFlag(Highlight.Transparent))
                    return Color.FromArgb(150, color);
                else
                    return color;
            }

            if (!simulationMode || road == null || !road.IsConnected)
                return ApplyHighlight(defaultColor);

            float speedRatio = (float)road.AverageSpeed / road.MaxSpeed;
            Debug.Assert(speedRatio >= 0);
            // Average speed can be higher than max speed if a car just arrived from a 'faster' road
            if (speedRatio > 1)
                speedRatio = 1;
            int red = (int)(255 * (1 - speedRatio));
            int green = (int)(255 * speedRatio);
            Color color = Color.FromArgb(red, green, 0);
            return ApplyHighlight(color);
        }

        private static (int dx, int dy) GetLaneOffsetDirections(Point from, Point to)
        {
            int diffX = to.X - from.X;
            int diffY = to.Y - from.Y;
            int dx = 0;
            int dy = 0;
            if (diffX == 0)
            {
                // vertical road
                Debug.Assert(diffY != 0);
                if ((diffY < 0) == (MapManager.roadSide == MapManager.RoadSide.Right))
                    // upwards road
                    dx = 1;
                else
                    // downwards road
                    dx = -1;
            }
            else
            {
                // horizontal road
                Debug.Assert(diffY == 0);
                if ((diffX > 0) == (MapManager.roadSide == MapManager.RoadSide.Right))
                    // rightwards road
                    dy = 1;
                else
                    // leftwards road
                    dy = -1;
            }
            return (dx, dy);
        }

        private static (int dx, int dy) ScaleLaneOffsets((int dx, int dy) dirs, int scale)
        {
            return (dirs.dx * scale, dirs.dy * scale);
        }

        private static int GetLaneOffset(int laneWidth) => laneWidth * 3 / 2;

        private static int GetIncreasedWidth(int width) => width * 3 / 2;

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
            public void Draw(Graphics graphics, Point origin, float zoom, bool simulationMode,
                Func<Point, bool> isVisible) =>
                gRoad.Draw(graphics, origin, zoom, simulationMode, isVisible);
        }
    }

    static class GraphicsExtensions
    {
        public static void FillArrow(this Graphics graphics, Brush brush, PointF from, PointF to, int width)
        {
            PointF[] points = new PointF[8];
            points[0] = to;
            points[7] = to;

            PointF vector = new(to.X - from.X, to.Y - from.Y);
            PointF orthVector = new(-vector.Y, vector.X);
            float vectorSize = (float)Math.Sqrt(Math.Pow(orthVector.X, 2) + Math.Pow(orthVector.Y, 2));
            orthVector.X = orthVector.X * width / vectorSize;
            orthVector.Y = orthVector.Y * width / vectorSize;
            PointF center = new((from.X + to.X) / 2, (from.Y + to.Y) / 2);
            
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
