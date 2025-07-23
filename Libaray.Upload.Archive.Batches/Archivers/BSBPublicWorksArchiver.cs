using Edocs.Libaray.Upload.Archive.Batches.Models;
using EdocsUSA.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
namespace Edocs.Libaray.Upload.Archive.Batches.Archivers
{
  public  class BSBPublicWorksArchiver
    {
        public string BatchId
        { get; set; }
        public string BatchDir
        { get; set; }
        public BSBPublicWorksArchiver(string batchID, string archiveFolder)
        {
            BatchId = batchID;
            if (!(archiveFolder.EndsWith(batchID)))
                BatchDir = UploadUtilities.CombPath(archiveFolder, batchID).ConfigureAwait(false).GetAwaiter().GetResult();
            else
                BatchDir = archiveFolder;

        }
        public async Task UploadBSBPWDImagesPDF(string archiverName,string archiverFolder)
        {

            int totRecodsRead = 0;
            int totRecordsInFile = 0;
            int totdup = 0;
            string message = string.Empty;

            Stopwatch executionTimer = Stopwatch.StartNew();
            string batchRecordsFile = UploadUtilities.CombPath(BatchDir, $"{BatchId}{UploadUtilities.JsonRecordsExten}").ConfigureAwait(false).GetAwaiter().GetResult();
            string batchSettingsFile = UploadUtilities.CombPath(BatchDir, $"{BatchId}{UploadUtilities.JsonBatchSettingExten}").ConfigureAwait(false).GetAwaiter().GetResult();
            Edocs_Utilities.EdocsUtilitiesInstance.CopyFile(batchRecordsFile, UploadUtilities.JsonFilesBackUpFolder, true, string.Empty, false);
            Edocs_Utilities.EdocsUtilitiesInstance.CopyFile(batchSettingsFile, UploadUtilities.JsonFilesBackUpFolder, true, string.Empty, false);
            BSBPWDSettingsModel settings = UploadUtilities.GetBatchSettingsObject<BSBPWDSettingsModel>(batchSettingsFile);
            List<object> record = UploadUtilities.GetRecords<BSBPWDRecords>(ref totRecodsRead, ref totRecordsInFile, batchRecordsFile);
            totRecodsRead = 0;
            foreach (BSBPWDRecords recs in record)
            {
                BSBPWDRecords bSBPWD = new BSBPWDRecords();
                bSBPWD.FileName = recs.FileName;
                bSBPWD.FileUrl = settings.UploadFolder;
                bSBPWD.ProjectDepartment = recs.ProjectDepartment;
                bSBPWD.ProjectName = recs.ProjectName;
                bSBPWD.ProjectYear = recs.ProjectYear;
                bSBPWD.ScanOperator = settings.ScanOperator;
                WebApi.UploadBSBProdDepRec(settings.UploadApiUrl, settings.UpLoadController.Trim(), bSBPWD).ConfigureAwait(false).GetAwaiter().GetResult();
                UploadUtilities.SavePdfLocalFile(System.IO.Path.Combine(archiverFolder,recs.FileName), System.IO.Path.Combine(settings.UploadFolder, recs.FileName)).ConfigureAwait(false).GetAwaiter().GetResult();
              
                UploadTrackingByProjectNameModel uploadTracking = new UploadTrackingByProjectNameModel();
                uploadTracking.EdocsCustomerID = settings.EdocsCustomerID;
                uploadTracking.NumberDocOCR = settings.TotalScanned;
                uploadTracking.NumberDocsScanned = settings.TotalScanned;
                uploadTracking.NumberDocsUploaded = settings.TotalPageCount;
                uploadTracking.NumberTypedPerDoc = settings.TotalType;
                uploadTracking.TrackingID = System.IO.Path.GetFileNameWithoutExtension(recs.FileName);
                uploadTracking.UserName = Environment.UserName;
                uploadTracking.ScanBatchID = settings.ScanBatchID;
                uploadTracking.ScanMachine = Environment.MachineName;
                uploadTracking.ScanOperator = Environment.UserName;
                uploadTracking.StandardLargeDocument = false;
                uploadTracking.FileName = System.IO.Path.Combine(settings.UploadFolder,recs.FileName);
                settings.TrackinUpLoadController = "EdocsITSTrackingByProjectName";
                //EdocsITSTrackingIDModel trackingIDModel = new EdocsITSTrackingIDModel();
                //trackingIDModel.TrackingID = settings.ScanBatchID;
                //trackingIDModel.NumberDocsScanned = settings.TotalScanned;
                //trackingIDModel.NumberDocsUploaded = settings.TotalPageCount;
                WebApi.UploadMDTTrackingInformation(settings.InventoryTrackingApiUrl, settings.TrackinUpLoadController, uploadTracking).ConfigureAwait(false).GetAwaiter().GetResult();
                if (settings.OCRRecordsUpLoad)
                {
                    Console.WriteLine("Getting ocr informtion");
                    string ocrTxtFileName = System.IO.Path.Combine(archiverFolder, $"{System.IO.Path.GetFileNameWithoutExtension(recs.FileName)}_ocr.txt");
                    UploadUtilities.GetOcrText(System.IO.Path.Combine(settings.OCRImageFolder, $"{System.IO.Path.GetFileNameWithoutExtension(recs.FileName)}_err.txt"), settings.OCRImageFolder, "*.png", ocrTxtFileName).ConfigureAwait(false).GetAwaiter().GetResult();
                }
                //  WebApi.UploadMDTTrackingInformation(settings.InventoryTrackingApiUrl, "EdocsITSInventoryTransferByTrackID", trackingIDModel).ConfigureAwait(false).GetAwaiter().GetResult();

            }
        }
    }
}
