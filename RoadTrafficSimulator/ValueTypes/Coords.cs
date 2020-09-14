using System;

namespace RoadTrafficSimulator.ValueTypes
{
    struct Coords : IEquatable<Coords>
    {
        public readonly int x;
        public readonly int y;

        public Coords(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public bool Equals(Coords other) => x == other.x && y == other.y;
    }
}