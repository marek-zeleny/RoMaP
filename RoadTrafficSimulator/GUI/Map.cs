using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using RoadTrafficSimulator.ValueTypes;

namespace RoadTrafficSimulator.GUI
{
    class Map : IMap
    {
        private const int crossroadSize = 15;
        private const int roadWidth = 5;

        private Dictionary<Coords, ICrossroad> crossroads = new Dictionary<Coords, ICrossroad>();
        private Dictionary<Vector, IRoad> roadSegments = new Dictionary<Vector, IRoad>();

        public bool AddCrossroad(ICrossroad crossroad, Coords coords) =>crossroads.TryAdd(coords, crossroad);

        public bool AddRoad(IRoad road, Vector vector) => roadSegments.TryAdd(vector, road);

        public bool RemoveCrossroad(Coords coords) => crossroads.Remove(coords);

        public bool RemoveRoad(Vector vector) => roadSegments.Remove(vector);

        public ICrossroad GetCrossroad(Coords coords)
        {
            if (!crossroads.TryGetValue(coords, out ICrossroad output))
                return null;
            return output;
        }

        public IRoad GetRoad(Vector vector, bool ignoreDirection)
        {
            if (!roadSegments.TryGetValue(vector, out IRoad output))
            {
                if (!ignoreDirection)
                    return null;
                else if (!roadSegments.TryGetValue(vector.Reverse(), out output))
                    return null;
            }
            return output;
        }

        public IEnumerable<ICrossroad> GetCrossroads() => crossroads.Select(pair => pair.Value);

        public IEnumerable<IRoad> GetRoads() => roadSegments.Select(pair => pair.Value).Distinct();

        public void Draw(Graphics graphics, Point origin, decimal zoom, int width, int height)
        {
            foreach (var (vector, road) in roadSegments)
            {
                Point from = MapManager.CalculatePoint(vector.from, origin, zoom);
                Point to = MapManager.CalculatePoint(vector.to, origin, zoom);
                if (IsInRange(from, width, height) || IsInRange(to, width, height))
                    road.Draw(graphics, from, to, roadWidth);
            }
            foreach (var (coords, crossroad) in crossroads)
            {
                Point point = MapManager.CalculatePoint(coords, origin, zoom);
                if (IsInRange(point, width, height))
                    crossroad.Draw(graphics, point, crossroadSize);
            }
        }

        private bool IsInRange (Point point, int width, int height)
        {
            return point.X >= 0 && point.X <= width && point.Y >= 0 && point.Y <= height;
        }
    }
}
