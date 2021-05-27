using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;

using RoadTrafficSimulator.Components;
using RoadTrafficSimulator.GUI;
using RoadTrafficSimulator.ValueTypes;

namespace RoadTrafficSimulator
{
    class MapManager
    {
        #region static

        private const int K = 100;
        private static readonly Speed defaultMaxSpeed = 50.KilometresPerHour();

        public static readonly Distance roadSegmentLength = 100.Metres();
        public static readonly Coords[] allowedDirections = new Coords[]
        {
                new Coords(1, 0),
                new Coords(-1, 0),
                new Coords(0, 1),
                new Coords(0, -1)
        };

        public static Point CalculatePoint(Coords coords, Point origin, float zoom)
        {
            Point output = new Point
            {
                X = origin.X + (int)(coords.x * K * zoom),
                Y = origin.Y + (int)(coords.y * K * zoom)
            };
            return output;
        }

        public static Coords CalculateCoords(Point point, Point origin, float zoom)
        {
            int x = (int)Math.Round((point.X - origin.X) / (K * zoom));
            int y = (int)Math.Round((point.Y - origin.Y) / (K * zoom));
            return new Coords(x, y);
        }

        public static Vector CalculateVector(Point point, Point origin, float zoom)
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

        public static IRoadBuilder CreateRoadBuilder(Map map, IGMap guiMap, Coords startingCoords, bool twoWayRoad)
        {
            return RoadBuilder.CreateRoadBuilder(map, guiMap, startingCoords, twoWayRoad);
        }

        public static bool IsMapWithoutRoadsAt(IGMap guiMap, Coords coords)
        {
            foreach (Coords diff in allowedDirections)
                if (guiMap.GetRoad(new Vector(coords, new Coords(coords.x + diff.x, coords.y + diff.y))) != null)
                    return false;
            return true;
        }

        #endregion static

        #region members

        private IGMap guiMap = new GMap();

        public Map Map { get; private set; } = new Map();

        public CrossroadWrapper? GetCrossroad(Coords coords)
        {
            IGCrossroad gCrossroad = guiMap.GetCrossroad(coords);
            if (gCrossroad == null)
                return null;
            Crossroad crossroad = (Crossroad)Map.GetNode(coords);
            return new CrossroadWrapper(gCrossroad, crossroad);
        }

        public IGRoad GetRoad(Vector vector)
        {
            return guiMap.GetRoad(vector);
        }

        public IEnumerable<IGRoad> GetAllRoads(Coords coords)
        {
            // May return the same road multiple times (twice) if it just passes through
            foreach (Coords diff in allowedDirections)
            {
                IGRoad road = GetRoad(new Vector(coords, new Coords(coords.x + diff.x, coords.y + diff.y)));
                if (road != null)
                    yield return road;
            }
        }

        public IRoadBuilder GetRoadBuilder(Coords startingCoords, bool twoWayRoad = true)
        {
            return CreateRoadBuilder(Map, guiMap, startingCoords, twoWayRoad);
        }

        public bool DestroyCrossroad(IGCrossroad crossroad)
        {
            Map.RemoveCrossroad(crossroad.CrossroadId);
            return DestroyGuiCrossroad(crossroad);
        }

        public bool DestroyRoad(IGRoad gRoad)
        {
            foreach (Road road in gRoad.GetRoads())
                Map.RemoveRoad(road.Id);
            return DestroyGuiRoad(gRoad);
        }

        public void Draw(Graphics graphics, Point origin, float zoom, int width, int height)
        {
            DrawGrid(graphics, origin, zoom, width, height);
            guiMap.Draw(graphics, origin, zoom, width, height);
        }

        public void SaveMap(StreamWriter writer)
        {
            MapSaverLoader.SaveMap(writer, Map, guiMap);
        }

