using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
 
using System.Security.Permissions;

using BS= Edocs.Azure.Blob.Storage;
using System.Security;

namespace Edocs.DOH.DL.Doc.Utilities
{
    public class DOH_Utilites
    {
        private static DOH_Utilites instance = null;

        private DOH_Utilites() { }

        public static DOH_Utilites DOHUtilitiesInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DOH_Utilites();
                }
                return instance;
            }
        }

        public string GetProgramDataFolder
        { get {
                string pdFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),Edocs_Constants.EdocsProgFolder);
                return (pdFolder); } }
        public string DownLoadFolder
        { get; set; }

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
        public double GetFileSizeKB(long fileSizeInBytes)
        {
            double fileSizeInKB = fileSizeInBytes / 1024.0;
            return fileSizeInKB;
        }
        public void StartProcess(string processName, string processArgs, bool waitForExit,bool useShellExec)
        {
            try
            {


                Process myProcess = new Process();
                myProcess.StartInfo.UseShellExecute = useShellExec;
                myProcess.StartInfo.FileName = processName;
                myProcess.StartInfo.Arguments = processArgs;
                myProcess.StartInfo.CreateNoWindow = true;
                myProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                if (!(myProcess.Start()))
                {
                    throw new Exception($"Process {processName} with args {processArgs} did not start");
                }
                if (waitForExit)

                    myProcess.WaitForExit();
            }
            catch (Exception ex)
            {
                Send_Emails.EmailInstance.SendEmail($"Running Process {processName} with args {processArgs} {ex.Message}");
                throw new Exception($"Running Process {processName} with args {processArgs} {ex.Message}");
            }

        }
        public bool HasAccess(string folderPath)
        {
            try
            {
                File.WriteAllText(Path.Combine(folderPath, "Test_User_Access.txt"), "can i write this text");
                if (File.Exists(Path.Combine(folderPath, "Test_User_Access.txt")))
                    File.Delete(Path.Combine(folderPath, "Test_User_Access.txt"));
                return true;
            }
            catch (SecurityException e)
            {
                System.Windows.Forms.MessageBox.Show($"Error No Write Access to folder {folderPath} for user {Environment.UserName} {e.Message}", "No Write Access", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);  
                return false;
            }
        }
    }
}
