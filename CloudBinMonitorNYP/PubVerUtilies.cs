using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Configuration;
using System.Reflection;
using System.Globalization;
using UlsFw = MaxEng_Common_ULS_Logging_Component;

namespace Publishing_Verification
{
    internal static class PubVerUtilies
    {
        internal const string QUOAT = "\"";
        /// <summary>
        /// Get values from the application config file
        /// </summary>
        /// <param name="key">the key for the values</param>
        /// <returns>string the values else null if value not found</returns>
        internal static string GetApplicationSetting(string key)
        {

            ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            return ConfigurationManager.AppSettings.Get(key);
        }
        /// <summary>
        /// Checks to see if a folder exists
        /// </summary>
        /// <param name="folderName">the name fo the folder</param>
        /// <returns>true if the folder exists else false if the folder does not exists</returns>
        internal static bool CheckFolderExists(string folderName)
        {
            if (string.IsNullOrEmpty(folderName))
                return false;
            return (Directory.Exists(folderName));
        }

        /// <summary>
        /// Add the backslashes to the end of a folder path
        /// </summary>
        /// <param name="folderPath">the folder path to check</param>
        /// <returns>string with the backslashes on the folder</returns>
        internal static string CheckFolderPath(string folderPath)
        {
            if (string.IsNullOrEmpty(folderPath))
                throw new Exception("Error: Folder path cannot be null or empty:");
            if (!(folderPath.EndsWith(@"\")))
                folderPath += @"\";
            return folderPath;

        }
        /// <summary>
        /// Get the name of a directory
        /// </summary>
        /// <param name="dirPath">the directory path</param>
        /// <returns>the name of the directory or empty string</returns>
        internal static string GetDirectoryName(string dirPath)
        {
            try
            {
                return (Path.GetDirectoryName(dirPath));
            }
            catch
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// Create a new folder
        /// </summary>
        /// <param name="folderName">the name of the folder to create</param>
        internal static void CreatFolder(string folderName)
        {
            // Logging.Loginfo.WriteLogging(String.Format("creating folder {0}", folderName), true, Logging.ErrorLevel.Info);
            if (string.IsNullOrEmpty(folderName))
            {
                throw new Exception("Folder name cannot be null");
            }
            if (folderName.Contains("."))
                folderName = GetDirectoryName(folderName);
            if (!(Directory.Exists(folderName)))
                Directory.CreateDirectory(folderName);
        }
        /// Copy a file from sorce folder to dest folder
        /// </summary>
        /// <param name="sourceFile">folder and name of source file</param>
        /// <param name="destFile">folder and name of dest file</param>
        /// <param name="overWrite">true to over write the file in the dest folder</param>
        internal static void CopyFile(string sourceFile, string destFile, bool overWrite)
        {
            try
            {
                //  Logging.Loginfo.WriteLogging(String.Format("Copying source file {0} to dest file {1} overwrite destfile {2}", sourceFile, destFile, overWrite), true, Logging.ErrorLevel.Info);
                if (!(CheckFileExists(sourceFile)))
                {
                    sourceFile = "n/A";
                    throw new Exception("Source file does not exists");

                }
                CreatFolder(Path.GetDirectoryName(destFile));
                File.SetAttributes(sourceFile, FileAttributes.Normal);
                DeleteFile(destFile);
                File.Copy(sourceFile, destFile, overWrite);
                File.SetAttributes(destFile, FileAttributes.Normal);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Could not copy souce file {0} to dest file {1} errror message {2} ", sourceFile, destFile, ex.Message));

            }

        }

        /// <summary>
        /// Loop through a directory and subdirectories to search for a file
        /// </summary>
        /// <param name="sDir">root directory</param>
        /// <param name="searchFile">file to search for</param>
        /// <param name="filesFound">return the files found</param>
        internal static void DirSearch(string sDir, string searchFile, ref List<string> filesFound)
        {
            try
            {
                foreach (string d in Directory.GetDirectories(sDir, "*.*", SearchOption.AllDirectories))
                {
                    foreach (string f in Directory.GetFiles(d, searchFile, SearchOption.AllDirectories))
                    {
                        filesFound.Add(f);
                    }
                    //  DirSearch(d, searchFile, ref filesFound);
                }

                foreach (string f in Directory.GetFiles(sDir, searchFile, SearchOption.TopDirectoryOnly))
                {
                    if (!(filesFound.Contains(f)))
                        filesFound.Add(f);
                }

                //  DirSearch(d, searchFile, ref filesFound);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        internal static string ForMatDateTime(DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd HH:mm:ss");
        }
        /// <summary>
        /// Delete a file
        /// </summary>
        /// <param name="fileName">the name fo the file to delete</param>
        public static void DeleteFile(string fileName)
        {
            try
            {
                if (CheckFileExists(fileName))
                {
                    File.SetAttributes(fileName, FileAttributes.Normal);
                    File.Delete(fileName);
                }


            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Could not delete file {0} error message {1}", fileName, ex.Message));

            }
        }

        /// <summary>
        /// Check if a file exist in a folder
        /// </summary>
        /// <param name="fileName">the name of the file</param>
        /// <returns>true if the file exists else false </returns>
        internal static bool CheckFileExists(string fileName)
        {

            if (string.IsNullOrEmpty(fileName))
                throw new Exception("File name cannot be empty");
            return (File.Exists(fileName));

        }
        internal static string GetDirectoryLastPath(string dName)
        {
            string[] lPath = dName.Split(Path.DirectorySeparatorChar);
            if (!(string.IsNullOrEmpty(lPath[lPath.Length - 1])))
                return lPath[lPath.Length - 1];
            return lPath[lPath.Length - 2];
        }
        /// <summary>
        /// Get the files for a current folder
        /// </summary>
        /// <param name="path">The folder of the source xml files</param>
        /// <param name="files">The file extension</param>
        /// <returns FileInfo>The list of the files in the current directory</returns>
        internal static FileInfo[] GetDirectoryFiles(string path, string filesExtension)
        {
            try
            {

                DirectoryInfo di = new DirectoryInfo(path);
                return di.GetFiles(filesExtension, SearchOption.AllDirectories);
            }
            catch (Exception ex)
            {
                throw new Exception("Could not get files for path: " + path + " with extension " + filesExtension + " " + ex.Message);
            }
        }
        /// get the assebbly title
        /// </summary>
        /// <returns></returns>
        internal static string GetAssemblyTitle()
        {
            object[] assAttribute = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);

            if (assAttribute.Length > 0)
            {
                AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)assAttribute[0];
                if (!(string.IsNullOrEmpty(titleAttribute.Title)))
                    return titleAttribute.Title;

            }
            return GetFileNameExtension(Assembly.GetExecutingAssembly().CodeBase);
        }
        /// <summary>
        /// Get a file extension
        /// </summary>
        /// <param name="fileName">the file name</param>
        /// <returns>the extension of the file or empty sting if cannot get the entension</returns>
        internal static string GetFileNameExtension(string fileName)
        {
            try
            {
                return (Path.GetExtension(fileName));
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Could not get file extension {0} error message {1}", fileName, ex.Message));
            }

        }
        /// <summary>
        /// get the assembly version
        /// </summary>
        /// <returns></returns>
        internal static string GetAssemblyVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        /// <summary>
        /// get the Assembly Copyright
        /// </summary>
        /// <returns></returns>
        internal static string GetAssemblyCopyright()
        {
            object[] assAttribute = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);

            if (assAttribute.Length > 0)
            {


                return ((AssemblyCopyrightAttribute)assAttribute[0]).Copyright;

            }
            return " ";
        }

        /// <summary>
        /// get the Assembly file version
        /// </summary>
        /// <returns></returns>
        internal static string GetAssemblyFileVersion()
        {
            object[] assAttribute = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false);

            if (assAttribute.Length > 0)
            {


                return ((AssemblyFileVersionAttribute)assAttribute[0]).Version;

            }
            return " ";
        }


        /// <summary>
        /// Get the Assembly company
        /// </summary>
        /// <returns></returns>
        internal static string GetAssemblyCompany()
        {
            object[] assAttribute = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);

            if (assAttribute.Length > 0)
            {


                return ((AssemblyCompanyAttribute)assAttribute[0]).Company;

            }
            return " ";
        }
        /// <summary>
        /// Get the Assembly description
        /// </summary>
        /// <returns></returns>
        internal static string GetAssemblyDescription()
        {
            object[] assAttribute = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);

