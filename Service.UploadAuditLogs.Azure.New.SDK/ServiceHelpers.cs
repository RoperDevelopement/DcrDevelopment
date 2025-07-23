using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Xml.Linq;
using System.Reflection;
using System.Configuration;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using Edocs.Service.UploadAuditLogs.Properties;

namespace Edocs.Service.UploadAuditLogs
{
    public class ServiceHelpers
    {


        internal const string RepStrApplicationDir = "{ApplicationDir}";
        internal const string SpGetEmailFreq = "sp_GetEmailFreq";
         internal const string Quote = "\"";
        static readonly ServiceHelpers ServiceHelpersInstance = new ServiceHelpers();
        internal bool RunOnWeekEnds
        {


            get { return Properties.Settings.Default.RunOnWeekEnds; }
        }
        internal string AuditLogErrorFilesFolder
        {


            get { return ($"{ServiceHelpers.Instance.CheckFolderPath(ServiceHelpers.Instance.GetApplicationDir())}{Settings.Default.AuditLogErrorFilesFolder}"); }
        }
        internal int LogFileMaxSize
        {


            get { return Properties.Settings.Default.LogFileMaxSize; }
        }
        internal int RunStartTime
        {


            get { return Properties.Settings.Default.RunStartTime; }
        }
        internal string RegxLogFileCompleted
          {  get
        { return Properties.Settings.Default.RegxLogFileCompleted; }
}
        internal string RegxLogFilesToCheck
        {
            get
            { return Properties.Settings.Default.RegxLogFilesToCheck; }
        }
        internal int RunEndTime
        {


            get { return Properties.Settings.Default.RunEndTime; }
        }

        public static ServiceHelpers Instance
        { get { return ServiceHelpersInstance; } }

        internal void DeleteFile(string fileName)
        {

            if (CheckFileExits(fileName))

                File.Delete(fileName);

        }
        internal bool CheckDelFile(string regx, string fileName, DateTimeOffset? dateTimeOffset)
        {
            if (Path.HasExtension(fileName))
                fileName = Path.GetFileNameWithoutExtension(fileName);
            if (dateTimeOffset.Value.Day == DateTimeOffset.UtcNow.Day)
            {
                if (Regex.IsMatch(fileName, regx, RegexOptions.IgnoreCase))
                    return false;

            }
            return true;
        }
        internal void CreateDir(string dirName)
        {
            //  EDL.LogInstance.WriteLoggingLogFile($"Creating directory:{dirName}", false, Logger.LoggingErrorType.Info);
            if (Path.HasExtension(dirName))
                dirName = Path.GetDirectoryName(dirName);
            if (!(Directory.Exists(dirName)))
                Directory.CreateDirectory(dirName);
        }
        internal string GetApplicationDir()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

        internal string GetAssemblyName()
        {
            return Assembly.GetExecutingAssembly().GetName().Name;

        }


        internal async Task<bool> CheckUpLoadAuditFiles()
        {
            bool runProcess = false;
            if ((DateTime.Now.DayOfWeek == DayOfWeek.Saturday) || (DateTime.Now.DayOfWeek == DayOfWeek.Saturday))
            {
                if (RunOnWeekEnds)
                    runProcess = true;
                else
                    return false;

            }
            if (int.TryParse(DateTime.Now.ToString("HH"), out int result))
            {
                if ((result >= RunStartTime) && (result <= RunEndTime))
                    runProcess = true;
            }

            return runProcess;

        }

