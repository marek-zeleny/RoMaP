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
    /// <summary>
    /// Manages both back-end and graphical maps and provides methods for interacting with them from the user interface.
    /// </summary>
    class MapManager
    {
        #region static

        /// <summary>
        /// Default setting for roads' maximum speed
        /// </summary>
        private static readonly Speed defaultMaxSpeed = 50.KilometresPerHour();
        /// <summary>
        /// Default length of one road segment
        /// </summary>
        public static readonly Distance roadSegmentDefaultLength = 100.Metres();

        /// <inheritdoc cref="RoadBuilder.CreateRoadBuilder(Map, IGMap, Coords)"/>
        public static IRoadBuilder CreateRoadBuilder(Map map, IGMap guiMap, Coords startingCoords)
        {
            return RoadBuilder.CreateRoadBuilder(map, guiMap, startingCoords);
        }

        /// <summary>
        /// Checks if there is no graphical road going through given coordinates on a given GUI map.
        /// </summary>
        /// <returns><c>true</c> if there is a road going through the coordinates, otherwise <c>false</c></returns>
        public static bool NoGuiRoadsAt(IGMap guiMap, Coords coords)
        {
            foreach (Coords diff in CoordsConvertor.GetAllowedDirections(guiMap.SideOfDriving))
                if (guiMap.GetRoad(new Vector(coords, coords + diff)) != null)
                    return false;
            return true;
        }

        // needs to be static so it can be called from RoadBuilder
        /// <summary>
        /// Fills priority crossing rules at a given crossroad.
        /// </summary>
        /// <param name="gCrossroad">GUI crossroad to fill the priority crossing rules at</param>
        /// <param name="gMap">GUI map where the crossroad is located</param>
        /// <param name="map">Back-end map where the crossroad is located</param>
        private static void FillPriorityCrossing(IGCrossroad gCrossroad, IGMap gMap, Map map)
        {
            FillPriorityCrossing(gCrossroad, gMap, map.GetNode(gCrossroad.CrossroadId) as Crossroad);
        }

        /// <inheritdoc cref="FillPriorityCrossing(IGCrossroad, IGMap, Map)"/>
        /// <param name="crossroad">Back-end crossroad corresponding to the GUI crossroad</param>
        private static void FillPriorityCrossing(IGCrossroad gCrossroad, IGMap gMap, Crossroad crossroad)
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
                    var mainRoadDirs = gCrossroad.MainRoadDirections;
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

        #endregion static

        private IGMap guiMap = new GMap();

        /// <summary>
        /// Back-end map attached to the manager
        /// </summary>
        public Map Map { get; private set; } = new();
        /// <summary>
        /// Side of driving associated with the map managed by the manager
        /// </summary>
        public RoadSide SideOfDriving { get => guiMap.SideOfDriving; }

        #region publicMethods

        /// <summary>
        /// Sets a given road side as the new side of driving for the managed map.
        /// </summary>
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
                    FillPriorityCrossing(gCrossroad, guiMap, crossroad);
            }
        }

        /// <summary>
        /// Gets a crossroad at given coordinates.
        /// </summary>
        /// <returns>Wrapper containing a crossroad if it exists at the coordinates, otherwise <c>null</c></returns>
        public CrossroadWrapper? GetCrossroad(Coords coords)
        {
            IGCrossroad gCrossroad = guiMap.GetCrossroad(coords);
            if (gCrossroad == null)
                return null;
            Crossroad crossroad = (Crossroad)Map.GetNode(coords); // May be null if only a GUI crossroad exists
            return new CrossroadWrapper(gCrossroad, crossroad);
        }

        /// <summary>
        /// Gets a crossroad at coordinates nearest to a given point.
        /// </summary>
        /// <param name="origin">Position of the map's origin</param>
        /// <param name="zoom">Current zoom of the map</param>
        /// <returns>
        /// Wrapper containing a crossroad if it exists at the nearest coordinates, otherwise <c>null</c>
        /// </returns>
        public CrossroadWrapper? GetNearestCrossroad(Point point, Point origin, float zoom)
        {
            Coords coords = CoordsConvertor.CalculateCoords(point, origin, zoom);
            return GetCrossroad(coords);
        }

        /// <param name="proximity">
        /// Outputs proximity coefficient of the point from the nearest crossroad relative to the distance between two
        /// neighbouring coordinates; if there is no crossroad, the value is undefined
        /// </param>
        /// <inheritdoc cref="GetNearestCrossroad(Point, Point, float)"/>
        public CrossroadWrapper? GetNearestCrossroad(Point point, Point origin, float zoom, out double proximity)
        {
            Coords coords = CoordsConvertor.CalculateCoords(point, origin, zoom);
            Point crossroadPoint = CoordsConvertor.CalculatePoint(coords, origin, zoom);
            proximity = CoordsConvertor.RelativeDistance(crossroadPoint, point, zoom);
            return GetCrossroad(coords);
        }

        /// <summary>
        /// Gets a road passing through a given vector.
        /// </summary>
        /// <returns>
        /// GUI road passing through the vector oriented in the vector's direction if it exists, otherwise <c>null</c>
        /// </returns>
        public IGRoad GetRoad(Vector vector)
        {
            return guiMap.GetRoad(vector);
        }

        /// <summary>
        /// Gets a road passing through given coordinates in a given direction.
        /// </summary>
        /// <returns>
        /// GUI road passing through the coordinates oriented in the given direction if it exists, otherwise <c>null</c>
        /// </returns>
        public IGRoad GetRoad(Coords from, CoordsConvertor.Direction direction)
        {
            Coords to = from + CoordsConvertor.GetCoords(direction);
            return GetRoad(new Vector(from, to));
        }

        /// <summary>
        /// Gets a road passing through a vector closest to a given point.
        /// </summary>
        /// <param name="origin">Position of the map's origin</param>
        /// <param name="zoom">Current zoom of the map</param>
        /// <returns>
        /// GUI road passing close to the point, oriented so that the forward direction has the road closer to that
        /// point (in case of a two-way road); if no road passes by, returns <c>null</c>
        /// </returns>
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
                    if (CoordsConvertor.ChooseCorrectOrientation(guiMap.SideOfDriving, vector1, vector2, point, origin, zoom))
                        return road1;
                    else
                        return road2;
                }
            }
            // Crossroad nearby
            else
            {
                Vector vector = CoordsConvertor.CalculateVector(point, origin, zoom);
                if (!CoordsConvertor.IsCorrectOrientation(guiMap.SideOfDriving, vector, point, origin, zoom))
                    vector = vector.Reverse();
                return GetRoad(vector);
            }
        }

        /// <summary>
        /// Gets all roads going through given coordinates.
        /// </summary>
        /// <remarks>
        /// May return the same road twice (once in each direction) if it just passes through the coordinates.
        /// </remarks>
        /// <returns>
        /// Sequence of roads oriented away from the coordinates sorted according to current side of driving
        /// </returns>
        public IEnumerable<IGRoad> GetAllRoads(Coords coords)
        {
            IGRoad Selector(Coords diff) => GetRoad(new Vector(coords, coords + diff));

            return CoordsConvertor.GetAllowedDirections(guiMap.SideOfDriving)
                .Select(Selector)
                .Where(gRoad => gRoad != null);
        }

        /// <summary>
        /// Gets all roads going through the base of a given vector, starting from the direction of the vector.
        /// </summary>
        /// <inheritdoc cref="GetAllRoads(Coords)"/>
        public IEnumerable<IGRoad> GetAllRoads(Vector vector)
        {
            IGRoad Selector(Coords diff) => GetRoad(new Vector(vector.from, vector.from + diff));

            return CoordsConvertor.GetAllowedDirections(guiMap.SideOfDriving, vector.Diff())
                .Select(Selector)
                .Where(gRoad => gRoad != null);
        }

        /// <summary>
        /// Gets a new road builder instance at given coordinates.
        /// </summary>
        /// <inheritdoc cref="CreateRoadBuilder(Map, IGMap, Coords)"/>
        public IRoadBuilder GetRoadBuilder(Coords startingCoords)
        {
            return CreateRoadBuilder(Map, guiMap, startingCoords);
        }

        /// <summary>
        /// Closes a given road.
        /// </summary>
        public void CloseRoad(IGRoad gRoad)
        {
            Debug.Assert(gRoad.GetRoad().IsConnected);
            Map.RemoveRoad(gRoad.GetRoad().Id);
            gRoad.SetHighlight(Highlight.Transparent, IGRoad.Direction.Forward);
            UpdateGuiCrossroad(guiMap.GetCrossroad(gRoad.From));
            UpdateGuiCrossroad(guiMap.GetCrossroad(gRoad.To));
        }

        /// <summary>
        /// Reopens a given closed road.
        /// </summary>
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

            FillPriorityCrossing(fromCrossroad, guiMap, Map);
            FillPriorityCrossing(toCrossroad, guiMap, Map);
        }

        /// <summary>
        /// Destroys a given crossroad, as well as any connected roads.
        /// </summary>
        /// <returns><c>true</c> if the crossroad was successfully destroyed, otherwise <c>false</c></returns>
        public bool DestroyCrossroad(IGCrossroad gCrossroad)
        {
            Map.RemoveCrossroad(gCrossroad.CrossroadId);
            return DestroyGuiCrossroad(gCrossroad);
        }

        /// <summary>
        /// Destroys a given road.
        /// </summary>
        /// <remarks>
        /// If a crossroad becomes isolated as a result of this operation, it's destroyed as well.
        /// </remarks>
        /// <returns><c>true</c> if the road was successfully destroyed, otherwise <c>false</c></returns>
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

        /// <summary>
        /// Checks if given two directions can form a main road at a given crossroad.
        /// </summary>
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

        /// <summary>
        /// Draws the managed map onto given graphics.
        /// </summary>
        /// <inheritdoc cref="IGMap.Draw(Graphics, Point, float, int, int, bool)"/>
        public void Draw(Graphics graphics, Point origin, float zoom, int width, int height, bool simulationMode)
        {
            guiMap.Draw(graphics, origin, zoom, width, height, simulationMode);
        }

        /// <summary>
        /// Saves the managed map using a given stream.
        /// </summary>
        public void SaveMap(Stream stream)
        {
            MapSaverLoader.SaveMap(stream, Map, guiMap);
        }

        /// <summary>
        /// Loads a saved map from a given stream.
        /// </summary>
        /// <returns><c>true</c> if the map was successfully loaded, otherwise <c>false</c></returns>
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
                        FillPriorityCrossing(gCrossroad, guiMap, crossroad);
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
        /// Destroys a given GUI crossroad and all its adjacent GUI roads.
        /// </summary>
        /// <remarks>
        /// All neighbouring crossroads should be updated after calling this method.
        /// </remarks>
        private bool DestroyGuiCrossroad(IGCrossroad gCrossroad)
        {
            foreach (var road in GetAllRoads(gCrossroad.CrossroadId))
            {
                if (!DestroyGuiRoad(road))
                    return false;
                UpdateGuiCrossroad(guiMap.GetCrossroad(road.To));
            }
            return guiMap.RemoveCrossroad(gCrossroad.CrossroadId);
        }

        /// <summary>
        /// Destroys a given GUI road.
        /// </summary>
        /// <remarks>
        /// Crossroads at both ends should be updated after calling this method.
        /// </remarks>
        private bool DestroyGuiRoad(IGRoad gRoad)
        {
            return guiMap.RemoveRoad(gRoad);
        }

        #endregion privateMethods

        #region nested_types

        /// <summary>
        /// Wrapper around a crossroad and its corresponding graphical component.
        /// </summary>
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

        /// <summary>
        /// Represents a builder of a single new road.
        /// </summary>
        private class RoadBuilder : IRoadBuilder
        {
            /// <summary>
            /// Creates a new road builder instance for given maps starting at given coordinates.
            /// </summary>
            /// <returns>
            /// Instance of a road builder if a road can begin at the starting coordinates, otherwise <c>null</c>
            /// </returns>
            public static IRoadBuilder CreateRoadBuilder(Map map, IGMap guiMap, Coords startingCoords)
            {
                if (map.GetNode(startingCoords) == null && !NoGuiRoadsAt(guiMap, startingCoords))
                    return null;
                else
                    return new RoadBuilder(map, guiMap, startingCoords);
            }

            private Map map;
            private IGMap gMap;
            private IMutableGRoad gRoad;

            private ICollection<Coords> Route { get => gRoad.Route; }

            public bool CanContinue { get; private set; }

            /// <summary>
            /// Creates a new instance for given maps starting at given coordinates.
            /// </summary>
            private RoadBuilder(Map map, IGMap gMap, Coords startCoords)
            {
                this.map = map;
                this.gMap = gMap;
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

            public bool FinishRoad(bool twoWayRoad, bool updatePriorityCrossing)
            {
                return FinishRoad(twoWayRoad, out var _, updatePriorityCrossing);
            }

            public bool FinishRoad(bool twoWayRoad, Speed maxSpeed, bool updatePriorityCrossing)
            {
                return FinishRoad(twoWayRoad, maxSpeed, out var _, updatePriorityCrossing);
            }

            public bool FinishRoad(bool twoWayRoad, out IGRoad builtRoad, bool updatePriorityCrossing)
            {
                return FinishRoad(twoWayRoad, defaultMaxSpeed, out builtRoad, updatePriorityCrossing);
            }

            public bool FinishRoad(bool twoWayRoad, Speed maxSpeed, out IGRoad builtRoad, bool updatePriorityCrossing)
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
                if (twoWayRoad)
                {
                    Road backRoad = map.AddRoad(gRoad.To, gRoad.From, roadLength, maxSpeed);
                    gRoad.SetRoad(backRoad, IGRoad.Direction.Backward);
                    // Set TrafficLight to always allow backward direction
                    from.TrafficLight.AddDefaultDirection(backRoad.Id, road.Id);
                    to.TrafficLight.AddDefaultDirection(road.Id, backRoad.Id);
                }

                if (updatePriorityCrossing)
                {
                    FillPriorityCrossing(fromG, gMap, from);
                    FillPriorityCrossing(toG, gMap, to);
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

            /// <summary>
            /// Gets a crossroad at given coordinates and if no such crossroad exists, creates a new one.
            /// </summary>
            /// <returns>Crossroad at given coordinates, either existing or a new one</returns>
            private IGCrossroad TryGetOrAddGuiCrossroad(Coords coords)
            {
                IGCrossroad output = gMap.GetCrossroad(coords);
                if (output != null)
                    return output;
                output = new GCrossroad(coords);
                gMap.AddCrossroad(output);
                return output;
            }

            /// <summary>
            /// Checks if the road being built can continue at given coordinates.
            /// </summary>
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

            /// <summary>
            /// Invalidates the road builder instance and disables any further building.
            /// </summary>
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
