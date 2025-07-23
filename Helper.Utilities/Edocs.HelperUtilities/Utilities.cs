using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Configuration;
using System.Diagnostics;
using System.ServiceProcess;
namespace Edocs.HelperUtilities
{
    public class Utilities
    {
        public static string GetApplicationDir()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }
        public static TimeSpan GetImeSpan(string timeSpan)
        {
            TimeSpan results = DateTime.Now - DateTime.Now;
            TimeSpan span;
            try
            { 
            if (TimeSpan.TryParse(timeSpan, out span))
            {
                results = span;
            }
            }
            catch(Exception ex)
            {
                throw new Exception($"Could not get timespan:{ex.Message}");
            }
            return results;
        }

        public static string ReplaceAppDirName(string inputStr,string toRep)
        {
            return inputStr.Replace(toRep, Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
        }
        public static bool GetBool(string inBool)
        {
           
            bool retBool = false;
            if (bool.TryParse(inBool, out retBool))
                return retBool;
            return false;
        }

        public static int ParseInt(string intToParse)
        {
            if (int.TryParse(intToParse, out int result))
                return result;

            return int.MinValue;
        }

        public static Int32 ParseInt32(string intToParse)
        {
            if (Int32.TryParse(intToParse, out int result))
                return result;

            return int.MinValue;
        }
        public static string GetAssemblyCopyright()
        {
            object[] assAttribute = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);

            if (assAttribute.Length > 0)
            {


                return ((AssemblyCopyrightAttribute)assAttribute[0]).Copyright;

            }
            return " ";
        }
        public static string GetAssemblyName()
        {
            return Assembly.GetExecutingAssembly().GetName().Name;

        }

        public static string GetAssemblyDescription()
        {
            object[] assAttribute = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);

