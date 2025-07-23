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
    public class RecordManagerArchiver : SQArchiverBase
    {
        public string MetadataSiteUrl { get; set; }

        //public string ReceiptStation { get; set; }

        public string AzureTableName
        { get; set; }
        public string AzureSPName
        { get; set; }
        public string AzureWebApiController
        { get; set; }
        public string ReceiptStation
        {
            get { return Environment.MachineName; }
        }
        public string AzureDataBaseName
        { get; set; }

        public string AzureShareName
        { get; set; }
        //        public string DefaultIndexNumber { get; set; }

        public string DefaultCategory { get; set; }

        private bool _EncryptBatchFiles = false;
        public bool EncryptBatchFiles
        {
            get { return _EncryptBatchFiles; }
            set { _EncryptBatchFiles = value; }
        }

        private string _OutputDir = @"C:\Archives\";
        public string OutputDir
        {
            get { return _OutputDir; }
            set { _OutputDir = value; }
        }

        public string ImportScriptPath { get; set; }

        

        private bool _RequiresRequisitionNumber = false;
        public bool RequiresRequisitionNumber 
        {
            get { return _RequiresRequisitionNumber; }
            set { _RequiresRequisitionNumber = value; }
        }

        static JavaScriptSerializer Serializer = new JavaScriptSerializer();

        public RecordManagerArchiver()
        {

        }

        public override IEnumerable<System.Threading.Tasks.Task<SQImage>> AcquireFromFile(IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken)
        {
            throw new NotImplementedException();
        }

        public override async System.Threading.Tasks.Task<IList<ISQCommand>> ReadCommands(SQImage image, int documentNumber, int pageNumber, System.Threading.CancellationToken cToken)
        {
            List<ISQCommand> commands = new List<ISQCommand>();

            //For the first page, add an indexfield for scan date
            if (pageNumber == 0)
            { commands.Add(new SQCommand_Document_IndexField(JsonsFieldConstants.JsonFieldScanDate, DateTime.Now.ToUniversalTime())); }
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
        { get { return Path.Combine(OutputDir, ""); } }


        public override async Task Send(IEnumerable<SQImage> images, IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, CancellationToken cToken)
        {
            /*
            if (Process.GetProcessesByName("optix").Length == 0)
            {
                MessageBox.Show("Optix is not running, please open optix, log into the server and try again");
                throw new OperationCanceledException();
            }
            */
            InputDialog.ReceiptDate = DateTime.Now;
            InputDialog.Category = DefaultCategory;
            InputDialog.TryShowDialog(DialogResult.OK);
            List<SQImage> test = images.ToList();
            
            //CurrentBatchSettings = new BatchSettings(InputDialog.ReceiptDate.ToUniversalTime(), InputDialog.ReceiptStation);
            
            CurrentBatchId = "manifest";
            string batchSettingsFileName = Path.ChangeExtension(CurrentBatchId + "_settings", "json");
            //string batchDir = Path.Combine(OutputDir, CurrentBatchId);
            Directory.CreateDirectory(CurrentBatchDir);
            string batchSettingsFilePath = Path.Combine(CurrentBatchDir, batchSettingsFileName);
            string imageFilePath = Path.Combine(CurrentBatchDir, "manifest_records.json");
            string recordsFilePath = Path.Combine(CurrentBatchDir, "manifest.tif");

            Dictionary<string, object> batchSettings = JsonSettings.GetJsonBatchSettings(CurrentBatchId, InputDialog.ReceiptDate.Value.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ"), ReceiptStation, InputDialog.Category, AzureDataBaseName, AzureShareName,AzureTableName,AzureSPName,AzureWebApiController);
            //Dictionary<string, object> batchSettings = new Dictionary<string, object>()
            //    { 
            //        {JsonsFieldConstants.JsonFieldScanBatch, CurrentBatchId },
            //        {JsonsFieldConstants.JsonFieldReceiptDate, InputDialog.ReceiptDate.Value.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ") },
            //        {JsonsFieldConstants.JsonFieldReceiptStation, ReceiptStation },
            //        {JsonsFieldConstants.JsonFieldCategory, InputDialog.Category }
            //    };

            string batchSettingsText = Serializer.Serialize(batchSettings);
            if (EncryptBatchFiles)
            { File.WriteAllText(batchSettingsFilePath, Encryption.EncryptToString(batchSettingsText, DataProtectionScope.LocalMachine)); }
            else
            { File.WriteAllText(batchSettingsFilePath, batchSettingsText); }

            await base.Send(images, progress, cToken);

            /*
             * Runs OCR on the given file. OCRtest must the debug version.
             * Waits for exit.
             */
            Process ocr = new Process();
            ocr.StartInfo.FileName = @"C:\RecordManager\OCRtest\OCRtest.exe";
            ocr.StartInfo.Arguments = string.Format("/ImageName:{0}", recordsFilePath);
            ocr.Start();
            await Task.Factory.StartNew(() => ocr.WaitForExit());
   
            /*
             * Runs Manual Indexing, allows user to fix what the automatic OCR read.
             */
            Process p = new Process();
            p.StartInfo.FileName = ImportScriptPath;
            p.StartInfo.Arguments = string.Format("/ImageName:{0}", imageFilePath);
            p.Start();
            
            await Task.Factory.StartNew(() => p.WaitForExit());
            if (p.ExitCode != 0) throw new OperationCanceledException(p.ExitCode.ToString());

            
        }


        public override IEnumerable<Task<SQImage>> AcquireForInsert(SQAcquireSource source, IProgress<ProgressEventArgs> progress, CancellationToken cToken)
        {
            throw new InvalidOperationException("This archiver does not support inserting.");
        }

        RecordManagerBatchDialog InputDialog = new RecordManagerBatchDialog();

        public override async System.Threading.Tasks.Task Send(SQFile file, int fileNumber, IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken)
        {
            foreach (SQCommand_Document_IndexField c in file.Commands.OfType<SQCommand_Document_IndexField>())
            {
                Debug.WriteLine(c.Name + " " + c.Value);
            }
            await Task.Factory.StartNew(() =>
            {
                Debug.WriteLine("Sending file");

                Dictionary<string, object> metadata = new Dictionary<string, object>();

                //Process the document index commands
                SQCommand_Document_IndexField[] indexFieldCommands = file.Commands.OfType<SQCommand_Document_IndexField>().ToArray();
                foreach (SQCommand_Document_IndexField indexFieldCommand in indexFieldCommands)
                { metadata[indexFieldCommand.Name] = indexFieldCommand.Value; }

                metadata[JsonsFieldConstants.JsonFieldScanDate] = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");

                string metadataFileName = Path.ChangeExtension(CurrentBatchId + "_records", "json");
                string metadataFilePath = Path.Combine(CurrentBatchDir, metadataFileName);
                Trace.TraceInformation(metadataFilePath);

                //Always use manifest, this is what the OCR/ManualIndexing reads.
                string documentFileName = "manifest";
                string documentFileNameWithExt = Path.ChangeExtension(documentFileName, file.FileExtension);
                string documentFilePath = Path.Combine(CurrentBatchDir, documentFileNameWithExt);
                Trace.TraceInformation(documentFilePath);

                if (File.Exists(metadataFilePath)) { File.Delete(metadataFilePath); }
                metadata[JsonsFieldConstants.JsonFieldFileName] = documentFileNameWithExt;

                if (EncryptBatchFiles)
                { File.WriteAllBytes(documentFilePath, Encryption.Encrypt(file.Data, DataProtectionScope.LocalMachine)); }
                else
                { File.WriteAllBytes(documentFilePath, file.Data); }

                string metadataText = Serializer.Serialize(metadata);

                if (EncryptBatchFiles)
                { File.AppendAllLines(metadataFilePath, new string[] { Encryption.EncryptToString(metadataText, DataProtectionScope.LocalMachine) }); }
                else
                {File.AppendAllLines(metadataFilePath, new string[] { metadataText }); }


                progress.Report(new EdocsUSA.Utilities.ProgressEventArgs(fileNumber, -1));
            });



        }
    }
}
