using EdocsUSA.Utilities;
using EdocsUSA.Utilities.Extensions;
using Scanquire.Public.ArchivesConstants;
using Scanquire.Public;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;


namespace Scanquire.Clients.NYP
{
    public class LabReqsArchiver : SQArchiverBase
    {
        public string AzureTableName
        { get; set; }
        public string AzureSPName
        { get; set; }
        public string AzureWebApiController
        { get; set; }
        public string AzureShareName { get; set; }

        //public string ReceiptStation { get; set; }
        public string ReceiptStation { get { return Environment.MachineName; } }

        public string AzureDataBaseName { get; set; }

        public string DefaultCategory { get; set; }

        private bool _EncryptBatchFiles = false;
        public bool EncryptBatchFiles
        {
            get { return _EncryptBatchFiles; }
            set { _EncryptBatchFiles = value; }
        }

        public string OutputDir { get; set; }

        public string ManualIndexingScriptPath { get; set; }

        public string AzureUploadScriptPath { get; set; }

        private bool _RequiresRequisitionNumber = false;
        public bool RequiresRequisitionNumber
        {
            get { return _RequiresRequisitionNumber; }
            set { _RequiresRequisitionNumber = value; }
        }
        private string RadioButton
        { get; set; }

        public bool RequireReqNums
        { get; set; }
        static JavaScriptSerializer Serializer = new JavaScriptSerializer();

        public LabReqsArchiver()
        {
            RadioButton = string.Empty;
        }

        public override IEnumerable<System.Threading.Tasks.Task<SQImage>> AcquireFromFile(IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken)
        {
            throw new NotImplementedException();
        }

        public override async System.Threading.Tasks.Task<IList<ISQCommand>> ReadCommands(SQImage image, int documentNumber, int pageNumber, System.Threading.CancellationToken cToken)
        {
            List<ISQCommand> commands = new List<ISQCommand>();

            //For the first page, add an indexfield for scan date
           // if (pageNumber == 0)
         //   { commands.Add(new SQCommand_Document_IndexField(JsonsFieldConstants.JsonFieldScanDate, DateTime.Now.ToUniversalTime())); }
            //For the second page, add a document terminator
            if (pageNumber > 1)
            { commands.Add(new SQCommand_TerminateDocument()); }
            //Add any commands from the specified command reader.
            commands.AddRange(await base.ReadCommands(image, documentNumber, pageNumber, cToken));
            return commands;
        }

        public static string GetCurrentYearSiteName()
        {
            return string.Concat("Lab_Reqs_Files_", DateTime.Now.Year.ToString());
        }

        protected string CurrentBatchId { get; set; }

        protected string CurrentBatchDir
        { get { return Path.Combine(OutputDir, CurrentBatchId); } }


