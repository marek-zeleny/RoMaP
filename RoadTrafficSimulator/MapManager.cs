﻿using System;
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

        public enum RoadSide { Right, Left };

        public const int gridSize = 100;

        private static readonly Speed defaultMaxSpeed = 50.KilometresPerHour();

        public static readonly Distance roadSegmentDefaultLength = 100.Metres();
        // Must be ordered in the counter-clockwise direction
        public static readonly Coords[] allowedDirections = new Coords[]
        {
                new Coords(1, 0),
                new Coords(0, -1),
                new Coords(-1, 0),
                new Coords(0, 1),
        };

        public static RoadSide roadSide = RoadSide.Right;

        /// <summary>
        /// Gets allowed directions in the correct order according to <see cref="roadSide"/> starting from the
        /// <paramref name="i"/>th index.
        /// </summary>
        public static IEnumerable<Coords> GetAllowedDirections(int i = 0)
        {
            yield return allowedDirections[i];
            int m = allowedDirections.Length;
            switch (roadSide)
            {
                case RoadSide.Right:
                    for (int j = (i + 1) % m; j != i; j = (j + 1) % m)
                        yield return allowedDirections[j];
                    break;
                case RoadSide.Left:
                    for (int j = (i - 1 + m) % m; j != i; j = (j - 1 + m) % m)
                        yield return allowedDirections[j];
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets allowed directions in the correct order according to <see cref="roadSide"/> starting from
        /// <paramref name="startDirection"/>.
        /// </summary>
        public static IEnumerable<Coords> GetAllowedDirections(Coords startDirection)
        {
            int i = 0;
            for (; i < allowedDirections.Length; i++)
                if (allowedDirections[i] == startDirection)
                    break;
            if (i == allowedDirections.Length)
                throw new ArgumentException("The given start direction isn't allowed.", nameof(startDirection));
            return GetAllowedDirections(i);
        }

        public static int DotProduct(Point p1, Point p2)
        {
            return p1.X * p2.X + p1.Y * p2.Y;
        }

        public static Point Normal(Point point)
        {
            return roadSide switch
            {
                RoadSide.Right => new Point(-point.Y, point.X),
                RoadSide.Left => new Point(point.Y, -point.X),
                _ => throw new NotImplementedException(),
            };
        }

        public static Point CalculatePoint(Coords coords, Point origin, float zoom)
        {
            Point output = new()
            {
                X = origin.X + (int)(coords.x * gridSize * zoom),
                Y = origin.Y + (int)(coords.y * gridSize * zoom)
            };
            return output;
        }

        public static Coords CalculateCoords(Point point, Point origin, float zoom)
        {
            int x = (int)Math.Round((point.X - origin.X) / (gridSize * zoom));
            int y = (int)Math.Round((point.Y - origin.Y) / (gridSize * zoom));
            return new Coords(x, y);
        }

        public static Vector CalculateVector(Point point, Point origin, float zoom)
        {
            Coords from = CalculateCoords(point, origin, zoom);
            Coords to;
            Point fromPoint = CalculatePoint(from, origin, zoom);
            int dx = point.X - fromPoint.X;
            int dy = point.Y - fromPoint.Y;
            int sx = Math.Sign(dx);
            int sy = Math.Sign(dy);
            bool horizontal = Math.Abs(dx) > Math.Abs(dy);
            if (horizontal)
                to = new Coords(from.x + sx, from.y);
            else
                to = new Coords(from.x, from.y + sy);
            return new Vector(from, to);
        }

        public static bool IsCorrectDirection(Vector vector, Point point, Point origin, float zoom)
        {
            // Returns true if the vector is correctly directed
            Point centre = CalculatePoint(vector.from, origin, zoom);
            Point to = CalculatePoint(vector.to, origin, zoom);
            to.Offset(-centre.X, -centre.Y);
            point.Offset(-centre.X, -centre.Y);
            return DotProduct(Normal(to), point) > 0;
        }

        public static bool IsCorrectDirection(Vector vector1, Vector vector2, Point point, Point origin, float zoom)
        {
            // Returns true if the correct direction is vector1 and false if it's vector2
            Debug.Assert(vector1.from == vector2.from);
            Point centre = CalculatePoint(vector1.from, origin, zoom);
            Point p1 = CalculatePoint(vector1.to, origin, zoom);
            Point p2 = CalculatePoint(vector2.to, origin, zoom);
            p1.Offset(-centre.X, -centre.Y);
            p2.Offset(-centre.X, -centre.Y);
            point.Offset(-centre.X, -centre.Y);
            Point n1 = Normal(p1);
            Point n2 = Normal(p2);
            bool switched = DotProduct(p1, n2) < 0;
            if (switched)
                (n1, n2) = (n2, n1);
            bool before1 = DotProduct(point, n1) > 0;
            bool after2 = DotProduct(point, n2) < 0;
            return (before1 || after2) ^ switched;
        }

        public static IRoadBuilder CreateRoadBuilder(Map map, IGMap guiMap, Coords startingCoords,
            bool twoWayRoad)
        {
            return RoadBuilder.CreateRoadBuilder(map, guiMap, startingCoords, twoWayRoad);
        }

        public static bool NoGuiRoadsAt(IGMap guiMap, Coords coords)
        {
            foreach (Coords diff in allowedDirections)
                if (guiMap.GetRoad(new Vector(coords, coords + diff)) != null)
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
            Crossroad crossroad = (Crossroad)Map.GetNode(coords); // may be null if only a GUI crossroad exists
            return new CrossroadWrapper(gCrossroad, crossroad);
        }

        public CrossroadWrapper? GetNearestCrossroad(Point point, Point origin, float zoom)
        {
            Coords coords = CalculateCoords(point, origin, zoom);
            return GetCrossroad(coords);
        }

        public CrossroadWrapper? GetNearestCrossroad(Point point, Point origin, float zoom, out double proximity)
        {
            static double CalculateDistance(Point p1, Point p2)
            {
                int dx = p1.X - p2.X;
                int dy = p1.Y - p2.Y;
                return Math.Sqrt(dx * dx + dy * dy);
            }

            Vector vector = CalculateVector(point, origin, zoom);
            Point from = CalculatePoint(vector.from, origin, zoom);
            Point to = CalculatePoint(vector.to, origin, zoom);
            double distance = CalculateDistance(from, point);
            proximity = distance / (gridSize * zoom);
            return GetCrossroad(vector.from);
        }

        public IGRoad GetRoad(Vector vector)
        {
            return guiMap.GetRoad(vector);
        }

        public IGRoad GetNearestRoad(Point point, Point origin, float zoom)
        {
            Coords coords = CalculateCoords(point, origin, zoom);

            int FindNextRoad(int i, out Vector vector, out IGRoad road)
            {
                for (; i < allowedDirections.Length; i++)
                {
                    vector = new Vector(coords, coords + allowedDirections[i]);
                    road = GetRoad(vector);
                    if (road != null)
                        return i;
                }
                vector = new();
                road = null;
                return i;
            }

            var nearestCrossroad = GetCrossroad(coords);
            // No crossroad in the area
            if (nearestCrossroad == null)
            {
                int i = FindNextRoad(0, out Vector vector1, out IGRoad road1);
                FindNextRoad(++i, out Vector vector2, out IGRoad road2);
                if (road1 == null)
                    return null;
                else
                {
                    Debug.Assert(road2 != null);
                    Debug.Assert(vector1.from == coords);
                    Debug.Assert(vector2.from == coords);
                    if (IsCorrectDirection(vector1, vector2, point, origin, zoom))
                        return road1;
                    else
                        return road2;
                }
            }
            // Crossroad nearby
            else
            {
                Vector vector = CalculateVector(point, origin, zoom);
                if (!IsCorrectDirection(vector, point, origin, zoom))
                    vector = vector.Reverse();
                return GetRoad(vector);
            }
        }

        /// <summary>
        /// Gets all GUI roads going from <paramref name="coords"/> sorted according to <see cref="roadSide"/>.
        /// May return the same road twice (in both directions) if it just passes through.
        /// </summary>
        public IEnumerable<IGRoad> GetAllGuiRoads(Coords coords)
        {
            IGRoad Selector(Coords diff) => GetRoad(new Vector(coords, coords + diff));

            return GetAllowedDirections().Select(Selector).Where(gRoad => gRoad != null);
        }

        /// <summary>
        /// Gets all GUI roads going from the starting point of <paramref name="vector"/> sorted according to
        /// <see cref="roadSide"/>, starting from the direction of <paramref name="vector"/>.
        /// May return the same road twice (in both directions) if it just passes through.
        /// </summary>
        public IEnumerable<IGRoad> GetAllGuiRoads(Vector vector)
        {
            IGRoad Selector(Coords diff) => GetRoad(new Vector(vector.from, vector.from + diff));

            return GetAllowedDirections(vector.Diff()).Select(Selector).Where(gRoad => gRoad != null);
        }

        public IRoadBuilder GetRoadBuilder(Coords startingCoords, bool twoWayRoad = true)
        {
            return CreateRoadBuilder(Map, guiMap, startingCoords, twoWayRoad);
        }

        public void CloseRoad(IGRoad gRoad)
        {
            Debug.Assert(gRoad.GetRoad().IsConnected);
            Map.RemoveRoad(gRoad.GetRoad().Id);
            gRoad.SetHighlight(Highlight.Transparent, IGRoad.Direction.Forward);
            UpdateGuiCrossroad(guiMap.GetCrossroad(gRoad.From));
            UpdateGuiCrossroad(guiMap.GetCrossroad(gRoad.To));
        }

        public void OpenRoad(IGRoad gRoad)
        {
            Debug.Assert(!gRoad.GetRoad().IsConnected);
            Road newRoad = Map.AddRoad(gRoad.GetRoad());
            Debug.Assert(newRoad != null);

            if (gRoad is IMutableGRoad mut)
                mut.SetRoad(newRoad, IGRoad.Direction.Forward);
            else
                (gRoad.GetReversedGRoad() as IMutableGRoad).SetRoad(newRoad, IGRoad.Direction.Backward);

            IGCrossroad fromCrossroad = guiMap.GetCrossroad(gRoad.From);
            IGCrossroad toCrossroad = guiMap.GetCrossroad(gRoad.To);

            gRoad.UnsetHighlight(Highlight.Transparent, IGRoad.Direction.Forward);
            fromCrossroad.UnsetHighlight(Highlight.Transparent);
            toCrossroad.UnsetHighlight(Highlight.Transparent);

            FillPriorityCrossing(guiMap, fromCrossroad, Map.GetNode(fromCrossroad.CrossroadId) as Crossroad);
            FillPriorityCrossing(guiMap, toCrossroad, Map.GetNode(toCrossroad.CrossroadId) as Crossroad);
        }

        public bool DestroyCrossroad(IGCrossroad gCrossroad)
        {
            Map.RemoveCrossroad(gCrossroad.CrossroadId);
            return DestroyGuiCrossroad(gCrossroad);
        }

        public bool DestroyRoad(IGRoad gRoad)
        {
            bool success = true;

            if (gRoad.GetRoad().IsConnected)
                Map.RemoveRoad(gRoad.GetRoad().Id);

            if (gRoad is IMutableGRoad mut)
                mut.SetRoad(null, IGRoad.Direction.Forward);
            else
                (gRoad.GetReversedGRoad() as IMutableGRoad).SetRoad(null, IGRoad.Direction.Backward);

            if (gRoad.GetRoad(IGRoad.Direction.Backward) == null)
                success = DestroyGuiRoad(gRoad);

            UpdateGuiCrossroad(guiMap.GetCrossroad(gRoad.From));
            UpdateGuiCrossroad(guiMap.GetCrossroad(gRoad.To));

            return success;
        }

        public void Draw(Graphics graphics, Point origin, float zoom, int width, int height, bool simulationMode)
        {
            DrawGrid(graphics, origin, zoom, width, height);
            guiMap.Draw(graphics, origin, zoom, width, height, simulationMode);
        }

        public void SaveMap(Stream stream)
        {
            MapSaverLoader.SaveMap(stream, Map, guiMap);
        }

        public bool LoadMap(Stream stream)
        {
            Map newMap = new();
            IGMap newGuiMap = new GMap();
            bool result = MapSaverLoader.LoadMap(stream, newMap, newGuiMap);
            if (result)
            {
                Map = newMap;
                guiMap = newGuiMap;
            }
            return result;
        }

        /// <summary>
        /// Updates highlighting and main roads of a GUI crossroad based on adjacent roads.
        /// If there are no roads left, removes the crossroad.
        /// </summary>
        private void UpdateGuiCrossroad(IGCrossroad gCrossroad)
        {
            if (NoGuiRoadsAt(guiMap, gCrossroad.CrossroadId))
            {
                // no adjacent roads exist, remove the crossroad
                guiMap.RemoveCrossroad(gCrossroad.CrossroadId);
                return;
            }
            else if (Map.GetNode(gCrossroad.CrossroadId) == null)
            {
                // only closed roads exist
                gCrossroad.SetHighlight(Highlight.Transparent);
                return;
            }
            else
            {
                // there is an open adjacent road
                gCrossroad.UnsetHighlight(Highlight.Transparent);
            }

            static bool HasOpenRoad(IGRoad gRoad)
            {
                if (gRoad == null)
                    return false;
                foreach (Road r in gRoad.GetRoads())
                    if (r.IsConnected)
                        return true;
                return false;
            }

            if (gCrossroad.MainRoadDirections.HasValue)
            {
                // if one of the main road directions doesn't have an open road, remove main road
                (Coords m1, Coords m2) = gCrossroad.MainRoadDirections.Value;
                IGRoad r1 = GetRoad(new Vector(gCrossroad.CrossroadId, gCrossroad.CrossroadId + m1));
                IGRoad r2 = GetRoad(new Vector(gCrossroad.CrossroadId, gCrossroad.CrossroadId + m2));
                if (!HasOpenRoad(r1) || !HasOpenRoad(r2))
                    gCrossroad.MainRoadDirections = null;
            }
        }

        /// <summary>
        /// Destroy a given GUI crossroad and all its adjacent GUI roads.
        /// You should update all neighbouring crossroads after calling this method.
        /// </summary>
        private bool DestroyGuiCrossroad(IGCrossroad gCrossroad)
        {
            foreach (var road in GetAllGuiRoads(gCrossroad.CrossroadId))
            {
                if (!DestroyGuiRoad(road))
                    return false;
                UpdateGuiCrossroad(guiMap.GetCrossroad(road.To));
            }
            return guiMap.RemoveCrossroad(gCrossroad.CrossroadId);
        }

        /// <summary>
        /// Destroy a given GUI road.
        /// You should update crossroads at both ends after calling this method.
        /// </summary>
        private bool DestroyGuiRoad(IGRoad gRoad)
        {
            Coords? last = null;
            foreach (Coords c in gRoad.GetRoute())
            {
                if (last.HasValue)
                {
                    if (!guiMap.RemoveRoad(new Vector(last.Value, c)))
                        return false;
                }
                last = c;
            }
            return true;
        }

        // TODO: move to static
        private static void FillPriorityCrossing(IGMap gMap, IGCrossroad gCrossroad, Crossroad crossroad)
        {
            Road GetRoad(Coords diff, IGRoad.Direction direction)
            {
                Road road = gMap.GetRoad(new Vector(crossroad.Id, crossroad.Id + diff))?.GetRoad(direction);
                if (road == null || !road.IsConnected)
                    return null;
                else
                    return road;
            }

            foreach (var fromDir in GetAllowedDirections())
            {
                Road fromRoad = GetRoad(fromDir, IGRoad.Direction.Backward);
                if (fromRoad == null)
                    continue;

                var mainRoadDirs = gCrossroad.MainRoadDirections;
                bool isMainRoad = mainRoadDirs.HasValue &&
                    (mainRoadDirs.Value.Item1 == fromDir || mainRoadDirs.Value.Item2 == fromDir);

                foreach (var toDir in GetAllowedDirections())
                {
                    Road toRoad = GetRoad(toDir, IGRoad.Direction.Forward);
                    if (toRoad == null)
                        continue;

                    Direction fromPriority = new(fromRoad.Id, toRoad.Id);
                    // Need to start *after* fromDir and end with it instead
                    var priorDirections = GetAllowedDirections(fromDir).Skip(1).Append(fromDir);
                    bool Pred(Coords dir) => dir != toDir;

                    var priorFromDirections = priorDirections.TakeWhile(Pred);
                    // Give priority also to both main roads
                    if (mainRoadDirs.HasValue && !isMainRoad)
                    {
                        priorFromDirections =
                            priorFromDirections.Append(mainRoadDirs.Value.Item1).Append(mainRoadDirs.Value.Item2);
                    }
                    foreach (var priorFromDir in priorFromDirections)
                    {
                        bool priorIsMainRoad = mainRoadDirs.HasValue &&
                            (mainRoadDirs.Value.Item1 == priorFromDir || mainRoadDirs.Value.Item2 == priorFromDir);
                        // Main road doesn't give priority to side roads
                        if (isMainRoad && !priorIsMainRoad)
                            continue;

                        Road priorFromRoad = GetRoad(priorFromDir, IGRoad.Direction.Backward);
                        if (priorFromRoad == null)
                            continue;

                        IEnumerable<Coords> priorToDirections;
                        if (!isMainRoad && priorIsMainRoad)
                            // Side road gives priority to all directions from a main road
                            priorToDirections = GetAllowedDirections();
                        else
                            priorToDirections = priorDirections.SkipWhile(Pred);

                        foreach (var priorToDir in priorToDirections)
                        {
                            Road priorToRoad = GetRoad(priorToDir, IGRoad.Direction.Forward);
                            if (priorToRoad == null)
                                continue;

                            Direction toPriority = new(priorFromRoad.Id, priorToRoad.Id);
                            crossroad.PriorityCrossing.AddPriority(fromPriority, toPriority);
                        }
                    }
                }
            }
        }

        private void DrawGrid(Graphics graphics, Point origin, float zoom, int width, int height)
        {
            float step = gridSize * zoom;
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
                if (map.GetNode(startingCoords) == null && !NoGuiRoadsAt(guiMap, startingCoords))
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
                gRoad.ResetHighlight(Highlight.Large);
                Route.Add(startCoords);
                CanContinue = true;
                TryGetOrAddGuiCrossroad(startCoords).ResetHighlight(Highlight.Large);
            }

            public bool AddSegment(Coords nextCoords)
            {
                if (!CanContinue)
                    return false;
                if (!CanEnterCoords(nextCoords))
                    return false;
                Coords lastCoords = gRoad.To;
                Vector vector = new(lastCoords, nextCoords);
                Coords diff = vector.Diff();
                if (!Array.Exists(allowedDirections, diff.Equals))
                    return false;
                if (!gMap.AddRoad(gRoad, vector))
                    return false;
                Route.Add(nextCoords);
                CanContinue = map.GetNode(nextCoords) == null;
                return true;
            }

            public bool FinishRoad()
            {
                return FinishRoad(out var _);
            }

            public bool FinishRoad(Speed maxSpeed)
            {
                return FinishRoad(maxSpeed, out var _);
            }

            public bool FinishRoad(out IGRoad builtRoad)
            {
                return FinishRoad(defaultMaxSpeed, out builtRoad);
            }

            public bool FinishRoad(Speed maxSpeed, out IGRoad builtRoad)
            {
                builtRoad = gRoad;
                if (Route.Count < 2)
                    return false;

                IGCrossroad fromG = TryGetOrAddGuiCrossroad(gRoad.From);
                IGCrossroad toG = TryGetOrAddGuiCrossroad(gRoad.To);

                fromG.ResetHighlight(Highlight.None);
                toG.ResetHighlight(Highlight.None);
                gRoad.ResetHighlight(Highlight.None);

                Distance roadLength = (Route.Count - 1) * roadSegmentDefaultLength;
                Road road = map.AddRoad(gRoad.From, gRoad.To, roadLength, maxSpeed);
                gRoad.SetRoad(road, IGRoad.Direction.Forward);

                Crossroad from = (Crossroad)road.FromNode;
                Crossroad to = (Crossroad)road.ToNode;
                if (twoWay)
                {
                    Road backRoad = map.AddRoad(gRoad.To, gRoad.From, roadLength, maxSpeed);
                    gRoad.SetRoad(backRoad, IGRoad.Direction.Backward);
                    // Set TrafficLight to always allow backward direction
                    from.TrafficLight.AddDefaultDirection(backRoad.Id, road.Id);
                    to.TrafficLight.AddDefaultDirection(road.Id, backRoad.Id);
                }

                FillPriorityCrossing(gMap, fromG, from);
                FillPriorityCrossing(gMap, toG, to);
                Invalidate();
                return true;
            }

            public void DestroyRoad()
            {
                Coords last = Route.First();
                if (map.GetNode(last) == null)
                    gMap.RemoveCrossroad(last);
                else
                    gMap.GetCrossroad(last).ResetHighlight(Highlight.None);
                foreach (Coords curr in Route.Skip(1))
                {
                    gMap.RemoveRoad(new Vector(last, curr));
                    last = curr;
                }
                Invalidate();
            }

            private IGCrossroad TryGetOrAddGuiCrossroad(Coords coords)
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
                return NoGuiRoadsAt(gMap, newCoords);
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
