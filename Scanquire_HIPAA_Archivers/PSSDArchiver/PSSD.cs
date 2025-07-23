using EdocsUSA.Utilities;
using EdocsUSA.Utilities.Extensions;
using EdocsUSA.Utilities.Logging;
using Scanquire.Public;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.PeerToPeer;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Edocs.Ocr.Convert.Libaray.Img.PDF;
using System.Security.Policy;
using Microsoft;
using Newtonsoft.Json;
using System.Net.Http;
namespace PSSDArchiver
{
    public class TrackingIDs
    {
        public string TrackingID
        { get; set; }
    }
    public class PSSD : SQFilesystemArchiver
    {
        public PSSDDialog InputDialog = new PSSDDialog();
        private readonly string RecordsJson = "_records.json";
        private readonly string SettingsJson = "_settings.json";
        readonly string MyStrQuote = "\"";
        public string Department
        { get; set; }
        public string OrigDepartment
        { get; set; }
        public string OCRApiKey
        { get; set; }
        public string OCRWebApi
        { get; set; }
        public bool OCRISTable
        { get; set; }
        public string RootPath
        {
            get;
            set;
        }
        public string SaveRootPath
        { get; set; }
        public string SaveFolderName
        { get; set; }
        public string ImagesSaveFolder
        { get; set; }
        public string DisPlayArchiverName
        { get; set; }
        public bool IncludeBlankDocs
        { get; set; }
        public bool ShowTotalDocsScanned
        { get; set; }
        public string UpLoadFolder
        { get; set; }
        public bool ShowCmdWindow
        { get; set; }
        public string UploadExe
        { get; set; }
        public string UploadExeParms
        { get; set; }
        public string InventoryTrackingApiUrl
        { get; set; }
        public string InventoryTrackingSP
        { get; set; }
        public string EdocsCustomerID
        { get; set; }
        public int TotalImagesSaved
        { get; set; }
        public string TrackinUpLoadController
        {
            get; set;
        }
        public bool OCRRecords
        { get; set; }
        public string GetTrackinController
        {
            get; set;
        }
        public string TransferByTrackIDController
        { get; set; }
        public override IEnumerable<System.Threading.Tasks.Task<SQImage>> AcquireFromScannerForNew(IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken)
        {


            //  InputDialog.TryShowDialog(DialogResult.OK);
            return base.AcquireFromScannerForNew(progress, cToken);
        }
        //public override async Task Send(SQDocument document, int documentNumber, IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken)
        //{

        //    if (InputDialog.TryShowDialog(DialogResult.OK) != DialogResult.OK)
        //        throw new OperationCanceledException();

