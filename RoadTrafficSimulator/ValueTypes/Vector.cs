using System;

namespace RoadTrafficSimulator.ValueTypes
{
    struct Vector : IEquatable<Vector>
    {
        public readonly Coords from, to;

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

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj is Vector v)
                return Equals(v);
            return false;
        }

        public override string ToString() => string.Format("<{0},{1}>", from, to);

        public static bool operator ==(Vector first, Vector second) => first.Equals(second);

        public static bool operator !=(Vector first, Vector second) => !(first == second);
    }
}