        public override async Task Send(IEnumerable<SQImage> images, IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, CancellationToken cToken)
        {
          
            if(images.Count() == 0)
            {
                MessageBox.Show("No images/documents to archive", "Error No Documents", MessageBoxButtons.OK, MessageBoxIcon.Error);
                EdocsUSA.Utilities.Logging.TraceLogger.TraceLoggerInstance.TraceError("No Documents imges to archive");
                throw new OperationCanceledException("No images/documents to archive");
            }
            InputDialog.TotalImagesScanned = images.Count().ToString();
          
            InputDialog.ReceiptDate = DateTime.Now;
            InputDialog.Category = DefaultCategory;
            InputDialog.TryShowDialog(DialogResult.OK);
            
            
            RequireReqNums = InputDialog.RequireReqNumbers;
            CurrentBatchId = Guid.NewGuid().ToString();
            EdocsUSA.Utilities.Logging.TraceLogger.TraceLoggerInstance.TraceInformation($"Starting LabRecs Archiver for batchid {CurrentBatchId} for direcoty {CurrentBatchDir} for ReceiptDate:{InputDialog.ReceiptDate.ToString()} for Category:{InputDialog.Category.ToString()}  ");

            
            
                EdocsUSA.Utilities.Logging.TraceLogger.TraceLoggerInstance.TraceInformation("Run indexing after archiving");

            string batchSettingsFileName = Path.ChangeExtension(CurrentBatchId + "_settings", "json");
            string batchDir = Path.Combine(OutputDir, CurrentBatchId);
            Directory.CreateDirectory(batchDir);
            string batchSettingsFilePath = Path.Combine(batchDir, batchSettingsFileName);
            //public class JsonSettings
            EdocsUSA.Utilities.Logging.TraceLogger.TraceLoggerInstance.TraceInformation($"batchSettingsFileName:{batchSettingsFileName} batchDir:{batchDir} batchSettingsFilePath:{batchSettingsFilePath} for labrecs archiver");

            Dictionary<string, object> batchSettings = JsonSettings.GetJsonBatchSettings(CurrentBatchId, InputDialog.ReceiptDate.Value.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ"), ReceiptStation, InputDialog.Category, AzureDataBaseName, AzureShareName, AzureTableName, AzureSPName, AzureWebApiController);
        

            string batchSettingsText = Serializer.Serialize(batchSettings);

            if (EncryptBatchFiles)
            {
                EdocsUSA.Utilities.Logging.TraceLogger.TraceLoggerInstance.TraceInformation($"EncryptBatchFiles writing out setting to file {batchSettingsFilePath}");
                File.WriteAllText(batchSettingsFilePath, Encryption.EncryptToString(batchSettingsText, DataProtectionScope.LocalMachine));
            }
            else
            {
                EdocsUSA.Utilities.Logging.TraceLogger.TraceLoggerInstance.TraceInformation($"Not EncryptBatchFiles writing out setting to file {batchSettingsFilePath}");
                File.WriteAllText(batchSettingsFilePath, batchSettingsText);
            }

           
                EdocsUSA.Utilities.Logging.TraceLogger.TraceLoggerInstance.TraceInformation($"Scan Operator selected Batch id {CurrentBatchId} are duplex documents");
                await base.Send(images, progress, cToken);
             


            Process p = new Process();
                p.StartInfo.FileName = ManualIndexingScriptPath;
                p.StartInfo.Arguments = string.Format("/batchid:{0} /RequiresRequisitionNumber:{1}", CurrentBatchId, RequireReqNums.ToString());
         //   p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                EdocsUSA.Utilities.Logging.TraceLogger.TraceLoggerInstance.TraceInformation($"Starting process {p.StartInfo.FileName} with parms {p.StartInfo.Arguments}");
                p.Start();
                await Task.Factory.StartNew(() => p.WaitForExit());
                if (p.ExitCode != 0)
                {
                    EdocsUSA.Utilities.Logging.TraceLogger.TraceLoggerInstance.TraceError($"running process {p.StartInfo.FileName} with parms {p.StartInfo.Arguments} exit code {p.ExitCode.ToString()}");
                    throw new OperationCanceledException(p.ExitCode.ToString());
                }

            
        }

        public override IEnumerable<Task<SQImage>> AcquireForInsert(SQAcquireSource source, IProgress<ProgressEventArgs> progress, CancellationToken cToken)
        {
            EdocsUSA.Utilities.Logging.TraceLogger.TraceLoggerInstance.TraceError("Archiver AcquireForInsert does not support inserting.");
            throw new InvalidOperationException("This archiver does not support inserting.");
        }

        LabReqsBatchDialog InputDialog = new LabReqsBatchDialog();

        public override async System.Threading.Tasks.Task Send(SQFile file, int fileNumber, IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken)
        {
            EdocsUSA.Utilities.Logging.TraceLogger.TraceLoggerInstance.TraceInformation($"Sending file number {fileNumber.ToString()}");
            //foreach (SQCommand_Document_IndexField c in file.Commands.OfType<SQCommand_Document_IndexField>())
            //{
            //    EdocsUSA.Utilities.Logging.TraceLogger.TraceLoggerInstance.TraceInformation($"Index Name {c.Name} Index Value {c.Value}");
            //}
            
            string documentFileName = Guid.NewGuid().ToString();

            await Task.Factory.StartNew(() =>
            {
                EdocsUSA.Utilities.Logging.TraceLogger.TraceLoggerInstance.TraceInformation($"Sending file {documentFileName}.tif");

                Dictionary<string, object> metadata = new Dictionary<string, object>();

                //Process the document index commands
                SQCommand_Document_IndexField[] indexFieldCommands = file.Commands.OfType<SQCommand_Document_IndexField>().ToArray();
                string indexNumber = string.Empty;
                string reqNumber = string.Empty;
                foreach (SQCommand_Document_IndexField indexFieldCommand in indexFieldCommands)
                {

                     
                          
                        if ((indexFieldCommand.Name == JsonsFieldConstants.JsonFieldIndexNumber) &&(!(string.IsNullOrWhiteSpace(indexFieldCommand.Value.ToString()))))
                    {
                        indexNumber = Edocs_Utilities.EdocsUtilitiesInstance.AddZeros(indexFieldCommand.Value.ToString(), 15, 15);
                        metadata[indexFieldCommand.Name] = indexNumber;
                        //  string pidCc = Edocs_Utilities.EdocsUtilitiesInstance.GetPatIdClientCode(indexNumber, 6, 0);
                        int strStart = indexNumber.Length - 10;
                        string pidCc = indexNumber.Substring(strStart).TrimStart('0');
                        if (!(string.IsNullOrEmpty(pidCc)))
                        {
                            metadata[JsonsFieldConstants.JsonFieldPatientID] = pidCc.ToString();
                            pidCc = Edocs_Utilities.EdocsUtilitiesInstance.GetPatIdClientCode(indexNumber, 0, 6);
                            if (!(string.IsNullOrEmpty(pidCc)))
                            {
                                metadata[JsonsFieldConstants.JsonFieldClientCode] = pidCc.ToString();
                            }
                        }
                    }
                    else
                    {
                        if (indexFieldCommand.Name == JsonsFieldConstants.JsonFieldRequisitionNumber)
                            reqNumber = indexFieldCommand.Value.ToString();

                       // metadata[indexFieldCommand.Name] =   indexFieldCommand.Value;
                        metadata[indexFieldCommand.Name] = indexFieldCommand.Value;
                    }

                }
            
                






                string metadataFileName = Path.ChangeExtension(CurrentBatchId + "_records", "json");
                string metadataFilePath = Path.Combine(CurrentBatchDir, metadataFileName);
                EdocsUSA.Utilities.Logging.TraceLogger.TraceLoggerInstance.TraceInformation($"MetadataFilePath {metadataFilePath}");



                string documentFileNameWithExt = Path.ChangeExtension(documentFileName, file.FileExtension);
                string documentFilePath = Path.Combine(CurrentBatchDir, documentFileNameWithExt);
                EdocsUSA.Utilities.Logging.TraceLogger.TraceLoggerInstance.TraceInformation($"DocumentFilePath {documentFilePath}");
                metadata[JsonsFieldConstants.JsonFieldFileName] = documentFileNameWithExt;
            //using (MemoryStream s = new MemoryStream(file.Data))
            //    {
            //        System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(s);
                     
            //        using (System.Drawing.Bitmap targetBmp = bitmap.Clone(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height), System.Drawing.Imaging.PixelFormat.Format8bppIndexed))
            //        {
            //            // targetBmp is now in the desired format.
            //            targetBmp.Save("l:\\t.png", System.Drawing.Imaging.ImageFormat.Png);
            //        }
                    
            //    }
                
                if (EncryptBatchFiles)
                { File.WriteAllBytes(documentFilePath, Encryption.Encrypt(file.Data, DataProtectionScope.LocalMachine)); }
                else
                { File.WriteAllBytes(documentFilePath, file.Data); }

                string metadataText = Serializer.Serialize(metadata);

                if (EncryptBatchFiles)
                { File.AppendAllLines(metadataFilePath, new string[] { Encryption.EncryptToString(metadataText, DataProtectionScope.LocalMachine) }); }
                else
                { File.AppendAllLines(metadataFilePath, new string[] { metadataText }); }


                progress.Report(new EdocsUSA.Utilities.ProgressEventArgs(fileNumber, -1));
            });



        }
    }
}
