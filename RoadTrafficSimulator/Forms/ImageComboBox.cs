using System;
using System.Drawing;
using System.Windows.Forms;

namespace RoadTrafficSimulator.Forms
{
    // Code inspired by https://stackoverflow.com/a/9706102/11983817
    // and fixes by https://stackoverflow.com/q/5864633/11983817

    /// <summary>
    /// Represents a combo box supporting images in items.
    /// </summary>
    class ImageComboBox : ComboBox
    {
        /// <summary>
        /// Creates a new image combo box.
        /// </summary>
        public ImageComboBox()
        {
            DropDownStyle = ComboBoxStyle.DropDownList;
            //DrawMode = DrawMode.OwnerDrawFixed;
            //SetStyle(ControlStyles.Opaque | ControlStyles.UserPaint, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // TODO
            base.OnPaint(e);
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            e.DrawBackground();
            e.DrawFocusRectangle();

            if (e.Index < 0)
            {
                base.OnDrawItem(e);
                return;
            }

            DropDownItem item = Items[e.Index] as DropDownItem;
            if (item.Image != null)
                e.Graphics.DrawImage(item.Image, e.Bounds.Left, e.Bounds.Top);
            Brush brush = new SolidBrush(e.ForeColor);
            e.Graphics.DrawString(item.Text, e.Font, brush, e.Bounds.Left + item.Image.Width, e.Bounds.Top + 2);

            base.OnDrawItem(e);
        }

        /// <summary>
        /// Represents an item in <see cref="ImageComboBox"/>.
        /// </summary>
        public class DropDownItem
        {
            /// <summary>
            /// Text of the item
            /// </summary>
            public string Text { get; set; }
            /// <summary>
            /// Image of the item (can be <c>null</c>)
            /// </summary>
            public Image Image { get; set; }

            public DropDownItem(string text, Image image)
            {
                Text = text;
                Image = image;
            }

            public override string ToString() => Text;
        }
    }
}
