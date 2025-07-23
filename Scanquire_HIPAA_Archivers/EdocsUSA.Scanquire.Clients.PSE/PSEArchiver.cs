using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using EdocsUSA.Controls;
using Scanquire.Public;
using Scanquire.Public.Extensions;
using Scanquire.Public.UserControls;
using EdocsUSA.Utilities;
using EdocsUSA.Utilities.Extensions;
using EdocsUSA.Utilities.Logging;

namespace EdocsUSA.Scanquire.Clients.PSE
{
    public class PSEArchiver : SQFilesystemArchiver
    {
        PSEArchiverDialog InputDialog = new PSEArchiverDialog();
        // private string _RootPath = @"C:\Archives\AltaCare\Medical Records\";
        private readonly string RecordsJson = "_records.json";
        private readonly string SettingsJson = "_settings.json";
        //public string RootPath
        //{
        //    get { return _RootPath; }
        //    set { _RootPath = value; }
        //}
        public string RootPath
        {
            get;
            set;
        }
        public string SaveRootPath
        { get; set; }
        public string DropDownItems
        { get; set; }
        public string SpStudentRecords
        { get; set; }
        public string SpStudentRecordsFinRecords
        { get; set; }
        public string WebApi
        { get; set; }
        public string ShowTotalRecordsDialog
        { get; set; }

        public string AzureShareName
        { get; set; }
        public string UploadExe
        { get; set; }
        public string AzureWebApiController
        { get; set; }

              public string AzureDataBaseName
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
            InputDialog.TxtDialog = DisPlayArchiverName;
            string dirPath = string.Empty;
            string fileName = GetNewGuid();
            string fileNameWithExt = string.Empty;
            string filePath = string.Empty;
            if (bool.TryParse(ShowTotalRecordsDialog, out bool result))
                InputDialog.ShowTotalRecordsScanned = result;
            InputDialog.TryShowDialog(DialogResult.OK);
            Dictionary<string, string> logEntrySRFR = new Dictionary<string, string>();
            Dictionary<string, string> settingsSRFR = new Dictionary<string, string>();
            settingsSRFR["ScanBatchID"] = fileName;
            settingsSRFR["ScanDate"] = DateTime.Now.ToString();
            settingsSRFR["ScanOperator"] = Environment.UserName;
            settingsSRFR["ScanMachine"] = Environment.MachineName;
            settingsSRFR["WebApi"] = WebApi;
            settingsSRFR["AzureShareName"] = AzureShareName;
            settingsSRFR["AzureWebApiController"] = AzureWebApiController;
            
            
            string saveFolder = GetNewGuid();
            if (Path.HasExtension(Log.FilePath))
                Log.FilePath = Path.GetDirectoryName(Log.FilePath);
            Log.FilePath = Path.Combine(Log.FilePath, saveFolder);
            //  Log.FilePath = Path.Combine(Log.FilePath,$"{saveFolder}{RecordsJson}");
            Log.FilePath = Path.Combine(Log.FilePath, $"{saveFolder}{SettingsJson}");
            
           
            dirPath = Path.Combine(SaveRootPath, saveFolder);
            fileNameWithExt = Path.ChangeExtension(fileName, file.FileExtension);
            filePath = Path.Combine(dirPath, fileNameWithExt);
            if (string.IsNullOrWhiteSpace(InputDialog.TotalRecordsScanned))
                logEntrySRFR["TotalScanned"] = "0";
            else
                logEntrySRFR["TotalScanned"] = InputDialog.TotalRecordsScanned;

