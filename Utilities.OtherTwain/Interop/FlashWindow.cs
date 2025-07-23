/*
 * User: Sam Brinly
 * Date: 7/23/2014
 */
using System;
using System.Runtime.InteropServices;

namespace EdocsUSA.Utilities.Interop
{
	
	 public static class FlashWindow
        {
            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            static extern bool FlashWindowEx(ref FLASHWINFO pwfi);
            [DllImport("kernel32.dll")]
            static extern IntPtr GetConsoleWindow();

            [StructLayout(LayoutKind.Sequential)]
            public struct FLASHWINFO
            {
                public UInt32 cbSize;
                public IntPtr hwnd;
                public UInt32 dwFlags;
                public UInt32 uCount;
                public UInt32 dwTimeout;
            }

            //Stop flashing. The system restores the window to its original state. 
            public const UInt32 FLASHW_STOP = 0;
            //Flash the window caption. 
            public const UInt32 FLASHW_CAPTION = 1;
            //Flash the taskbar button. 
            public const UInt32 FLASHW_TRAY = 2;
            //Flash both the window caption and taskbar button.
            //This is equivalent to setting the FLASHW_CAPTION | FLASHW_TRAY flags. 
            public const UInt32 FLASHW_ALL = 3;
            //Flash continuously, until the FLASHW_STOP flag is set. 
            public const UInt32 FLASHW_TIMER = 4;
            //Flash continuously until the window comes to the foreground. 
            public const UInt32 FLASHW_TIMERNOFG = 12;

            public static void Flash(UInt32 flags)
            {
                FLASHWINFO fInfo = new FLASHWINFO();

                fInfo.cbSize = Convert.ToUInt32(Marshal.SizeOf(fInfo));
                fInfo.hwnd = GetConsoleWindow();
                fInfo.dwFlags = flags;
                fInfo.uCount = UInt32.MaxValue;
                fInfo.dwTimeout = 0;

                FlashWindowEx(ref fInfo);
            }
        }
}