        public bool LoadMap(StreamReader reader)
        {
            Map newMap = new();
            IGMap newGuiMap = new GMap();
            bool result = MapSaverLoader.LoadMap(reader, newMap, newGuiMap);
            if (result)
            {
                Map = newMap;
                guiMap = newGuiMap;
            }
            return result;
        }

        private bool DestroyGuiCrossroad(IGCrossroad crossroad)
        {
            foreach (RoadView road in GetAllRoads(crossroad.CrossroadId))
                if (!DestroyGuiRoad(road.GuiRoad))
                    return false;
            // The crossroad should be destroyed in the previous cycle
            return guiMap.GetCrossroad(crossroad.CrossroadId) == null;
        }

        private bool DestroyGuiRoad(IGRoad road)
        {
            void CheckMainRoad(Coords crossroadCoords, Coords direction)
            {
                IGCrossroad crossroad = guiMap.GetCrossroad(crossroadCoords);
                var mainRoads = crossroad.MainRoadDirections;
                if (mainRoads.HasValue
                    && (mainRoads.Value.Item1 == direction || mainRoads.Value.Item2 == direction))
                    crossroad.MainRoadDirections = null;
            }

            var enumerator = road.GetRoute().GetEnumerator();
            // Need pre-last coords to check for main road
            bool result = enumerator.MoveNext();
            Debug.Assert(result);
            Coords prelast = enumerator.Current;
            result = enumerator.MoveNext();
            Debug.Assert(result);
            Coords last = enumerator.Current;
            if (!guiMap.RemoveRoad(new Vector(prelast, last)))
                return false;
            CheckMainRoad(road.From, last);
            while (enumerator.MoveNext())
            {
                if (!guiMap.RemoveRoad(new Vector(last, enumerator.Current)))
                    return false;
                prelast = last;
                last = enumerator.Current;
            }
            CheckMainRoad(road.To, prelast);
            // Destroy crossroads that became stand-alone after destroying this road
            if (IsMapWithoutRoadsAt(guiMap, road.From))
                if (!guiMap.RemoveCrossroad(road.From))
                    return false;
            if (IsMapWithoutRoadsAt(guiMap, road.To))
                if (!guiMap.RemoveCrossroad(road.To))
                    return false;
            return true;
        }

        private void DrawGrid(Graphics graphics, Point origin, float zoom, int width, int height)
        {
            float step = K * zoom;
            Coords firstCoords = CalculateCoords(new Point(0, 0), origin, zoom);
            Point firstPoint = CalculatePoint(firstCoords, origin, zoom);

            Pen pen = new Pen(Color.Gray, 1)
            {
                DashStyle = System.Drawing.Drawing2D.DashStyle.Dash
            };
            Font font = new Font(SystemFonts.DefaultFont.FontFamily, 10f);
            Brush brush = Brushes.DarkOrange;

            int xCoord = firstCoords.x;
            for (float x = firstPoint.X; x < width; x += step)
            {
                graphics.DrawLine(pen, x, 0, x, height);
                graphics.DrawString(string.Format("[{0}]", xCoord++), font, brush, x + 5, 5);
            }
            int yCoord = firstCoords.y;
            for (float y = firstPoint.Y; y < height; y += step)
            {
                graphics.DrawLine(pen, 0, y, width, y);
                graphics.DrawString(string.Format("[{0}]", yCoord++), font, brush, 5, y + 5);
            }
        }

        #endregion members

        #region nested_types

        public readonly struct CrossroadWrapper
        {
            public readonly IGCrossroad gCrossroad;
            public readonly Crossroad crossroad;
            public CrossroadWrapper(IGCrossroad gCrossroad, Crossroad crossroad)
            {
                this.gCrossroad = gCrossroad;
                this.crossroad = crossroad;
            }
        }

        private class RoadBuilder : IRoadBuilder
        {
            public static IRoadBuilder CreateRoadBuilder(Map map, IGMap guiMap, Coords startingCoords, bool twoWayRoad)
            {
                if (map.GetNode(startingCoords) == null && !IsMapWithoutRoadsAt(guiMap, startingCoords))
                    return null;
                else
                    return new RoadBuilder(map, guiMap, startingCoords, twoWayRoad);
            }

