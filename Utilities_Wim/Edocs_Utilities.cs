using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Reflection;

namespace EdocsUSA.Utilities
{
    public class Edocs_Utilities
    {
        public static Edocs_Utilities EdocsUtilitiesInstance = null;

        public string PasswordKey
        { get; set; }

        public string PrintImageFileName
        { get; set; }
        protected Edocs_Utilities()
        { }
        static Edocs_Utilities()
        {

            if (EdocsUtilitiesInstance == null)
            {
                EdocsUtilitiesInstance = new Edocs_Utilities();
            }

        }

        
        public bool DecriptImage
        { get; set; }
        public bool BakupOrgImages
        { get; set; }
        public void CopyBackUpOrgImage(string sourceImage)
        {
            if (BakupOrgImages)
            {
                if(!(DecriptImage))
                { 
                string destFolder = $"{CheckDirectoryPath(CheckPathHasExtension(sourceImage))}SourceImg\\";
                CopyFile(sourceImage, destFolder, true, string.Empty, false);
                }
            }
        }

        public double FileSize(string fileName)
        {
            long length = new System.IO.FileInfo(fileName).Length;
            
                return (length / 1024f) / 1024f;
           
        }
        public bool CloseRunningProcess(string processName)
        {

            try
            {
                EdocsUSA.Utilities.Logging.TraceLogger.TraceLoggerInstance.TraceInformation(string.Format("Checking if another instance of {0} is already running.", Process.GetCurrentProcess().ProcessName));
                Process[] pname = Process.GetProcessesByName(processName);
                if (pname.Length > 1)
                {

                    EdocsUSA.Utilities.Logging.TraceLogger.TraceLoggerInstance.TraceWarning(string.Format("Another instance of {0} is already running.", Process.GetCurrentProcess().ProcessName));
                    EdocsUSA.Utilities.Logging.TraceLogger.TraceLoggerInstance.TraceWarning(string.Format("Start time of instance fist instance {0}", pname[0].StartTime));
                    EdocsUSA.Utilities.Logging.TraceLogger.TraceLoggerInstance.TraceWarning(string.Format("Start time of instance secone instance {0}", pname[1].StartTime));
                    if (pname[0].StartTime < pname[1].StartTime)
                    {
                        EdocsUSA.Utilities.Logging.TraceLogger.TraceLoggerInstance.TraceWarning(string.Format("Stopping instace with start time of instance fist instance {0}", pname[0].StartTime));
                        pname[0].Kill();
                    }
                    else
                    {
                        EdocsUSA.Utilities.Logging.TraceLogger.TraceLoggerInstance.TraceWarning(string.Format("Stopping instace with start time of instance fist instance {0}", pname[1].StartTime));
                        pname[1].Kill();
                    }

                }



            }
            catch (Exception ex)
            {
                EdocsUSA.Utilities.Logging.TraceLogger.TraceLoggerInstance.TraceError($"Could not stop task:{processName} process name:{processName} message:{ex.Message}");
                return false;
            }
            return true;
        }
        public void CreateDir(string dirName)
        {
            dirName = CheckDirectoryPath(dirName);
            if (!Directory.Exists(dirName))
                Directory.CreateDirectory(dirName);
        }
        public string[] GetFiles(string dirName, string fileExtension)
        {
            dirName = CheckDirectoryPath(dirName);
            return Directory.GetFiles(dirName, fileExtension);
        }
        public string CheckDirectoryPath(string dirPath)
        {
            if (dirPath.EndsWith("\\"))
                return dirPath;
            return string.Format("{0}\\", dirPath);
        }
        public void CopyFiles(string sourceFolder, string destFolder, bool delSourceFile, string fileExtension)
        {
            try
            {


                if (!(Directory.Exists(destFolder)))
                    Directory.CreateDirectory(destFolder);
                sourceFolder = CheckPathHasExtension(sourceFolder);
                sourceFolder = CheckDirectoryPath(sourceFolder);
                var dir = new DirectoryInfo(Path.GetDirectoryName(sourceFolder));
                foreach (var file in dir.GetFiles(fileExtension, SearchOption.AllDirectories))
                {
                    try
                    {
                        string destFileName = Path.Combine(destFolder, file.Name);
                        File.Copy(file.FullName, destFileName, true);
                        if (delSourceFile)
                            File.Delete(file.FullName);
                    }
                    catch { }

                }
            }
            catch { }
        }
        public string GetPatIdClientCode(string indexNumber, int startIndex, int endIndex)
        {
            try
            {
                if (!(Regex.IsMatch(indexNumber, @"^\d{15}$")))
                    return string.Empty;
                if (endIndex == 0)
                    return indexNumber.Substring(startIndex).TrimStart('0');

                return indexNumber.Substring(startIndex, endIndex);
            }
            catch (Exception ex)
            {

            }
            return string.Empty;

        }
        public string CheckPathHasExtension(string path)
        {
            if (Path.HasExtension(path))
                path = Path.GetDirectoryName(path);
            return path;
        }

