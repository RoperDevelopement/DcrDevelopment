using EdocsUSA.Utilities;
using EdocsUSA.Utilities.Extensions;

using Scanquire.Public;
using Scanquire.Public.ArchivesConstants;
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
/**This archiver was created November 2017, by Payson Lippert
 * It was requested by Raj from NYP and is very similar to the Lab Reqs.
 * This archiver asked for the date of service of well though.
 * 
 */
namespace Scanquire.Clients.NYP
{
    public class PathReqsArchiver : SQArchiverBase
    {
        public string MetadataSiteUrl { get; set; }

        public string ReceiptStation { get; set; }

        public string DefaultCategory { get; set; }

        private bool _EncryptBatchFiles = false;
        public bool EncryptBatchFiles
        {
            get { return _EncryptBatchFiles; }
            set { _EncryptBatchFiles = value; }
        }

        public string OutputDir { get; set; }

        public string ImportScriptPath { get; set; }

        private bool _RequiresRequisitionNumber = false;
        public bool RequiresRequisitionNumber 
        {
            get { return _RequiresRequisitionNumber; }
            set { _RequiresRequisitionNumber = value; }
        }

        static JavaScriptSerializer Serializer = new JavaScriptSerializer();

        public PathReqsArchiver()
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
        { get { return Path.Combine(OutputDir, CurrentBatchId); } }

        
        public override async Task Send(IEnumerable<SQImage> images, IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, CancellationToken cToken)
        {
            
            InputDialog.ReceiptDate = DateTime.Now;
            InputDialog.DateOfService = DateTime.Now;
            InputDialog.Category = DefaultCategory;
            InputDialog.TryShowDialog(DialogResult.OK);

            CurrentBatchId = Guid.NewGuid().ToString();
            string batchSettingsFileName = Path.ChangeExtension(CurrentBatchId + "_settings", "json");
            string batchDir = Path.Combine(OutputDir, CurrentBatchId);
            Directory.CreateDirectory(batchDir);
            string batchSettingsFilePath = Path.Combine(batchDir, batchSettingsFileName);
            Dictionary<string, object> batchSettings = new Dictionary<string, object>()
                { 
                    {JsonsFieldConstants.JsonFieldScanBatch, CurrentBatchId },
                    {JsonsFieldConstants.JsonFieldReceiptDate, InputDialog.ReceiptDate.Value.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ") },
                    {JsonsFieldConstants.JsonFieldDateOfService, InputDialog.DateOfService.Value.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ") },
                    {JsonsFieldConstants.JsonFieldReceiptDate, ReceiptStation },
                    {JsonsFieldConstants.JsonFieldCategory, InputDialog.Category }
                };

            string batchSettingsText = Serializer.Serialize(batchSettings);
            if (EncryptBatchFiles)
            { File.WriteAllText(batchSettingsFilePath, Encryption.EncryptToString(batchSettingsText, DataProtectionScope.LocalMachine)); }
            else
            { File.WriteAllText(batchSettingsFilePath, batchSettingsText); }

            await base.Send(images, progress, cToken);

            Process p = new Process();
            p.StartInfo.FileName = ImportScriptPath;
            p.StartInfo.Arguments = string.Format("/batchid:{0} /RequiresRequisitionNumber:{1}", CurrentBatchId, RequiresRequisitionNumber.ToString());
            p.Start();
            await Task.Factory.StartNew(() => p.WaitForExit());
            if (p.ExitCode != 0) throw new OperationCanceledException(p.ExitCode.ToString());


        }


        public override IEnumerable<Task<SQImage>> AcquireForInsert(SQAcquireSource source, IProgress<ProgressEventArgs> progress, CancellationToken cToken)
        {
            throw new InvalidOperationException("This archiver does not support inserting.");
        }

        PathReqsBatchDialog InputDialog = new PathReqsBatchDialog();

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
                metadata[JsonsFieldConstants.JsonFieldDateOfService] = InputDialog.DateOfService.Value.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");

                string metadataFileName = Path.ChangeExtension(CurrentBatchId + "_records", "json");
                string metadataFilePath = Path.Combine(CurrentBatchDir, metadataFileName);
                Trace.TraceInformation(metadataFilePath);

                string documentFileName = Guid.NewGuid().ToString();
                string documentFileNameWithExt = Path.ChangeExtension(documentFileName, file.FileExtension);
                string documentFilePath = Path.Combine(CurrentBatchDir, documentFileNameWithExt);
                Trace.TraceInformation(documentFilePath);
                metadata[JsonsFieldConstants.JsonFieldFileName] = documentFileNameWithExt;

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
