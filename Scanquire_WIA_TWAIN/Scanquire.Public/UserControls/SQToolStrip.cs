using System;
using System.Windows.Forms;

using EdocsUSA.Controls;

namespace Scanquire.Public.UserControls
{
    /// <summary>A non-selectable ToolStrip that defaults to a borderless Professional render mode.</summary>
	public class SQToolStrip : ToolStrip
	{
		public SQToolStrip()
		{
            if (this.DesignMode == false)
            { SetStyle(ControlStyles.Selectable, false); }
			Renderer = new BorderlessToolstripRenderer();
			RenderMode = ToolStripRenderMode.Professional;
		}
	}
}