        public string GetFileName(string path)
        {
            if (Path.HasExtension(path))
                path = Path.GetFileName(path);
            return path;
        }
        public void CleanUpLogFiles(string path, int numberDays, string fExtension)
        {
            try
            {
                path = CheckPathHasExtension(path);
                if (path.ToLower().IndexOf("scanquire") > 0)
                {
                    path = path.Substring(0, path.ToLower().IndexOf("scanquire")) + "scanquire\\";
                }
                var dir = new DirectoryInfo(path);

                DateTime dateTime = DateTime.Now;

                foreach (var file in dir.GetFiles(fExtension, SearchOption.AllDirectories))
                {
                    TimeSpan ts = dateTime - file.LastWriteTime;
                    if (ts.Days >= numberDays)
                    {
                        file.Delete();
                    }
                }
            }
            catch (Exception ex)
            {


            }
        }

        public void CopyFile(string souceFile, string destFolder, bool overWrite, string fileName, bool delSourceFile)
        {
            CreateDir(destFolder);

            if (!(string.IsNullOrWhiteSpace(fileName)))
            {
                souceFile = $"{CheckDirectoryPath(souceFile)}{fileName}";
                destFolder = $"{CheckDirectoryPath(destFolder)}{fileName}";
            }
            else
            {
                destFolder = $"{CheckDirectoryPath(destFolder)}{GetFileName(souceFile)}";
            }
            // CHeckFileExists(souceFile,false);
            if (!(Directory.Exists(Path.GetDirectoryName(souceFile))))
                return;
            File.Copy(souceFile, destFolder, overWrite);
            try
            {
                if (delSourceFile)
                    File.Delete(souceFile);
            }
            catch { }


        }
        public void CheckDirectExists(string dirName)
        {
            dirName = CheckPathHasExtension(dirName);
            if (!(Directory.Exists(dirName)))
            {
                throw new Exception($"Dir not found:{dirName}");
            }
        }
        public string AddDateTimeToFolderName(string folderName)
        {
            CreateDir(folderName);
            folderName = CheckDirectoryPath(folderName);
            folderName = $"{folderName}{DateTime.Now.ToString("yyyy")}\\";
            CreateDir(folderName);
            folderName = $"{folderName}{DateTime.Now.ToString("MM-dd")}\\";
            return folderName;

        }
        public void DelDirectores(string dirName, int numberDays, string searchPattern, SearchOption searchOption)
        {
            DirectoryInfo directoryInfos = new DirectoryInfo(dirName);
            DirectoryInfo[] subDir = directoryInfos.GetDirectories(searchPattern, searchOption);
            DateTime dt = DateTime.Now;
            for (int i = subDir.Count() - 1; i >= 0; i--)

            {
                DirectoryInfo directory = subDir[i];
                TimeSpan ts = dt - directory.CreationTime;
                if (ts.Days >= numberDays)
                    DelFolder(directory.FullName, true);
            }

        }
        public void DelFolder(string folderName, bool recursive)
        {
            if ((Directory.Exists(folderName)))
                Directory.Delete(folderName, recursive);
        }
        public void CHeckFileExists(string file, bool thowExceptionFileExists)
        {
            if (!(File.Exists(file)))
                throw new Exception($"File not found {file}");
            else
            {
                if (thowExceptionFileExists)
                    throw new Exception($"File found {file} and should not exists");
            }
        }
        public bool CHeckFileExists(string file)
        {
            if (!(File.Exists(file)))
                return false;
            return true;
            
        }
        /// <summary>Unique entropy (salt) to apply to encryption.</summary>
        private byte[] ProtectedDataEntropy
        { get { return Convert.FromBase64String(PasswordKey); } }


        public byte[] Encrypt(byte[] data, DataProtectionScope scope)
        { return ProtectedData.Protect(data, ProtectedDataEntropy, scope); }

        /// <summary>Encrypt a string to an encrypted byte array.</summary>
        /// <param name="data">String value to encrypt.</param>
        /// <returns>Encrypted byte array.</returns>
        public byte[] Encrypt(string data, DataProtectionScope scope)
        {
            byte[] decryptedData = StringEncoding.GetBytes(data);
            return Encrypt(decryptedData, scope);
        }

