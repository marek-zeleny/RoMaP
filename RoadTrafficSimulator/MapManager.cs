using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using RoadTrafficSimulator.Components;
using RoadTrafficSimulator.GUI;
using RoadTrafficSimulator.ValueTypes;

namespace RoadTrafficSimulator
{
    class MapManager
    {
        #region static

        private static readonly Meters roadSegmentLength = 100.Meters();
        private const int K = 100;

        private static readonly Coords[] directions = new Coords[]
        {
                new Coords(1, 0),
                new Coords(-1, 0),
                new Coords(0, 1),
                new Coords(0, -1)
        };

        public static Point CalculatePoint(Coords coords, Point origin, decimal zoom)
        {
            Point output = new Point
            {
                X = origin.X + (int)(coords.x * K * zoom),
                Y = origin.Y + (int)(coords.y * K * zoom)
            };
            return output;
        }

        public static Coords CalculateCoords(Point point, Point origin, decimal zoom)
        {
            int x = (int)Math.Round((point.X - origin.X) / (K * zoom));
            int y = (int)Math.Round((point.Y - origin.Y) / (K * zoom));
            return new Coords(x, y);
        }

        public static Vector CalculateVector(Point point, Point origin, decimal zoom)
        {
            Coords from = CalculateCoords(point, origin, zoom);
            Coords to;
            Point fromPoint = CalculatePoint(from, origin, zoom);
            int dx = point.X - fromPoint.X;
            int dy = point.Y - fromPoint.Y;
            if (Math.Abs(dx) < Math.Abs(dy))
                to = new Coords(from.x, from.y + Math.Sign(dy));
            else
                to = new Coords(from.x + Math.Sign(dx), from.y);
            return new Vector(from, to);
        }

        #endregion static

        private Components.Map map;
        private IMap guiMap = new GUI.Map();

        public MapManager(Components.Map map)
        {
            this.map = map;
        }

        public CrossroadView GetCrossroad(Coords coords)
        {
            ICrossroad guiCrossroad = guiMap.GetCrossroad(coords);
            if (guiCrossroad == null)
                return null;
            Components.Crossroad crossroad = (Components.Crossroad)map.GetNode(coords);
            return new CrossroadView(crossroad, guiCrossroad);
        }

        public RoadView GetRoad(Vector vector)
        {
            IRoad guiRoad = guiMap.GetRoad(vector, true);
            if (guiRoad == null)
                return null;
            Components.Road road = (Components.Road)map.GetEdge(guiRoad.GetRoadIds().First());
            return new RoadView(road, guiRoad);
        }

        public IEnumerable<RoadView> GetAllRoads(Coords coords)
        {
            // May return the same road multiple times (twice)
            foreach (Coords diff in directions)
            {
                RoadView road = GetRoad(new Vector(coords, new Coords(coords.x + diff.x, coords.y + diff.y)));
                if (road != null)
                    yield return road;
            }
        }

        public RoadView GetOppositeRoad(RoadView roadView)
        {
            foreach (int id in roadView.GuiRoad.GetRoadIds())
                if (id != roadView.Id)
                    return new RoadView((Components.Road)map.GetEdge(id), roadView.GuiRoad);
            return null;
        }

        public IRoadBuilder GetRoadBuilder(Coords startingCoords, bool twoWayRoad = true)
        {
            if (map.GetNode(startingCoords) == null && !IsRoadFree(startingCoords))
                return null;
            return new RoadBuilder(this, startingCoords, twoWayRoad);
        }

        public bool DestroyCrossroad(CrossroadView crossroadView)
        {
            map.RemoveCrossroad(crossroadView.Coords);
            return DestroyGuiCrossroad(crossroadView.GuiCrossroad);
        }

        public bool DestroyRoad(RoadView roadView)
        {
            map.RemoveRoad(roadView.Id);
            return DestroyGuiRoad(roadView.GuiRoad);
        }

        public void Draw(Graphics graphics, Point origin, decimal zoom, int width, int height)
        {
            DrawGrid(graphics, origin, zoom, width, height);
            guiMap.Draw(graphics, origin, zoom, width, height);
        }

        private bool DestroyGuiCrossroad(ICrossroad crossroad)
        {
            foreach (RoadView road in GetAllRoads(crossroad.CrossroadId))
                if (!DestroyGuiRoad(road.GuiRoad))
                    return false;
            // The crossroad should be destroyed in the previous cycle
            return guiMap.GetCrossroad(crossroad.CrossroadId) == null;
        }

        private bool DestroyGuiRoad(IRoad road)
        {
            var enumerator = road.GetRoute().GetEnumerator();
            enumerator.MoveNext();
            Coords last = enumerator.Current;
            while (enumerator.MoveNext())
            {
                if (!guiMap.RemoveRoad(new Vector(last, enumerator.Current)))
                    return false;
                last = enumerator.Current;
            }
            // Destroy crossroads that became stand-alone after destroying this road
            if (IsRoadFree(road.From))
                if (!guiMap.RemoveCrossroad(road.From))
                    return false;
            if (IsRoadFree(road.To))
                if (!guiMap.RemoveCrossroad(road.To))
                    return false;
            return true;
        }

