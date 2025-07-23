using Newtonsoft.Json;
using Scanquire.Public;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using EdocsUSA.Utilities.Extensions;
using System.Threading.Tasks;

namespace DemoArchivers
{
    public class NYPSendoutPackingSlipArchiver : SQArchiverBase
    {
        public string OutputDir { get; set; }

        public string ImportScriptPath { get; set; }

        public string ReceiptStation { get; set; }

        private bool _EncryptBatchFiles = false;
        public bool EncryptBatchFiles
        {
            get { return _EncryptBatchFiles; }
            set { _EncryptBatchFiles = value; }
        }

        protected string CurrentBatchId;

        public override async System.Threading.Tasks.Task Send(IEnumerable<SQImage> images, IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken)
        {
            //Make sure optix is running
            if (Process.GetProcessesByName("optix").Length == 0)
            {
                MessageBox.Show("Optix is not running, please open optix, log into the server and try again");
                throw new OperationCanceledException();
            }
            
            await base.Send(images, progress, cToken);
        }

        public override async System.Threading.Tasks.Task Send(SQFile file, int fileNumber, IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken)
        {
            NypSendoutPackingSlipsInputDialog inputDialog = new NypSendoutPackingSlipsInputDialog();
            inputDialog.TryShowDialog(DialogResult.OK);
            if (inputDialog.ProcDate.HasValue ==false)
            { throw new OperationCanceledException("Proc Date was not provided"); }

            string batchId = Guid.NewGuid().ToString();
            string batchDir = Path.Combine(OutputDir, batchId);
            Directory.CreateDirectory(batchDir);

            string documentFileName = batchId;
            string documentFileNameWithExt = Path.ChangeExtension(documentFileName, file.FileExtension);
            string documentFilePath = Path.Combine(batchDir, documentFileNameWithExt);

            string batchSettingsFileName = Path.ChangeExtension(batchId + "_settings", "json");
            string batchSettingsFilePath = Path.Combine(batchDir, batchSettingsFileName);
            
            Dictionary<string, object> batchSettings = new Dictionary<string, object>()
            { 
                { "Batch_ID", batchId },
                { "Proc_Date", inputDialog.ProcDate.Value.ToString("yyyy-MM-dd") },
                { "Scan_Date", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ") },
                { "Receipt_Station", ReceiptStation },
                { "File_Name", documentFileNameWithExt}
            };

            string batchSettingsText = JsonConvert.SerializeObject(batchSettings);
            if (EncryptBatchFiles)
            { 
                File.WriteAllText(batchSettingsFilePath, Encryption.EncryptToString(batchSettingsText, DataProtectionScope.LocalMachine)); 
                File.WriteAllBytes(documentFilePath, Encryption.Encrypt(file.Data, DataProtectionScope.LocalMachine));
            }
            else
            { 
                File.WriteAllText(batchSettingsFilePath, batchSettingsText); 
                File.WriteAllBytes(documentFilePath, file.Data);
            }            

            Process p = new Process();
            p.StartInfo.FileName = ImportScriptPath;
            p.StartInfo.Arguments = string.Format("/batchid:{0}", batchId);
            p.Start();
            await Task.Factory.StartNew(() => p.WaitForExit());

            if (p.ExitCode != 0) throw new OperationCanceledException(p.ExitCode.ToString());
        }

        public override IEnumerable<System.Threading.Tasks.Task<SQImage>> AcquireFromFile(IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken)
        {
            throw new NotImplementedException();
        }
    }
}