            if (assAttribute.Length > 0)
            {


                return ((AssemblyDescriptionAttribute)assAttribute[0]).Description;

            }
            return " ";
        }
        internal static string GetFileNameNoExt(string fileName)
        {
            return Path.GetFileNameWithoutExtension(fileName);
        }
        internal static void RenameFile(string sourceF, string destFn)
        {
            try
            {
                UlsFw.Uls.SendStructuredTrace(UlsFw.Category.MAXEngCore, UlsFw.Level.Info, string.Format("Moving source file:{0} to dest file:{1}", sourceF, destFn), "In Method   internal static void RenameFile(string sourceF,string destFn)");
                CreatFolder(GetDirectoryName(destFn));
                DeleteFile(destFn);
                FileInfo file = new FileInfo(sourceF);
                file.MoveTo(destFn);
            }
            catch (Exception ex)
            {
                UlsFw.Uls.SendStructuredTrace(UlsFw.Category.MAXEngCore, UlsFw.Level.Error, string.Format("Moving source file:{0} to dest file:{1} message{2} ", sourceF, destFn, ex.Message), "In Method   internal static void RenameFile(string sourceF,string destFn)");
            }

        }
        internal static bool GetTimeSpan(DateTime startTime,DateTime endTime,int totalTimeSpan)
        {
            
            TimeSpan span = endTime.Subtract(startTime);
            if (span.TotalMinutes <= totalTimeSpan)
                return true;
            return false;
        }
        /// Get the data from a file
        /// </summary>
        /// <param name="fileName">the name of the file</param>
        /// <returns>the data inside the file or empty string if an exception occurs</returns>
        internal static string GetFileData(string fileName)
        {
            string retStr = string.Empty;
            UlsFw.Uls.SendStructuredTrace(UlsFw.Category.MAXEngCore, UlsFw.Level.Info, string.Format("Reading file:{0}", fileName), "In Method internal static string GetFileData(string fileName)");
            try
            {
                using (StreamReader sr = new StreamReader(fileName))
                {
                    retStr = sr.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                UlsFw.Uls.SendStructuredTrace(UlsFw.Category.MAXEngCore, UlsFw.Level.Error, string.Format("Reading file:{0} message{1} ", fileName, ex.Message), "In Method internal static string GetFileData(string fileName)");

            }

            return retStr;

        }
        internal static DateTime AddSubTime(DateTime dt, int addSubMinutes)
        {
            return dt.AddMinutes(addSubMinutes);
        }
        internal static string GetLcidCulture(string lcidCulture, bool cultureLcid)
        {
            try
            {

                if (cultureLcid)
                {
                    CultureInfo culLcid = new CultureInfo(int.Parse(lcidCulture));
                    UlsFw.Uls.SendStructuredTrace(UlsFw.Category.MAXEngCore, UlsFw.Level.Info, string.Format("Using culture Name:{0} for culture:{1}", culLcid.Name, lcidCulture), "In Method private string GetLcid(string locale)");
                    return culLcid.Name;
                }
                else
                {
                    CultureInfo culLcid = new CultureInfo(lcidCulture.ToLower());
                    UlsFw.Uls.SendStructuredTrace(UlsFw.Category.MAXEngCore, UlsFw.Level.Info, string.Format("Using lcid:{0} for culture:{1}", culLcid.LCID.ToString(), lcidCulture), "In Method private string GetLcid(string locale)");
                    return culLcid.LCID.ToString();
                }
            }
            catch (Exception ex)
            {
                UlsFw.Uls.SendStructuredTrace(UlsFw.Category.MAXEngCore, UlsFw.Level.Error, string.Format("Getting lcid for local:{0} message{1} ", lcidCulture, ex.Message), "In Method internal static string GetLcid(string locale)");
            }
            return string.Empty;
        }

        /// <summary>
        /// Run a program from the command line
        /// </summary>
        /// <param name="programName">the name of the program to run</param>
        /// <param name="paramaters">the paramaters for the program</param>

        internal static void RunDosCmd(string programName, string paramaters, string workingFolder)
        {
            UlsFw.Uls.SendStructuredTrace(UlsFw.Category.MAXEngCore, UlsFw.Level.Info, string.Format("Running command for program {0} with params {1} in working folder {2}", programName, paramaters, workingFolder), "In Method public static void RunDosCmd(string programName, string paramaters, string workingFolder)");
            int exitCode = 0;

            Environment.CurrentDirectory = workingFolder;
            ProcessStartInfo startInfo = new ProcessStartInfo();
           // paramaters = "ScheduleJob /DataStore:"+PubVerUtilies.QUOAT+"Max Eng Jobs"+PubVerUtilies.QUOAT+ " /Job { /ID:73 /Parameter { /Name:AssemblyClassXmnlFile /Value:%Dx3PORoot%\\Tools\\CreateIcmTickets\\AssembliesCLasses.xml } /Parameter { /Name:ClassName /Value:PubVer } } /MachinePool:" + PubVerUtilies.QUOAT+"$\\ICM"+PubVerUtilies.QUOAT;

            startInfo.Arguments = paramaters;
            startInfo.UseShellExecute = false;
            startInfo.WindowStyle = ProcessWindowStyle.Maximized;
            startInfo.CreateNoWindow = false;
            startInfo.FileName = programName;
            startInfo.WorkingDirectory = workingFolder;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardInput = false;
            Process CmdProcess = new Process();
            try
            {
                CmdProcess = Process.Start(startInfo);
                CmdProcess.WaitForExit();
                exitCode = CmdProcess.ExitCode;
                string outPutMessage = CmdProcess.StandardOutput.ReadToEnd();
                CmdProcess.Close();
                CmdProcess.Dispose();
                if (exitCode != 0)
                    throw new Exception(string.Format("Running:{0} with params:{1} retruned exit code:{2} message:{3}", programName, paramaters, exitCode.ToString(), outPutMessage));

            }
            catch (Exception ex)
            {
                UlsFw.Uls.SendStructuredTrace(UlsFw.Category.MAXEngCore, UlsFw.Level.Info, string.Format("Running command for program {0} with params {1} in working folder {2} message:{3}", programName, paramaters, workingFolder, ex.Message), "In Method public static void RunDosCmd(string programName, string paramaters, string workingFolder)");
                throw new Exception(ex.Message);

            }

        }

    }
}
