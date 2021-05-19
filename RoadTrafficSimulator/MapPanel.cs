using System;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;

namespace RoadTrafficSimulator
{
    class MapPanel : Panel
    {
        private const float minZoom = 0.2f;
        private const float maxZoom = 5f;

        private Point origin; // Do not access directly, use the Origin property
        private float zoom; // Do not access directly, use the Zoom property
        private Point prevMouseLocation;
        private bool drag;
        private bool dragOccured;

        public Point Center { get => new Point(Width / 2, Height / 2); }
        public Point Origin
        {
            get => origin;
            private set
            {
                origin = value;
                Invalidate();
                OriginChanged?.Invoke(this, new EventArgs());
            }
        }
        public float Zoom
        {
            get => zoom;
            set
            {
                if (value < minZoom)
                    zoom = minZoom;
                else if (value > maxZoom)
                    zoom = maxZoom;
                else
                    zoom = value;
                Invalidate();
                ZoomChanged?.Invoke(this, new EventArgs());
            }
        }

        [Browsable(true)]
        [Category("PropertyChanged")]
        [Description("Occurs when the map origin moves.")]
        public event EventHandler OriginChanged;

        [Browsable(true)]
        [Category("PropertyChanged")]
        [Description("Occurs when map zoom changes.")]
        public event EventHandler ZoomChanged;

        public MapPanel()
        {
            // Enable double-buffering
            typeof(Panel).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.SetProperty
                | System.Reflection.BindingFlags.Instance
                | System.Reflection.BindingFlags.NonPublic,
                null,
                this,
                new object[] { true });
            // Custom properties
            Origin = Center;
            Zoom = 1f;
            // Panel properties
            Cursor = Cursors.Cross;
        }

        public void ResetOrigin()
        {
            Origin = Center;
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (dragOccured)
                return;
            base.OnMouseClick(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                drag = true;
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                drag = false;
                dragOccured = false;
            }
            base.OnMouseUp(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (drag)
            {
                Point offset = new Point(e.Location.X - prevMouseLocation.X, e.Location.Y - prevMouseLocation.Y);
                Origin.Offset(offset);
                dragOccured = true;
            }
            prevMouseLocation = e.Location;
            base.OnMouseMove(e);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            const float coeff = 1.2f;
            // For centring the zoom at cursor position
            float prevZoom = Zoom;
            Point newOrigin = Origin;
            if (e.Delta > 0)
            {
                Zoom *= coeff;
                newOrigin.X = (int)(newOrigin.X * coeff - e.Location.X * (coeff - 1));
                newOrigin.Y = (int)(newOrigin.Y * coeff - e.Location.Y * (coeff - 1));
            }
            else if (e.Delta < 0)
            {
                Zoom /= coeff;
                newOrigin.X = (int)((newOrigin.X + e.Location.X * (coeff - 1)) / coeff);
                newOrigin.Y = (int)((newOrigin.Y + e.Location.Y * (coeff - 1)) / coeff);
            }
            if (Zoom != prevZoom)
                Origin = newOrigin;
            base.OnMouseWheel(e);
        }
    }
}
