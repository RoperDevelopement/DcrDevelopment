using System;
using System.Drawing;
using System.Windows.Forms;

namespace Scanquire.Public.UserControls
{
    /// <summary>A standard SplitContainer with a rectangular grabber</summary>
	public class SQSplitContainer : SplitContainer
	{
        public SQSplitContainer()
        {
            
        }

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
			/*
			Rectangle splitterRectangle = new Rectangle(){
				X = base.SplitterDistance,
				Y = 0,
				Width = SplitterWidth,
				Height = base.Height
			};
			e.Graphics.FillRectangle(SystemBrushes.ControlDark, splitterRectangle);
			*/
		}
	}
}
