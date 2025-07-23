using System;
using System.Drawing;

namespace EdocsUSA.Utilities.Extensions
{
	public static class SizeExtensions
	{
		public static Size GetScaledInstance(this Size value, int maxWidth, int maxHeight)
		{
			decimal ratioW = decimal.Divide(value.Width, maxWidth);
			decimal ratioH = decimal.Divide(value.Height, maxHeight);
			decimal ratio = Math.Max(ratioW, ratioH);
			
			//Calculate scaled size
			decimal newWidth = decimal.Divide(value.Width, ratio);
			decimal newHeight = decimal.Divide(value.Height, ratio);

			return new Size((int)newWidth, (int)newHeight);
		}
		
		public static Size GetScaledInstance(this Size value, Size maxSize)
		{ return value.GetScaledInstance(maxSize.Width, maxSize.Height); }
	}
}
