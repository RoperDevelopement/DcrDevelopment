using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using EdocsUSA.Controls;
using Scanquire.Public;
using Scanquire.Public.Extensions;
using Scanquire.Public.UserControls;
using EdocsUSA.Utilities;
using EdocsUSA.Utilities.Extensions;
using EdocsUSA.Utilities.Logging;
using System.Runtime.Remoting.Contexts;


namespace Edocs.MDT.Archiver
{
    public class MDTArchiver : SQFilesystemArchiver
    {
        MDTArchiverDialog InputDialog = new MDTArchiverDialog();
        private readonly string RecordsJson = "_records.json";
        private readonly string SettingsJson = "_settings.json";
        public string DisPlayArchiverName
        { get; set; }
        public bool IncludeBlankDocs
        { get; set; }
        public bool ShowTotalDocsScanned
        { get; set; }
        public string SaveRootPath
        { get; set; }
        public string UpLoadFolder
        { get; set; }
        public string ProjectNumberNameTextFile
        { get; set; }
        public bool ShowCmdWindow
        { get; set; }
        public string UploadExe
        { get; set; }
        public string UploadExeParms
        { get; set; }
        public string UploadExeParmsArchiver
        { get; set; }
        public string InventoryTrackingApiUrl
        { get; set; }
        public string InventoryTrackingSP
        { get; set; }

        public string BoxNumber
        { get; set; }
        public bool ShowConfirmDialogBox
        { get; set; }
        public string EdocsCustomerID
        { get; set; }
        public override IEnumerable<System.Threading.Tasks.Task<SQImage>> AcquireFromScannerForNew(IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken)
        {


            //  InputDialog.TryShowDialog(DialogResult.OK);
            return base.AcquireFromScannerForNew(progress, cToken);
        }
        public async override Task Send(SQFile file, int fileNumber, IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cancelToken)
        {
            // OpenFileDialog _OpenFileDialog = new OpenFileDialog() { RestoreDirectory = true, Multiselect = true };
            // string[] paths = _OpenFileDialog.SelectFiles(null, null);
            //   EdocsUSA.Utilities.Extensions.OpenFileDialogExtensions.TryShowDialog(null,null);
            string upLoadDir = string.Empty;
            InputDialog.Text = DisPlayArchiverName;
            InputDialog.ShowConfirmDialogBox = ShowConfirmDialogBox;
            InputDialog.CmboxNumbers = BoxNumber;
            InputDialog.IncludeBlankDocs = IncludeBlankDocs;
            InputDialog.ShowTotalDocsScanned = ShowTotalDocsScanned;
            InputDialog.ProjectNameNums = ProjectNumberNameTextFile;
            InputDialog.TryShowDialog(DialogResult.OK);
            string saveFolder = GetNewGuid();
            string dirPath = Path.Combine(SaveRootPath, saveFolder);

            string[] pN = InputDialog.Project.Trim().Split(';');

            upLoadDir = $"{pN[0]} {pN[1]}";


            string fileName = $"{InputDialog.DocType}_{upLoadDir}";

            //   string fileNameWithExt = Path.ChangeExtension(fileName, file.FileExtension);
            string fileNameWithExt = $"{fileName}.pdf";
            // Path.ChangeExtension(fileName, file.FileExtension);

            string filePath = Path.Combine(dirPath, fileNameWithExt);


            Dictionary<string, string> recordsJs = new Dictionary<string, string>();
            Dictionary<string, string> settingsJs = new Dictionary<string, string>();
            settingsJs["ScanBatchID"] = saveFolder;
            settingsJs["ScanDate"] = DateTime.Now.ToString();
            settingsJs["ScanOperator"] = Environment.UserName;
            settingsJs["ScanMachine"] = Environment.MachineName;
            settingsJs["InventoryTrackingApiUrl"] = InventoryTrackingApiUrl;
            settingsJs["InventoryTrackingSP"] = InventoryTrackingSP;
            // settingsJs["UploadFolder"] = UpLoadFolder.Replace("{ProjectName}", $"{InputDialog.BoxNumber}\\{upLoadDir}");
            settingsJs["UploadFolder"] = UpLoadFolder.Replace("{ProjectName}", $"{InputDialog.BoxNumber}\\");
            settingsJs["EdocsCustomerID"] = EdocsCustomerID;
            Log.FilePath = Path.Combine(dirPath, $"{saveFolder}{SettingsJson}");

            Log.Append(settingsJs);
            recordsJs["FileName"] = fileNameWithExt;
            recordsJs["DocumentType"] = InputDialog.DocType;
            recordsJs["ProjectName"] = pN[0].Trim();
            recordsJs["ProjectNumber"] = pN[1].Trim();
            recordsJs["TotalScanned"] = InputDialog.TotalScanned;
            recordsJs["TotalPageCount"] = file.PageCount.ToString();
            recordsJs["BoxNumber"] = InputDialog.BoxNumber;

            Log.FilePath = Path.Combine(dirPath, $"{saveFolder}{RecordsJson}");
            Log.Append(recordsJs);
            SQFilesystemConnector.SaveFileResult saveFileResult = await FilesystemConnector.SaveFile(filePath, file.Data);
         
            StartUpload(saveFolder, SaveRootPath).ConfigureAwait(false).GetAwaiter().GetResult();

        }
        
        public string GetNewGuid()
        {
            return Guid.NewGuid().ToString();
        }
        private async Task StartUpload(string batchID,string uploadFolder)
        {
            try
            {
               // uploadFolder = uploadFolder.Replace("{ProjectName}", "").Trim();
            Process p = new Process();

            p.StartInfo.FileName = UploadExe;
            if (!(ShowCmdWindow))
                p.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
            // p.StartInfo.Arguments = string.Format("/batchid:{0} /archiver:{1}", batchID, archiver);
            p.StartInfo.Arguments = string.Format(UploadExeParms, batchID, uploadFolder);
            EdocsUSA.Utilities.Logging.TraceLogger.TraceLoggerInstance.TraceInformation($"Starting process {p.StartInfo.FileName} with parms {p.StartInfo.Arguments}");
             p.Start();
                // await Task.Factory.StartNew(() => p.WaitForExit());
                p.WaitForExit();
                if (p.ExitCode != 0)
                {
                    EdocsUSA.Utilities.Logging.TraceLogger.TraceLoggerInstance.TraceError($"running process {p.StartInfo.FileName} with parms {p.StartInfo.Arguments} exit code {p.ExitCode.ToString()}");
                    EdocsUSA.Utilities.Logging.TraceLogger.TraceLoggerInstance.TraceError($"running process {UploadExe} with parms {batchID} {uploadFolder}");
                    MessageBox.Show($"Process {UploadExe} with parms {batchID} {uploadFolder} did not start", "Error Uploading", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    throw new Exception(p.ExitCode.ToString());
                }
             }
            catch(Exception ex)
            {
               
                throw new OperationCanceledException(ex.Message);
            }
        }
    }

}
