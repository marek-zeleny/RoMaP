using System;
using System.Collections.Generic;
using System.Drawing;

namespace RoadTrafficSimulator.GUI
{
    class Road : IRoad
    {
        protected static readonly Color color = Color.Blue;

        public int RoadId { get; set; }

        public Highlight Highlight { protected get; set; }

        public virtual IEnumerable<int> GetRoadIds()
        {
            yield return RoadId;
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
        public int BackwardRoadId { get; set; }

        public override IEnumerable<int> GetRoadIds()
        {
            yield return RoadId;
            yield return BackwardRoadId;
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