        /// <summary>Encrypt a byte array of data to a string.</summary>
        /// <param name="data">Data to encrypt.</param>
        /// <returns>Encrypted string.</returns>
        public string EncryptToString(byte[] data, DataProtectionScope scope)
        {
            byte[] encryptedData = Encrypt(data, scope);
            return Convert.ToBase64String(encryptedData);
        }

        /// <summary>Encrypt a string to a string.</summary>
        /// <param name="data">Data to encrypt.</param>
        /// <returns>Encrypted string.</returns>
		public string EncryptToString(string data, DataProtectionScope scope)
        {
            byte[] decryptedData = StringEncoding.GetBytes(data);
            return EncryptToString(decryptedData, scope);
        }

        public Encoding StringEncoding { get { return Encoding.Unicode; } }




        /// <summary>Decrypt a byte array to a byte array.</summary>
        /// <param name="data">Encrypted data to decrypt.</param>
        /// <returns>Decrpted byte array.</returns>
        public byte[] Decrypt(byte[] data, DataProtectionScope scope)
        { return ProtectedData.Unprotect(data, ProtectedDataEntropy, scope); }

        /// <summary>Decrypt a string to a byte array.</summary>
        /// <param name="data">Encrypted data to decrypt.</param>
        /// <returns>Decrpted byte array.</returns>        
		public byte[] Decrypt(string data, DataProtectionScope scope)
        {
            byte[] encryptedData = Convert.FromBase64String(data);
            return Decrypt(encryptedData, scope);
        }

        /// <summary>Decrypt a byte array to a string.</summary>
        /// <param name="data">Encrypted data to decrypt.</param>
        /// <returns>Decrypted string.</returns>
        public string DecryptToString(byte[] data, DataProtectionScope scope)
        {
            byte[] decryptedData = Decrypt(data, scope);
            return StringEncoding.GetString(decryptedData);
        }

        public string EncryptCipher(string input, string key)
        {
            byte[] inputArray = UTF8Encoding.UTF8.GetBytes(input);
            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
            tripleDES.Key = UTF8Encoding.UTF8.GetBytes(key);
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDES.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            tripleDES.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        public string DecryptCipher(string input, string key)
        {
            byte[] inputArray = Convert.FromBase64String(input);
            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
            tripleDES.Key = UTF8Encoding.UTF8.GetBytes(key);
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDES.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            tripleDES.Clear();
            return UTF8Encoding.UTF8.GetString(resultArray);
        }
        /// <summary>Decrypt a string to a string.</summary>
        /// <param name="data">Encrypted data to decrypt.</param>
        /// <returns>Decrypted string.</returns>
        public string DecryptToString(string data, DataProtectionScope scope)
        {

            byte[] encryptedData = Convert.FromBase64String(data);
            return DecryptToString(encryptedData, scope);
        }

        public   void DeleteFiles(string path)
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
        public   string GetAppConfigSetting(string key)
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
        public  string GetApplicationDir()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }
        public string GetAppConfigSetting(string key,string path)
        {
            string retConfig = string.Empty;
            try
            {
                
                ExeConfigurationFileMap configMap = new ExeConfigurationFileMap();
                configMap.ExeConfigFilename = path;
                                Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configMap,ConfigurationUserLevel.None);
                if(config.HasFile)
                {
                    KeyValueConfigurationCollection keyValueConfigurationCollection = config.AppSettings.Settings;// figurationManager.AppSettings.Get(key).ToString();
                    foreach (KeyValueConfigurationElement configurationElement in keyValueConfigurationCollection)
                    {
                       // if (configurationElement.Key.ToLower() == key.ToLower())
                       if(string.Compare(configurationElement.Key,key,true) == 0)
                        {
                            retConfig = configurationElement.Value;
                            break;
                        }
                    }
                }
            }
            catch (ConfigurationErrorsException ex)
            {
                //throw new Exception($"Could not get applicatiion config key:{key} message:{ex.Message}");
                retConfig = string.Empty;
            }
            return retConfig;
}
        public string AddZeros(string inStr, int maxDigits, int maxStrLength)
        {
            string zeros = string.Empty;
            if (inStr.Length < maxStrLength)
            {
                int loop = inStr.Length;
                while (loop < maxDigits)
                {
                    zeros += "0";
                    loop++;
                }
                inStr = $"{zeros.Trim()}{inStr}";
            }

            return inStr;
        }
        public void StartProcess(string processName,string processArgs,bool waitForExit)
        {
            try
            {


                Process myProcess = new Process();
                myProcess.StartInfo.UseShellExecute = false;
                myProcess.StartInfo.FileName = processName;
                myProcess.StartInfo.Arguments = processArgs;
                myProcess.StartInfo.CreateNoWindow = true;
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
    }
}
