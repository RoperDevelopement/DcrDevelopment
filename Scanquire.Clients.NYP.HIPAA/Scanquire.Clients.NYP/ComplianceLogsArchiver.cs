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
using DebenuPDFLibraryDLL0915;
namespace Scanquire.Clients.NYP
{
    public class ComplianceLogsArchiver : AzureCloudArchiver
    {
        ComplianceLogRecordDialog recordDialog = new ComplianceLogRecordDialog();
       public string LogStationFileDir
        { get; set; }

        public override IEnumerable<Task<SQImage>> AcquireFromScannerForNew(IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken)
        {
            recordDialog.Clear();
         //   recordDialog.TryShowDialog(DialogResult.OK);
            return base.AcquireFromScannerForNew(progress, cToken);
        }

        public override async Task Send(IEnumerable<SQImage> images, IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken)
        {

            string cboxItems = GetLogFiles(base.ComboBox);
            //  recordDialog.CmbBoxItems = base.ComboBox;
            recordDialog.CmbBoxItems = cboxItems;
            recordDialog.LogStationFoler = LogStationFileDir;
            CurrentBatchId = GenerateNewBatchId();
        //     recordDialog.TryShowDialog(DialogResult.OK);
             DateTime batchTime = DateTime.Now;
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Starting the ComplianceLog archiver for batch id:{CurrentBatchId} for batchtime:{batchTime.ToString()}");
            if (base.EnableAzureBatch)
            {
                Dictionary<string, object> sharepointBatchSettings = JsonSettings.GetJsonBatchSettings(CurrentBatchId, batchTime.ToUniversalTime().ToString(SHAREPOINT_DATETIME_FORMAT),base.ScanStationMachineName,string.Empty,base.AzureDataBaseName, base.AzureShareName,base.AzureTableName,base.AzureSPName,base.AzureWebApiController);
                //Dictionary<string, string> sharepointCommonFields = new Dictionary<string, string>();
                //sharepointCommonFields[JsonsFieldConstants.JsonFieldScanBatch] = base.CurrentBatchId;
                //sharepointCommonFields[JsonsFieldConstants.JsonFieldScanStation] =  base.ScanStationMachineName;
                //sharepointCommonFields[JsonsFieldConstants.JsonFieldScanDate] = batchTime.ToUniversalTime().ToString(SHAREPOINT_DATETIME_FORMAT);
                //SharepointBatchSettings sharepointBatchSettings = new SharepointBatchSettings(base.AzureShareName, base.AzureDataBaseName, null, base.SharepointUserName, base.UnprotectedSharepointPassword, sharepointCommonFields);
                //SharepointBatchHelper.WriteSettings(CurrentSharepointBatchDir, sharepointBatchSettings);
                SharepointBatchHelper.WriteSettings(CurrentSharepointBatchDir, sharepointBatchSettings);
            }
             
            await base.Send(images, progress, cToken);
        }

