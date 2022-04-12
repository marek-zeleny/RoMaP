using System;

using RoadTrafficSimulator.ValueTypes;

namespace RoadTrafficSimulator.Components
{
    /// <summary>
    /// Defines methods for a crossing algorithm that avoids collisions at crossroads.
    /// </summary>
    interface ICrossingAlgorithm
    {
        /// <summary>
        /// Determines whether a given car is allowed to cross from an ingoing road to an outgoing road.
        /// </summary>
        /// <remarks>
        /// This method can be called repeatedly by the same car and is allowed to change its decisions between ticks.
        /// After the car has crossed the crossroad, <see cref="CarCrossed(Car, int, int)"/> must be called.
        /// </remarks>
        /// <param name="fromRoadId">ID of the road from which the car is coming</param>
        /// <param name="toRoadId">ID of the road where the car is headed</param>
        /// <param name="expectedArrival">Amount of time in which the car is expected to arrive at the crossroad</param>
        /// <returns><c>true</c> if the car is allowed to cross when it arrives, otherwise <c>false</c></returns>
        bool CanCross(Car car, int fromRoadId, int toRoadId, Time expectedArrival);

        /// <summary>
        /// Informs the crossing algorithm that a given car has crossed from an ingoing road to an outgoing road.
        /// </summary>
        /// <param name="fromRoadId">ID of the road from which the car came to the crossroad</param>
        /// <param name="toRoadId">ID of the road where the car continues</param>
        void CarCrossed(Car car, int fromRoadId, int toRoadId);

        /// <summary>
        /// Performs a simulation step of a given duration on the crossing algorithm.
        /// </summary>
        void Tick(Time time);
    }

    /// <summary>
    /// Represents a direction between two roads (i.e. not a spacial direction, but in the form [fromRoad -> toRoad]).
    /// </summary>
    public readonly struct Direction : IEquatable<Direction>
    {
        public readonly int fromRoadId, toRoadId;

        public Direction(int fromRoadId, int toRoadId)
        {
            this.fromRoadId = fromRoadId;
            this.toRoadId = toRoadId;
        }

        public override string ToString() => $"Direction: {fromRoadId} -> {toRoadId}";

        public bool Equals(Direction other)
        {
            return fromRoadId == other.fromRoadId && toRoadId == other.toRoadId;
        }

        public override bool Equals(object obj)
        {
            return obj is Direction other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(fromRoadId.GetHashCode(), toRoadId.GetHashCode());
        }

        public static bool operator ==(Direction first, Direction second)
        {
            return first.Equals(second);
        }

        public static bool operator !=(Direction first, Direction second)
        {
            return !(first == second);
        }
    }
}
