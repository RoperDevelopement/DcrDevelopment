using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using HU = Edocs.HelperUtilities.Utilities;
using TL = EdocsUSA.Utilities.Logging;
using SE = Edocs.Send.Emails.Send_Emails;
using BS = Edocs.Upload.Azure.Blob.Storage.AzureBlobStorage;
using ABS = Edocs.Upload.Azure.Blob.Storage.AzureBlobStorage;
namespace Edocs.Check.CleanUp.Space
{
    class CleanSpace
    {
        static readonly double OneKb = 1024.0;
        static readonly string ArgCheckSpace = "/checkspace:";
        static readonly string ArgCleanup = "/del:";
        static readonly string ArgSearchPattern = "/srcpat:";
        static string SearchPattern = "*.*";
        static readonly string AppConfigKeyDelFilesRootFolder = "DelFilesRootFolder";
        static readonly string AppConfigKeyAuditLogUploadFolder = "AuditLogUploadFolder";
        static readonly string AppConfigKeyUploadAuditLogs = "UploadAuditLogs";
        static readonly string AppConfigKeyMinSpace = "MinSpace";
        static readonly string AppConfigKeyAzureBlobStorageConnectionString = "AzureBlobStorageConnectionString";
        static readonly string AppConfigKeyAzureBlobContanierAuditShare = "AzureBlobContanierAuditShare";
        static readonly string AppConfigKeyAzureBlobAccountName = "AzureBlobAccountName";
        static readonly string AppConfigKeyAzureBlobAccountKey = "AzureBlobAccountKey";
        static readonly string AppConfigKeySkippDirectory = "SkippDirectory";
        
        static readonly string AppConfigKeyAuditLogsFolder = "AuditLogsFolder";
        static readonly string RepStrApplicationDir = "{ApplicationDir}";
        static string folderLog = string.Empty;

        static string SkippDirectory
        {
            get
            {
                return HU.GetAppConfigSetting(AppConfigKeySkippDirectory);
            }
        }
        static string AzureBlobStorageConnectionString
        {
            get
            {
                return HU.GetAppConfigSetting(AppConfigKeyAzureBlobStorageConnectionString);
            }
        }
        static string AzureBlobContanierAuditShare
        {
            get
            {
                return HU.GetAppConfigSetting(AppConfigKeyAzureBlobContanierAuditShare);
            }
        }
        static string AzureBlobAccountName
        {
            get
            {
                return HU.GetAppConfigSetting(AppConfigKeyAzureBlobAccountName);
            }
        }
        static string AzureBlobAccountKey
        {
            get
            {
                return HU.GetAppConfigSetting(AppConfigKeyAzureBlobAccountKey);
            }
        }
        static string LogFolder
        {
            get
            {
                folderLog = HU.GetAppConfigSetting(AppConfigKeyAuditLogsFolder);
                folderLog = HU.ReplaceString(folderLog, RepStrApplicationDir, HU.GetApplicationDir());
                folderLog = $"{HU.CheckFolderPath(folderLog)}CheckCleanSpace_{DateTime.Now.ToString("MM_dd_yyyy_HH_mm_ss")}.log";
                return folderLog;
            }
            set
            {
                folderLog = value;
            }

        }
        static int TotalDelFiles
        { get; set; }
        static int TotalFilesRead
        { get; set; }
        static string Folders
        {
            get
            {
                return HU.GetAppConfigSetting(AppConfigKeyDelFilesRootFolder);
            }
        }
        static string ProcessRunning
        { get; set; }
        static int TotalErros
        { get; set; }
        static int MinDiskSpace
        {
            get
            {
                return HU.ParseInt32(HU.GetAppConfigSetting(AppConfigKeyMinSpace));
            }
        }
        static string AuditLogUploadFolder
        {
            get
            {
                return HU.GetAppConfigSetting(AppConfigKeyAuditLogUploadFolder);
            }
        }
        static bool UploadAuditLogs
        {
            get
            {
                return HU.GetBool(HU.GetAppConfigSetting(AppConfigKeyUploadAuditLogs));
            }
        }
        static StringBuilder ErrorsFound
        {
            get; set;
        }
        //  private const long OneMb = OneKb * 1024;