        public override async Task Send(SQDocument document, int documentNumber, IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken)
        {
           
            SQCommand_Document_IndexField[] indexFieldCommands = document.Commands.OfType<SQCommand_Document_IndexField>().ToArray();
            SQCommand_Document_IndexField logStationIndexFieldCommand = indexFieldCommands.Where(c => c.Name.Equals(JsonsFieldConstants.JsonFieldLogStation, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            SQCommand_Document_IndexField logDateIndexFieldCommand = indexFieldCommands.Where(c => c.Name.Equals(JsonsFieldConstants.JsonFieldLogDate, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

            string logStation = (logStationIndexFieldCommand == null) 
                ? null
                : (string)logStationIndexFieldCommand.Value;
            bool logDateValid = false;
            DateTime logDate = default(DateTime);
            if (logDateIndexFieldCommand != null)
            { logDateValid = DateTime.TryParse((string)logDateIndexFieldCommand.Value, out logDate); }

            bool manualIndexingRequired = (string.IsNullOrWhiteSpace(logStation)) || (logDateValid == false);
            if (manualIndexingRequired)
            {

                recordDialog.Clear();
                if (string.IsNullOrWhiteSpace(logStation) == false)
                { recordDialog.LogStation = logStation; }
                if(logDate == default(DateTime))
                {
                    string cDateM = DateTime.Now.Month.ToString();
                    string cYear = DateTime.Now.Year.ToString();
                    cDateM = $"{cDateM}/01/{cYear}";
                    logDate = DateTime.Parse(cDateM);
                    logDateValid = true;
                }
                if (logDateValid)
                { recordDialog.LogDate = logDate; }
                recordDialog.CurrentImage = document.Pages[0].Image.LatestRevision.GetOriginalImageBitmap();
                recordDialog.TryShowDialog(DialogResult.OK);

                logStation = recordDialog.LogStation;
                logDate = recordDialog.LogDate.Value;
               
               
                if (logStationIndexFieldCommand != null)
                { document.Commands.Remove(logStationIndexFieldCommand); }
                if (logDateIndexFieldCommand != null)
                { document.Commands.Remove(logDateIndexFieldCommand); }
                EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Sending to file for ComplianceLog archiver for batch id:{CurrentBatchId} for logStation:{logStation}  logDate:{logDate} ");
                document.Commands.Add(new SQCommand_Document_IndexField(JsonsFieldConstants.JsonFieldLogStation, logStation));
                document.Commands.Add(new SQCommand_Document_IndexField(JsonsFieldConstants.JsonFieldLogDate, logDate.ToString("yyyy-MM-dd")));
            }

            await base.Send(document, documentNumber, progress, cToken);
        }
        private string GetLogFiles(string logStat)
        {
            string logStationFolder = LogStationFileDir.Replace("{UserFolder}", $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}");

            if (System.IO.File.Exists(logStationFolder))
            {
                string txtLogStation = System.IO.File.ReadAllText(logStationFolder);
                if (!(string.IsNullOrWhiteSpace(txtLogStation)))
                    logStat = $"{logStat},{txtLogStation}";
            }
            return logStat;
        }
        public override async Task Send(SQFile sharepointFile, SQFile optixFile, int fileNumber, IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken)
        {
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Sending to file for ComplianceLog archiver for batch id:{CurrentBatchId}");

            SQCommand_Document_IndexField[] indexFieldCommands = sharepointFile.Commands.OfType<SQCommand_Document_IndexField>().ToArray();
            SQCommand_Document_IndexField logStationIndexFieldCommand = indexFieldCommands.Where(c => c.Name.Equals(JsonsFieldConstants.JsonFieldLogStation, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            SQCommand_Document_IndexField logDateIndexFieldCommand = indexFieldCommands.Where(c => c.Name.Equals(JsonsFieldConstants.JsonFieldLogDate, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            if (string.Compare(ComboBox, recordDialog.CmbBoxItems, true) != 0)
                base.ComboBox = recordDialog.CmbBoxItems;
            string logStation = (string)logStationIndexFieldCommand.Value;
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation(string.Format("Getting date from {0}", logDateIndexFieldCommand.Value));
            DateTime logDate = DateTime.Parse((string)logDateIndexFieldCommand.Value);

            string fileName = Guid.NewGuid().ToString();

            if (EnableAzureBatch)
            {
                EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Creating azure batch {CurrentBatchId} for ComplianceLog archiver for logStation:{logStation} logdate:{ logDate.Date.ToUniversalTime().ToString(SHAREPOINT_DATETIME_FORMAT)} Checksum:{sharepointFile.Checksum}");
                Dictionary<string, string> sharepointRecordFields = new Dictionary<string, string>();
                sharepointRecordFields[JsonsFieldConstants.JsonFieldLogStation] = logStation;
                sharepointRecordFields[JsonsFieldConstants.JsonFieldLogDate] = logDate.Date.ToUniversalTime().ToString(SHAREPOINT_DATETIME_FORMAT);
                sharepointRecordFields[JsonsFieldConstants.JsonFieldChecksum] = sharepointFile.Checksum;

                string sharepointRecordFileName = Path.ChangeExtension(fileName, sharepointFile.FileExtension);
                SharepointBatchRecord sharepointBatchRecord = new SharepointBatchRecord(sharepointRecordFileName, sharepointRecordFields);
                await Task.Factory.StartNew(() =>
                { SharepointBatchHelper.WriteRecord(CurrentSharepointBatchDir, sharepointBatchRecord, sharepointFile.Data); });
            }

            
        }
    }
}
