/*
 * User: Sam
 * Date: 10/10/2011
 * Time: 1:22 PM
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using EdocsUSA.Utilities.Extensions;

namespace EdocsUSA.Utilities
{
	public static class ImageTools
	{
		#region Constants
		
		public const int POINTS_PER_INCH = 72;
		public const int POINTS_PER_HALF_INCH = 36;
		public const int POINTS_PER_QUARTER_INCH = 18;
		public const int POINTS_PER_EIGHTH_INCH = 9;
		
		public const float INCHES_PER_METER = 39.3700787F;
		
		#endregion Constants
		
		
		#region Encoding		
		
		public static ImageCodecInfo GetImageEncoder(string mimeType)
		{
			mimeType = mimeType.ToUpper();
			ImageCodecInfo codecInfo = ImageCodecInfo.GetImageEncoders().Where(x => x.MimeType.ToUpper().Equals(mimeType)).FirstOrDefault();
            if (codecInfo == null)
            {
                Logging.TraceLogger.TraceLoggerInstance.TraceError("Image codec for " + mimeType + " could not be found. mimeType");
                throw new ArgumentException("Image codec for " + mimeType + " could not be found.", "mimeType");
            }
            else return codecInfo;
		}
		
		public static byte[] EncodeToBytes(this Image image, ImageFormat imageFormat)
		{
			using (MemoryStream imageStream = new MemoryStream())
			{
				image.Save(imageStream, imageFormat);
				return imageStream.ToArray();
			}
		}
		
		public static byte[] EncodeToBytes(this Image image, ImageCodecInfo imageCodecInfo, EncoderParameters encoderParameters)
		{
			using (MemoryStream imageStream = new MemoryStream())
			{
				image.Save(imageStream, imageCodecInfo, encoderParameters);
				return imageStream.ToArray();
			}
		}
		
		public static byte[] EncodeToTiffBytes(this Image image, EncoderValue tiffFormat)
		{
			ImageCodecInfo tiffCodecInfo = GetImageEncoder("image/tiff");
			
			Encoder tiffEncoder = Encoder.Compression;
			EncoderParameters tiffEncoderParameters = new EncoderParameters();
			tiffEncoderParameters.Param[0] = new EncoderParameter(tiffEncoder, (long)tiffFormat);
			
			return image.EncodeToBytes(tiffCodecInfo, tiffEncoderParameters);
		}
		
		public static byte[] EncodeToJpegBytes(this Image image, int quality, bool scaleToNearest16Pixels)
		{
			if (scaleToNearest16Pixels)
			{
				//Calucluate the new dimensions
				int newWidth = (int)(Math.Round((image.Width) / 16.0, MidpointRounding.AwayFromZero) * 16.0);
				int newHeight = (int)(Math.Round((image.Height) / 16.0, MidpointRounding.AwayFromZero) * 16.0);
				
				//Create a temp bitmap to draw the scaled version to
				Bitmap b = new Bitmap(newWidth, newHeight);
				b.SetResolution(image.HorizontalResolution, image.VerticalResolution);
				
				//Copy the image from the image to the bitmap
				using (Image imageCopy = (Image)b)
				using (Graphics g = Graphics.FromImage(imageCopy))
				{
					g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
					g.DrawImage(image, 0, 0, imageCopy.Width, imageCopy.Height);
					return imageCopy.EncodeToJpegBytes(quality, false);
				}
			}
			//Image is already scaled, or does not need to be scaled, convert it to JPEG
			else
			{
				ImageCodecInfo jpegCodecInfo = GetImageEncoder("image/jpeg");
				
				Encoder jpegEncoder = Encoder.Quality;
				EncoderParameters jpegEncoderParameters = new EncoderParameters();
				jpegEncoderParameters.Param[0] = new EncoderParameter(jpegEncoder, (long)quality);
				
				return image.EncodeToBytes(jpegCodecInfo, jpegEncoderParameters);
			}
		}
		
		#endregion Encoding
		
		#region Loading
		
		public static Image LoadFromBytes(byte[] imageBytes)
		{ return Image.FromStream(new MemoryStream(imageBytes), true, false); }
		
		#endregion Loading
		
		#region Thumbnail
		
		public static Image GenerateThumbnail(this Image image, Size maxSize)
		{
			//Calculate the new width and height, maintaining the ratio
			int thumbWidth, thumbHeight;
			if (image.Width > image.Height) 
			{
				thumbWidth = maxSize.Width;
				thumbHeight = (image.Height * maxSize.Width) / image.Width;
			}
			else 
			{
				thumbHeight = maxSize.Height;
				thumbWidth = (image.Width * maxSize.Height) / image.Height;
			}
			return image.GetThumbnailImage(thumbWidth, thumbHeight, null, IntPtr.Zero);
		}
		
		#endregion Thumbnail
		
		#region Loading
		
		public static byte[] HDIBToBytes(IntPtr hDib, bool freeHDib)
		{
			try
			{
				IntPtr pDIB = Interop.Kernel32.GlobalLock(hDib);
				return DIBToBytes(pDIB);
			}
			finally 
			{ 
				Interop.Kernel32.GlobalUnlock(hDib); 
				if (freeHDib) FreeHDIB(hDib);
			}
		}
		
		public static byte[] HDIBToBytes(IntPtr hDib)
		{ return HDIBToBytes(hDib, false); }
		
		public static void FreeHDIB(IntPtr hDib) { Interop.Kernel32.GlobalFree(hDib); }
     
		public static byte[] DIBToBytes(IntPtr pDIB)
		{
			//Get the bitmap header info
			BITMAPINFOHEADER infoHeader = (BITMAPINFOHEADER)Marshal.PtrToStructure(pDIB, typeof(BITMAPINFOHEADER));
			
			if (infoHeader.biSizeImage == 0)
			{ infoHeader.biSizeImage = ((((infoHeader.biWidth * infoHeader.biBitCount) + 31) & ~31) * Math.Abs(infoHeader.biHeight)); }
			if ((infoHeader.biClrUsed == 0) && (infoHeader.biBitCount < 16))
			{ infoHeader.biClrUsed = 1 << infoHeader.biBitCount; }
			
			int fileHeaderSize = Marshal.SizeOf(typeof(BITMAPFILEHEADER));
			int dibSize = infoHeader.biSize + (infoHeader.biClrUsed * 4) + infoHeader.biSizeImage;
			
			BITMAPFILEHEADER fileHeader = new BITMAPFILEHEADER();
			fileHeader.bfType = 0x4D42;
			fileHeader.bfSize = fileHeaderSize + dibSize;
			fileHeader.bfOffBits = fileHeaderSize + infoHeader.biSize + (infoHeader.biClrUsed * 4);
			
			byte[] data = new Byte[fileHeader.bfSize];
			IntPtr fileHeaderPtr = Marshal.AllocHGlobal((int)fileHeaderSize);
			Marshal.StructureToPtr(fileHeader, fileHeaderPtr, true);
			Marshal.Copy(fileHeaderPtr, data, 0, (int)fileHeaderSize);
			Marshal.FreeHGlobal(fileHeaderPtr);
			Marshal.Copy(pDIB, data, (int)fileHeaderSize, (int)dibSize);
			
			return data;
		}
     
		#endregion Loading
		
		#region Misc
     
		public static Bitmap Rotate(this Bitmap bitmap, float angle)
		{
			PixelFormat pf = bitmap.PixelFormat.HasFlag(PixelFormat.Indexed) ? PixelFormat.Format24bppRgb : bitmap.PixelFormat;
			Bitmap rotatedBitmap = new Bitmap(bitmap.Width, bitmap.Height, pf);
			rotatedBitmap.SetResolution(bitmap.HorizontalResolution, bitmap.VerticalResolution);
			using (Graphics g = Graphics.FromImage(rotatedBitmap))
			{
				g.Clear(Color.White);
				g.TranslateTransform((float)bitmap.Width/2, (float)bitmap.Height/2);
				g.RotateTransform(angle);
				g.TranslateTransform(-(float)bitmap.Width/2, -(float)bitmap.Height/2);
				g.DrawImage(bitmap, 0, 0);
			}
			rotatedBitmap.SetResolution(bitmap.HorizontalResolution, bitmap.VerticalResolution);
			return rotatedBitmap;
		}
		
		public static Bitmap GenerateBlankBitmap(int width, int height, Color color)
		{
			Bitmap b = new Bitmap(width, height, PixelFormat.Format24bppRgb);
			using (Graphics g = Graphics.FromImage(b))
			{
				g.Clear(color);
				g.Save();
			}
			return b;
		}
		/*
		public static IEnumerable<Point> IterateRectangle(Point p1, Point p2)
		{
			int startX, startY, endX, endY;
			if (p1.X <= p2.X)
			{
				startX = p1.X;
				endX = p2.X;
			}
			else
			{
				startX = p2.X;
				endX = p1.X;
			}
			
			if (p1.Y <= p2.Y)
			{
				startY = p1.Y;
				endY = p2.Y;
			}
			else
			{
				startY = p2.Y;
				endY = p1.Y;
			}
			
			
			for (int x = startX; x <= endX; x++)
			{
				for(int y = startY; x <= endY; y++)
				{
					yield return new Point(x, y);
				}
			}
		}
		*/
		
		public static float PointsToPixels(float value, float dpi)
		{ return ((value / POINTS_PER_INCH) * dpi); }
		
		public static PointF PointsToPixels(PointF value, float dpi)
		{ 
			return new PointF(
				PointsToPixels(value.X, dpi)
				, PointsToPixels(value.Y, dpi));
		}
		
		public static Point PointsToPixels(Point value, float dpi)
		{
			return new Point(
				(int)(PointsToPixels(value.X, dpi))
				, (int)(PointsToPixels(value.Y, dpi)));
		}
		
		public static RectangleF PointsToPixels(RectangleF value, float dpi)
		{
			return new RectangleF(
				PointsToPixels(value.Left, dpi)
				, PointsToPixels(value.Top, dpi)
				, PointsToPixels(value.Width, dpi)
				, PointsToPixels(value.Height, dpi));
		}
		
		public static Rectangle PointsToPixels(Rectangle value, float dpi)
		{
			return new Rectangle(
				(int)(PointsToPixels(value.Left, dpi))
				, (int)(PointsToPixels(value.Top, dpi))
				, (int)(PointsToPixels(value.Width, dpi))
				, (int)(PointsToPixels(value.Bottom, dpi)));
		}
		
		public static Padding PointsToPixels(Padding value, float dpi)
		{
			return new Padding(
				(int)(PointsToPixels(value.Left, dpi))
				, (int)(PointsToPixels(value.Top, dpi))
				, (int)(PointsToPixels(value.Right, dpi))
          	, (int)(PointsToPixels(value.Bottom, dpi)));
		}
		
		public static float PixelsToPoints(float pixels, float dpi)
		{ return ((pixels / dpi) * POINTS_PER_INCH); }
		
		public static PointF PixelsToPoints(PointF value, float dpi)
		{ 
			return new PointF(
				PixelsToPoints(value.X, dpi)
				, PixelsToPoints(value.Y, dpi));
		}
		
		public static Point PixelsToPoints(Point value, float dpi)
		{
			return new Point(
				(int)(PixelsToPoints(value.X, dpi))
				, (int)(PixelsToPoints(value.Y, dpi)));
		}
		
		public static RectangleF PixelsToPoints(RectangleF value, float dpi)
		{
			return new RectangleF(
				PixelsToPoints(value.Left, dpi)
				, PixelsToPoints(value.Top, dpi)
				, PixelsToPoints(value.Width, dpi)
				, PixelsToPoints(value.Height, dpi));
		}
		
		public static Rectangle PixelsToPoints(Rectangle value, float dpi)
		{
			return new Rectangle(
				(int)(PixelsToPoints(value.Left, dpi))
				, (int)(PixelsToPoints(value.Top, dpi))
				, (int)(PixelsToPoints(value.Width, dpi))
				, (int)(PixelsToPoints(value.Bottom, dpi)));
		}
		
		public static Padding PixelsToPoints(Padding value, float dpi)
		{
			return new Padding(
				(int)(PixelsToPoints(value.Left, dpi))
				, (int)(PixelsToPoints(value.Top, dpi))
				, (int)(PixelsToPoints(value.Right, dpi))
				, (int)(PixelsToPoints(value.Bottom, dpi)));
		}
		
		public static float InchesToPoints(float value)
		{ return value * POINTS_PER_INCH; }
		
		public static PointF InchesToPoints(PointF value, float dpi)
		{ 
			return new PointF(
				InchesToPoints(value.X)
				, InchesToPoints(value.Y));
		}
		
		public static Point InchesToPoints(Point value, float dpi)
		{
			return new Point(
				(int)(InchesToPoints(value.X))
				, (int)(InchesToPoints(value.Y)));
		}		
		
		public static RectangleF InchesToPoints(RectangleF value)
		{
			return new RectangleF(
				InchesToPoints(value.Left)
				, InchesToPoints(value.Top)
				, InchesToPoints(value.Width)
				, InchesToPoints(value.Height));
		}
		
		public static Rectangle InchesToPoints(Rectangle value)
		{
			return new Rectangle(
				(int)(InchesToPoints(value.Left))
				, (int)(InchesToPoints(value.Top))
				, (int)(InchesToPoints(value.Width))
				, (int)(InchesToPoints(value.Height)));			
		}
		
		public static Padding InchesToPoints(Padding value)
		{
			return new Padding(
				(int)(InchesToPoints(value.Left))
				, (int)(InchesToPoints(value.Top))
				, (int)(InchesToPoints(value.Right))
				, (int)(InchesToPoints(value.Bottom)));
		}
		
		public static float PointsToInches(float value)
		{ return value / POINTS_PER_INCH; }

		public static PointF PointsToInches(PointF value)
		{ 
			return new PointF(
				PointsToInches(value.X)
				, PointsToInches(value.Y));
		}
		
		public static Point PointsToInches(Point value)
		{
			return new Point(
				(int)(PointsToInches(value.X))
				, (int)(PointsToInches(value.Y)));
		}		
		
		public static RectangleF PointsToInches(RectangleF value)
		{
			return new RectangleF(
				PointsToInches(value.Left)
				, PointsToInches(value.Top)
				, PointsToInches(value.Width)
				, PointsToInches(value.Height));
		}
		
		public static Rectangle PointsToInches(Rectangle value)
		{
			return new Rectangle(
				(int)(PointsToInches(value.Left))
				, (int)(PointsToInches(value.Top))
				, (int)(PointsToInches(value.Width))
				, (int)(PointsToInches(value.Height)));
		}
		
		public static Padding PointsToInches(Padding value)
		{
			return new Padding(
				(int)(PointsToInches(value.Left))
				, (int)(PointsToInches(value.Top))
				, (int)(PointsToInches(value.Right))
				, (int)(PointsToInches(value.Bottom)));
		}
		
		public static float InchesToPixels(float value, float dpi)
		{ return value * dpi; }
		
		public static PointF InchesToPixels(PointF value, float dpi)
		{ 
			return new PointF(
				InchesToPixels(value.X, dpi)
				, InchesToPixels(value.Y, dpi));
		}
		
		public static Point InchesToPixels(Point value, float dpi)
		{
			return new Point(
				(int)(InchesToPixels(value.X, dpi))
				, (int)(InchesToPixels(value.Y, dpi)));
		}		
		
		public static RectangleF InchesToPixels(RectangleF value, float dpi)
		{
			return new RectangleF(
				InchesToPixels(value.Left, dpi)
				, InchesToPixels(value.Top, dpi)
				, InchesToPixels(value.Width, dpi)
				, InchesToPixels(value.Height, dpi));
		}
		
		public static RectangleF InchesToPixels(Rectangle value, float dpi)
		{
			return new Rectangle(
				(int)(InchesToPixels(value.Left, dpi))
				, (int)(InchesToPixels(value.Top, dpi))
				, (int)(InchesToPixels(value.Width, dpi))
				, (int)(InchesToPixels(value.Height, dpi)));
		}
		
		public static Padding InchesToPixels(Padding value, float dpi)
		{
			return new Padding(
				(int)(InchesToPixels(value.Left, dpi))
				, (int)(InchesToPixels(value.Top, dpi))
				, (int)(InchesToPixels(value.Right, dpi))
				, (int)(InchesToPixels(value.Bottom, dpi)));
		}
		
		public static float PixelsToInches(float value, float dpi)
		{ return value / dpi; }
		
		public static PointF PixelsToInches(PointF value, float dpi)
		{ 
			return new PointF(
				PixelsToInches(value.X, dpi)
				, PixelsToInches(value.Y, dpi));
		}
		
		public static Point PixelsToInches(Point value, float dpi)
		{
			return new Point(
				(int)(PixelsToInches(value.X, dpi))
				, (int)(PixelsToInches(value.Y, dpi)));
		}		
		
		public static RectangleF PixelsToInches(RectangleF value, float dpi)
		{
			return new RectangleF(
				PixelsToInches(value.Left, dpi)
				, PixelsToInches(value.Top, dpi)
				, PixelsToInches(value.Width, dpi)
				, PixelsToInches(value.Height, dpi));
		}
		
		public static Rectangle PixelsToInches(Rectangle value, float dpi)
		{
			return new Rectangle(
				(int)(PixelsToInches(value.Left, dpi))
				, (int)(PixelsToInches(value.Top, dpi))
				, (int)(PixelsToInches(value.Width, dpi))
				, (int)(PixelsToInches(value.Height, dpi)));
		}
		
		public static Padding PixelsToInches(Padding value, float dpi)
		{
			return new Padding(
				(int)(PixelsToInches(value.Left, dpi))
				, (int)(PixelsToInches(value.Top, dpi))
				, (int)(PixelsToInches(value.Right, dpi))
				, (int)(PixelsToInches(value.Bottom, dpi)));			
		}
		
		public static float GetDPIFromPPM(float value, float ppm)
		{
			float inchesPerMeter = INCHES_PER_METER;
			float dpi = (float)Math.Round(ppm / inchesPerMeter);
			return dpi;
		}
     
		/// <summary>
		/// Set a single pixel of an indexed bitmap
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="bmd"></param>
		/// <param name="pixel"></param>
		/// <remarks>Taken from http://www.bobpowell.net/onebit.htm</remarks>
		private static void SetIndexedPixel(int x,int y,BitmapData bmd, bool pixel)
		{
			int index=y*bmd.Stride+(x>>3);
			byte p=Marshal.ReadByte(bmd.Scan0,index);
			byte mask=(byte)(0x80>>(x&0x7));
			if(pixel)  p |=mask;
			else p &=(byte)(mask^0xff);
			
			Marshal.WriteByte(bmd.Scan0,index,p);
		}
     
     #endregion Misc
		
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct BITMAPINFOHEADER
		{
			public int biSize;
			public int biWidth;
			public int biHeight;
			public short biPlanes;
			public short biBitCount;
			public int biCompression;
			public int biSizeImage;
			public int biXPelsPerMeter;
			public int biYPelsPerMeter;
			public int biClrUsed;
			public int biClrImportant;
		
			public void Init()
			{
				biSize = Marshal.SizeOf(this);
			}
		}
		
		[StructLayout(LayoutKind.Sequential, Pack=1)]
		public struct BITMAPFILEHEADER
		{
			public short bfType;
			public int bfSize;
			public short bfReserved1;
			public short bfReserved2;
			public int bfOffBits;
		}		
	}
}
