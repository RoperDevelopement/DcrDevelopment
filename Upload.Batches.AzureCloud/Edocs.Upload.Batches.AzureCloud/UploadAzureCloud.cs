using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Edocs.Libaray.AzureCloud.Upload.Batches;
using edl = EdocsUSA.Utilities.Logging;
using EdocsUSA.Utilities;
using System.IO;
using SE = ScanQuire_SendEmails;
using Edocs.Upload.Azure.Blob.Storage;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace Edocs.Upload.Batches.AzureCloud
{
    class UploadAzureCloud
    {
        private readonly string ArgBatchId = "/batchid:";
        private readonly string ArgArchiver = "/archiver:";
        private readonly string ArgEmailPW = "/encemailpw:";
        private readonly string EdosUsaIncStr = "e-Docs USA";
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
                Edocs_Utilities.EdocsUtilitiesInstance.CopyFile(traceLog, EdocsUSA.Utilities.SettingsManager.AuditLogsUploadDirectroy.Replace("e-Docs USA Inc", EdosUsaIncStr), false, string.Empty,true);
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
                traceLog = $"{tracelogFolder}{AssemblyInfo.GetAssemblyTitle()}_{DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")}.log";

            }
            else
                traceLog = $"{tracelogFolder}{AssemblyInfo.GetAssemblyTitle()}_{batchID}_{DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")}.log";
            edl.TraceLogger.TraceLoggerInstance.RunningAssembley = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.exe";
            edl.TraceLogger.TraceLoggerInstance.OpenTraceLogFile(traceLog, AssemblyInfo.GetAssemblyTitle());
            edl.TraceLogger.TraceLoggerInstance.WriteTraceHeader();
            edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Opened trace log file:{traceLog}");
            edl.TraceLogger.TraceLoggerInstance.TraceInformation($"AssemblyTitle:{AssemblyInfo.GetAssemblyTitle()}");
            edl.TraceLogger.TraceLoggerInstance.TraceInformation($"AssemblyCopyright:{AssemblyInfo.GetAssemblyCopyright()}");
            edl.TraceLogger.TraceLoggerInstance.TraceInformation($"AssemblyDescription:{AssemblyInfo.GetAssemblyDescription()}");
            edl.TraceLogger.TraceLoggerInstance.TraceInformation($"AssemblyVersion:{AssemblyInfo.GetAssemblyVersion()}");
        }

        private void UplaodImages(string batchID, string archiver)
        {
            AzureBlobStorage.BlobStorageInstance.AzureBlobAccountKey = UploadUtilities.AzureBlobAccountKey;
            AzureBlobStorage.BlobStorageInstance.AzureBlobAccountName = UploadUtilities.AzureBlobAccountName;
            AzureBlobStorage.BlobStorageInstance.AzureBlobStorageConnectionString = UploadUtilities.AzureBlobStorageConnectionString;
            //if(archiver.ToUpper() == "LABREQS"
            if (Properties.Settings.Default.AuditLogFolder)
                OpenTraceLog($"{archiver}_{batchID}");
            else
            {
                CloseTraceLog();
                OpenTraceLog(LogFolder, $"{archiver}_{batchID}");
            }
            edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Uploading images for batch id{batchID} archiver{archiver}");
            edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Uploading images for batch id{batchID} archiver{archiver}");
            if (string.Compare(archiver, "LABREQS", true) == 0)
            {
                EdocsUploadLabRecs edocsUploadLabRecs = new EdocsUploadLabRecs(batchID, archiver);
            }
            else
            {
                EdocsUploadRecs edocsUploadRecs = new EdocsUploadRecs(batchID, archiver);
            }






        }
        private void EncryptEmailPw(string emPw)
        {
            edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Encrypring email pw:{emPw}");
            SE.Send_Emails.EmailInstance.UpDateEmailPw(emPw);
            edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Email pw:{emPw} encrypted");
        }
        private void GetInputArgs(string[] args)
        {
            string batchID = string.Empty;
            string archiver = string.Empty;
            string emailPW = string.Empty;
            try
            {


                foreach (string inputArgs in args)
                {
                    edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Looking for input arg:{inputArgs}");
                    if (inputArgs.StartsWith(ArgBatchId, StringComparison.OrdinalIgnoreCase))
                    {
                        batchID = inputArgs.Substring(ArgBatchId.Length);
                        edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Found arg:{inputArgs} for input vlaue:{batchID}");
                        edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Found arg:{inputArgs} for input vlaue:{batchID}");

                    }
                    if (inputArgs.StartsWith(ArgArchiver, StringComparison.OrdinalIgnoreCase))
                    {
                        archiver = inputArgs.Substring(ArgArchiver.Length);
                        edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Found arg:{inputArgs} for input vlaue:{archiver}");
                    }
                    if (inputArgs.StartsWith(ArgEmailPW, StringComparison.OrdinalIgnoreCase))
                    {
                        emailPW = inputArgs.Substring(ArgEmailPW.Length);
                        edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Found arg:{inputArgs} to set email pw");
                    }
                }
                if ((string.IsNullOrEmpty(batchID)) && (string.IsNullOrEmpty(archiver)) && (string.IsNullOrEmpty(emailPW)))
                {
                    throw new Exception("Invalid args no args found");
                    //edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole("Usage:Edocs.Upload.Batches.AzureCloud.exe /batchid:22806e74-f914-454c-ac4d-7cbb91364501 /archiver:labrecs");
                    //edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole("Usage:Edocs.Upload.Batches.AzureCloud.exe /encemailpw:test");
                    //edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole("/encemailpw: this encrypts the email password");
                    //edl.TraceLogger.TraceLoggerInstance.TraceError("No args found");
                }
                else if (!(string.IsNullOrWhiteSpace(emailPW)))
                {
                    if (!(string.IsNullOrEmpty(batchID)) && (!(string.IsNullOrEmpty(archiver))))
                        throw new Exception($"Invlaid args:{batchID} {archiver} cannot be used when encrypting setting email pw");
                    EncryptEmailPw(emailPW.Trim());
                }
                else
                {
                    
                    EncryptEmailPw("6746edocs");
                    UplaodImages(batchID, archiver);
                    if ((Properties.Settings.Default.AuditLogFolder))
                        CopyAuditLogs();

                }

            }
            catch (Exception ex)
            {
                string message = $"Upload to azure cloud did not start on scanning machine {Environment.MachineName} error:{ex.Message}";
                UploadUtilities.SEmail(message, true);
                edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole("Usage:Edocs.Upload.Batches.AzureCloud.exe /batchid:22806e74-f914-454c-ac4d-7cbb91364501 /archiver:labrecs");
                edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole("Usage:Edocs.Upload.Batches.AzureCloud.exe /encemailpw:test");
                edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole("/encemailpw: this encrypts the email password");
                edl.TraceLogger.TraceLoggerInstance.TraceError("No args found");
                edl.TraceLogger.TraceLoggerInstance.TraceError($"{ex.Message}");
                edl.TraceLogger.TraceLoggerInstance.TraceErrorConsole($"{ex.Message}");
                UploadUtilities.NotifyUser(batchID, message);

            }



        }
        static void Main(string[] args)
        {
            UploadAzureCloud uploadAzureCloud = new UploadAzureCloud();

            uploadAzureCloud.OpenTraceLog(uploadAzureCloud.LogFolder, "nobatchid");
            uploadAzureCloud.GetInputArgs(args);
        }
    }
}
