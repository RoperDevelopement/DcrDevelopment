using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using EdocsUSA.Utilities;
using EDL = EdocsUSA.Utilities.Logging;
using Edocs.HelperUtilities;
using System.Reflection;
namespace ManualIndexingTemp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            
            try
            {
                string batchId = GetCommandLineArgument(args, "/batchid:");
                if (string.IsNullOrWhiteSpace(batchId))
                { throw new ArgumentNullException("batch id required"); }
              
                bool requiresRequisitionNumber = false;
              //  bool.TryParse(GetCommandLineArgument(args, "/RequiresRequisitionNumber:"), out requiresRequisitionNumber);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Form1 f = new Form1();
                f.TraceFolder = $"{Path.Combine(f.LogFolder, batchId)}.log";
                if(Properties.Settings.Default.AuditLogFolder)
                {
                    f.TraceFolder = Path.Combine(SettingsManager.AuditLogsDirectroy, string.Format("ManualIndexing_{0}_{1}.log", batchId, DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")));
                    f.TraceFolder = f.TraceFolder.Replace("ManualIndexingTemp", "e-Docs USA");
                }
                Edocs.HelperUtilities.Utilities.CreateDirectory(f.TraceFolder);
               
                EDL.TraceLogger.TraceLoggerInstance.RunningAssembley = Assembly.GetEntryAssembly().GetName().Name;
                EDL.TraceLogger.TraceLoggerInstance.OpenTraceLogFile(f.TraceFolder, "ManuelIndexing");
                EDL.TraceLogger.TraceLoggerInstance.WriteTraceHeader();
                EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Starting Manual indexing batch id: {batchId} for assembly {EDL.TraceLogger.TraceLoggerInstance.RunningAssembley}");
                f.BatchId = batchId;
                f.RequiresRequisitionNumber = requiresRequisitionNumber;
             //   f.RequiresRequisitionNumber = false;
              Application.Run(f);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Running manuel indexing {ex.Message}");
                EDL.TraceLogger.TraceLoggerInstance.CloseTraceFile();
                Environment.ExitCode = -1;
                Application.Exit();
            }
            //EDL.TraceLogger.TraceLoggerInstance.Dispose();
           
        }

        static string GetCommandLineArgument(string[] args, string def)
        {
            foreach (string arg in args)
            {
                if (arg.StartsWith(def, StringComparison.OrdinalIgnoreCase))
                { return arg.Substring(def.Length); }
            }
            return null;
        }
    }
}
