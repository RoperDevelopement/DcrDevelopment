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
    public class AccountingArchiver : SQFilesystemArchiver
    {
        AccountingRecordDialog InputDialog = new AccountingRecordDialog();

        public override IEnumerable<System.Threading.Tasks.Task<SQImage>> AcquireFromScannerForNew(IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken)
        {
            InputDialog.Clear();
            InputDialog.TryShowDialog(DialogResult.OK);
            return base.AcquireFromScannerForNew(progress, cToken);
        }

        public override async System.Threading.Tasks.Task Send(SQFile file, int fileNumber, IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cancelToken)
        {
            InputDialog.TryShowDialog(DialogResult.OK);

            string relativeDirectory = InputDialog.Year;

            string fileName = string.Concat(
                InputDialog.Month
                , "_", InputDialog.LineItem);
            string fileNameWithExt = Path.ChangeExtension(fileName, file.FileExtension);

            string relativeFilePath = Path.Combine(relativeDirectory, fileNameWithExt);

            SQFilesystemConnector.SaveFileResult saveResults;
            saveResults = await FilesystemConnector.SaveFile(relativeFilePath, file.Data);
            if (saveResults.Success)
            {
                Dictionary<string, string> logEntry = new Dictionary<string, string>()
                { 
                    {"Year", InputDialog.Year }
                    , { "Month", InputDialog.Month }
                    , { "Line Item", InputDialog.LineItem }
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
