using System;
using System.Drawing;

using RoadTrafficSimulator.ValueTypes;

namespace RoadTrafficSimulator.GUI
{
    interface IGCrossroad
    {
        Coords CrossroadId { get; }
        (CoordsConvertor.Direction, CoordsConvertor.Direction)? MainRoadDirections { get; set; }
        bool IsMainRoadDirection(CoordsConvertor.Direction direction)
        {
            if (!MainRoadDirections.HasValue)
                return false;
            return MainRoadDirections.Value.Item1 == direction || MainRoadDirections.Value.Item2 == direction;
        }
        bool IsMainRoadDirection(Coords coords)
        {
            if (!MainRoadDirections.HasValue)
                return false;
            return CoordsConvertor.AreEqual(coords, MainRoadDirections.Value.Item1)
                || CoordsConvertor.AreEqual(coords, MainRoadDirections.Value.Item2);
        }
        void ResetHighlight(Highlight highlight);
        void SetHighlight(Highlight highlight);
        void UnsetHighlight(Highlight highlight);
        void Draw(Graphics graphics, Point origin, float zoom, Func<Point, bool> isVisible);
    }
}
