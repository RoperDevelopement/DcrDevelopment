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

namespace Edocs.School.Archiver
{
    public class SchoolArchiver : SQFilesystemArchiver
    {
        SchoolArchiverDIalog InputDialog = new SchoolArchiverDIalog();
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

                string fileName = $"{InputDialog.EmpFName}_{InputDialog.EmpLName}";
                string fileNameWithExt = $"{fileName}.pdf"; ;
                string filePath = string.Empty;
                Dictionary<string, string> schoolJsonFiles = new Dictionary<string, string>();
                Dictionary<string, string> settingsSchool = new Dictionary<string, string>();
                string saveFolder = $"{InputDialog.EmpFName}_{InputDialog.EmpLName}_{InputDialog.EmpID}";
                string dirPath = Path.Combine(SaveRootPath, saveFolder);
               // fileName = Path.Combine(SaveRootPath, fileName);
                settingsSchool["ScanDate"] = DateTime.Now.ToString();
                settingsSchool["ScanOperator"] = Environment.UserName;
                settingsSchool["ScanMachine"] = Environment.MachineName;
                settingsSchool["UploadFolder"] = "TBA";
                settingsSchool["TotalScanned"] = InputDialog.TotalScanned;
                settingsSchool["TrackingID"] = InputDialog.TrackingID;
                Log.FilePath = Path.Combine(dirPath, $"{fileName}{SettingsJson}");
                Log.Append(settingsSchool);

                schoolJsonFiles["FileName"] = fileNameWithExt;
                schoolJsonFiles["Employee_First_Name"] = InputDialog.EmpFName;
                schoolJsonFiles["Employee_Last_Name"] = InputDialog.EmpLName;
                
               
                
                schoolJsonFiles["TotalPageCount"] = file.PageCount.ToString();
                Log.FilePath = Path.Combine(dirPath, $"{fileName}{RecordsJson}");
                Log.Append(schoolJsonFiles);
                fileName = Path.Combine(dirPath, $"{fileName}.pdf");
                SQFilesystemConnector.SaveFileResult saveFileResult = await FilesystemConnector.SaveFile(fileName, file.Data);
            }
            catch(Exception ex)
            {
                throw new OperationCanceledException(ex.Message);
            }

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
    }
}
