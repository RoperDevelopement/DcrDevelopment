using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using EU = EdocsUSA.Utilities;
using Edocs.Check.HL7.Running.ConstProperties;
using Edocs.Check.HL7.Running.SEmail;
using EHU = Edocs.HelperUtilities;
using TL = EdocsUSA.Utilities.Logging.TraceLogger;
namespace Edocs.Check.HL7.Running
{
    class CHKH7RevecingFiles : IDisposable
    {
        public void HL7RecevingFiles()
        {
            //eadonly string AppConfigKeyHL7ListenerLogsFolder = "HL7ListenerLogsFolder";
            //readonly string AppConfigKeyHL7ListenerLogsFileName = "HL7ListenerLogsFileName";
            //readonly string AppConfigKeyHL7TrafficLogsFolder = "HL7TrafficLogsFolder";
            //readonly string AppConfigKeyHL7TrafficLogsFileName = "HL7TrafficLogsFileName";
            string hl7TrafficlLogFile = string.Empty;
             
            string hl7ListenerLogsFolder = string.Empty;
            string eMailMess = string.Empty;
            bool startService = true;
            int loop = 1;
            OpenAuditLog().ConfigureAwait(false).GetAwaiter().GetResult();
            TL.TraceLoggerInstance.TraceInformation($"In Method HL7RecevingFiles");
            try
            {

                startService = PropertiesConst.HL7Instance.HL7ReStartProcess;
                TL.TraceLoggerInstance.TraceInformation($"In Method HL7RecevingFiles start service set to {startService}");

                //  if (startService)
                // {
                startService = ProcessHL7TrafficLog(startService).ConfigureAwait(true).GetAwaiter().GetResult();
                if (startService)
                {
                    TL.TraceLoggerInstance.TraceInformation($"Checking if service is running");
                    startService = false;
                    ProcessHL7TrafficLog(startService).ConfigureAwait(true).GetAwaiter().GetResult();
                }
                // }
                // else
                //   ProcessHL7TrafficLog(startService).ConfigureAwait(true).GetAwaiter().GetResult();

                //if (HelperUtilities.Utilities.CheckFileExists(hl7ListenerLogsFolder))
                //{
                //    string llDestF = Path.Combine(wf, PropertiesConst.HL7Instance.HL7ListenerLogsFileName);
                //    Console.WriteLine($"Copying {hl7ListenerLogsFolder} to {llDestF}");
                //    EHU.Utilities.CopyFile(hl7ListenerLogsFolder, llDestF, true);
                //    lastLineLogFile = File.ReadLines(llDestF).Last();
                //    Console.WriteLine($"last line {lastLineLogFile}" );
                //}
                //else
                //    throw new Exception($"Traffic log file not found {hl7TrafficlLogFile}");
            }

            catch (Exception ex)
            {
                eMailMess = $"Error checking HL7 service running for traffic log file {hl7TrafficlLogFile} </br></br> {ex.Message}";
            }
            if (!(string.IsNullOrWhiteSpace(eMailMess)))
            {
                try
                {
                    TL.TraceLoggerInstance.TraceInformation($"Sending email message {eMailMess} {DateTime.Now.ToString()}");
                    SEmail.SEmail.SendEMailsInstance.EmailSend(eMailMess, true).ConfigureAwait(false).GetAwaiter().GetResult();
                }
                catch (Exception ex)
                {
                    TL.TraceLoggerInstance.TraceError($"Sending email message {eMailMess} {DateTime.Now.ToString()} {ex.Message}");
                }
            }

            TL.TraceLoggerInstance.TraceInformation($"End time checking HL7 Receving files {DateTime.Now.ToString()}");
            TL.TraceLoggerInstance.Dispose();
        }
        private async Task<bool> ProcessHL7TrafficLog(bool startService)
        {
            string eMailMess = string.Empty;

            string hl7TrafficlLogFile = PropertiesConst.HL7Instance.HL7TrafficLogsFolder;
            string hl7ListenerLogsFolder = PropertiesConst.HL7Instance.HL7ListenerLogsFolder;
            TL.TraceLoggerInstance.TraceInformation($"HL7 TrafficLogs Folder {hl7TrafficlLogFile}");
            TL.TraceLoggerInstance.TraceInformation($"HL7 ListenerLogs Folder {hl7ListenerLogsFolder}");
            string lastLineLogFile = string.Empty;
            string wf = PropertiesConst.HL7Instance.HL7RunningWorkingFolder;
            int durTime = 0;
            TL.TraceLoggerInstance.TraceInformation($"HL7 Working Folder {wf}");
            EHU.Utilities.CreateDirectory(wf);
            EHU.Utilities.DeleteFiles(wf, 0);
            if (HelperUtilities.Utilities.CheckFileExists(hl7TrafficlLogFile))
            {
                string tlfDestF = Path.Combine(wf, PropertiesConst.HL7Instance.HL7TrafficLogsFileName);
                TL.TraceLoggerInstance.TraceInformation($"Copying {hl7TrafficlLogFile} to {tlfDestF}");
                EHU.Utilities.CopyFile(hl7TrafficlLogFile, tlfDestF, true);
                lastLineLogFile = File.ReadLines(tlfDestF).Last();
                TL.TraceLoggerInstance.TraceInformation($"Processing HL7 traffic log file line {lastLineLogFile}");

                if (string.IsNullOrWhiteSpace(lastLineLogFile))
                {
                    TL.TraceLoggerInstance.TraceError($"Could not get traffic log last line for file {tlfDestF}");
                    throw new Exception($"String is empty for traffic log file {tlfDestF}");
                }

                string currTime24 = PropertiesConst.HL7Instance.GetHHMM(lastLineLogFile).ConfigureAwait(false).GetAwaiter().GetResult();
                TL.TraceLoggerInstance.TraceInformation($"Traffic log HL7 Received last file at {currTime24}");
                bool serviceRunning = PropertiesConst.HL7Instance.GetTimeDifference(currTime24, ref durTime);
                TL.TraceLoggerInstance.TraceInformation($"Duration between {currTime24} and {DateTime.Now.ToString("HH:mm:ss")} {durTime}");
                EHU.Utilities.DeleteFile(PropertiesConst.HL7Instance.NoLogFile);
                //  bool serviceRunning =
                if (!(serviceRunning))
                {

                    TL.TraceLoggerInstance.TraceError($"HL7 not receving files");
                    if (startService)
                    {

                        string HL7ServiceName = PropertiesConst.HL7Instance.HL7ServiceName;

                        int waitForExitSeconds = PropertiesConst.HL7Instance.StartServiceWaitForExitSeconds;
                        eMailMess = $"<p>HL7 service not running</p></br><p>Current time in traffic log file {currTime24} compared to computer current time {DateTime.Now.ToString("HH:mm:ss")}</p></br></br><p>HL7 traffic log file {hl7TrafficlLogFile}</p></br></br><p>HL7 trafficlog file last file received {currTime24}</p></br></br><p>Duration since last file received {durTime}</p></br></br><p>HL7 traffic log file last line {lastLineLogFile}</p></br> Starting Stopping HL7 Service with params taskToRun {HL7ServiceName} sleeping for {PropertiesConst.HL7Instance.ThredSleepAfterRestartServiceSecs}";
                        StartHL7Listner(eMailMess).ConfigureAwait(false).GetAwaiter().GetResult();
                        //SEmail.SEmail.SendEMailsInstance.EmailSend(eMailMess, true).ConfigureAwait(false).GetAwaiter().GetResult();
                        //TL.TraceLoggerInstance.TraceInformation($"Sending email message {eMailMess} {DateTime.Now.ToString()}");
                        //EHU.Utilities.RestartService(HL7ServiceName, waitForExitSeconds);
                        //TL.TraceLoggerInstance.TraceInformation($"Service restarted sleeping for {PropertiesConst.HL7Instance.ThredSleepAfterRestartServiceSecs} ");
                        //SEmail.SEmail.SendEMailsInstance.EmailSend($"Service restarted sleeping for {PropertiesConst.HL7Instance.ThredSleepAfterRestartServiceSecs}", false).ConfigureAwait(false).GetAwaiter().GetResult();
                        //System.Threading.Thread.Sleep(PropertiesConst.HL7Instance.ThredSleepAfterRestartServiceSecs);
                        return startService;

                    }
                    else
                    {
                        eMailMess = $"<p>HL7 service not running</p></br><p>Current time in traffic log file {currTime24} compared to computer current time {DateTime.Now.ToString("HH:mm:ss")}</p></br></br><p>HL7 traffic log file {hl7TrafficlLogFile}</p></br></br><p>HL7 trafficlog file last file received {currTime24}</p></br></br><p>Duration since last file received {durTime}</p></br></br><p>HL7 traffic log file last line {lastLineLogFile}</p>";
                        StartHL7Listner(eMailMess).ConfigureAwait(false).GetAwaiter().GetResult();
                        return true;
                        //TL.TraceLoggerInstance.TraceInformation($"Sending email message {eMailMess} {DateTime.Now.ToString()}");
                        //SEmail.SEmail.SendEMailsInstance.EmailSend(eMailMess, true).ConfigureAwait(false).GetAwaiter().GetResult();

                    }

                }
            }
            else
            {
                if(EHU.Utilities.CheckFileExists(PropertiesConst.HL7Instance.NoLogFile))
                {
                    eMailMess = $"<p>HL7 service not running</p></br><p>No Log file found {DateTime.Now.ToString("HH:mm:ss")}</p></br></br><p>HL7 traffic log file {hl7TrafficlLogFile}</p></br></br><p>HL7 Log file restarting service</p></br></br><p>Duration since last file received {durTime}</p></br></br><p>HL7 traffic log file last line {lastLineLogFile}</p>";
                    StartHL7Listner(eMailMess).ConfigureAwait(false).GetAwaiter().GetResult();
                    TL.TraceLoggerInstance.TraceInformation($"Sending email message {eMailMess} {DateTime.Now.ToString()}");
                   // SEmail.SEmail.SendEMailsInstance.EmailSend(eMailMess, true).ConfigureAwait(false).GetAwaiter().GetResult();
                    System.Threading.Thread.Sleep(PropertiesConst.HL7Instance.ThredSleepAfterRestartServiceSecs);
                    return true;
                }
                EHU.Utilities.WriteOutPut(PropertiesConst.HL7Instance.NoLogFile, "no log file");
               
                throw new Exception($"HL7 Traffic log file not found {hl7TrafficlLogFile} Created file {PropertiesConst.HL7Instance.NoLogFile}");
            }
            return false;
           

        }
        private async Task StartHL7Listner(string eMailMess)
        {
            string HL7ServiceName = PropertiesConst.HL7Instance.HL7ServiceName;

            int waitForExitSeconds = PropertiesConst.HL7Instance.StartServiceWaitForExitSeconds;
           
            SEmail.SEmail.SendEMailsInstance.EmailSend(eMailMess, true).ConfigureAwait(false).GetAwaiter().GetResult();
            TL.TraceLoggerInstance.TraceInformation($"Sending email message {eMailMess} {DateTime.Now.ToString()}");
            EHU.Utilities.RestartService(HL7ServiceName, waitForExitSeconds);
            TL.TraceLoggerInstance.TraceInformation($"Service restarted sleeping for {PropertiesConst.HL7Instance.ThredSleepAfterRestartServiceSecs} ");
            SEmail.SEmail.SendEMailsInstance.EmailSend($"Service restarted sleeping for {PropertiesConst.HL7Instance.ThredSleepAfterRestartServiceSecs}", false).ConfigureAwait(false).GetAwaiter().GetResult();
            System.Threading.Thread.Sleep(PropertiesConst.HL7Instance.ThredSleepAfterRestartServiceSecs);
        }
        private async Task RestartHL7Service()
        {
            string tasktoRun = string.Empty;
            string taskArgs = string.Empty;

            EHU.Utilities.RunTask(tasktoRun, taskArgs, string.Empty, true);
        }
        private async Task OpenAuditLog()
        {
            string auditLogsFolder = PropertiesConst.HL7Instance.AuditLogsFolder;
            //try
            //{
            string logFileName = $"{EHU.Utilities.CheckFolderPath(auditLogsFolder)}Edocs.Check.HL7.Running_AuditLog_{DateTime.Now.ToString("MM_dd_yyyy")}.log";
            EHU.Utilities.CreateDirectory(logFileName);
            TL.TraceLoggerInstance.RunningAssembley = "Edocs.Check.HL7.Running";
            if (EHU.Utilities.CheckFileExists(logFileName))
                TL.TraceLoggerInstance.OpenTraceLogFile(logFileName, "AuditLog", false);
            else
            {
                // TL.TraceLoggerInstance.OpenTraceLogFile($"{EHU.Utilities.CheckFolderPath(auditLogsFolder)}\\AuditLog\\{EHU.Utilities.GetAssemblyName()}_AuditLog_{DateTime.Now.ToString//("MM_dd_yyyy")}.log", "AuditLog", true);
                TL.TraceLoggerInstance.OpenTraceLogFile(logFileName, "AuditLog", true);

                TL.TraceLoggerInstance.TraceInformation(($@"Company:{EHU.Utilities.GetAssemblyCompnayName()} 
                                             CopyRight:{EHU.Utilities.GetAssemblyCopyright()} Product:{EHU.Utilities.GetAssemblyProduct()}
                                    Title:{EHU.Utilities.GetAssemblyTitle()} FileVersion:1.1.0.0 Assembly Version:1.0.0.0"));
            }
            //}
            //catch (Exception ex)
            //{
            //    SbErrorMessages.AppendLine($"<p>Error Opening trace log file {ServiceHelpers.Instance.CheckFolderPath(ServiceHelpers.Instance.GetApplicationDir())}{ServiceHelpers.Instance.GetAssemblyName()}_AuditLog_{DateTime.Now.ToString("MM_dd_yyyy")}.log total time {TL.TraceLoggerInstance.StopStopWatch()} {ex.Message}</p></br>");
            //    SEmail.SendEMailsInstance.EmailSend(SbErrorMessages.ToString(), true);
            //}
            TL.TraceLoggerInstance.TraceInformation($"Time start checking HL7 Receving files {DateTime.Now.ToString()}");
        }
        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {

                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~ProcessAuditLogs() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            try
            {
                // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
                Dispose(true);
                // TODO: uncomment the following line if the finalizer is overridden above.
                GC.SuppressFinalize(this);
            }
            catch { }
        }
        #endregion
    }
}
