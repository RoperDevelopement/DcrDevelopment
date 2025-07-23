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
using System.IO.Compression;
using Microsoft;
//https://mozilla.github.io/pdf.js/examples/ https://www.aspsnippets.com/Articles/ASPNet-Core-Razor-Pages-Display-PDF-files-from-Database-in-View.aspx
namespace Edocs.Columbia_River_Gorge.Archiver
{
    public class CRGArchiver : SQFilesystemArchiver
    {
        private readonly string RecordsJson = "_records.json";
        private readonly string SettingsJson = "_settings.json";
        CRGAArchiverDialog InputDialog = new CRGAArchiverDialog();
        public string DisPlayArchiverName
        { get; set; }
        public bool IncludeBlankDocs
        { get; set; }
        public bool ShowTotalDocsScanned
        { get; set; }
        public string SaveRootPath
        { get; set; }
        public string OCRApiKey
        { get; set; }
        public string OCRWebApi
        { get; set; }
        public bool OCRISTable
        { get; set; }
        public bool ShowConfirmDialogBox
        { get; set; }
        public string UploadExe
        { get; set; }
        public string UploadExeParms
        { get; set; }
        public bool ShowCmdWindow
        { get; set; }
        public string InventoryTrackingApiUrl { get; set; }
        public string InventoryTrackingSP { get; set; }
        public string CRGUploadApiUrl { get; set; }
        public string CRGrodDepUpLoadController { get; set; }
        public string InventoryTrackingUpLoadController { get; set; }
        public string OcrFolder { get; set; }
        private string OcrOutFolder { get; set; }
        public int CustomerID
        { get; set; }
        public bool ZipDownLoadFolder
        { get; set; }
        public string OCRSearchblePDF
        { get; set; }
        public override IEnumerable<System.Threading.Tasks.Task<SQImage>> AcquireFromScannerForNew(IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken)
        {


            //  InputDialog.TryShowDialog(DialogResult.OK);
            return base.AcquireFromScannerForNew(progress, cToken);
        }
        public override async System.Threading.Tasks.Task Send(IEnumerable<SQImage> images, IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken)
        {
            //Make sure optix is running

            SaveImages(images,progress, cToken).ConfigureAwait(false).GetAwaiter().GetResult();

            await base.Send(images, progress, cToken);
        }
        public async override Task Send(SQFile file, int fileNumber, IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cancelToken)
        {
            // FileNumberArchiver(file, fileNumber, progress, cancelToken).ConfigureAwait(false).GetAwaiter().GetResult();
            InputDialog.Name = DisPlayArchiverName;
            InputDialog.TryShowDialog(DialogResult.OK);
            Dictionary<string, string> recordsJs = new Dictionary<string, string>();
            Dictionary<string, string> settingsJs = new Dictionary<string, string>();
            string saveFolder = InputDialog.FileName;
            if ((string.IsNullOrWhiteSpace(saveFolder)))
                saveFolder = InputDialog.ParcelNumLot;

            string dirPath = Path.Combine(SaveRootPath, $"{saveFolder}");
            Log.FilePath = Path.Combine(dirPath, $"{saveFolder}{SettingsJson}");
            string fileNameWithExt = Path.ChangeExtension(saveFolder, file.FileExtension);

            settingsJs["ScanBatchID"] = GetNewGuid();
            settingsJs["EdocsCustomerID"] = CustomerID.ToString();
            settingsJs["ScanDate"] = DateTime.Now.ToString();
            settingsJs["ScanOperator"] = Environment.UserName;
            settingsJs["TotalScanned"] = InputDialog.ImagesScanned;
            settingsJs["TotalPageCount"] = file.PageCount.ToString();
            settingsJs["InventoryTrackingApiUrl"] = InventoryTrackingApiUrl;
            settingsJs["InventoryTrackingSP"] = InventoryTrackingSP;
             
            settingsJs["CRGUploadApiUrl"] = CRGUploadApiUrl;
            settingsJs["CRGrodDepUpLoadController"] = CRGrodDepUpLoadController;
            settingsJs["InventoryTrackingID"] = saveFolder;
            settingsJs["InventoryTrackingUpLoadController"] = InventoryTrackingUpLoadController;
            settingsJs["TotalCharTyped"] = saveFolder.Length.ToString();
            settingsJs["ImagesFoder"] = dirPath;
            settingsJs["InventoryTrackingUpLoadController"] = InventoryTrackingUpLoadController;
            settingsJs["OCRFolder"] = OcrOutFolder;
            
         
            settingsJs["PdfSavedFile"] = Path.Combine(dirPath, fileNameWithExt);
            settingsJs["ZipDownLoadFolder"] = ZipDownLoadFolder.ToString();
            settingsJs["OCRSearchblePDF"] = OCRSearchblePDF;
            
            Log.Append(settingsJs);
            //  settingsJs["ScanMachine"] = Environment.MachineName;

            if (!(string.IsNullOrWhiteSpace(InputDialog.ParcelNumLot)))

                recordsJs["ParcelNumberTaxLot"] = InputDialog.ParcelNumLot;
            else
                recordsJs["FileNumber"] = InputDialog.FileName;
           
            Log.FilePath = Path.Combine(dirPath, $"{saveFolder}{RecordsJson}");

            Log.Append(recordsJs);
           
            SQFilesystemConnector.SaveFileResult saveFileResult = await FilesystemConnector.SaveFile(Path.Combine(dirPath, fileNameWithExt), file.Data);
            StartUpload(Path.Combine(dirPath, $"{saveFolder}{SettingsJson}")).ConfigureAwait(false).GetAwaiter().GetResult();
            //https://csharp.hotexamples.com/examples/-/ICSharpCode.SharpZipLib.Zip.ZipFile/-/php-icsharpcode.sharpziplib.zip.zipfile-class-examples.html
            //  ICSharpCode.SharpZipLib.Zip.ZipFile zf = null;

        }
        private async Task SaveImages(IEnumerable<SQImage> images, IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken)
        {
            //foreach (SQImage page in document.)
            // {



            OcrOutFolder = Path.Combine(string.Format(OcrFolder, GetNewGuid()));
            Edocs_Utilities.EdocsUtilitiesInstance.CreateDir(OcrOutFolder);
            string checkingMess = $"Saving Images";
            progress.Report(new ProgressEventArgs(1, images.Count(), checkingMess));
            foreach (SQImage img in images)
            {
                try
                {

                    img.BeginEdit();
                    progress.Report(new EdocsUSA.Utilities.ProgressEventArgs(1, images.Count(), checkingMess));

                    System.Drawing.Bitmap bitmap = img.LatestRevision.GetOriginalImageBitmap();
                    string imgFolder = System.IO.Path.Combine(OcrOutFolder, $"{Path.GetFileNameWithoutExtension(img.LatestRevision.OriginalImageFilePath)}.png");
                    bitmap.Save(imgFolder, System.Drawing.Imaging.ImageFormat.Png);
                    //GetOcrText(imgFolder).ConfigureAwait(false).GetAwaiter().GetResult();
                }
                catch { }



            }
        }
           
              
        public string GetNewGuid()
        {
            return Guid.NewGuid().ToString();
        }
        private async Task StartUpload(string settingFile)
        {
            try
            {
                // uploadFolder = uploadFolder.Replace("{ProjectName}", "").Trim();
                Process p = new Process();

                p.StartInfo.FileName = UploadExe;
                if (!(ShowCmdWindow))
                    p.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
                // p.StartInfo.Arguments = string.Format("/batchid:{0} /archiver:{1}", batchID, archiver);
                p.StartInfo.Arguments = string.Format(UploadExeParms,settingFile);
                EdocsUSA.Utilities.Logging.TraceLogger.TraceLoggerInstance.TraceInformation($"Starting process {p.StartInfo.FileName} with parms {p.StartInfo.Arguments}");
                p.Start();
                // Task.Factory.StartNew(() => p.WaitForExit()).ConfigureAwait(false).GetAwaiter().GetResult();
                // p.WaitForExit();
                //if (p.ExitCode != 0)
                //{
                //    EdocsUSA.Utilities.Logging.TraceLogger.TraceLoggerInstance.TraceError($"running process {p.StartInfo.FileName} with parms {p.StartInfo.Arguments} exit code {p.ExitCode.ToString()}");
                //    EdocsUSA.Utilities.Logging.TraceLogger.TraceLoggerInstance.TraceError($"running process {UploadExe} with parms  {settingFile}");
                //    MessageBox.Show($"Process {UploadExe} with parms {settingFile}  did not start", "Error Uploading", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    throw new Exception(p.ExitCode.ToString());
                //}
            }
            catch (Exception ex)
            {
                 EdocsUSA.Utilities.Logging.TraceLogger.TraceLoggerInstance.TraceError($"running process {UploadExe} with parms {string.Format(UploadExeParms, settingFile)} {ex.Message}");
                    
                    MessageBox.Show($"Process {UploadExe} with parms {string.Format(UploadExeParms, settingFile)} {ex.Message}  did not start", "Error Uploading", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw new OperationCanceledException(ex.Message);
            }
        }
    }
}
