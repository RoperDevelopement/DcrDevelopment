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
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DemoArchivers
{
    public class NYPLabRecordsArchiver : SQArchiverBase
    {
        public string MetadataSiteUrl { get; set; }

        public string ReceiptStation { get; set; }

        public string DefaultIndexNumber { get; set; }

        public string DefaultCategory { get; set; }

        private bool _EncryptBatchFiles = false;
        public bool EncryptBatchFiles
        {
            get { return _EncryptBatchFiles;}
            set { _EncryptBatchFiles = value; }
        }

        public string OutputDir { get; set; }

        public string ImportScriptPath { get; set; }

        public string[] IndexNumberPriorities = new string[] { @"^(r|R|\d)\d{8}$", @"^\d{15}$" };

        public NYPLabRecordsArchiver()
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
            { commands.Add(new SQCommand_Document_IndexField("Scan_x0020_Date", DateTime.Now.ToUniversalTime())); }
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
            
            //CurrentBatchSettings = new BatchSettings(InputDialog.ReceiptDate.ToUniversalTime(), InputDialog.ReceiptStation);

            CurrentBatchId = Guid.NewGuid().ToString();
            string batchSettingsFileName = Path.ChangeExtension(CurrentBatchId + "_settings", "json");
            string batchDir = Path.Combine(OutputDir, CurrentBatchId);
            Directory.CreateDirectory(batchDir);
            string batchSettingsFilePath = Path.Combine(batchDir,  batchSettingsFileName);
            Dictionary<string, object> batchSettings = new Dictionary<string, object>()
                { 
                    { "Scan_x0020_Batch", CurrentBatchId },
                    { "Receipt_x0020_Date", InputDialog.ReceiptDate.Value.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ") },
                    { "Receipt_x0020_Station", ReceiptStation },
                    { "Category", InputDialog.Category }
                };

            string batchSettingsText = JsonConvert.SerializeObject(batchSettings);
            if (EncryptBatchFiles)
            { File.WriteAllText(batchSettingsFilePath, Encryption.EncryptToString(batchSettingsText, DataProtectionScope.LocalMachine)); }
            else
            { File.WriteAllText(batchSettingsFilePath, batchSettingsText); }

            await base.Send(images, progress, cToken);

            Process p = new Process();
            p.StartInfo.FileName = ImportScriptPath;
            p.StartInfo.Arguments = string.Format("/batchid:{0}", CurrentBatchId);
            p.Start();
            await Task.Factory.StartNew(() => p.WaitForExit());
            if (p.ExitCode != 0) throw new OperationCanceledException(p.ExitCode.ToString());

           
            //p.Start();

            //Process.Start(@"C:\Utilities\UploadLabReqs\Edocs.NYP.UploadLabReqsBatch.exe", "/batchid:" + CurrentBatchSettings.BatchID + " /username:" + InputDialog.Credentials.UserName + " /password:" + InputDialog.Credentials.Password);
            //Process.Start(@"C:\CodeTest\NYPUploadLabBatch\NYPUploadLabBatch\bin\Debug\NYPUploadLabBatch.exe", CurrentBatchSettings.BatchId + " \"" + CurrentBatchSettings.AuthHeader + "\"");
        }
        /*
        public override IEnumerable<Task<SQImage>> AcquireForAppend(SQAcquireSource source, IProgress<ProgressEventArgs> progress, CancellationToken cToken)
        {
            throw new InvalidOperationException("This archiver does not support appending.");
        }
        */
        
        public override IEnumerable<Task<SQImage>> AcquireForInsert(SQAcquireSource source, IProgress<ProgressEventArgs> progress, CancellationToken cToken)
        {
            throw new InvalidOperationException("This archiver does not support inserting.");
        }
        
        NYPLabRecordsInputDialog InputDialog = new NYPLabRecordsInputDialog();
        
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
                string indexNumber = null;
                foreach (string pattern in IndexNumberPriorities)
                {
                    SQCommand_Document_IndexField[] matchingIndexFieldCommands = file.Commands.OfType<SQCommand_Document_IndexField>()
                        .Where(cmd=>cmd.Name=="Index_x0020_Number")
                        .Where(cmd=>(Regex.IsMatch((string)(cmd.Value), pattern)))
                        .ToArray();

                    //If no matches, go to the next priority
                    if (matchingIndexFieldCommands.Length == 0)
                    { continue;}
                    else
                    {
                        string tIndexNumber = null;
                        foreach (SQCommand_Document_IndexField indexFieldCommand in matchingIndexFieldCommands)
                        {
                            if (string.IsNullOrWhiteSpace(tIndexNumber))
                            { tIndexNumber = (string)indexFieldCommand.Value; }
                            else
                            { 
                                if (tIndexNumber.Equals((string)(indexFieldCommand.Value), StringComparison.OrdinalIgnoreCase) == false)
                                { 
                                    tIndexNumber = null;
                                    break;
                                }
                            }
                        }
                        if (string.IsNullOrWhiteSpace(tIndexNumber) == false)
                        { 
                            indexNumber = tIndexNumber;
                            break;
                        }
                    }
                }
                if (string.IsNullOrWhiteSpace(indexNumber))
                { indexNumber = DefaultIndexNumber; }
                metadata["Index_x0020_Number"] = indexNumber;
                
                /*
                string indexNumber = null;
                foreach (SQCommand_Document_IndexField indexFieldCommand in file.Commands.OfType<SQCommand_Document_IndexField>())
                {
                    if (indexNumber == null)
                    { indexNumber = (string)indexFieldCommand.Value; }

                    
                }
                //If there is a single document index field command, set it
                //Otherwise (none or more than one), set it to the default value
                if (indexFieldCommands.Length == 1)
                { metadata["Index_x0020_Number"] = indexFieldCommands[0]; }
                else
                { metadata["Index_x0020_Number"] = DefaultIndexNumber; }
                */
                /*
                foreach (SQCommand_Document_IndexField indexFieldCommand in file.Commands.OfType<SQCommand_Document_IndexField>())
                { metadata[indexFieldCommand.Name] = indexFieldCommand.Value; }
                */

                /*
                //If no accession number was provided, set to default value.
                if (metadata.ContainsKey("Index_x0020_Number") == false)
                { metadata["Index_x0020_Number"] = DefaultIndexNumber; }
                */

                metadata["Scan_x0020_Date"] = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");

                string metadataFileName = Path.ChangeExtension(CurrentBatchId + "_records", "json");
                string metadataFilePath = Path.Combine(CurrentBatchDir, metadataFileName);
                Trace.TraceInformation(metadataFilePath);

                string documentFileName = Guid.NewGuid().ToString();
                string documentFileNameWithExt = Path.ChangeExtension(documentFileName, file.FileExtension);
                string documentFilePath = Path.Combine(CurrentBatchDir, documentFileNameWithExt);
                Trace.TraceInformation(documentFilePath);
                metadata["File_x0020_Name"] = documentFileNameWithExt;
                
                if (EncryptBatchFiles)
                { File.WriteAllBytes(documentFilePath, Encryption.Encrypt(file.Data, DataProtectionScope.LocalMachine)); }
                else
                { File.WriteAllBytes(documentFilePath, file.Data); }

                string metadataText = JsonConvert.SerializeObject(metadata);

                if (EncryptBatchFiles)
                { File.AppendAllLines(metadataFilePath, new string[] { Encryption.EncryptToString(metadataText, DataProtectionScope.LocalMachine) }); }
                else
                { File.AppendAllLines(metadataFilePath, new string[] { metadataText }); }
                


                progress.Report(new EdocsUSA.Utilities.ProgressEventArgs(fileNumber, -1));
            });


            
        }
    }
}
