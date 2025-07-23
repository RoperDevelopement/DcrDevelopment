using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Web.Script.Serialization;
using System.Security.Cryptography;
using Edocs.Service.UploadAuditLogs.Properties;


using TL = EdocsUSA.Utilities.Logging.TraceLogger;
using Azure.Storage.Blobs.Models;
using ABS = Edocs.Upload.Azure.Blob.Storage.AzureBlobStorage;

namespace Edocs.Service.UploadAuditLogs
{
    public enum AzureBolbStorageTypes
    {
        AppendBlob,
        BlockBlob,
        AllBlobs,
        ScanQuire
    }
    public class ProcessAuditLogs : IDisposable
    {


        private string eMessage = string.Empty;
        private string auditLogsDownLoadFolder = string.Empty;
        private string AuditLogsScanQuire
        {
            // get { return ServiceHelpers.Instance.CheckFolderPath(Settings.Default.AuditLogsFolder.Replace(CommonApplicationData, Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)).Replace(AssemblyCompany, ServiceHelpers.Instance.GetAssemblyCompany())); }
            get { return Settings.Default.AuditLogsScanQuire; }

        }
        //private string AuditLogsUploadFolder
        //{
        //    get { return ServiceHelpers.Instance.CheckFolderPath(Settings.Default.AuditLogsUploadFolder.Replace(CommonApplicationData, Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)).Replace(AssemblyCompany, ServiceHelpers.Instance.GetAssemblyCompany())); }
        //}
        private string AzureBlobStorageConnectionString
        {


            get { return Settings.Default.AzureBlobStorageConnectionString; }
        }

        private string AzureBlobContanierAuditShare
        {


            get { return Settings.Default.AzureBlobContanierAuditShare; }
        }


        private string AzureBlobContanierAuditUploadShare
        {


            get { return Settings.Default.AzureBlobContanierAuditUploadShare; }
        }

        private string AzureBlobAccountName
        {


            get { return Settings.Default.AzureBlobAccountName; }
        }

        private bool SendEmailsErrorsOnly
        {
            get { return Settings.Default.SendEmailsErrorsOnly; }
        }
        private string AzureBlobAccountKey
        {


            get { return Settings.Default.AzureBlobAccountKey; }
        }
        public StringBuilder SbErrorMessages
        { get; set; }
        private string AuditLogsDownLoadFolder
        {


            get { return ($"{ServiceHelpers.Instance.CheckFolderPath(ServiceHelpers.Instance.GetApplicationDir())}{Settings.Default.AuditLogsDownLoadFolder}"); }
            set { auditLogsDownLoadFolder = value; }
        }

        private int TotalFilesProcessed
        { get; set; } = 0;
        private int TotalFilesUpLoaded
        { get; set; } = 0;


        private bool UpLoadScanQuireAL
        {
            get { return Settings.Default.UpLoadScanQuireAL; }
        }
        private string RegxMatchFiles
        { get { return Settings.Default.RegxMatchFiles; } }
        private Uri WebApi
        {
            get { return new Uri(Settings.Default.WebApi); }
        }


