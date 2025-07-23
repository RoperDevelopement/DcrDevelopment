using EdocsUSA.Utilities.Extensions;
using EdocsUSA.Utilities.Interop;
using Scanquire.Clients.NYP;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using SP = Microsoft.SharePoint.Client;
using LF = Edocs.Logger;
namespace SharepointBatchUploader
{
    class Program
    {
        //@vtext.com
        static Settings.Batch BatchSettings = Settings.Batch.Default;
        static string LogFolder = string.Format("{0}\\Local\\EdocsUsaSharePointUpload\\logs", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
        static void Main(string[] args)
        {
            Trace.Listeners.Add(new ConsoleTraceListener());
            LF.Logger.LogInstance.LogFileName = string.Format("{0}\\SharePointUpload_{1}.log", LogFolder, DateTime.Now.ToString("yyyy_MM_dd_HH_MM_ss_ff"));
            LF.Logger.LogInstance.WriteLoggingLogFile(string.Format("Opening log file:{0}",LF.Logger.LogInstance.LogFileName), false, LF.LoggingErrorType.Info);
            DeleteLogFiles();
            try
            {
                //Process the command line arguments
                string batchDir = null;

                foreach (string arg in args)
                {
                    if (arg.StartsWith("/batchdir:", StringComparison.OrdinalIgnoreCase))
                    { batchDir = arg.Substring("/batchdir:".Length); }
                }

                //If any command line args were ommited, flash the window, then prompt for entry
                if (string.IsNullOrWhiteSpace(batchDir))
                { FlashWindow.Flash(FlashWindow.FLASHW_TIMERNOFG); }

                if (string.IsNullOrWhiteSpace(batchDir))
                {
                    Console.WriteLine("Batch Directory:");

                    batchDir = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(batchDir))
                    { throw new ArgumentNullException("BatchId", "Batch ID is required"); }
                }

                Trace.TraceInformation("Processing input dir " + batchDir);
                LF.Logger.LogInstance.WriteLoggingLogFile(string.Format("Processing input dir:{0}", batchDir), false, LF.LoggingErrorType.Info);
                if (Directory.Exists(batchDir) == false)
                { throw new DirectoryNotFoundException("Cannot find batch directory at " + batchDir); }

                Trace.TraceInformation("Reading batch settings");
                LF.Logger.LogInstance.WriteLoggingLogFile("Reading batch settings", false, LF.LoggingErrorType.Info);

                SharepointBatchSettings batchSettings = SharepointBatchHelper.ReadSettings(batchDir);
                
                Trace.TraceInformation("Reading batch records");
                LF.Logger.LogInstance.WriteLoggingLogFile("Reading batch records", false, LF.LoggingErrorType.Info);
                SharepointBatchRecord[] batchRecords = SharepointBatchHelper.ReadRecords(batchDir).ToArray();
                Trace.TraceInformation("Processing " + batchRecords.Length + " batch records");
                LF.Logger.LogInstance.WriteLoggingLogFile(string.Format("Processing:{0} batch records", batchDir.Length.ToString()), false, LF.LoggingErrorType.Info);

                Trace.TraceInformation("Connecting to " + batchSettings.SiteUrl);
                LF.Logger.LogInstance.WriteLoggingLogFile(string.Format("Connecting to share point site:{0}", batchSettings.SiteUrl), false, LF.LoggingErrorType.Info);
                using (SP.ClientContext context = new SP.ClientContext(batchSettings.SiteUrl))
                {                    
                    context.RequestTimeout = 300000;
                    context.AuthenticationMode = SP.ClientAuthenticationMode.FormsAuthentication;
                    Trace.TraceInformation("Setting u/p to " + batchSettings.UserName + "/" + batchSettings.Password);
                    LF.Logger.LogInstance.WriteLoggingLogFile(string.Format("Setting u/p username:{0} password:{1}", batchSettings.UserName, batchSettings.Password), false, LF.LoggingErrorType.Info);
                    context.FormsAuthenticationLoginInfo = new SP.FormsAuthenticationLoginInfo(batchSettings.UserName, batchSettings.Password);
                    Trace.TraceInformation("Getting web");
                    LF.Logger.LogInstance.WriteLoggingLogFile("Getting web", false, LF.LoggingErrorType.Info);
                    SP.Web web = context.Web;
                    Trace.TraceInformation("Getting library " + batchSettings.LibraryName);
                    LF.Logger.LogInstance.WriteLoggingLogFile(string.Format("Getting library:{0}", batchSettings.LibraryName), false, LF.LoggingErrorType.Info);
                    SP.List library = GetListByTitle(context, batchSettings.LibraryName);
                    Trace.TraceInformation("Library id is " + library.Id);
                    LF.Logger.LogInstance.WriteLoggingLogFile(string.Format("Library id is:{0}",library.Id), false, LF.LoggingErrorType.Info);
                    Trace.TraceInformation("Ensuring/Creating subfolder " + batchSettings.FolderRelativeUrl);
                    LF.Logger.LogInstance.WriteLoggingLogFile(string.Format("Ensuring/Creating subfolder:{0}", batchSettings.FolderRelativeUrl), false, LF.LoggingErrorType.Info);
                    SP.Folder folder = CreateSubfolders(context, library, batchSettings.FolderRelativeUrl);

                    int batchRecordCounter = 0;
                    foreach (SharepointBatchRecord batchRecord in batchRecords)
                    {
                        batchRecordCounter++;
                        Trace.TraceInformation("Uploading record " + batchRecordCounter + " of " + batchRecords.Length);
                        LF.Logger.LogInstance.WriteLoggingLogFile(string.Format("Uploading record:{0} of {1}", batchRecordCounter, batchRecords.Length), false, LF.LoggingErrorType.Info);
                        //Read the records file content
                        string batchRecordFilePath = Path.Combine(batchDir, batchRecord.FileName);
                        byte[] batchRecordFileData = File.ReadAllBytes(batchRecordFilePath);
                        using (MemoryStream stream = new MemoryStream(batchRecordFileData))
                        {

                            SP.FileCreationInformation batchRecordFileCreationInfo = new SP.FileCreationInformation()
                            {
                                ContentStream = stream,
                                Url = batchRecord.FileName,
                                Overwrite = true
                            };
                            SP.File batchRecordFile = folder.Files.Add(batchRecordFileCreationInfo);

                            //Apply the common fields metadata
                            foreach (KeyValuePair<string, string> field in batchSettings.CommonFields)
                            { batchRecordFile.ListItemAllFields[field.Key] = field.Value; }

                            //Apply the record specific metadata
                            foreach (KeyValuePair<string, string> field in batchRecord.Fields)
                            { batchRecordFile.ListItemAllFields[field.Key] = field.Value; }
                            batchRecordFile.ListItemAllFields.Update();
                            context.ExecuteQuery();
                        }
                    }
                }
                if (BatchSettings.DeleteOnSuccess == true)
                { Directory.Delete(batchDir, true); }

                Trace.TraceInformation("Process complete");
                LF.Logger.LogInstance.WriteLoggingLogFile("Process Complete", false, LF.LoggingErrorType.Info);
                LF.Logger.LogInstance.Dispose();

                Environment.Exit(0);
              //  Environment.ExitCode = 0;
            }
            catch (Exception ex)
            {
                LF.Logger.LogInstance.WriteLoggingLogFile(string.Format("Message:{0}",ex.Message), false, LF.LoggingErrorType.Error);
                LF.Logger.LogInstance.WriteLoggingLogFile(string.Format("Stack Trace:{0}", ex.StackTrace), false, LF.LoggingErrorType.Error);
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                LF.Logger.LogInstance.Dispose();
                FlashWindow.Flash(FlashWindow.FLASHW_TIMERNOFG);
                Console.WriteLine("Press any key to continue");
                Console.ReadLine();
                Environment.ExitCode = -1;
            }
            Environment.Exit(-1);
        }
        public static void CreateLogFolder()
        {
            try
            { 
            if (!(Directory.Exists(LogFolder)))
                Directory.CreateDirectory(LogFolder);
            }
            catch(Exception ex)
            {
                Trace.TraceInformation("Error Creating folder " + LogFolder+ " "+ex.Message);
            }
        }
        public static string GetAppConfigSetting(string key)
        {
            try
            {
                ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            }
            catch (ConfigurationErrorsException ex)
            {
                throw new Exception(ex.Message);
            }
            return ConfigurationManager.AppSettings.Get(key);
        }
        public static void DeleteLogFiles()
        {
            int numDays = 3;
            try
            {
                
                var dir = new DirectoryInfo(Path.GetDirectoryName(LF.Logger.LogInstance.LogFileName));
                DateTime dateTime = DateTime.Now;
                foreach (var file in dir.EnumerateFiles("*.*"))
                {
                    TimeSpan ts = dateTime - file.CreationTime;
                    if (ts.Days >= numDays)
                    {
                        LF.Logger.LogInstance.WriteLoggingLogFile($"deleting file {file.FullName} for number days {numDays.ToString()}", false, LF.LoggingErrorType.Warning);
                        file.Delete();
                    }
                }
            }
            catch (Exception ex)
            {
                LF.Logger.LogInstance.WriteLoggingLogFile($"Path {LF.Logger.LogInstance.LogFileName} for number days {numDays.ToString()} message {ex.Message}", false, LF.LoggingErrorType.Error);
            }

        }
        public static SP.List GetListByTitle(SP.ClientContext clientContext, string title)
        {
            SP.Web web = clientContext.Web;
            SP.ListCollection lists = web.Lists;

            IEnumerable<SP.List> existingLists = clientContext.LoadQuery(
                     lists.Where(l => l.Title == title));
            clientContext.ExecuteQuery();
            return existingLists.FirstOrDefault();
        }

