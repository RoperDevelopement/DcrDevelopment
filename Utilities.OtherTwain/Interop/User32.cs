using System;
using System.Runtime.InteropServices;

namespace EdocsUSA.Utilities.Interop
{
	public static class User32
	{
		[DllImport("user32.dll")]
		public static extern uint GetMessagePos();
		
		[DllImport("user32.dll")]
		public static extern int GetMessageTime();
		
		[DllImport("user32.dll")]
		public static extern int ShowScrollBar(IntPtr hwnd, int wBar, bool bShow);
		
		public enum ScrollBar
		{
			SB_HORZ = 0,
			SB_VERT = 1,
			SB_CTL = 2,
			SB_BOTH = 3,
		}
	}
}
