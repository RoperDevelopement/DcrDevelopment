using Scanquire.Public;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using EdocsUSA.Utilities.Extensions;
using System.IO;
using System.Diagnostics;

namespace DemoArchivers
{
    public class WorkersCompRecordArchiver : SQFilesystemArchiver
    {
        WorkersCompRecordDialog InputDialog = new WorkersCompRecordDialog();

        private string _LogHeader = "Client Name,Case Number,Form Type,Form Date,Form Expiration,Page Count,Checksum,User Name, Timestamp";
        public string LogHeader
        {
            get { return _LogHeader; }
            set { _LogHeader = value; }
        }

        private string _RootPath = @"C:\Edocs\Archives\Cases\";
        public string RootPath
        {
            get { return _RootPath; }
            set { _RootPath = value; }
        }

        public override IEnumerable<System.Threading.Tasks.Task<SQImage>> AcquireForNew(SQAcquireSource source, IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken)
        {
            if (InputDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            { return base.AcquireForNew(source, progress, cToken); }
            else
            { throw new OperationCanceledException(); }
        }

        public override async System.Threading.Tasks.Task Send(SQFile file, int fileNumber, IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cancelToken)
        {
            this.InputDialog.TryShowDialog(DialogResult.OK);

            string clientDirName = string.Join("_", InputDialog.CaseNumber, InputDialog.ClientName);
            string recordFileName = string.Join("_", InputDialog.CaseNumber, InputDialog.ClientName, InputDialog.FormType, InputDialog.FormDate.Value.ToString("yyyyMMdd"));
            
            string recordFileNameWithExt = Path.ChangeExtension(recordFileName, file.FileExtension);

            string recordFileRelPath = Path.Combine(clientDirName, recordFileNameWithExt);
            Trace.TraceInformation("Saving to " + recordFileRelPath);
            await FilesystemConnector.SaveFile(recordFileRelPath, file.Data);


            Dictionary<string, string> logEntry = new Dictionary<string, string>();
            logEntry["Case Number"] = InputDialog.CaseNumber;
            logEntry["Client Name"] = InputDialog.ClientName;
            logEntry["Form Type"] = InputDialog.FormType;
            logEntry["Form Date"] = InputDialog.FormDate.Value.ToShortDateString();
            
            logEntry["Page Count"] = file.PageCount.ToString();
            logEntry["Checksum"] = file.Checksum;
            logEntry["User Name"] = Environment.UserName;
            logEntry["Timestamp"] = DateTime.Now.ToUniversalTime().ToLongTimeString();
            logEntry["File Path"] = recordFileRelPath;
            Trace.TraceInformation("Adding log entry to {0}", Log.FilePath);
            Log.Append(logEntry);
            Trace.TraceInformation("Done saving");
            /*
            if (Process.GetProcessesByName("BoxV2Test").Length == 0)
            { Process.Start(@"C:\CodeTest\BoxV2Test\BoxV2Test\bin\Debug\BoxV2Test.exe"); }
            */
        }
    }
}
