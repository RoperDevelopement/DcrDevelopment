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
 	public class ProgressMonitorToolstripItem : ToolStripControlHost
	{
 		protected ProgressMonitor ProgressMonitor;
 		
 		private void InitializeComponent()
 		{ this.ProgressMonitor = this.Control as ProgressMonitor; }
 		
 		public ProgressMonitorToolstripItem() : base (new ProgressMonitor())
		{ InitializeComponent(); }
		
		public void StartMonitoring(Task task, Progress progress, CancellationTokenSource cancellationTokenSource)
		{ ProgressMonitor.StartMonitoring(task, progress, cancellationTokenSource); }
	}
}
