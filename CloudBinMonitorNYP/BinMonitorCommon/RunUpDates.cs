using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using UpDateEdocsSoftware;
using System.Diagnostics;
using System.Reflection;
namespace BinMonitor.Common
{
    public class RunUpDates
    {
        private int numberDays;
        private string currentVersionNumber;

        public RunUpDates(int numberSinceLastCheck, string versionNumber)
        {
            numberDays = numberSinceLastCheck;
            currentVersionNumber = versionNumber;
        }
        public bool GetUpdates()
        {
            if (GetNumerDaysSinceCheckForUpDate())
            {
                string errorMessage = UpDateEdocsSoftWareCloud.UpdateCloudInstance.CheckForUpdates(BinUtilities.UpDateDownLoadFolder);
                if (!(string.IsNullOrEmpty(errorMessage)))
                {
                    System.Windows.Forms.MessageBox.Show(errorMessage, "Error", System.Windows.Forms.MessageBoxButtons.OK);
                    return false;
                }
            }
            return UpDateFound();

        }
        private bool UpDateFound()
        {
            UpDateEdocsSoftware.UpDateEdocsSoftWareCloud.UpdateCloudInstance.GetSqUpdatelInfo();
            string verFile = string.Format("{0}\\{1}", BinUtilities.UpDateDownLoadFolder, UpDateEdocsSoftware.UpDateEdocsSoftWareCloud.UpdateCloudInstance.VersionFileName);
            if (File.Exists(verFile))
            {
                string strVersion = BinUtilities.BinMointorUtilties.ReadFile(verFile);
                if (strVersion.Trim() != currentVersionNumber)
                    return true;
                else
                {
                    verFile = Path.GetDirectoryName(verFile);
                    BinUtilities.BinMointorUtilties.DeleteFiles(verFile);
                }
            }
            return false;
        }
        private bool GetNumerDaysSinceCheckForUpDate()
        {
            try
            {

                if (!(File.Exists(BinUtilities.UpDateFileName)))
                {
                    BinUtilities.BinMointorUtilties.WriteOutPut(BinUtilities.UpDateFileName, DateTime.Now.ToString());
                    return true;
                }

                DateTime dtOld = DateTime.Now;
                if (DateTime.TryParse(BinUtilities.BinMointorUtilties.ReadFile(BinUtilities.UpDateFileName).Trim(), out dtOld))
                {
                    DateTime dt = DateTime.Now;
                    TimeSpan ts = dt.Subtract(dtOld);
                    if (ts.Days >= numberDays)
                    {
                        return true;
                    }
                }
            }
            catch { }
            return false;
        }

        private void LastUpDateCheck()
        {
            BinUtilities.BinMointorUtilties.WriteOutPut(BinUtilities.UpDateFileName, DateTime.Now.ToString());
        }
        public bool RunUpdate()
        {
            try
            {
                UpDateEdocsSoftware.UpDateEdocsSoftWareCloud.UpdateCloudInstance.GetSqUpdatelInfo();
                string verFile = string.Format("{0}\\{1}", BinUtilities.UpDateDownLoadFolder, UpDateEdocsSoftware.UpDateEdocsSoftWareCloud.UpdateCloudInstance.VersionFileName);
                string updateFile = string.Format("{0}\\{1}", BinUtilities.UpDateDownLoadFolder, UpDateEdocsSoftware.UpDateEdocsSoftWareCloud.UpdateCloudInstance.UpDateFileName);
                string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var executable = System.Diagnostics.Process.GetCurrentProcess().MainModule;
                Process UpDateProcess = new Process();
                string processExe = $"{assemblyPath }\\{BinUtilities.UpDateProgramName}";
                // UpDateProcess.StartInfo.Arguments = $"\"{BinUtilities.UpDateFileName}\\\" {currentVersionNumber} {updateFile} \"{assemblyPath}\\\" \"{verFile}\\\" \"{BinUtilities.UpDateDownBackUpFolder}\\\" \"{executable.FileName}\"";
                UpDateProcess.StartInfo.Arguments = $"\"{BinUtilities.UpDateFileName},{currentVersionNumber},{updateFile},{assemblyPath}\\,{verFile},{BinUtilities.UpDateDownBackUpFolder}\\,{executable.FileName}\"";
                Trace.TraceInformation($"Running update with paramater {processExe} {UpDateProcess.StartInfo.Arguments}");
                UpDateProcess.StartInfo.FileName = processExe;
                UpDateProcess.Start();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"Error running update process message:{ex.Message}", "Error Running Updates", System.Windows.Forms.MessageBoxButtons.OK);
                return false;
            }
            return true;
        }
    }
}
