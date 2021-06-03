using System;

namespace RoadTrafficSimulator.ValueTypes
{
    readonly struct Coords : IEquatable<Coords>
    {
        public readonly int x, y;

        public Coords(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public bool Equals(Coords other) => x == other.x && y == other.y;

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj is Coords c)
                return Equals(c);
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(x.GetHashCode(), y.GetHashCode());
        }

        public override string ToString() => $"({x};{y})";

        public static bool operator ==(Coords first, Coords second) => first.Equals(second);

        public static bool operator !=(Coords first, Coords second) => !(first == second);

        public static Coords operator +(Coords first, Coords second) => new(first.x + second.x, first.y + second.y);

        public static Coords operator -(Coords first, Coords second) => new(first.x - second.x, first.y - second.y);
    }
}