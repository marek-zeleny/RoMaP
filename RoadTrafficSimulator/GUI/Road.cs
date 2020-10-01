using System;
using System.Collections.Generic;
using System.Drawing;

using RoadTrafficSimulator.ValueTypes;

namespace RoadTrafficSimulator.GUI
{
    class Road : IMutableRoad
    {
        protected static readonly Color color = Color.Blue;

        protected int roadId;

        public virtual bool IsTwoWay { get => false; }
        public Highlight Highlight { protected get; set; }
        public IList<Coords> Route { get; } = new List<Coords>();

        public virtual IEnumerable<int> GetRoadIds()
        {
            yield return roadId;
        }

        public virtual void SetRoadId(int id, IMutableRoad.Direction direction)
        {
            if (direction == IMutableRoad.Direction.Backward)
                throw new ArgumentException(string.Format("This {0} is unidirectional and cannto set ID for {1}", nameof(Road), nameof(IMutableRoad.Direction.Backward)), nameof(direction));
            roadId = id;
        }

        public IEnumerable<Coords> GetRoute()
        {
            return Route;
        }

        public virtual void Draw(Graphics graphics, Point from, Point to, int width)
        {
            Color color = Road.color;
            switch (Highlight)
            {
                case Highlight.Low:
                    color = Color.FromArgb(150, color);
                    break;
                case Highlight.High:
                    width = IncreaseWidth(width);
                    break;
            }
            Pen pen = new Pen(color, width);
            graphics.DrawLine(pen, from, to);
        }

        protected int IncreaseWidth(int width) => width + width / 2;
    }

    class TwoWayRoad : Road
    {
        private int backwardRoadId;

        public override bool IsTwoWay { get => true; }

        public override IEnumerable<int> GetRoadIds()
        {
            yield return roadId;
            yield return backwardRoadId;
        }

        public override void SetRoadId(int id, IMutableRoad.Direction direction)
        {
            switch (direction)
            {
                case IMutableRoad.Direction.Forward:
                    roadId = id;
                    break;
                case IMutableRoad.Direction.Backward:
                    backwardRoadId = id;
                    break;
            }
        }

        public override void Draw(Graphics graphics, Point from, Point to, int width)
        {
            Point from1 = from, from2 = from;
            Point to1 = to, to2 = to;
            int distance = width * 3 / 4;
            if (Highlight == Highlight.High)
                distance = IncreaseWidth(width) * 3 / 4;
            int diffX = to.X - from.X;
            int diffY = to.Y - from.Y;
            if (diffX == 0)
            {
                if (diffY == 0)
                    throw new ArgumentException(string.Format("The given {0}s cannot be identical.", nameof(Point)));
                else if (diffY < 0)
                    distance = -distance;
                from1.Offset(distance, 0);
                to1.Offset(distance, 0);
                from2.Offset(-distance, 0);
                to2.Offset(-distance, 0);
            }
            else
            {
                if (diffY != 0)
                    throw new ArgumentException(string.Format("The given {0}s must be horizontally or vertically aligned.", nameof(Point)));
                else if (diffX < 0)
                    distance = -distance;
                from1.Offset(0, distance);
                to1.Offset(0, distance);
                from2.Offset(0, -distance);
                to2.Offset(0, -distance);
            }
            base.Draw(graphics, from1, to1, width);
            base.Draw(graphics, from2, to2, width);
        }
    }
}
