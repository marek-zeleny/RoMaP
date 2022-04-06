using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

using RoadTrafficSimulator.ValueTypes;

namespace RoadTrafficSimulator
{
    static class CoordsConvertor
    {
        // Must be ordered in the counter-clockwise direction
        public enum Direction
        {
            Right,
            Up,
            Left,
            Down,
        }

        // Must be in the same order as Direction enum values
        private static readonly Coords[] allowedDirections = new Coords[]
        {
                new Coords(1, 0),
                new Coords(0, -1),
                new Coords(-1, 0),
                new Coords(0, 1),
        };

        public static bool IsAllowedDirection(Coords coords)
        {
            return Array.Exists(allowedDirections, coords.Equals);
        }

        public static Coords GetCoords(Direction dir)
        {
            return allowedDirections[(int)dir];
        }

        public static Direction? GetDirection(Coords coords)
        {
            int index = Array.IndexOf(allowedDirections, coords);
            if (index >= 0)
                return (Direction)index;
            else
                return null;
        }

        /// <summary>
        /// Gets allowed directions in the correct order according to <see cref="roadSide"/> starting from the
        /// <paramref name="i"/>th index.
        /// </summary>
        public static IEnumerable<Coords> GetAllowedDirections(int i = 0)
        {
            yield return allowedDirections[i];
            int m = allowedDirections.Length;
            switch (MapManager.roadSide)
            {
                case MapManager.RoadSide.Right:
                    for (int j = (i + 1) % m; j != i; j = (j + 1) % m)
                        yield return allowedDirections[j];
                    break;
                case MapManager.RoadSide.Left:
                    for (int j = (i - 1 + m) % m; j != i; j = (j - 1 + m) % m)
                        yield return allowedDirections[j];
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets allowed directions in the correct order according to <see cref="roadSide"/> starting from
        /// <paramref name="startDirection"/>.
        /// </summary>
        public static IEnumerable<Coords> GetAllowedDirections(Coords startDirection)
        {
            int index = Array.IndexOf(allowedDirections, startDirection);
            if (index >= 0)
                return GetAllowedDirections(index);
            else
                throw new ArgumentException("The given start direction isn't allowed.", nameof(startDirection));
        }

        /// <summary>
        /// Gets allowed directions in the correct order according to <see cref="roadSide"/> starting from
        /// <paramref name="startDirection"/>.
        /// </summary>
        public static IEnumerable<Coords> GetAllowedDirections(Direction startDirection)
        {
            return GetAllowedDirections((int)startDirection);
        }

        public static int DotProduct(Point p1, Point p2)
        {
            return p1.X * p2.X + p1.Y * p2.Y;
        }

        public static Point Normal(Point point)
        {
            return MapManager.roadSide switch
            {
                MapManager.RoadSide.Right => new Point(-point.Y, point.X),
                MapManager.RoadSide.Left => new Point(point.Y, -point.X),
                _ => throw new NotImplementedException(),
            };
        }

        public static Point CalculatePoint(Coords coords, Point origin, float zoom)
        {
            Point output = new()
            {
                X = origin.X + (int)(coords.x * MapManager.gridSize * zoom),
                Y = origin.Y + (int)(coords.y * MapManager.gridSize * zoom)
            };
            return output;
        }

        public static Coords CalculateCoords(Point point, Point origin, float zoom)
        {
            int x = (int)Math.Round((point.X - origin.X) / (MapManager.gridSize * zoom));
            int y = (int)Math.Round((point.Y - origin.Y) / (MapManager.gridSize * zoom));
            return new Coords(x, y);
        }

        public static Vector CalculateVector(Point point, Point origin, float zoom)
        {
            Coords from = CalculateCoords(point, origin, zoom);
            Coords to;
            Point fromPoint = CalculatePoint(from, origin, zoom);
            int dx = point.X - fromPoint.X;
            int dy = point.Y - fromPoint.Y;
            int sx = Math.Sign(dx);
            int sy = Math.Sign(dy);
            bool horizontal = Math.Abs(dx) > Math.Abs(dy);
            if (horizontal)
                to = new Coords(from.x + sx, from.y);
            else
                to = new Coords(from.x, from.y + sy);
            return new Vector(from, to);
        }

        public static bool IsCorrectDirection(Vector vector, Point point, Point origin, float zoom)
        {
            // Returns true if the vector is correctly directed
            Point centre = CalculatePoint(vector.from, origin, zoom);
            Point to = CalculatePoint(vector.to, origin, zoom);
            to.Offset(-centre.X, -centre.Y);
            point.Offset(-centre.X, -centre.Y);
            return DotProduct(Normal(to), point) > 0;
        }

        public static bool IsCorrectDirection(Vector vector1, Vector vector2, Point point, Point origin, float zoom)
        {
            // Returns true if the correct direction is vector1 and false if it's vector2
            Debug.Assert(vector1.from == vector2.from);
            Point centre = CalculatePoint(vector1.from, origin, zoom);
            Point p1 = CalculatePoint(vector1.to, origin, zoom);
            Point p2 = CalculatePoint(vector2.to, origin, zoom);
            p1.Offset(-centre.X, -centre.Y);
            p2.Offset(-centre.X, -centre.Y);
            point.Offset(-centre.X, -centre.Y);
            Point n1 = Normal(p1);
            Point n2 = Normal(p2);
            bool switched = DotProduct(p1, n2) < 0;
            if (switched)
                (n1, n2) = (n2, n1);
            bool before1 = DotProduct(point, n1) > 0;
            bool after2 = DotProduct(point, n2) < 0;
            return (before1 || after2) ^ switched;
        }
    }
}
