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
    public class JHCCCArchiver : SQFilesystemArchiver
    {
        JHCCCRecordDialog InputDialog = new JHCCCRecordDialog();

        public override IEnumerable<System.Threading.Tasks.Task<SQImage>> AcquireFromScannerForNew(IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken)
        {
            InputDialog.Clear();
            InputDialog.TryShowDialog(DialogResult.OK);
            return base.AcquireFromScannerForNew(progress, cToken);
        }

        public override async System.Threading.Tasks.Task Send(SQFile file, int fileNumber, IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cancelToken)
        {
            InputDialog.TryShowDialog(DialogResult.OK);

            string relativeDirectory = InputDialog.BoxId;

            string fileName = string.Concat(
                InputDialog.LastName
                , "_", InputDialog.FirstName
                , "_", InputDialog.RecordId
                , "_", InputDialog.ClosingYear);
            string fileNameWithExt = Path.ChangeExtension(fileName, file.FileExtension);

            string relativeFilePath = Path.Combine(relativeDirectory, fileNameWithExt);

            SQFilesystemConnector.SaveFileResult saveResults;
            saveResults = await FilesystemConnector.SaveFile(relativeFilePath, file.Data);
            if (saveResults.Success)
            {
                Dictionary<string, string> logEntry = new Dictionary<string, string>()
                { 
                    {"Last Name", InputDialog.LastName }
                    , { "First Name", InputDialog.FirstName }
                    , { "Record ID", InputDialog.RecordId }
                    , { "Close Year", InputDialog.ClosingYear }
                    , { "Page Count", InputDialog.ManualPageCount.ToString() }
                    , { "Box ID", InputDialog.BoxId }
                    , { "Image Count", file.PageCount.ToString() }
                    , { "Checksum", file.Checksum }
                    , { "User Name", Environment.UserName }
                    , { "Timestamp", DateTime.Now.ToString() }
                };

                Log.Append(logEntry);
            }
            else throw new OperationCanceledException();
        }

    }
}