        static void Main(string[] args)
        {
            ErrorsFound = new StringBuilder();
            string logFile = string.Empty;
            try
            {

                ProcessRunning = "Did not run";
                logFile = LogFolder;

                TL.TraceLogger.TraceLoggerInstance.OpenTraceLogFile(logFile, "DelFiles", true);
                TL.TraceLogger.TraceLoggerInstance.RunningAssembley = HU.GetAssemblyTitle();
                TL.TraceLogger.TraceLoggerInstance.StartStopStopWatch();
                TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Running on machine {Environment.MachineName} for user {TL.TraceLogger.TraceLoggerInstance.UserName}");
                TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Assembly Title {HU.GetAssemblyTitle()}");
                TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Assembly Version {HU.GetAssemblyVersion()}");
                TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Assembly Description {HU.GetAssemblyDescription()}");
                GetInputArgs(args);




            }
            catch (Exception ex)
            {

                TL.TraceLogger.TraceLoggerInstance.TraceError($"Running {ex.Message}");
                TotalErros++;
                ErrorsFound.AppendLine(ex.Message);

            }
            TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Total Files Read {TotalFilesRead} Total Files Deleted {TotalDelFiles} Total Errors {TotalErros}");
            string ts = TL.TraceLogger.TraceLoggerInstance.StopStopWatch();

            EmailSend(ts).ConfigureAwait(false).GetAwaiter().GetResult();
            TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Done checking space Total Time {ts}");
            CopyUploadLog(logFile).ConfigureAwait(false).GetAwaiter().GetResult();
            //HU.DeleteFile(logFile);
        }

