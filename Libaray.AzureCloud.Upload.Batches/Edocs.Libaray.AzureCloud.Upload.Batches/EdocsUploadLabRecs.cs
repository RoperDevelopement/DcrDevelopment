using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using FreeImageAPI;
using DebenuPDFLibraryDLL0915;
using System.Web.Script.Serialization;
using EdocsUSA.Utilities;
using EdocsUSA.Utilities.Extensions;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.IO;
using edl = EdocsUSA.Utilities.Logging;
namespace Edocs.Libaray.AzureCloud.Upload.Batches
{
    public sealed class EdocsUploadLabRecs
    {

        private string batchId;
        private string batchDir;
        private readonly string NoPatID = "GRACIE1000|ERROR|CANCEL|TSTLOG|PRBLOG|BMH0000|NYPHWCD";
        // private string LabRecFolder
        //  { get { return Edocs_Utilities.EdocsUtilitiesInstance.CheckDirectoryPath(Properties.Settings.Default.LabRecsUploadFolder); } }
        public EdocsUploadLabRecs(string batch, string archiver)
        {

            archiver = Edocs_Utilities.EdocsUtilitiesInstance.CheckDirectoryPath(archiver);
            batchDir = $"{Edocs_Utilities.EdocsUtilitiesInstance.CheckDirectoryPath(UploadUtilities.InputDir)}{archiver}{batch}\\";
            edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Getting lab records for batch id {batchId} from folder {batchDir}");
            edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Getting lab records for batch id {batchId} from folder {batchDir}");
            batchId = batch;
            UpLoadLabRecs(archiver);
        }
        //private async Task UpLoad(JsonFileLabRecsModel jsonTxt, string webapi)
        //{
        //    edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Uploading json file to web api{webapi}");
        //    edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Uploading json file to web api{webapi}");
        //   // await WebApi.UplaodLabReqs(UploadUtilities.EdocsWebApi, jsonTxt, webapi);
        //}
        private async Task UpLoad(JsonFileLabRecsModel jsonTxt, string webapi)
        {
            edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Uploading json file to web api{webapi}");
            edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Uploading json file to web api{webapi}");
            int loop = 0;
            while (loop++ <= UploadUtilities.UploadLoop)
            {
                try
                {

                    await WebApi.UplaodLabReqs(UploadUtilities.EdocsWebApi, jsonTxt, webapi);
                    loop = 100;
                    break;
                }
                catch (Exception ex)

                {
                    edl.TraceLogger.TraceLoggerInstance.TraceError($"Uploading json file to web api {webapi} number loop {loop.ToString()} {ex.Message}");
                    if (loop > UploadUtilities.UploadLoop)
                        throw new Exception(ex.Message);
                    System.Threading.Thread.Sleep(UploadUtilities.ThreadSleep);
                }
            }
        }
        private async Task<List<string>> GetLabRecs(string scanDate, string webapi, string tableName)
        {
            edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Getting lab records for scandate {scanDate} from web api{webapi}");
            edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Getting lab records for scandate {scanDate} from web api{webapi}");
            return (await WebApi.GetLabReqs(UploadUtilities.EdocsWebApi, webapi, scanDate, tableName));

        }

