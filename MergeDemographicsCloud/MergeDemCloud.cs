using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Reflection;
using System.Diagnostics;
using System.Data.SqlClient;
using ET = Edocs.HelperUtilities.Utilities;
using TL = EdocsUSA.Utilities.Logging.TraceLogger;
using MergeDemographicsCloud.Models;
using AZ = Edocs.Upload.Azure.Blob.Storage.AzureBlobStorage;
using SE = Edocs.Send.Emails;
namespace MergeDemographicsCloud
{
    class MergeDemCloud
    {

        static readonly string ArgDateOfServiceSpan = "/DateOfServiceSpan:";
        static readonly string ArgMerge = "/Merge:";
        static readonly string ArgMergeLabReqs = "/MergeLabReqs:";
        static readonly string ArgReMergeLabReqs = "/ReMerge:";
        static readonly string ArgSetEmailPw = "/emilpw:";
        static readonly string ArgSFullText = "/sfulltext:";
        static readonly string ArgShowUsage = "/?";
        static readonly string AppConfigDateOfServiceSpan = "DateOfServiceSpan";
        static readonly string ArgCenterDateOfService = "/CenterDateOfService:";
        static readonly string AppConfigKeyAuditLogsFolder = "AuditLogsFolder";
        static readonly string AppConfigKeyDaysToKeepLogFiles = "DaysToKeepLogFiles";
        static readonly string AppConfigKeyThreadSleepSecondsProcessRunning = "ThreadSleepSecondsProcessRunning";
        static readonly string AppConfigKeyMaxLoopWaitForProcess = "MaxLoopWaitForProcess";
        static readonly string AppConfigKeyWaitProcessRunning = "WaitProcessRunning";


        static readonly string AppConfigKeyRepStrApplicationDir = "{ApplicationDir}";
        static readonly string AppConfigKeySPUpDateCovid19HL7 = "SPUpDateCovid19HL7";
        static readonly string AppConfigKeySPMergeCovid19LabReqs = "SPMergeCovid19LabReqs";
        static readonly string AppConfigKeySPUpDateLabRecsHL7 = "SPUpDateLabRecsHL7";
        static readonly string AppConfigKeySPMergeLabRecs = "SPMergeLabRecs";
        static readonly string AppConfigKeyAzureBlobStorageConnectionString = "AzureBlobStorageConnectionString";
        static readonly string AppConfigKeyAzureBlobContanierAuditShare = "AzureBlobContanierAuditShare";
        static readonly string AppConfigKeyAzureBlobAccountName = "AzureBlobAccountName";
        static readonly string AppConfigKeyAzureBlobAccountKey = "AzureBlobAccountKey";
        static readonly string NoCovid19 = "nocovid19";

