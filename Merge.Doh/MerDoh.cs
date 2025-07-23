using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ET = Edocs.HelperUtilities.Utilities;
using TL = EdocsUSA.Utilities.Logging.TraceLogger;
using EdocsUSA.Merge.Doh.Models;
using AZ = Edocs.Upload.Azure.Blob.Storage.AzureBlobStorage;
using SE = Edocs.Send.Emails;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace EdocsUSA.Merge.Doh
{
    class MerDoh
    {
        static readonly string ArgDateOfServiceSpan = "/DateOfServiceSpan:";
        static readonly string ArgShowUsage = "/?";
        static readonly string AppConfigKeyAuditLogsFolder = "AuditLogsFolder";
        static readonly string AppConfigKeyDaysToKeepLogFiles = "DaysToKeepLogFiles";
        static readonly string AppConfigKeyRepStrApplicationDir = "{ApplicationDir}";
        static readonly string AppConfigKeyAzureBlobStorageConnectionString = "AzureBlobStorageConnectionString";
        static readonly string AppConfigKeyAzureBlobContanierAuditShare = "AzureBlobContanierAuditShare";
        static readonly string AppConfigKeyAzureBlobAccountName = "AzureBlobAccountName";
        static readonly string AppConfigKeyAzureBlobAccountKey = "AzureBlobAccountKey";
        static int TotalMerged
        { get; set; } = 0;
        static int TotalNotMerged
        { get; set; } = 0;
        static int TotalRead
        { get; set; } = 0;
        static int TotalErrors
        { get; set; } = 0;
        static StringBuilder SbErrors
        { get; set; }
        static void Main(string[] args)
        {
            SbErrors = new StringBuilder();
            try
            {
                TL.TraceLoggerInstance.StartWatch();
                CheckMultiProcessingRunning(args);
                OpenLogFile();
                GetInputArgs(args);

            }
            catch (Exception ex)
            {
                TotalErrors++;
                Console.WriteLine(ex.Message);
                SbErrors.Append(ex.Message);
            }
            TimeSpan span = TL.TraceLoggerInstance.TSStopWatch();
            EmailSend(SbErrors.ToString(),span );
            TL.TraceLoggerInstance.TraceInformation($"Total DOH Records Read {TotalRead}");
            TL.TraceLoggerInstance.TraceInformation($"Total DOH Records Merged {TotalMerged}");
            TL.TraceLoggerInstance.TraceInformation($"Total DOH Records Not Merged {TotalNotMerged}");
            TL.TraceLoggerInstance.TraceInformation($"Total DOH Record Errors {TotalErrors}");
            TL.TraceLoggerInstance.TraceInformation($"Total Run time {span}");
            UpLoadAuditLogs().ConfigureAwait(false).GetAwaiter().GetResult();
        }
        static void GetInputArgs(string[] args)
        {

            int dateOfServiceSpan = 0;
            TL.TraceLoggerInstance.TraceInformation($"Start time of {ET.GetAssemblyTitle()} {DateTime.Now.ToString()}");
            foreach (string arg in args)
            {
                if (arg.StartsWith(ArgDateOfServiceSpan, StringComparison.OrdinalIgnoreCase))
                {
                    dateOfServiceSpan = ET.ParseInt32(arg.Substring(ArgDateOfServiceSpan.Length));
                }
            }
            GetDOHRecords(dateOfServiceSpan);
        }
        static void GetDOHRecords(int dateOfServiceSpan)
        {
            DateTime startDt = DateTime.Now.AddDays(dateOfServiceSpan);
            DateTime endDt = DateTime.Now.AddDays(1);
            TL.TraceLoggerInstance.TraceInformation($"Date of service span start date {startDt} end date {endDt}");
            SqlCmd cmd = new SqlCmd();
            foreach (DOHRecsModel model in cmd.GetMergeDOHRecs(startDt, endDt, 0))
            {
                try
                {
                    TotalRead++;
                    HL7Model hL7 = cmd.GetHL7Records(model, startDt).ConfigureAwait(false).GetAwaiter().GetResult();
                    if (hL7 != null)
                    {
                        TL.TraceLoggerInstance.TraceWarning($"Updating DOH information for DOH id {model.DOHID}");
                        cmd.UpdateDOHTable(hL7, model, cmd.SPUpDateDOHRecsByID).ConfigureAwait(false).GetAwaiter().GetResult();
                        TotalMerged++;
                    }
                    else
                    {
                        TotalNotMerged++;
                        TL.TraceLoggerInstance.TraceWarning($"No HL7 information for DOH id {model.DOHID}");
                    }
                }
                catch (Exception ex)
                {
                    TotalErrors++;
                    SbErrors.AppendLine($"Error processing labreq id {model.DOHID} {ex.Message}");
                    TL.TraceLoggerInstance.TraceError($"Updating HL7 information for DOH id {model.DOHID} {ex.Message}");
                }
            }
        }
        static async Task UpLoadAuditLogs()
        {
            string logFolder = LogFolder();
            TL.TraceLoggerInstance.TraceInformation($"Uploading files {logFolder} to azure cloud");
            TL.TraceLoggerInstance.TraceInformationConsole($"Uploading files {logFolder} to azure cloud");
            TL.TraceLoggerInstance.CloseTraceFile();
            AZ.BlobStorageInstance.AzureBlobAccountKey = ET.GetAppConfigSetting(AppConfigKeyAzureBlobAccountKey);
            AZ.BlobStorageInstance.AzureBlobAccountName = ET.GetAppConfigSetting(AppConfigKeyAzureBlobAccountName);
            AZ.BlobStorageInstance.AzureBlobStorageConnectionString = ET.GetAppConfigSetting(AppConfigKeyAzureBlobStorageConnectionString);
            string azContaier = ET.GetAppConfigSetting(AppConfigKeyAzureBlobContanierAuditShare);
            try
            {
                foreach (var file in ET.GetDirFilesName(logFolder))
                {

                    string fileName = Path.Combine(logFolder, file);
                    //  TL.TraceLoggerInstance.TraceInformationConsole($"Uploading {fileName} to azure container {AppConfigKeyAzureBlobContanierAuditShare}");
                    string fileContents = ET.ReadFile(fileName);

                    AZ.BlobStorageInstance.UploadAzureBlobTextFile(file, azContaier, fileContents).ConfigureAwait(false).GetAwaiter().GetResult();
                    // TL.TraceLoggerInstance.TraceInformationConsole($"File uploaded to azure cloud");
                    ET.DeleteFile(fileName);
                }
            }
            catch (Exception ex)
            {
                // TL.TraceLoggerInstance.TraceErrorConsole($"Uploading audit log folder {logFolder} to azure container {AppConfigKeyAzureBlobContanierAuditShare} {ex.Message}");
                EmailSend($"Uploading audit log files to azure cloud log folder {logFolder} to azure container {AppConfigKeyAzureBlobContanierAuditShare} {ex.Message}", true);
            }
        }
        static void CheckMultiProcessingRunning(string[] args)
        {
            try
            {


                int processId = ET.CheckMuptiProcessingRunning("EdocsUSA.Merge.Doh");
                Stopwatch executionTimer = Stopwatch.StartNew();
                if (processId > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (string ar in args)
                        sb.AppendLine(ar);

                    string errMess = $"Processing all ready running <br/> stopping process id {processId}<br/> with args {sb.ToString()}";
                    TimeSpan ts = DateTime.Now - DateTime.Now;
                    EmailSend(errMess, executionTimer.Elapsed, true);
                    TL.TraceLoggerInstance.CloseTraceFile();
                    ET.EndTaskById(processId);
                }
                executionTimer.Stop();
                //OpenLogFile();
            }
            catch (Exception ex)
            {
                Environment.Exit(-1);
            }

        }
        static void OpenLogFile()
        {

            string logFolder = LogFolder();
            TL.TraceLoggerInstance.RunningAssembley = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}";
            TL.TraceLoggerInstance.TraceInformationConsole($"Opeing log file: {logFolder}{ET.GetAssemblyTitle()}");
            TL.TraceLoggerInstance.OpenTraceFileAddHHMMSS(logFolder, ET.GetAssemblyTitle(), ET.GetAssemblyTitle(), true);

            TL.TraceLoggerInstance.TraceInformation($"Ass Title {ET.GetAssemblyTitle()}");
            TL.TraceLoggerInstance.TraceInformation($"Ass Description {ET.GetAssemblyDescription()}");
            TL.TraceLoggerInstance.TraceInformation($"Ass Version {ET.GetAssemblyVersion()}");
            TL.TraceLoggerInstance.TraceInformationConsole($"Ass Title {ET.GetAssemblyTitle()}");
            TL.TraceLoggerInstance.TraceInformationConsole($"Ass Description {ET.GetAssemblyDescription()}");
            TL.TraceLoggerInstance.TraceInformationConsole($"Ass Version {ET.GetAssemblyVersion()}");


        }
        static string LogFolder()
        {
            string logFolder = ET.CheckFolderPath(ET.GetAppConfigSetting(AppConfigKeyAuditLogsFolder));

            if (string.IsNullOrWhiteSpace(logFolder))
            {
                logFolder = ET.CheckFolderPath(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
                logFolder = $"{logFolder}\\Local\\MergeDOHRecs";
            }
            else
            {
                logFolder = ET.CheckFolderPath(ET.ReplaceString(logFolder, AppConfigKeyRepStrApplicationDir, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)));

            }
            TL.TraceLoggerInstance.TraceInformationConsole($"Using log folder: {logFolder}");
            return ET.CheckFolderPath(logFolder);
        }
        static void EmailSend(string message, TimeSpan ts, bool err)
        {
            try
            {


                //   SE.Send_Emails.EmailInstance.UpDateEmailPw("6746edocs");
                TL.TraceLoggerInstance.TraceInformationConsole($"Sending email");
                string subject = $"Execution Summary running process {System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.exe on machine {Environment.MachineName} runtime {DateTime.Now.ToString()}"; ;
                if (err)
                    subject = $"Execution Summary error running process {System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.exe machine {Environment.MachineName} runtime {DateTime.Now.ToString()}";
                string body = "Summary"
                    + "<br/>Total DOH recs read: " + TotalRead
                    + "<br/>Total DOH recs merged: " + TotalMerged
                    + "<br/>Total DOH recs not merged: " + TotalNotMerged
                    + "<br/>Execution Time: " + ts
                    + "<br/>Error Message: " + message;
                SE.Send_Emails.EmailInstance.SendEmail(string.Empty, body, subject, string.Empty, true, string.Empty);
                if (err)
                {
                    message = $"Error running  {System.Reflection.Assembly.GetExecutingAssembly().GetName().Name} {message}";
                    SE.Send_Emails.EmailInstance.SendTxtMessage(message, true);
                }
                TL.TraceLoggerInstance.TraceInformation("email sent");
            }
            catch (Exception ex)
            {
                TL.TraceLoggerInstance.TraceError($"Sending email {ex.Message}");
                TL.TraceLoggerInstance.TraceErrorConsole($"Sending email {ex.Message}");
            }
        }
        static void EmailSend(string message, TimeSpan ts)
        {
            try
            {


                //   SE.Send_Emails.EmailInstance.UpDateEmailPw("6746edocs");
                TL.TraceLoggerInstance.TraceInformationConsole($"Sending email");
                string subject = $"Execution Summary running process {System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.exe on machine {Environment.MachineName} runtime {DateTime.Now.ToString()}"; ;
                if (TotalErrors > 0)
                    subject = $"Execution Summary error running process {System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.exe machine {Environment.MachineName} runtime {DateTime.Now.ToString()}";
                else
                    message = "no Errors found";
                string body = "Summary"
                    + "<br/>Total DOH recs read: " + TotalRead
                    + "<br/>Total DOH recs merged: " + TotalMerged
                    + "<br/>Total DOH recs not merged: " + TotalNotMerged
                     + "<br/>Total DOH recs errors: " + TotalErrors
                    + "<br/>Execution Time: " + ts
                    + "<br/>Error Message: " + message;
                SE.Send_Emails.EmailInstance.SendEmail(string.Empty, body, subject, string.Empty, true, string.Empty);
                if (TotalErrors > 0)
                {
                    message = $"Error running  {System.Reflection.Assembly.GetExecutingAssembly().GetName().Name} {message}";
                    SE.Send_Emails.EmailInstance.SendTxtMessage(message, true);
                }
                TL.TraceLoggerInstance.TraceInformation("email sent");
            }
            catch (Exception ex)
            {
                TL.TraceLoggerInstance.TraceError($"Sending email {ex.Message}");
                TL.TraceLoggerInstance.TraceErrorConsole($"Sending email {ex.Message}");
            }
        }
        static void EmailSend(string message, bool err)
        {
            // SE.Send_Emails.EmailInstance.UpDateEmailPw("6746edocs");
            try
            {
                TL.TraceLoggerInstance.TraceInformationConsole($"Sending email");
                string subject = $"Execution Summary running process {System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.exe runtime {DateTime.Now.ToString()}"; ;
                if (err)
                    subject = $"Execution Summary error running process {System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.exe runtime {DateTime.Now.ToString()}";
                string body = "Summary"
                    + "<br/>Error Message: " + message;
                SE.Send_Emails.EmailInstance.SendEmail(string.Empty, body, subject, string.Empty, true, string.Empty);
                if (err)
                {
                    message = $"Error running  {System.Reflection.Assembly.GetExecutingAssembly().GetName().Name} {message}";
                    SE.Send_Emails.EmailInstance.SendTxtMessage(message, true);
                }
            }
            catch (Exception ex)
            {
                TL.TraceLoggerInstance.TraceError($"Sending email {ex.Message}");
                TL.TraceLoggerInstance.TraceErrorConsole($"Sending email {ex.Message}");
            }
        }
    }
}
