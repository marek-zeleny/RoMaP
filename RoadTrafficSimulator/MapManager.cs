﻿using System;
using System.Collections.Generic;
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
        private static readonly MetersPerSecond defaultMaxSpeed = 14.MetersPerSecond();

        public static readonly Meters roadSegmentLength = 100.Meters();
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

        public static IRoadBuilder CreateRoadBuilder(Components.Map map, IMap guiMap, Coords startingCoords, bool twoWayRoad)
        {
            return RoadBuilder.CreateRoadBuilder(map, guiMap, startingCoords, twoWayRoad);
        }

        public static bool IsMapWithoutRoadsAt(IMap guiMap, Coords coords)
        {
            foreach (Coords diff in allowedDirections)
                if (guiMap.GetRoad(new Vector(coords, new Coords(coords.x + diff.x, coords.y + diff.y)), true) != null)
                    return false;
            return true;
        }

        #endregion static

        #region members

        private Components.Map map;
        private IMap guiMap = new GUI.Map();

        public MapManager(Components.Map map)
        {
            this.map = map;
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

        public IEnumerable<RoadView> GetAllRoads(Coords coords)
        {
            // May return the same road multiple times (twice) if it just passes through
            foreach (Coords diff in allowedDirections)
            {
                RoadView road = GetRoad(new Vector(coords, new Coords(coords.x + diff.x, coords.y + diff.y)));
                if (road != null)
                    yield return road;
            }
        }

        public RoadView GetOppositeRoad(RoadView roadView)
        {
            foreach (int id in roadView.GuiRoad.GetRoadIds())
                if (id != roadView.Id)
                    return new RoadView((Components.Road)map.GetEdge(id), roadView.GuiRoad);
            return null;
        }

        public IRoadBuilder GetRoadBuilder(Coords startingCoords, bool twoWayRoad = true)
        {
            return CreateRoadBuilder(map, guiMap, startingCoords, twoWayRoad);
        }

        public bool DestroyCrossroad(ICrossroad crossroad)
        {
            map.RemoveCrossroad(crossroad.CrossroadId);
            return DestroyGuiCrossroad(crossroad);
        }

        public bool DestroyRoad(IRoad road)
        {
            foreach (int id in road.GetRoadIds())
                map.RemoveRoad(id);
            return DestroyGuiRoad(road);
        }

        public void Draw(Graphics graphics, Point origin, float zoom, int width, int height)
        {
            DrawGrid(graphics, origin, zoom, width, height);
            guiMap.Draw(graphics, origin, zoom, width, height);
        }

        public void SaveMap(StreamWriter writer)
        {
            MapSaverLoader.SaveMap(writer, map, guiMap);
        }

        public bool LoadMap(StreamReader reader, out Components.Map newMap)
        {
            newMap = new Components.Map();
            IMap newGuiMap = new GUI.Map();
            bool result = MapSaverLoader.LoadMap(reader, newMap, newGuiMap);
            if (result)
            {
                map = newMap;
                guiMap = newGuiMap;
            }
            return result;
        }

        private bool DestroyGuiCrossroad(ICrossroad crossroad)
        {
            foreach (RoadView road in GetAllRoads(crossroad.CrossroadId))
                if (!DestroyGuiRoad(road.GuiRoad))
                    return false;
            // The crossroad should be destroyed in the previous cycle
            return guiMap.GetCrossroad(crossroad.CrossroadId) == null;
        }

        private bool DestroyGuiRoad(IRoad road)
        {
            var enumerator = road.GetRoute().GetEnumerator();
            enumerator.MoveNext();
            Coords last = enumerator.Current;
            while (enumerator.MoveNext())
            {
                if (!guiMap.RemoveRoad(new Vector(last, enumerator.Current)))
                    return false;
                last = enumerator.Current;
            }
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

        private class RoadBuilder : IRoadBuilder
        {
            public static IRoadBuilder CreateRoadBuilder(Components.Map map, IMap guiMap, Coords startingCoords, bool twoWayRoad)
            {
                if (map.GetNode(startingCoords) == null && !IsMapWithoutRoadsAt(guiMap, startingCoords))
                    return null;
                else
                    return new RoadBuilder(map, guiMap, startingCoords, twoWayRoad);
            }

            private Components.Map map;
            private IMap guiMap;
            private IMutableRoad road;

            private IList<Coords> Route { get => road.Route; }

            public bool CanContinue { get; private set; }

            private RoadBuilder(Components.Map map, IMap guiMap, Coords startingCoords, bool twoWayRoad)
            {
                this.map = map;
                this.guiMap = guiMap;
                if (twoWayRoad)
                    road = new TwoWayRoad();
                else
                    road = new GUI.Road();
                road.Route.Add(startingCoords);
                road.Highlight = Highlight.High;
                CanContinue = true;
                TryGetOrAddCrossroad(startingCoords).Highlight = Highlight.High;
            }

            public bool AddSegment(Coords nextCoords)
            {
                if (!CanContinue)
                    return false;
                if (!CanEnterCoords(nextCoords))
                    return false;
                Coords lastCoords = Route[Route.Count - 1];
                Vector vector = new Vector(lastCoords, nextCoords);
                var (dx, dy) = vector.Diff();
                Coords diff = new Coords(dx, dy);
                if (!Array.Exists(allowedDirections, c => c == diff))
                    return false;
                if (!guiMap.AddRoad(road, vector))
                    return false;
                Route.Add(nextCoords);
                CanContinue = map.GetNode(nextCoords) == null;
                return true;
            }

            public bool FinishRoad()
            {
                return FinishRoad(defaultMaxSpeed);
            }

            public bool FinishRoad(MetersPerSecond maxSpeed)
            {
                if (Route.Count < 2)
                    return false;
                Coords from = Route[0];
                Coords to = Route[Route.Count - 1];
                TryGetOrAddCrossroad(from).Highlight = Highlight.Normal;
                TryGetOrAddCrossroad(to).Highlight = Highlight.Normal;
                this.road.Highlight = Highlight.Normal;
                Meters roadLength = (Route.Count - 1) * roadSegmentLength;
                Components.Road road = map.AddRoad(from, to, roadLength, maxSpeed);
                this.road.SetRoadId(road.Id);
                if (this.road.IsTwoWay)
                {
                    Components.Road backRoad = map.AddRoad(to, from, roadLength, maxSpeed);
                    this.road.SetRoadId(backRoad.Id, IMutableRoad.Direction.Backward);
                    // Set TrafficLight to always allow backward direction
                    ((Components.Crossroad)road.ToNode).TrafficLight.AddDefaultDirection(road.Id, backRoad.Id);
                    ((Components.Crossroad)backRoad.ToNode).TrafficLight.AddDefaultDirection(backRoad.Id, road.Id);
                }
                Invalidate();
                return true;
            }

            public void DestroyRoad()
            {
                for (int i = 1; i < Route.Count; i++)
                    guiMap.RemoveRoad(new Vector(Route[i - 1], Route[i]));
                if (map.GetNode(Route[0]) == null)
                    guiMap.RemoveCrossroad(Route[0]);
                else
                    guiMap.GetCrossroad(Route[0]).Highlight = Highlight.Normal;
                Invalidate();
            }

            private ICrossroad TryGetOrAddCrossroad(Coords coords)
            {
                ICrossroad output = guiMap.GetCrossroad(coords);
                if (output != null)
                    return output;
                output = new GUI.Crossroad(coords);
                guiMap.AddCrossroad(output, coords);
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
                return IsMapWithoutRoadsAt(guiMap, newCoords);
            }

            private void Invalidate()
            {
                map = null;
                guiMap = null;
                road = null;
            }
        }

        #endregion nested_types
    }
}
