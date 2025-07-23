using Edocs.Libaray.Upload.Archive.Batches.Models;
using EdocsUSA.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using edl = EdocsUSA.Utilities.Logging;


namespace Edocs.Libaray.Upload.Archive.Batches.Archivers
{
   public class BSBPropDepArchiver
    {
        public string BatchId
        { get; set; }
        public string BatchDir
        { get; set; }

        public BSBPropDepArchiver(string batchID, string archiveFolder)
        {
            BatchId = batchID;
            if (!(archiveFolder.EndsWith(batchID)))
                BatchDir = UploadUtilities.CombPath(archiveFolder, batchID).ConfigureAwait(false).GetAwaiter().GetResult();
            else
                BatchDir = archiveFolder;
           
        }
        public BSBPropDepArchiver()
        {

        }

        public async Task UploadBSBImagesPDF(string archiverName)
        {
            int totRecodsRead = 0;
            int totRecordsInFile = 0;
            int totdup = 0;
            string message = string.Empty;

            Stopwatch executionTimer = Stopwatch.StartNew();
            try
            {




                string batchRecordsFile = UploadUtilities.CombPath(BatchDir, $"{BatchId}{UploadUtilities.JsonRecordsExten}").ConfigureAwait(false).GetAwaiter().GetResult();
                string batchSettingsFile = UploadUtilities.CombPath(BatchDir, $"{BatchId}{UploadUtilities.JsonBatchSettingExten}").ConfigureAwait(false).GetAwaiter().GetResult();
                Edocs_Utilities.EdocsUtilitiesInstance.CopyFile(batchRecordsFile, UploadUtilities.JsonFilesBackUpFolder, true, string.Empty, false);
                Edocs_Utilities.EdocsUtilitiesInstance.CopyFile(batchSettingsFile, UploadUtilities.JsonFilesBackUpFolder, true, string.Empty, false);
                BSBProdDepSettingsModel settings = UploadUtilities.GetBatchSettingsObject<BSBProdDepSettingsModel>(batchSettingsFile);
                List<object> record = UploadUtilities.GetRecords<BSBPropDepRecordsModel>(ref totRecodsRead, ref totRecordsInFile, batchRecordsFile);
                totRecodsRead = 0;
                foreach (BSBPropDepRecordsModel recs in record)
                {


                    recs.ScanBatchID = settings.ScanBatchID;
                    recs.ScanOperator = settings.ScanOperator;
                    recs.FileUrl = settings.UploadFolder;

                    string sFile = System.IO.Path.Combine(BatchDir, recs.FileName);
                    //if (settings.UploadFolder.StartsWith("m:"))
                    //    settings.UploadFolder = settings.UploadFolder.Replace("m:", "d:");
                    // string dFile = System.IO.Path.Combine(settings.UploadFolder, recs.FileName);
                    //  string[] dFile = CheckUploadFileName(sFile,System.IO.Path.Combine(settings.UploadFolder,$"{recs.DocumentType} {recs.ProjectName} {recs.ProjectNumber}.pdf")).ConfigureAwait(false).GetAwaiter().GetResult();

                    // UpLoadMDTrackingModel upLoadMDTracking = new UpLoadMDTrackingModel();
                    //   upLoadMDTracking.FileName = System.IO.Path.Combine(settings.UploadFolder, $"{recs.ProjectName} {recs.ProjectNumber}\\{recs.DocumentType}_//{recs.ProjectName} {recs.ProjectNumber}.pdf");
                    //    bool overWriteFile = UploadUtilities.SavePdfLocalFile(sFile, upLoadMDTracking.FileName).ConfigureAwait(false).GetAwaiter().GetResult();
                    // upLoadMDTracking.EdocsCustomerID = settings.EdocsCustomerID;
                    //// upLoadMDTracking.InventoryTrackingSP = settings.InventoryTrackingSP;
                    // upLoadMDTracking.ScanBatchID = settings.ScanBatchID;
                    //// upLoadMDTracking.ScanMachine = settings.ScanMachine;
                    // upLoadMDTracking.ScanOperator = settings.ScanOperator;
                    // upLoadMDTracking.TotalPageCount = recs.TotalPageCount;
                    // upLoadMDTracking.TotalScanned = recs.TotalScanned;
                    // upLoadMDTracking.FileName = dFile[1];
                    //  upLoadMDTracking.TotalType = recs.DocumentType.Trim().Length + recs.ProjectName.Trim().Length + recs.ProjectNumber.Trim().Length;
                    //if (string.Compare(settings.InventoryTrackingID, settings.InventoryTrackingSP, true) == 0)
                    //  settings.InventoryTrackingID = $"{recs.BoxNumber} {recs.DocumentType} {recs.ProjectName} {recs.ProjectNumber}";
                    //else

                    // upLoadMDTracking.InventoryTrackingID = settings.InventoryTrackingID;
                    //if (!(string.IsNullOrWhiteSpace(UploadUtilities.EdocsWebApi)))
                    //    settings.InventoryTrackingApiUrl = UploadUtilities.EdocsWebApi;
                    //if (upLoadMDTracking.EdocsCustomerID == 0)
                    //    upLoadMDTracking.EdocsCustomerID = 1001;
                    //if (overWriteFile)
                    //    upLoadMDTracking.OverWriteFile = true;
                    // upLoadMDTracking.InventoryTrackingID = $"New File_{recs.DocumentType}_{recs.ProjectName} {recs.ProjectNumber}";

                    //  upLoadMDTracking.InventoryTrackingID = $"{recs.DocumentType}_{recs.ProjectName} {recs.ProjectNumber}";
                    if (((recs.FileName.ToLower().EndsWith(".pdf"))))
                        recs.FileName = $"{recs.FileName}.pdf";
                    if(string.Compare(settings.UploadApiUrl,"NA",true) !=0)
                    WebApi.UploadBSBProdDepRec(settings.UploadApiUrl,settings.UpLoadController,recs).ConfigureAwait(false).GetAwaiter().GetResult();
                    UploadUtilities.SavePdfLocalFile(sFile, System.IO.Path.Combine(settings.UploadFolder,recs.FileName)).ConfigureAwait(false).GetAwaiter().GetResult();
                    //UpLoadMDTrackingModel upLoadMDTracking = new UpLoadMDTrackingModel();
                    //upLoadMDTracking.EdocsCustomerID = settings.EdocsCustomerID;
                    //upLoadMDTracking.InventoryTrackingSP = settings.InventoryTrackingSP.ToString();
                    //upLoadMDTracking.ScanBatchID = settings.ScanBatchID;
                    //upLoadMDTracking.ScanMachine = Environment.MachineName;
                    //upLoadMDTracking.ScanOperator = settings.ScanOperator;
                    //upLoadMDTracking.TotalPageCount = recs.TotalPageCount;
                    //upLoadMDTracking.TotalScanned = recs.TotalScanned;
                    //upLoadMDTracking.FileName = System.IO.Path.Combine(settings.UploadFolder, recs.FileName);
                    //upLoadMDTracking.InventoryTrackingID = recs.PermitNumber.ToString();
                    //// upLoadMDTracking.FileName = dFile[1];
                    //upLoadMDTracking.TotalType = recs.TotalType;
                    //upLoadMDTracking.TotalOCR = recs.TotalOCR;
                    //if (string.Compare(settings.InventoryTrackingID, settings.InventoryTrackingSP, true) == 0)
                    //settings.InventoryTrackingID = $"{recs.PermitNumber.ToString()}";a
                    BSPProdDeptUploadSearchTxt deptUploadSearchTxt = new BSPProdDeptUploadSearchTxt();
                     string append = $"{recs.PermitNumber} {recs.Address} {recs.ConstCo} {recs.DateExpired} {recs.DateIssued} {recs.ExePermitNumber} {recs.GoCode} {recs.ParcelNumber} {recs.ZoneNumber} ";
                    deptUploadSearchTxt.SearchStr = UploadUtilities.GetSearchableTxt(recs.LCROCTxtFile, string.Empty).ConfigureAwait(false).GetAwaiter().GetResult();
                  if(!(string.IsNullOrWhiteSpace(deptUploadSearchTxt.SearchStr)))
                    {
                         
                          deptUploadSearchTxt.PermitNumber = recs.PermitNumber;
                         WebApi.UploadBSBProdSearchTxr(settings.UploadApiUrl, "BSBPlanDepSearchText",deptUploadSearchTxt).ConfigureAwait(false).GetAwaiter().GetResult();
                    }
                  //  WebApi.UploadMDTTrackingInformation(settings.InventoryTrackingApiUrl, "EdocsITSTrackingByProjectName", upLoadMDTracking).ConfigureAwait(false).GetAwaiter().GetResult();
                }
                message = $"No errors uploading {BatchDir} for MDT batchid:{BatchId} running on scanning machine:{Environment.MachineName}  {executionTimer.Elapsed.ToString()}";
                message = $"{message} Total records read {totRecodsRead} total dup {totdup} in total records json file  {totRecordsInFile} total Records upload {totRecodsRead - totdup}";
                UploadUtilities.SEmail(message, false);
                UploadUtilities.CLeanUp(BatchDir, archiverName, BatchId);

            }
            catch (Exception ex)
            {
                edl.TraceLogger.TraceLoggerInstance.TraceError($"Processing batch id {BatchId} for folder {BatchDir} {ex.Message}");
                edl.TraceLogger.TraceLoggerInstance.TraceErrorConsole($"Processing batch id {BatchId} for folder {BatchDir} {ex.Message}");
                message = $"Error uploading batchid:{BatchId} for folder {BatchDir} running on scanning machine:{Environment.MachineName} error message:{ex.Message}   total time {executionTimer.Elapsed.ToString()}";
                UploadUtilities.SEmail(message, true);
                UploadUtilities.NotifyUser(BatchId, ex.Message);
                UploadUtilities.ExitCode = -1;
            }
            edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Total records read {totRecodsRead}");
            edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Total Records upload {totRecodsRead - totdup}");
            edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Total Records dup {totdup}");
            edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Total Records in file {totRecordsInFile}");
            edl.TraceLogger.TraceLoggerInstance.TraceInformation($"End time upload lab recs {DateTime.Now.ToString()}");
            edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"End time upload MDT Records {DateTime.Now.ToString()} for batchid {BatchId} for folder {BatchDir}");
            executionTimer.Stop();
            edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Total Time to upload lab recs {executionTimer.Elapsed.ToString()} for batchid {BatchId} for folder {BatchDir}");
            //UploadUtilities.CLeanUp(batchDir, archiver, batchId);
        }
    }
}
