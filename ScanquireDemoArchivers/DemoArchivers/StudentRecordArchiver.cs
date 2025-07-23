using Scanquire.Public;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using EdocsUSA.Utilities.Extensions;
using System.IO;

namespace DemoArchivers
{
    public class StudentRecordArchiver : SQFilesystemArchiver
    {
        StudentRecordRecordDialog InputDialog = new StudentRecordRecordDialog();
        
        private string _LogHeader = "Last Name,First Name,Form Type,Form Date,Page Count,Checksum,User Name, Timestamp";
        public string LogHeader
        {
            get { return _LogHeader; }
            set { _LogHeader = value; }
        }

        private string _RootPath = @"C:\Archives\Student Records\";
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

            string studentDirName = InputDialog.LastName + "_" + InputDialog.FirstName;
            string recordFileName = string.Join("_", InputDialog.LastName, InputDialog.FirstName
                , InputDialog.FormType, InputDialog.FormDate.Value.ToString("yyyyMMdd"));
            string recordFileNameWithExt = Path.ChangeExtension(recordFileName, file.FileExtension);

            string recordFileRelPath = Path.Combine(studentDirName, recordFileNameWithExt);
            await FilesystemConnector.SaveFile(recordFileRelPath, file.Data);
                        

            Dictionary<string, string> logEntry = new Dictionary<string, string>();
            logEntry["Last Name"] = InputDialog.LastName;
            logEntry["First Name"] = InputDialog.FirstName;
            logEntry["Form Type"] = InputDialog.FormType;
            logEntry["Form Date"] = InputDialog.FormDate.Value.ToShortDateString();

            logEntry["Page Count"] = file.PageCount.ToString();
            logEntry["Checksum"] = file.Checksum;
            logEntry["User Name"] = Environment.UserName;
            logEntry["Timestamp"] = DateTime.Now.ToUniversalTime().ToLongTimeString();
            logEntry["File Path"] = recordFileRelPath;
            Log.Append(logEntry);
        }
    }
}
