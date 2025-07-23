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

namespace Edocs.ITS.Archiver
{
    public class ITSArchiver : SQFilesystemArchiver
    {
        ITSArchiverDialog InputDialog = new ITSArchiverDialog();
        private readonly string RecordsJson = "_records.json";
        private readonly string SettingsJson = "_settings.json";
        public string RootPath
        {
            get;
            set;
        }
        public string SaveRootPath
        { get; set; }
        public string DropDownItems
        { get; set; }
        public string SpUploadPSEFinancialRecordsJson
        { get; set; }
        public string SpEdocsITS
        { get; set; }
        public string AzureWebApiController
        { get; set; }
        public string ITSWebAPI
        { get; set; }
        public string ITSAzureDataBaseName
        { get; set; }
        public string ShowTotalRecordsDialog
        { get; set; }
        public string SpAddITSDocsScanned
        { get; set; }
        public string AzureShareName
        { get; set; }
        public string UploadExe
        { get; set; }
        public string WebApi
        { get; set; }
        public string AzureWebApiITSController
        { get; set; }

        public string AzureDataBaseName
        { get; set; }
        public string UploadExeParms
        { get; set; }
        public string UploadExeParmsArchiver
        { get; set; }
        public string DisPlayArchiverName
        { get; set; }
        public bool ShowCmdWindow
        { get; set; }

        public override IEnumerable<System.Threading.Tasks.Task<SQImage>> AcquireFromScannerForNew(IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken)
        {


            //  InputDialog.TryShowDialog(DialogResult.OK);
            return base.AcquireFromScannerForNew(progress, cToken);
        }
        public async override Task Send(SQFile file, int fileNumber, IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cancelToken)
        {
            try
            {

            
            InputDialog.TxtDialog = DisPlayArchiverName;
           InputDialog.CmbBoxItems = DropDownItems.Split(',');  
            if (bool.TryParse(ShowTotalRecordsDialog, out bool result))
                InputDialog.ShowTotalRecordsScanned = result;
            InputDialog.TryShowDialog(DialogResult.OK);
            string dirPath = string.Empty;
            string fileName = GetNewGuid();
            string fileNameWithExt = string.Empty;
            string filePath = string.Empty;
            Dictionary<string, string> UploadEntryITS = new Dictionary<string, string>();
            Dictionary<string, string> settingsITS = new Dictionary<string, string>();
            settingsITS["ScanBatch"] = fileName;
            settingsITS["ScanDate"] = DateTime.Now.ToString();
            settingsITS["ScanOperator"] = Environment.UserName;
            settingsITS["ScanMachine"] = Environment.MachineName;
            settingsITS["AzureWebApiController"] = AzureWebApiController;
            settingsITS["AzureShareName"] = AzureShareName;
                settingsITS["WebApi"] = WebApi;
                
                settingsITS["AzureSPName"] = SpUploadPSEFinancialRecordsJson;
                settingsITS["AzureWebApiITSController"] = AzureWebApiITSController;
            settingsITS["ITSWebAPI"] = ITSWebAPI;
            settingsITS["ITSAzureDataBaseName"] = ITSAzureDataBaseName;
                settingsITS["TrackingID"] = InputDialog.TrackingID; 
                     settingsITS["SpAddITSDocsScanned"] = SpAddITSDocsScanned;
                settingsITS["TotalScanned"] = InputDialog.TotalRecordsScanned;
                settingsITS["TotalRecordsUploaded"] = file.PageCount.ToString();
                
                
            string saveFolder = GetNewGuid();
            if (Path.HasExtension(Log.FilePath))
                Log.FilePath = Path.GetDirectoryName(Log.FilePath);
            Log.FilePath = Path.Combine(Log.FilePath, saveFolder);
            if (!(Directory.Exists(Log.FilePath)))
                Directory.CreateDirectory(Log.FilePath);
            dirPath = Path.Combine(SaveRootPath, saveFolder);
            fileNameWithExt = Path.ChangeExtension(fileName, file.FileExtension);
            filePath = Path.Combine(dirPath, fileNameWithExt);
            //  Log.FilePath = Path.Combine(Log.FilePath,$"{saveFolder}{RecordsJson}");
            // Log.FilePath = Path.Combine(Log.FilePath, $"{saveFolder}{SettingsJson}");
            Log.FilePath = Path.Combine(Log.FilePath, $"{saveFolder}{SettingsJson}");
            Log.Append(settingsITS);
            UploadEntryITS["StartYear"] = InputDialog.StYear.ToString("MM-dd-yyyy");
            UploadEntryITS["EndYear"] = InputDialog.EndYear.ToString("MM-dd-yyyy");
            UploadEntryITS["FinancialCaterogyName"] = InputDialog.FinIdentifier;
          
            UploadEntryITS["FileName"] = Path.GetFileName(filePath);
            SQFilesystemConnector.SaveFileResult saveFileResult = await FilesystemConnector.SaveFile(filePath, file.Data);
            Log.FilePath = Log.FilePath.Replace(SettingsJson, RecordsJson);
            Log.Append(UploadEntryITS);
            Log.FilePath = GetDirName(Log.FilePath, saveFolder);
                StartUpload(UploadExe, saveFolder).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch(Exception ex)
            { throw new Exception(ex.Message); }
        }
        public string GetDirName(string dir, string repStr)
        {
            dir = Path.GetDirectoryName(dir);

            return dir.Replace(repStr, string.Empty).Trim();
        }
        public string GetNewGuid()
        {
            return Guid.NewGuid().ToString();
        }
        private async Task StartUpload(string exePath,string batchID)
        {
            Process p = new Process();




            p.StartInfo.FileName = exePath;
            if (!(ShowCmdWindow))
                p.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
            // p.StartInfo.Arguments = string.Format("/batchid:{0} /archiver:{1}", batchID, archiver);
            p.StartInfo.Arguments = string.Format(UploadExeParms, batchID, UploadExeParmsArchiver);
            EdocsUSA.Utilities.Logging.TraceLogger.TraceLoggerInstance.TraceInformation($"Starting process {p.StartInfo.FileName} with parms {p.StartInfo.Arguments}");
            p.Start();
            //await Task.Factory.StartNew(() => p.WaitForExit());
            //if (p.ExitCode != 0)
            //{
            //    EdocsUSA.Utilities.Logging.TraceLogger.TraceLoggerInstance.TraceError($"running process {p.StartInfo.FileName} with parms {p.StartInfo.Arguments} exit code {p.ExitCode.ToString()}");
            //    throw new OperationCanceledException(p.ExitCode.ToString());
            //}
        }
    }
}
