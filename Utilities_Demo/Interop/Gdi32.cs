using System;
using System.Runtime.InteropServices;

namespace EdocsUSA.Utilities.Interop
{
	public static class Gdi
	{
		[DllImport("gdi32.dll")]
		public static extern bool DeleteObject(IntPtr hObject);
	}
}
