using System;
using System.Drawing;
using System.IO;

namespace EdocsUSA.Utilities.Extensions
{
	public static class BitmapExtensions
	{
		public static Bitmap FromBytes(byte[] value)
		{
			return new Bitmap(new MemoryStream(value));
		}
	}
}
