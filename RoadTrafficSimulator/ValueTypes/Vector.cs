using System;

namespace RoadTrafficSimulator.ValueTypes
{
    /// <summary>
    /// Represents a vector between two pairs of coordinates.
    /// </summary>
    readonly struct Vector : IEquatable<Vector>
    {
        public readonly Coords from, to;

        public Vector(Coords from, Coords to)
        {
            this.from = from;
            this.to = to;
        }

        /// <summary>
        /// Obtains a vector of the opposite direction.
        /// </summary>
        /// <returns></returns>
        public Vector Reverse()
        {
            return new Vector(to, from);
        }

        /// <summary>
        /// Obtains the difference of coordinates forming the vector (i.e. shifts the vector's origin to (0;0)).
        /// </summary>
        public Coords Diff()
        {
            return to - from;
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

        public override int GetHashCode()
        {
            return HashCode.Combine(from.GetHashCode(), to.GetHashCode());
        }

        public override string ToString() => $"<{from},{to}>";

        public static bool operator ==(Vector first, Vector second) => first.Equals(second);

        public static bool operator !=(Vector first, Vector second) => !(first == second);
    }
}
