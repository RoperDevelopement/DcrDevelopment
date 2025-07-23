using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Configuration;
//using System.Diagnostics.Process;
//using EdocsUSA.Utilities;
using edl = EdocsUSA.Utilities.Logging;
using System.Diagnostics;
//C:\Program Files (x86)\e-Docs USA\ScanQuire_V7.0\Scanquire_HIPAA.exe
namespace Edocs.Restart.ShutDown.Process
{
    class ShutDownRestartProcess
    {
        private readonly static string ArgStopProcess = "/stopproc:";
        private readonly static string ArgStartProcess = "/startproc:";
        private readonly static string AppConfigKeyLoggingFolder = "LogFolder";
        private static string traceLog = string.Empty;
        private readonly static string AppConfigKeyLogUpLoadFolder = "LogUpLoadFolder";

        static void Main(string[] args)
        {
          try
                {
            OpenTraceLog();

            GetInputArgs(args);
            CopyAuditLogs();
            }
            catch 
            { }
        }

        private static string LogFolder
        {
            get
            {
                string lFolder = GetAppConfigSetting(AppConfigKeyLoggingFolder);
                if (string.IsNullOrWhiteSpace(lFolder))
                {
                    lFolder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AssemblyInfo.GetAssemblyTitle());
                }
                return lFolder;
            }

            //  get { return (Properties.Settings.Default.LogFolder.Replace("{ApplicationDir}", Environment.GetFolderPath//(Environment.SpecialFolder.ApplicationData)).Replace("{ApplicarionName}", AssemblyInfo.GetAssemblyTitle())); }
        }
        private static string LogUpLoadFolder
        {
            get
            {

                return GetAppConfigSetting(AppConfigKeyLogUpLoadFolder);
            }

            //  get { return (Properties.Settings.Default.LogFolder.Replace("{ApplicationDir}", Environment.GetFolderPath//(Environment.SpecialFolder.ApplicationData)).Replace("{ApplicarionName}", AssemblyInfo.GetAssemblyTitle())); }
        }
        private static void CloseTraceLog()
        {
            edl.TraceLogger.TraceLoggerInstance.TraceInformation("Closing trace logging file");
            edl.TraceLogger.TraceLoggerInstance.CloseTraceFile();
        }
        private static void CopyAuditLogs()
        {
            try
            {

                edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Copying audit log {traceLog}");
                edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Copying audit log {traceLog}");
                CloseTraceLog();
                string destFolder = LogUpLoadFolder;
                if (!(string.IsNullOrWhiteSpace(destFolder)))
                {
                    destFolder = Path.Combine(destFolder, Path.GetFileName(traceLog));
                    edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Copying audit log {traceLog} to {destFolder}");
                    edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Copying audit log {traceLog} to {destFolder}");
                    File.Copy(traceLog, destFolder, true);
                    File.Delete(traceLog);
                }
                //   Edocs_Utilities.EdocsUtilitiesInstance.CopyFile(traceLog, EdocsUSA.Utilities.SettingsManager.AuditLogsUploadDirectroy.Replace("e-Docs USA Inc", EdosUsaIncStr), false, string.Empty, true);
            }
            catch
            { }
        }

