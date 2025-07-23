using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Testing.ImageListViewer
{
    /// <summary>A split container with a more intuitive splitter bar.</summary>
    internal class SQImageListViewer_SplitContainer : SplitContainer
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Point[] points = new Point[3];

            if (base.Orientation == Orientation.Horizontal)
            {
                points[0] = new Point(base.Width / 2, base.SplitterDistance + (base.SplitterWidth / 2));
                points[1] = new Point(points[0].X - 10, points[0].Y);
                points[2] = new Point(points[0].X + 10, points[0].Y);
                //pointsRect = new Rectangle(points[1].X - 2, points[1].Y - 2, points[2].X + 2, points[2].Y + 2);
            }
            else
            {
                points[0] = new Point(base.SplitterDistance + (base.SplitterWidth / 2), base.Height / 2);
                points[1] = new Point(points[0].X, points[0].Y - 10);
                points[2] = new Point(points[0].X, points[0].Y + 10);
                //pointsRect = new Rectangle(points[1].X - 2, points[1].Y - 2, points[2].X + 2, points[2].Y + 2);
            }

            foreach (Point point in points)
            {
                point.Offset(-2, -2);
                e.Graphics.FillEllipse(SystemBrushes.ControlDarkDark, new Rectangle(point, new Size(3, 3)));

                point.Offset(1, 1);
                e.Graphics.FillEllipse(SystemBrushes.ControlLight, new Rectangle(point, new Size(3, 3)));
            }
        }
    }
}
