/*
 * Created by SharpDevelop.
 * User: Sam Brinly
 * Date: 5/23/2011
 * Time: 8:38 AM
 */
 
using System;
using System.Windows.Forms;

namespace EdocsUSA.Controls
{
	public class BorderlessToolstripRenderer : ToolStripSystemRenderer
	{
		protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e) { }
	}
}