        public static SP.Folder CreateSubfolders(SP.ClientContext context, SP.List list, string listRelativeUrl)
        {
            SP.Folder currentFolder = list.RootFolder;
            if (string.IsNullOrWhiteSpace(listRelativeUrl))
            { return currentFolder; }

            foreach (string folderName in listRelativeUrl.Split('/'))
            {
                Trace.TraceInformation("Checking " + folderName);
                LF.Logger.LogInstance.WriteLoggingLogFile(string.Format("Checking:{0}",folderName), false, LF.LoggingErrorType.Info);
                if (string.IsNullOrWhiteSpace(folderName))
                { continue; }

                context.Load(currentFolder, f => f.ServerRelativeUrl);
                context.ExecuteQuery();

                SP.Folder subFolder = GetSubFolderByName(context, currentFolder, folderName);
                if (subFolder == null)
                {
                    Trace.TraceInformation("Creating folder " + folderName);
                    LF.Logger.LogInstance.WriteLoggingLogFile(string.Format("Creating folder:{0}", folderName), false, LF.LoggingErrorType.Info);
                    SP.ListItem subFolderItem = list.AddItem(new SP.ListItemCreationInformation()
                    {
                        LeafName = folderName,
                        FolderUrl = currentFolder.ServerRelativeUrl,
                        UnderlyingObjectType = SP.FileSystemObjectType.Folder
                    });
                    subFolderItem["Title"] = folderName;
                    subFolderItem.Update();
                    context.Load(subFolderItem.Folder);
                    context.ExecuteQuery();
                    currentFolder = subFolderItem.Folder;
                }
                else
                {
                    Trace.TraceInformation("Skipping folder " + folderName + " already exists");
                    LF.Logger.LogInstance.WriteLoggingLogFile(string.Format("Skipping folder:{0} already exists", folderName), false, LF.LoggingErrorType.Info);
                    currentFolder = subFolder;
                }

            }
            return currentFolder;
        }

        public static SP.Folder GetSubFolderByName(SP.ClientContext context, SP.Folder folder, string name)
        {
            SP.FolderCollection folders = folder.Folders;

            IEnumerable<SP.Folder> existingFolders = context.LoadQuery<SP.Folder>(
                folders.Where(fldr => fldr.Name == name));
            context.ExecuteQuery();
            return existingFolders.FirstOrDefault();
        }

        static IEnumerable<string> IterateFolders(string folders)
        {
            string[] args = folders.Trim('/').Split('/');
            string full = null;
            foreach (string folderName in folders.Split('/'))
            {
                if (string.IsNullOrWhiteSpace(folderName))
                { continue; }

                full = string.Join("/", full, folderName);
                yield return full;
            }
        }
    }
}
