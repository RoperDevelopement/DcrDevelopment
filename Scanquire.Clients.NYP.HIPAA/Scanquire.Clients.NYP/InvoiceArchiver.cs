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
    public class InvoiceArchiver : AzureCloudArchiver
    {
        InvoiceRecordDialog recordDialog = new InvoiceRecordDialog();

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
          
            if (base.EnableAzureBatch)
            {
                Dictionary<string, object> sharepointBatchSettings = JsonSettings.GetJsonBatchSettings(CurrentBatchId, batchTime.ToUniversalTime().ToString(SHAREPOINT_DATETIME_FORMAT), base.ScanStationMachineName, string.Empty, base.AzureDataBaseName, base.AzureShareName, base.AzureTableName, base.AzureSPName, base.AzureWebApiController);
                SharepointBatchHelper.WriteSettings(CurrentSharepointBatchDir, sharepointBatchSettings);
                //Dictionary<string, string> sharepointCommonFields = new Dictionary<string, string>();
                //sharepointCommonFields[JsonsFieldConstants.JsonFieldScanBatch] = base.CurrentBatchId;
                //sharepointCommonFields[JsonsFieldConstants.JsonFieldScanStation] = base.ScanStationMachineName;
                //sharepointCommonFields[JsonsFieldConstants.JsonFieldScanDate] = batchTime.ToUniversalTime().ToString(SHAREPOINT_DATETIME_FORMAT);
                //SharepointBatchSettings sharepointBatchSettings = new SharepointBatchSettings(base.AzureShareName, base.AzureDataBaseName, null, base.SharepointUserName, base.UnprotectedSharepointPassword, sharepointCommonFields);
                //SharepointBatchHelper.WriteSettings(CurrentSharepointBatchDir, sharepointBatchSettings);
            }

            await base.Send(images, progress, cToken);
        }

        public override async Task Send(SQFile sharepointFile, SQFile optixFile, int fileNumber, IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken)
        {
           EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Sending file");
            string fileName = Guid.NewGuid().ToString();

            if (EnableAzureBatch)
            {
                Trace.TraceInformation("Creating sharepoint batch");
                Dictionary<string, string> spRec = new Dictionary<string, string>();
                spRec[JsonsFieldConstants.JsonFieldInvoiceDate] = recordDialog.InvoiceDate.Value.Date.ToUniversalTime().ToString(SHAREPOINT_DATETIME_FORMAT);
                spRec[JsonsFieldConstants.JsonFieldDepartment] = recordDialog.Department;
                spRec[JsonsFieldConstants.JsonFieldCategory] = recordDialog.Category;
                spRec[JsonsFieldConstants.JsonFieldAccount] = recordDialog.Account;
                spRec[JsonsFieldConstants.JsonFieldReference] = recordDialog.Reference;

                string spFileName = Path.ChangeExtension(fileName, sharepointFile.FileExtension);
                SharepointBatchRecord spBatch = new SharepointBatchRecord(spFileName, spRec);
                Trace.TraceInformation("Saving sharepoint batch record");
                await Task.Factory.StartNew(() =>
                { SharepointBatchHelper.WriteRecord(CurrentSharepointBatchDir, spBatch, sharepointFile.Data); });
            }

           
        }
    }
}