            if (string.Compare(InputDialog.ArchiveName, "FinRecords", true) == 0)
            {
                settingsSRFR["AzureSPName"] = SpStudentRecordsFinRecords;
                settingsSRFR["DocumentType"] = "FinancialRecords";
                Log.Append(settingsSRFR);
                PSEFinancialRecordsDialog recordsDialog = new PSEFinancialRecordsDialog();
                recordsDialog.CmbBoxItems = DropDownItems;
                recordsDialog.TryShowDialog(DialogResult.OK);
                // dirPath = string.Format("{0}{1}\\{2}_{3}_{4}", SaveRootPath, FinRecordsFolder, recordsDialog.StYear, recordsDialog.EndYear, recordsDialog.FinIdentifier);
                //dirPath = string.Format("{0}{1}\\{2}_{3}_{4}", SaveRootPath,saveFolder );
               
                // fileName = string.Format("{0}_{1}_{2}", recordsDialog.StYear, recordsDialog.EndYear, recordsDialog.FinIdentifier);
               // fileName = string.Format("{0}_{1}_{2}", recordsDialog.StYear, recordsDialog.EndYear, recordsDialog.FinIdentifier);
                
                logEntrySRFR["StartYear"] = recordsDialog.StYear.ToString("MM-dd-yyyy");
                logEntrySRFR["EndYear"] = recordsDialog.EndYear.ToString("MM-dd-yyyy");
                logEntrySRFR["FinancialCaterogyName"] = recordsDialog.FinIdentifier;
                logEntrySRFR["TotalRecords"] = file.PageCount.ToString();
                logEntrySRFR["PSEPath"] = Path.GetFileName(filePath);
                SQFilesystemConnector.SaveFileResult saveFileResult = await FilesystemConnector.SaveFile(filePath, file.Data);
                Log.FilePath = Log.FilePath.Replace(SettingsJson, RecordsJson);
                Log.Append(logEntrySRFR);
                Log.FilePath = GetDirName(Log.FilePath, saveFolder);
            }
            else
            {
                settingsSRFR["AzureSPName"] = SpStudentRecords;
                settingsSRFR["DocumentType"] = "StudentRecords";
                Log.Append(settingsSRFR);
                PSEStudentDialog studentDialog = new PSEStudentDialog();
                studentDialog.TryShowDialog(DialogResult.OK);
                //dirPath = string.Format("{0}{1}\\{2}_{3}", SaveRootPath, StudentFolder, studentDialog.StudentFName, studentDialog.StudentLName);
                //fileName = string.Format("{0}_{1}", studentDialog.StudentFName, studentDialog.StudentLName);
                //fileNameWithExt = Path.ChangeExtension(fileName, file.FileExtension);
                //filePath = Path.Combine(dirPath, fileNameWithExt);
                Dictionary<string, string> logEntrySR = new Dictionary<string, string>();
                logEntrySRFR["StudentFirstName"] = studentDialog.StudentFName;
                logEntrySRFR["StudentLastName"] = studentDialog.StudentLName;
                logEntrySRFR["StudentDateOfBirth"] = studentDialog.StudentDOB.ToString();
                logEntrySRFR["PSEPath"] = Path.GetFileName(filePath);
                logEntrySRFR["TotalRecords"] = file.PageCount.ToString();
                
                SQFilesystemConnector.SaveFileResult saveFileResult = await FilesystemConnector.SaveFile(filePath, file.Data);
                Log.FilePath = Log.FilePath.Replace(SettingsJson, RecordsJson);
                Log.Append(logEntrySRFR);
                Log.FilePath = GetDirName(Log.FilePath, saveFolder);
            }

            StartUpload(UploadExe, saveFolder, "pse").ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public string GetDirName(string dir,string repStr)
        {
            dir = Path.GetDirectoryName(dir);

            return dir.Replace(repStr, string.Empty).Trim();
        }
        public string GetNewGuid()
        {
            return Guid.NewGuid().ToString();
        }
        private async Task StartUpload(string exePath,string batchID,string archiver)
        {
            Process p = new Process();




            p.StartInfo.FileName = exePath;
            if(!(ShowCmdWindow))
                p.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
            p.StartInfo.Arguments = string.Format("/batchid:{0} /archiver:{1}",batchID,archiver);
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

