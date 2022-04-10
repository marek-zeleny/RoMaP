using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using RoadTrafficSimulator.ValueTypes;

namespace RoadTrafficSimulator.GUI
{
    class GMap : IGMap
    {
        private Dictionary<Coords, IGCrossroad> crossroads = new();
        private Dictionary<Vector, IGRoad> roadSegments = new();
        private HashSet<IGRoad> roads = new();

        public bool AddCrossroad(IGCrossroad crossroad, Coords coords)
        {
            return crossroads.TryAdd(coords, crossroad);
        }

        public bool AddRoadSegment(IGRoad road, Vector vector)
        {
            if (roadSegments.ContainsKey(vector) || roadSegments.ContainsKey(vector.Reverse()))
                return false;
            roadSegments.Add(vector, road);
            roads.Add(road);
            return true;
        }

        public bool RemoveCrossroad(Coords coords)
        {
            return crossroads.Remove(coords);
        }

        public bool RemoveRoad(IGRoad road)
        {
            var route = road.GetRoute();
            Coords prev = route.First();
            foreach (Coords coords in route.Skip(1))
            {
                Vector v = new(coords, prev);
                if (!roadSegments.Remove(v) && !roadSegments.Remove(v.Reverse()))
                    return false;
                prev = coords;
            }
            return roads.Remove(road) || roads.Remove(road.GetReversedGRoad());
        }

        public bool RemoveRoadAt(Vector vector)
        {
            if (!roadSegments.TryGetValue(vector, out IGRoad road)
                && !roadSegments.TryGetValue(vector.Reverse(), out road))
                return false;
            return RemoveRoad(road);
        }

        public IGCrossroad GetCrossroad(Coords coords)
        {
            if (!crossroads.TryGetValue(coords, out IGCrossroad output))
                return null;
            return output;
        }

        public IGRoad GetRoad(Vector vector)
        {
            if (roadSegments.TryGetValue(vector, out IGRoad output))
                return output;
            else if (roadSegments.TryGetValue(vector.Reverse(), out output))
                return output.GetReversedGRoad();
            else
                return null;
        }

        public IEnumerable<IGCrossroad> GetCrossroads() => crossroads.Select(pair => pair.Value);

        public IEnumerable<IGRoad> GetRoads() => roadSegments.Select(pair => pair.Value).Distinct();

        public void Draw(Graphics graphics, Point origin, float zoom, int width, int height, bool simulationMode)
        {
            bool IsVisible(Point point) => IsInRange(point, width, height);

            foreach (IGRoad road in roads)
                road.Draw(graphics, origin, zoom, simulationMode, IsVisible);

            foreach (var crossroad in crossroads.Values)
                crossroad.Draw(graphics, origin, zoom, IsVisible);
        }

        private static bool IsInRange (Point point, int width, int height)
        {
            return point.X >= 0 && point.X <= width && point.Y >= 0 && point.Y <= height;
        }
    }
}
