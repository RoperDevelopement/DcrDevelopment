using Edocs.Libaray.Upload.Archive.Batches.Interfaces;
using Edocs.Libaray.Upload.Archive.Batches.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using edl = EdocsUSA.Utilities.Logging;
using EdocsUSA.Utilities;
using System.Windows.Forms;

namespace Edocs.Libaray.Upload.Archive.Batches.Archivers
{
    public class PSUSDArchiver
    {
        public PSUSDArchiver(string batchID, string archiveFolder)
        {
            UploadUtilities.BatchId = batchID;
            //  if (!(archiveFolder.EndsWith(batchID)))
            //    UploadUtilities.BatchDir = UploadUtilities.CombPath(archiveFolder, batchID).ConfigureAwait(false).GetAwaiter().GetResult();
            //  else
            UploadUtilities.BatchDir = Edocs_Utilities.EdocsUtilitiesInstance.CheckDirectoryPath(archiveFolder);

        }
        public PSUSDArchiver()
        {

        }
        public async Task UploadPSUSDImagesPDF(string archiverName, string archiveFolder)
        {
            int totRecodsRead = 0;
            int totRecordsInFile = 0;
            int totdup = 0;
            string message = string.Empty;

            Stopwatch executionTimer = Stopwatch.StartNew();


            string batchRecordsFile = $"{UploadUtilities.BatchDir}{UploadUtilities.BatchId}{UploadUtilities.JsonRecordsExten}";//UploadUtilities.CombPath(UploadUtilities.BatchDir, $"{UploadUtilities.BatchId}{UploadUtilities.JsonRecordsExten}").ConfigureAwait(false).GetAwaiter().GetResult();
            string batchSettingsFile = $"{UploadUtilities.BatchDir}{UploadUtilities.BatchId}{UploadUtilities.JsonBatchSettingExten}";//UploadUtilities.CombPath(UploadUtilities.BatchDir, $"{UploadUtilities.BatchId}{UploadUtilities.JsonBatchSettingExten}").ConfigureAwait(false).GetAwaiter().GetResult();
            Edocs_Utilities.EdocsUtilitiesInstance.CreateDir(UploadUtilities.JsonFilesBackUpFolder);
            Edocs_Utilities.EdocsUtilitiesInstance.CopyFile(batchRecordsFile, UploadUtilities.JsonFilesBackUpFolder, true, string.Empty, false, true);
            Edocs_Utilities.EdocsUtilitiesInstance.CopyFile(batchSettingsFile, UploadUtilities.JsonFilesBackUpFolder, true, string.Empty, false, true);
            PSUSDSettingsModel settings = UploadUtilities.GetBatchSettingsObject<PSUSDSettingsModel>(batchSettingsFile);

            string imagesFolder = settings.OCRImageFolder;
            string ocrImageSaveFolder = $"{UploadUtilities.BatchDir}{UploadUtilities.BatchId}_ocr.txt";// System.IO.Path.Combine(UploadUtilities.BatchDir, rec.TrackID);
            string errMess = string.Empty;
            //  string ocrImageSaveFolder = System.IO.Path.Combine()
            //  OCRImages(errorFileName, settings.OCRImageFolder, "*.png",  ocrImageSaveFolder)
            List<object> record = UploadUtilities.GetRecords<PSUSDRecordsModel>(ref totRecodsRead, ref totRecordsInFile, batchRecordsFile);
            totRecodsRead = 0;
            EdocsITSTrackingIDModel trackingIDModel = new EdocsITSTrackingIDModel();
            try
            {
                foreach (PSUSDRecordsModel rec in record)
                {
                     if(settings.OCRRecords)
                        { 
                    if (totRecodsRead == 0)
                    {
                      
                        string errorFileName = $@"D:\Archives\{archiverName}_OCRError\{settings.EdocsCustomerID}\{System.IO.Path.GetFileNameWithoutExtension(rec.PDFFileName)}_error.txt";
                       UploadUtilities.OCRImages(errorFileName, settings.OCRImageFolder, "*.png", ocrImageSaveFolder).ConfigureAwait(true).GetAwaiter().GetResult();
                        string backupFolder = $@"D:\Archives\{archiverName}_PDFBack\{settings.EdocsCustomerID}\{System.IO.Path.GetFileNameWithoutExtension(rec.PDFFileName)}";
                         UploadUtilities.SearchablePDF(errorFileName, archiveFolder, "*.pdf", archiveFolder, backupFolder).ConfigureAwait(true).GetAwaiter().GetResult();
                    }
                    }
                    UploadTrackingByProjectNameModel uploadTracking = new UploadTrackingByProjectNameModel();
                    uploadTracking.EdocsCustomerID = settings.EdocsCustomerID;
                    uploadTracking.NumberDocOCR = settings.NumberDocOCR;
                    uploadTracking.NumberDocsScanned = settings.NumberDocsScanned;
                    uploadTracking.NumberDocsUploaded = settings.NumberDocsUploaded;
                    uploadTracking.NumberTypedPerDoc = settings.NumberTypedPerDoc;
                    uploadTracking.TrackingID = System.IO.Path.GetFileNameWithoutExtension(rec.PDFFileName);
                    uploadTracking.UserName = Environment.UserName;
                    uploadTracking.ScanBatchID = settings.ScanBatchID;
                    uploadTracking.ScanMachine = Environment.MachineName;
                    uploadTracking.ScanOperator = Environment.UserName;
                    uploadTracking.StandardLargeDocument = false;
                    uploadTracking.FileName = Edocs_Utilities.EdocsUtilitiesInstance.CheckDirectoryPath(settings.UploadFolder);
                    settings.TrackinUpLoadController = "EdocsITSTrackingByProjectName";
                    trackingIDModel.TrackingID = settings.ScanBatchID;
                    trackingIDModel.NumberDocsScanned = settings.NumberDocsScanned;
                    trackingIDModel.NumberDocsUploaded = settings.NumberDocsUploaded;
                    if((settings.NumberDocsUploaded) != (settings.NumberImagesSaved))
                    {
                        errMess = $"Number images scanned {trackingIDModel.NumberDocsScanned} not equal to number images saved {uploadTracking.NumberImagesSaved} for upload file name {uploadTracking.FileName} for archiver {archiverName} fpr archive folder {archiveFolder}";
                    }
                    uploadTracking.FileName = $@"{uploadTracking.FileName}{rec.Department}\{settings.ScanBatchID}\{rec.DateOfRecords}\{rec.DescriptionOfRecords}\{rec.MethOfFiling}\{rec.PDFFileName}";
                    WebApi.UploadMDTTrackingInformation(settings.InventoryTrackingApiUrl, settings.TrackinUpLoadController, uploadTracking).ConfigureAwait(false).GetAwaiter().GetResult();
                    WebApi.UploadMDTTrackingInformation(settings.InventoryTrackingApiUrl, settings.TransferByTrackIDController, trackingIDModel).ConfigureAwait(false).GetAwaiter().GetResult();
                    EdocsUSA.Utilities.Edocs_Utilities.EdocsUtilitiesInstance.CreateDir(System.IO.Path.GetDirectoryName(uploadTracking.FileName));
                    Edocs_Utilities.EdocsUtilitiesInstance.CopyFile($"{UploadUtilities.BatchDir}{rec.PDFFileName}", System.IO.Path.GetDirectoryName(uploadTracking.FileName), true, string.Empty, false, false);
                    Edocs_Utilities.EdocsUtilitiesInstance.CopyFile($"{UploadUtilities.BatchDir}{System.IO.Path.GetFileName(batchRecordsFile)}", System.IO.Path.GetDirectoryName(uploadTracking.FileName), true, string.Empty, false, false);
                    UploadUtilities.UploadPSSDRecords(rec, settings.EdocsCustomerID, uploadTracking.TrackingID).ConfigureAwait(false).GetAwaiter().GetResult();
                     UploadUtilities.UploadPSSDFullText(uploadTracking.TrackingID,ocrImageSaveFolder,rec,settings.ScanBatchID).ConfigureAwait(false).GetAwaiter().GetResult();

                    totRecodsRead++;
                }
            }
            catch (Exception ex)
            {
                edl.TraceLogger.TraceLoggerInstance.TraceError($"Processing batch id {archiverName} for folder {UploadUtilities.BatchDir}{UploadUtilities.BatchId} {ex.Message}");
                edl.TraceLogger.TraceLoggerInstance.TraceErrorConsole($"Processing batch id {archiverName} for folder {UploadUtilities.BatchDir}{UploadUtilities.BatchId} {ex.Message}");
                message = $"Error uploading batchid:{archiverName} for folder {UploadUtilities.BatchDir}{UploadUtilities.BatchId} running on scanning machine:{Environment.MachineName} error message:{ex.Message}   total time {executionTimer.Elapsed.ToString()}";
                UploadUtilities.SEmail(message, true);
                UploadUtilities.NotifyUser(archiverName, ex.Message);
                UploadUtilities.ExitCode = -1;
            }
            if(!(string.IsNullOrWhiteSpace(errMess)))
            {
                edl.TraceLogger.TraceLoggerInstance.TraceError(errMess);
                UploadUtilities.SEmail(errMess, true);
                UploadUtilities.NotifyUser(archiverName, errMess);
            }
                
        }
    }
}
