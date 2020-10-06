using System;

namespace RoadTrafficSimulator.ValueTypes
{
    struct Coords : IEquatable<Coords>
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

        public override string ToString() => string.Format("({0};{1})", x, y);

        public static bool operator ==(Coords first, Coords second) => first.Equals(second);

        public static bool operator !=(Coords first, Coords second) => !(first == second);
    }
}