        private static void GetInputArgs(string[] args)
        {
            string startProcess = string.Empty;
            string stopProcess = string.Empty;


            try
            {


                foreach (string inputArgs in args)
                {
                    // edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Looking for input arg:{inputArgs}");
                    if (inputArgs.StartsWith(ArgStopProcess, StringComparison.OrdinalIgnoreCase))
                    {
                        stopProcess = inputArgs.Substring(ArgStopProcess.Length);
                        //edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Found arg:{inputArgs} for input vlaue:{batchID}");
                        // edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Found arg:{inputArgs} for input vlaue:{batchID}");

                    }
                    if (inputArgs.StartsWith(ArgStartProcess, StringComparison.OrdinalIgnoreCase))
                    {
                        startProcess = inputArgs.Substring(ArgStartProcess.Length);
                        // edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Found arg:{inputArgs} for input vlaue:{archiver}");
                    }

                    //if (inputArgs.StartsWith(ArgEmailPW, StringComparison.OrdinalIgnoreCase))
                    //{
                    //    emailPW = inputArgs.Substring(ArgEmailPW.Length);
                    //    edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Found arg:{inputArgs} to set email pw");
                    //}
                }
                if ((string.IsNullOrEmpty(stopProcess)) && (string.IsNullOrEmpty(startProcess)))
                {
                    throw new Exception("Invalid args no args found");
                    //edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole("Usage:Edocs.Upload.Batches.AzureCloud.exe /batchid:22806e74-f914-454c-ac4d-7cbb91364501 /archiver:labrecs");
                    //edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole("Usage:Edocs.Upload.Batches.AzureCloud.exe /encemailpw:test");
                    //edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole("/encemailpw: this encrypts the email password");
                    //edl.TraceLogger.TraceLoggerInstance.TraceError("No args found");
                }
                //else if (!(string.IsNullOrWhiteSpace(emailPW)))
                //{
                //    if (!(string.IsNullOrEmpty(batchID)) && (!(string.IsNullOrEmpty(archiver))))
                //        throw new Exception($"Invlaid args:{batchID} {archiver} cannot be used when encrypting setting email pw");
                //    // EncryptEmailPw(emailPW.Trim());
                //}
                else
                {

                    //   EncryptEmailPw("6746edocs");
                    // UplaodImages(batchID, archiver);
                    // if ((Properties.Settings.Default.AuditLogFolder))
                    // CopyAuditLogs();

                }
                StopStartProcess(startProcess, stopProcess);
            }
            catch (Exception ex)
            {
                string message = $"Upload to azure cloud did not start on scanning machine {Environment.MachineName} error:{ex.Message}";
                //// UploadUtilities.SEmail(message, true);
                //edl.TraceLogger.TraceLoggerInstance.TraceError("Usage:Edocs.Upload.Batches.AzureCloud.exe /batchid:22806e74-f914-454c-ac4d-7cbb91364501 /archiver:labrecs");
                //edl.TraceLogger.TraceLoggerInstance.("Usage:Edocs.Upload.Batches.AzureCloud.exe /encemailpw:test");
                //edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole("/encemailpw: this encrypts the email password");
                ////  edl.TraceLogger.TraceLoggerInstance.TraceError("No args found");
                ////  edl.TraceLogger.TraceLoggerInstance.TraceError($"{ex.Message}");
                edl.TraceLogger.TraceLoggerInstance.TraceErrorConsole($"Error starting Process {ex.Message}");
                //  UploadUtilities.NotifyUser(batchID, message);

            }



        }
        private static void StopStartProcess(string pStart,string pStop)
        {
            if(!(string.IsNullOrWhiteSpace(pStop)))
            {
                EndTask(pStop);
            }
            if (!(string.IsNullOrWhiteSpace(pStart)))
            {
                RunTask(pStart, string.Empty, false, false, false, System.Diagnostics.ProcessWindowStyle.Maximized, Path.GetDirectoryName(pStart), Path.GetDirectoryName(pStart));
            }
        }
        //private static void OpenTraceLog(string batchID,string ald)
        //{
        //    CloseTraceLog();

        // //   string ald = SettingsManager.AuditLogsDirectroy.Replace("e-Docs USA Inc", EdosUsaIncStr);
        //    Directory.CreateDirectory(ald);



