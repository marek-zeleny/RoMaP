using System;
using System.Collections.Generic;
using System.Drawing;

using RoadTrafficSimulator.ValueTypes;

namespace RoadTrafficSimulator
{
    /// <summary>
    /// Provides methods for converting between coordinates and graphic pixels.
    /// Also defines allowed directions for road segments in the GUI.
    /// </summary>
    static class CoordsConvertor
    {
        /// <summary>
        /// Directions of road segments in the GUI
        /// </summary>
        /// <remarks>
        /// The directions must be ordered counter-clockwise.
        /// </remarks>
        public enum Direction
        {
            Right = 0,
            Up,
            Left,
            Down,
        }

        /// <summary>
        /// Coordinates representing allowed directions of road segments.
        /// </summary>
        /// <remarks>
        /// The ordering must correspond to the values of <see cref="Direction"/>.
        /// </remarks>
        private static readonly Coords[] allowedDirections = new Coords[]
        {
            new Coords(1, 0),
            new Coords(0, -1),
            new Coords(-1, 0),
            new Coords(0, 1),
        };

        /// <summary>
        /// Checks if given coordinates represent an allowed direction.
        /// </summary>
        public static bool IsAllowedDirection(Coords coords)
        {
            return Array.Exists(allowedDirections, coords.Equals);
        }

        /// <summary>
        /// Checks if given coordinates correspond to a given direction.
        /// </summary>
        public static bool AreEqual(Coords coords, Direction dir)
        {
            return coords == GetCoords(dir);
        }

        /// <summary>
        /// Converts given direction into coordinate representation.
        /// </summary>
        public static Coords GetCoords(Direction dir)
        {
            return allowedDirections[(int)dir];
        }

        /// <summary>
        /// Converts given coordinates into the direction they represent.
        /// </summary>
        /// <returns>
        /// If the coordinates correspond to an allowed direction, returns that direction, otherwise returns no value
        /// </returns>
        public static Direction? GetDirection(Coords coords)
        {
            int index = Array.IndexOf(allowedDirections, coords);
            if (index >= 0)
                return (Direction)index;
            else
                return null;
        }

