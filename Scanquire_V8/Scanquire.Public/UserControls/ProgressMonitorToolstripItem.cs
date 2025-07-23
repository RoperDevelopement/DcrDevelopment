using EdocsUSA.Utilities;
using Microsoft;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Scanquire.Public.UserControls
{
    /// <summary>
    /// Wrapper to allow a ProgressMonitor to be placed on a ToolStrip.
    /// </summary>
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.MenuStrip |
                                       ToolStripItemDesignerAvailability.ContextMenuStrip |
                                       ToolStripItemDesignerAvailability.StatusStrip)]
    [DefaultProperty("Value")]
    public class ProgressMonitorToolstripItem : ToolStripControlHost
    {
        public ProgressMonitor Value;

        private void InitializeComponent()
        { this.Value = this.Control as ProgressMonitor; }

        public ProgressMonitorToolstripItem()
            : base(new ProgressMonitor())
        { InitializeComponent(); }

        public void StartMonitoring(Progress<ProgressEventArgs> progress, CancellationTokenSource cancelSource)
        { Value.StartMonitoring(progress, cancelSource); }

        public void StartMonitoring<T>(Progress<ProgressEventArgs<T>> progress, CancellationTokenSource cancelSource)
        { Value.StartMonitoring(progress, cancelSource); }

        public void StopMonitoring(Progress<ProgressEventArgs> progress, ProgressMonitor.StopMonitoringReason reason = ProgressMonitor.StopMonitoringReason.None)
        { Value.StopMonitoring(progress, reason); }

        public void StopMonitoring(Progress<ProgressEventArgs> progress, string reason,Color color)
        { Value.StopMonitoring(progress, reason,color); }

        public void StopMonitoring<T>(Progress<ProgressEventArgs<T>> progress, ProgressMonitor.StopMonitoringReason reason = ProgressMonitor.StopMonitoringReason.None)
        { Value.StopMonitoring(progress, reason); }

        protected override void OnHostedControlResize(EventArgs e)
        {
            base.OnHostedControlResize(e);
            this.Size = Value.Size;
        }
    }
}
