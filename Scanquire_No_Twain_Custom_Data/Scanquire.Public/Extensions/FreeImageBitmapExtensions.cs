/*
 * User: Sam Brinly
 * Date: 1/29/2013
 */
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using FreeImageAPI;
using EdocsUSA.Utilities;
using EdocsUSA.Utilities.Extensions;
using EDL = EdocsUSA.Utilities.Logging;

namespace Scanquire.Public.Extensions
{
	public static class FreeImageBitmapExtensions
	{
		public static byte[] Save(this FreeImageBitmap fib, FREE_IMAGE_FORMAT format, FREE_IMAGE_SAVE_FLAGS flags)
		{
			using (MemoryStream stream = new MemoryStream())
			{
				fib.Save(stream, format, flags);
				return stream.ToArray();
			}
		}
		
		public static byte[] Save(this FreeImageBitmap fib, FREE_IMAGE_FORMAT format)
		{ return fib.Save(format, FREE_IMAGE_SAVE_FLAGS.DEFAULT); }
		
		/// <summary>Fill a rectangular region with a solid color.</summary>
		/// <remarks>Images less than 24BPP will be converted to 24BPP.</remarks>
		public static void FillRectangle(this FreeImageBitmap fib, Rectangle rect, Color color)
		{
			//TODO: Convert back to original color depth?
			if (fib.ColorDepth < 24) fib.ConvertColorDepth(FREE_IMAGE_COLOR_DEPTH.FICD_24_BPP);
			
			//Loop throug the pixels and set the colors
			//FreeImage pixel access functions operate from bottom left
			foreach (int x in IntExtensions.Range(rect.X, rect.X + rect.Width))
			{
				foreach (int y in IntExtensions.Range(fib.Height - (rect.Y + rect.Height), fib.Height - rect.Y))
				{ fib.SetPixel(x, y, color); }
			}
		}
		
		/// <summary>
		/// Perform a rotate/flip operation using .net orientation (clockwise)
		/// rather than the default (counter-clockwise) rotation of FreeImage
		/// </summary>
		/// <param name="fib"></param>
		/// <param name="rotateFlipType"></param>
		public static void RotateFlipEx(this FreeImageBitmap fib, RotateFlipType rotateFlipType)
		{
			//Reverse 90 and 270 rotations to account for FreeImage's counter-clockwise rotation orientation.
			switch (rotateFlipType)
			{
			    case RotateFlipType.RotateNoneFlipNone: //Also Rotate180FlipXY
			    case RotateFlipType.RotateNoneFlipY: //Also Rotate180FlipX
			    case RotateFlipType.Rotate180FlipNone: //Also RotateNoneFlipXY
			    case RotateFlipType.RotateNoneFlipX: //Also Rotate180FlipY
			        break;
			    case RotateFlipType.Rotate90FlipNone: //Also Rotate270FlipXY
			        rotateFlipType = RotateFlipType.Rotate270FlipNone;
			        break;
			    case RotateFlipType.Rotate270FlipNone: //Also Rotate90FlipXY
			        rotateFlipType = RotateFlipType.Rotate90FlipNone;
			        break;
			    case RotateFlipType.Rotate90FlipX: //Also Rotate270FlipY
			        rotateFlipType = RotateFlipType.Rotate270FlipX;
			        break;
			    case RotateFlipType.Rotate90FlipY: //Also Rotate270FlipX
			        rotateFlipType = RotateFlipType.Rotate270FlipY;
			        break;
			    default:
			        throw new ArgumentException("Unhandled RotateFlipType", "rotateFlipType");
			}
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation(string.Format("DPI before rotate {0}", fib.HorizontalResolution));
            //FreeImage.NET has a bug that does not reset the DPI for 4bit images after rotate, so have to do it manually
            float hRes = fib.HorizontalResolution;
            float vRes = fib.VerticalResolution;
			fib.RotateFlip(rotateFlipType);
            fib.SetResolution(hRes, vRes);
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation(string.Format("DPI after rotate {0}", fib.HorizontalResolution));
        }
		
