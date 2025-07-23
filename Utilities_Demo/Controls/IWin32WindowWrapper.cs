/*
 * Created by SharpDevelop.
 * User: Sam Brinly
 * Date: 3/21/2011
 * Time: 11:55 AM
 */
using System;
using System.Windows.Forms;

namespace EdocsUSA.Controls
{
	public class IWin32WindowWrapper : IWin32Window
    {
        private IntPtr _hwnd;

        public IWin32WindowWrapper(IntPtr handle) { _hwnd = handle; }

        public IntPtr Handle { get { return _hwnd; } }
    }
}
