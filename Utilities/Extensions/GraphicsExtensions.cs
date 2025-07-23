/*
 * User: Sam Brinly
 * Date: 1/15/2013
 */
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace EdocsUSA.Utilities.Extensions
{
	public static class GraphicsExtensions
	{
		public static Region DrawRoundedRectangle(Rectangle rect, int radius)
		{
			GraphicsPath gp = new GraphicsPath();
			gp.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
			gp.AddArc(rect.X + rect.Width - radius, rect.Y, radius, radius, 270, 90);
			gp.AddArc(rect.X + rect.Width - radius, rect.Y + rect.Height - radius, radius, radius, 0, 90);
			gp.AddArc(rect.X, rect.Y + rect.Height - radius, radius, radius, 90, 90);
			return new Region(gp);			
		}
	}
}