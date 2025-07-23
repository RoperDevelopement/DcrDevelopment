using Scanquire.Public;
using Scanquire.Public.ArchivesConstants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EdocsUSA.Utilities.Extensions;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using EDL = EdocsUSA.Utilities.Logging;
namespace Scanquire.Clients.NYP
{

    class SendoutPackingSlipsArchiver : AzureCloudArchiver
    {

        SendoutPackingSlipsRecordDialog recordDialog = new SendoutPackingSlipsRecordDialog();

        public override IEnumerable<Task<SQImage>> AcquireFromScannerForNew(IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken)
        {
            recordDialog.Clear();
            recordDialog.TryShowDialog(DialogResult.OK);
            return base.AcquireFromScannerForNew(progress, cToken);
        }

        public override async Task Send(IEnumerable<SQImage> images, IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken)
        {
            CurrentBatchId = GenerateNewBatchId();
            recordDialog.TryShowDialog(DialogResult.OK);
            DateTime batchTime = DateTime.Now;
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Starting Sendout Packing Slips Archiver for batchid:{CurrentBatchId} for batchtime:{batchTime.ToString()}");
            if (base.EnableAzureBatch)
            {
                //Dictionary<string, string> sharepointCommonFields = new Dictionary<string, string>();
                Dictionary<string, object> sharepointBatchSettings = JsonSettings.GetJsonBatchSettings(CurrentBatchId, batchTime.ToUniversalTime().ToString(SHAREPOINT_DATETIME_FORMAT), base.ScanStationMachineName, string.Empty, base.AzureDataBaseName, base.AzureShareName, base.AzureTableName, base.AzureSPName, base.AzureWebApiController);
                //sharepointCommonFields[JsonsFieldConstants.JsonFieldScanBatch] = base.CurrentBatchId;
                //sharepointCommonFields[JsonsFieldConstants.JsonFieldScanStation] = base.ScanStationMachineName;
                //sharepointCommonFields[JsonsFieldConstants.JsonFieldScanDate] = batchTime.ToUniversalTime().ToString(SHAREPOINT_DATETIME_FORMAT);
                //SharepointBatchSettings sharepointBatchSettings = new SharepointBatchSettings(base.AzureShareName, base.AzureDataBaseName, null, base.SharepointUserName, base.UnprotectedSharepointPassword, sharepointCommonFields);
                //SharepointBatchHelper.WriteSettings(CurrentSharepointBatchDir, sharepointBatchSettings);
                SharepointBatchHelper.WriteSettings(CurrentSharepointBatchDir, sharepointBatchSettings);
            }
            


            await base.Send(images, progress, cToken);
        }

        public override async Task Send(SQFile sharepointFile, SQFile optixFile, int fileNumber, IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken)
        {
            DateTime date = recordDialog.Date.Value;
            
            string fileName = Guid.NewGuid().ToString();

            if (EnableAzureBatch)
            {
                EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Creating azure batch for Packing Slips Archiver for batchid:{CurrentBatchId} for record date:{date.ToString()}");
                Dictionary<string, string> sharepointRecordFields = new Dictionary<string, string>();
                sharepointRecordFields[JsonsFieldConstants.JsonFieldDateOfService] = date.ToUniversalTime().ToString(SHAREPOINT_DATETIME_FORMAT);

                string sharepointRecordFileName = Path.ChangeExtension(fileName, sharepointFile.FileExtension);
                SharepointBatchRecord sharepointBatchRecord = new SharepointBatchRecord(sharepointRecordFileName, sharepointRecordFields);
                await Task.Factory.StartNew(() =>
                { SharepointBatchHelper.WriteRecord(CurrentSharepointBatchDir, sharepointBatchRecord, sharepointFile.Data); });
            }

            
        }

    }
}