        //    ald = Edocs_Utilities.EdocsUtilitiesInstance.CheckDirectoryPath(ald);
        //    OpenTraceLog(ald, batchID);
        //}
        public static void EndTask(string taskname)
        {
            string processName = taskname.Replace(".exe", "");
            try
            {
                foreach (System.Diagnostics.Process process in System.Diagnostics.Process.GetProcessesByName(processName))
                {
                    edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Stopping process {taskname}");
                    edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Stopping process {taskname}");
                    process.Kill();
                    edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Process {taskname} stopped");
                    edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Process {taskname} stopped");
                    System.Threading.Thread.Sleep(15000);
                    return;
                }
                throw new Exception($"Could not stop task:{taskname} process name:{processName}");
            }
            catch (Exception ex)
            {
                edl.TraceLogger.TraceLoggerInstance.TraceError($"Message:{ex.Message}");
                edl.TraceLogger.TraceLoggerInstance.TraceErrorConsole($"Message:{ex.Message}");
                throw new Exception($"Could not stop task:{taskname} process name:{processName} message:{ex.Message}");
            }
        }
        public static void RunTask(string processName, string processArgs, bool waitForExit, bool useShellExecute, bool createNoWindow, ProcessWindowStyle processWindowStyle, string workingDirectory, string startFolder, int waitForExitMillSeconds = 0)
        {
            try
            {

                edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Method Runtask");
                edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Starting process {processName}");
                System.Diagnostics.Process myProcess = new System.Diagnostics.Process();
                edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Using Shell Execution {useShellExecute}");
                myProcess.StartInfo.UseShellExecute = useShellExecute;
                // myProcess.StartInfo.FileName = Path.GetFileName(processName);
                myProcess.StartInfo.FileName = processName;
                if (!(string.IsNullOrWhiteSpace(processArgs)))
                {
                    edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Using args {processArgs}");
                    myProcess.StartInfo.Arguments = processArgs;
                }
                else
                    edl.TraceLogger.TraceLoggerInstance.TraceWarning($"No args passed");
                edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Create not window {createNoWindow}");
                myProcess.StartInfo.CreateNoWindow = createNoWindow;
                edl.TraceLogger.TraceLoggerInstance.TraceInformation($"windows Style {processWindowStyle}");
                myProcess.StartInfo.WindowStyle = processWindowStyle;
                if (!(string.IsNullOrWhiteSpace(startFolder)))
                {
                    edl.TraceLogger.TraceLoggerInstance.TraceInformation($"start folder {startFolder}");
                    Environment.CurrentDirectory = startFolder;
                    myProcess.StartInfo.WorkingDirectory = startFolder;
                }
                if (!(string.IsNullOrEmpty(workingDirectory)))
                {
                    edl.TraceLogger.TraceLoggerInstance.TraceInformation($"WOrking folder {workingDirectory}");
                    myProcess.StartInfo.WorkingDirectory = workingDirectory;

                }
                if (!(myProcess.Start()))
                {
                    throw new Exception($"Process {processName} with args {processArgs} did not start");
                }
                edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Wait for process to exit {waitForExit}");
                if (waitForExit)
                    myProcess.WaitForExit();
            }
            catch (Exception ex)
            {
                edl.TraceLogger.TraceLoggerInstance.TraceError($"Process did not start {ex.Message}");
                throw new Exception(ex.Message);
            }

        }
        private static void OpenTraceLog()
        {
            traceLog = LogFolder;
            edl.TraceLogger.TraceLoggerInstance.CreateDirectory(traceLog);
            traceLog = Path.Combine(traceLog, $"{AssemblyInfo.GetAssemblyTitle()}_{DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")}.log");
            edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Opening trace lof file for trace folder:{traceLog}");

            edl.TraceLogger.TraceLoggerInstance.RunningAssembley = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.exe";
            edl.TraceLogger.TraceLoggerInstance.OpenTraceLogFile(traceLog, AssemblyInfo.GetAssemblyTitle());
            edl.TraceLogger.TraceLoggerInstance.WriteTraceHeader();
            edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Opened trace log file:{traceLog}");
            edl.TraceLogger.TraceLoggerInstance.TraceInformation($"AssemblyTitle:{AssemblyInfo.GetAssemblyTitle()}");
            edl.TraceLogger.TraceLoggerInstance.TraceInformation($"AssemblyCopyright:{AssemblyInfo.GetAssemblyCopyright()}");
            edl.TraceLogger.TraceLoggerInstance.TraceInformation($"AssemblyDescription:{AssemblyInfo.GetAssemblyDescription()}");
            edl.TraceLogger.TraceLoggerInstance.TraceInformation($"AssemblyVersion:{AssemblyInfo.GetAssemblyVersion()}");
        }
        private static string GetAppConfigSetting(string key)
        {

            try
            {
                Configuration c = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            }
            catch (ConfigurationErrorsException ex)
            {
                throw new Exception($"Could not get applicatiion config key:{key} message:{ex.Message}");
            }
            return ConfigurationManager.AppSettings.Get(key);
        }

    }
}
