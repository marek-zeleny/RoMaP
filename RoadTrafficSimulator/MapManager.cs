using System;
using System.Collections.Generic;
using System.Drawing;

using RoadTrafficSimulator.Components;
using RoadTrafficSimulator.GUI;
using RoadTrafficSimulator.ValueTypes;

namespace RoadTrafficSimulator
{
    class MapManager
    {
        private static readonly Meters roadSegmentLength = 100.Meters();

        private static readonly Coords[] directions = new Coords[]
        {
                new Coords(1, 0),
                new Coords(-1, 0),
                new Coords(0, 1),
                new Coords(0, -1)
        };

        public static Point CalculatePoint(Coords coords, Coords origin, decimal zoom)
        {
            throw new NotImplementedException(); // TODO
        }

        public static Coords CalculateCoords(Point point, Coords origin, decimal zoom)
        {
            throw new NotImplementedException(); // TODO
        }

        private Components.Map map;
        private IMap guiMap = new GUI.Map();
        private GuiSettings settings;

        public MapManager(Components.Map map)
        {
            this.map = map;
        }

        public void Draw(Graphics graphics, int width, int height)
        {
            DrawGrid(graphics, width, height);
            guiMap.Draw(graphics, settings.origin, settings.zoom, width, height);
        }

        public IRoadBuilder GetRoadBuilder(Coords startingCoords, bool twoWayRoad = true)
        {
            if (map.GetNode(startingCoords) == null && !IsFree(startingCoords))
                return null;
            return new RoadBuilder(this, startingCoords, twoWayRoad);
        }

        private void DrawGrid(Graphics graphics, int width, int height)
        {
            throw new NotImplementedException(); // TODO
        }

        private bool IsFree(Coords coords)
        {
            foreach (Coords diff in directions)
                if (guiMap.GetRoadSegment(new Vector(coords, new Coords(coords.x + diff.x, coords.y + diff.y)), true) != null)
                    return false;
            return true;
        }

        private struct GuiSettings
        {
            public Coords origin;
            public decimal zoom;
        }

        private class RoadBuilder : IRoadBuilder
        {
            private MapManager manager;
            private IRoadSegment roadSegment;
            private List<Coords> coords;

            public bool CanContinue { get; private set; } = true;

            public RoadBuilder(MapManager manager, Coords startingCoords, bool twoWayRoad)
            {
                this.manager = manager;
                if (twoWayRoad)
                    roadSegment = new TwoWayRoadSegment();
                else
                    roadSegment = new RoadSegment();
                roadSegment.Highlight = Highlight.High;
                coords = new List<Coords> { startingCoords };
                TryGetOrAddCrossroad(startingCoords).Highlight = Highlight.High;
            }

            public bool AddSegment(Coords nextCoords)
            {
                if (!CanContinue)
                    return false;
                if (!CanEnterCoords(nextCoords))
                    return false;
                Coords lastCoords = coords[coords.Count - 1];
                Vector vector = new Vector(lastCoords, nextCoords);
                (int dx, int dy) = vector.Diff();
                if (Math.Abs(dx) + Math.Abs(dy) != 1)
                    return false;
                if (!manager.guiMap.AddRoadSegment(roadSegment, vector))
                    return false;
                coords.Add(nextCoords);
                CanContinue = manager.map.GetNode(nextCoords) == null;
                return true;
            }

            public bool FinishRoad(MetersPerSecond maxSpeed)
            {
                if (coords.Count < 2)
                    return false;
                Coords from = coords[0];
                Coords to = coords[coords.Count - 1];
                manager.guiMap.GetCrossroad(from).Highlight = Highlight.Normal;
                TryGetOrAddCrossroad(to).Highlight = Highlight.Normal;
                roadSegment.Highlight = Highlight.Normal;
                Meters roadLength = (coords.Count - 1) * roadSegmentLength;
                Road road = manager.map.AddRoad(from, to, roadLength, maxSpeed);
                ((RoadSegment)roadSegment).RoadId = road.Id;
                if (roadSegment is TwoWayRoadSegment s)
                {
                    road = manager.map.AddRoad(to, from, roadLength, maxSpeed);
                    s.BackwardRoadId = road.Id;
                }
                Invalidate();
                return true;
            }

            public void DestroyRoad()
            {
                for (int i = 1; i < coords.Count; i++)
                    manager.guiMap.RemoveRoadSegment(new Vector(coords[i - 1], coords[i]));
                if (manager.map.GetNode(coords[0]) == null)
                    manager.guiMap.RemoveCrossroad(coords[0]);
                else
                    manager.guiMap.GetCrossroad(coords[0]).Highlight = Highlight.Normal;
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
                foreach (Coords c in coords)
                    if (c.Equals(newCoords))
                        return false;
                if (manager.map.GetNode(newCoords) == null)
                    return true;
                return manager.IsFree(newCoords);
            }

            private void Invalidate()
            {
                manager = null;
                roadSegment = null;
                coords = null;
            }
        }
    }
}
