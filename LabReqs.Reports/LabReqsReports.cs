using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Edocs.Libaray.AzureCloud.Upload.Batches;
using Edocs.Libaray.JsonSettingsFileModel;
using EH = Edocs.HelperUtilities.Utilities;
using SE = Edocs.Send.Emails.Send_Emails;
namespace Edocs.LabReqs.Reports
{
    class LabReqsReports
    {
        private static readonly string JsonRecordsExten = "_records.json";
        private static readonly string JsonBatchSettingExten = "*_settings.json";
        private static readonly string WebApiLabReqReports = "LabReqReports/";
        //private static readonly string WebUrl = "http://localhost:5555/api/";
        private static readonly string ArgsJsonFolder = "/jfolder:";
        private static readonly string ArgsDateAdd = "/dateadd:";
        private static readonly string ArgsJsonBackFolder = "/jsbfolder:";
        private static readonly string ArgsMoveJsFiles = "/mjsfiles:";

        private static string LabReqsJsonFolder
        {
            get { return EH.GetAppConfigSetting("LabReqsJsonFolder"); }
        }
        private static string LabReqArchiveFolder
        {
            get { return EH.GetAppConfigSetting("LabReqArchiveFolder"); }
        }
        private static string LabReqsJsonArchiveFolder
        { get { return EH.GetAppConfigSetting("LabReqsJsonArchiveFolder"); } }
        private static string WebUrl
        { get { return EH.GetAppConfigSetting("WebApi"); } }
        private static string SaveEmailReport
        { get { return EH.GetAppConfigSetting("SaveEmailReport"); } }

        private static string JsonFilesOld
        { get { return EH.GetAppConfigSetting("JsonOldFiles"); } }
        private static string JsonBackFolder
        { get { return EH.GetAppConfigSetting("JsonBackFolder"); } }
        private static string EmailSubject
        { get { return EH.GetAppConfigSetting("EmailSubject"); } }

        private static string EmailTo
        { get { return EH.GetAppConfigSetting("EmailTo"); } }
        private static string EmailCC
        { get { return EH.GetAppConfigSetting("EmailCC"); } }


        private static StringBuilder SbErrors
        { get; set; }

