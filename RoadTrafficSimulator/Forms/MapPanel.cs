using System;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;

namespace RoadTrafficSimulator.Forms
{
    /// <summary>
    /// Component for visualising a map.
    /// </summary>
    class MapPanel : Panel
    {
        private const float minZoom = 0.2f;
        private const float maxZoom = 5f;

        private Point origin;
        private float zoom;
        private Point prevMouseLocation;
        private bool drag;
        private bool dragOccured;

        /// <summary>
        /// Central point of the panel
        /// </summary>
        public Point Center { get => new(Width / 2, Height / 2); }
        /// <summary>
        /// Position of the map's origin (coordinates (0;0))
        /// </summary>
        public Point Origin
        {
            get => origin;
            private set
            {
                origin = value;
                Redraw();
                OriginChanged?.Invoke(this, new EventArgs());
            }
        }
        /// <summary>
        /// Zoom of the map
        /// </summary>
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
                Redraw();
                ZoomChanged?.Invoke(this, new EventArgs());
            }
        }

        /// <summary>
        /// Occurs when the map origin moves.
        /// </summary>
        [Browsable(true)]
        [Category("Property Changed")]
        [Description("Occurs when the map origin moves.")]
        public event EventHandler OriginChanged;

        /// <summary>
        /// Occurs when map zoom changes.
        /// </summary>
        [Browsable(true)]
        [Category("Property Changed")]
        [Description("Occurs when map zoom changes.")]
        public event EventHandler ZoomChanged;

        /// <summary>
        /// Creates a new map panel.
        /// </summary>
        public MapPanel()
        {
            // Panel properties
            DoubleBuffered = true;
            Cursor = Cursors.Cross;
            // Custom properties
            Origin = Center;
            Zoom = 1f;
        }

        /// <summary>
        /// Ensures that the panel is redrawn.
        /// </summary>
        public void Redraw()
        {
            Invalidate();
        }

        /// <summary>
        /// Resets the map's origin and places it into the center of the panel.
        /// </summary>
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
                Point offset = new(e.Location.X - prevMouseLocation.X, e.Location.Y - prevMouseLocation.Y);
                // Careful with mutable structs...
                Point newOrigin = Origin;
                newOrigin.Offset(offset);
                Origin = newOrigin;
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
