using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EdocsUSA.Utilities;
using System.Threading;
using Microsoft;
using System.Diagnostics;
using ETL = EdocsUSA.Utilities.Logging.TraceLogger;
namespace Scanquire.Public.UserControls
{
    /// <summary>Progress bar to monitor progress from a Progress and respond to CancelationToken requests.</summary>
    public partial class ProgressMonitor : UserControl
    {
        public enum StopMonitoringReason
        {
            None,
            ProcessCompleted,
            ProcessFailed,
            ProcessCanceled,
            ProcessAutoQaComplete,
            ProcessRunningAutoQa,
            StartMarkBlankDocuments,
            EndMarkBlankDocuments,
            EditingImages,
            StartProcess,
            ProcessRunningConvertImagesToViedo,
            ProcessRunningConvertToViedoImages

        }

        public BusyStatus BusyStatus = new BusyStatus();

        protected CancellationTokenSource CancellationTokenSource = null;

        protected bool _SupportsCancellation = false;
        /// <summary>If true, monitors the the cancelation token.</summary>
        public bool SupportsCancellation
        {
            get { return _SupportsCancellation; }
            set
            {
                _SupportsCancellation = value;
                OnSupportsCancellationChanged();
            }
        }

        public ProgressMonitor()
        { InitializeComponent(); }

        protected void OnSupportsCancellationChanged()
        { this.CancelTaskButton.Visible = (SupportsCancellation == true); }

        /// <remarks><see cref="StopMonitoring">Must be called before you can monitor anything else.</see>/></remarks>
        public void StartMonitoring(Microsoft.Progress<ProgressEventArgs> progress, CancellationTokenSource cancelSource)
        {
            ETL.TraceLoggerInstance.TraceInformation("Start monitoring setting busy to true");
            BusyStatus.Set();
            CancellationTokenSource = cancelSource;

            ProgressBar.BarColor = SystemColors.ControlDark;
            ProgressBar.Value = 0;
            CancelTaskButton.Enabled = (SupportsCancellation == true);

            progress.ProgressChanged += ProgressChanged;
        }

     



        /// <remarks><see cref="StopMonitoring">Must be called before you can monitor anything else.</see>/></remarks>
        public void StartMonitoring<T>(Progress<ProgressEventArgs<T>> progress, CancellationTokenSource cancelSource)
        {
            ETL.TraceLoggerInstance.TraceInformation("Start monitoring setting busy to true");
            BusyStatus.Set();
            CancellationTokenSource = cancelSource;

            ProgressBar.BarColor = SystemColors.ControlDark;
            ProgressBar.Value = 0;
            CancelTaskButton.Enabled = (SupportsCancellation == true);

            progress.ProgressChanged += ProgressChanged;
        }

        void ProgressChanged(object sender, ProgressEventArgs e)
        {
            ProgressBar.Value = e.PercentComplete;
            ProgressBar.Caption = e.Caption;
        }

        void ProgressChanged<T>(object sender, ProgressEventArgs<T> e)
        {
            ProgressBar.Value = e.PercentComplete;
            ProgressBar.Caption = e.Caption;
        }