        static void Main(string[] args)
        {
            if ((args.Length ==1) && (args[0] == "/?"))
            {
                PrintUsage();
            }
            else
                GetInputArgs(args);


        }
        private static void PrintUsage()
        {
            Console.WriteLine("Usage");
            Console.WriteLine("Edocs.LabReqs.Reports.exe /? for usage");
            Console.WriteLine($"Edocs.LabReqs.Reports.exe not paramaters to process files in folder {LabReqsJsonFolder}");
            Console.WriteLine($"Edocs.LabReqs.Reports.exe {ArgsDateAdd} number of days to take off current date to process files in folder {LabReqArchiveFolder}");
            Console.WriteLine($"Edocs.LabReqs.Reports.exe {ArgsDateAdd}-10");
            Console.WriteLine($"Edocs.LabReqs.Reports.exe {ArgsDateAdd} 0 to process files in folder {LabReqsJsonFolder}");
            Console.WriteLine($"Edocs.LabReqs.Reports.exe {ArgsDateAdd}0");
            Console.WriteLine($"Edocs.LabReqs.Reports.exe {ArgsMoveJsFiles} true to move the jsonfiles from folder {LabReqsJsonFolder} to folder {LabReqsJsonArchiveFolder}");
            Console.WriteLine($"Edocs.LabReqs.Reports.exe {ArgsMoveJsFiles}true");
            Console.WriteLine($"Edocs.LabReqs.Reports.exe {ArgsJsonFolder} the jsonfolder to process");
            Console.WriteLine($"Edocs.LabReqs.Reports.exe {ArgsJsonFolder}{LabReqsJsonArchiveFolder}");
            Console.WriteLine($"Edocs.LabReqs.Reports.exe {ArgsJsonFolder}{LabReqsJsonFolder}");
            Console.WriteLine($"Edocs.LabReqs.Reports.exe {ArgsDateAdd}-10 {ArgsJsonFolder}{LabReqsJsonArchiveFolder}");
            Console.WriteLine($"Edocs.LabReqs.Reports.exe {ArgsDateAdd}-10 {ArgsMoveJsFiles}true {ArgsJsonFolder}{LabReqsJsonFolder}");
            Console.WriteLine($"Edocs.LabReqs.Reports.exe {ArgsDateAdd}0 {ArgsMoveJsFiles}true {ArgsJsonFolder}{LabReqsJsonFolder}");
            Console.WriteLine($"Edocs.LabReqs.Reports.exe {ArgsDateAdd}0 {ArgsJsonFolder}{LabReqsJsonFolder}");
            Console.WriteLine($"Edocs.LabReqs.Reports.exe {ArgsJsonFolder}{LabReqsJsonFolder}");
            Console.WriteLine();
        }
        private static void GetInputArgs(string[] args)
        {
            string jsonFolder = string.Empty;
            SbErrors = new StringBuilder();
            int dateAdd = 0;
            string jsonBakFolder = string.Empty;
            bool moveJsFiles = false;
            try
            {
                if (args.Length > 4)
                {
                    string invArgs = "Invalid args entered";
                    foreach (string invaild in args)
                    {
                        invArgs = $"{invArgs} {invaild}";
                    }
                    invArgs = $"{invArgs} run Edocs.LabReqs.Reports.exe /? for usage";
                    Console.WriteLine(invArgs);
                    throw new Exception(invArgs);
                }


                foreach (string inputArgs in args)
                {
                    // edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Looking for input arg:{inputArgs}");
                    if (inputArgs.StartsWith(ArgsJsonFolder, StringComparison.InvariantCultureIgnoreCase))
                    {
                        jsonFolder = inputArgs.Substring(ArgsJsonFolder.Length);
                        //   edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Processing arg:{inputArgs} for folders");
                        //   edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Processing arg:{inputArgs} for folders");
                        //   UploadBatchesByFolder(inputArgs.Substring(ArgFolder.Length)).ConfigureAwait(false).GetAwaiter().GetResult();



                    }
                    else if (inputArgs.StartsWith(ArgsJsonBackFolder, StringComparison.InvariantCultureIgnoreCase))
                    {
                        jsonBakFolder = inputArgs.Substring(ArgsJsonBackFolder.Length);
                    }
                    else if (inputArgs.StartsWith(ArgsMoveJsFiles, StringComparison.InvariantCultureIgnoreCase))
                    {
                        moveJsFiles = Edocs.HelperUtilities.Utilities.GetBool(inputArgs.Substring(ArgsMoveJsFiles.Length));
                    }

                    else if (inputArgs.StartsWith(ArgsDateAdd, StringComparison.InvariantCultureIgnoreCase))
                    {
                        dateAdd = int.Parse(inputArgs.Substring(ArgsDateAdd.Length));
                        //   edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Processing arg:{inputArgs} for folders");
                        //   edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Processing arg:{inputArgs} for folders");
                        //   UploadBatchesByFolder(inputArgs.Substring(ArgFolder.Length)).ConfigureAwait(false).GetAwaiter().GetResult();



                    }
                    else
                    {
                        throw new Exception($"Invlaid arg {inputArgs}");

                    }

                }
                if (string.IsNullOrWhiteSpace(jsonBakFolder))
                    jsonBakFolder = JsonBackFolder;
                if (dateAdd >= 0)
                {
                    if (string.IsNullOrWhiteSpace(jsonFolder))
                        jsonFolder = LabReqsJsonFolder;
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(jsonFolder))
                        jsonFolder = LabReqsJsonArchiveFolder;
                }
                if ((string.Compare(HelperUtilities.Utilities.CheckFolderPath(jsonFolder), HelperUtilities.Utilities.CheckFolderPath(LabReqsJsonArchiveFolder), true) == 0) && (moveJsFiles))
                    throw new Exception($"Invalid args {ArgsJsonFolder}{jsonFolder} and {ArgsMoveJsFiles}{moveJsFiles} cannot move archive folder");
                BuildReport(jsonFolder, dateAdd, jsonBakFolder, moveJsFiles).ConfigureAwait(false).GetAwaiter().GetResult();

            }
            catch (Exception ex)
            {
                string message = $"Error running report error message {ex.Message}";
                Console.WriteLine(message);
                SEmails(message, true, string.Empty, false).ConfigureAwait(false).GetAwaiter().GetResult();


            }



        }
        private static void CopyJsFilesToArchive(string jsFolder, string archiveFolder,bool delSFile)
        {
            foreach (string dirN in Edocs.HelperUtilities.Utilities.GetDirectoryFiles(jsFolder, "*.*", SearchOption.TopDirectoryOnly).ConfigureAwait(false).GetAwaiter().GetResult())
            {
                try
                {
                    string desFname = Path.Combine(archiveFolder, Path.GetFileName(dirN));
                    Edocs.HelperUtilities.Utilities.CopyFile(dirN, desFname,true);
                    if(delSFile)
                    Edocs.HelperUtilities.Utilities.DeleteFile(dirN);
                }
                catch { }
            }


        }
        private static List<string> GetOtherJsFiles(string curretJsFolder, string oldJsFolder, ref int addtodate)
        {
            List<string> retList = new List<string>();
            DateTime currDateTime = DateTime.Now;
            int dateToAdd = 0;
            try
            {
                foreach (string fileJson in Directory.GetFiles(oldJsFolder, "*.*"))
                {
                    if (addtodate < 0)
                    {
                        if (fileJson.ToLower().EndsWith("_settings.json"))
                        {
                            FileInfo info = new FileInfo(fileJson);
                            if (info.CreationTime < currDateTime)
                            {
                                TimeSpan ts = info.CreationTime - DateTime.Now;
                                dateToAdd = ts.Days;
                                currDateTime = info.CreationTime;
                            }


                        }
                    }
                    string destFname = Path.Combine(curretJsFolder, Path.GetFileName(fileJson));
                    Edocs.HelperUtilities.Utilities.CopyFile(fileJson, destFname,true);

                    retList.Add(fileJson);

                }
                if (addtodate < 0)
                {
                    if (Math.Abs(dateToAdd) > Math.Abs(addtodate))
                        addtodate = dateToAdd;
                    if (addtodate > 0)
                        addtodate *= -1;
                }
            }
            catch { }
            return retList;
        }
        private static async Task DeleteOldFile(List<string> oldFiles)
        {
            foreach (string s in oldFiles)
            {
                try
                {
                    Edocs.HelperUtilities.Utilities.DeleteFile(s);
                }
                catch { }


            }

        }
        private static async Task<StringBuilder> CheckForMissingUpLoads(StringBuilder sb)
        {

            foreach (string archiveFolder in HelperUtilities.Utilities.GetDirectories(LabReqArchiveFolder, "*.*", SearchOption.TopDirectoryOnly).ConfigureAwait(false).GetAwaiter().GetResult())
            {
                Console.WriteLine(archiveFolder);
                foreach (string archiveSubFolder in HelperUtilities.Utilities.GetDirectories(archiveFolder, "*.*", SearchOption.TopDirectoryOnly).ConfigureAwait(false).GetAwaiter().GetResult())
                {

                    string[] myStr = archiveSubFolder.Split('\\');

                    string missingArchiver = myStr[myStr.Length - 2];
                    string missingBatchID = myStr[myStr.Length - 1];
                    SbErrors.AppendLine($"<p style={WebApi.Quote}color:red{WebApi.Quote}>Error archiver {missingArchiver} BatchID {missingBatchID} found under folder {archiveSubFolder} </p>");
                }
            }
            return sb;
        }
        private static async Task BuildReport(string jsonFolderName, int addToDate, string jsonBakFolder, bool moveJsFiles)
        {
            if (!(Directory.Exists(jsonFolderName)))
                throw new Exception($"Json files folder {jsonFolderName} not found ");

            List<string> otherJsFiles = GetOtherJsFiles(jsonFolderName, jsonBakFolder, ref addToDate);
            StringBuilder sb = WebApi.CreateHeder();

            List<string> scanDates = new List<string>();
            DateTime processDate = DateTime.Now.AddDays(addToDate);
            string restApi = WebUrl;
            bool errs = false;
            string bId = string.Empty;
            if((addToDate < 0) && (string.Compare(HelperUtilities.Utilities.CheckFolderPath(LabReqsJsonFolder), HelperUtilities.Utilities.CheckFolderPath(jsonFolderName),true) == 0))
                CopyJsFilesToArchive(LabReqsJsonFolder,jsonFolderName, false);
            foreach (string fileJson in Directory.GetFiles(jsonFolderName, JsonBatchSettingExten))
            {
                try
                {
                    if (addToDate < 0)
                    {
                        FileInfo info = new FileInfo(fileJson);
                        if (info.CreationTime.Year != processDate.Year)
                        {

                            continue;
                        }
                        if (String.Compare(info.CreationTime.ToString("MM-dd-yyyy"), processDate.ToString("MM-dd-yyyy")) < 0)
                            continue;
                    }
                    Console.WriteLine($"Processing {fileJson}");
                    int totalRecords = 0;
                    int totalUpLoaded = 0;
                    JsonSettingsFileModel batchSettings = WebApi.GetBatchSettingsObject<JsonSettingsFileModel>(fileJson);
                    string folderName = Path.GetDirectoryName(fileJson);
                    string recordsFName = $"{batchSettings.ScanBatch}{JsonRecordsExten}";
                    bId = batchSettings.ScanBatch;
                    totalRecords = WebApi.GetTotalRecords($"{Path.Combine(folderName, recordsFName)}");
                    if (string.Compare(batchSettings.AzureTableName, "LabReqisitions", true) == 0)
                        batchSettings.AzureTableName = "LabRequisition";
                    if (!(scanDates.Contains(batchSettings.ScanDate.ToString("MM-dd-yyyy"))))
                        scanDates.Add(batchSettings.ScanDate.ToString("MM-dd-yyyy"));
                    if (totalRecords > 0)
                        totalUpLoaded = WebApi.GetLabRecsSent(restApi, WebApiLabReqReports, batchSettings.ScanBatch, recordsFName, batchSettings.AzureTableName, totalRecords, batchSettings.ScanDate.ToString()).ConfigureAwait(false).GetAwaiter().GetResult(); ;

                    if (totalRecords != totalUpLoaded)
                    {
                        errs = true;
                        SbErrors.AppendLine($"<p style={WebApi.Quote}color:red{WebApi.Quote}> Scanned totals don't match Azure totals for BatchID {batchSettings.ScanBatch}</p>");
                        sb.AppendLine($"<tr style ={WebApi.Quote} background-color:red{WebApi.Quote}>");
                    }
                    else
                        sb.AppendLine("<tr>");
                    sb.AppendLine($"<td>{batchSettings.ScanDate.ToString()}</td>");
                    sb.AppendLine($"<td>{batchSettings.ScanBatch}</td>");
                    sb.AppendLine($"<td>{batchSettings.AzureTableName}</td>");
                    sb.AppendLine($"<td>{totalRecords.ToString()}</td>");
                    if (totalUpLoaded < 0)
                        sb.AppendLine($"<td>Batch {batchSettings.ScanBatch} not found in Azure</td>");
                    else
                        sb.AppendLine($"<td>{totalUpLoaded.ToString()}</td>");
                    sb.AppendLine($"</tr>");
                    string procesedJFiles = Path.Combine(JsonFilesOld, recordsFName);


                }
                catch (Exception ex)
                {
                    errs = true;
                    //Console.WriteLine();
                    SbErrors.AppendLine($"<p style={WebApi.Quote}color:red{WebApi.Quote}> Error processing BatchID {bId} message {ex.Message}</p>");
                }
            }
            if ((scanDates == null) || (scanDates.Count < 1))
            {
                scanDates.Add(DateTime.Now.ToString("MM-dd-yyyy"));
                sb.AppendLine("<tr>");
                sb.AppendLine($"<td>N/A</td>");
                sb.AppendLine($"<td>N/A</td>");
                sb.AppendLine($"<td>0</td>");
                sb.AppendLine($"<td>0</td>");
                sb.AppendLine($"<td>0</td>");
                sb.AppendLine($"</tr>");
            }

            scanDates.Sort();
            SbErrors = CheckForMissingUpLoads(SbErrors).ConfigureAwait(false).GetAwaiter().GetResult();
            if (SbErrors.Length == 0)
            {
                SbErrors.AppendLine($"<p style={WebApi.Quote}color:#00f;{WebApi.Quote}>No differences found</p>");
            }
            
            string minScanDate = scanDates[0];
            string maxScanDate = scanDates[scanDates.Count - 1];
            sb.AppendLine("</table>");
            sb.AppendLine("<br/>");
            sb.AppendLine("<br/>");
            sb.AppendLine(SbErrors.ToString());
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");
            string output = sb.ToString();
            output = output.Replace("ScanStartDate", minScanDate);
            output = output.Replace("ScanEndDate", maxScanDate);
            string saveReport = Path.Combine(jsonFolderName, SaveEmailReport);
            saveReport = Path.Combine(saveReport, "AzureUploadReport.html");

            EH.WriteOutPut(saveReport, output);
            if ((otherJsFiles.Count > 0) && (moveJsFiles))
                DeleteOldFile(otherJsFiles).ConfigureAwait(false).GetAwaiter().GetResult();
            if ((moveJsFiles) && ( SbErrors.ToString().Contains("No differences found")))
                CopyJsFilesToArchive(jsonFolderName, LabReqsJsonArchiveFolder,true);
            SEmails(output, errs, saveReport, true).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private static async Task SEmails(string html, bool err, string emailAtt, bool emailHtml)
        {
            try
            {
                
                string eSubject = string.Empty;
                if (err)
                    eSubject = EmailSubject.Replace("{Error}", "Errors Found Running Report").Replace("{reportTime}", DateTime.Now.ToString());
                else
                    eSubject = EmailSubject.Replace("{Error}", "").Replace("{reportTime}", DateTime.Now.ToString());


                if (!(string.IsNullOrWhiteSpace(emailAtt)))
                    SE.EmailInstance.SendEmail(EmailTo, EmailCC, emailAtt, eSubject, emailAtt, emailHtml, string.Empty);
                else
                    SE.EmailInstance.SendEmail(EmailTo, EmailCC, html, eSubject, string.Empty, emailHtml, string.Empty);

            }
            catch { }


        }
    }
}