        public ProcessAuditLogs()
        {
            TL.TraceLoggerInstance.StartStopStopWatch();
            OpenAuditLog();
            SbErrorMessages = new StringBuilder();
        }
        private void OpenAuditLog()
        {
            try
            {
                TL.TraceLoggerInstance.OpenTraceLogFile($"{ServiceHelpers.Instance.CheckFolderPath(ServiceHelpers.Instance.GetApplicationDir())}AuditLog\\{ServiceHelpers.Instance.GetAssemblyName()}_AuditLog_{DateTime.Now.ToString("MM_dd_yyyy")}.log", "AuditLog", true);
                TL.TraceLoggerInstance.RunningAssembley = ServiceHelpers.Instance.GetAssemblyName();
                TL.TraceLoggerInstance.TraceInformation(($@"Company:{ServiceHelpers.Instance.GetAssemblyCompany()} CompanyName:{ServiceHelpers.Instance.GetAssemblyCompnayName()} 
                                              CopyRight:{ServiceHelpers.Instance.GetAssemblyCopyright()} Product:{ServiceHelpers.Instance.GetAssemblyProduct()}
                                      Title:{ServiceHelpers.Instance.GetAssemblyTitle()} FileVersion:{ServiceHelpers.Instance.GetAssemblyFileVersion()} Assembly Version:{ServiceHelpers.Instance.GetAssemblyVersion()}"));
            }
            catch (Exception ex)
            {
                SbErrorMessages.AppendLine($"<p style={ServiceHelpers.Quote}background-color:red{ServiceHelpers.Quote} >Error Opening trace log file {ServiceHelpers.Instance.CheckFolderPath(ServiceHelpers.Instance.GetApplicationDir())}{ServiceHelpers.Instance.GetAssemblyName()}_AuditLog_{DateTime.Now.ToString("MM_dd_yyyy")}.log total time {TL.TraceLoggerInstance.StopStopWatch()} {ex.Message}</p></br>");
                SEmail.SendEMailsInstance.EmailSend(SbErrorMessages.ToString(), true);
            }
        }
        private async Task UploadServiceAuditLogs()
        {
            string serviceALF = $"{ServiceHelpers.Instance.CheckFolderPath(ServiceHelpers.Instance.GetApplicationDir())}AuditLog";
            ServiceHelpers.Instance.CreateDir(serviceALF);
            TL.TraceLoggerInstance.TraceInformation($"Method UploadServiceAuditLogs for folder {serviceALF}");
            foreach (string als in Directory.GetFiles(serviceALF, "*.*"))
            {
                try
                {

                    var fileInfo = new FileInfo(als);
                    if (fileInfo.CreationTime.Date != DateTime.Now.Date)
                    {
                        TotalFilesProcessed++;
                        TL.TraceLoggerInstance.TraceInformation($"Method UploadServiceAuditLogs file {als}");
                        UploadAzureAuditLog(als, AzureBlobContanierAuditShare, AzureBolbStorageTypes.ScanQuire, fileInfo.CreationTime).ConfigureAwait(false).GetAwaiter().GetResult();
                    }
                }
                catch (Exception ex)
                {
                    TL.TraceLoggerInstance.TraceError($"Method UploadServiceAuditLogs file {als} {ex.Message}");
                    SbErrorMessages.AppendLine($"<p>Method UploadServiceAuditLogs file {als} {ex.Message}</p></br>");
                }



            }
        }
        public async Task UpLoadAuditLogs(AzureBolbStorageTypes storageTypes)
        {

            try
            {

                if (ServiceHelpers.Instance.CheckUpLoadAuditFiles().ConfigureAwait(true).GetAwaiter().GetResult())
                {

                    ABS.BlobStorageInstance.AzureBlobAccountKey = AzureBlobAccountKey;
                    ABS.BlobStorageInstance.AzureBlobAccountName = AzureBlobAccountName;
                    ABS.BlobStorageInstance.AzureBlobStorageConnectionString = AzureBlobStorageConnectionString;
                    auditLogsDownLoadFolder = ServiceHelpers.Instance.CheckFolderPath(AuditLogsDownLoadFolder);
                    UploadServiceAuditLogs().ConfigureAwait(false).GetAwaiter().GetResult();
                    if (UpLoadScanQuireAL)
                    {
                        TL.TraceLoggerInstance.TraceInformation($"Method UpLoadAuditLogs for scanquire");
                        UploadScanQuireAuditLogs().ConfigureAwait(false).GetAwaiter().GetResult();
                    }

                    else
                    {
                        TL.TraceLoggerInstance.TraceInformation($"Start method UpLoadAuditLogs");
                        TL.TraceLoggerInstance.TraceInformation($"In method UpLoadAuditLogs from azure cloud using download folder {auditLogsDownLoadFolder} for storage type :{storageTypes.ToString()}");
                        ServiceHelpers.Instance.CreateDir(auditLogsDownLoadFolder);
                        if (storageTypes == AzureBolbStorageTypes.AllBlobs)
                        {
                            AuditLogsUpload(AzureBolbStorageTypes.AppendBlob, AzureBlobContanierAuditUploadShare).ConfigureAwait(true).GetAwaiter().GetResult();
                            AuditLogsUpload(AzureBolbStorageTypes.BlockBlob, AzureBlobContanierAuditUploadShare).ConfigureAwait(true).GetAwaiter().GetResult();
                        }
                        else
                            AuditLogsUpload(storageTypes, AzureBlobContanierAuditUploadShare).ConfigureAwait(true).GetAwaiter().GetResult();
                    }
                }
                else
                {
                    TL.TraceLoggerInstance.TraceInformation($"Not running since datetime is {DateTime.Now.ToString()} and run start time set to {ServiceHelpers.Instance.RunStartTime.ToString()} and run end time is set to {ServiceHelpers.Instance.RunEndTime.ToString()} and run on weekends is set to {ServiceHelpers.Instance.RunOnWeekEnds.ToString()} ");
                }
            }
            catch (Exception ex)
            {
                TL.TraceLoggerInstance.TraceError($"In method UpLoadAuditLogs for storage type :{storageTypes.ToString()} {ex.Message}");
                SbErrorMessages.AppendLine($"<p style={ServiceHelpers.Quote}background-color:red{ServiceHelpers.Quote}>In method UpLoadAuditLogs for storage type :{storageTypes.ToString()} {ex.Message}</p></br>");
            }
            string tRun = TL.TraceLoggerInstance.StopStopWatch();
            SendEmail(tRun).ConfigureAwait(false).GetAwaiter().GetResult();

            TL.TraceLoggerInstance.TraceInformation($"End method UpLoadAuditLogs total time {tRun}");
            TL.TraceLoggerInstance.CloseTraceFile();
        }
        public async Task SendEmail(string tRun)
        {
            try
            {
                TL.TraceLoggerInstance.TraceInformation("Method sendemail");
                TL.TraceLoggerInstance.TraceInformation($"Method Total Files uploaded {TotalFilesUpLoaded.ToString()} Total Files processed {TotalFilesProcessed.ToString()}");
                if (SbErrorMessages.Length > 0)
                {
                    SbErrorMessages.AppendLine($"<p>Total Files uploaded {TotalFilesUpLoaded.ToString()}</p></br>");
                    SbErrorMessages.AppendLine($"<p>Total Files processed {TotalFilesProcessed.ToString()}</p></br>");
                    SbErrorMessages.AppendLine($"<p>Done uploading audit files total time {tRun}</p></br>");
                    SEmail.SendEMailsInstance.EmailSend(SbErrorMessages.ToString(), true);
                }

                else
                {
                    if (!(SendEmailsErrorsOnly))
                    {
                        SbErrorMessages.AppendLine($"<p>Total Files uploaded {TotalFilesUpLoaded.ToString()}</p></br>");
                        SbErrorMessages.AppendLine($"<p>Total Files processed {TotalFilesProcessed.ToString()}</p></br>");
                        SbErrorMessages.AppendLine($"<p>Done uploading audit files total time {tRun}</p></br>");
                        SEmail.SendEMailsInstance.EmailSend(SbErrorMessages.ToString(), false);
                    }
                }
                
                TL.TraceLoggerInstance.TraceInformation($"Email Sent");
            }
            catch (Exception ex)
            {
                TL.TraceLoggerInstance.TraceError($"Method sendemail {ex.Message}");
            }
        }
        public async Task UploadScanQuireAuditLogs()
        {
            TL.TraceLoggerInstance.TraceInformation($"Method UploadScanQuireAuditLogs for folders {AuditLogsScanQuire}");
            string[] audFolders = AuditLogsScanQuire.Split(',');

            foreach (var alStr in audFolders)
            {
                if (string.IsNullOrWhiteSpace(alStr))
                    continue;
                TL.TraceLoggerInstance.TraceInformation($"Method UploadScanQuireAuditLogs processing folder {alStr}");
                foreach (string csvFile in Directory.GetFiles(alStr, "*.*"))
                {
                    var fileInfo = new FileInfo(csvFile);
                    TotalFilesProcessed++;
                    try
                    {
                        TL.TraceLoggerInstance.TraceInformation($"Method UploadScanQuireAuditLogs processing file {csvFile}");
                        UploadAzureAuditLog(csvFile, AzureBlobContanierAuditShare, AzureBolbStorageTypes.ScanQuire, fileInfo.CreationTime).ConfigureAwait(false).GetAwaiter().GetResult();
                    }
                    catch (Exception ex)
                    {
                        TL.TraceLoggerInstance.TraceError($"Method UploadScanQuireAuditLogs processing file {csvFile} {ex.Message}");
                        SbErrorMessages.AppendLine($"<p style={ServiceHelpers.Quote}background-color:red{ServiceHelpers.Quote}>Method UploadScanQuireAuditLogs processing file {csvFile} {ex.Message}</p> </br>");
                    }

                }

            }

        }
        public async Task AuditLogsUpload(AzureBolbStorageTypes bolbStorageTypes, string azureContanier)
        {
            TL.TraceLoggerInstance.TraceInformation($"Method AuditLogsUpload for storage type {bolbStorageTypes.ToString()} for azure container {azureContanier}");
            if (bolbStorageTypes == AzureBolbStorageTypes.AllBlobs)
            {
                GetAzureAuditLogsAllBlobs(azureContanier).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            else if (bolbStorageTypes == AzureBolbStorageTypes.AppendBlob)
            {
                GetAzureAppendBlobAuditLogs(azureContanier).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            else
            {
                GetAzureBlockBlobAuditLogs(azureContanier).ConfigureAwait(false).GetAwaiter().GetResult();
            }

        }
        private async Task<List<AuditLogsUploadModel>> GetAuditLogs(string csvFileName)
        {
            List<AuditLogsUploadModel> values = File.ReadAllLines(csvFileName)
                    .Skip(1).Select(v => AuditLogsUploadModel.GetAuditLogsModels(v, csvFileName)).ToList();
            return values;
        }
        private async Task<List<AuditLogsUploadModel>> GetStrAuditLogs(string auditLogText, string fileName)
        {
            List<AuditLogsUploadModel> values = File.ReadAllLines(auditLogText)
                    .Skip(1).Select(v => AuditLogsUploadModel.GetAuditLogsModels(v, fileName)).ToList();
            return values;
        }
        private async Task GetAzureAppendBlobAuditLogs(string azureContanier)
        {
            TL.TraceLoggerInstance.TraceInformation($"In Method GetAzureAppendBlobAuditLogs for azure container {azureContanier}");
            string fileName = string.Empty;
            string dlFileName = string.Empty;
            foreach (var azureAL in ABS.BlobStorageInstance.GetCloudAppendBlobFiles(null, azureContanier))
            {
                TotalFilesProcessed++;
                try
                {
                    ServiceHelpers.Instance.DeleteFile($"{auditLogsDownLoadFolder}{azureAL.Name}");
                    fileName = Path.GetFileName(azureAL.StorageUri.ToString());
                    dlFileName = ABS.BlobStorageInstance.DownLoadFileAppendBlod(azureContanier, fileName, auditLogsDownLoadFolder).ConfigureAwait(true).GetAwaiter().GetResult();
                    string lfComp = ServiceHelpers.Instance.CheckAuditLogFileComplete(dlFileName).ConfigureAwait(false).GetAwaiter().GetResult();
                    if (!(string.IsNullOrWhiteSpace(lfComp)))
                    {

                        SbErrorMessages.AppendLine($"<p style={ServiceHelpers.Quote}background-color:red{ServiceHelpers.Quote}>Error {dlFileName} {lfComp}</p></br>");
                        TL.TraceLoggerInstance.TraceError($"<p>Error {dlFileName} {lfComp}</p></br>");

                    }
                    TL.TraceLoggerInstance.TraceInformation($"In Method GetAzureAppendBlobAuditLogs for download filename {dlFileName} getting azure file name {fileName}");
                    lfComp = ServiceHelpers.Instance.CheckAuditLogFileSize(dlFileName).ConfigureAwait(false).GetAwaiter().GetResult();
                    if (string.IsNullOrWhiteSpace(lfComp))
                        UploadAzureAuditLog(dlFileName, AzureBlobContanierAuditShare, AzureBolbStorageTypes.AppendBlob, azureAL.Create, azureAL.StorageUri).ConfigureAwait(false).GetAwaiter().GetResult();
                    else
                    {
                        SbErrorMessages.AppendLine($"<p style={ServiceHelpers.Quote}background-color:red{ServiceHelpers.Quote}>Error {dlFileName} {lfComp}</p></br>");
                        TL.TraceLoggerInstance.TraceError($"<p>Error {dlFileName} {lfComp}</p></br>");
                        ABS.BlobStorageInstance.DeleteAzureBlobFile(Path.GetFileName(fileName), azureContanier).ConfigureAwait(false).GetAwaiter().GetResult();
                    }

                }
                catch (Exception ex)
                {
                    TL.TraceLoggerInstance.TraceError($"In Method GetAzureAppendBlobAuditLogs for download filename {dlFileName} getting azure file name {fileName} {ex.Message}");
                    SbErrorMessages.AppendLine($"<p>In Method GetAzureAppendBlobAuditLogs for download filename {dlFileName} getting azure file name {fileName} {ex.Message}</p></br>");
                }
            }


        }
        private async Task UploadAzureAuditLog(string csvFile, string azureContanierALFoler, AzureBolbStorageTypes azureBolbStorageTypes, DateTimeOffset? timeOffset, Uri auditULFolder = null)
        {
            try
            {
                TL.TraceLoggerInstance.TraceInformation($"Method UploadAzureAuditLog for csv file {csvFile} AzureBolbStorageType {azureBolbStorageTypes.ToString()} ");
                if (!(File.Exists(csvFile)))
                {
                    TL.TraceLoggerInstance.TraceError($"Method UploadAzureAuditLog csv file {csvFile} not found AzureBolbStorageType {azureBolbStorageTypes.ToString()} ");
                    throw new Exception($"Method UploadAzureAuditLog csv file {csvFile} not found AzureBolbStorageType {azureBolbStorageTypes.ToString()} ");
                }
                string uriAuditLogs = string.Empty;
                Uri uri = WebApi;
                AuditLogsUploadModel.UploadList = new List<string>();
                List<AuditLogsUploadModel> logsModels = GetAuditLogs(csvFile).ConfigureAwait(true).GetAwaiter().GetResult();
                logsModels = logsModels.Where(p => p.AuditLogApplicationName != "na").ToList();
                if (logsModels.Count > 0)
                {
                    uriAuditLogs = $"{azureContanierALFoler }/{logsModels[0].AuditLogDate.ToString("yyyy-MM-dd")}";
                }
                else
                {
                    TL.TraceLoggerInstance.TraceInformation($"Method UploadAzureAuditLog no records in csv file {csvFile} AzureBolbStorageType {azureBolbStorageTypes.ToString()} ");
                    throw new Exception($"Method UploadAzureAuditLog no records in csv file {csvFile} AzureBolbStorageType {azureBolbStorageTypes.ToString()} ");
                }

                Uri upLoadUri = null;
                foreach (var aLogs in logsModels)
                {
                    if (upLoadUri == null)
                    {

                        TL.TraceLoggerInstance.TraceInformation($"Method UploadAzureAuditLog uploading file to azure cloud {csvFile} azure container {uriAuditLogs} AzureBolbStorageType {azureBolbStorageTypes.ToString()} ");
                        upLoadUri = ABS.BlobStorageInstance.UploadAzureBlobTextFile(ServiceHelpers.Instance.GetFileData(csvFile).ConfigureAwait(false).GetAwaiter().GetResult(), Path.GetFileName(csvFile).Replace(".log", ".csv"), uriAuditLogs, true).ConfigureAwait(false).GetAwaiter().GetResult();
                        TotalFilesUpLoaded++;
                    }
                    aLogs.AuditLogUrl = upLoadUri;
                    TL.TraceLoggerInstance.TraceInformation($"Method UploadAzureAuditLog uploading file to  {aLogs.AuditLogUrl} to database web {uri.ToString()} to controller {AuditLogsUploadModel.AuditLogsController} ");
                    AuditLogsWebApi.ALInstance.UpLoadAuditLogsDataBase(uri, AuditLogsUploadModel.AuditLogsController, aLogs).ConfigureAwait(false).GetAwaiter().GetResult();

                }

                switch (azureBolbStorageTypes)
                {
                    case AzureBolbStorageTypes.AppendBlob:
                        if (ServiceHelpers.Instance.CheckDelFile(RegxMatchFiles, csvFile, timeOffset))
                            ABS.BlobStorageInstance.DeleteAzureAppendBlobFile(Path.GetFileName(csvFile), AzureBlobContanierAuditUploadShare).ConfigureAwait(false).GetAwaiter().GetResult();
                        break;
                    case AzureBolbStorageTypes.BlockBlob:
                        if (ServiceHelpers.Instance.CheckDelFile(RegxMatchFiles, csvFile, timeOffset))
                            ABS.BlobStorageInstance.DeleteAzureBlobFile(Path.GetFileName(csvFile), AzureBlobContanierAuditUploadShare).ConfigureAwait(false).GetAwaiter().GetResult();
                        break;
                    case AzureBolbStorageTypes.ScanQuire:
                        if (ServiceHelpers.Instance.CheckDelFile(RegxMatchFiles, csvFile, timeOffset))
                            ServiceHelpers.Instance.DeleteFile(csvFile);
                        break;

                }
                ServiceHelpers.Instance.DeleteFile(csvFile);
            }
            catch (Exception ex)
            {
                TL.TraceLoggerInstance.TraceError($"Method UploadAzureAuditLog for csv file {csvFile} AzureBolbStorageType {azureBolbStorageTypes.ToString()} {ex.Message} ");
                SbErrorMessages.AppendLine($"<p style={ServiceHelpers.Quote}background-color:red{ServiceHelpers.Quote}>Method UploadAzureAuditLog for csv file {csvFile} AzureBolbStorageType {azureBolbStorageTypes.ToString()} {ex.Message}</p></br> ");

            }

        }
        private async Task GetAzureAuditLogsAllBlobs(string azureContanier)
        {
            TL.TraceLoggerInstance.TraceInformation($"Method GetAzureAuditLogsAllBlobs azure container:{azureContanier} ");
            foreach (var azureAL in ABS.BlobStorageInstance.GetCloudAppendBlobFiles(null, azureContanier))
            {
                try
                {

                    TotalFilesProcessed++;
                    string dlFileName = string.Empty;
                    var blobType = azureAL.GetType().ToString();
                    string azureFileName = Path.GetFileName(azureAL.StorageUri.ToString());
                    if (string.Compare(blobType, "Microsoft.Azure.Storage.Blob.CloudAppendBlob", true) == 0)
                    {

                        TL.TraceLoggerInstance.TraceInformation($"Method GetAzureAuditLogsAllBlobs downloading file {azureFileName} from azure container {azureContanier} for blob tye: {blobType} ");
                        dlFileName = ABS.BlobStorageInstance.DownLoadFileAppendBlod(azureContanier, azureFileName, auditLogsDownLoadFolder).ConfigureAwait(true).GetAwaiter().GetResult();
                        string lfComp = ServiceHelpers.Instance.CheckAuditLogFileComplete(dlFileName).ConfigureAwait(false).GetAwaiter().GetResult();
                        if (!(string.IsNullOrWhiteSpace(lfComp)))
                        {

                            SbErrorMessages.AppendLine($"<p style={ServiceHelpers.Quote}background-color:red{ServiceHelpers.Quote}>Error {dlFileName} {lfComp}</p></br>");
                            TL.TraceLoggerInstance.TraceError($"<p style={ServiceHelpers.Quote}background-color:red{ServiceHelpers.Quote}>Error {dlFileName} {lfComp}</p></br>");
                        }
                        TL.TraceLoggerInstance.TraceInformation($"Method GetAzureAuditLogsAllBlobs uploading  file {dlFileName} from azure container {azureContanier} for blob tye: {blobType} for azure uri azureAL.Uri");
                        lfComp = ServiceHelpers.Instance.CheckAuditLogFileSize(dlFileName).ConfigureAwait(false).GetAwaiter().GetResult();
                        if (string.IsNullOrWhiteSpace(lfComp))
                            UploadAzureAuditLog(dlFileName, AzureBlobContanierAuditShare, AzureBolbStorageTypes.AppendBlob, azureAL.Create, azureAL.StorageUri).ConfigureAwait(false).GetAwaiter().GetResult();
                        else
                        {
                            SbErrorMessages.AppendLine($"<p style={ServiceHelpers.Quote}background-color:red{ServiceHelpers.Quote}>Error {dlFileName} {lfComp}</p></br>");
                            TL.TraceLoggerInstance.TraceError($"<p style={ServiceHelpers.Quote}background-color:red{ServiceHelpers.Quote}>Error {dlFileName} {lfComp}</p></br>");
                            ABS.BlobStorageInstance.DeleteAzureBlobFile(Path.GetFileName(azureFileName), azureContanier).ConfigureAwait(false).GetAwaiter().GetResult();
                        }

                    }

                    else
                    {
                        TL.TraceLoggerInstance.TraceInformation($"Method GetAzureAuditLogsAllBlobs downloading file {azureFileName} from azure container {azureContanier} for blob tye: {blobType} ");
                        dlFileName = ABS.BlobStorageInstance.DownLoadFileBlockBlod(azureContanier, azureFileName, auditLogsDownLoadFolder).ConfigureAwait(true).GetAwaiter().GetResult();
                        string lfComp = ServiceHelpers.Instance.CheckAuditLogFileComplete(dlFileName).ConfigureAwait(false).GetAwaiter().GetResult();
                        if (!(string.IsNullOrWhiteSpace(lfComp)))
                        {

                            SbErrorMessages.AppendLine($"<p style={ServiceHelpers.Quote}background-color:red{ServiceHelpers.Quote}>Error {dlFileName} {lfComp}</p></br>");
                            TL.TraceLoggerInstance.TraceError($"<p>Error {dlFileName} {lfComp}</p></br>");
                        }
                        TL.TraceLoggerInstance.TraceInformation($"Method GetAzureAuditLogsAllBlobs uploading  file {dlFileName} from azure container {azureContanier} for blob tye: {blobType} for azure uri azureAL.Uri");
                        lfComp = ServiceHelpers.Instance.CheckAuditLogFileSize(dlFileName).ConfigureAwait(false).GetAwaiter().GetResult();
                        if (string.IsNullOrWhiteSpace(lfComp))
                            UploadAzureAuditLog(dlFileName, AzureBlobContanierAuditShare, AzureBolbStorageTypes.BlockBlob, azureAL.Create, azureAL.StorageUri).ConfigureAwait(false).GetAwaiter().GetResult();
                        else
                        {
                            SbErrorMessages.AppendLine($"<p style={ServiceHelpers.Quote}background-color:red{ServiceHelpers.Quote}>Error {dlFileName} {lfComp}</p></br>");
                            TL.TraceLoggerInstance.TraceError($"<p>Error {dlFileName} {lfComp}</p></br>");
                            ABS.BlobStorageInstance.DeleteAzureBlobFile(Path.GetFileName(azureFileName), azureContanier).ConfigureAwait(false).GetAwaiter().GetResult();
                        }

                    }
                }
                catch (Exception ex)
                {
                    TL.TraceLoggerInstance.TraceInformation($"Method GetAzureAuditLogsAllBlobs azure container:{azureContanier} {ex.Message} ");
                    SbErrorMessages.AppendLine($"<p style={ServiceHelpers.Quote}background-color:red{ServiceHelpers.Quote}>Method GetAzureAuditLogsAllBlobs azure container:{azureContanier} {ex.Message}</p></br> ");
                }

            }


        }
        private async Task GetAzureBlockBlobAuditLogs(string azureContanier)
        {
            TL.TraceLoggerInstance.TraceInformation($"In Method GetAzureBlockBlobAuditLogs for azure container {azureContanier}");
            string azureFileName = string.Empty;
            string dlFileName = string.Empty;

            foreach (var azureAL in ABS.BlobStorageInstance.GetCloudBlockBlobFiles(null, azureContanier))
            {
                try
                {
                    TotalFilesProcessed++;
                    ServiceHelpers.Instance.DeleteFile($"{auditLogsDownLoadFolder}{azureAL.Name}");
                    azureFileName = Path.GetFileName(azureAL.StorageUri.AbsoluteUri);
                    dlFileName = ABS.BlobStorageInstance.DownLoadFileBlockBlod(azureContanier, azureFileName, auditLogsDownLoadFolder).ConfigureAwait(true).GetAwaiter().GetResult();
                  
                    string lfComp = ServiceHelpers.Instance.CheckAuditLogFileComplete(dlFileName).ConfigureAwait(false).GetAwaiter().GetResult();
                    if (!(string.IsNullOrWhiteSpace(lfComp)))
                    {

                        SbErrorMessages.AppendLine($"<p style={ServiceHelpers.Quote}background-color:red{ServiceHelpers.Quote}>Error {dlFileName} {lfComp}</p></br>");
                        TL.TraceLoggerInstance.TraceError($"<p>Error {dlFileName} {lfComp}</p></br>");
                    }
                    lfComp = ServiceHelpers.Instance.CheckAuditLogFileSize(dlFileName).ConfigureAwait(false).GetAwaiter().GetResult();
                    TL.TraceLoggerInstance.TraceInformation($"In Method GetAzureAppendBlobAuditLogs for download filename {dlFileName} getting azure file name {azureFileName} for azure uri {azureAL.StorageUri.ToString()}");
                    if(string.IsNullOrWhiteSpace(lfComp))
                    UploadAzureAuditLog(dlFileName, AzureBlobContanierAuditShare, AzureBolbStorageTypes.BlockBlob, azureAL.Create, azureAL.StorageUri).ConfigureAwait(false).GetAwaiter().GetResult();
                    else
                    {
                        SbErrorMessages.AppendLine($"<p style={ServiceHelpers.Quote}background-color:red{ServiceHelpers.Quote}>Error {dlFileName} {lfComp}</p></br>");
                        TL.TraceLoggerInstance.TraceError($"<p>Error {dlFileName} {lfComp}</p></br>");
                        ABS.BlobStorageInstance.DeleteAzureBlobFile(Path.GetFileName(azureFileName), azureContanier).ConfigureAwait(false).GetAwaiter().GetResult();
                    }

                }
                catch (Exception ex)
                {
                    TL.TraceLoggerInstance.TraceError($"In Method GetAzureBlockBlobAuditLogs for azure container for azure file name {azureFileName} for download filename {dlFileName} {azureContanier} {ex.Message}");
                    SbErrorMessages.AppendLine($"<p>In Method GetAzureBlockBlobAuditLogs for azure container for azure file name {azureFileName} for download filename {dlFileName} {azureContanier} {ex.Message}</p></br>");

                }
            }


        }



        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {

                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~ProcessAuditLogs() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            try
            {
                // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
                Dispose(true);
                // TODO: uncomment the following line if the finalizer is overridden above.
                GC.SuppressFinalize(this);
            }
            catch { }
        }
        #endregion


    }
}



