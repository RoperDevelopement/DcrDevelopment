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
    public class SendoutArchiver : AzureCloudArchiver
    {
        const string FIELD_INDEX_PERFORMING_LAB = "01";
        const string FIELD_INDEX_ACCESSION_NUMBER = "02";
        const string FIELD_INDEX_FINANCIAL_NUMBER = "03";
        const string FIELD_INDEX_MEDICAL_RECORD_NUMBER = "04";
        const string FIELD_INDEX_DATE_OF_SERVICE = "05";
        const string FIELD_INDEX_LAST_NAME = "06";
        const string FIELD_INDEX_FIRST_NAME = "07";

        

        protected SendoutRecordDialog recordDialog = new SendoutRecordDialog();

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
            string financialNumber = null;
            string medicalRecordNumber = null;
            string lastName = null;
            string firstName = null;
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
                        case FIELD_INDEX_FINANCIAL_NUMBER:
                            financialNumber = (string)indexFieldCommand.Value;
                            break;
                        case FIELD_INDEX_FIRST_NAME:
                            firstName = (string)indexFieldCommand.Value;
                            break;
                        case FIELD_INDEX_LAST_NAME:
                            lastName = (string)indexFieldCommand.Value;
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
             //   Trace.TraceInformation("Not using patch cards");
                performingLaboratory = recordDialog.PerformingLab;
                accessionNumber = recordDialog.AccessionNumber;
                financialNumber = recordDialog.FinancialNumber;
                medicalRecordNumber = recordDialog.MedicalRecordNumber;
                lastName = recordDialog.LastName;
                firstName = recordDialog.FirstName;
                dateOfService = recordDialog.DateOfService.Value;
            }

            string fileName = Guid.NewGuid().ToString();

            if (EnableAzureBatch)
            {
              //  Trace.TraceInformation("Creating sharepoint batch");
                Dictionary<string, string> sharepointRecordFields = new Dictionary<string, string>();
                sharepointRecordFields[JsonsFieldConstants.JsonFieldPerformingLab] = performingLaboratory;
                sharepointRecordFields[JsonsFieldConstants.JsonFieldAccessionNumber] = accessionNumber;
                sharepointRecordFields[JsonsFieldConstants.JsonFieldFinancialNumber] = financialNumber;
                sharepointRecordFields[JsonsFieldConstants.JsonFieldMedicalRecordNumber] = medicalRecordNumber;
                sharepointRecordFields[JsonsFieldConstants.JsonFieldLastName] = lastName;
                sharepointRecordFields[JsonsFieldConstants.JsonFieldFirstName] = firstName;
                sharepointRecordFields[JsonsFieldConstants.JsonFieldDateOfService] = dateOfService.Date.ToUniversalTime().ToString(SHAREPOINT_DATETIME_FORMAT);

                string sharepointRecordFileName = Path.ChangeExtension(fileName, sharepointFile.FileExtension);
                SharepointBatchRecord sharepointBatchRecord = new SharepointBatchRecord(sharepointRecordFileName, sharepointRecordFields);
                await Task.Factory.StartNew(() =>
                { SharepointBatchHelper.WriteRecord(CurrentSharepointBatchDir, sharepointBatchRecord, sharepointFile.Data); });
            }

         
        }
    }
}
