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

namespace Scanquire.Clients.NYP
{
    public class DOHArchiverOLD : AzureCloudArchiver
    {
        const string FIELD_INDEX_PERFORMING_LAB = "01";
        const string FIELD_INDEX_ACCESSION_NUMBER = "02";
        const string FIELD_INDEX_MEDICAL_RECORD_NUMBER = "03";
        const string FIELD_INDEX_DATE_OF_SERVICE = "04";

        protected DOHRecordDialogOLD recordDialog = new DOHRecordDialogOLD();

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
                Dictionary<string, string> sharepointCommonFields = new Dictionary<string, string>();
                sharepointCommonFields[JsonsFieldConstants.JsonFieldScanBatch] = base.CurrentBatchId;
                sharepointCommonFields[JsonsFieldConstants.JsonFieldScanStation] = base.ScanStationMachineName;
                sharepointCommonFields[JsonsFieldConstants.JsonFieldScanDate] = batchTime.ToUniversalTime().ToString(SHAREPOINT_DATETIME_FORMAT);
                SharepointBatchSettings sharepointBatchSettings = new SharepointBatchSettings(base.AzureShareName, base.AzureDataBaseName, null, base.SharepointUserName, base.UnprotectedSharepointPassword, sharepointCommonFields);
                SharepointBatchHelper.WriteSettings(CurrentSharepointBatchDir, sharepointBatchSettings);
            }

            await base.Send(images, progress, cToken);
        }

        public override Task<IList<ISQCommand>> TranslateCommands(IEnumerable<ISQCommand> commands, int documentNumber, int pageNumber, System.Threading.CancellationToken cToken)
        {
            return Task.Factory.StartNew<IList<ISQCommand>>(() =>
            {
                List<ISQCommand> newCommands = commands.ToList();

                if (commands.OfType<SQCommand_Document_IndexField>().Count() > 0)
                { newCommands.Add(new SQCommand_TerminateDocument()); }
                return newCommands;
            });
        }

        public override async Task Send(SQFile sharepointFile, SQFile optixFile, int fileNumber, IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken)
        {
            string performingLaboratory = null;
            string accessionNumber = null;
            string medicalRecordNumber = null;
            DateTime dateOfService = DateTime.MinValue;
            DateTime scanDate = DateTime.Now;

            if (recordDialog.UsePatchCards)
            {
                Trace.TraceInformation("Using Patch Cards");
                SQCommand_Document_IndexField[] indexFieldCommands = sharepointFile.Commands.OfType<SQCommand_Document_IndexField>().ToArray();
                foreach (SQCommand_Document_IndexField indexFieldCommand in indexFieldCommands)
                {
                    switch (indexFieldCommand.Name)
                    {
                        case FIELD_INDEX_ACCESSION_NUMBER:
                            accessionNumber = (string)indexFieldCommand.Value;
                            break;
                        case FIELD_INDEX_DATE_OF_SERVICE:
                            dateOfService = DateTime.Parse((string)indexFieldCommand.Value);
                            break;
                        case FIELD_INDEX_MEDICAL_RECORD_NUMBER:
                            medicalRecordNumber = (string)indexFieldCommand.Value;
                            break;
                        case FIELD_INDEX_PERFORMING_LAB:
                            performingLaboratory = (string)indexFieldCommand.Value;
                            break;
                    }
                }
            }
            else
            {
                Trace.TraceInformation("Not using patch cards");
                performingLaboratory = recordDialog.PerformingLab;
                accessionNumber = recordDialog.AccessionNumber;
                medicalRecordNumber = recordDialog.MedicalRecordNumber;
                dateOfService = recordDialog.DateOfService.Value;
            }

            string fileName = Guid.NewGuid().ToString();

            if (EnableAzureBatch)
            {
                Trace.TraceInformation("Creating sharepoint batch");
                Dictionary<string, string> sharepointCommonFields = new Dictionary<string, string>();
                Dictionary<string, string> sharepointRecordFields = new Dictionary<string, string>();
                sharepointRecordFields[JsonsFieldConstants.JsonFieldPerformingLab] = performingLaboratory;
                sharepointRecordFields[JsonsFieldConstants.JsonFieldAccessionNumber] = accessionNumber;
                sharepointRecordFields[JsonsFieldConstants.JsonFieldMedicalRecordNumber] = medicalRecordNumber;
                sharepointRecordFields[JsonsFieldConstants.JsonFieldDateOfService] = dateOfService.Date.ToUniversalTime().ToString(SHAREPOINT_DATETIME_FORMAT);

                string sharepointRecordFileName = Path.ChangeExtension(fileName, sharepointFile.FileExtension);
                SharepointBatchRecord sharepointBatchRecord = new SharepointBatchRecord(sharepointRecordFileName, sharepointRecordFields);
                await Task.Factory.StartNew(() =>
                { SharepointBatchHelper.WriteRecord(CurrentSharepointBatchDir, sharepointBatchRecord, sharepointFile.Data); });
            }
        }
    }
}
