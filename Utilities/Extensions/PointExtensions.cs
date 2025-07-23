/*
 * User: Sam Brinly
 * Date: 1/30/2013
 */
using System;
using System.Drawing;

namespace EdocsUSA.Utilities.Extensions
{
	/// <summary>
	/// Description of RectangleExtensions.
	/// </summary>
	public static class PointExtensions
	{
		public static Point ConstrainTo(this Point value, Rectangle rectangle)
		{
			return new Point()
			{
				X = value.X.ConstrainTo(rectangle.X, rectangle.Right),
				Y = value.Y.ConstrainTo(rectangle.Y, rectangle.Bottom)
			};
		}
	}
}
