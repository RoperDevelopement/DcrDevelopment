using System;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using EdocsUSA.Utilities;
using edl = EdocsUSA.Utilities.Logging;
using System.Diagnostics;
namespace Edocs.Libaray.AzureCloud.Upload.Batches
{
    public class EdocsUploadRecs
    {
        private string batchId;
        private string batchDir;
        public EdocsUploadRecs(string idBatch, string archiver)
        {
            batchId = idBatch;
            archiver = Edocs_Utilities.EdocsUtilitiesInstance.CheckDirectoryPath(archiver);
            batchDir = $"{Edocs_Utilities.EdocsUtilitiesInstance.CheckDirectoryPath(UploadUtilities.InputDir)}{archiver}{idBatch}\\";
            edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Getting lab records for batch id {batchId} from folder {batchDir}");
            edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Getting lab records for batch id {batchId} from folder {batchDir}");
            UploadRecords(archiver);
        }

        private void UploadRecords(string archiver)
        {
            string message;
            Stopwatch executionTimer = Stopwatch.StartNew();
            edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Start time upload lab recs {DateTime.Now.ToString()} for archiver:{archiver}");
            int totRecodsRead = 0;
            int totRecordsInFile = 0;
            int totdup = 0;
            try
            {
                string batchRecordsFile = UploadUtilities.GetBatchRecordsFileName(batchDir, batchId);
                string batchSettingsFile = UploadUtilities.GetBatchSettingsFileName(batchDir, batchId);
                Edocs_Utilities.EdocsUtilitiesInstance.CopyFile(batchRecordsFile, UploadUtilities.JsonFilesBackUpFolder, true, string.Empty, false);
                Edocs_Utilities.EdocsUtilitiesInstance.CopyFile(batchSettingsFile, UploadUtilities.JsonFilesBackUpFolder, true, string.Empty, false);
                UploadUtilities.BackupFiles(batchRecordsFile, batchSettingsFile).ConfigureAwait(false).GetAwaiter().GetResult();
              
                //   Edocs_Utilities.EdocsUtilitiesInstance.CopyFiles()


                edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Getting records json file {batchRecordsFile}");
                edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Getting batch json setting file {batchSettingsFile}");
                JsonSettingsFileModel batchSettings = UploadUtilities.GetBatchSettingsObject<JsonSettingsFileModel>(batchSettingsFile);
                edl.TraceLogger.TraceLoggerInstance.UserName = batchSettings.ScanOperator;
                AzureCloudBatchRecordsModel[] azureCloudBatches = UploadUtilities.ReadRecords(batchRecordsFile, ref totRecordsInFile);
                string uploadFolder = UploadUtilities.GetUploadFolder(batchSettings.AzureShareName, batchSettings.ScanDate);
                List<string> LlabRecs = WebApi.GetLabReqs(WebApi.EdocsWebApi, batchSettings.AzureWebApiController, batchSettings.ScanDate.ToString(), batchSettings.AzureTableName).Result;

                foreach (AzureCloudBatchRecordsModel batchRecord in azureCloudBatches)
                {
                    string fName = Path.GetFileNameWithoutExtension(batchRecord.FileName);
                    totRecodsRead++;
                    if (LlabRecs.Count() > 0)
                    {

                        if (LlabRecs.Contains(fName, StringComparer.OrdinalIgnoreCase))
                        {
                            totdup++;
                            continue;
                        }
                    }
                    // Edocs_Utilities.EdocsUtilitiesInstance.CHeckFileExists($"{batchDir}{batchRecord.FileName}", false);
                    batchRecord.DateUpload = DateTime.Now.AddHours(UploadUtilities.DateTimeMins); ;
                    batchRecord.FileExtension = Path.GetExtension(batchRecord.FileName);
                    batchRecord.ScanBatch = batchSettings.ScanBatch;
                    batchRecord.ScanDate = batchSettings.ScanDate.AddHours(UploadUtilities.DateTimeMins);
                    batchRecord.ScanMachine = batchSettings.ReceiptStation;
                    batchRecord.ScanOperator = batchSettings.ScanOperator;
                    if (WebApi.UpLoadAzureCloud)
                        batchRecord.FileUrl = UploadUtilities.SavePdfFileAzureCloud(batchDir, uploadFolder, batchRecord.FileName).GetAwaiter().GetResult().ToString();
                    else
                        batchRecord.FileUrl = UploadUtilities.SavePdfLocalFile(batchDir, batchId, uploadFolder, batchRecord.FileName).ConfigureAwait(false).GetAwaiter().GetResult().ToString();
                    batchRecord.FileName = fName;
                    WebApi.UploadAzureBatches(WebApi.EdocsWebApi, batchRecord, batchSettings.AzureWebApiController, batchSettings.AzureSPName, batchSettings.AzureTableName).Wait();


                }
                message = $"No errors uploading {archiver} to azure cloud batchid:{batchId} running on scanning machine:{Environment.MachineName} for archiver {archiver} total time {executionTimer.Elapsed.ToString()}";
                message = $"{message} Total records read {totRecodsRead} total dup {totdup} in total records json file  {totRecordsInFile} total Records upload {totRecodsRead - totdup}";
                UploadUtilities.SEmail(message, false);
                UploadUtilities.CLeanUp(batchDir, archiver, batchId);

            }
            catch (Exception ex)
            {
                edl.TraceLogger.TraceLoggerInstance.TraceError(ex.Message);
                message = $"Error uploading to azure cloud batchid:{batchId} running on scanning machine:{Environment.MachineName} error message:{ex.Message} for archiver {archiver} total time {executionTimer.Elapsed.ToString()}";

                UploadUtilities.SEmail(message, true);
               // UploadUtilities.NotifyUser(batchId, ex.Message);
            }

            edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Total records read {totRecodsRead}");
            edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Total Records upload {totRecodsRead - totdup}");
            edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Total Records dup {totdup}");
            edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Total Records in json file {totRecordsInFile}");
            edl.TraceLogger.TraceLoggerInstance.TraceInformation($"End time upload lab recs {DateTime.Now.ToString()} for archiver {archiver}");
            executionTimer.Stop();
            edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Total Time to upload lab recs {executionTimer.Elapsed.ToString()} for archiver {archiver}");
        }
    }
}
