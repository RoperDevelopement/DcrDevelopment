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
    public class EmployeeComplianceArchiver : AzureCloudArchiver
    {
        
        EmployeeComplianceArchiverDialog RecordDialog = new EmployeeComplianceArchiverDialog();

        public override IEnumerable<Task<SQImage>> AcquireFromScannerForNew(IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken)
        {
            RecordDialog.Clear();
            RecordDialog.TryShowDialog(DialogResult.OK);
            return base.AcquireFromScannerForNew(progress, cToken);
        }

        public override async Task Send(IEnumerable<SQImage> images, IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken)
        {
            CurrentBatchId = GenerateNewBatchId();
            RecordDialog.TryShowDialog(DialogResult.OK);
            DateTime batchTime = DateTime.Now;
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Starting Employee Compliance archiver for batchid:{CurrentBatchId.ToString()} batchtime:{batchTime.ToString()}");
            if (base.EnableAzureBatch)
            {
                Dictionary<string, object> sharepointBatchSettings = JsonSettings.GetJsonBatchSettings(CurrentBatchId, batchTime.ToUniversalTime().ToString(SHAREPOINT_DATETIME_FORMAT), base.ScanStationMachineName, string.Empty, base.AzureDataBaseName, base.AzureShareName,base.AzureTableName, base.AzureSPName, base.AzureWebApiController);
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
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Sending file for Employee Compliance archiver for batchid:{CurrentBatchId.ToString()}");
            
            
            //Get the new additional log settings
            string form_firstname = RecordDialog.LogFirstName;
            string form_lastname = RecordDialog.LogLastName;
            string form_id_number = RecordDialog.LogIDNumber;
            string form_department = RecordDialog.LogDepartment;
            string form_job_title = RecordDialog.LogJobTitle;
            string form_document_type = RecordDialog.LogDocumentType;
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Sending file for Employee Compliance archiver for batchid:{CurrentBatchId.ToString()} for form_firstname:{form_firstname} form_lastname:{form_lastname} form_id_number:{form_id_number} form_department:{form_department}  form_job_title :{form_job_title}  form_document_type:{form_document_type}");

            string fileName = Guid.NewGuid().ToString();

            if (EnableAzureBatch)
            {
                EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Creating azure batch file for Employee Compliance archiver for batchid:{CurrentBatchId.ToString()}");
                
                Dictionary<string, string> sharepointRecordFields = new Dictionary<string, string>();
                
                //add the record fields here:
                sharepointRecordFields[JsonsFieldConstants.JsonFieldFirstName] = form_firstname;
                sharepointRecordFields[JsonsFieldConstants.JsonFieldLastName] = form_lastname;
                sharepointRecordFields[JsonsFieldConstants.JsonFieldIDNumber] = form_id_number;
                sharepointRecordFields[JsonsFieldConstants.JsonFieldDepartment] = form_department;
                sharepointRecordFields[JsonsFieldConstants.JsonFieldJobTitle] = form_job_title;
                sharepointRecordFields[JsonsFieldConstants.JsonFieldDocumentType] = form_document_type;
                string sharepointRecordFileName = Path.ChangeExtension(fileName, sharepointFile.FileExtension);
                SharepointBatchRecord sharepointBatchRecord = new SharepointBatchRecord(sharepointRecordFileName, sharepointRecordFields);
                await Task.Factory.StartNew(() =>
                { SharepointBatchHelper.WriteRecord(CurrentSharepointBatchDir, sharepointBatchRecord, sharepointFile.Data); });
            }

          
        }
    }
}
