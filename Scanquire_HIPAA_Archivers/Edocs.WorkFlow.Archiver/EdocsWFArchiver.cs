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
using Edocs.WorkFlow.Archiver.ArchiverForms;
using Edocs.WorkFlow.Archiver.Models;
using Edocs.WorkFlow.Archiver.WFApi;
using Edocs.WorkFlow.Archiver.InterFaces;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Edocs.Ocr.Convert.Libaray.Img.PDF;

using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Security.Policy;
using Microsoft;
using Newtonsoft.Json.Linq;
using DebenuPDFLibraryDLL0915;
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;

namespace Edocs.WorkFlow.Archiver
{
    public class EdocsWFArchiver : SQFilesystemArchiver
    {
        DialogArchiver InputDialog = new DialogArchiver();
        DialogWFUsers DialogWFUsers = new DialogWFUsers();
        private readonly string RecordsJson = "_records.json";
        private readonly string SettingsJson = "_settings.json";
        private readonly string SettingsWFJson = "_wf_users.json";

        public string OCRApiKey
        { get; set; }
        public string OCRWebApi
        { get; set; }
        public string WorkFlowApi
        { get; set; }
        public string WFUsersController
        { get; set; }
        private string WFUsersId
        { get; set; }
        public string RootPath
        { get; set; }
        public string AzureShareName
        { get; set; }
        public string AzureDataBaseName
        { get; set; }
        public string UploadExe
        { get; set; }
        public string SpWFEmpId
        { get; set; }
        public string SpEmpInfo
        { get; set; }
        public bool ShowCmdWindow
        { get; set; }
        private string UploadExeParms
        { get; set; }
        public bool UpLoadPDF
        { get; set; }
        public bool OCRISTable
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
                
                string upLoadDir = string.Empty;
                string saveFolder = GetNewGuid();
                string dirPath = Path.Combine(RootPath, saveFolder);
                EdocsUSA.Utilities.Edocs_Utilities.EdocsUtilitiesInstance.CreateDir(dirPath);
await GetWfUsers();
                 await OCRPDF(file, progress, cancelToken);
                DialogResult dr = DialogWFUsers.ShowDialog();
if (dr == DialogResult.OK)
                {
                    if (!(string.IsNullOrWhiteSpace(DialogWFUsers.SelectUsersId)))
                        await GetUserIds(dirPath, saveFolder);
                }
                InputDialog.TryShowDialog(DialogResult.OK);
                EdocsUSA.Utilities.Edocs_Utilities.EdocsUtilitiesInstance.CreateDir(dirPath);
                //if (!(System.IO.Directory.Exists(dirPath)))
                //    System.IO.Directory.CreateDirectory(dirPath);
                //  string fileNameWithExt = Path.ChangeExtension(fileName, file.FileExtension);
                await SaveEmpInfo(dirPath, saveFolder);
                Dictionary<string, string> batchSettings = new Dictionary<string, string>();


                batchSettings["ScanBatchID"] = saveFolder;
                batchSettings["ScanDate"] = DateTime.Now.ToString();
                batchSettings["ScanOperator"] = Environment.UserName;
                batchSettings["ScanMachine"] = Environment.MachineName;
                batchSettings["AzureWebApiController"] = WorkFlowApi;
                batchSettings["WFUsersController"] = WFUsersController;

                batchSettings["AzureDataBaseName"] = AzureDataBaseName;
                batchSettings["SpWFEmpId"] = SpWFEmpId;
                batchSettings["SpEmpInfo"] = SpEmpInfo;
                batchSettings["AzureShareName"] = AzureShareName;





                Log.FilePath = Path.Combine(dirPath, $"{saveFolder}{SettingsJson}");
                Log.Append(batchSettings);
                string fileNameWithExt = $"{saveFolder}.pdf";
                string filePath = Path.Combine(dirPath, fileNameWithExt);

