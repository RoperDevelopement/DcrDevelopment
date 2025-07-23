using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Edocs.Upload.Archiver;
using edl = EdocsUSA.Utilities.Logging;
using EdocsUSA.Utilities;
using System.IO;
using Edocs.Libaray.Upload.Archive.Batches;
using SE = ScanQuire_SendEmails;
using System.Reflection;

//using BA = Edocs.Libaray.Upload.Archive.Batches;

//using ARC = Edocs.Libaray.Upload.Archive.Batches.Archivers;
namespace Edocs.Upload.Archiver
{
    class UpLoadArchiver
    {
        private readonly string ArgBatchId = "/batchid:";
        private readonly string ArgArchiverFolder = "/af:";
        private readonly string ArgArchiverName = "/archiver:";
        private readonly string ArgEmailPW = "/encemailpw:";
        private readonly string EdosUsaIncStr = "e-Docs USA";
        private readonly string MDTArchiver = "MDTPDFFiles";
        private readonly string BSBPropDep = "BSBPropDep";
        private readonly string BSBPWD = "BSBPWD";
        private readonly string PSUSD = "PSUSD";
        private readonly string DOH = "DOH";
        private string traceLog = string.Empty;
        private string TraceLog
        {
            get { return traceLog; }
            set { traceLog = value; }
        }
        private string LogFolder
        {
            get { return (Properties.Settings.Default.LogFolder.Replace("{ApplicationDir}", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).Replace("{ApplicarionName}", AssemblyInfo.GetAssemblyTitle())); }
        }
        private void CloseTraceLog()
        {
            edl.TraceLogger.TraceLoggerInstance.TraceInformation("Closing trace logging file");
            edl.TraceLogger.TraceLoggerInstance.CloseTraceFile();
        }
        private void CopyAuditLogs()
        {
            try
            {

                edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Copying audit log {traceLog}");
                edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Copying audit log {traceLog}");
                CloseTraceLog();
                Edocs_Utilities.EdocsUtilitiesInstance.CopyFile(traceLog, EdocsUSA.Utilities.SettingsManager.AuditLogsUploadDirectroy.Replace("e-Docs USA Inc", EdosUsaIncStr), false, string.Empty, true);
            }
            catch
            { }
        }
        private void OpenTraceLog(string batchID)
        {
            CloseTraceLog();

            string ald = SettingsManager.AuditLogsDirectroy.Replace("e-Docs USA Inc", EdosUsaIncStr);
            Directory.CreateDirectory(ald);



            ald = Edocs_Utilities.EdocsUtilitiesInstance.CheckDirectoryPath(ald);
            OpenTraceLog(ald, batchID);
        }
        private void OpenTraceLog(string tracelogFolder, string batchID)
        {
            edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Opening trace lof file for trace folder:{tracelogFolder} for batchid:{batchID}");

            if (batchID == "nobatchid")
            {
                Directory.CreateDirectory(tracelogFolder);
                traceLog = $"{tracelogFolder}{ AssemblyInfo.GetAssemblyTitle()}_{DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")}.log";

            }
            else
                traceLog = $"{tracelogFolder}{ AssemblyInfo.GetAssemblyTitle()}_{batchID}_{DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")}.log";
            edl.TraceLogger.TraceLoggerInstance.RunningAssembley = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.exe";
            edl.TraceLogger.TraceLoggerInstance.OpenTraceLogFile(traceLog, Edocs.Libaray.Upload.Archive.Batches.AssemblyInfo.GetAssemblyTitle());
            edl.TraceLogger.TraceLoggerInstance.WriteTraceHeader();
            edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Opened trace log file:{traceLog}");
            edl.TraceLogger.TraceLoggerInstance.TraceInformation($"AssemblyTitle:{Edocs.Libaray.Upload.Archive.Batches.AssemblyInfo.GetAssemblyTitle()}");
            edl.TraceLogger.TraceLoggerInstance.TraceInformation($"AssemblyCopyright:{Edocs.Libaray.Upload.Archive.Batches.AssemblyInfo.GetAssemblyCopyright()}");
            edl.TraceLogger.TraceLoggerInstance.TraceInformation($"AssemblyDescription:{Edocs.Libaray.Upload.Archive.Batches.AssemblyInfo.GetAssemblyDescription()}");
            edl.TraceLogger.TraceLoggerInstance.TraceInformation($"AssemblyVersion:{Edocs.Libaray.Upload.Archive.Batches.AssemblyInfo.GetAssemblyVersion()}");
        }
        private async Task ProcessFolder(string folder)
        {
            Edocs.Libaray.Upload.Archive.Batches.Archivers.MDTArchiver arcMDT = new Edocs.Libaray.Upload.Archive.Batches.Archivers.MDTArchiver();
            foreach (var uploadFiles in Directory.GetDirectories(folder, "*.*", SearchOption.AllDirectories))
            {
                try
                {
                    edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Uploading folder {uploadFiles}");
                    edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Uploading folder {uploadFiles}");
                    arcMDT.BatchId = Edocs.Libaray.Upload.Archive.Batches.UploadUtilities.GetBatchID(uploadFiles).ConfigureAwait(true).GetAwaiter().GetResult();
                    arcMDT.BatchDir = uploadFiles;
                    arcMDT.UploadMDTImagesPDF("MDTPDFFiles").ConfigureAwait(false).GetAwaiter().GetResult();
                }
                catch (Exception ex)
                {
                    edl.TraceLogger.TraceLoggerInstance.TraceError($"Uploading folder {uploadFiles} {ex.Message}");
                    edl.TraceLogger.TraceLoggerInstance.TraceErrorConsole($"Uploading folder {uploadFiles} {ex.Message}");
                }
            }
        }
        private async Task GetInputArgs(string[] args)
        {
            try
            {
                string batchID = string.Empty;
                string archiverName = string.Empty;
                string archiverFolder = string.Empty;
                foreach (string inputArgs in args)
                {
                    if (inputArgs.StartsWith(ArgBatchId, StringComparison.OrdinalIgnoreCase))
                    {
                        batchID = inputArgs.Substring(ArgBatchId.Length);
                    }

                    else if (inputArgs.StartsWith(ArgArchiverFolder, StringComparison.OrdinalIgnoreCase))
                    {
                        archiverFolder = inputArgs.Substring(ArgArchiverFolder.Length);
                    }
                    else if (inputArgs.StartsWith(ArgArchiverName, StringComparison.OrdinalIgnoreCase))
                    {
                        archiverName = inputArgs.Substring(ArgArchiverName.Length).Replace("r\n", "").Trim();
                    }


                    else
                        throw new Exception($"Invalid arg {inputArgs}");
                }


                if (!(string.IsNullOrWhiteSpace(batchID)))
                {
                    OpenTraceLog(batchID);
                    if (string.IsNullOrWhiteSpace(archiverName))
                        archiverName = Edocs.Libaray.Upload.Archive.Batches.UploadUtilities.GetArchiveName(archiverFolder, batchID).ConfigureAwait(false).GetAwaiter().GetResult();
                    if (string.Compare(MDTArchiver, archiverName, true) == 0)
                    {
                        Edocs.Libaray.Upload.Archive.Batches.Archivers.MDTArchiver arcMDT = new Edocs.Libaray.Upload.Archive.Batches.Archivers.MDTArchiver(batchID, archiverFolder);
                        arcMDT.UploadMDTImagesPDF(archiverName).ConfigureAwait(false).GetAwaiter().GetResult();

                    }
                    else if (string.Compare(BSBPropDep, archiverName, true) == 0)
                    {
                        Edocs.Libaray.Upload.Archive.Batches.Archivers.BSBPropDepArchiver arcBSB = new Edocs.Libaray.Upload.Archive.Batches.Archivers.BSBPropDepArchiver(batchID, archiverFolder);
                        arcBSB.UploadBSBImagesPDF(archiverName).ConfigureAwait(false).GetAwaiter().GetResult(); ;

                    }
                    else if (string.Compare(PSUSD, archiverName, true) == 0)
                    {
                        Edocs.Libaray.Upload.Archive.Batches.Archivers.PSUSDArchiver arcPSUSD = new Edocs.Libaray.Upload.Archive.Batches.Archivers.PSUSDArchiver(batchID, archiverFolder);
                        arcPSUSD.UploadPSUSDImagesPDF(archiverName, archiverFolder).ConfigureAwait(false).GetAwaiter().GetResult(); ;

                    }
                    else if (string.Compare(BSBPWD, archiverName, true) == 0)
                    {
                        Edocs.Libaray.Upload.Archive.Batches.Archivers.BSBPublicWorksArchiver arcPSUSD = new Edocs.Libaray.Upload.Archive.Batches.Archivers.BSBPublicWorksArchiver(batchID, archiverFolder);
                        arcPSUSD.UploadBSBPWDImagesPDF(archiverName, archiverFolder).ConfigureAwait(false).GetAwaiter().GetResult(); ;

                    }

                   //else if (string.Compare(DOH,archiverName, true) == 0)
                   //{
                   //    Edocs.Libaray.Upload.Archive.Batches.Archivers.BSBPublicWorksArchiver arcDOH = new Edocs.Libaray.Upload.Archive.Batches.Archivers.DOHArchiver(batchID, archiverFolder);
                   ////    arcPSUSD.UploadBSBPWDImagesPDF(archiverName, archiverFolder).ConfigureAwait(false).GetAwaiter().GetResult(); ;

                   // }




                    else if (string.IsNullOrWhiteSpace(archiverFolder))
                    {
                        throw new Exception("Invalid input args");
                        ProcessFolder(archiverFolder).ConfigureAwait(false).GetAwaiter().GetResult();
                    }
                    else
                        throw new Exception("Invalid input args");
                }
                else
                    throw new Exception("Invalid input args");
            }
            catch (Exception ex)
            {

                UploadUtilities.SEmail($"{ex.Message}", true);
                edl.TraceLogger.TraceLoggerInstance.TraceError($"Invalid args {ex.Message}");
                edl.TraceLogger.TraceLoggerInstance.TraceErrorConsole($"Invalid args {ex.Message}");
                edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole("Usage /af: archive folder");
                edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole("Usage /af: archive folder /batchid:");
                UploadUtilities.ExitCode = -1;

            }
        }
        static void Main(string[] args)
        {
            UpLoadArchiver UpLoadArch = new UpLoadArchiver();
            UpLoadArch.OpenTraceLog(UpLoadArch.LogFolder, "nobatchid");
            UpLoadArch.GetInputArgs(args).ConfigureAwait(false).GetAwaiter().GetResult();
            UpLoadArch.CloseTraceLog();
            Environment.Exit(UploadUtilities.ExitCode);
        }
    }
}
