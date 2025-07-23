using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BinMonitor.Common
{
    /// <summary>A text box that autohighlights on entry.</summary>
    /// <credit>
    /// http://stackoverflow.com/questions/97459/automatically-select-all-text-on-focus-in-winforms-textbox
    /// </credit>
    public class HighlightingTextBox : TextBox
    {
        private bool _focused;

        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            if (MouseButtons == MouseButtons.None)
            {
                SelectAll();
                _focused = true;
            }
        }

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            _focused = false;
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            base.OnMouseUp(mevent);
            if (!_focused)
            {
                if (SelectionLength == 0)
                    SelectAll();
                _focused = true;
            }
        }
    }
}