        static void GetInputArgs(string[] args)
        {
            TotalDelFiles = 0;
            TotalFilesRead = 0;
            bool checkSpace = false;
            bool delFiles = false;
          
            foreach (string inputArgs in args)
            {
                if (inputArgs.StartsWith(ArgCheckSpace, StringComparison.InvariantCultureIgnoreCase))
                {
                    checkSpace = HU.GetBool(inputArgs.Substring(ArgCheckSpace.Length));
                }
                if (inputArgs.StartsWith(ArgCleanup, StringComparison.InvariantCultureIgnoreCase))
                {
                    delFiles = HU.GetBool(inputArgs.Substring(ArgCleanup.Length));
                }
                if (inputArgs.StartsWith(ArgSearchPattern, StringComparison.InvariantCultureIgnoreCase))
                {
                    SearchPattern = inputArgs.Substring(ArgSearchPattern.Length);
                }


            }
            if (!(checkSpace) && (!(delFiles)))
                throw new Exception("Invalid input args");

            if (checkSpace)
            {
                ProcessRunning = "Check disk space";
                    CheckSpace().ConfigureAwait(false).GetAwaiter().GetResult();
            }
                
            if (delFiles)
            {
                ProcessRunning = "Check disk space and delete files";
                DeleFilesFolders().ConfigureAwait(false).GetAwaiter().GetResult();
                CheckSpace().ConfigureAwait(false).GetAwaiter().GetResult();
            }


        }
        static async Task EmailSend(string ts)
        {
            TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Sending email");
            try
            {
                string message = "No Errors found";
                if (ErrorsFound.Length > 0)
                    message = ErrorsFound.ToString();
                string subject = $"Execution Summary process that ran {ProcessRunning} running process {System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.exe on machine {Environment.MachineName} runtime {DateTime.Now.ToString()}"; ;
                if (ErrorsFound.Length > 0)
                    subject = $"Execution Summary error running process {System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.exe machine {Environment.MachineName} runtime {DateTime.Now.ToString()}";
                //SE.EmailInstance.(string.Empty)
                string body = "Summary"
                   + "<br/>Total Files Read: " + TotalFilesRead
                   + "<br/>Total Files Deleted: " + TotalDelFiles
                   + "<br/>Total Errors Found: " + TotalErros
                   + "<br/>Execution Time: " + ts
                   + "<br/>Process that ran: " + ProcessRunning

                   + "<br/>Error Message: " + message;
                SE.EmailInstance.SendEmail(string.Empty, body, subject, string.Empty, true, string.Empty);
                if (ErrorsFound.Length > 0)
                {

                    TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Sending txt");
                    message = $"Error running  {System.Reflection.Assembly.GetExecutingAssembly().GetName().Name} {message}";
                    SE.EmailInstance.SendTxtMessage(message, true);
                    TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Text Sent");
                }
                TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Email Sent");
            }
            catch (Exception ex)
            {
                TL.TraceLogger.TraceLoggerInstance.TraceError($"Error sending email {ex.Message}");
            }
            TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Done Method Sending email");
        }
        static async Task CopyUploadLog(string logFileName)
        {
            TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Method CopyUploadLog log file name: {logFileName}");
            TL.TraceLogger.TraceLoggerInstance.Dispose();
            if (!(UploadAuditLogs))
                TL.TraceLogger.TraceLoggerInstance.CopyTraceFiles(logFileName, AuditLogUploadFolder, true, true, "*.*");
            else
            {
                UpLoadLogAzure(logFileName).ConfigureAwait(false).GetAwaiter().GetResult();
            }
        }
        static async Task UpLoadLogAzure(string logFileName)
        {
            try
            {

            
            ABS.BlobStorageInstance.AzureBlobAccountKey = AzureBlobAccountKey;
            ABS.BlobStorageInstance.AzureBlobAccountName = AzureBlobAccountName;
            ABS.BlobStorageInstance.AzureBlobStorageConnectionString = AzureBlobStorageConnectionString;
            logFileName = Path.GetDirectoryName(logFileName);
            foreach (var file in HU.GetDirFilesName(logFileName))
            {
                string fileName = Path.Combine(logFileName, file);
                string upLoadText = File.ReadAllText(fileName, System.Text.Encoding.UTF8);

                ABS.BlobStorageInstance.UploadAzureBlobTextFile(file, AzureBlobContanierAuditShare, upLoadText).ConfigureAwait(false).GetAwaiter().GetResult();

                HU.DeleteFile(fileName);
            }
            }
            catch(Exception ex)
            {
                try
                {


                    string message = $"Error uploading clean logs to azure cloud running assembly {System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.exe on machine {Environment.MachineName} runtime {DateTime.Now.ToString()} {ex.Message}";
                    SE.EmailInstance.SendEmail(string.Empty, ex.Message, $"Error uploading clean logs azure cloud machine {Environment.MachineName}", string.Empty, false, message);

                }
                catch { }
            }

        }
        static async Task DeleFilesFolders()
        {
            TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Method DeleFilesFolders for folders {Folders}");
            foreach (string drive in Folders.Split(','))
            {
                try
                {

                    string skippDir = SkippDirectory;
                    string[] dirFiles = null;
                    string[] checkFolders = drive.Split(';');
                    int numDays = HU.ParseInt(checkFolders[1]);
                    string[] dir = GetDirectories(checkFolders[0], SearchPattern, SearchOption.AllDirectories).ConfigureAwait(false).GetAwaiter().GetResult();
                    if ((dir == null) || (dir.Length == 0))
                    {
                        ProcessDirFiles(checkFolders[0], SearchPattern, SearchOption.AllDirectories, numDays).ConfigureAwait(true).GetAwaiter().GetResult();
                    }
                    else
                    {
                        foreach (string dirName in dir)
                        {
                            if (dirName.ToLower().Contains(skippDir.ToLower()))
                            {
                                TL.TraceLogger.TraceLoggerInstance.TraceWarning($"{dirName} since found skip {skippDir} in app config");
                                continue;
                            }
                                
                            ProcessDirFiles(dirName, SearchPattern, SearchOption.AllDirectories, numDays).ConfigureAwait(true).GetAwaiter().GetResult();
                        }
                    }
                    //  HU.DelDirectory(checkFolders[0],"*.",SearchOption.AllDirectories, true).ConfigureAwait(true).GetAwaiter().GetResult();
                }
                catch (Exception ex)
                {
                    TotalErros++;
                    TL.TraceLogger.TraceLoggerInstance.TraceError($"Method DeleFilesFolders for drive {drive}");
                }

            }
            DeleteEmptyFolders().ConfigureAwait(true).GetAwaiter().GetResult();
            TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Done Method DeleFilesFolders for folders {Folders}");
        }
        static async Task DeleteEmptyFolders()
        {
            string[] dirFiles = null;
            TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Method DeleteEmptyFolders");
            foreach (string drive in Folders.Split(','))
            {
                try
                {
                    string[] checkFolders = drive.Split(';');
                    string skippDir = SkippDirectory;
                    int numDays = HU.ParseInt(checkFolders[1]);
                    string[] dir = GetDirectories(checkFolders[0], SearchPattern, SearchOption.AllDirectories).ConfigureAwait(false).GetAwaiter().GetResult();
                    if ((dir == null) || (dir.Length == 0))
                    {
                        HU.DelDirectory(checkFolders[0], SearchPattern, SearchOption.AllDirectories, true).ConfigureAwait(true).GetAwaiter().GetResult();
                    }
                    else
                    {
                        foreach (string dirName in dir)
                        {
                            if (dirName.ToLower().Contains(skippDir.ToLower()))
                            {
                                TL.TraceLogger.TraceLoggerInstance.TraceWarning($"{dirName} since found skip {skippDir} in app config");
                                continue;
                            }
                            HU.DelDirectory($"{dirName}\\", SearchPattern, SearchOption.AllDirectories, true).ConfigureAwait(true).GetAwaiter().GetResult();
                        }
                    }
                }
                catch (Exception ex)
                {
                    TotalErros++;
                    TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Method DeleteEmptyFolders {ex.Message}");
                }
            }
            TL.TraceLogger.TraceLoggerInstance.TraceInformation($"DOne Method DeleteEmptyFolders");
        }
        static async Task ProcessDirFiles(string path, string searchPattern, SearchOption searchOpt, int numDaysKeepFile)
        {
            TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Method ProcessDirFiles for path {path} search pattern {searchPattern} search opt {searchOpt} number days to keep files {numDaysKeepFile} ");
            int totalFilesRead = 0;
            int totalFilesDel = 0;
            try
            {

                string[] dirFiles = GetDirectoryFiles(path, SearchPattern, searchOpt).ConfigureAwait(false).GetAwaiter().GetResult();
                if ((dirFiles != null) && (dirFiles.Length != 0))
                {
                    DateTime currDT = DateTime.Now;
                    foreach (string dirFileName in dirFiles)
                    {
                        FileInfo info = new FileInfo(dirFileName);
                        TimeSpan span = currDT - info.LastWriteTime;
                        totalFilesRead++;
                        if ((Int32)span.TotalDays > numDaysKeepFile)
                        {
                            TL.TraceLogger.TraceLoggerInstance.TraceWarning($"Deleting file {info.FullName}");
                            HU.DeleteFile(info.FullName);
                            totalFilesDel++;
                        }
                    }
                }
                HU.DelDirectory(path, searchPattern, searchOpt, true).ConfigureAwait(true).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                TotalErros++;
                TL.TraceLogger.TraceLoggerInstance.TraceError($"Method ProcessDirFiles {ex.Message}");
            }
            TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Total Files Read {totalFilesRead} total files deleted {totalFilesDel} for path {path}");
            TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Done Method ProcessDirFiles for path {path} search pattern {searchPattern} search opt {searchOpt} number days to keep files {numDaysKeepFile} ");
            TotalFilesRead += totalFilesRead;
            TotalDelFiles += totalFilesDel;
        }
        static async Task<string[]> GetDirectories(string path, string searchPattern, SearchOption searchOpt)
        {
            return HU.GetDirectories(path, searchPattern, searchOpt).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        static async Task<string[]> GetDirectoryFiles(string path, string searchPattern, SearchOption searchOpt)
        {
            return HU.GetDirectoryFiles(path, searchPattern, searchOpt).ConfigureAwait(true).GetAwaiter().GetResult();
        }
        static async Task CheckSpace()
        {
            TL.TraceLogger.TraceLoggerInstance.TraceInformation("Starting Method CheckSpace");
            try
            {


                DriveInfo[] allDrives = DriveInfo.GetDrives();
                int minSpace = MinDiskSpace;
                foreach (DriveInfo infoDrive in allDrives)
                {
                    TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Checking drive {infoDrive.Name}");
                    if (infoDrive.DriveType == DriveType.Fixed)
                    {
                        double currSpace = ConvertBytesToMegabytes(infoDrive.TotalFreeSpace);
                        currSpace = ConvertMegabytesToGigabytes(currSpace);
                        if ((int)currSpace <= minSpace)
                        {
                            ErrorsFound.AppendLine($"Current space on drive {infoDrive.Name} {currSpace} and min space is {minSpace}");
                            TL.TraceLogger.TraceLoggerInstance.TraceError($"Current space {(int)currSpace} on drive {infoDrive.Name} less then min space  {minSpace}");
                        }
                        else
                        {
                            TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Current space {(int)currSpace} on drive {infoDrive.Name} under min space is {minSpace}");
                        }
                    }
                    else
                    {
                        
                        TL.TraceLogger.TraceLoggerInstance.TraceWarning($"Skipping drive {infoDrive.Name} since drive type is {infoDrive.DriveType}");
                    }

                }
            }
            catch (Exception ex)
            {
                TotalErros++;
                TL.TraceLogger.TraceLoggerInstance.TraceError($"Method CheckSpace {ex.Message}");
            }
            TL.TraceLogger.TraceLoggerInstance.TraceInformation("Completed Method CheckSpace");
        }
        static double ConvertMegabytesToGigabytes(double megabytes) // SMALLER
        {
            // 1024 megabyte in a gigabyte
            return megabytes / OneKb;
        }

        static double ConvertBytesToMegabytes(long bytes)
        {
            return (bytes / OneKb) / OneKb;
        }

        static double ConvertKilobytesToMegabytes(long kilobytes)
        {
            return kilobytes / OneKb;
        }
    }
}