        private void UpLoadLabRecs(string archiver)
        {
            int totRecodsRead = 0;
            int totRecordsInFile = 0;
            int totdup = 0;
            string message = string.Empty;
            Stopwatch executionTimer = Stopwatch.StartNew();
            try
            {
                edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Start time upload lab recs {DateTime.Now.ToString()} for archiver {archiver}");


                string batchRecordsFile = UploadUtilities.GetBatchRecordsFileName(batchDir, batchId);
                string batchSettingsFile = UploadUtilities.GetBatchSettingsFileName(batchDir, batchId);
                UploadUtilities.BackupFiles(batchRecordsFile, batchSettingsFile).ConfigureAwait(false).GetAwaiter().GetResult();
                // Edocs_Utilities.EdocsUtilitiesInstance.CopyFile(batchRecordsFile, UploadUtilities.JsonFilesBackUpFolder, true, string.Empty,false);
                // Edocs_Utilities.EdocsUtilitiesInstance.CopyFile(batchSettingsFile, UploadUtilities.JsonFilesBackUpFolder, true, string.Empty,false);



                edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Getting records json file {batchRecordsFile}");
                edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Getting batch json setting file {batchSettingsFile}");
                //  int totalRecords = File.ReadAllLines(batchRecordsFile).Count();


                //   Dictionary<string, object> batchSettings = AppSettingsManager.GetBatchSettings(batchSettingsFile);

                JsonSettingsFileModel batchSettings = UploadUtilities.GetBatchSettingsObject<JsonSettingsFileModel>(batchSettingsFile);
                edl.TraceLogger.TraceLoggerInstance.UserName = batchSettings.ScanOperator;
                List<object> record = UploadUtilities.GetRecords<LabRecJasonFileSettings>(ref totRecodsRead, ref totRecordsInFile, batchRecordsFile);
                edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Total records in uplaod file {batchRecordsFile} {totRecordsInFile.ToString()}");
                edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Total records uplaod file {batchRecordsFile} {totRecordsInFile.ToString()}");
                edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Total records read uplaod file {batchRecordsFile} {totRecodsRead.ToString()}");
                edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Total records read uplaod file {batchRecordsFile} {totRecodsRead.ToString()}");
                if (totRecodsRead == 0)
                    throw new Exception($"No records found in file {batchRecordsFile}");

                string uploadFolder = UploadUtilities.GetUploadFolder(batchSettings.AzureShareName, batchSettings.ScanDate);

                totRecodsRead = 0;
                edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Upload images to folder {uploadFolder}");
                edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Upload images to folder {uploadFolder}");
                List<string> currentLabRecs = GetLabRecs(batchSettings.ScanDate.ToString(), batchSettings.AzureWebApiController, batchSettings.AzureTableName).Result;
                // JavaScriptSerializer Serializer = new JavaScriptSerializer();
                int totalRecsToUload = record.Count();
                edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Total Recs to upload {totalRecsToUload}");
                edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Total Recs to upload {totalRecsToUload}");

                StringBuilder sb = new StringBuilder();
                foreach (LabRecJasonFileSettings labRec in record)
                {
                    totRecodsRead++;

                    if (string.IsNullOrWhiteSpace(labRec.IndexNumber))
                        throw new Exception("Index number not found");
                    // Edocs_Utilities.EdocsUtilitiesInstance.CHeckFileExists($"{batchDir}{labRec.FileName}", false);
                    string fName = Path.GetFileNameWithoutExtension(labRec.FileName);
                    if (currentLabRecs.Count > 0)
                    {
                        if (currentLabRecs.Contains(fName, StringComparer.OrdinalIgnoreCase))
                        {
                            totdup++;
                            edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Found Dup Record {labRec.FileName}");
                            edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Upload images to folder {labRec.FileName}");
                            continue;
                        }

                    }
                    edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Uploading lab Record {labRec.FileName}");

                    if ((string.IsNullOrWhiteSpace(labRec.CsnNumber)))
                        labRec.CsnNumber = labRec.IndexNumber;
                    JsonFileLabRecsModel uploadJson = new JsonFileLabRecsModel();
                    //   uploadJson.FileUrl = UploadUtilities.SaveTffFileAzureCloud(batchDir, uploadFolder, labRec.FileName).GetAwaiter().GetResult().ToString();
                    uploadJson.Category = batchSettings.Category;
                    uploadJson.ClientCode = labRec.ClientCode;
                    uploadJson.CsnNumber = labRec.CsnNumber;
                    uploadJson.DateOfService = batchSettings.ReceiptDate.AddHours(UploadUtilities.DateTimeMins);
                    uploadJson.DateUpload = DateTime.Now.AddHours(-5);
                    uploadJson.FileExtension = ".pdf";
                    uploadJson.FileName = fName;
                    uploadJson.AzureStpredProcedureName = batchSettings.AzureSPName;
                    uploadJson.IndexNumber = labRec.IndexNumber;
                    uploadJson.PatientID = labRec.PatientID;
                    uploadJson.ReceiptDate = batchSettings.ReceiptDate.AddHours(UploadUtilities.DateTimeMins);
                    uploadJson.RequisitionNumber = labRec.RequisitionNumber;
                    uploadJson.ScanBatch = batchId;
                    uploadJson.ScanDate = batchSettings.ScanDate.AddHours(UploadUtilities.DateTimeMins);
                    uploadJson.ScanMachine = Environment.MachineName;
                    uploadJson.ScanOperator = batchSettings.ScanOperator;


                    if (!(string.IsNullOrWhiteSpace(labRec.IndexNumber)))
                    {
                        if (System.Text.RegularExpressions.Regex.IsMatch(labRec.IndexNumber, NoPatID, System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                        {
                            //uploadJson.PatientID = string.Empty;
                            edl.TraceLogger.TraceLoggerInstance.TraceWarning($"Invalid LabReq Number: {labRec.IndexNumber} 000000");
                            uploadJson.ClientCode = "000000";
                        }
                        else
                        {
                            if ((uploadJson.IndexNumber.StartsWith("00000410") || (uploadJson.IndexNumber.StartsWith("410"))))
                            {
                                edl.TraceLogger.TraceLoggerInstance.TraceWarning($"Found index number {uploadJson.IndexNumber} that starts with 410");
                                edl.TraceLogger.TraceLoggerInstance.TraceWarning($"Chaning PatientID value { uploadJson.PatientID} to null value");
                                edl.TraceLogger.TraceLoggerInstance.TraceWarning($"Chaning ClientCode value {uploadJson.ClientCode} 000000");
                                uploadJson.PatientID = string.Empty;
                                uploadJson.ClientCode = "000000";
                            }
                            else
                            {
                                if (uploadJson.IndexNumber.Length >= 15)
                                {
                                    //string zeros = string.Empty;
                                    int strStart = uploadJson.IndexNumber.Length - 10;
                                    //uploadJson.IndexNumber = uploadJson.IndexNumber.Remove(0, strStart);
                                    //string pad = zeros.PadLeft(strStart, '0');
                                    //uploadJson.IndexNumber = $"{pad}{uploadJson.IndexNumber}";
                                    if ((string.IsNullOrWhiteSpace(labRec.PatientID)) || (labRec.PatientID.Length < 10) || (labRec.PatientID.Length > 10))
                                    {

                                        if (strStart > 0)
                                        {
                                            uploadJson.PatientID = uploadJson.IndexNumber.Substring(strStart).TrimStart('0');
                                            if (uploadJson.PatientID.Length < 10)
                                                uploadJson.PatientID = uploadJson.IndexNumber.Substring(5).TrimStart('0');
                                            edl.TraceLogger.TraceLoggerInstance.TraceWarning($"Using Index Number {uploadJson.IndexNumber} to get patient id {uploadJson.PatientID}");
                                        }
                                        else
                                        {
                                            edl.TraceLogger.TraceLoggerInstance.TraceError($"Using Index Number {uploadJson.IndexNumber} get negtive number {strStart}");
                                        }


                                        //if ((uploadJson.IndexNumber.Length < 17))
                                        //{ 
                                        //    uploadJson.PatientID = uploadJson.IndexNumber.Substring(6).TrimStart('0');
                                        //    if(uploadJson.PatientID.Length < 10)
                                        //        uploadJson.PatientID = uploadJson.IndexNumber.Substring(5).TrimStart('0');
                                        //}
                                        //else
                                        //    uploadJson.PatientID = uploadJson.IndexNumber.Substring(7).TrimStart('0');
                                    }
                                    if (!(string.IsNullOrWhiteSpace(labRec.PatientID)) && (!(labRec.PatientID.StartsWith("2"))))
                                    {

                                        if (!(labRec.PatientID.StartsWith("1")) && !(labRec.PatientID.StartsWith("4")))
                                        //  if (!(Regex.IsMatch(uploadJson.PatientID,UploadUtilities.RegxMatchMRN,RegexOptions.IgnoreCase)))
                                        {
                                            edl.TraceLogger.TraceLoggerInstance.TraceError($"Using Index Number {uploadJson.IndexNumber} to get patient id {uploadJson.PatientID} and does not startwith  1 or 4");
                                            sb.Append($"Using Index Number {uploadJson.IndexNumber} to get patient id {uploadJson.PatientID} and does not startwith  1 or 4");
                                        }
                                    }

                                }
                            }
                        }
                    }


                    if (!(string.IsNullOrWhiteSpace(uploadJson.CsnNumber)))
                    {
                        if (uploadJson.CsnNumber.Contains("-"))
                            throw new Exception($"Invalid csn number {uploadJson.CsnNumber}");
                    }

                    if (WebApi.UpLoadAzureCloud)
                        uploadJson.FileUrl = UploadUtilities.SaveTffFileAzureCloud(batchDir, uploadFolder, labRec.FileName).ConfigureAwait(false).GetAwaiter().GetResult().ToString();

                    else
                        uploadJson.FileUrl = UploadUtilities.SaveTffFileLocal(batchDir, uploadFolder, labRec.FileName).ConfigureAwait(false).GetAwaiter().GetResult().ToString();
                    UpLoad(uploadJson, batchSettings.AzureWebApiController).Wait();
                    if (UploadUtilities.SaveOcrFile)
                        UploadUtilities.UploadTiffFileAzureCloud(Path.Combine(batchDir, labRec.FileName), UploadUtilities.OcrLabReqsContainer).ConfigureAwait(false).GetAwaiter().GetResult();
                    edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Total Recs upload  {totRecodsRead} Records left to up load {--totalRecsToUload}");
                    edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Total Recs upload  {totRecodsRead} Records left to up load {totalRecsToUload}");


                }
                if (sb.Length > 0)
                {
                    UploadUtilities.CLeanUp(batchDir, archiver, batchId);
                    message = sb.ToString();
                    throw new Exception(message);
                }

                message = $"No errors uploading {archiver} to azure cloud batchid:{batchId} running on scanning machine:{Environment.MachineName} for archiver {archiver} total time {executionTimer.Elapsed.ToString()}";
                message = $"{message} Total records read {totRecodsRead} total dup {totdup} in total records json file  {totRecordsInFile} total Records upload {totRecodsRead - totdup}";
                UploadUtilities.SEmail(message, false);
                UploadUtilities.CLeanUp(batchDir, archiver, batchId);
            }
            catch (Exception ex)
            {
                edl.TraceLogger.TraceLoggerInstance.TraceError($"Processing batch id {batchId} {ex.Message}");
                edl.TraceLogger.TraceLoggerInstance.TraceErrorConsole($"Processing batch id {batchId} {ex.Message}");
                message = $"Error uploading to azure cloud batchid:{batchId} running on scanning machine:{Environment.MachineName} error message:{ex.Message} for archiver {archiver} total time {executionTimer.Elapsed.ToString()}";
                UploadUtilities.SEmail(message, true);
                // UploadUtilities.NotifyUser(batchId, ex.Message);

            }
            edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Total records read {totRecodsRead}");
            edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Total Records upload {totRecodsRead - totdup}");
            edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Total Records dup {totdup}");
            edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Total Records in file {totRecordsInFile}");
            edl.TraceLogger.TraceLoggerInstance.TraceInformation($"End time upload lab recs {DateTime.Now.ToString()}");
            edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"End time upload lab recs {DateTime.Now.ToString()} for archiver {archiver}");
            executionTimer.Stop();
            edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Total Time to upload lab recs {executionTimer.Elapsed.ToString()} for archiver {archiver}");

        }
    }
}

