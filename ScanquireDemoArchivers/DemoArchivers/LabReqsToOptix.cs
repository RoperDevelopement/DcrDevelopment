using EdocsUSA.Utilities;
using EdocsUSA.Utilities.Extensions;
using Newtonsoft.Json;
using Scanquire.Public;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web.Script.Serialization;

namespace DemoArchivers
{
    public class LabReqsToOptix : SQArchiverBase
    {
        public string ReceiptStation { get; set; }

        public string ImportScriptPath
        { get; set; }

        public string OutputDir { get; set; }


        public string DefaultCategory { get; set; }

        public override IEnumerable<Task<SQImage>> AcquireFromFile(IProgress<ProgressEventArgs> progress, CancellationToken cToken)
        {
            throw new NotImplementedException();
        }

        public override async System.Threading.Tasks.Task<IList<ISQCommand>> ReadCommands(SQImage image, int documentNumber, int pageNumber, System.Threading.CancellationToken cToken)
        {
            List<ISQCommand> commands = new List<ISQCommand>();

            //For the first page, add an indexfield for scan date
            if (pageNumber == 0)
            { commands.Add(new SQCommand_Document_IndexField("Scan_x0020_Date", DateTime.Now.ToUniversalTime())); }
            //For the second page, add a document terminator
            if (pageNumber > 1)
            { commands.Add(new SQCommand_TerminateDocument()); }
            //Add any commands from the specified command reader.
            //Trace.TraceInformation(this.CommandReaderName);
            //Trace.TraceInformation(base.CommandReaderName);
            commands.AddRange(await base.ReadCommands(image, documentNumber, pageNumber, cToken));
            return commands;
        }

        public override IEnumerable<Task<SQImage>> AcquireForInsert(SQAcquireSource source, IProgress<ProgressEventArgs> progress, CancellationToken cToken)
        {
            if (MessageBox.Show("Inserting to the wrong location can throw off the page order and cause problems."
                + Environment.NewLine
                + "If you are sure you want to insert, click OK, otherwise, click Cancel"
                , "Confirm Insert", MessageBoxButtons.OKCancel) != DialogResult.OK)
            { throw new OperationCanceledException(); }
            return base.AcquireForInsert(source, progress, cToken);
        }

        private string _OptixProcessName = "optix";

        public LabReqsToOptix()
        {

        }

        NYPLabRecordsInputDialog InputDialog = new NYPLabRecordsInputDialog();

        public override async Task Send(IEnumerable<SQImage> images, IProgress<ProgressEventArgs> progress, CancellationToken cToken)
        {
            if (Process.GetProcessesByName(_OptixProcessName).Length == 0)
            {
                MessageBox.Show("Optix is not running, please open optix, log into the server and try again");
                throw new OperationCanceledException();
            }
            await base.Send(images, progress, cToken);
        }

        public override async System.Threading.Tasks.Task Send(SQFile file, int fileNumber, IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken)
        {
            InputDialog.TryShowDialog(DialogResult.OK);
            string batchId = Guid.NewGuid().ToString();
            string fileName = batchId;
            string fileNameWithExt = Path.ChangeExtension(fileName, file.FileExtension);
            string filePath = Path.Combine(OutputDir, fileNameWithExt);
       
            SQFilesystemConnector.SaveFileResult saveResults;
            await Task.Factory.StartNew(()=>File.WriteAllBytes(filePath, file.Data));

            Dictionary<string, string> record = new Dictionary<string, string>()
            { 
                { "Receipt Date", InputDialog.ReceiptDate.Value.ToString("yyyy-MM-dd") },
                { "Receipt Station", this.ReceiptStation },
                { "Category", InputDialog.Category }
            };

            string mDataPath = Path.Combine(OutputDir, Path.ChangeExtension(batchId, "json"));
            File.WriteAllText(mDataPath, new JavaScriptSerializer().Serialize(record));

            Process p = new Process();
            p.StartInfo.FileName = ImportScriptPath;
            p.StartInfo.Arguments = "/batchid:" + batchId;
            //p.StartInfo.CreateNoWindow = true;
            p.Start();

            await Task.Factory.StartNew(() => p.WaitForExit());
            if (p.ExitCode != 0) throw new OperationCanceledException();
        }
    }
}
