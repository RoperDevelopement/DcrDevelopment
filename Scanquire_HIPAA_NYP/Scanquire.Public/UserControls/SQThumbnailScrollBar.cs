using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Scanquire.Public.UserControls
{
    /// <summary>Unselectable scrollbar for use with the SQThumbnailPanel</summary>
    public class SQThumbnailScrollBar : VScrollBar
    {
        public SQThumbnailScrollBar()
        { SetStyle(ControlStyles.Selectable, false); }
    }
}
