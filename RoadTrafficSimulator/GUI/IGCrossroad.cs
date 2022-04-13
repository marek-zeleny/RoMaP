using System;
using System.Drawing;

using RoadTrafficSimulator.ValueTypes;

namespace RoadTrafficSimulator.GUI
{
    /// <summary>
    /// Defines methods for a graphical representation of a crossroad.
    /// </summary>
    interface IGCrossroad
    {
        /// <summary>
        /// ID of the underlying crossroad
        /// </summary>
        Coords CrossroadId { get; }
        /// <summary>
        /// Directions of a main road on the crossroad; if there is no main road, the value is not set
        /// </summary>
        (CoordsConvertor.Direction, CoordsConvertor.Direction)? MainRoadDirections { get; set; }

        /// <summary>
        /// Checks if a given direction is a main road direction on the crossroad.
        /// </summary>
        /// <returns><c>true</c> if the given direction belongs to a main road, otherwise <c>false</c></returns>
        bool IsMainRoadDirection(CoordsConvertor.Direction direction)
        {
            if (!MainRoadDirections.HasValue)
                return false;
            return MainRoadDirections.Value.Item1 == direction || MainRoadDirections.Value.Item2 == direction;
        }

        /// <summary>
        /// Checks if a given coordinate-represented direction is a main road direction on the crossroad.
        /// </summary>
        /// <returns><c>true</c> if the given direction belongs to a main road, otherwise <c>false</c></returns>
        bool IsMainRoadDirection(Coords coords)
        {
            if (!MainRoadDirections.HasValue)
                return false;
            return CoordsConvertor.AreEqual(coords, MainRoadDirections.Value.Item1)
                || CoordsConvertor.AreEqual(coords, MainRoadDirections.Value.Item2);
        }

        /// <summary>
        /// Resets the crossroad's highlight to a given value (i.e. overrides the previous value).
        /// </summary>
        void ResetHighlight(Highlight highlight);

        /// <summary>
        /// Sets a given highlight flag on the crossroad.
        /// </summary>
        void SetHighlight(Highlight highlight);

        /// <summary>
        /// Unsets a given highlight flag on the crossroad.
        /// </summary>
        void UnsetHighlight(Highlight highlight);

        /// <summary>
        /// Draws the crossroad onto given graphics.
        /// </summary>
        /// <param name="origin">Position of the map's origin</param>
        /// <param name="zoom">Current zoom of the map</param>
        /// <param name="isVisible">Predicate for checking if a given point is currently visible in the GUI</param>
        void Draw(Graphics graphics, Point origin, float zoom, Func<Point, bool> isVisible);
    }
}