        static int ThreadSleepSecondsProcessRunning
        {
            get { return ET.ParseInt(ET.GetAppConfigSetting(AppConfigKeyThreadSleepSecondsProcessRunning)); }
        }
        static int MaxLoopWaitForProcess
        {
            get { return ET.ParseInt(ET.GetAppConfigSetting(AppConfigKeyMaxLoopWaitForProcess)); }
        }
        //static bool WaitProcessRunning
        //{
        //    get { return ET.TryParseBoold(ET.GetAppConfigSetting(AppConfigKeyWaitProcessRunning)); }
        //}
        static int TotalMerged
        { get; set; }
        static int TotalNotMerged
        { get; set; }
        static int TotalRead
        { get; set; }
        static int TotalReprocess
        { get; set; }=0;
        static string ErrMess
        { get; set; }
        static int Merged
        { get; set; }
        static int MergeLabReqs
        { get; set; }
        static bool ReMerge
        { get; set; }
        static int TotalSkipped
        { get; set; }
        static void Main(string[] args)
        {
            try
            {

                if (ET.TryParseBoold(ET.GetAppConfigSetting(AppConfigKeyWaitProcessRunning)))
                {
                    OpenLogFile();
                    WaitProcessRunning(args).ConfigureAwait(false).GetAwaiter().GetResult();
                    OpenLogFile();
                }
                else
                {
                    CheckMultiProcessingRunning(args);
                    OpenLogFile();
                }
                GetInputArgs(args);

                UpLoadAuditLogs().ConfigureAwait(false).GetAwaiter().GetResult();

            }
            catch (Exception ex)
            { Console.WriteLine($"Error starting merge {ex.Message}"); }
        }
        static async Task WaitProcessRunning(string[] args)
        {
            try
            {
                TL.TraceLoggerInstance.TraceInformation("Checking to see if another process is running:");
                
                int maxLoopWaitPR = MaxLoopWaitForProcess;
                int loop = 0;
                //      Console.ReadKey();
                Stopwatch executionTimer = Stopwatch.StartNew();
                Process[] runPro = ET.GetProcessRunning("MergeDemographicsCloud");
                if (runPro.Count() > 1)
                {
                    maxLoopWaitPR *= runPro.Count();
                    int processRunningTSleep = (ThreadSleepSecondsProcessRunning* runPro.Count()) * SqlCmds.MillSecond;
                   
                    StringBuilder sb = new StringBuilder();
                    IList<int> procTimes = new List<int>();
                    for (int i = 0; i < runPro.Count(); i++)
                    {

                        int sTime = int.Parse($"{runPro[i].StartTime.Hour}{runPro[i].StartTime.Minute}{runPro[i].StartTime.Second}");
                        procTimes.Add(sTime);
                    }

                    foreach (string ar in args)
                        sb.AppendLine(ar);
                    string errMess = $"Another another process is running checking max time:{processRunningTSleep} thread sleeping {maxLoopWaitPR} number process running {runPro.Count()} <br/> with args: {sb.ToString()}";
                    TL.TraceLoggerInstance.TraceInformation($"Another another process is running checking max time:{processRunningTSleep} thread sleeping {maxLoopWaitPR} number process running {runPro.Count()}");
                    EmailSend(errMess);
                    while (loop++ < maxLoopWaitPR)
                    {

                        System.Threading.Thread.Sleep(processRunningTSleep);
                        // runPro = ET.GetProcessRunning("MergeDemographicsCloud");
                        procTimes = CheckOtherProcessDone(procTimes).ConfigureAwait(false).GetAwaiter().GetResult();
                        if ((procTimes.Count == 1) || (procTimes.Count == 0))
                            break;
                        TL.TraceLoggerInstance.TraceInformationConsole($"number process still running {procTimes.Count} times through loop {loop}");
                    }
                    if (loop >= maxLoopWaitPR)
                    {
                        TL.TraceLoggerInstance.TraceError($"Timed out waiting for other process to exit number process still running {runPro.Count()} times through loop {loop} ");
                        errMess = $"Timed out waiting for other processes to exit <br/> number process running {runPro.Count()} times through loop {loop} number process running before this one {procTimes.Count} execution time {executionTimer.Elapsed} <br/> with args: {sb.ToString()}";
                        EmailSend(errMess, executionTimer.Elapsed, true);
                        Environment.Exit(-1);
                    }
                }

                TL.TraceLoggerInstance.TraceInformation($"Number times through loop {loop} excution time {executionTimer.Elapsed}");
                executionTimer.Stop();
                //if(runPro.Count() > 1)
                //  {
                //      int currentPID = runPro[runPro.Count() - 1].Id;
                //      DateTime lastStime = DateTime.Now.AddDays(-1);
                //      int pid = 0;


                //      for(int i=0; i<runPro.Count();i++)
                //      {
                //          if (runPro[i].StartTime > lastStime)
                //              pid = runPro[i].Id;
                //          Console.WriteLine(i);
                //      }
                //      Process pname = Process.GetProcessById(pid);

                //  while (loop++ < maxLoopWaitPR)
                //  {
                //          pname.WaitForExit(processRunningTSleep);
                //        //if (ET.CheckProcessingRunningByID(pid))
                //         // {
                //           //   System.Threading.Thread.Sleep(processRunningTSleep);
                //          //}
                //     // Console.WriteLine("press any key to exit");
                //      //Console.ReadLine();
                //      //Environment.Exit(0);
                //  }
                //  }
            }
            catch (Exception ex)
            {
                TL.TraceLoggerInstance.TraceError($"Error checking waiting for process to finish {ex.Message}");
                EmailSend($"Error checking waiting for process to finish </br> {ex.Message}", true);
                Environment.Exit(-1);
            }
        }
        static async Task<IList<int>> CheckOtherProcessDone(IList<int> checkProc)
        {
            IList<int> retList = new List<int>();
            Process[] runPro = ET.GetProcessRunning("MergeDemographicsCloud");
            if (runPro.Count() > 1)

            {

                for (int i = 0; i < runPro.Count(); i++)
                {

                    int sTime = int.Parse($"{runPro[i].StartTime.Hour}{runPro[i].StartTime.Minute}{runPro[i].StartTime.Second}");
                    if (checkProc.Contains(sTime))
                        retList.Add(sTime);
                }
            }

            return retList;
        }
        static async Task UpLoadAuditLogs()
        {

            string logFolder = LogFolder();
            TL.TraceLoggerInstance.TraceInformation($"Uploading files {logFolder} to azure cloud");
            TL.TraceLoggerInstance.TraceInformationConsole($"Uploading files {logFolder} to azure cloud");
            TL.TraceLoggerInstance.CloseTraceFile();
            Process[] runPro = ET.GetProcessRunning("MergeDemographicsCloud");
            if (runPro.Count() > 1)
            {
                TL.TraceLoggerInstance.TraceInformation($"Skipping Uploading audit logs to azure cloud number process still running {runPro.Count()}");
                return;
            }
              
                AZ.BlobStorageInstance.AzureBlobAccountKey = ET.GetAppConfigSetting(AppConfigKeyAzureBlobAccountKey);
            AZ.BlobStorageInstance.AzureBlobAccountName = ET.GetAppConfigSetting(AppConfigKeyAzureBlobAccountName);
            AZ.BlobStorageInstance.AzureBlobStorageConnectionString = ET.GetAppConfigSetting(AppConfigKeyAzureBlobStorageConnectionString);
            string azContaier = ET.GetAppConfigSetting(AppConfigKeyAzureBlobContanierAuditShare);
            try
            {
                foreach (var file in ET.GetDirFilesName(logFolder))
                {

                    string fileName = Path.Combine(logFolder, file);
                    TL.TraceLoggerInstance.TraceInformationConsole($"Uploading {fileName} to azure container {AppConfigKeyAzureBlobContanierAuditShare}");
                    string fileContents = ET.ReadFile(fileName);

                    AZ.BlobStorageInstance.UploadAzureBlobTextFile(file, azContaier, fileContents).ConfigureAwait(false).GetAwaiter().GetResult();
                    TL.TraceLoggerInstance.TraceInformationConsole($"File uploaded to azure cloud");
                    ET.DeleteFile(fileName);
                }
            }
            catch (Exception ex)
            {
                TL.TraceLoggerInstance.TraceErrorConsole($"Uploading audit log folder {logFolder} to azure container {AppConfigKeyAzureBlobContanierAuditShare} {ex.Message}");
                EmailSend($"Uploading audit log files to azure cloud log folder {logFolder} to azure container {AppConfigKeyAzureBlobContanierAuditShare} {ex.Message}", true);
            }
        }
        static string LogFolder()
        {
            string logFolder = ET.CheckFolderPath(ET.GetAppConfigSetting(AppConfigKeyAuditLogsFolder));

            if (string.IsNullOrWhiteSpace(logFolder))
            {
                logFolder = ET.CheckFolderPath(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
                logFolder = $"{logFolder}\\Local\\MergeDem";
            }
            else
            {
                logFolder = ET.CheckFolderPath(ET.ReplaceString(logFolder, AppConfigKeyRepStrApplicationDir, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)));

            }
            TL.TraceLoggerInstance.TraceInformationConsole($"Using log folder: {logFolder}");
            return ET.CheckFolderPath(logFolder);
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
        static void ShowUsage()
        {

            TL.TraceLoggerInstance.TraceError("show usage");
            TL.TraceLoggerInstance.TraceError($"Usage {System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.exe {ArgDateOfServiceSpan}10 ->date of service space in lab recs");
            TL.TraceLoggerInstance.TraceError($"Usage {System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.exe {ArgDateOfServiceSpan}10 ->date of service space in lab recs {ArgCenterDateOfService}10-23-2019 ->use differnet center date of service");
            TL.TraceLoggerInstance.TraceError($"Usage {System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.exe {ArgSetEmailPw}newemailpw ->Set email pw");
            Console.WriteLine("show usage");
            Console.WriteLine($"Usage {System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.exe {ArgDateOfServiceSpan}10 ->date of service space in lab recs");
            Console.WriteLine($"Usage {System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.exe {ArgDateOfServiceSpan}10 ->date of service space in lab recs {ArgCenterDateOfService}10-23-2019 ->use differnet center date of service");
            Console.WriteLine($"Usage {System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.exe {ArgSetEmailPw}newemailpw ->Set email pw");
            Console.WriteLine($"Usage {System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.exe {ArgSFullText} to search full text");

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
                    + "<br/>Total Lab recs read: " + TotalRead
                    + "<br/>Total Lab recs merged: " + TotalMerged
                    + "<br/>Total Lab recs not merged: " + TotalNotMerged
                      + "<br/>Total Lab recs skipped: " + TotalSkipped
                      +"<br/>Total Found not merged: "+ TotalReprocess
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
        static void EmailSend(string message)
        {
            // SE.Send_Emails.EmailInstance.UpDateEmailPw("6746edocs");
            try
            {
                TL.TraceLoggerInstance.TraceInformationConsole($"Sending email");
                string subject = $"Execution Summary running process {System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.exe runtime {DateTime.Now.ToString()}"; ;

                string body = "Summary"
                    + "<br/>Message: " + message;
                SE.Send_Emails.EmailInstance.SendEmail(string.Empty, body, subject, string.Empty, true, string.Empty);

            }
            catch (Exception ex)
            {
                TL.TraceLoggerInstance.TraceError($"Sending email {ex.Message}");
                TL.TraceLoggerInstance.TraceErrorConsole($"Sending email {ex.Message}");
            }
        }
        static void GetInputArgs(string[] args)
        {
            TotalMerged = TotalNotMerged = TotalRead = TotalSkipped = 0;
            Stopwatch executionTimer = Stopwatch.StartNew();
            Merged = 0;
            MergeLabReqs = 0;
            TotalSkipped = 0;
            ReMerge = false;
            bool sFullText = false;
            ErrMess = string.Empty;
            int dateOfServiceSpan = 0;
            DateTime centerDateOfService = DateTime.Now;
            TL.TraceLoggerInstance.TraceInformation($"Start time of {ET.GetAssemblyTitle()} {DateTime.Now.ToString()}");
            try
            {
                foreach (string arg in args)
                {
                    if (arg.StartsWith(ArgDateOfServiceSpan, StringComparison.OrdinalIgnoreCase))
                    {
                        dateOfServiceSpan = ET.ParseInt32(arg.Substring(ArgDateOfServiceSpan.Length));

                        if (dateOfServiceSpan <= 0)
                        {
                            dateOfServiceSpan = ET.ParseInt32(ET.GetAppConfigSetting(AppConfigDateOfServiceSpan));
                            TL.TraceLoggerInstance.TraceWarning($"Invaild Arg {ArgDateOfServiceSpan} so using default in app config {dateOfServiceSpan}");
                        }
                        TL.TraceLoggerInstance.TraceInformation($"Arg {ArgDateOfServiceSpan} input {dateOfServiceSpan}");
                        TL.TraceLoggerInstance.TraceInformationConsole($"Arg {ArgDateOfServiceSpan} input {dateOfServiceSpan}");
                    }

                    else if (arg.StartsWith(ArgMergeLabReqs, StringComparison.OrdinalIgnoreCase))
                    {
                        MergeLabReqs = ET.ParseInt32(arg.Substring(ArgMergeLabReqs.Length));
                        if ((MergeLabReqs > 1) || (MergeLabReqs < 0))
                        {
                            TL.TraceLoggerInstance.TraceWarning($"{ArgMergeLabReqs} can onl be 0 or 1 so setting to 0");
                            TL.TraceLoggerInstance.TraceWaringConsole($"{ArgMergeLabReqs} can onl be 0 or 1 so setting to 0");
                            MergeLabReqs = 0;
                        }

                    }
                    else if (arg.StartsWith(ArgSFullText, StringComparison.OrdinalIgnoreCase))
                    {
                        sFullText = true;

                    }
                    else if (arg.StartsWith(ArgReMergeLabReqs, StringComparison.OrdinalIgnoreCase))
                    {
                        int reMerge = ET.ParseInt32(arg.Substring(ArgReMergeLabReqs.Length));
                        if ((reMerge > 1) || (reMerge < 0))
                        {
                            TL.TraceLoggerInstance.TraceWarning($"{ArgReMergeLabReqs} can onl be 0 or 1 so setting to 0");
                            TL.TraceLoggerInstance.TraceWaringConsole($"{ArgReMergeLabReqs} can onl be 0 or 1 so setting to false");

                        }
                        else
                        {
                            if (reMerge == 1)
                                ReMerge = true;
                        }

                    }
                    else if (arg.StartsWith(ArgShowUsage, StringComparison.OrdinalIgnoreCase))
                    {
                        TL.TraceLoggerInstance.TraceInformation($"Show usage");
                        TL.TraceLoggerInstance.TraceInformationConsole($"Show usage");
                        ShowUsage();
                        return;
                    }
                    else if (arg.StartsWith(ArgMerge, StringComparison.OrdinalIgnoreCase))
                    {
                        Merged = ET.ParseInt32(arg.Substring(ArgMerge.Length));
                        if ((Merged > 1) || (Merged < 0))
                        {
                            TL.TraceLoggerInstance.TraceWarning($"{ArgMerge} can onl be 0 or 1 so setting to 0");
                            TL.TraceLoggerInstance.TraceWaringConsole($"{ArgMerge} can onl be 0 or 1 so setting to 0");
                            Merged = 0;
                        }
                    }
                    else if (arg.StartsWith(ArgCenterDateOfService, StringComparison.OrdinalIgnoreCase))
                    {
                        string sCenterDateOfService = arg.Substring(ArgCenterDateOfService.Length);
                        if (DateTime.TryParse(sCenterDateOfService, out centerDateOfService) == false)
                        {
                            centerDateOfService = DateTime.Now;
                            TL.TraceLoggerInstance.TraceWarning($"Invaild Arg {ArgCenterDateOfService} so using default to {DateTime.Now.ToString()}");
                        }
                        TL.TraceLoggerInstance.TraceInformation($"Arg {ArgCenterDateOfService} input {centerDateOfService.ToString()}");
                        TL.TraceLoggerInstance.TraceInformationConsole($"Arg {ArgCenterDateOfService} input {centerDateOfService.ToString()}");
                    }
                    else if (arg.StartsWith(ArgSetEmailPw, StringComparison.OrdinalIgnoreCase))
                    {
                        if (args.Length > 1)
                        {
                            ShowUsage();
                            TL.TraceLoggerInstance.TraceErrorConsole("Invalid argss");
                            throw new Exception("To many args");
                        }
                        TL.TraceLoggerInstance.TraceInformation($"setting email pw for {ArgSetEmailPw.Length}");
                        TL.TraceLoggerInstance.TraceInformationConsole($"setting email pw for {ArgSetEmailPw.Length}");
                        SE.Send_Emails.EmailInstance.UpDateEmailPw(arg.Substring(ArgSetEmailPw.Length));
                        return;
                    }
                    else
                    {
                        ShowUsage();
                        throw new Exception($"Invalid arg {arg}");
                    }
                }


                if (dateOfServiceSpan > 0)
                {
                    TL.TraceLoggerInstance.TraceInformation($"Getting labrecs for dateOfServiceSpan {dateOfServiceSpan} for centerdateofservice {centerDateOfService.ToString()} for merged {Merged}");
                    TL.TraceLoggerInstance.TraceInformationConsole($"Getting labrecs for dateOfServiceSpan {dateOfServiceSpan} for centerdateofservice {centerDateOfService.ToString()} for merged {Merged}");
                    string mergSpValue = ET.GetAppConfigSetting(AppConfigKeySPMergeLabRecs);
                    string updateSpValue = ET.GetAppConfigSetting(AppConfigKeySPUpDateLabRecsHL7);
                    TL.TraceLoggerInstance.TraceInformation($"Getting labrecs for merger sp {mergSpValue} for update sp {updateSpValue}");
                    TL.TraceLoggerInstance.TraceInformationConsole($"Getting labrecs for merger sp {mergSpValue} for update sp {updateSpValue}");
                    GetRecords(dateOfServiceSpan, centerDateOfService, mergSpValue, updateSpValue, sFullText);
                    mergSpValue = ET.GetAppConfigSetting(AppConfigKeySPMergeCovid19LabReqs);
                    updateSpValue = ET.GetAppConfigSetting(AppConfigKeySPUpDateCovid19HL7);
                    TL.TraceLoggerInstance.TraceInformation($"Getting covid-19 for merger sp {mergSpValue} for update sp {updateSpValue}");
                    TL.TraceLoggerInstance.TraceInformationConsole($"Getting covid-19 for merger sp {mergSpValue} for update sp {updateSpValue}");
                    if (!(sFullText))
                    {
                        if ((string.Compare(mergSpValue, NoCovid19, true) != 0) && (string.Compare(updateSpValue, NoCovid19, true) != 0))
                            GetRecords(dateOfServiceSpan, centerDateOfService, mergSpValue, updateSpValue, sFullText);
                        else
                        {
                            TL.TraceLoggerInstance.TraceInformation($"Skipping Covid-19 since merger sp {mergSpValue} for update sp {updateSpValue}");
                            TL.TraceLoggerInstance.TraceInformationConsole($"Skipping Covid-19 since merger sp {mergSpValue} for update sp {updateSpValue}");
                        }
                    }

                }
                else
                    throw new Exception($"Invalid date of service span:{dateOfServiceSpan}");

            }
            catch (Exception ex)
            {
                TL.TraceLoggerInstance.TraceError($"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.exe {ArgDateOfServiceSpan}10");
                TL.TraceLoggerInstance.TraceError($"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.exe {ArgDateOfServiceSpan}10 {ArgCenterDateOfService}10-23-2019");
                TL.TraceLoggerInstance.TraceError($"{ex.Message}");
                TL.TraceLoggerInstance.TraceErrorConsole($"{ex.Message}");
                ErrMess = $"{ErrMess} {ex.Message}";
            }
            TL.TraceLoggerInstance.TraceInformation($"End time of {ET.GetAssemblyTitle()} {DateTime.Now.ToString()}");
            executionTimer.Stop();
            TL.TraceLoggerInstance.TraceInformation($"Process {System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.exe  completed in {executionTimer.Elapsed}");
            TL.TraceLoggerInstance.TraceInformation($"Total lab recs read {TotalRead} total merged {TotalMerged} total not merged {TotalNotMerged}");
            TL.TraceLoggerInstance.TraceInformationConsole($"Process {System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.exe  completed in {executionTimer.Elapsed}");
            TL.TraceLoggerInstance.TraceInformationConsole($"with params {ArgDateOfServiceSpan}:{dateOfServiceSpan}<br/>{ArgCenterDateOfService}:{centerDateOfService}<br/>{ArgMerge}:{Merged}");
            TL.TraceLoggerInstance.TraceInformation($"with params {ArgDateOfServiceSpan}:{dateOfServiceSpan}<br/>{ArgCenterDateOfService}:{centerDateOfService}<br/>{ArgMerge}:{Merged}");
            if (string.IsNullOrWhiteSpace(ErrMess))
            {
                if (!(sFullText))
                    ErrMess = $"{ErrMess} No Errors found<br/> with params {ArgDateOfServiceSpan}:{dateOfServiceSpan}<br/>{ArgCenterDateOfService}:{centerDateOfService}<br/>{ArgMerge}:{Merged}";
                else
                    ErrMess = $"{ErrMess} No Errors found<br/> with params {ArgDateOfServiceSpan}:{dateOfServiceSpan}<br/>{ArgCenterDateOfService}:{centerDateOfService}<br/>{ArgMerge}:{Merged}<br/>Search full text";
                EmailSend(ErrMess, executionTimer.Elapsed, false);
            }
            else
            {
                if (!(sFullText))
                    ErrMess = $"{ErrMess}<br/> with params {ArgDateOfServiceSpan}:{dateOfServiceSpan}<br/>{ArgCenterDateOfService}:{centerDateOfService}<br/>{ArgMerge}:{Merged}";
                else
                    ErrMess = $"{ErrMess}<br/> with params {ArgDateOfServiceSpan}:{dateOfServiceSpan}<br/>{ArgCenterDateOfService}:{centerDateOfService}<br/>{ArgMerge}:{Merged}<br/>Search full text";

                EmailSend(ErrMess.Trim(), executionTimer.Elapsed, true);
            }


        }


        static void CheckMultiProcessingRunning(string[] args)
        {
            try
            {


                int processId = ET.CheckMuptiProcessingRunning("MergeDemographicsCloud");
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
                    // ET.EndTaskById(processId);
                    Environment.Exit(-1);
                }
                executionTimer.Stop();
                //OpenLogFile();
            }
            catch (Exception ex)
            {
                Environment.Exit(-1);
            }
        }
        static void GetRecords(int dateOfServiceSpan, DateTime centerDateOfService, string spNameMerge, string spNameUpDate, bool fullTextSearch)
        {

            DateTime startDt = centerDateOfService.Subtract(TimeSpan.FromDays(dateOfServiceSpan));
            DateTime endDt = centerDateOfService.Add(TimeSpan.FromDays(1.00));
            IDictionary<string, HL7Model> hl7Recs = new Dictionary<string, HL7Model>();
            // IDictionary<string, HL7Model> hL7s = new Dictionary<string, HL7Model>();
            List<int> hL7s = new List<int>();
            TL.TraceLoggerInstance.TraceInformation($"Getting labrecs for start date of serevice {startDt.ToString()}  to end date of service {endDt.ToString()}");
            TL.TraceLoggerInstance.TraceInformationConsole($"Getting labrecs for start date of serevice {startDt.ToString()}  to end date of service {endDt.ToString()}");
            SqlCmds sql = new SqlCmds();
            for (DateTime cDos = startDt; cDos <= endDt; cDos += TimeSpan.FromDays(1))
            {
                if (cDos > DateTime.Now)
                {
                    TL.TraceLoggerInstance.TraceWarning($"Labrecs start date of serevice {cDos.ToString()} bigger then  end date of service {endDt.ToString()} so skipping");
                    TL.TraceLoggerInstance.TraceWaringConsole($"Labrecs start date of serevice {cDos.ToString()} bigger then  end date of service {endDt.ToString()} so skipping");

                    //Debug.WriteLine("Date is in future, skipping "  + cDos.ToShortDateString());
                    continue;
                }
                TL.TraceLoggerInstance.TraceInformation($"Getting lab recs for date {startDt.ToString()}");
                TL.TraceLoggerInstance.TraceInformation($"Getting hl recs for start date {startDt.ToString()} and end date {endDt.ToString()}");
                foreach (LabRecsModel labRecsModel in sql.GetLabRecs(cDos, spNameMerge, MergeLabReqs))
                {
                    TotalRead++;
                    HL7Model hL7 = null;
                    // HL7Model hL7 = sql.GetHl7(startDt, endDt, labRecsModel, Merged);
                    if (fullTextSearch)
                    {
                        hL7 = sql.GetFullTextHL7(startDt, endDt, labRecsModel.LabReqID, Merged, dateOfServiceSpan);
                    }
                    else
                    {
                        hL7 = sql.GetHl7MatchDefault(startDt, endDt, labRecsModel, Merged, dateOfServiceSpan);
                    }
                    try
                    {
                        //if (hL7 == null)
                        //    hL7 = sql.GetHl7Match(startDt, endDt, labRecsModel, Merged, dateOfServiceSpan);

                        if (hL7 != null)
                        {
                            if (!(hl7Recs.ContainsKey(hL7.FinancialNumber)))
                                hl7Recs.Add(hL7.FinancialNumber, hL7);
                            TotalMerged++;
                            //if (Merged == 0)
                            //{
                            //    if (!(hL7s.ContainsKey(hL7.FinancialNumber)))
                            //        hL7s.Add(hL7.FinancialNumber, hL7);

                            //}
                            //  sql.UpdateLabRecs(hL7, labRecsModel, spNameUpDate).ConfigureAwait(false).GetAwaiter().GetResult();
                            sql.UpdateLRTable(hL7, labRecsModel, spNameUpDate).ConfigureAwait(false).GetAwaiter().GetResult();
                            sql.CheckLabReqMerg(labRecsModel.LabReqID);
                            if (Merged == 0)
                            {
                                if (!(hL7s.Contains(hL7.HL7RowID)))
                                    hL7s.Add(hL7.HL7RowID);
                                //   sql.UpDateMergeHL7Files(hL7s).ConfigureAwait(false).GetAwaiter().GetResult();
                                //sql.UpDateMergeHL7Files(hL7.HL7RowID).ConfigureAwait(false).GetAwaiter().GetResult();
                                //if (!(hL7s.ContainsKey(hL7.FinancialNumber)))
                                //  hL7s.Add(hL7.FinancialNumber, hL7);
                                //  break;

                            }
                        }
                        else
                        {
                            //sql.UpdateLRTable(hL7, labRecsModel, spNameUpDate).ConfigureAwait(false).GetAwaiter().GetResult();

                            if (ReMerge)
                            {
                                TL.TraceLoggerInstance.TraceInformation($"Running ReMerge {startDt.ToString()} and end date {endDt.ToString()} for labrec {labRecsModel.FinancialNumber}");
                                TL.TraceLoggerInstance.TraceInformationConsole($"Running ReMerge for start date {startDt.ToString()} and end date {endDt.ToString()} for labrec {labRecsModel.FinancialNumber}");
                                TotalMerged++;
                                sql.ReMerge(labRecsModel, spNameUpDate).ConfigureAwait(false).GetAwaiter().GetResult();
                            }
                            else
                            {
                                TotalNotMerged++;
                                TL.TraceLoggerInstance.TraceWarning($"No hl recs for start date {startDt.ToString()} and end date {endDt.ToString()} for labrec {labRecsModel.FinancialNumber}");
                                TL.TraceLoggerInstance.TraceWaringConsole($"No hl recs for start date {startDt.ToString()} and end date {endDt.ToString()} for labrec {labRecsModel.FinancialNumber}");
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        TotalNotMerged++;
                        TL.TraceLoggerInstance.TraceError($"Updating labrec id {labRecsModel.FinancialNumber} {ex.Message}");
                        TL.TraceLoggerInstance.TraceErrorConsole($"Updating labrec id {labRecsModel.FinancialNumber} {ex.Message}");
                        ErrMess = $"{ErrMess} Updating labrec id {labRecsModel.FinancialNumber} {ex.Message}";

                    }
                }

            }

            if ((hL7s != null) && (hL7s.Count > 0))
            {
                sql.UpDateMergeHL7Files(hL7s).ConfigureAwait(false).GetAwaiter().GetResult();

            }
            if ((hl7Recs != null) && (hl7Recs.Count() > 0))
                ChekForNotMerged(hl7Recs, startDt, endDt, spNameMerge, spNameUpDate).ConfigureAwait(false).GetAwaiter().GetResult();
            TotalSkipped = sql.TotalSkipped;
        }
        static async Task ChekForNotMerged(IDictionary<string, HL7Model> hl7Recs, DateTime startDt, DateTime endDt, string spNameMerge, string spNameUpDate)

        {
            
            SqlCmds sql = new SqlCmds();
            for (DateTime cDos = startDt; cDos <= endDt; cDos += TimeSpan.FromDays(1))
            {
                if (cDos > DateTime.Now)
                {
                    TL.TraceLoggerInstance.TraceWarning($"Labrecs start date of serevice {cDos.ToString()} bigger then  end date of service {endDt.ToString()} so skipping");
                    TL.TraceLoggerInstance.TraceWaringConsole($"Labrecs start date of serevice {cDos.ToString()} bigger then  end date of service {endDt.ToString()} so skipping");

                    //Debug.WriteLine("Date is in future, skipping "  + cDos.ToShortDateString());
                    continue;
                }
                foreach (LabRecsModel labRecsModel in sql.GetLabRecs(cDos, spNameMerge, 0))
                {
                    if (hl7Recs.TryGetValue(labRecsModel.IndexnumberNoZeros, out HL7Model value))
                    {

                        TotalMerged++;
                        //if (Merged == 0)
                        //{
                        //    if (!(hL7s.ContainsKey(hL7.FinancialNumber)))
                        //        hL7s.Add(hL7.FinancialNumber, hL7);

                        //}
                        //  sql.UpdateLabRecs(hL7, labRecsModel, spNameUpDate).ConfigureAwait(false).GetAwaiter().GetResult();
                        TotalReprocess++;
                        sql.UpdateLRTable(value, labRecsModel, spNameUpDate).ConfigureAwait(false).GetAwaiter().GetResult();
                        sql.CheckLabReqMerg(labRecsModel.LabReqID);

                    }

                }
            }
        }
    }
}
