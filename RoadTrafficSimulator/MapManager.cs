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

        public const int gridSize = 120;

        private static readonly Speed defaultMaxSpeed = 50.KilometresPerHour();

        public static readonly Distance roadSegmentDefaultLength = 100.Metres();

        public static IRoadBuilder CreateRoadBuilder(Map map, IGMap guiMap, Coords startingCoords,
            bool twoWayRoad)
        {
            return RoadBuilder.CreateRoadBuilder(map, guiMap, startingCoords, twoWayRoad);
        }

        public static bool NoGuiRoadsAt(IGMap guiMap, Coords coords)
        {
            foreach (Coords diff in CoordsConvertor.GetAllowedDirections(guiMap.SideOfDriving))
                if (guiMap.GetRoad(new Vector(coords, coords + diff)) != null)
                    return false;
            return true;
        }

        // needs to be static so it can be called from RoadBuilder
        private static void FillPriorityCrossing(IGMap gMap, IGCrossroad gCrossroad, Map map)
        {
            FillPriorityCrossing(gMap, gCrossroad, map.GetNode(gCrossroad.CrossroadId) as Crossroad);
        }

        private static void FillPriorityCrossing(IGMap gMap, IGCrossroad gCrossroad, Crossroad crossroad)
        {
            Road GetRoad(Coords diff, IGRoad.Direction direction)
            {
                Road road = gMap.GetRoad(new Vector(crossroad.Id, crossroad.Id + diff))?.GetRoad(direction);
                if (road?.IsConnected == true)
                    return road;
                else
                    return null;
            }

            foreach (var fromDir in CoordsConvertor.GetAllowedDirections(gMap.SideOfDriving))
            {
                Road fromRoad = GetRoad(fromDir, IGRoad.Direction.Backward);
                if (fromRoad == null)
                    continue;

                var mainRoadDirs = gCrossroad.MainRoadDirections;
                bool isMainRoad = gCrossroad.IsMainRoadDirection(fromDir);

                foreach (var toDir in CoordsConvertor.GetAllowedDirections(gMap.SideOfDriving))
                {
                    Road toRoad = GetRoad(toDir, IGRoad.Direction.Forward);
                    if (toRoad == null)
                        continue;

                    Direction fromPriority = new(fromRoad.Id, toRoad.Id);
                    // Need to start *after* fromDir and end with it instead
                    var priorDirections = CoordsConvertor.GetAllowedDirections(gMap.SideOfDriving, fromDir)
                        .Skip(1)
                        .Append(fromDir);
                    bool Pred(Coords dir) => dir != toDir;

                    var priorFromDirections = priorDirections.TakeWhile(Pred);
                    // Give priority also to both main roads
                    if (mainRoadDirs.HasValue && !isMainRoad)
                    {
                        priorFromDirections = priorFromDirections
                            .Append(CoordsConvertor.GetCoords(mainRoadDirs.Value.Item1))
                            .Append(CoordsConvertor.GetCoords(mainRoadDirs.Value.Item2));
                    }
                    foreach (var priorFromDir in priorFromDirections)
                    {
                        bool priorIsMainRoad = gCrossroad.IsMainRoadDirection(priorFromDir);
                        // Main road doesn't give priority to side roads
                        if (isMainRoad && !priorIsMainRoad)
                            continue;

                        Road priorFromRoad = GetRoad(priorFromDir, IGRoad.Direction.Backward);
                        if (priorFromRoad == null)
                            continue;

                        IEnumerable<Coords> priorToDirections;
                        if (!isMainRoad && priorIsMainRoad)
                            // Side road gives priority to all directions from a main road
                            priorToDirections = CoordsConvertor.GetAllowedDirections(gMap.SideOfDriving);
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

        private static void DrawGrid(Graphics graphics, Point origin, float zoom, int width, int height)
        {
            float step = gridSize * zoom;
            Coords firstCoords = CoordsConvertor.CalculateCoords(new Point(0, 0), origin, zoom);
            Point firstPoint = CoordsConvertor.CalculatePoint(firstCoords, origin, zoom);

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

        #endregion static

        private IGMap guiMap = new GMap();

        public Map Map { get; private set; } = new Map();
        public RoadSide SideOfDriving { get => guiMap.SideOfDriving; }

        #region publicMethods

        public void SetSideOfDriving(RoadSide sideOfDriving)
        {
            if (sideOfDriving == guiMap.SideOfDriving)
                return;
            guiMap.SideOfDriving = sideOfDriving;
            // Need to update priority crossing after the change
            foreach (IGCrossroad gCrossroad in guiMap.GetCrossroads())
            {
                Crossroad crossroad = (Crossroad)Map.GetNode(gCrossroad.CrossroadId);
                if (crossroad != null)
                    FillPriorityCrossing(guiMap, gCrossroad, crossroad);
            }
        }

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
            Coords coords = CoordsConvertor.CalculateCoords(point, origin, zoom);
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

            Coords coords = CoordsConvertor.CalculateCoords(point, origin, zoom);
            Point crossroadPoint = CoordsConvertor.CalculatePoint(coords, origin, zoom);
            double distance = CalculateDistance(crossroadPoint, point);
            proximity = distance / (gridSize * zoom);
            return GetCrossroad(coords);
        }

        public IGRoad GetRoad(Vector vector)
        {
            return guiMap.GetRoad(vector);
        }

        public IGRoad GetRoad(Coords from, CoordsConvertor.Direction direction)
        {
            Coords to = from + CoordsConvertor.GetCoords(direction);
            return GetRoad(new Vector(from, to));
        }

        public IGRoad GetNearestRoad(Point point, Point origin, float zoom)
        {
            Coords coords = CoordsConvertor.CalculateCoords(point, origin, zoom);

            (Vector, IGRoad) FindNextRoad(IEnumerator<Coords> e)
            {
                while (e.MoveNext())
                {
                    Vector vector = new(coords, coords + e.Current);
                    IGRoad road = GetRoad(vector);
                    if (road != null)
                        return (vector, road);
                }
                return (new(), null);
            }

            var nearestCrossroad = GetCrossroad(coords);
            // No crossroad in the area
            if (nearestCrossroad == null)
            {
                IEnumerator<Coords> e = CoordsConvertor.GetAllowedDirections(guiMap.SideOfDriving).GetEnumerator();
                var (vector1, road1) = FindNextRoad(e);
                var (vector2, road2) = FindNextRoad(e);
                if (road1 == null)
                    return null;
                else
                {
                    Debug.Assert(road2 != null);
                    Debug.Assert(vector1.from == coords);
                    Debug.Assert(vector2.from == coords);
                    if (CoordsConvertor.IsCorrectDirection(guiMap.SideOfDriving, vector1, vector2, point, origin, zoom))
                        return road1;
                    else
                        return road2;
                }
            }
            // Crossroad nearby
            else
            {
                Vector vector = CoordsConvertor.CalculateVector(point, origin, zoom);
                if (!CoordsConvertor.IsCorrectDirection(guiMap.SideOfDriving, vector, point, origin, zoom))
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

            return CoordsConvertor.GetAllowedDirections(guiMap.SideOfDriving)
                .Select(Selector)
                .Where(gRoad => gRoad != null);
        }

        /// <summary>
        /// Gets all GUI roads going from the starting point of <paramref name="vector"/> sorted according to
        /// <see cref="roadSide"/>, starting from the direction of <paramref name="vector"/>.
        /// May return the same road twice (in both directions) if it just passes through.
        /// </summary>
        public IEnumerable<IGRoad> GetAllGuiRoads(Vector vector)
        {
            IGRoad Selector(Coords diff) => GetRoad(new Vector(vector.from, vector.from + diff));

            return CoordsConvertor.GetAllowedDirections(guiMap.SideOfDriving, vector.Diff())
                .Select(Selector)
                .Where(gRoad => gRoad != null);
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

            FillPriorityCrossing(guiMap, fromCrossroad, Map);
            FillPriorityCrossing(guiMap, toCrossroad, Map);
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

        public bool CanBeMainRoad(IGCrossroad gCrossroad,
            CoordsConvertor.Direction dir1, CoordsConvertor.Direction dir2)
        {
            IGRoad road1 = GetRoad(gCrossroad.CrossroadId, dir1);
            IGRoad road2 = GetRoad(gCrossroad.CrossroadId, dir2);

            if (road1 == null || road2 == null)
                return false;
            if (road1.GetRoad(IGRoad.Direction.Backward)?.IsConnected == true &&
                road2.GetRoad(IGRoad.Direction.Forward)?.IsConnected == true)
                return true;
            if (road2.GetRoad(IGRoad.Direction.Backward)?.IsConnected == true &&
                road1.GetRoad(IGRoad.Direction.Forward)?.IsConnected == true)
                return true;
            return false;
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
                foreach (IGCrossroad gCrossroad in guiMap.GetCrossroads())
                {
                    // Update crossroads to account for closed roads
                    UpdateGuiCrossroad(gCrossroad);
                    // Fill priority crossing rules (done here for efficiency) - only for open crossroads!
                    Crossroad crossroad = (Crossroad)Map.GetNode(gCrossroad.CrossroadId);
                    if (crossroad != null)
                        FillPriorityCrossing(guiMap, gCrossroad, crossroad);
                }
            }
            return result;
        }

        #endregion publicMethods

        #region privateMethods

        /// <summary>
        /// Updates highlighting and main roads of a GUI crossroad based on adjacent roads.
        /// If there are no roads left, removes the crossroad.
        /// </summary>
        private void UpdateGuiCrossroad(IGCrossroad gCrossroad)
        {
            if (NoGuiRoadsAt(guiMap, gCrossroad.CrossroadId))
            {
                // No adjacent roads exist, remove the crossroad
                guiMap.RemoveCrossroad(gCrossroad.CrossroadId);
                return;
            }
            else if (Map.GetNode(gCrossroad.CrossroadId) == null)
            {
                // Only closed roads exist
                gCrossroad.SetHighlight(Highlight.Transparent);
                return;
            }
            else
            {
                // There is an open adjacent road
                gCrossroad.UnsetHighlight(Highlight.Transparent);
            }

            if (gCrossroad.MainRoadDirections.HasValue)
            {
                // If the main road is no longer valid, remove it
                var (dir1, dir2) = gCrossroad.MainRoadDirections.Value;
                if (!CanBeMainRoad(gCrossroad, dir1, dir2))
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
            return guiMap.RemoveRoad(gRoad);
        }

        #endregion privateMethods

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
                if (!CoordsConvertor.IsAllowedDirection(vector.Diff()))
                    return false;
                if (!gMap.AddRoadSegment(gRoad, vector))
                    return false;
                Route.Add(nextCoords);
                CanContinue = map.GetNode(nextCoords) == null;
                return true;
            }

            public bool FinishRoad(bool updatePriorityCrossing)
            {
                return FinishRoad(out var _, updatePriorityCrossing);
            }

            public bool FinishRoad(Speed maxSpeed, bool updatePriorityCrossing)
            {
                return FinishRoad(maxSpeed, out var _, updatePriorityCrossing);
            }

            public bool FinishRoad(out IGRoad builtRoad, bool updatePriorityCrossing)
            {
                return FinishRoad(defaultMaxSpeed, out builtRoad, updatePriorityCrossing);
            }

            public bool FinishRoad(Speed maxSpeed, out IGRoad builtRoad, bool updatePriorityCrossing)
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

                if (updatePriorityCrossing)
                {
                    FillPriorityCrossing(gMap, fromG, from);
                    FillPriorityCrossing(gMap, toG, to);
                }
                Invalidate();
                return true;
            }

            public void DestroyRoad()
            {
                Coords first = Route.First();
                if (map.GetNode(first) == null)
                    gMap.RemoveCrossroad(first);
                else
                    gMap.GetCrossroad(first).ResetHighlight(Highlight.None);
                gMap.RemoveRoad(gRoad);
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
