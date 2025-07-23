/*
 * User: Sam Brinly
 * Date: 2/12/2013
 */
using System;
using System.Drawing;
using FreeImageAPI;

namespace Scanquire.Public
{
    //TODO:Convert to percentage based areas.
    /// <summary>A region of an image.</summary>
	public enum SQImageRegion
	{
		All,
		BottomHalf,
		BottomLeft,
		BottomRight,
		BottomThird,
		TopHalf,
		TopLeft,
		TopRight,
		TopThird,
		MiddleThird,
	}

	public static class SQImageRegionExtensions
	{
        /// <summary>Convert an SQImageRegion to a rectangle.</summary>
        /// <param name="region">Region of the image to return.</param>
        /// <param name="fib">The image to extract the rectangle from.</param>
        /// <returns>Rectangle (in pixels) defined by the specifued SQImageRegion</returns>
		public static Rectangle ToRectangle(this SQImageRegion region, FreeImageBitmap fib)
		{
			int imageHeight = fib.Height;
			int imageWidth = fib.Width;
			
			int thirdRegionHeight = (int)(Math.Floor(decimal.Divide(imageHeight, 3)));
			int halfRegionHeight = (int)(Math.Floor(decimal.Divide(imageHeight, 2)));
			int quarterRegionHeight = (int)(Math.Floor(decimal.Divide(imageHeight, 4)));
			int quarterRegionWidth = (int)(Math.Floor(decimal.Divide(imageWidth, 2)));
			
			switch (region)
			{
				case SQImageRegion.BottomHalf:
					return new Rectangle(0, halfRegionHeight, imageWidth, halfRegionHeight);
				case SQImageRegion.BottomLeft:
					return new Rectangle(0, quarterRegionHeight, quarterRegionWidth, quarterRegionHeight);
				case SQImageRegion.BottomRight:
					return new Rectangle(quarterRegionWidth, quarterRegionHeight, quarterRegionWidth, quarterRegionHeight);
				case SQImageRegion.BottomThird:
					return new Rectangle(0, thirdRegionHeight * 2, imageWidth, thirdRegionHeight);
				case SQImageRegion.MiddleThird:
					return new Rectangle(0, thirdRegionHeight, imageWidth, thirdRegionHeight);
				case SQImageRegion.TopHalf:
					return new Rectangle(0, 0, imageWidth, halfRegionHeight);
				case SQImageRegion.TopLeft:
					return new Rectangle(0, 0, quarterRegionWidth, quarterRegionHeight);
				case SQImageRegion.TopRight:
					return new Rectangle(quarterRegionWidth, 0, quarterRegionWidth, quarterRegionHeight);
				case SQImageRegion.TopThird:
					return new Rectangle(0, 0, imageWidth, thirdRegionHeight);
				default:
					return new Rectangle(0, 0, imageWidth, imageHeight);
			}
		}

        /// <summary>Convert an SQImageRegion to a rectangle.</summary>
        /// <param name="region">Region of the image to return.</param>
        /// <param name="fib">The image to extract the rectangle from.</param>
        /// <returns>Rectangle (in pixels) defined by the specifued SQImageRegion</returns>
		public static Rectangle ToRectangle(this SQImageRegion region, Bitmap bitmap)
		{
			using (FreeImageBitmap fib = new FreeImageBitmap(bitmap))
			{ return region.ToRectangle(fib); }
		}

        /// <summary>Convert an SQImageRegion to a rectangle.</summary>
        /// <param name="region">Region of the image to return.</param>
        /// <param name="fib">The image to extract the rectangle from.</param>
        /// <returns>Rectangle (in pixels) defined by the specifued SQImageRegion</returns>
		public static Rectangle ToRectangle(this SQImageRegion region, Image image)
		{ return region.ToRectangle((Bitmap)image); }
	}
}