            if (assAttribute.Length > 0)
            {


                return ((AssemblyDescriptionAttribute)assAttribute[0]).Description;


            }
            return string.Empty;
        }
        public static void RunTask(string processName, string processArgs, bool waitForExit,bool useShellExecute,bool createNoWindow, ProcessWindowStyle processWindowStyle,string workingDirectory, string startFolder, int waitForExitMillSeconds = 0)
        {
            try
            {


                Process myProcess = new Process();
                myProcess.StartInfo.UseShellExecute = useShellExecute;
                myProcess.StartInfo.FileName = processName;
                myProcess.StartInfo.Arguments = processArgs;
                myProcess.StartInfo.CreateNoWindow = createNoWindow;
                myProcess.StartInfo.WindowStyle = processWindowStyle;
                if (!(string.IsNullOrWhiteSpace(startFolder)))
                {
                    Environment.CurrentDirectory = startFolder;
                    myProcess.StartInfo.WorkingDirectory = startFolder;
                }
                if (!(string.IsNullOrEmpty(workingDirectory)))
                    myProcess.StartInfo.WorkingDirectory = workingDirectory;
                if (!(myProcess.Start()))
                {
                    throw new Exception($"Process {processName} with args {processArgs} did not start");
                }
                if (waitForExit)
                    myProcess.WaitForExit();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        //public static string GetAssemblyDescription()
        //{
        //    object[] assAttribute = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);

        //    if (assAttribute.Length > 0)
        //    {


        //        return ((AssemblyDescriptionAttribute)assAttribute[0]).Description;


        //    }
        //    return string.Empty;
        //}

        public static string GetAssemblyVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
        public static string GetAssemblyCompnayName()
        {

            //    string assemblyCompnayName = ((AssemblyCompanyAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyCompanyAttribute), false))?.Company ?? "E-DocsUsa";
            return ((AssemblyCompanyAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyCompanyAttribute), false))?.Company ?? "E-DocsUsa";
        }

        public static void RestartService(string serviceName, int timeoutMilliseconds)
        {
            ServiceController service = new ServiceController(serviceName);
            try
            {
                int millisec1 = Environment.TickCount;
                TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);

                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);

                // count the rest of the timeout
                int millisec2 = Environment.TickCount;
                timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds - (millisec2 - millisec1));

                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running, timeout);
            }
            catch(Exception ex)
            {
                throw new Exception($"Service {serviceName} did not start or stop waited timeoutMilliseconds {timeoutMilliseconds} {ex.Message}");
            }
        }
        public static string GetAssemblyProduct()
        {

            //string assemblyCompnayName = ((AssemblyTitleAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyTitleAttribute), false))?.Title ?? "unkown title";
            return ((AssemblyProductAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyProductAttribute), false))?.Product ?? "unkown product";// GetApplicationTitle => ((AssemblyTitleAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyCompanyAttribute), false))?.Company ?? "Unknown Title";
        }
        public static string GetAssemblyFileVersion()
        {

            //string assemblyCompnayName = ((AssemblyTitleAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyTitleAttribute), false))?.Title ?? "unkown title";
            return ((AssemblyFileVersionAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyFileVersionAttribute), false))?.Version ?? "unkown version";// GetApplicationTitle => ((AssemblyTitleAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyCompanyAttribute), false))?.Company ?? "Unknown Title";
        }

        //public string GetAssemblyTitle()
        //{

        //    //string assemblyCompnayName = ((AssemblyTitleAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyTitleAttribute), false))?.Title ?? "unkown title";
        //    return ((AssemblyTitleAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyTitleAttribute), false))?.Title ?? "unkown title";// GetApplicationTitle => ((AssemblyTitleAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyCompanyAttribute), false))?.Company ?? "Unknown Title";
        //}

        public static void AddApplicationKey(string key, string value)
        {
            try
            {
                string currKey;
                ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                currKey = ConfigurationManager.AppSettings.Get(key);
                if (string.IsNullOrEmpty(currKey))
                    ConfigurationManager.AppSettings.Add(key, value);
            }
            catch(Exception ex)
            {
                throw new Exception($"Could not add application config key:{key} value:{value} message:{ex.Message}");
            }
        }

        public static void UpDateApplicationKey(string key, string value)
        {
            try
            { 
            string currKey;
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            currKey = ConfigurationManager.AppSettings.Get(key);
            if (!(string.IsNullOrEmpty(currKey)))
            {
                config.AppSettings.Settings[key].Value = value;
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
            else
                AddApplicationKey(key, value);
            }
            catch(Exception ex)
            {
                throw new Exception($"Could not update application config key:{key} value:{value} message:{ex.Message}");
            }


        }
     
        public static string GetAppConfigSetting(string key)
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
        public static string GetAssemblyTitle()
        {
            object[] assAttribute = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);

            if (assAttribute.Length > 0)
            {
                AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)assAttribute[0];
                if (!(string.IsNullOrEmpty(titleAttribute.Title)))
                    return titleAttribute.Title;

            }
            return string.Empty;
        }
        //public static string GetAssemblyCopyright()
        //{
        //    object[] assAttribute = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);

        //    if (assAttribute.Length > 0)
        //    {


        //        return ((AssemblyCopyrightAttribute)assAttribute[0]).Copyright;

        //    }
        //    return " ";
        //}
        public static async Task<string[]> GetDirectories(string path,string searchPattern,SearchOption searchOpt)
        {
            return Directory.GetDirectories(path, searchPattern, searchOpt);
        }
        public static async Task<string[]> GetDirectoryFiles(string path, string searchPattern, SearchOption searchOpt)
        {
            return Directory.GetFiles(path, searchPattern, searchOpt);
        }
        public static async Task DelDirectory(string path, string searchPattern, SearchOption searchOpt,bool recursive)
        {
             string[] currFiles = Directory.GetFiles(path, searchPattern, searchOpt);
            if((currFiles == null) || (currFiles.Length == 0))
            {
                 
                    Directory.Delete(path, recursive);
                 
            }
        }
        public static List<string> GetDirFilesNamePath(string path)
        {
            List<string> retList = new List<string>();
            try
            {

                var dir = new DirectoryInfo(path);
                foreach (var file in dir.EnumerateFiles("*.*"))
                {
                    retList.Add(file.FullName);
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Could not get files for path:{path} message:{ex.Message}");
                //Trace.TraceError(string.Format("Error getting files message: {0}", ex.Message));
            }
            return retList;
        }
        public static void RunTask(string tasktoRun, string taskArgs, string startFolder, bool waitForExit,int waitForExitMillSeconds=0)
        {
            try
            {
                Process process = new Process();
                process.StartInfo.FileName = tasktoRun;
                process.StartInfo.Arguments = taskArgs;
                process.StartInfo.CreateNoWindow = true;

                if (!(string.IsNullOrWhiteSpace(startFolder)))
                {
                    Environment.CurrentDirectory = startFolder;
                    process.StartInfo.WorkingDirectory = startFolder;
                }
                process.Start();
                if (waitForExit)
                    if(waitForExitMillSeconds > 0)
                    process.WaitForExit(waitForExitMillSeconds);
                else
                        process.WaitForExit();
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not start process:{tasktoRun} with args:{taskArgs} in startfolder:{startFolder} message:{ex.Message}");
            }

        }
        public static void EndTask(string taskname)
        {
            string processName = taskname.Replace(".exe", "");
            try
            {
                foreach (Process process in Process.GetProcessesByName(processName))
                {
                    process.Kill();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not stop task:{taskname} process name:{processName} message:{ex.Message}");
            }
        }
        public static void EndTaskById(int taskID)
        {
            
            try
            {
                Process process = Process.GetProcessById(taskID);
                 process.Kill();
                
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not stop taskid:{taskID} message:{ex.Message}");
            }
        }
        public static string CheckFolderPath(string path)
        {
            CheckEmptyString(path);
            if (!(path.EndsWith(@"\")))
                return $@"{path}\";
            return path;

        }
       static private void CheckEmptyString(string empty)
        {
            if (string.IsNullOrWhiteSpace(empty))
                throw new Exception("String is empty");
        }
        public static bool TaskRunning(string taskname)
        {
            string processName = taskname.Replace(".exe", "");
            try
            {
                Process[] pname = Process.GetProcessesByName(processName);
                if (pname.Length > 0)
                    return true;

            }
            catch (Exception ex)
            {
                throw new Exception($"Could not stop task:{taskname} process name:{processName} message:{ex.Message}");
            }
            return false;
        }

        public static List<string> GetDirFilesName(string path)
        {
            List<string> retList = new List<string>();
            try
            {

                var dir = new DirectoryInfo(path);
                foreach (var file in dir.EnumerateFiles("*.*"))
                {
                    retList.Add(file.Name);
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Could not get directory files for path:{path} message:{ex.Message}");
                //Trace.TraceError(string.Format("Error getting files message: {0}", ex.Message));
            }
            retList.Sort();
            return retList;
        }

        public static void CreateDirectory(string path)
        {
            try
            {
                CheckEmptyString(path);
                if (Path.HasExtension(path))
                    path =   Path.GetDirectoryName(path);
                if (!(Directory.Exists(path)))
                {
                    // Trace.TraceInformation(string.Format("Creating directory:{0}", path));
                    Directory.CreateDirectory(path);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not create directory:{path} message:{ex.Message}");
            }
        }
        public static void DeleteFile(string fileName)
        {
            try
            {
                CheckEmptyString(fileName);
                if (File.Exists(fileName))
                    File.Delete(fileName);
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not get del file:{fileName} message:{ex.Message}");
            }
        }

        public static bool CheckFileExists(string fName)
        {
            CheckEmptyString(fName);
            if (File.Exists(fName))
                return true;
            return false;
        }
        public static void DeleteFiles(string path, int numDays)
        {
            try
            {
                if (Path.HasExtension(path))
                    path = Path.GetDirectoryName(path);
                //    var dir = new DirectoryInfo(Path.GetDirectoryName(path));
                var dir = new DirectoryInfo(path);
                DateTime dateTime = DateTime.Now;
                foreach (var file in dir.EnumerateFiles("*.*"))
                {
                    TimeSpan ts = dateTime - file.CreationTime;
                    if (ts.Days >= numDays)
                    {
                       
                        file.Delete();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Deleting files for directory:{path} for number of days:{numDays.ToString()} message:{ex.Message}");
                //  Logger.Logger.LogInstance.WriteLoggingLogFile($"Path {path} for number days {numDays.ToString()} message {ex.Message}", false, Logger.LoggingErrorType.Error);
            }
        }

        public static void DeleteFiles(string path)
        {
            try
            {
                if (Path.HasExtension(path))
                    path = Path.GetDirectoryName(path);
                //    var dir = new DirectoryInfo(Path.GetDirectoryName(path));
                var dir = new DirectoryInfo(path);

                foreach (var file in dir.EnumerateFiles("*.*"))
                {
                    try
                    {
                        file.Delete();
                    }
                    catch
                    {
                        Console.WriteLine();
                        //foreach (var process in Process.GetProcesses())
                        //{
                        //    if (process.MainWindowTitle.Contains(Path.GetFileName(file.Name)))
                        //    {
                        //        process.Kill();
                        //        file.Delete();
                        //        break;
                        //    }
                        }


                    }
                 
            }
            catch (Exception ex)
            {
                throw new Exception($"Deleting files for directory:{path} message:{ex.Message}");
                //  Logger.Logger.LogInstance.WriteLoggingLogFile($"Path {path} for number days {numDays.ToString()} message {ex.Message}", false, Logger.LoggingErrorType.Error);
            }
        }


        public static void WriteOutPut(string fileName, string fileData)
        {
            CreateDirectory(fileName);
            object lockFile = new object();
            try
            {
                lock (lockFile)
                {
                    CreateDirectory(Path.GetDirectoryName(fileName));
                    DeleteFile(fileName);
                    using (StreamWriter sw = new StreamWriter(fileName, false))
                    {
                        sw.WriteLine(fileData);
                        sw.Flush();

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error writing file:{fileName} message:{ex.Message}");
                //Trace.TraceError(string.Format("Error writing to file:{0} message: {1}", fileName, ex.Message));
            }
        }

        public static string ReadFile(string fileName)
        {
            try
            {
                using (StreamReader sr = new StreamReader(fileName))
                {
                    return sr.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not read file:{fileName} message:{ex.Message}");
                //Trace.TraceError(string.Format("Error reading file:{0} message: {1}", fileName, ex.Message));
            }
            
        }
        public static void WriteTextAppend(string filename,string text)
        {
            CreateDirectory(filename);
            File.AppendAllText(filename, $"{text}\r\n");
        }
        public static bool TryParseBoold(string inBool)
        {
            if (bool.TryParse(inBool, out bool results))
                return results;
            return false;
        }
        public static string ReplaceString(string currentStr, string oldStr, string newStr)
        {
            CheckEmptyString(currentStr);
            CheckEmptyString(oldStr);
            return currentStr.Replace(oldStr, newStr);
        }

        public static void CopyFile(string sourceFile, string destFile, bool overwrite)
        {
            try
            {
                CreateDirectory(destFile);
                if ((overwrite))
                    DeleteFile(destFile);
                File.Copy(sourceFile, destFile, overwrite);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Error copying source file:{0} to dest file:{1} message: {2}", sourceFile, destFile, ex.Message));
            }
        }

        public static bool CloseRunningProcess(string processName)
        {

            try
            {
              
                Process[] pname = Process.GetProcessesByName(processName);
                if (pname.Length > 1)
                {

              
                    if (pname[1].StartTime > pname[0].StartTime)
                    {
              
                        pname[1].Kill();
                    }
                    else
                    {
              
                        pname[1].Kill();
                    }

                }



            }
            catch (Exception ex)
            {

                return false;
            }
            return true;
        }
        public static int CheckMuptiProcessingRunning(string processName)
        {
            int processId = 0;
            try
            {

                Process[] pname = Process.GetProcessesByName(processName);
                if (pname.Length > 1)
                {


                    if (pname[1].StartTime > pname[0].StartTime)
                    {

                        processId = pname[1].Id;
                    }
                    else
                    {
                        processId = pname[0].Id;
                    }
                   

                }



            }
            catch (Exception ex)
            {

                return processId;
            }
            return processId;
        }
        public static Process[] GetProcessRunning(string processName)
        {
           
            try
            {

                return( Process.GetProcessesByName(processName));
                //if (pname.Length > 1)
                //{


                //    if (pname[1].StartTime > pname[0].StartTime)
                //    {

                //        processId = pname[1].Id;
                //    }
                //    else
                //    {
                //        processId = pname[0].Id;
                //    }


                //}



            }
            
            catch (Exception ex)
            {

                return null;
            }
             
        }
        public static bool CheckProcessingRunningByID(int processID)
        {
            int processId = 0;
            try
            {

               
                Console.WriteLine(1);



            }
            catch (Exception ex)
            {

                return true;
            }
            return false;
        }
    }
}
