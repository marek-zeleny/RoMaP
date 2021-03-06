using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using RoadTrafficSimulator.ValueTypes;

namespace RoadTrafficSimulator.GUI
{
    /// <summary>
    /// Represents the graphical part of a map.
    /// </summary>
    class GMap : IGMap
    {
        private Dictionary<Coords, IGCrossroad> crossroads = new();
        private Dictionary<Vector, IGRoad> roadSegments = new();
        private HashSet<IGRoad> roads = new();

        public RoadSide SideOfDriving { get; set; } = RoadSide.Right;

        public bool AddCrossroad(IGCrossroad crossroad)
        {
            return crossroads.TryAdd(crossroad.CrossroadId, crossroad);
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

            DrawGrid(graphics, origin, zoom, width, height);

            foreach (IGRoad road in roads)
                road.Draw(graphics, origin, zoom, SideOfDriving, simulationMode, IsVisible);

            foreach (var crossroad in crossroads.Values)
                crossroad.Draw(graphics, origin, zoom, IsVisible);
        }

        /// <summary>
        /// Determines if a given point is within a range of given dimensions.
        /// </summary>
        /// <returns><c>true</c> if the point is within the range, otherwise <c>false</c></returns>
        private static bool IsInRange (Point point, int width, int height)
        {
            return point.X >= 0
                && point.X <= width
                && point.Y >= 0
                && point.Y <= height;
        }

        /// <summary>
        /// Draws a coordinate grid onto given graphics.
        /// </summary>
        /// <param name="origin">Position of the map's origin</param>
        /// <param name="zoom">Current zoom of the map</param>
        /// <param name="width">Width of the visible part of the map (in pixels)</param>
        /// <param name="height">Height of the visible part of the map (in pixels)</param>
        private static void DrawGrid(Graphics graphics, Point origin, float zoom, int width, int height)
        {
            float step = CoordsConvertor.gridSize * zoom;
            Coords firstCoords = CoordsConvertor.CalculateCoords(new Point(0, 0), origin, zoom);
            Point firstPoint = CoordsConvertor.CalculatePoint(firstCoords, origin, zoom);

            Pen pen = new(Color.Gray, 1)
            {
                DashStyle = System.Drawing.Drawing2D.DashStyle.Dash
            };
            Font font = new(SystemFonts.DefaultFont.FontFamily, 10f);
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
    }
}
