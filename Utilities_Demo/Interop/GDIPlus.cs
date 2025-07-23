using System;
using System.Runtime.InteropServices;

namespace EdocsUSA.Utilities.Interop
{
	public static class GDIPlus
	{
		[DllImport("GdiPlus.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
		public static extern int GdipCreateBitmapFromGdiDib(IntPtr pBIH, IntPtr pPix, out IntPtr pBitmap);
		
		[DllImport("GdiPlus.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
		public static extern int GdipCreateHBITMAPFromBitmap(IntPtr pBmp, out IntPtr pHBmp, int background);
		
		[DllImport("GdiPlus.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
		public static extern int GdipDisposeImage(IntPtr pImg);
		
		public enum Status
		{
			Ok = 0,
			GenericError = 1,
			InvalidParameter = 2,
			OutOfMemory = 3,
			ObjectBusy = 4,
			InsufficientBuffer = 5,
			NotImplemented = 6,
			Win32Error = 7,
			WrongState = 8,
			Aborted = 9,
			FileNotFound = 10,
			ValueOverflow = 11,
			AccessDenied = 12,
			UnknownImageFormat = 13,
			FontFamilyNotFound = 14,
			FontStyleNotFound = 15,
			NotTrueTypeFont = 16,
			UnsupportedGdiplusVersion = 17,
			GdiplusNotInitialized = 18,
			PropertyNotFound = 19,
			PropertyNotSupported = 20,
			ProfileNotFound = 21
		}
	}
}
