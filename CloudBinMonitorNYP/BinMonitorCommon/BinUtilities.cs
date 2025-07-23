using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Reflection;
namespace BinMonitor.Common
{
    public class BinUtilities
    {
        public static BinUtilities BinMointorUtilties = null;
        public static readonly string BinMonUserFolder = string.Format("{0}\\Local\\EdocsUsaBmC\\Config\\Users", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
        public static readonly string BinMonBinsFolder = string.Format(@"{0}\\Local\\EdocsUsaBmC\\Data\\Bins", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
        public static readonly string BinMonUserProfilesFolder = string.Format("{0}\\Local\\EdocsUsaBmC\\Config\\UserProfiles", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
        public static readonly string BinMonMasterCategoriesFolder = string.Format("{0}\\Local\\EdocsUsaBmC\\Config\\Master Categories", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
        public static readonly string BinMonCategoriesFolder = string.Format("{0}\\Local\\EdocsUsaBmC\\Config\\Categories", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
        public static readonly string BinMonSpecimenBatchesFolder = string.Format("{0}\\Local\\EdocsUsaBmC\\Data\\Specimen Batches", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
        public static readonly string BinMonTransactionQueueFolder = string.Format("{0}\\Local\\EdocsUsaBmC\\Data\\Transaction Queue", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
        public static readonly string BinMonBatchArchiveFolder = string.Format("{0}\\Local\\EdocsUsaBmC\\Data\\Batch Archive", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
        public static readonly string BinMonLogFolder = string.Format("{0}\\Local\\EdocsUsaBmC\\Logs", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
        public static readonly string BinSpecBatchesArchiveFolder = string.Format("{0}\\Local\\EdocsUsaBmC\\Data\\Specimen Batches\\Archive", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
        public static readonly string NumberDaysFile = string.Format("{0}\\Local\\EdocsUsaBmC\\NumDays.txt", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
        public static readonly string UpDateFileName = string.Format("{0}\\Local\\EdocsUsaBmC\\Update.txt", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
        public static readonly string UpDateDownLoadFolder = string.Format("{0}\\Local\\EdocsUsaBmC\\UpLoadFolder", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
        public static readonly string UpDateDownBackUpFolder = string.Format("{0}\\Local\\EdocsUsaBmC\\BackUpFolder", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
        public static readonly string UpDateProgramName = "WinFormUpdateSoftWare.exe";

        public int NumberDays
        { get; set; }
        BinUtilities()
        {
        }
        static BinUtilities()
        {
            if (BinMointorUtilties == null)
                BinMointorUtilties = new BinUtilities();
        }
        private bool DelArchiveFolder()
        {
            try
            {
                if (!(File.Exists(NumberDaysFile)))
                {
                    WriteOutPut(NumberDaysFile, DateTime.Now.ToString());
                    return false;
                }

                DateTime dtOld = DateTime.Now;
                if (DateTime.TryParse(ReadFile(NumberDaysFile).Trim(), out dtOld))
                {
                    DateTime dt = DateTime.Now;
                    TimeSpan ts = dt.Subtract(dtOld);
                    if (ts.Days >= NumberDays)
                    {
                        WriteOutPut(NumberDaysFile, DateTime.Now.ToString());
                        return true;
                    }
                }
            }
            catch { }
            return false;
        }
    
        
        public void CleanUpFolders()
        {
            if (CheckFolderExists(BinMonUserFolder))
            {
                Trace.TraceInformation(string.Format("Removing files under folder:{0}", BinMonUserFolder));
                DeleteFiles(BinMonUserFolder);
            }
            if (CheckFolderExists(BinMonLogFolder))
            {
                Trace.TraceInformation(string.Format("Removing files under folder:{0}", BinMonLogFolder));
                DeleteFiles(BinMonLogFolder);
            }
            if (CheckFolderExists(BinMonMasterCategoriesFolder))
            {
                Trace.TraceInformation(string.Format("Removing files under folder:{0}", BinMonMasterCategoriesFolder));
                DeleteFiles(BinMonMasterCategoriesFolder);
            }
            if (CheckFolderExists(BinMonCategoriesFolder))
            {
                Trace.TraceInformation(string.Format("Removing files under folder:{0}", BinMonCategoriesFolder));
                DeleteFiles(BinMonCategoriesFolder);
            }
            if (CheckFolderExists(BinMonBinsFolder))
            {
                Trace.TraceInformation(string.Format("Removing files under folder:{0}", BinMonBinsFolder));
                DeleteFiles(BinMonBinsFolder);
            }
            if (CheckFolderExists(BinMonUserProfilesFolder))
            {
                Trace.TraceInformation(string.Format("Removing files under folder:{0}", BinMonUserProfilesFolder));
                DeleteFiles(BinMonUserProfilesFolder);
            }
            if (CheckFolderExists(BinMonSpecimenBatchesFolder))
            {
                Trace.TraceInformation(string.Format("Removing files under folder:{0}", BinMonSpecimenBatchesFolder));
                DeleteFiles(BinMonSpecimenBatchesFolder);
            }
            if (DelArchiveFolder())
            { 
            
            if (CheckFolderExists(BinMonBatchArchiveFolder))
            {
                Trace.TraceInformation(string.Format("Removing files under folder:{0}", BinMonBatchArchiveFolder));
                DeleteFiles(BinMonBatchArchiveFolder);
            }
            if (CheckFolderExists(BinMonTransactionQueueFolder))
            {
                Trace.TraceInformation(string.Format("Removing files under folder:{0}", BinMonTransactionQueueFolder));
                DeleteFiles(BinMonTransactionQueueFolder);
            }
            
           
            if (CheckFolderExists(BinSpecBatchesArchiveFolder))
            {
                Trace.TraceInformation(string.Format("Removing files under folder:{0}", BinSpecBatchesArchiveFolder));
                DeleteFiles(BinSpecBatchesArchiveFolder);
            }
            }
          
        }

        public bool CheckFolderExists(string path)
        {
            return Directory.Exists(path);
        }

        public void DeleteFiles(string path)
        {
            try
            {
                var dir = new DirectoryInfo(path);
                foreach (var file in dir.EnumerateFiles("*.*"))
                {
                    file.Delete();
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(string.Format("Error deleting file message: {0}", ex.Message));
            }
        }

        public List<string> GetDirFilesNamePath(string path)
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
                Trace.TraceError(string.Format("Error getting files message: {0}", ex.Message));
            }
            return retList;
        }

        public List<string> GetDirFilesName(string path)
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
                Trace.TraceError(string.Format("Error getting files message: {0}", ex.Message));
            }
            return retList;
        }

        public void CreateDirectory(string path)
        {
            try
            {
                if (!(Directory.Exists(path)))
                {
                    Trace.TraceInformation(string.Format("Creating directory:{0}", path));
                    Directory.CreateDirectory(path);
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(string.Format("Error creating directory: {0} message : {1}", path, ex.Message));
            }
        }
        public void DeleteFile(string fileName)
        {
            try
            {

                if (File.Exists(fileName))
                    File.Delete(fileName);
            }
            catch (Exception ex)
            {
                Trace.TraceError(string.Format("Error deleting file:{0} message:{1}", fileName, ex.Message));
            }
        }
        public void WriteOutPut(string fileName, string fileData)
        {
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
                Trace.TraceError(string.Format("Error writing to file:{0} message: {1}",fileName, ex.Message));
            }
        }
        public bool CheckEmailAddress(string emaiAddress)
        {
            // Return true if strIn is in valid email format.
            try
            {
                return (Regex.IsMatch(emaiAddress,
                      @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                      @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                      RegexOptions.IgnoreCase));
            }
            catch (Exception ex)
            {
                Trace.TraceError(string.Format("Error checking email address:{0} message: {1}",emaiAddress, ex.Message));
                return false;
            }

        }

        public string ReadFile(string fileName)
        {
            try
            {
                using (StreamReader sr = new StreamReader(fileName))
                {
                    return sr.ReadToEnd();
                }
            }
            catch(Exception ex)
            {
                Trace.TraceError(string.Format("Error reading file:{0} message: {1}",fileName, ex.Message));
            }
            return string.Empty;
        }

        public string ReplaceString(string currentStr, string oldStr, string newStr)
        {
           
            return currentStr.Replace(oldStr, newStr);
        }
        public void CopyFile(string sourceFile, string destFile)
        {
            try
            { 
            DeleteFile(destFile);
            File.Copy(sourceFile, destFile, true);
            }
            catch(Exception ex)
            {
                Trace.TraceError(string.Format("Error copying source file:{0} to dest file:{1} message: {2}",sourceFile,destFile, ex.Message));
            }
        }

        public string GetAssemblyDescription()
        { 
            object[] assAttribute = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);

            if (assAttribute.Length > 0)
            {


                return ((AssemblyDescriptionAttribute) assAttribute[0]).Description;


            }
            return string.Empty;
        }
        public  string GetAssemblyVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

    }
}
