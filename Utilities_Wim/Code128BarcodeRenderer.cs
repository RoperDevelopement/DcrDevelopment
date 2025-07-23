using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Security.Cryptography;
using System.Windows.Forms;
using EdocsUSA.Utilities;

namespace Scanquire.Public
{
	public class Code128Barcode
	{
		#region BarSpaceWidth definitions
		
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
		
		#endregion BarSpaceWidth definitions
		
		private const int CodeAStartChar = 103;
		private const int CodeBStartChar = 104;
		private const int CodeCStartChar = 105;
		private const int Code128StopChar = 106;
		
		#region Instance Properties
		
		private readonly string _Value;
		public string Value 
		{ get { return _Value; } }
		
		private readonly string _Caption;
		public string Caption
		{ get { return _Caption; } }
		
		private ReadOnlyCollection<int> _BarSpaceWidths;
		public ReadOnlyCollection<int> BarSpaceWidths
		{ get { return _BarSpaceWidths; } }
		
		#endregion Instance Properties
		
		#region Constructors
		
		public Code128Barcode(string value, string caption)
		{
			this._Value = value;
			this._BarSpaceWidths = GetBarSpaceWidths(this._Value).AsReadOnly();
			this._Caption = caption;
		}
		
		#endregion Constructors
		
		public static List<int> GetBarSpaceWidths(string value)
		{
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
			
			return barSpaceWidths;
		}		
		
		//TODO: Find a more generic place for this
		private static int AsciiTo128(int asciiValue)
		{
			return asciiValue - 32;
		}
		
		//TODO: Find a more generic place for this
		private static int CharTo128(char c)
		{
			return AsciiTo128((int)c);
		}
	}
	
	public class Code128BarcodeRenderer
	{		
		/// <summary>Brush used for painting the barcode.</summary>
		private Brush BarcodeBrush = new SolidBrush(Color.Black);
		
		/// <summary>Brush used for painting the barcode caption.</summary>
		private Brush CaptionBrush = new SolidBrush(Color.Black);
		
		private SerializableFont _CaptionFont = new SerializableFont(new Font(GenericFontFamilies.SansSerif.ToString(), ImageTools.POINTS_PER_QUARTER_INCH, FontStyle.Regular, GraphicsUnit.Point));
		/// <summary>Font used for paiting the barcode caption</summary>
		/// <remarks>GraphicsUnits should be Points</remarks>
		public SerializableFont CaptionFont
		{
			get { return _CaptionFont; }
			set { _CaptionFont = value; }
		}
		
		private int _ModuleWidth = 1;
		///<summary>Width of an individual barcode module line (in Points)</summary>
		public int ModuleWidth
		{
			get { return _ModuleWidth; }
			set { _ModuleWidth = value; }
		}
		
		private int _BarcodeHeight = ImageTools.POINTS_PER_HALF_INCH;
		/// <summary>Height of the barcode (in Points)</summary>
		public int BarcodeHeight
		{
			get { return _BarcodeHeight; }
			set { _BarcodeHeight = value; }
		}
		
		private Padding _BarcodeMargin = new Padding(ImageTools.POINTS_PER_HALF_INCH, ImageTools.POINTS_PER_QUARTER_INCH, ImageTools.POINTS_PER_HALF_INCH, 0);
		/// <summary>Margin around the barcode (in Points)</summary>
		public Padding BarcodeMargin
		{
			get { return _BarcodeMargin; }
			set { _BarcodeMargin = value; }
		}
		
		private Padding _CaptionMargin = new Padding(ImageTools.POINTS_PER_HALF_INCH, 0, ImageTools.POINTS_PER_HALF_INCH, 0);
		/// <summary>Margin around the caption (in Points)</summary>
		public Padding CaptionMargin
		{
			get { return _CaptionMargin; }
			set {_CaptionMargin = value; }
		}
		
		public Code128BarcodeRenderer()
		{
			
		}
		
		private SizeF Measure(IEnumerable<Code128Barcode> barcodes)
		{
			SizeF size = new SizeF(0, 0);
			foreach (Code128Barcode barcode in barcodes)
			{
				SizeF currentBarcodeSize = Measure(barcode);
				if (size.Width < currentBarcodeSize.Width)
				{ size.Width = currentBarcodeSize.Width; }
				size.Height += currentBarcodeSize.Height;
			}
			return size;
		}
		
		/// <returns>Total size of a barcode and caption, with all padding (in points)</returns>
		private SizeF Measure(Code128Barcode barcode)
		{
			SizeF barcodeImageSize = MeasureBarcode(barcode.BarSpaceWidths);
			SizeF captionImageSize = MeasureCaption(barcode.Caption, barcodeImageSize.Width - BarcodeMargin.Horizontal);
			
			float totalWidth = Math.Max(barcodeImageSize.Width, captionImageSize.Width);
			float totalHeight = barcodeImageSize.Height + captionImageSize.Height;
			
			return new SizeF(totalWidth, totalHeight);
		}
		
