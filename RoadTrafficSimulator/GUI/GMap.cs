﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using RoadTrafficSimulator.ValueTypes;

namespace RoadTrafficSimulator.GUI
{
    class GMap : IGMap
    {
        private const int crossroadSize = 20;
        private const int roadWidth = 7;

        private Dictionary<Coords, IGCrossroad> crossroads = new Dictionary<Coords, IGCrossroad>();
        private Dictionary<Vector, IGRoad> roadSegments = new Dictionary<Vector, IGRoad>();

        public bool AddCrossroad(IGCrossroad crossroad, Coords coords) =>crossroads.TryAdd(coords, crossroad);

        public bool AddRoad(IGRoad road, Vector vector) => roadSegments.TryAdd(vector, road);

        public bool RemoveCrossroad(Coords coords) => crossroads.Remove(coords);

        public bool RemoveRoad(Vector vector) => roadSegments.Remove(vector);

        public IGCrossroad GetCrossroad(Coords coords)
        {
            if (!crossroads.TryGetValue(coords, out IGCrossroad output))
                return null;
            return output;
        }

        public IGRoad GetRoad(Vector vector, bool ignoreDirection)
        {
            if (!roadSegments.TryGetValue(vector, out IGRoad output))
            {
                if (!ignoreDirection)
                    return null;
                else if (!roadSegments.TryGetValue(vector.Reverse(), out output))
                    return null;
            }
            return output;
        }

        public IEnumerable<IGCrossroad> GetCrossroads() => crossroads.Select(pair => pair.Value);

        public IEnumerable<IGRoad> GetRoads() => roadSegments.Select(pair => pair.Value).Distinct();

        public void Draw(Graphics graphics, Point origin, float zoom, int width, int height)
        {
            int realRoadWidth = (int)(roadWidth * zoom);
            foreach (var (vector, road) in roadSegments)
            {
                Point from = MapManager.CalculatePoint(vector.from, origin, zoom);
                Point to = MapManager.CalculatePoint(vector.to, origin, zoom);
                if (IsInRange(from, width, height) || IsInRange(to, width, height))
                    road.Draw(graphics, from, to, realRoadWidth);
            }
            int realCrossroadSize = (int)(crossroadSize * zoom);
            foreach (var (coords, crossroad) in crossroads)
            {
                Point point = MapManager.CalculatePoint(coords, origin, zoom);
                if (IsInRange(point, width, height))
                    crossroad.Draw(graphics, point, realCrossroadSize);
            }
        }

        private static bool IsInRange (Point point, int width, int height)
        {
            return point.X >= 0 && point.X <= width && point.Y >= 0 && point.Y <= height;
        }
    }
}