        //}
        public override IEnumerable<System.Threading.Tasks.Task<SQImage>> AcquireFromFile(IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken)
        {

            throw new Exception("Invalid Option");
            //  InputDialog.TryShowDialog(DialogResult.OK);
            //   return base.AcquireFromScannerForNew(progress, cToken);
        }
        public override async System.Threading.Tasks.Task Send(IEnumerable<SQImage> images, IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken)
        {
            //InputDialog.Department = Department;
            //   InputDialog.OrigDepartment = OrigDepartment;
            string checkingMess = $"Getting Archive Information";

            progress.Report(new ProgressEventArgs(0, 0, checkingMess));
            //progress.Report(new EdocsUSA.Utilities.ProgressEventArgs(0, 0, checkingMess));
            await LoadImage(images, cToken, progress);
            await base.Send(images, progress, cToken);
            //    await SaveImage(images, cToken, progress);
            //  if (InputDialog.TryShowDialog(DialogResult.OK) != DialogResult.OK)
            //   throw new OperationCanceledException();

        }
        public async override Task Send(SQFile file, int fileNumber, IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cancelToken)
        {
            InputDialog.Department = Department;
            InputDialog.OrigDepartment = OrigDepartment;
            if ((InputDialog.ListTrackingIDS == null) || (InputDialog.ListTrackingIDS.Count == 0))
                InputDialog.ListTrackingIDS = await GetTrackingIDS();
            if (InputDialog.TryShowDialog(DialogResult.OK) != DialogResult.OK)
                throw new OperationCanceledException();

            Dictionary<string, string> settingsJs = new Dictionary<string, string>();
            Dictionary<string, string> recordsJs = new Dictionary<string, string>();

            // settingsJs["ScanDate"] = DateTime.Now.ToString();
            settingsJs["ScanOperator"] = Environment.UserName;
            //  settingsJs["ScanMachine"] = Environment.MachineName;
            settingsJs["InventoryTrackingApiUrl"] = InventoryTrackingApiUrl;
            settingsJs["InventoryTrackingSP"] = InventoryTrackingSP;
            //   settingsJs["UploadApiUrl"] = BSBProdDepUploadApiUrl;
            //  settingsJs["UpLoadController"] = BSBProdDepUpLoadController;
            // settingsJs["UploadFolder"] = UpLoadFolder.Replace("{ProjectName}", $"{InputDialog.BoxNumber}\\{upLoadDir}");
            //   settingsJs["UploadFolder"] = UpLoadFolder.Replace("{ProjectName}", $"{InputDialog.BoxNumber}\\");
            settingsJs["UploadFolder"] = UpLoadFolder;
            settingsJs["EdocsCustomerID"] = EdocsCustomerID;
            settingsJs["TrackinUpLoadController"] = TrackinUpLoadController;
            settingsJs["TransferByTrackIDController"] = TransferByTrackIDController;
            settingsJs["OCRRecords"] = OCRRecords.ToString();



            recordsJs["Department"] = InputDialog.RecordDepartment;
            recordsJs["OrginationDepartment"] = InputDialog.OrgDepartment;
            recordsJs["DateOfRecords"] = $"{InputDialog.DatetRecsSDate.ToString("MM.dd.yyyy")}-{InputDialog.DatetRecsEDate.ToString("MM.dd.yyyy")}";
            recordsJs["DescriptionOfRecords"] = InputDialog.DescRecords;
            recordsJs["MethOfFiling"] = InputDialog.MethFiling;
            recordsJs["FirsName"] = InputDialog.Fname;
            recordsJs["LastName"] = InputDialog.Lname;

            recordsJs["DateOfBirth"] = InputDialog.DateDOB.ToString("MM.dd.yyyy");
            string SaveFolderName = $@"{InputDialog.RecordDepartment}\{InputDialog.OrgDepartment}\{InputDialog.BoxNumTrackID}\{InputDialog.DatetRecsSDate.ToString("MM.dd.yyyy")}-{InputDialog.DatetRecsEDate.ToString("MM.dd.yyyy")}_{InputDialog.DescRecords}_{InputDialog.MethFiling}";
            string pdfFileName = $"{InputDialog.Lname}_{InputDialog.Fname}_{InputDialog.DateDOB.ToString("MM.dd.yyyy")}";
            recordsJs["PDFFileName"] = $"{pdfFileName}.pdf";
            Log.FilePath = Path.Combine(Path.Combine(SaveRootPath, SaveFolderName), $"{pdfFileName}{RecordsJson}");
            Log.Append(recordsJs);
            settingsJs["ScanBatchID"] = InputDialog.BoxNumTrackID;

            settingsJs["NumberDocsScanned"] = InputDialog.TotalScanned;
            settingsJs["NumberDocsUploaded"] = file.PageCount.ToString();
            settingsJs["OCRImageFolder"] = ImagesSaveFolder;
            if (OCRRecords)
            {
                settingsJs["NumberImagesSaved"] = TotalImagesSaved.ToString();

                settingsJs["NumberDocOCR"] = TotalImagesSaved.ToString();
            }
            else
            {
                settingsJs["NumberImagesSaved"] = file.PageCount.ToString();

                settingsJs["NumberDocOCR"] = file.PageCount.ToString();

            }


            int totalChar = InputDialog.RecordDepartment.Length + InputDialog.OrgDepartment.Length + InputDialog.DescRecords.Length + InputDialog.MethFiling.Length + InputDialog.DatetRecsEDate.ToString().Length + InputDialog.Lname.Length + InputDialog.Fname.Length + InputDialog.DateDOB.ToString().Length + InputDialog.BoxNumTrackID.Length + InputDialog.DatetRecsSDate.ToString().Length;
            Log.FilePath = Path.Combine(Path.Combine(SaveRootPath, SaveFolderName), $"{pdfFileName}{SettingsJson}");
            settingsJs["NumberTypedPerDoc"] = totalChar.ToString();
            Log.Append(settingsJs);
            string saveFolderName = Path.Combine(SaveRootPath, SaveFolderName);
            File.WriteAllBytes(Path.Combine(saveFolderName, $"{pdfFileName}.pdf"), file.Data);
            //  if (TotalImagesSaved != file.PageCount)
            //  {
            //      throw new OperationCanceledException($"Error: saving images for OCR Total images exported {TotalImagesSaved} not samae as total pages {file.PageCount}");
            //    }
            await StartUpload(pdfFileName, "PSUSD", $"{MyStrQuote}{saveFolderName}{MyStrQuote}");
        }
        private async Task<List<string>> GetTrackingIDS()
        {
            List<string> retList = new List<string>();
            try
            {

                string cont = GetTrackinController;
                if (!(cont.EndsWith("/")))
                    cont = $"{cont}/";

                using (var client = new HttpClient())
                {
                    if (!(InventoryTrackingApiUrl.EndsWith("/")))
                        InventoryTrackingApiUrl = $"{InventoryTrackingApiUrl}/";
                    client.BaseAddress = new Uri(InventoryTrackingApiUrl);
                    client.Timeout = TimeSpan.FromMinutes(10);
                    var responseTask = client.GetAsync($"{client.BaseAddress.AbsoluteUri}{cont}{EdocsCustomerID}");
                    responseTask.Wait();
                    var results = responseTask.Result;
                    if (results.IsSuccessStatusCode)
                    {
                        var readTask = results.Content.ReadAsStringAsync();
                        readTask.Wait();
                        //var trackID = JsonConvert.DeserializeObject<List<TrackingID>>(readTask.Result);
                        List<TrackingIDs> trackID = JsonConvert.DeserializeObject<List<TrackingIDs>>(readTask.Result);
                        //string responseBody = client.Content.ReadAsStriAsync().ConfigureAwait(true).GetAwaiter().GetResult();
                        //  var trackID = JsonConvert.DeserializeObject<List<TrackingID>>(readTask.Result);
                        foreach (TrackingIDs id in trackID)
                            //Console.WriteLine();
                            retList.Add(id.TrackingID);

                        return retList;
                        //   RetMess retStr = readTask.Result[0] as RetMess;
                        //  return retStr.ReturnMessage;
                    }


                }

            }
            catch (Exception ex)

            {
            }
            return retList;
        }
        private async Task LoadImage(IEnumerable<SQImage> images, System.Threading.CancellationToken cToken, IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress)
        {
            try
            {
                
                string checkingMess = $"Getting Archive Information";
                progress.Report(new EdocsUSA.Utilities.ProgressEventArgs(0, 0, checkingMess));
                  if(OCRRecords)
                 SaveImage(images, cToken, progress);
               // System.Threading.Thread.Sleep(5000);
                //   SQPage sQPage = document.Pages[0];
                //    SQImage image = sQPage.Image;
                //   SQImageEditLock image_Lock = image.BeginEdit();
                //   recordDialog.DOHImage.ClearAll(false, true);
                //   recordDialog.DOHImage.Add(image);
                //  recordDialog.DOHImage.NavigateTo(0);
                //   recordDialog.DOHImage.ActiveImageViewer.ScaleToFitHeight();
                //  image.DiscardEdit(image_Lock);
                System.Drawing.Bitmap bitmap = null;
                Uri uri = new Uri(OCRWebApi);
                foreach (SQImage page in images)
                {
                    // SQImage image = page.Image;
                    // SQImageEditLock image_Lock = image.BeginEdit();
                    page.BeginEdit();
                    InputDialog.PSDImage.ClearAll(false, false);

                    InputDialog.PSDImage.Add(page);
                    InputDialog.PSDImage.NavigateTo(0);
                    InputDialog.PSDImage.ActiveImageViewer.ScaleToFitHeight();
                    bitmap = page.LatestRevision.GetOriginalImageBitmap();
                    if (OCRRecords)
                    {


                        byte[] imageByte = ConvertImagePDF.ImageToBase64(page.LatestRevision.GetOriginalImageBitmap(), System.Drawing.Imaging.ImageFormat.Png);

                        string OCRResults = await ConvertImagePDF.OCRSrace(imageByte, true, false, OCRApiKey, "2", OCRISTable, uri, "image.png");
                        if (!(string.IsNullOrWhiteSpace(OCRResults)))
                        {
                            if (OCRResults.ToLower().Contains("keira"))
                            {
                                InputDialog.Fname = "Keira";
                                InputDialog.Lname = "Edgar";

                            }
                        }
                    }

                    break;
                }
                // await SaveImage(images, cToken, progress);
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Error loading Image {ex.Message}");
                //throw new OperationCanceledException($"Error loading Image {ex.Message}");
            }
        }
        private async Task SaveImage(IEnumerable<SQImage> images, System.Threading.CancellationToken cToken, IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress)
        {
            try
            {
                string checkingMess = $"Getting Archive Information";
                progress.Report(new EdocsUSA.Utilities.ProgressEventArgs(0, 0, checkingMess)); ;
                //   SQPage sQPage = document.Pages[0];
                //    SQImage image = sQPage.Image;
                //   SQImageEditLock image_Lock = image.BeginEdit();
                //   recordDialog.DOHImage.ClearAll(false, true);
                //   recordDialog.DOHImage.Add(image);
                //  recordDialog.DOHImage.NavigateTo(0);
                //   recordDialog.DOHImage.ActiveImageViewer.ScaleToFitHeight();
                //  image.DiscardEdit(image_Lock);
                //   System.Drawing.Bitmap bitmap = null;
                Uri uri = new Uri(OCRWebApi);
                ImagesSaveFolder = $"OCRImages\\{GetNewGuid()}";
                TotalImagesSaved = 0;
                ImagesSaveFolder = Path.Combine(SaveRootPath, ImagesSaveFolder);
                Edocs_Utilities.EdocsUtilitiesInstance.CreateDir(ImagesSaveFolder);
                foreach (SQImage page in images)
                {
                    // SQImage image = page.Image;
                    // SQImageEditLock image_Lock = image.BeginEdit();
                    page.BeginEdit();
                    //InputDialog.PSDImage.ClearAll(false, false);
                    System.Drawing.Bitmap bitmap = page.LatestRevision.GetOriginalImageBitmap();
                    string imgFolder = System.IO.Path.Combine(ImagesSaveFolder, $"{Path.GetFileNameWithoutExtension(page.LatestRevision.OriginalImageFilePath)}.png");
                    bitmap.Save(imgFolder, System.Drawing.Imaging.ImageFormat.Png);
                    TotalImagesSaved++;
                    //InputDialog.PSDImage.Add(page);
                    // InputDialog.PSDImage.NavigateTo(0);
                    //InputDialog.PSDImage.ActiveImageViewer.ScaleToFitHeight();
                    // byte[] imageByte = ConvertImagePDF.ImageToBase64(page.LatestRevision.GetOriginalImageBitmap(), System.Drawing.Imaging.ImageFormat.Png);
                    //string OCRResults = await ConvertImagePDF.OCRSrace(imageByte, true, false, OCRApiKey, "2", OCRISTable, uri, "image.png");
                    //bitmap = page.LatestRevision.GetOriginalImageBitmap();

                }
            }
            catch (Exception ex)
            {
                throw new OperationCanceledException($"Error loading Image {ex.Message}");
            }
        }
        public string GetNewGuid()
        {
            return Guid.NewGuid().ToString();
        }
        private async Task StartUpload(string batchid, string archive, string afolder)
        {
            try
            {
                // uploadFolder = uploadFolder.Replace("{ProjectName}", "").Trim();
                Process p = new Process();

                p.StartInfo.FileName = UploadExe;
                if (!(ShowCmdWindow))
                    p.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
                p.StartInfo.Arguments = string.Format(UploadExeParms, batchid, afolder, archive);
                //    p.StartInfo.Arguments = string.Format(UploadExeParms, settingFile);
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
                //    EdocsUSA.Utilities.Logging.TraceLogger.TraceLoggerInstance.TraceError($"running process {UploadExe} with parms {string.Format(UploadExeParms, settingFile)} {ex.Message}");

                //    MessageBox.Show($"Process {UploadExe} with parms {string.Format(UploadExeParms, settingFile)} {ex.Message}  did not start", "Error Uploading", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw new Exception($"Error uploading {ex.Message}");
            }
        }
    }

}
