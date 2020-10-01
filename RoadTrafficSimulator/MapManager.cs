using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using RoadTrafficSimulator.Components;
using RoadTrafficSimulator.GUI;
using RoadTrafficSimulator.ValueTypes;

namespace RoadTrafficSimulator
{
    class MapManager
    {
        #region static

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

        public static Vector CalculateVector(Point point, Point origin, decimal zoom)
        {
            Coords from = CalculateCoords(point, origin, zoom);
            Coords to;
            Point fromPoint = CalculatePoint(from, origin, zoom);
            int dx = point.X - fromPoint.X;
            int dy = point.Y - fromPoint.Y;
            if (Math.Abs(dx) < Math.Abs(dy))
                to = new Coords(from.x, from.y + Math.Sign(dy));
            else
                to = new Coords(from.x + Math.Sign(dx), from.y);
            return new Vector(from, to);
        }

        #endregion static

        private Components.Map map;
        private IMap guiMap = new GUI.Map();
        public GuiSettings Settings { get; } = new GuiSettings();

        public MapManager(Components.Map map)
        {
            this.map = map;
            Settings.Zoom = 1m;
        }

        public CrossroadView GetCrossroad(Coords coords)
        {
            ICrossroad guiCrossroad = guiMap.GetCrossroad(coords);
            if (guiCrossroad == null)
                return null;
            Components.Crossroad crossroad = (Components.Crossroad)map.GetNode(coords);
            return new CrossroadView(crossroad, guiCrossroad);
        }

        public RoadView GetRoad(Vector vector)
        {
            IRoad guiRoad = guiMap.GetRoad(vector, true);
            if (guiRoad == null)
                return null;
            Components.Road road = (Components.Road)map.GetEdge(guiRoad.GetRoadIds().First());
            return new RoadView(road, guiRoad);
        }

        public RoadView GetOppositeRoad(RoadView roadView)
        {
            Components.Road road = null;
            if (roadView.GuiRoad is TwoWayRoad twoWayRoad)
            {
                if (twoWayRoad.RoadId != roadView.Id)
                    road = (Components.Road)map.GetEdge(twoWayRoad.RoadId);
                else
                    road = (Components.Road)map.GetEdge(twoWayRoad.BackwardRoadId);
            }
            if (road == null)
                return null;
            return new RoadView(road, roadView.GuiRoad);
        }

        public IRoadBuilder GetRoadBuilder(Coords startingCoords, bool twoWayRoad = true)
        {
            if (map.GetNode(startingCoords) == null && !IsFree(startingCoords))
                return null;
            return new RoadBuilder(this, startingCoords, twoWayRoad);
        }

        public void Draw(Graphics graphics, int width, int height)
        {
            DrawGrid(graphics, width, height);
            guiMap.Draw(graphics, Settings.Origin, Settings.Zoom, width, height);
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
                if (guiMap.GetRoad(new Vector(coords, new Coords(coords.x + diff.x, coords.y + diff.y)), true) != null)
                    return false;
            return true;
        }

        #region nested classes

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
            private IRoad road;
            private List<Coords> route;

            public bool CanContinue { get; private set; }

            public RoadBuilder(MapManager manager, Coords startingCoords, bool twoWayRoad)
            {
                this.manager = manager;
                if (twoWayRoad)
                    road = new TwoWayRoad();
                else
                    road = new GUI.Road();
                road.Highlight = Highlight.High;
                route = new List<Coords> { startingCoords };
                CanContinue = true;
                TryGetOrAddCrossroad(startingCoords).Highlight = Highlight.High;
            }

            public bool AddSegment(Coords nextCoords)
            {
                if (!CanContinue)
                    return false;
                if (!CanEnterCoords(nextCoords))
                    return false;
                Coords lastCoords = route[route.Count - 1];
                Vector vector = new Vector(lastCoords, nextCoords);
                (int dx, int dy) = vector.Diff();
                if (Math.Abs(dx) + Math.Abs(dy) != 1)
                    return false;
                if (!manager.guiMap.AddRoad(road, vector))
                    return false;
                route.Add(nextCoords);
                CanContinue = manager.map.GetNode(nextCoords) == null;
                return true;
            }

            public bool FinishRoad(MetersPerSecond maxSpeed)
            {
                if (route.Count < 2)
                    return false;
                Coords from = route[0];
                Coords to = route[route.Count - 1];
                TryGetOrAddCrossroad(from).Highlight = Highlight.Normal;
                TryGetOrAddCrossroad(to).Highlight = Highlight.Normal;
                this.road.Highlight = Highlight.Normal;
                Meters roadLength = (route.Count - 1) * roadSegmentLength;
                Components.Road road = manager.map.AddRoad(from, to, roadLength, maxSpeed);
                ((GUI.Road)this.road).RoadId = road.Id;
                if (this.road is TwoWayRoad s)
                {
                    road = manager.map.AddRoad(to, from, roadLength, maxSpeed);
                    s.BackwardRoadId = road.Id;
                }
                Invalidate();
                return true;
            }

            public void DestroyRoad()
            {
                for (int i = 1; i < route.Count; i++)
                    manager.guiMap.RemoveRoad(new Vector(route[i - 1], route[i]));
                if (manager.map.GetNode(route[0]) == null)
                    manager.guiMap.RemoveCrossroad(route[0]);
                else
                    manager.guiMap.GetCrossroad(route[0]).Highlight = Highlight.Normal;
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
                foreach (Coords c in route)
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
                road = null;
                route = null;
            }
        }

        #endregion nested classes
    }
}
