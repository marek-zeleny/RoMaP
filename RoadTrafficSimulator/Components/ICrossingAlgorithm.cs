using System;

namespace RoadTrafficSimulator.Components
{
    interface ICrossingAlgorithm
    {
        bool CanCross(int fromRoadId, int toRoadId);
    }

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