		/// <returns>Total size of a caption, with all padding (in points)</returns>
		private SizeF MeasureCaption(string value, float maxWidth)
		{
			using (Bitmap b = new Bitmap(1, 1))
			{				
				using (Graphics g = Graphics.FromImage(b))
				{
					g.PageUnit = GraphicsUnit.Point;
					g.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;
					SizeF captionSize = g.MeasureString(value, CaptionFont.Value, new SizeF(maxWidth, 0));
					
					//Apply the padding
					captionSize.Width += CaptionMargin.Horizontal;
					captionSize.Height += CaptionMargin.Vertical;
					
					return captionSize;
				}
			}
		}
		
		/// <returns>Total size of a barcode, with all padding (in points)</returns>
		private SizeF MeasureBarcode(IEnumerable<int> barSpaceWidths)
		{			
			SizeF barcodeSize = new Size(0, BarcodeHeight);
			foreach(int barSpaceWidth in barSpaceWidths) 
				barcodeSize.Width += (barSpaceWidth * ModuleWidth);
			
			//Apply the padding
			barcodeSize.Width += BarcodeMargin.Horizontal;
			barcodeSize.Height += BarcodeMargin.Vertical;
			return barcodeSize;
		}
		
		public void Render(Graphics g, IEnumerable<Code128Barcode> barcodes, Point location)
		{
			Point currentPoint = new Point(location.X, location.Y);
			GraphicsUnit previousGraphicsUnit = g.PageUnit;
			TextRenderingHint previousTextRenderingHint = g.TextRenderingHint;
			
			try
			{
				g.PageUnit = GraphicsUnit.Point;
				g.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;
				
				//TODO: Ensure it will fit.
				
				foreach (Code128Barcode barcode in barcodes)
				{
					SizeF currentBarcodeSize = Measure(barcode);
					_Render(g, barcode, currentPoint);
					currentPoint.Y += (int)currentBarcodeSize.Height;
				}
			}
			finally
			{
				g.PageUnit = previousGraphicsUnit;
				g.TextRenderingHint = previousTextRenderingHint;				
			}
		}
		
		public void Render(Graphics g, Code128Barcode barcode, Point location)
		{ Render(g, new Code128Barcode[] { barcode }, location); }
		
		private void _Render(Graphics g, Code128Barcode barcode, Point location)
		{
			Point currentPoint = new Point(location.X + BarcodeMargin.Left, location.Y + BarcodeMargin.Top);
				
			for (int barSpaceIndex = 0; barSpaceIndex < barcode.BarSpaceWidths.Count; barSpaceIndex++)
			{
				int barSpaceWidth = ModuleWidth * barcode.BarSpaceWidths[barSpaceIndex];
				
				//If index is zero or even, it's a bar, so draw it
				if ((barSpaceIndex %2) == 0)
				{ 
					Rectangle r = new Rectangle(currentPoint.X, currentPoint.Y, barSpaceWidth, BarcodeHeight);
					g.FillRectangle(BarcodeBrush, r);
				}
				//Increment X value of the current point by the bar or space width
				currentPoint = new Point(currentPoint.X + barSpaceWidth, currentPoint.Y);
			}		
			
			currentPoint = new Point(location.X + CaptionMargin.Left, location.Y + BarcodeHeight + BarcodeMargin.Vertical + CaptionMargin.Top);
			g.DrawString(barcode.Caption, CaptionFont.Value, CaptionBrush, currentPoint.X, currentPoint.Y);
			g.Save();			
			
		}
		
		public Bitmap Generate(IEnumerable<Code128Barcode> barcodes, int dpi)
		{
			SizeF totalSizeInPoints = Measure(barcodes);
			SizeF totalSizeInPixels = new SizeF
			(
				totalSizeInPoints.Width / 72 * dpi
				, totalSizeInPoints.Height / 72 * dpi
			);
			Bitmap bitmap = new Bitmap((int)totalSizeInPixels.Width, (int)totalSizeInPixels.Height);
			
			bitmap.SetResolution(dpi, dpi);
			
			using (Graphics g = Graphics.FromImage(bitmap))
			{
				g.Clear(Color.White);
				Render(g, barcodes, new Point(0, 0));
			}
			bitmap.SetResolution(dpi, dpi);
			return bitmap;
		}
		
		public Bitmap Generate(Code128Barcode barcode, int dpi)
		{ return Generate(new Code128Barcode[] { barcode }, dpi); }
		
	}
}