                SQFilesystemConnector.SaveFileResult saveFileResult = await FilesystemConnector.SaveFile(filePath, file.Data);
                StartUpload(saveFolder, dirPath).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }
        private async Task StartUpload(string batchID, string uploadFolder)
        {
            try
            {
                if (UpLoadPDF)
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
                        //  EdocsUSA.Utilities.Logging.TraceLogger.TraceLoggerInstance.TraceError($"running process {p.StartInfo.FileName} with parms {p.StartInfo.Arguments} exit code {p.ExitCode.ToString()}");
                        //  EdocsUSA.Utilities.Logging.TraceLogger.TraceLoggerInstance.TraceError($"running process {UploadExe} with parms {batchID} {uploadFolder}");
                        MessageBox.Show($"Process {UploadExe} with parms {batchID} {uploadFolder} did not start", "Error Uploading", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        throw new Exception(p.ExitCode.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                throw new OperationCanceledException(ex.Message);
            }

        }

        //private async Task OCRImages(IEnumerable<SQImage> images, IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken)
        //        private async Task OCRPDF(SQFile file, IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken)

        //        {

        //            Uri uri = new Uri(OCRWebApi);
        //            //SQFilesystemConnector.SaveFileResult saveFileResult = await FilesystemConnector.SaveFile(@"l:\testpdf.pdf", file.Data);
        //            PDFLibrary pdf = new PDFLibrary();
        //            pdf.SetOrigin(0);

        //            //  pdf.loLoadFromFile(pdfFileName, pdfPassWord);
        //            pdf.LoadFromString(file.Data, string.Empty);
        //            int iNumPages = pdf.PageCount();
        //            string OcrText = string.Empty;
        //          //  pdfFileName = Path.GetFileNameWithoutExtension(pdfFileName);

        //            //    string bmpSaveFileName = Path.Combine(saveImgDir, $"{pdfFileName}.tif");
        //        //    string bmpSaveFileName = Path.Combine(saveImgDir, $"{pdfFileName}.{extension}");


        //            pdf.RenderDocumentToFile(100, 1, iNumPages, 0, @"L:\ImagesEmp\empimg.png");
        //            pdf.ReleaseLibrary();
        //            foreach(string images in Directory.GetFiles(@"L:\ImagesEmp\","empimg*.png"))
        //{
        //                Image imageFileByte = Image.FromFile(images);

        //                byte[] imageByte = ConvertImagePDF.ImageToBase64(imageFileByte, System.Drawing.Imaging.ImageFormat.Png);
        //               OcrText = await ConvertImagePDF.OCRSrace(imageByte, true, false, OCRApiKey, "1", false, uri, "image.png");
        //            }
        //            //     string OCRResults = await Ocr.Convert.Libaray.Img.PDF.ConvertImagePDF.OCRSrace(@"l:\testpdf.pdf", false, false, OCRApiKey,"1", true, uri);

        //        }

        private async Task OCRPDF(SQFile file, IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken)

        {

            Uri uri = new Uri(OCRWebApi);
            //SQFilesystemConnector.SaveFileResult saveFileResult = await FilesystemConnector.SaveFile(@"l:\testpdf.pdf", file.Data);
            PDFLibrary pdf = new PDFLibrary();
            pdf.SetOrigin(0);

            //  pdf.loLoadFromFile(pdfFileName, pdfPassWord);
            pdf.LoadFromString(file.Data, string.Empty);
            int iNumPages = pdf.PageCount();
            string OcrText = string.Empty;
            //  pdfFileName = Path.GetFileNameWithoutExtension(pdfFileName);

            //    string bmpSaveFileName = Path.Combine(saveImgDir, $"{pdfFileName}.tif");
            //    string bmpSaveFileName = Path.Combine(saveImgDir, $"{pdfFileName}.{extension}");

            int pageCount = pdf.PageCount();
            //  pdf.RenderDocumentToFile(100, 1, iNumPages, 0, @"L:\ImagesEmp\empimg.png");

            // foreach (string images in Directory.GetFiles(@"L:\ImagesEmp\", "empimg*.png"))
            // {
            //   Image imageFileByte = Image.FromFile(images);

            //   byte[] imageByte = ConvertImagePDF.ImageToBase64(imageFileByte, System.Drawing.Imaging.ImageFormat.Png);
            //   OcrText = await ConvertImagePDF.OCRSrace(imageByte, true, false, OCRApiKey, "1", false, uri, "image.png");
            //  }
            //     string OCRResults = await Ocr.Convert.Libaray.Img.PDF.ConvertImagePDF.OCRSrace(@"l:\testpdf.pdf", false, false, OCRApiKey,"1", true, uri);
            for (int currentPageIndex = 1; currentPageIndex <= pageCount; currentPageIndex++)
            {
                using (var ms = new MemoryStream(pdf.RenderPageToString(100, currentPageIndex, 0)))
                {
                    System.Drawing.Image imageFileByte = System.Drawing.Image.FromStream(ms);
                       byte[] imageByte = ConvertImagePDF.ImageToBase64(imageFileByte, System.Drawing.Imaging.ImageFormat.Png);
                       OcrText += await ConvertImagePDF.OCRSrace(imageByte, true, true, OCRApiKey, "2", true, uri, "image.png");
                    //  Image imageFileByte = Image.FromStream(ms);
                }
            }
            
            pdf.ReleaseLibrary();
        }
        private string GetNewGuid()
        {
            return Guid.NewGuid().ToString();
        }
        private async Task GetUserIds(string dirPath, string saveFolder)
        {
            WFUsersId = string.Empty;
            saveFolder = $"{saveFolder}{SettingsWFJson}";
            dirPath = Path.Combine(dirPath, saveFolder);
            foreach (var ids in DialogWFUsers.SelectUsersId.Split('*'))
            {
                if (!(string.IsNullOrWhiteSpace(ids)))
                {
                    foreach (var flName in DialogWFUsers.UsersWF)
                    {

                        string compName = $"{flName.Value.FName} {flName.Value.LName}";
                        if (string.Compare(ids, compName, true) == 0)
                        {
                            // WFUsersId += $"{flName.Key} ";
                            await SaveEmpIDs(dirPath, flName.Key);
                            break;
                        }
                    }
                }

            }
            if (!(string.IsNullOrWhiteSpace(WFUsersId)))
                WFUsersId = WFUsersId.Trim();
        }
        private async Task SaveEmpIDs(string dirPath, int empId)
        {

            WFUservVIewL ID = new WFUservVIewL();

            ID.EmployeeID = empId;
            var empID = JsonConvert.SerializeObject(ID);
            empID = $"{empID}\r\n";
            File.AppendAllText(dirPath, empID);
        }
        private async Task SaveEmpInfo(string dirPath, string saveFolder)
        {
            saveFolder = $"{saveFolder}{RecordsJson}";
            dirPath = Path.Combine(dirPath, saveFolder);
            EmployeeModel employee = new EmployeeModel();
            employee.EmpCellPhone = InputDialog.EmpCellPhone;
            employee.Comments = InputDialog.Comments;
            employee.EmpAddress = InputDialog.EmpAddress;
            employee.EmpCity = InputDialog.EmpCity;
            employee.EmpEmail = InputDialog.EmpEmail;
            employee.EmpFName = InputDialog.EmpFName;
            employee.EmpHomePhone = InputDialog.EmpHomePhone;
            employee.EmpLName = InputDialog.EmpLName;
            employee.EmpPay = InputDialog.EmpPay;
            employee.EmpState = InputDialog.EmpState;
            employee.EmpStDate = InputDialog.EmpStDate;
            employee.EmpZip = InputDialog.EmpZip;
            employee.EmpSSN = InputDialog.EmpSSN;

            employee.PosApply = InputDialog.PosApply;
            var empData = JsonConvert.SerializeObject(employee);
            File.WriteAllText(dirPath, empData);
        }

        private async Task GetWfUsers()
        {
            if (DialogWFUsers.UsersWF == null)
                DialogWFUsers.UsersWF = new Dictionary<int, WFUsersModel>();
            if (DialogWFUsers.UsersWF.Count == 0)
                DialogWFUsers.UsersWF = WebApi.GetWFUsers(WorkFlowApi, WFUsersController).ConfigureAwait(false).GetAwaiter().GetResult();



        }

    }
}



//InputDialog.TxtDialog = DisPlayArchiverName;
//InputDialog.CmbBoxItems = DropDownItems.Split(',');
//if (bool.TryParse(ShowTotalRecordsDialog, out bool result))
//    InputDialog.ShowTotalRecordsScanned = result;
//InputDialog.TryShowDialog(DialogResult.OK);
//string dirPath = string.Empty;
//string fileName = GetNewGuid();
//string fileNameWithExt = string.Empty;
//string filePath = string.Empty;
//Dictionary<string, string> UploadEntryITS = new Dictionary<string, string>();
//Dictionary<string, string> settingsITS = new Dictionary<string, string>();
//settingsITS["ScanBatch"] = fileName;
//settingsITS["ScanDate"] = DateTime.Now.ToString();
//settingsITS["ScanOperator"] = Environment.UserName;
//settingsITS["ScanMachine"] = Environment.MachineName;
//settingsITS["AzureWebApiController"] = AzureWebApiController;
//settingsITS["AzureShareName"] = AzureShareName;
//settingsITS["WebApi"] = WebApi;

//settingsITS["AzureSPName"] = SpUploadPSEFinancialRecordsJson;
//settingsITS["AzureWebApiITSController"] = AzureWebApiITSController;
//settingsITS["ITSWebAPI"] = ITSWebAPI;
//settingsITS["ITSAzureDataBaseName"] = ITSAzureDataBaseName;
//settingsITS["TrackingID"] = InputDialog.TrackingID;
//settingsITS["SpAddITSDocsScanned"] = SpAddITSDocsScanned;
//settingsITS["TotalScanned"] = InputDialog.TotalRecordsScanned;
//settingsITS["TotalRecordsUploaded"] = file.PageCount.ToString();


//string saveFolder = GetNewGuid();
//if (Path.HasExtension(Log.FilePath))
//    Log.FilePath = Path.GetDirectoryName(Log.FilePath);
//Log.FilePath = Path.Combine(Log.FilePath, saveFolder);
//if (!(Directory.Exists(Log.FilePath)))
//    Directory.CreateDirectory(Log.FilePath);
//dirPath = Path.Combine(SaveRootPath, saveFolder);
//fileNameWithExt = Path.ChangeExtension(fileName, file.FileExtension);
//filePath = Path.Combine(dirPath, fileNameWithExt);
////  Log.FilePath = Path.Combine(Log.FilePath,$"{saveFolder}{RecordsJson}");
//// Log.FilePath = Path.Combine(Log.FilePath, $"{saveFolder}{SettingsJson}");
//Log.FilePath = Path.Combine(Log.FilePath, $"{saveFolder}{SettingsJson}");
//Log.Append(settingsITS);
//UploadEntryITS["StartYear"] = InputDialog.StYear.ToString("MM-dd-yyyy");
//UploadEntryITS["EndYear"] = InputDialog.EndYear.ToString("MM-dd-yyyy");
//UploadEntryITS["FinancialCaterogyName"] = InputDialog.FinIdentifier;

//UploadEntryITS["FileName"] = Path.GetFileName(filePath);
//SQFilesystemConnector.SaveFileResult saveFileResult = await FilesystemConnector.SaveFile(filePath, file.Data);
//Log.FilePath = Log.FilePath.Replace(SettingsJson, RecordsJson);
//Log.Append(UploadEntryITS);
//Log.FilePath = GetDirName(Log.FilePath, saveFolder);
//StartUpload(UploadExe, saveFolder).ConfigureAwait(false).GetAwaiter().GetResult();
//            }
//            catch (Exception ex)
//{ throw new Exception(ex.Message); }
//        }