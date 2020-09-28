using System;
using System.Collections.Generic;
using System.Drawing;

using RoadTrafficSimulator.Components;
using RoadTrafficSimulator.GUI;
using RoadTrafficSimulator.ValueTypes;

namespace RoadTrafficSimulator
{
    class MapManager
    {
        private static readonly Meters roadSegmentLength = 100.Meters();
        private const int K = 100;
        private const decimal minZoom = 0.2m;
        private const decimal maxZoom = 5;

        private static readonly Coords[] directions = new Coords[]
        {
                new Coords(1, 0),
                new Coords(-1, 0),
                new Coords(0, 1),
                new Coords(0, -1)
        };

        public static Point CalculatePoint(Coords coords, Point origin, decimal zoom)
        {
            Point output = new Point
            {
                X = origin.X + (int)(coords.x * K * zoom),
                Y = origin.Y + (int)(coords.y * K * zoom)
            };
            return output;
        }

        public static Coords CalculateCoords(Point point, Point origin, decimal zoom)
        {
            int x = (int)Math.Round((point.X - origin.X) / (K * zoom));
            int y = (int)Math.Round((point.Y - origin.Y) / (K * zoom));
            return new Coords(x, y);
        }

        private Components.Map map;
        private IMap guiMap = new GUI.Map();
        public GuiSettings Settings { get; } = new GuiSettings();

        public MapManager(Components.Map map)
        {
            this.map = map;
            Settings.Zoom = 1m;
        }

        public void Draw(Graphics graphics, int width, int height)
        {
            DrawGrid(graphics, width, height);
            guiMap.Draw(graphics, Settings.Origin, Settings.Zoom, width, height);
        }

        public IRoadBuilder GetRoadBuilder(Coords startingCoords, bool twoWayRoad = true)
        {
            if (map.GetNode(startingCoords) == null && !IsFree(startingCoords))
                return null;
            return new RoadBuilder(this, startingCoords, twoWayRoad);
        }

        private void DrawGrid(Graphics graphics, int width, int height)
        {
            int step = (int)(K * Settings.Zoom);
            int firstX = Settings.Origin.X % step;
            int firstY = Settings.Origin.Y % step;
            Pen pen = new Pen(Color.Gray, 1)
            {
                DashStyle = System.Drawing.Drawing2D.DashStyle.Dash
            };
            for (int x = firstX; x < width; x += step)
                graphics.DrawLine(pen, x, 0, x, height);
            for (int y = firstY; y < height; y += step)
                graphics.DrawLine(pen, 0, y, width, y);
        }

        private bool IsFree(Coords coords)
        {
            foreach (Coords diff in directions)
                if (guiMap.GetRoadSegment(new Vector(coords, new Coords(coords.x + diff.x, coords.y + diff.y)), true) != null)
                    return false;
            return true;
        }

        public class GuiSettings
        {
            private Point origin;
            private decimal zoom;

            public Point Origin { get => origin; set => origin = value; }
            public decimal Zoom
            {
                get => zoom;
                set
                {
                    if (value < minZoom)
                        zoom = minZoom;
                    else if (value > maxZoom)
                        zoom = maxZoom;
                    else
                        zoom = value;
                }
            }

            public void MoveOrigin(Point offset)
            {
                origin.Offset(offset);
            }
        }

        private class RoadBuilder : IRoadBuilder
        {
            private MapManager manager;
            private IRoadSegment roadSegment;
            private List<Coords> coords;

            public bool CanContinue { get; private set; }

            public RoadBuilder(MapManager manager, Coords startingCoords, bool twoWayRoad)
            {
                this.manager = manager;
                if (twoWayRoad)
                    roadSegment = new TwoWayRoadSegment();
                else
                    roadSegment = new RoadSegment();
                roadSegment.Highlight = Highlight.High;
                coords = new List<Coords> { startingCoords };
                CanContinue = true;
                TryGetOrAddCrossroad(startingCoords).Highlight = Highlight.High;
            }

            public bool AddSegment(Coords nextCoords)
            {
                if (!CanContinue)
                    return false;
                if (!CanEnterCoords(nextCoords))
                    return false;
                Coords lastCoords = coords[coords.Count - 1];
                Vector vector = new Vector(lastCoords, nextCoords);
                (int dx, int dy) = vector.Diff();
                if (Math.Abs(dx) + Math.Abs(dy) != 1)
                    return false;
                if (!manager.guiMap.AddRoadSegment(roadSegment, vector))
                    return false;
                coords.Add(nextCoords);
                CanContinue = manager.map.GetNode(nextCoords) == null;
                return true;
            }

            public bool FinishRoad(MetersPerSecond maxSpeed)
            {
                if (coords.Count < 2)
                    return false;
                Coords from = coords[0];
                Coords to = coords[coords.Count - 1];
                TryGetOrAddCrossroad(from).Highlight = Highlight.Normal;
                TryGetOrAddCrossroad(to).Highlight = Highlight.Normal;
                roadSegment.Highlight = Highlight.Normal;
                Meters roadLength = (coords.Count - 1) * roadSegmentLength;
                Road road = manager.map.AddRoad(from, to, roadLength, maxSpeed);
                ((RoadSegment)roadSegment).RoadId = road.Id;
                if (roadSegment is TwoWayRoadSegment s)
                {
                    road = manager.map.AddRoad(to, from, roadLength, maxSpeed);
                    s.BackwardRoadId = road.Id;
                }
                Invalidate();
                return true;
            }

            public void DestroyRoad()
            {
                for (int i = 1; i < coords.Count; i++)
                    manager.guiMap.RemoveRoadSegment(new Vector(coords[i - 1], coords[i]));
                if (manager.map.GetNode(coords[0]) == null)
                    manager.guiMap.RemoveCrossroad(coords[0]);
                else
                    manager.guiMap.GetCrossroad(coords[0]).Highlight = Highlight.Normal;
                Invalidate();
            }

            private ICrossroad TryGetOrAddCrossroad(Coords coords)
            {
                ICrossroad output = manager.guiMap.GetCrossroad(coords);
                if (output != null)
                    return output;
                output = new GUI.Crossroad(coords);
                manager.guiMap.AddCrossroad(output, coords);
                return output;
            }

            private bool CanEnterCoords(Coords newCoords)
            {
                foreach (Coords c in coords)
                    if (c.Equals(newCoords))
                        return false;
                // Always allow if there is a (different from the starting one) crossroad at newCoords
                if (manager.map.GetNode(newCoords) != null)
                    return true;
                return manager.IsFree(newCoords);
            }

            private void Invalidate()
            {
                manager = null;
                roadSegment = null;
                coords = null;
            }
        }
    }
}
