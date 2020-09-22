using System;
using System.Collections.Generic;
using System.Drawing;

using RoadTrafficSimulator.ValueTypes;

namespace RoadTrafficSimulator.GUI
{
    class Map : IMap
    {
        private Dictionary<Coords, ICrossroad> crossroads;
        private Dictionary<Vector, IRoadSegment> roadSegments;

        public bool AddCrossroad(ICrossroad crossroad, Coords coords)
        {
            return crossroads.TryAdd(coords, crossroad);
        }

        public bool AddRoadSegment(IRoadSegment roadSegment, Vector vector)
        {
            return roadSegments.TryAdd(vector, roadSegment);
        }

        public ICrossroad GetCrossroad(Coords coords)
        {
            if (!crossroads.TryGetValue(coords, out ICrossroad output))
                return null;
            return output;
        }

        public IRoadSegment GetRoadSegment(Vector vector)
        {
            if (!roadSegments.TryGetValue(vector, out IRoadSegment output))
                return null;
            return output;
        }

        public void Draw(Graphics graphics, Coords origin, decimal zoom, int width, int height)
        {
            foreach (var (vector, roadSegment) in roadSegments)
            {
                Point from = CalculatePoint(vector.from, origin, zoom);
                Point to = CalculatePoint(vector.to, origin, zoom);
                if (IsInRange(from, width, height) || IsInRange(to, width, height))
                    roadSegment.Draw(graphics, from, to, 10);
            }
            foreach (var (coords, crossroad) in crossroads)
            {
                Point point = CalculatePoint(coords, origin, zoom);
                if (IsInRange(point, width, height))
                    crossroad.Draw(graphics, point, 20);
            }
        }

        private Point CalculatePoint (Coords coords, Coords origin, decimal zoom)
        {
            throw new NotImplementedException(); // TODO
        }

        private bool IsInRange (Point point, int width, int height)
        {
            return point.X >= 0 && point.X <= width && point.Y >= 0 && point.Y <= height;
        }
    }
}
