/*
 * User: Sam Brinly
 * Date: 1/29/2013
 */
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using EdocsUSA.Controls;
using EdocsUSA.Utilities;
using EDL = EdocsUSA.Utilities.Logging;
using FreeImageAPI;
using Scanquire.Public;
using Scanquire.Public.Extensions;
using Microsoft;
using ScanquireLogin;

namespace Scanquire
{
    internal sealed class Program
    {
      //  const int EvalDays = 30;
        [STAThread]
        
        private static void Main(string[] args)
        {

            Form applicationForm = null;
            try
            {
             
                //Create the directory for managing temporary files.
                Directory.CreateDirectory(SettingsManager.TempDirectoryPath);
                
                //  string TraceFilePath = Path.Combine(SettingsManager.TempDirectoryPath, string.Format("ScanQuire_{0}.log", DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")));
                string TraceFilePath = string.Empty;
                if (Scanquire.Properties.Scanquire.Default.AuditLogs)
                {
                    
                   // TraceFilePath = Path.Combine(SettingsManager.AuditLogsDirectroy, string.Format("ScanQuire_{0}.log", DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")));
                    TraceFilePath = EDL.TraceLogger.TraceLoggerInstance.GetTraceFileName(SettingsManager.AuditLogsDirectroy, "ScanQuire", Scanquire.Properties.Scanquire.Default.DaysToKeepLogFiles, "*.*", SettingsManager.AuditLogsUploadDirectroy);
                    Edocs_Utilities.EdocsUtilitiesInstance.CopyFiles(SettingsManager.AuditLogsDirectroy, SettingsManager.AuditLogsUploadDirectroy, true, "*.*");
                }
                else
                {
                    TraceFilePath= EDL.TraceLogger.TraceLoggerInstance.GetTraceFileName(SettingsManager.TempDirectoryPath, "ScanQuire", Scanquire.Properties.Scanquire.Default.DaysToKeepLogFiles,"*.*", SettingsManager.TempDirectoryPath);
                }

                EDL.TraceLogger.TraceLoggerInstance.TotalImages = 0;
               
                //  Trace.Listeners.Remove("LOGFILE");
                //string TraceFilePath = Path.Combine(SettingsManager.TempDirectoryPath, string.Format("{0}_{1}_sessionlog.txt", Environment.UserName, DateTime.Now.ToString("yyyy_MM_dd_mm_ss")));
                EDL.TraceLogger.TraceLoggerInstance.RunningAssembley = Assembly.GetEntryAssembly().GetName().Name;
                //Initialize the trace file listener
                //TextWriterTraceListener textFileListener = EDL.TraceLogger.TraceLoggerInstance.OpenTextWriterTraceListener(TraceFilePath, "SCANQUIRELOGFILE");
                //Trace.Listeners.Add(textFileListener);
                // TextWriterTraceListener textFileListener = new TextWriterTraceListener(TraceFilePath, "LOGFILE");
                // Trace.Listeners.Add(textFileListener);
                EDL.TraceLogger.TraceLoggerInstance.OpenTraceLogFile(TraceFilePath, "SCANQUIRELOGFILE"); 
                EDL.TraceLogger.TraceLoggerInstance.WriteTraceHeader();
                EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Loading ScanQuire using log file:{TraceFilePath}");
                if (!(Edocs_Utilities.EdocsUtilitiesInstance.CloseRunningProcess(EDL.TraceLogger.TraceLoggerInstance.RunningAssembley)))
                {
                    string message = string.Format("Cound not stop other instance of {0} call e-Docs Support @406-565-3465.", Process.GetCurrentProcess().ProcessName);
                    EDL.TraceLogger.TraceLoggerInstance.TraceError(message);
                    MessageBox.Show(message);
                    return;
                }
                //DateTime dtEval = DateTime.Now.AddDays(EvalDays);
                //if(DateTime.Now >= dtEval)
                //{
                //    MessageBox.Show("Evaluation period expired");
                //    return;

                //}
                //if (Directory.Exists(SQImageRevision.ImageFilesDirectory))
               // {
                    try
                    {
                        EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Deleting files for Folder {SQImageRevision.ImageFilesDirectory}");
                        Edocs_Utilities.EdocsUtilitiesInstance.CleanUpLogFiles(SQImageRevision.ImageFilesDirectory, Scanquire.Properties.Scanquire.Default.DaysToKeepImageFiles, "*.*");
                        //   Directory.Delete(SQImageRevision.ImageFilesDirectory, true);
                    }
                    catch (Exception ex)
                    {
                        EDL.TraceLogger.TraceLoggerInstance.TraceError($"Deleting files for folder {SQImageRevision.ImageFilesDirectory} {ex.Message.Replace(",", " ")}");
                    }
                //}


               
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
           
                //Set the default Icon of all InputDialogs to the e-Docs E.
                InputDialogBase.DefaultIcon = Icons.E;
               
                applicationForm = new MainForm();
                
                
                //  DecriptPw 

                //Add exception handling.
                Application.ThreadException += delegate (object sender, ThreadExceptionEventArgs e)
                {
                    EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Thread Exception");

                    if (e.Exception is OperationCanceledException)
                    {
                        EDL.TraceLogger.TraceLoggerInstance.TraceWarning("User canceled operation");
                    }
                    else
                    {
                        string errorMessage = e.Exception.Message;
                        Trace.TraceError($"Log Time {DateTime.Now.ToString()},{Environment.UserName},{Environment.MachineName},{Environment.UserDomainName},error message {e.Exception.Message}");
                        Trace.TraceError($"Log Time {DateTime.Now.ToString()},{Environment.UserName},{Environment.MachineName},{Environment.UserDomainName},stack trace {e.Exception.StackTrace}");
                        MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };

                AppDomain.CurrentDomain.UnhandledException += delegate (object sender, UnhandledExceptionEventArgs e)
                {

                    Trace.TraceInformation($"Log Time {DateTime.Now.ToString()},{Environment.UserName},{Environment.MachineName},{Environment.UserDomainName},App domain exception");
                    Exception ex = (Exception)e.ExceptionObject;
                    Trace.TraceError($"Log Time {DateTime.Now.ToString()},{Environment.UserName},{Environment.MachineName},{Environment.UserDomainName},error message {ex.Message}");
                    Trace.TraceError($"Log Time {DateTime.Now.ToString()},{Environment.UserName},{Environment.MachineName},{Environment.UserDomainName},stack trace {ex.StackTrace}");
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                };

                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error launching application " + ex.Message);
                Trace.TraceError($"Log Time {DateTime.Now.ToString()},{Environment.UserName},{Environment.MachineName},{Environment.UserDomainName},Error launching application message {ex.Message}");
                Trace.TraceError($"Log Time {DateTime.Now.ToString()},{Environment.UserName},{Environment.MachineName},{Environment.UserDomainName},Error launching application stack trace {ex.StackTrace}");
#if DEBUG
                MessageBox.Show(ex.StackTrace);
#endif

               // Application.Exit();
            }
            Application.Run(applicationForm);
            //Launch the main form

        }

      
    }
}