		static object FromBytesLock = new object();
		
		public static FreeImageBitmap FromBytes(byte[] data)
		{ 
			lock(FromBytesLock)
			{
				try
				{
					//Bitmap b = new Bitmap(new MemoryStream(data));
					return new FreeImageBitmap(new MemoryStream(data));
				}
				catch (Exception ex)
				{
                    EDL.TraceLogger.TraceLoggerInstance.TraceError($"Error reading FreeImageBitmap from bytes {ex.Message}");
                    throw ex;
				}
			}
		}

		public static void Normalize1BPPToMinIsWhite(this FreeImageBitmap fib)
		{
			if (fib.ColorType == FREE_IMAGE_COLOR_TYPE.FIC_MINISBLACK)
			{	
				fib.Palette.Reverse();
				fib.Invert();
			}
		}
		
		/// <summary>Performs Rescale command, but maintains original proportions.</summary>
		/// <param name="scaleResolution">True to also scale the image's resolution.</param>
		public static void RescaleEx(this FreeImageBitmap value, Size maxSize, FREE_IMAGE_FILTER filter, bool scaleResolution)
		{ 
			Size originalSize = value.Size;
			float originalHorizontalResolution = value.HorizontalResolution;
			float originalVerticalResolution = value.VerticalResolution;
			Size newSize = originalSize.GetScaledInstance(maxSize);
			
			value.Rescale(newSize, filter);
			
			if (scaleResolution)
			{
				float HorizontalResolution = ((float)newSize.Width / (float)originalSize.Width) * originalHorizontalResolution;
				float VerticalResolution = ((float)newSize.Height / (float)originalSize.Height) * originalVerticalResolution;
				
				value.SetResolution(HorizontalResolution, VerticalResolution);
			}
		}

		/// <summary>Performs Rescale command, but maintains original proportions.</summary>
		/// <param name="scaleResolution">True to also scale the image's resolution.</param>
		public static void RescaleEx(this FreeImageBitmap value, int maxWidth, int maxHeight, FREE_IMAGE_FILTER filter, bool scaleResolution)
		{ value.RescaleEx(new Size(maxWidth, maxHeight), filter, scaleResolution); }
		
		/// <summary>Performs GetScaledInstance command, but maintains original proportions.</summary>
		/// <param name="scaleResolution">True to also scale the image's resolution.</param>
		public static FreeImageBitmap GetScaledInstanceEx(this FreeImageBitmap value, Size maxSize, FREE_IMAGE_FILTER filter, bool scaleResolution)
		{
			Size originalSize = value.Size;
			float originalHorizontalResolution = value.HorizontalResolution;
			float originalVerticalResolution = value.VerticalResolution;
			Size newSize = originalSize.GetScaledInstance(maxSize);
			
			FreeImageBitmap scaledValue = value.GetScaledInstance(newSize, filter);
			
			if (scaleResolution)
			{
				float HorizontalResolution = ((float)newSize.Width / (float)originalSize.Width) * originalHorizontalResolution;
				float VerticalResolution = ((float)newSize.Height / (float)originalSize.Height) * originalVerticalResolution;
			
				scaledValue.SetResolution(HorizontalResolution, VerticalResolution);
			}
			
			return scaledValue;
		}
		
		/// <summary>Performs GetScaledInstance command, but maintains original proportions.</summary>
		/// <param name="scaleResolution">True to also scale the image's resolution.</param>
		public static FreeImageBitmap GetScaledInstanceEx(this FreeImageBitmap value, int maxWidth, int maxHeight, FREE_IMAGE_FILTER filter, bool scaleResolution)
		{ return value.GetScaledInstanceEx(new Size(maxWidth, maxHeight), filter, scaleResolution); }
	}
}