using EdocsUSA.Utilities;
using EdocsUSA.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ETL = EdocsUSA.Utilities.Logging.TraceLogger;
namespace Scanquire.Public.UserControls
{
    /// <summary>
    /// Dialog to view the Trace history for a selected TextWriterTraceListener.
    /// </summary>
    public partial class SessionLogViewer : Form
    {
        //TODO: Implement Save As File / Email To Support

        /// <summary>List of available TextWriterTraceListeners that are writing to a FileStream.</summary>
        protected readonly SynchronizedBindingList<KeyValuePair<string, string>> FileListeners;

        public SessionLogViewer()
        {
            InitializeComponent();
            ETL.TraceLoggerInstance.TraceInformation($"Opening dialog{this.Name}");
            FileListeners = new SynchronizedBindingList<KeyValuePair<string, string>>(this);
            TraceListenerComboBox.DisplayMember = "Key";
            TraceListenerComboBox.ValueMember = "Value";
            TraceListenerComboBox.DataSource = FileListeners;
        }

        protected async override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            
            if (this.IsInDesignMode() == false)
            {
                //Refresh the listener list in case any were dropped or added.
                await LoadListeners();
            }
        }

        private async void TraceListenerComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            string selectedPath = (string)TraceListenerComboBox.SelectedValue;
            if (string.IsNullOrWhiteSpace(selectedPath))
            {
                ETL.TraceLoggerInstance.TraceInformation("Ignoring, selectedPath is empty");
                SessionLogTextBox.Clear();
                return;
            }
            if (File.Exists(selectedPath) == false)
            {
                ETL.TraceLoggerInstance.TraceWarning("selectedPath does not exist " + selectedPath);
                MessageBox.Show("Could not locate TraceListener's file path " + selectedPath);
                SessionLogTextBox.Clear();
                return;
            }
            ETL.TraceLoggerInstance.TraceWarning("Loading session log for path " + selectedPath);
            await LoadSessionLog(selectedPath);
        }

        /// <summary>Populate Listeners with all TextWriterTraceListeners that output to files.</summary>
        protected async Task LoadListeners()
        {
            this.Enabled = false;
            try
            {
                FileListeners.Clear();
                //First add an empty value to avoid un-needed loading.
                FileListeners.Add(new KeyValuePair<string, string>("Please select a TraceListener", null));
                await Task.Factory.StartNew(() =>
                {
                    foreach (TraceListener listener in Trace.Listeners)
                    {
                        string listenerName = listener.Name;
                        string listenerFilePath;
                        if (TryGetListenerFilePath(listener, out listenerFilePath) == true)
                        { FileListeners.Add(new KeyValuePair<string, string>(listenerName, listenerFilePath)); }
                    }
                });
            }
            finally
            { this.Enabled = true; }
        }

        /// <summary>Attempt to get the output filepath for a TraceListener.</summary>
        /// <param name="path">
        /// On False: null.
        /// On True: The absolute path to the file being written to by the specified TraceListener.
        /// </param>
        /// <returns>
        /// False if listener is not a TextWriterTraceListener that is outputting to a file.
        /// True if listener is a TextWriterTraceListener that is outputting to a file.
        /// </returns>
        protected bool TryGetListenerFilePath(TraceListener listener, out string path)
        {
            if (listener == null)
            { 
               ETL.TraceLoggerInstance.TraceWarning("Listener is null");
                path = null;
                return false;
            }

            //Ensure that it's a TextWriterTraceListener
            string listenerName = listener.Name;
            if ((listener is TextWriterTraceListener) == false)
            {
                ETL.TraceLoggerInstance.TraceInformation("Skipping " + listenerName + " -- not a TextWriterTraceListener");
                path = null;
                return false;
            }
            TextWriterTraceListener twListener = (TextWriterTraceListener)listener;
            
            //Ensure that its writer is a streamwriter
            if ((twListener.Writer is StreamWriter) == false)
            {
                ETL.TraceLoggerInstance.TraceInformation("Skipping " + listenerName + " -- Writer is not a StreamWriter");
                path = null;
                return false;
            }
            StreamWriter streamWriter = (StreamWriter)(twListener.Writer);
            
            //Ensure that the stream writer's base stream is a file stream
            if ((streamWriter.BaseStream is FileStream) == false)
            {
                ETL.TraceLoggerInstance.TraceInformation("Skipping " + listenerName + " -- StreamWriter is not a FileStream"); 
                path = null;
                return false;
            }
            FileStream fileStream = (FileStream)(streamWriter.BaseStream);

            //Get the path to the file (from the name property).
            string traceFilePath = fileStream.Name;
            if (File.Exists(traceFilePath) == false)
            {
                ETL.TraceLoggerInstance.TraceInformation("Skipping " + listenerName + " -- File not found " + traceFilePath);
                path = null;
                return false;
            }

            path = traceFilePath;
            ETL.TraceLoggerInstance.TraceInformation("Retuing listiner file path " +path);
            return true;
        }

        /// <summary>Populate SessionLogTextBox with the contents of the specified trace file.</summary>
        /// <remarks>File is opened with read only access, so it should not interfere with continued tracing operations.</remarks>
        protected async Task LoadSessionLog(string traceFilePath)
        {
            this.Enabled = false;
            try
            {
                SessionLogTextBox.Clear();
                string traceContents = string.Empty;
                await Task.Factory.StartNew(() =>
                {
                    using (FileStream stream = new FileStream(traceFilePath, FileMode.Open, FileAccess.Read, System.IO.FileShare.ReadWrite))
                    using (StreamReader reader = new StreamReader(stream))
                    { traceContents = reader.ReadToEnd(); }
                });
                SessionLogTextBox.Text = traceContents;
                //Highlight "warning" in orange
                 SessionLogTextBox.HighlightText("WARNING", Color.Orange, StringComparison.OrdinalIgnoreCase);
                //SessionLogTextBox.HighlightText("Information", Color.Orange, StringComparison.OrdinalIgnoreCase);
                //Highlight "error" in red
                SessionLogTextBox.HighlightText("ERROR", Color.Red, StringComparison.OrdinalIgnoreCase);
            }
            finally
            { this.Enabled = true; }

        }
    }
}
