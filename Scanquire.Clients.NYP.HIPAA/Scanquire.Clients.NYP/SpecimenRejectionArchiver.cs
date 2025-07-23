using Scanquire.Public;
using Scanquire.Public.ArchivesConstants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using EdocsUSA.Utilities.Extensions;
using System.Diagnostics;
using System.IO;
using EDL = EdocsUSA.Utilities.Logging;

namespace Scanquire.Clients.NYP
{
    public class SpecimenRejectionArchiver : AzureCloudArchiver
    {
        public string CaseNumberRequiresDash
        { get; set; }
        public string CaseNumberMinLength
        { get; set; }
        public string RejectionReasons
        { get; set; }
        public string CaseNumberSplitMinLength
        { get; set; }
        public string RejectionReasonsFileDir
        { get; set; }
        SpecimenRejectionRecordDialog RecordDialog = new SpecimenRejectionRecordDialog();

        public override IEnumerable<Task<SQImage>> AcquireFromScannerForNew(IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken)
        {
            //RecordDialog.Clear();
            //RecordDialog.RejectionResons = RejectionReasons;
            //RecordDialog.TryShowDialog(DialogResult.OK);
            return base.AcquireFromScannerForNew(progress, cToken);
        }

        public override async Task Send(IEnumerable<SQImage> images, IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken)
        {
            if (images.Count() == 0)
            {
                MessageBox.Show("No images/documents to archive", "Error No Documents", MessageBoxButtons.OK, MessageBoxIcon.Error);
                EdocsUSA.Utilities.Logging.TraceLogger.TraceLoggerInstance.TraceError("No Documents imges to archive");
                throw new OperationCanceledException("No images/documents to archive");
            }
            
            CurrentBatchId = GenerateNewBatchId();
            RecordDialog.RejectionResons = GetRejectionReasons(RejectionReasons);
            RecordDialog.RejectionReasonsFileDir = RejectionReasonsFileDir;
            RecordDialog.CaseNumberMinLength = 6;
            RecordDialog.CaseNumberSplitMinLength = 3;
            RecordDialog.CaseNumberRequiresDash = true;
            if (int.TryParse(CaseNumberMinLength, out int results))
                RecordDialog.CaseNumberMinLength = results;
            if (int.TryParse(CaseNumberSplitMinLength, out int resultsCaseSplit))
                RecordDialog.CaseNumberSplitMinLength = resultsCaseSplit;
            if (bool.TryParse(CaseNumberRequiresDash, out bool resultsCN))
                RecordDialog.CaseNumberRequiresDash = resultsCN;
            RecordDialog.TryShowDialog(DialogResult.OK);
            DateTime batchTime = DateTime.Now;
          EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Starting Specimen Rejection Archiver for batchid:{CurrentBatchId} batchtime:{batchTime.ToString()}");
            if (base.EnableAzureBatch)
            {
                Dictionary<string, object> sharepointBatchSettings = JsonSettings.GetJsonBatchSettings(CurrentBatchId, batchTime.ToUniversalTime().ToString(SHAREPOINT_DATETIME_FORMAT), base.ScanStationMachineName, string.Empty, base.AzureDataBaseName, base.AzureShareName, base.AzureTableName, base.AzureSPName, base.AzureWebApiController);
                SharepointBatchHelper.WriteSettings(CurrentSharepointBatchDir, sharepointBatchSettings);
                //Dictionary<string, string> sharepointCommonFields = new Dictionary<string, string>();
                //sharepointCommonFields[JsonsFieldConstants.JsonFieldScanBatch] = base.CurrentBatchId;
                //sharepointCommonFields[JsonsFieldConstants.JsonFieldScanStation] = base.ScanStationMachineName;
                //sharepointCommonFields[JsonsFieldConstants.JsonFieldScanDate] = batchTime.ToUniversalTime().ToString(SHAREPOINT_DATETIME_FORMAT);
                //   SharepointBatchSettings sharepointBatchSettings = new SharepointBatchSettings(base.AzureShareName, base.AzureDataBaseName, null, base.SharepointUserName, base.UnprotectedSharepointPassword, sharepointCommonFields);
                //SharepointBatchHelper.WriteSettings(CurrentSharepointBatchDir, sharepointBatchSettings);
            }

            await base.Send(images, progress, cToken);
        }

        public override async Task Send(SQFile sharepointFile, SQFile optixFile, int fileNumber, IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken)
        {
            
            DateTime ScanDate = RecordDialog.ScanDate.Value.Date;
            //Get the new additional log settings
            string scanDate = ScanDate.ToString("MM-dd-yyyy");
            string caseNumber = RecordDialog.CaseNumber.ToUpper();
            string form_reason = RecordDialog.LogReason;


            string fileName = Guid.NewGuid().ToString();
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Sending Specimen Rejection Archiver file for batchid:{CurrentBatchId} ScanDate:{ScanDate.ToString()} Case Number:{caseNumber} form_reason :{form_reason}");

            if (EnableAzureBatch)
            {
                EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Creating azure batch for Specimen Rejection Archiver file for batchid:{CurrentBatchId} ScanDate:{ScanDate.ToString()} Case Number:{caseNumber} form_reason :{form_reason}");
                Dictionary<string, string> sharepointRecordFields = new Dictionary<string, string>();
                sharepointRecordFields[JsonsFieldConstants.JsonFieldLogDate] = scanDate;//LogDate.ToUniversalTime().ToString(SHAREPOINT_DATETIME_FORMAT);
                //add the record fields here:
                sharepointRecordFields[JsonsFieldConstants.JsonFieldFormNumber] = caseNumber;
                sharepointRecordFields[JsonsFieldConstants.JsonFieldFormReason] = form_reason;
                string sharepointRecordFileName = Path.ChangeExtension(fileName, sharepointFile.FileExtension);
                SharepointBatchRecord sharepointBatchRecord = new SharepointBatchRecord(sharepointRecordFileName, sharepointRecordFields);
                await Task.Factory.StartNew(() =>
                { SharepointBatchHelper.WriteRecord(CurrentSharepointBatchDir, sharepointBatchRecord, sharepointFile.Data); });
            }

          
        }
        private string GetRejectionReasons(string rejReasons)
        {
            string rejFolder = RejectionReasonsFileDir.Replace("{UserFolder}", $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}");

            if(System.IO.File.Exists(rejFolder))
            {
                string txtRejRes = System.IO.File.ReadAllText(rejFolder);
                if (!(string.IsNullOrWhiteSpace(rejFolder)))
                    rejReasons = $"{rejReasons},{txtRejRes}";
            }
                return rejReasons;
        }
    }
}