            private Map map;
            private IGMap gMap;
            private IMutableGRoad gRoad;
            private bool twoWay;

            private ICollection<Coords> Route { get => gRoad.Route; }

            public bool CanContinue { get; private set; }

            private RoadBuilder(Map map, IGMap gMap, Coords startCoords, bool twoWay)
            {
                this.map = map;
                this.gMap = gMap;
                this.twoWay = twoWay;
                gRoad = new GRoad();
                gRoad.Highlight(Highlight.High);
                Route.Add(startCoords);
                CanContinue = true;
                TryGetOrAddCrossroad(startCoords).Highlight = Highlight.High;
            }

            public bool AddSegment(Coords nextCoords)
            {
                if (!CanContinue)
                    return false;
                if (!CanEnterCoords(nextCoords))
                    return false;
                Coords lastCoords = gRoad.To;
                Vector vector = new(lastCoords, nextCoords);
                var (dx, dy) = vector.Diff();
                Coords diff = new Coords(dx, dy);
                if (!Array.Exists(allowedDirections, c => c == diff))
                    return false;
                if (!gMap.AddRoad(gRoad, vector))
                    return false;
                Route.Add(nextCoords);
                CanContinue = map.GetNode(nextCoords) == null;
                return true;
            }

            public bool FinishRoad()
            {
                return FinishRoad(defaultMaxSpeed);
            }

            public bool FinishRoad(Speed maxSpeed)
            {
                if (Route.Count < 2)
                    return false;
                TryGetOrAddCrossroad(gRoad.From).Highlight = Highlight.Normal;
                TryGetOrAddCrossroad(gRoad.To).Highlight = Highlight.Normal;
                gRoad.Highlight(Highlight.Normal);
                Distance roadLength = (Route.Count - 1) * roadSegmentLength;
                Road road = map.AddRoad(gRoad.From, gRoad.To, roadLength, maxSpeed);
                gRoad.SetRoad(road, IGRoad.Direction.Forward);
                if (twoWay)
                {
                    Road backRoad = map.AddRoad(gRoad.To, gRoad.From, roadLength, maxSpeed);
                    gRoad.SetRoad(backRoad, IGRoad.Direction.Backward);
                    // Set TrafficLight to always allow backward direction
                    ((Crossroad)road.ToNode).TrafficLight.AddDefaultDirection(road.Id, backRoad.Id);
                    ((Crossroad)backRoad.ToNode).TrafficLight.AddDefaultDirection(backRoad.Id, road.Id);
                }
                Invalidate();
                return true;
            }

            public void DestroyRoad()
            {
                Coords last = Route.First();
                if (map.GetNode(last) == null)
                    gMap.RemoveCrossroad(last);
                else
                    gMap.GetCrossroad(last).Highlight = Highlight.Normal;
                foreach (Coords curr in Route.Skip(1))
                {
                    gMap.RemoveRoad(new Vector(last, curr));
                    last = curr;
                }
                Invalidate();
            }

            private IGCrossroad TryGetOrAddCrossroad(Coords coords)
            {
                IGCrossroad output = gMap.GetCrossroad(coords);
                if (output != null)
                    return output;
                output = new GCrossroad(coords);
                gMap.AddCrossroad(output, coords);
                return output;
            }

            private bool CanEnterCoords(Coords newCoords)
            {
                foreach (Coords c in Route)
                    if (c.Equals(newCoords))
                        return false;
                // Always allow if there is a (different from the starting one) crossroad at newCoords
                if (map.GetNode(newCoords) != null)
                    return true;
                return IsMapWithoutRoadsAt(gMap, newCoords);
            }

            private void Invalidate()
            {
                map = null;
                gMap = null;
                gRoad = null;
            }
        }

        #endregion nested_types
    }
}