        internal string GetAssemblyCompnayName()
        {

            //    string assemblyCompnayName = ((AssemblyCompanyAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyCompanyAttribute), false))?.Company ?? "E-DocsUsa";
            return ((AssemblyCompanyAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyCompanyAttribute), false))?.Company ?? "E-DocsUsa";
        }
        internal string GetAssemblyTitle()
        {

            //string assemblyCompnayName = ((AssemblyTitleAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyTitleAttribute), false))?.Title ?? "unkown title";
            return ((AssemblyTitleAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyTitleAttribute), false))?.Title ?? "unkown title";// GetApplicationTitle => ((AssemblyTitleAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyCompanyAttribute), false))?.Company ?? "Unknown Title";
        }

        internal string GetAssemblyCompany()
        {

            //string assemblyCompnayName = ((AssemblyTitleAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyTitleAttribute), false))?.Title ?? "unkown title";
            return ((AssemblyCompanyAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyCompanyAttribute), false))?.Company ?? "unkown company";// GetApplicationTitle => ((AssemblyTitleAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyCompanyAttribute), false))?.Company ?? "Unknown Title";
        }

        internal string GetAssemblyProduct()
        {

            //string assemblyCompnayName = ((AssemblyTitleAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyTitleAttribute), false))?.Title ?? "unkown title";
            return ((AssemblyProductAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyProductAttribute), false))?.Product ?? "unkown product";// GetApplicationTitle => ((AssemblyTitleAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyCompanyAttribute), false))?.Company ?? "Unknown Title";
        }

        internal string GetAssemblyCopyright()
        {

            //string assemblyCompnayName = ((AssemblyTitleAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyTitleAttribute), false))?.Title ?? "unkown title";
            return ((AssemblyCopyrightAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyCopyrightAttribute), false))?.Copyright ?? "unkown copyright";// GetApplicationTitle => ((AssemblyTitleAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyCompanyAttribute), false))?.Company ?? "Unknown Title";
        }
        internal string GetAssemblyVersion()
        {
            string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            if (string.IsNullOrWhiteSpace(version))
                return "Assembly Version Unkown";
            return version;
            //return (Assembly.GetExecutingAssembly().GetName().Version.ToString());
            //string assemblyCompnayName = ((AssemblyTitleAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyTitleAttribute), false))?.Title ?? "unkown title";
            //return ((AssemblyVersionAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyVersionAttribute), false))?.Version ?? "unkown version";// GetApplicationTitle => ((AssemblyTitleAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyCompanyAttribute), false))?.Company ?? "Unknown Title";
        }

        internal string GetAssemblyFileVersion()
        {

            //string assemblyCompnayName = ((AssemblyTitleAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyTitleAttribute), false))?.Title ?? "unkown title";
            return ((AssemblyFileVersionAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyFileVersionAttribute), false))?.Version ?? "unkown version";// GetApplicationTitle => ((AssemblyTitleAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyCompanyAttribute), false))?.Company ?? "Unknown Title";
        }

        internal string CheckFolderPath(string folderPath)
        {

            if (string.IsNullOrWhiteSpace(folderPath))
                throw new Exception("Folder path cannot be empty");
            if (!(folderPath.EndsWith(@"\")))
                return ($@"{folderPath}\");
            return folderPath;
        }
        private bool CheckFileExits(string fName)
        {

            return File.Exists(fName);
        }
        internal void CopyFile(string sourceFile, string destFile, bool overWriteFile, bool delSourceFile)
        {

            if (!(overWriteFile) && (CheckFileExits(destFile)))
                throw new Exception($"Cannot overwrite file:{destFile}");
            CreateDir(destFile);
            File.Copy(sourceFile, destFile, true);
            if (delSourceFile)
                DeleteFile(sourceFile);
        }
        internal async Task<MemoryStream> ConvertStringToStrem(string instr)
        {
            MemoryStream memStream = new MemoryStream();
            StreamWriter streamWriter = new StreamWriter(memStream);
            streamWriter.Write(instr);
            streamWriter.Flush();
            memStream.Position = 0;
            return memStream;
        }
        internal async Task<string> GetFileData(string fileName)
        {
            using (StreamReader sr = new StreamReader(fileName, Encoding.UTF8))
            {
                return sr.ReadToEndAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            }
        }
        internal bool CheckRegx(string instr,string regx)
        {
            Match regMatch = Regex.Match(instr.ToLower(), regx.ToLower(), RegexOptions.IgnoreCase);
            if (regMatch.Success)
                return true;
            return false;
        }
        internal async Task<string> CheckAuditLogFileComplete(string logFileName)
        {
              
            if (CheckRegx(logFileName,RegxLogFilesToCheck))
            {
                string logFileTxt = System.IO.File.ReadAllText(logFileName);
                 if (!(CheckRegx(logFileTxt, RegxLogFileCompleted)))
                {
                    logFileTxt = $"{logFileName} not closed";
                    return logFileTxt;
                }    
            }
           
            return string.Empty;
        }
        internal async Task<string> CheckAuditLogFileSize(string logFileName)
        {
            System.IO.FileInfo infor = new FileInfo(logFileName);
            if( infor.Length < LogFileMaxSize)
            {
                CopyFile(logFileName, Path.Combine(AuditLogErrorFilesFolder, Path.GetFileName(logFileName)), true, true);
                return $"{logFileName} is smaller then logfile max sizr {LogFileMaxSize} log file size {infor.Length} moving logfile to folder {AuditLogErrorFilesFolder}";
              
            }
            
            return string.Empty;
        }
        internal void DeleteFiles(string folder, string fileExtension, int numberDays)
        {
            DateTime dateTime = DateTime.Now;

            if (string.IsNullOrWhiteSpace(fileExtension))
                fileExtension = "*.*";
            var dir = new DirectoryInfo(folder);
            foreach (var file in dir.EnumerateFiles(fileExtension))
            {
                DateTime fileDT = file.CreationTime;
                TimeSpan ts = dateTime - fileDT;
                try
                {

                    if (ts.TotalDays > numberDays)
                        DeleteFile(file.FullName);

                }
                catch (Exception ex)
                {

                    throw new Exception($"Deleting file {file.FullName} {ex.Message}");
                }
            }

        }
    }
}
