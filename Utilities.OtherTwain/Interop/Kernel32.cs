using System;
using System.Runtime.InteropServices;
using System.Text;

namespace EdocsUSA.Utilities.Interop
{
	public static class Kernel32
	{
		[DllImport("kernel32.dll")]
		public static extern IntPtr GlobalLock(IntPtr hMem);
	
		[DllImport("kernel32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GlobalUnlock(IntPtr hMem);
	
		[DllImport("kernel32.dll")]
		public static extern IntPtr GlobalAlloc(uint uFlags, UIntPtr dwBytes);
		
		[DllImport("kernel32.dll")]
		public static extern IntPtr GlobalAlloc(uint uFlags, int size);
	
		[DllImport("kernel32.dll")]
		public static extern IntPtr GlobalFree(IntPtr hMem);
		
		[DllImport("kernel32.dll", CharSet=CharSet.Unicode, SetLastError=true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool WritePrivateProfileString(string lpAppName, string lpKeyName, string lpString, string lpFileName);
		
		[DllImport("kernel32.dll", CharSet=CharSet.Unicode)]
		public static extern uint GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, uint nSize, string lpFileName);
		
		
	}
}
