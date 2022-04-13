using System;
using System.Collections.Generic;
using System.Drawing;

using RoadTrafficSimulator.ValueTypes;

namespace RoadTrafficSimulator.GUI
{
    /// <summary>
    /// Defines methods for a graphical representation of a map.
    /// </summary>
    interface IGMap
    {
        /// <summary>
        /// Side of the road on which the cars drive (in case of two-way roads)
        /// </summary>
        /// <remarks>
        /// This property might also influence some crossroad-crossing algorithms like priority crossing.
        /// </remarks>
        public RoadSide SideOfDriving { get; set; }

        /// <summary>
        /// Adds a given crossroad to the map.
        /// </summary>
        /// <returns><c>true</c> if the crossroad was successfully added, otherwise <c>false</c></returns>
        bool AddCrossroad(IGCrossroad crossroad);

        /// <summary>
        /// Adds a segment of a given road at a given vector to the map.
        /// </summary>
        /// <returns><c>true</c> if the segment was successfully added, otherwise <c>false</c></returns>
        bool AddRoadSegment(IGRoad road, Vector vector);

        /// <summary>
        /// Removes a crossroad at given coordinates from the map.
        /// </summary>
        /// <returns><c>true</c> if the crossroad was successfully removed, otherwise <c>false</c></returns>
        bool RemoveCrossroad(Coords coords);

        /// <summary>
        /// Removes all road segments of a given road from the map.
        /// </summary>
        /// <returns><c>true</c> if the road was successfully removed, otherwise <c>false</c></returns>
        bool RemoveRoad(IGRoad road);

        /// <summary>
        /// Removes all road segments of a road going through a given vector from the map.
        /// </summary>
        /// <returns><c>true</c> if the road was found and successfully removed, otherwise <c>false</c></returns>
        bool RemoveRoadAt(Vector vector);

        /// <summary>
        /// Gets a crossroad at given coordinates.
        /// </summary>
        /// <returns>Crossroad residing at the coordinates if it exists, otherwise <c>null</c></returns>
        IGCrossroad GetCrossroad(Coords coords);

        /// <summary>
        /// Gets a road going through a given vector.
        /// </summary>
        /// <returns>
        /// Road going through the vector oriented in the vector's direction if it exists, otherwise <c>null</c>
        /// </returns>
        IGRoad GetRoad(Vector vector);

        /// <summary>
        /// Gets all crossroads in the map.
        /// </summary>
        /// <returns>Sequence of crossroads in an undefined order</returns>
        IEnumerable<IGCrossroad> GetCrossroads();

        /// <summary>
        /// Gets all roads in the map.
        /// </summary>
        /// <returns>Sequence road in an undefined order (each road is present only once)</returns>
        IEnumerable<IGRoad> GetRoads();

        /// <summary>
        /// Draws the map onto given graphics.
        /// </summary>
        /// <param name="origin">Position of the map's origin</param>
        /// <param name="zoom">Current zoom of the map</param>
        /// <param name="width">Width of the visible part of the map (in pixels)</param>
        /// <param name="height">Height of the visible part of the map (in pixels)</param>
        /// <param name="simulationMode">If <c>true</c>, the map will be drawn in simulation mode</param>
        void Draw(Graphics graphics, Point origin, float zoom, int width, int height, bool simulationMode);
    }

    /// <summary>
    /// Side of the road in the direction of travel
    /// </summary>
    public enum RoadSide { Right, Left };

    /// <summary>
    /// Highlight flags of a graphical element
    /// </summary>
    [Flags]
    enum Highlight : byte
    {
        None = 0,
        Transparent = 1,
        Large = 2,
    }
}