        protected void SetProgressBarCompletedColor(StopMonitoringReason reason)
        {
            switch (reason)
            {
                case StopMonitoringReason.None:
                    ProgressBar.BarColor = SystemColors.ControlDark;
                    ProgressBar.Caption = "";
                    ETL.TraceLoggerInstance.TraceInformation("Stop monitoring reason none");
                    break;
                case StopMonitoringReason.ProcessCanceled:
                    ProgressBar.BarColor = Color.Orange;
                    ProgressBar.Caption = "Canceled";
                    ETL.TraceLoggerInstance.TraceInformation("Stop monitoring reason Canceled");
                    break;
                case StopMonitoringReason.ProcessFailed:
                    ProgressBar.BarColor = Color.Red;
                    ProgressBar.Caption = "Error";
                    ETL.TraceLoggerInstance.TraceInformation("Stop monitoring reason Error");
                    break;
                case StopMonitoringReason.ProcessCompleted:
                    ProgressBar.BarColor = Color.LightGreen;
                    ETL.TraceLoggerInstance.TraceInformation("Stop monitoring reason Complete");
                    ProgressBar.Caption = "Complete";
                    break;
                case StopMonitoringReason.ProcessAutoQaComplete:
                    ProgressBar.BarColor = Color.AliceBlue;
                    ProgressBar.Caption = "AutoQa Complete";
                    ETL.TraceLoggerInstance.TraceInformation("Stop monitoring reason AutoQa Complete");
                    break;
                case StopMonitoringReason.ProcessRunningAutoQa:
                    ProgressBar.BarColor = SystemColors.HighlightText;
                    ProgressBar.Caption = "Running AutoQA";
                    ETL.TraceLoggerInstance.TraceInformation("Start monitoring reason Running AutoQa");
                    break;
                case StopMonitoringReason.ProcessRunningConvertImagesToViedo:
                    ProgressBar.BarColor = SystemColors.HighlightText;
                    ProgressBar.Caption = "Convert Images To Viedo";
                    ETL.TraceLoggerInstance.TraceInformation("Start monitoring reason Running ConvertImagesToViedo");
                    break;
                case StopMonitoringReason.ProcessRunningConvertToViedoImages:
                    ProgressBar.BarColor = SystemColors.HighlightText;
                    ProgressBar.Caption = "Convert Images To Viedo";
                    ETL.TraceLoggerInstance.TraceInformation("Start monitoring reason Running RunningConvertToViedoImages");
                    break;
                case StopMonitoringReason.StartMarkBlankDocuments:
                    ProgressBar.BarColor = SystemColors.HighlightText;
                    ProgressBar.Caption = "Marking Blank Documents";
                    ETL.TraceLoggerInstance.TraceInformation("Start Marking Blank Documents");
                    break;
                case StopMonitoringReason.EndMarkBlankDocuments:
                    ProgressBar.BarColor = Color.Wheat;
                    ProgressBar.Caption = "Done Documents";
                    ETL.TraceLoggerInstance.TraceInformation("Done Marking Blank Documents");
                    break;
                case StopMonitoringReason.EditingImages:
                    ProgressBar.BarColor = Color.DarkGreen;
                    ProgressBar.Caption = "Done UpDating Images";
                    ETL.TraceLoggerInstance.TraceInformation("Updating Images");
                    break;
                case StopMonitoringReason.StartProcess:
                    ProgressBar.BarColor = Color.Yellow;
                    ProgressBar.Caption = "Starting Process";
                    ETL.TraceLoggerInstance.TraceInformation("Starting Process");
                    break;

                default:
                    ProgressBar.BarColor = SystemColors.ControlDark;
                    ETL.TraceLoggerInstance.TraceInformation("Stop monitoring reason Unknown");
                    ProgressBar.Caption = "Unknown";
                    break;
            }
        }

        protected void SetProgressBarCompletedColor(string reason,Color colors)
        {
            
                    ProgressBar.BarColor =colors;
                    ProgressBar.Caption = reason;
                    ETL.TraceLoggerInstance.TraceInformation("Stop monitoring reason none");
            
        }




        public void StopMonitoring(Progress<ProgressEventArgs> progress, StopMonitoringReason reason = StopMonitoringReason.None)
        {
            ETL.TraceLoggerInstance.TraceInformation("Stopping monitoring for " + reason);
            ProgressBar.Value = 100;
            progress.ProgressChanged -= ProgressChanged;
            SetProgressBarCompletedColor(reason);
            CancellationTokenSource = null;
            CancelTaskButton.Enabled = false;
            BusyStatus.Clear();
        }
        public void StopMonitoring(Progress<ProgressEventArgs> progress, string reaon, Color barColor)
        {
            ETL.TraceLoggerInstance.TraceInformation("Stopping monitoring for " + reaon);
            ProgressBar.Value = 100;
            progress.ProgressChanged -= ProgressChanged;
            SetProgressBarCompletedColor(reaon, barColor);
            CancellationTokenSource = null;
            CancelTaskButton.Enabled = false;
            BusyStatus.Clear();
        }

        public void StopMonitoring<T>(Progress<ProgressEventArgs<T>> progress, StopMonitoringReason reason = StopMonitoringReason.None)
        {
            ETL.TraceLoggerInstance.TraceInformation("Stopping monitoring for " + reason);

            ProgressBar.Value = 100;
            progress.ProgressChanged -= ProgressChanged;
            SetProgressBarCompletedColor(reason);
            CancellationTokenSource = null;
            CancelTaskButton.Enabled = false;
            BusyStatus.Clear();
        }

       
    }
}
