using System;

namespace RoadTrafficSimulator.ValueTypes
{
    struct Vector : IEquatable<Vector>
    {
        public readonly Coords from;
        public readonly Coords to;

        public Vector(Coords from, Coords to)
        {
            this.from = from;
            this.to = to;
        }

        public Vector Reverse()
        {
            return new Vector(to, from);
        }

        public (int, int) Diff()
        {
            return (to.x - from.x, to.y - from.y);
        }

        public bool Equals(Vector other) => from.Equals(other.from) && to.Equals(other.to);
    }
}