        private void DrawGrid(Graphics graphics, Point origin, decimal zoom, int width, int height)
        {
            int step = (int)(K * zoom);
            int firstX = origin.X % step;
            int firstY = origin.Y % step;
            Coords firstCoords = CalculateCoords(new Point(firstX, firstY), origin, zoom);
            Pen pen = new Pen(Color.Gray, 1)
            {
                DashStyle = System.Drawing.Drawing2D.DashStyle.Dash
            };
            Font font = new Font(SystemFonts.DefaultFont.FontFamily, 10f);
            Brush brush = Brushes.DarkOrange;
            int xCoord = firstCoords.x;
            for (int x = firstX; x < width; x += step)
            {
                graphics.DrawLine(pen, x, 0, x, height);
                graphics.DrawString(string.Format("[{0}]", xCoord++), font, brush, x + 5, 5);
            }
            int yCoord = firstCoords.y;
            for (int y = firstY; y < height; y += step)
            {
                graphics.DrawLine(pen, 0, y, width, y);
                graphics.DrawString(string.Format("[{0}]", yCoord++), font, brush, 5, y + 5);
            }
        }

        private bool IsRoadFree(Coords coords)
        {
            return !GetAllRoads(coords).Any();
        }

        private class RoadBuilder : IRoadBuilder
        {
            private MapManager manager;
            private IMutableRoad road;

            private IList<Coords> Route { get => road.Route; }

            public bool CanContinue { get; private set; }

            public RoadBuilder(MapManager manager, Coords startingCoords, bool twoWayRoad)
            {
                this.manager = manager;
                if (twoWayRoad)
                    road = new TwoWayRoad();
                else
                    road = new GUI.Road();
                road.Route.Add(startingCoords);
                road.Highlight = Highlight.High;
                CanContinue = true;
                TryGetOrAddCrossroad(startingCoords).Highlight = Highlight.High;
            }

            public bool AddSegment(Coords nextCoords)
            {
                if (!CanContinue)
                    return false;
                if (!CanEnterCoords(nextCoords))
                    return false;
                Coords lastCoords = Route[Route.Count - 1];
                Vector vector = new Vector(lastCoords, nextCoords);
                (int dx, int dy) = vector.Diff();
                if (Math.Abs(dx) + Math.Abs(dy) != 1)
                    return false;
                if (!manager.guiMap.AddRoad(road, vector))
                    return false;
                Route.Add(nextCoords);
                CanContinue = manager.map.GetNode(nextCoords) == null;
                return true;
            }

            public bool FinishRoad(MetersPerSecond maxSpeed)
            {
                if (Route.Count < 2)
                    return false;
                Coords from = Route[0];
                Coords to = Route[Route.Count - 1];
                TryGetOrAddCrossroad(from).Highlight = Highlight.Normal;
                TryGetOrAddCrossroad(to).Highlight = Highlight.Normal;
                this.road.Highlight = Highlight.Normal;
                Meters roadLength = (Route.Count - 1) * roadSegmentLength;
                Components.Road road = manager.map.AddRoad(from, to, roadLength, maxSpeed);
                this.road.SetRoadId(road.Id);
                if (this.road.IsTwoWay)
                {
                    Components.Road backRoad = manager.map.AddRoad(to, from, roadLength, maxSpeed);
                    this.road.SetRoadId(backRoad.Id, IMutableRoad.Direction.Backward);
                    // Set TrafficLight to always allow backward direction
                    ((Components.Crossroad)road.ToNode).TrafficLight.AddDefaultDirection(road.Id, backRoad.Id);
                    ((Components.Crossroad)backRoad.ToNode).TrafficLight.AddDefaultDirection(backRoad.Id, road.Id);
                }
                Invalidate();
                return true;
            }

            public void DestroyRoad()
            {
                for (int i = 1; i < Route.Count; i++)
                    manager.guiMap.RemoveRoad(new Vector(Route[i - 1], Route[i]));
                if (manager.map.GetNode(Route[0]) == null)
                    manager.guiMap.RemoveCrossroad(Route[0]);
                else
                    manager.guiMap.GetCrossroad(Route[0]).Highlight = Highlight.Normal;
                Invalidate();
            }

            private ICrossroad TryGetOrAddCrossroad(Coords coords)
            {
                ICrossroad output = manager.guiMap.GetCrossroad(coords);
                if (output != null)
                    return output;
                output = new GUI.Crossroad(coords);
                manager.guiMap.AddCrossroad(output, coords);
                return output;
            }

            private bool CanEnterCoords(Coords newCoords)
            {
                foreach (Coords c in Route)
                    if (c.Equals(newCoords))
                        return false;
                // Always allow if there is a (different from the starting one) crossroad at newCoords
                if (manager.map.GetNode(newCoords) != null)
                    return true;
                return manager.IsRoadFree(newCoords);
            }

            private void Invalidate()
            {
                manager = null;
                road = null;
            }
        }
    }
}
