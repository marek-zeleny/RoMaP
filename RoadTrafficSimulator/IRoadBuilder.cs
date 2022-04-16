using System;

using RoadTrafficSimulator.ValueTypes;
using RoadTrafficSimulator.GUI;

namespace RoadTrafficSimulator
{
    /// <summary>
    /// Defines methods for building a new road.
    /// </summary>
    interface IRoadBuilder
    {
        /// <summary>
        /// <c>false</c> if the road can't continue any further and must be finished or destroyed, otherwise <c>true</c>
        /// </summary>
        bool CanContinue { get; }

        /// <summary>
        /// Prolongs the road being built by a segment going from the current end of the road to given coordinates, if
        /// such road segment is allowed.
        /// </summary>
        /// <returns><c>true</c> if the segment was successfully appended to the road, otherwise <c>false</c></returns>
        bool AddSegment(Coords nextCoords);

        /// <summary>
        /// Finishes building the road by creating the corresponding back-end infrastructure (roads and crossroads).
        /// </summary>
        /// <param name="twoWayRoad">
        /// If <c>true</c>, the created road will be two-way, otherwise a one-way road in the building direction will be
        /// created
        /// </param>
        /// <param name="updatePriorityCrossing">
        /// If <c>true</c>, priority crossing will be updated at the starting and ending crossroad
        /// </param>
        /// <returns><c>true</c> if the road was successfully built, otherwise <c>false</c></returns>
        bool FinishRoad(bool twoWayRoad, bool updatePriorityCrossing = true);

        /// <inheritdoc cref="FinishRoad(bool, bool)"/>
        /// <param name="maxSpeed">Initial maximum speed on the created road</param>
        bool FinishRoad(bool twoWayRoad, Speed maxSpeed, bool updatePriorityCrossing = true);

        /// <inheritdoc cref="FinishRoad(bool, bool)"/>
        /// <param name="builtRoad">
        /// Output parameter set to the built GUI road; if the operation was unsuccessful, the value is undefined
        /// </param>
        bool FinishRoad(bool twoWayRoad, out IGRoad builtRoad, bool updatePriorityCrossing = true);

        /// <inheritdoc cref="FinishRoad(bool, Speed, bool)"/>
        /// <inheritdoc cref="FinishRoad(bool, out IGRoad, bool)"/>
        bool FinishRoad(bool twoWayRoad, Speed maxSpeed, out IGRoad builtRoad, bool updatePriorityCrossing = true);

        /// <summary>
        /// Destroys the road being built.
        /// </summary>
        void DestroyRoad();
    }
}
