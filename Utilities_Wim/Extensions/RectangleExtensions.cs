/*
 * User: Sam Brinly
 * Date: 2/4/2013
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace EdocsUSA.Utilities.Extensions
{
	public static class RectangleExtensions
	{
		public static IEnumerable<Point> EnumeratePoints(this Rectangle rect)
		{
			for (int x = rect.Left; x <= rect.Right; x++)
			{
				for (int y = rect.Top; y <= rect.Bottom; y++)
				{ yield return new Point(x, y); }
			}
		}
		
		public static Rectangle GetScaledInstance(this Rectangle value, int maxWidth, int maxHeight)
		{
			Size newSize = value.Size.GetScaledInstance(maxWidth, maxHeight);
			return new Rectangle(value.X, value.Y, newSize.Width, newSize.Height);
		}
		
		public static Rectangle GetScaledInstance(this Rectangle value, Size maxSize)
		{ return value.GetScaledInstance(maxSize.Width, maxSize.Height); }
		
		public static Rectangle ToRectangle(this RectangleF value, MidpointRounding rounding)
		{
			return new Rectangle(
				(int)Math.Round(value.X, rounding),
				(int)Math.Round(value.Y, rounding),
				(int)Math.Round(value.Width, rounding),
				(int)Math.Round(value.Height, rounding));
		}
		
		public static Rectangle ToRectangle(this RectangleF value)
		{ 
			return new Rectangle(
				(int)Math.Floor(value.X),
				(int)Math.Floor(value.Y),
				(int)Math.Floor(value.Width),
				(int)Math.Floor(value.Height));
		}
		
		public static RectangleF ToRectangleF(this Rectangle value)
		{ return new RectangleF(value.X, value.Y, value.Width, value.Height); }
	}
}
