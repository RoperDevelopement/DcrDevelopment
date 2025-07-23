/*
 * Created by SharpDevelop.
 * User: Sam Brinly
 * Date: 5/23/2011
 * Time: 8:39 AM
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;

namespace EdocsUSA.Utilities
{
	public static class Code128BarcodeGenerator
	{
		private static Dictionary<int, int[]> Code128BarSpaceWidths = new Dictionary<int, int[]>()
		{
			{0, new int[] {2, 1, 2, 2, 2, 2} }
			,{1, new int[] {2, 2, 2, 1, 2, 2} }
			,{2, new int[] {2, 2, 2, 2, 2, 1} }
			,{3, new int[] {1, 2, 1, 2, 2, 3} }
			,{4, new int[] {1, 2, 1, 3, 2, 2} }
			,{5, new int[] {1, 3, 1, 2, 2, 2} }
			,{6, new int[] {1, 2, 2, 2, 1, 3} }
			,{7, new int[] {1, 2, 2, 3, 1, 2} }
			,{8, new int[] {1, 3, 2, 2, 1, 2} }
			,{9, new int[] {2, 2, 1, 2, 1, 3} }
			,{10, new int[] {2, 2, 1, 3, 1, 2} }
			,{11, new int[] {2, 3, 1, 2, 1, 2} }
			,{12, new int[] {1, 1, 2, 2, 3, 2} }
			,{13, new int[] {1, 2, 2, 1, 3, 2} }
			,{14, new int[] {1, 2, 2, 2, 3, 1} }
			,{15, new int[] {1, 1, 3, 2, 2, 2} }
			,{16, new int[] {1, 2, 3, 1, 2, 2} }
			,{17, new int[] {1, 2, 3, 2, 2, 1} }
			,{18, new int[] {2, 2, 3, 2, 1, 1} }
			,{19, new int[] {2, 2, 1, 1, 3, 2} }
			,{20, new int[] {2, 2, 1, 2, 3, 1} }
			,{21, new int[] {2, 1, 3, 2, 1, 2} }
			,{22, new int[] {2, 2, 3, 1, 1, 2} }
			,{23, new int[] {3, 1, 2, 1, 3, 1} }
			,{24, new int[] {3, 1, 1, 2, 2, 2} }
			,{25, new int[] {3, 2, 1, 1, 2, 2} }
			,{26, new int[] {3, 2, 1, 2, 2, 1} }
			,{27, new int[] {3, 1, 2, 2, 1, 2} }
			,{28, new int[] {3, 2, 2, 1, 1, 2} }
			,{29, new int[] {3, 2, 2, 2, 1, 1} }
			,{30, new int[] {2, 1, 2, 1, 2, 3} }
			,{31, new int[] {2, 1, 2, 3, 2, 1} }
			,{32, new int[] {2, 3, 2, 1, 2, 1} }
			,{33, new int[] {1, 1, 1, 3, 2, 3} }
			,{34, new int[] {1, 3, 1, 1, 2, 3} }
			,{35, new int[] {1, 3, 1, 3, 2, 1} }
			,{36, new int[] {1, 1, 2, 3, 1, 3} }
			,{37, new int[] {1, 3, 2, 1, 1, 3} }
			,{38, new int[] {1, 3, 2, 3, 1, 1} }
			,{39, new int[] {2, 1, 1, 3, 1, 3} }
			,{40, new int[] {2, 3, 1, 1, 1, 3} }
			,{41, new int[] {2, 3, 1, 3, 1, 1} }
			,{42, new int[] {1, 1, 2, 1, 3, 3} }
			,{43, new int[] {1, 1, 2, 3, 3, 1} }
			,{44, new int[] {1, 3, 2, 1, 3, 1} }
			,{45, new int[] {1, 1, 3, 1, 2, 3} }
			,{46, new int[] {1, 1, 3, 3, 2, 1} }
			,{47, new int[] {1, 3, 3, 1, 2, 1} }
			,{48, new int[] {3, 1, 3, 1, 2, 1} }
			,{49, new int[] {2, 1, 1, 3, 3, 1} }
			,{50, new int[] {2, 3, 1, 1, 3, 1} }
			,{51, new int[] {2, 1, 3, 1, 1, 3} }
			,{52, new int[] {2, 1, 3, 3, 1, 1} }
			,{53, new int[] {2, 1, 3, 1, 3, 1} }
			,{54, new int[] {3, 1, 1, 1, 2, 3} }
			,{55, new int[] {3, 1, 1, 3, 2, 1} }
			,{56, new int[] {3, 3, 1, 1, 2, 1} }
			,{57, new int[] {3, 1, 2, 1, 1, 3} }
			,{58, new int[] {3, 1, 2, 3, 1, 1} }
			,{59, new int[] {3, 3, 2, 1, 1, 1} }
			,{60, new int[] {3, 1, 4, 1, 1, 1} }
			,{61, new int[] {2, 2, 1, 4, 1, 1} }
			,{62, new int[] {4, 3, 1, 1, 1, 1} }
			,{63, new int[] {1, 1, 1, 2, 2, 4} }
			,{64, new int[] {1, 1, 1, 4, 2, 2} }
			,{65, new int[] {1, 2, 1, 1, 2, 4} }
			,{66, new int[] {1, 2, 1, 4, 2, 1} }
			,{67, new int[] {1, 4, 1, 1, 2, 2} }
			,{68, new int[] {1, 4, 1, 2, 2, 1} }
			,{69, new int[] {1, 1, 2, 2, 1, 4} }
			,{70, new int[] {1, 1, 2, 4, 1, 2} }
			,{71, new int[] {1, 2, 2, 1, 1, 4} }
			,{72, new int[] {1, 2, 2, 4, 1, 1} }
			,{73, new int[] {1, 4, 2, 1, 1, 2} }
			,{74, new int[] {1, 4, 2, 2, 1, 1} }
			,{75, new int[] {2, 4, 1, 2, 1, 1} }
			,{76, new int[] {2, 2, 1, 1, 1, 4} }
			,{77, new int[] {4, 1, 3, 1, 1, 1} }
			,{78, new int[] {2, 4, 1, 1, 1, 2} }
			,{79, new int[] {1, 3, 4, 1, 1, 1} }
			,{80, new int[] {1, 1, 1, 2, 4, 2} }
			,{81, new int[] {1, 2, 1, 1, 4, 2} }
			,{82, new int[] {1, 2, 1, 2, 4, 1} }
			,{83, new int[] {1, 1, 4, 2, 1, 2} }
			,{84, new int[] {1, 2, 4, 1, 1, 2} }
			,{85, new int[] {1, 2, 4, 2, 1, 1} }
			,{86, new int[] {4, 1, 1, 2, 1, 2} }
			,{87, new int[] {4, 2, 1, 1, 1, 2} }
			,{88, new int[] {4, 2, 1, 2, 1, 1} }
			,{89, new int[] {2, 1, 2, 1, 4, 1} }
			,{90, new int[] {2, 1, 4, 1, 2, 1} }
			,{91, new int[] {4, 1, 2, 1, 2, 1} }
			,{92, new int[] {1, 1, 1, 1, 4, 3} }
			,{93, new int[] {1, 1, 1, 3, 4, 1} }
			,{94, new int[] {1, 3, 1, 1, 4, 1} }
			,{95, new int[] {1, 1, 4, 1, 1, 3} }
			,{96, new int[] {1, 1, 4, 3, 1, 1} }
			,{97, new int[] {4, 1, 1, 1, 1, 3} }
			,{98, new int[] {4, 1, 1, 3, 1, 1} }
			,{99, new int[] {1, 1, 3, 1, 4, 1} }
			,{100, new int[] {1, 1, 4, 1, 3, 1} }
			,{101, new int[] {3, 1, 1, 1, 4, 1} }
			,{102, new int[] {4, 1, 1, 1, 3, 1} }
			,{103, new int[] {2, 1, 1, 4, 1, 2} }
			,{104, new int[] {2, 1, 1, 2, 1, 4} }
			,{105, new int[] {2, 1, 1, 2, 3, 2} }
			,{106, new int[] {2, 3, 3, 1, 1, 1, 2} }
		};
		
		private const int CodeAStartChar = 103;
		private const int CodeBStartChar = 104;
		private const int CodeCStartChar = 105;
		private const int Code128StopChar = 106;
				
		private static int AsciiTo128(int asciiValue)
		{
			return asciiValue - 32;
		}
		
		private static int CharTo128(char c)
		{
			return AsciiTo128((int)c);
		}
		
		private static Brush BarcodeBrush = new SolidBrush(Color.Black);
		public static Brush BarcodeCaptionBrush = new SolidBrush(Color.Black);
		public static Font BarcodeCaptionFont = new Font(GenericFontFamilies.SansSerif.ToString(), 20, FontStyle.Regular, GraphicsUnit.Point);
		
		public static Bitmap GenerateBarcode(string value, int moduleWidth, int barcodeHeight, int imageDpi)
		{ return GenerateBarcode(value, moduleWidth, barcodeHeight, imageDpi, null); }
		
		public static Bitmap GenerateBarcode(string value, int moduleWidth, int barcodeHeight, int imageDpi, string caption)
		{
			int quietZoneLeftRight = moduleWidth * 10;
			int quietZoneTopBottom = (int)(quietZoneLeftRight / 2);
			
			//Start the checksum with the CodeB start value			
			int checksum = CodeBStartChar;

			//Build the list of bar space widths			
			List<int> barSpaceWidths = new List<int>();
			barSpaceWidths.AddRange(Code128BarSpaceWidths[CodeBStartChar]);			
			for (int charIndex = 0; charIndex < value.Length; charIndex++)
			{
				char c = value[charIndex];
				int code128Value = CharTo128(c);
				//TODO: Validate ascii value
				barSpaceWidths.AddRange(Code128BarSpaceWidths[code128Value]);
				checksum += (charIndex + 1) * code128Value;			
			}
			
			checksum %= 103;
			
			barSpaceWidths.AddRange(Code128BarSpaceWidths[checksum]);
			barSpaceWidths.AddRange(Code128BarSpaceWidths[Code128StopChar]);

			//Calculate the caption size
			Size captionSize = new Size(0, 0);
			if (caption.IsNotEmpty())
			{
				using (Bitmap captionBitmap = new Bitmap(1, 1))
				{
					captionBitmap.SetResolution(imageDpi, imageDpi);
					using (Graphics captionGraphics = Graphics.FromImage(captionBitmap))
					{ 
						captionSize = captionGraphics.MeasureString(caption, BarcodeCaptionFont).ToSize();
						captionSize.Width += (quietZoneLeftRight * 2);
						//Caption will already be top padded
						captionSize.Height += (quietZoneTopBottom);
					}
				}
			}
			
			//Calculate the total bitmap width
			int bitmapWidth = quietZoneLeftRight;
			foreach(int barSpaceWidth in barSpaceWidths) bitmapWidth += (barSpaceWidth * moduleWidth);
			bitmapWidth += quietZoneLeftRight;
			if (captionSize.Width > bitmapWidth) bitmapWidth = captionSize.Width;
						
			//Calculate the total bitmap height
			int bitmapHeight = quietZoneTopBottom + barcodeHeight + quietZoneTopBottom + captionSize.Height;
		
			Bitmap bmp = new Bitmap(bitmapWidth, bitmapHeight);
			bmp.SetResolution(imageDpi, imageDpi);
			using (Graphics g = Graphics.FromImage(bmp))
			{
				g.Clear(Color.White);
				g.PageUnit = GraphicsUnit.Pixel;
				Point p = new Point(quietZoneLeftRight, quietZoneTopBottom);
				//Draw the barcode
				for (int barSpaceIndex = 0; barSpaceIndex < barSpaceWidths.Count; barSpaceIndex++)
				{
					int barSpaceWidth = moduleWidth * barSpaceWidths[barSpaceIndex];
					//If i is zero or even, it's a bar, so draw it
					if ((barSpaceIndex % 2) == 0) g.FillRectangle(BarcodeBrush, p.X, p.Y, barSpaceWidth, barcodeHeight);
					//Increment X value of the current point by the bar or space width
					p = new Point(p.X + barSpaceWidth, p.Y);
				}
				//Draw the caption
				p = new Point(0, quietZoneTopBottom + barcodeHeight + quietZoneTopBottom);
				g.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;
				g.DrawString(caption, BarcodeCaptionFont, BarcodeCaptionBrush, p);
				
				g.Flush();
			}
			return bmp;
		}
	}
}
