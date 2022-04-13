using System;
using System.Collections.Generic;
using System.Drawing;

using RoadTrafficSimulator.ValueTypes;
using RoadTrafficSimulator.Components;

namespace RoadTrafficSimulator.GUI
{
    /// <summary>
    /// Defines methods for a graphical representation of a road.
    /// </summary>
    /// <remarks>
    /// A GUI road can correspond to up to two back-end roads, depending on it being one- or two-way.
    /// </remarks>
    interface IGRoad
    {
        public enum Direction { Forward, Backward }

        /// <summary>
        /// Beginning of the road
        /// </summary>
        Coords From { get; }
        /// <summary>
        /// End of the road
        /// </summary>
        Coords To { get; }
        /// <summary>
        /// <c>true</c> if the road is two-way, <c>false</c> if it's one-way
        /// </summary>
        bool IsTwoWay { get; }

        /// <summary>
        /// Gets an underlying back-end road going in a given direction.
        /// </summary>
        /// <returns>A back-end road if it exists in this direction, otherwise <c>null</c></returns>
        Road GetRoad(Direction direction = Direction.Forward);

        /// <summary>
        /// Gets all underlying back-end roads.
        /// </summary>
        /// <returns>Sequence of roads in an undefined order</returns>
        IEnumerable<Road> GetRoads();

        /// <summary>
        /// Gets the road's route through the spacial map representation.
        /// </summary>
        /// <returns>Sequence of coordinates ordered according to the given direction</returns>
        IEnumerable<Coords> GetRoute(Direction direction = Direction.Forward);

        /// <summary>
        /// Gets a representation of the same GUI road in the opposite direction.
        /// </summary>
        IGRoad GetReversedGRoad();

        /// <summary>
        /// Resets the road's highlight in both directions to a given value (i.e. overrides the previous values).
        /// </summary>
        void ResetHighlight(Highlight highlight);

        /// <summary>
        /// Resets the road's highlight in a given direction to a given value (i.e. overrides the previous value).
        /// </summary>
        void ResetHighlight(Highlight highlight, Direction direction);

        /// <summary>
        /// Sets a given highlight flag on the road in a given direction.
        /// </summary>
        void SetHighlight(Highlight highlight, Direction direction);

        /// <summary>
        /// Unsets a given highlight flag on the road in a given direction.
        /// </summary>
        void UnsetHighlight(Highlight highlight, Direction direction);

        /// <summary>
        /// Draws the road onto given graphics.
        /// </summary>
        /// <param name="origin">Position of the map's origin</param>
        /// <param name="zoom">Current zoom of the map</param>
        /// <param name="sideOfDriving">Side on which the cars drive</param>
        /// <param name="simulationMode">If <c>true</c>, the road will be drawn in simulation mode</param>
        /// <param name="isVisible">Predicate for checking if a given point is currently visible in the GUI</param>
        void Draw(Graphics graphics, Point origin, float zoom, RoadSide sideOfDriving, bool simulationMode,
            Func<Point, bool> isVisible);
    }

    /// <summary>
    /// Defines methods for changing the structure of a road's graphical representation.
    /// </summary>
    interface IMutableGRoad : IGRoad
    {
        /// <summary>
        /// The road's route as a collection of coordinates
        /// </summary>
        ICollection<Coords> Route { get; }

        /// <summary>
        /// Sets a given back-end road as an underlying road in a given direction.
        /// </summary>
        void SetRoad(Road road, Direction direction);
    }
}
