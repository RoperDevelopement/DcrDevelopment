using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace EdocsUSA.Utilities.Controls
{
	[ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.MenuStrip |
                                       ToolStripItemDesignerAvailability.ContextMenuStrip | 
                                       ToolStripItemDesignerAvailability.StatusStrip)]
 	public class ProgressBarExToolstripItem : ToolStripControlHost
	{
 		protected ProgressBarEx ProgressBarEx;
 		
 		private void InitializeComponent()
 		{ this.ProgressBarEx = this.Control as ProgressBarEx; }
 		
 		public ProgressBarExToolstripItem() : base(new ProgressBarEx())
		{ InitializeComponent(); }
	}
 	
	
}