        /// <summary>
        /// Gets all allowed directions starting from a given index.
        /// </summary>
        /// <remarks>
        /// The directions are ordered according to given side of driving.
        /// For right-side driving, the ordering is counter-clockwise, for left-side driving it's clockwise.
        /// </remarks>
        /// <returns>Sequence of coordinates representing all allowed directions</returns>
        public static IEnumerable<Coords> GetAllowedDirections(GUI.RoadSide sideOfDriving, int i = 0)
        {
            yield return allowedDirections[i];
            int m = allowedDirections.Length;
            switch (sideOfDriving)
            {
                case GUI.RoadSide.Right:
                    for (int j = (i + 1) % m; j != i; j = (j + 1) % m)
                        yield return allowedDirections[j];
                    break;
                case GUI.RoadSide.Left:
                    for (int j = (i - 1 + m) % m; j != i; j = (j - 1 + m) % m)
                        yield return allowedDirections[j];
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets all allowed directions starting from a given direction.
        /// </summary>
        /// <remarks>
        /// The directions are ordered according to given side of driving.
        /// For right-side driving, the ordering is counter-clockwise, for left-side driving it's clockwise.
        /// </remarks>
        /// <returns>Sequence of coordinates representing all allowed directions</returns>
        /// <exception cref="ArgumentException">Start direction is not an allowed direction</exception>
        public static IEnumerable<Coords> GetAllowedDirections(GUI.RoadSide sideOfDriving, Coords startDirection)
        {
            int index = Array.IndexOf(allowedDirections, startDirection);
            if (index >= 0)
                return GetAllowedDirections(sideOfDriving, index);
            else
                throw new ArgumentException("The given start direction isn't allowed.", nameof(startDirection));
        }

        /// <summary>
        /// Gets all allowed directions starting from a given direction.
        /// </summary>
        /// <remarks>
        /// The directions are ordered according to given side of driving.
        /// For right-side driving, the ordering is counter-clockwise, for left-side driving it's clockwise.
        /// </remarks>
        /// <returns>Sequence of coordinates representing all allowed directions</returns>
        public static IEnumerable<Coords> GetAllowedDirections(GUI.RoadSide sideOfDriving, Direction startDirection)
        {
            return GetAllowedDirections(sideOfDriving, (int)startDirection);
        }

        /// <summary>
        /// Computes the dot product of two given points.
        /// </summary>
        public static int DotProduct(Point p1, Point p2)
        {
            return p1.X * p2.X + p1.Y * p2.Y;
        }

        /// <summary>
        /// Computes a normal of a given point.
        /// </summary>
        /// <remarks>
        /// The normal is oriented with respect to given side of driving, i.e. for right-side driving it's oriented
        /// to the right and vice versa.
        /// </remarks>
        /// <returns>Point perpendicular to the given point with the same size</returns>
        public static Point Normal(Point point, GUI.RoadSide sideOfDriving)
        {
            return sideOfDriving switch
            {
                GUI.RoadSide.Right => new Point(-point.Y, point.X),
                GUI.RoadSide.Left => new Point(point.Y, -point.X),
                _ => throw new NotImplementedException(),
            };
        }

        /// <summary>
        /// Calculates a pixel point corresponding to given coordinates.
        /// </summary>
        /// <param name="origin">Pixel position of the origin of the coordinate system (0;0)</param>
        /// <param name="zoom">Zoom ratio of the map's visualisation</param>
        public static Point CalculatePoint(Coords coords, Point origin, float zoom)
        {
            Point output = new()
            {
                X = origin.X + (int)(coords.x * MapManager.gridSize * zoom),
                Y = origin.Y + (int)(coords.y * MapManager.gridSize * zoom)
            };
            return output;
        }

        /// <summary>
        /// Calculates the nearest coordinates with respect to a given pixel position.
        /// </summary>
        /// <param name="origin">Pixel position of the origin of the coordinate system (0;0)</param>
        /// <param name="zoom">Zoom ratio of the map's visualisation</param>
        public static Coords CalculateCoords(Point point, Point origin, float zoom)
        {
            int x = (int)Math.Round((point.X - origin.X) / (MapManager.gridSize * zoom));
            int y = (int)Math.Round((point.Y - origin.Y) / (MapManager.gridSize * zoom));
            return new Coords(x, y);
        }

        /// <summary>
        /// Calculates the nearest vector with respect to a given pixel position.
        /// </summary>
        /// <remarks>
        /// The beginning of the vector is the nearest coordinates to the given point, the end is the second nearest
        /// coordinates. The resulting vector's difference is guaranteed to be an allowed direction for road segments.
        /// </remarks>
        /// <param name="origin">Pixel position of the origin of the coordinate system (0;0)</param>
        /// <param name="zoom">Zoom ratio of the map's visualisation</param>
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

        /// right-side driving example:
        /// 
        ///             |
        ///       +     |
        ///    point 1  |   +
        ///   (correct) |  point 2
        ///             |(incorrect)
        ///             |
        ///             v
        ///           vector
        /// 
        /// <summary>
        /// Checks if a given vector is correctly oriented with respect to a given point and side of driving.
        /// </summary>
        /// <remarks>
        /// This method is intended for determining which side of a two-way road should be selected based on a mouse
        /// click when there is a crossroad at the nearest coordinates.
        /// 
        /// The vector is correctly oriented if the given point is on the side of driving, i.e. for driving on the right
        /// the point is on the right side of the vector and vice versa.
        /// </remarks>
        /// <param name="origin">Pixel position of the origin of the coordinate system (0;0)</param>
        /// <param name="zoom">Zoom ratio of the map's visualisation</param>
        public static bool IsCorrectOrientation(GUI.RoadSide sideOfDriving, Vector vector, Point point,
            Point origin, float zoom)
        {
            Point centre = CalculatePoint(vector.from, origin, zoom);
            Point to = CalculatePoint(vector.to, origin, zoom);
            to.Offset(-centre.X, -centre.Y);
            point.Offset(-centre.X, -centre.Y);
            return DotProduct(Normal(to, sideOfDriving), point) > 0;
        }

        /// right-side driving example:
        /// 
        ///      +      .
        ///   point 1  /|
        /// (vector 1 / |   +
        /// is correct) | point 2 (vector 1 is correct)
        ///         / + |
        ///        / point 3 (vector 2 is correct)
        ///       L     |
        /// vector 1    V
        ///            vector2
        /// 
        /// <summary>
        /// Given two vectors originating at the same coordinates, checks which of the vectors is correctly oriented
        /// with respect to a given point and side of driving.
        /// </summary>
        /// <remarks>
        /// This method is intended for determining which side of a two-way road should be selected based on a mouse
        /// click when there is no crossroad at the nearest coordinates (i.e. a road is passing through).
        /// 
        /// The correct vector for right-side driving is the first one encountered when rotating the point
        /// counter-clockwise around the vectors' shared origin; for left-side driving, the rotation is clockwise.
        /// </remarks>
        /// <param name="origin">Pixel position of the origin of the coordinate system (0;0)</param>
        /// <param name="zoom">Zoom ratio of the map's visualisation</param>
        /// <returns><c>true</c> if the correctly oriented vector is vector 1, <c>false</c> for vector 2</returns>
        /// <exception cref="ArgumentException">Vectors don't originate at the same coordinates</exception>
        public static bool ChooseCorrectOrientation(GUI.RoadSide sideOfDriving, Vector vector1, Vector vector2,
            Point point, Point origin, float zoom)
        {
            if (vector1.from != vector2.from)
                throw new ArgumentException("The vectors don't originate at the same coordinates.", nameof(vector2));
            Point centre = CalculatePoint(vector1.from, origin, zoom);
            Point p1 = CalculatePoint(vector1.to, origin, zoom);
            Point p2 = CalculatePoint(vector2.to, origin, zoom);
            p1.Offset(-centre.X, -centre.Y);
            p2.Offset(-centre.X, -centre.Y);
            point.Offset(-centre.X, -centre.Y);
            Point n1 = Normal(p1, sideOfDriving);
            Point n2 = Normal(p2, sideOfDriving);
            bool switched = DotProduct(p1, n2) < 0;
            if (switched)
                (n1, n2) = (n2, n1);
            bool before1 = DotProduct(point, n1) > 0;
            bool after2 = DotProduct(point, n2) < 0;
            return (before1 || after2) ^ switched;
        }
    }
}
