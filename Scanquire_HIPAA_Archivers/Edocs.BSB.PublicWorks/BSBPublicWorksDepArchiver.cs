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
using EdocsUSA.Controls;

namespace Edocs.BSB.PublicWorks.Dep.Archiver
{
    public class BSBPublicWorksDepArchiver : SQFilesystemArchiver
    {
        public BSBPPublicWorksDepArchiverDialog InputDialog = new BSBPPublicWorksDepArchiverDialog();
        private readonly string RecordsJson = "_records.json";
        private readonly string SettingsJson = "_settings.json";
        private readonly string LCROCTxtFile = "_lcrocr.txt";

        private string ImagesSaveFolder
        { get; set; }

        int documentNumber = 0;
        public bool OCRRecords
        { get; set; }
        public string DisPlayArchiverName
        { get; set; }
        public bool IncludeBlankDocs
        { get; set; }
        public bool ShowTotalDocsScanned
        { get; set; }
        public int TotalImagesSaved
        { get; set; }
        public string SaveRootPath
        { get; set; }
        public string UpLoadFolder
        { get; set; }
        public string BSBProdDepUploadApiUrl
        { get; set; }
        public string BSBProdDepUpLoadController
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
        private string saveFolder
        { get; set; }
        public string OCRApiKey
        { get; set; }
        public string OCRWebApi
        { get; set; }
        public bool OCRISTable
        { get; set; }
        public string CmbDepartment
        { get; set; }
        public bool ShowConfirmDialogBox
        { get; set; }
        public string EdocsCustomerID
        { get; set; }
        private bool FoundDate
        { get; set; }
        private StringBuilder SB
        { get; set; }
        public string ArchiverValues
        { get; set; }
        public string LCROCROutPutFolder { get; set; }
        public string LCROCRExe { get; set; }
        public string LCROCRExeParms { get; set; }
        public string LCROCRMineType { get; set; }
        private SQDocument BSBDocument
        { get; set; }
        private JsonFileDictionaryLogger Log
        { get; set; }
        private int DocICROCR
        { get; set; }
        public bool SaveImages
        { get; set; }
        public bool OCRRecordsUpLoad
        { get; set; }
        public bool CreateSearchabelPDF
        { get; set; }
        public string TrackinUpLoadController
        { get; set; }
        public bool WaitForExit
        { get; set; }
        public override IEnumerable<System.Threading.Tasks.Task<SQImage>> AcquireFromScannerForNew(IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken)
        {


            //  InputDialog.TryShowDialog(DialogResult.OK);
            return base.AcquireFromScannerForNew(progress, cToken);
        }
        //public override async Task Send(SQDocument document, int documentNumber, IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken)
        //{
        //    BSBDocument.Pages.Clear();
        //    //  await ICROCRImage(document, documentNumber, progress, cToken, LCROCROutPutFolder);
        //    try
        //    {
        //        foreach (var page in document.Pages)
        //        {
        //            documentNumber++;
        //            await ICROCRImage(page, documentNumber, progress, cToken, LCROCROutPutFolder);
        //            BSBDocument.Pages.Add(page);
        //            await base.Send(BSBDocument, documentNumber, progress, cToken);
        //            BSBDocument.Pages.Clear();

        //        }
        //        StartUpload(saveFolder, SaveRootPath, "BSBPropDep").ConfigureAwait(false).GetAwaiter().GetResult();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new OperationCanceledException();
        //    }


        //}
        public override IEnumerable<System.Threading.Tasks.Task<SQImage>> AcquireFromFile(IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken)
        {

            throw new Exception("");
            //  InputDialog.TryShowDialog(DialogResult.OK);
            //   return base.AcquireFromScannerForNew(progress, cToken);
        }
        //public override async Task Send(IEnumerable<SQImage> images, IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken)
        //{
        //    await base.Send(images, progress, cToken);
        //}
        //public override async Task Send(SQDocument document, int documentNumber, IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken)
        //{
        //    await OCRImage(document, documentNumber, progress, cToken);
        //}
        public override async System.Threading.Tasks.Task Send(IEnumerable<SQImage> images, IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken)
        {
            //Make sure optix is running
            // BSBDocument = new SQDocument();
            string checkingMess = $"Getting Lcr Ocr txt";
            progress.Report(new ProgressEventArgs(1, images.Count(), checkingMess));
            // ICROCRImage(images, documentNumber, progress, cToken, LCROCROutPutFolder).ConfigureAwait(false).GetAwaiter().GetResult();
            saveFolder = GetNewGuid();
            Log = new JsonFileDictionaryLogger();
            await LoadImage(images, cToken, progress);
            await base.Send(images, progress, cToken);
        }
        public async override Task Send(SQFile file, int fileNumber, IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cancelToken)
        {

            // OpenFileDialog _OpenFileDialog = new OpenFileDialog() { RestoreDirectory = true, Multiselect = true };
            // string[] paths = _OpenFileDialog.SelectFiles(null, null);
            //   EdocsUSA.Utilities.Extensions.OpenFileDialogExtensions.TryShowDialog(null,null);
            //await OCRImage(document, documentNumber, progress, cToken);

            string upLoadDir = string.Empty;
            InputDialog.Text = DisPlayArchiverName;
            FoundDate = false;
            //  InputDialog.ShowConfirmDialogBox = ShowConfirmDialogBox;
            //  InputDialog.CmboxNumbers = BoxNumber;
            InputDialog.IncludeBlankDocs = IncludeBlankDocs;
            InputDialog.ShowTotalDocsScanned = ShowTotalDocsScanned;
            InputDialog.CmbBoxDep = CmbDepartment;


            if (InputDialog.TryShowDialog(DialogResult.OK) != DialogResult.OK)
                throw new OperationCanceledException();
            string fileName = $"{InputDialog.Department.Trim()}_{InputDialog.ProjectYear.Trim()}_ {InputDialog.ProjectName.Trim()}";
            fileName= fileName.Replace(" ", "_");
            saveFolder = fileName;
            string dirPath = Path.Combine(SaveRootPath, saveFolder);



            // string fileName = GetNewGuid();
            //string fileName = $"{InputDialog.Department}_{InputDialog.ProjectYear}_{InputDialog.ProjectName}";

            string fileNameWithExt = Path.ChangeExtension(fileName, file.FileExtension);

            Path.ChangeExtension(fileName, file.FileExtension);

            string filePath = Path.Combine(dirPath, fileNameWithExt);


            Dictionary<string, string> recordsJs = new Dictionary<string, string>();
            // if (!(Edocs_Utilities.EdocsUtilitiesInstance.CHeckFileExists(Path.Combine(dirPath, $"{saveFolder}{SettingsJson}"))))
            // {
            Dictionary<string, string> settingsJs = new Dictionary<string, string>();
            settingsJs["ScanBatchID"] = saveFolder;
            // settingsJs["ScanDate"] = DateTime.Now.ToString();
            settingsJs["ScanOperator"] = Environment.UserName;
            //  settingsJs["ScanMachine"] = Environment.MachineName;
            settingsJs["InventoryTrackingApiUrl"] = InventoryTrackingApiUrl;
            settingsJs["InventoryTrackingSP"] = InventoryTrackingSP;
            settingsJs["UploadApiUrl"] = BSBProdDepUploadApiUrl;
            settingsJs["UpLoadController"] = BSBProdDepUpLoadController;
            // settingsJs["UploadFolder"] = UpLoadFolder.Replace("{ProjectName}", $"{InputDialog.BoxNumber}\\{upLoadDir}");
            //   settingsJs["UploadFolder"] = UpLoadFolder.Replace("{ProjectName}", $"{InputDialog.BoxNumber}\\");
            settingsJs["UploadFolder"] = UpLoadFolder;
            settingsJs["EdocsCustomerID"] = EdocsCustomerID;
            //settingsJs["LCROCTxtFile"] = Path.Combine(dirPath, $"{saveFolder}{LCROCTxtFile}");
            settingsJs["LCROCTxtFile"] = Path.Combine(dirPath, $"{fileName}{LCROCTxtFile}");
            settingsJs["TotalScanned"] = InputDialog.TotalScanned;
            settingsJs["TotalPageCount"] = InputDialog.TotalScanned;
            settingsJs["TotalImagesSaved"] = TotalImagesSaved.ToString();

            settingsJs["TotalOCR"] = DocICROCR.ToString();
            int tTyped = InputDialog.Department.Length + InputDialog.ProjectYear.Length + InputDialog.ProjectName.Length;
            settingsJs["TotalType"] = tTyped.ToString();
            settingsJs["OCRRecordsUpLoad"] = OCRRecordsUpLoad.ToString();
            settingsJs["CreateSearchabelPDF"] = CreateSearchabelPDF.ToString();
            settingsJs["OCRImageFolder"] = ImagesSaveFolder;
            settingsJs["TrackinUpLoadController"] = TrackinUpLoadController;

            Log.FilePath = Path.Combine(dirPath, $"{Path.GetFileName(fileName)}{SettingsJson}");

            Log.Append(settingsJs);
            //  }
            Log.FilePath = Path.Combine(dirPath, $"{fileName}{LCROCTxtFile}");

            if (SB.Length > 0)
            {

                File.WriteAllText(Log.FilePath, SB.ToString());
            }


            recordsJs["FileName"] = fileNameWithExt;
            recordsJs["ProjectDepartment"] = InputDialog.Department.Trim();
            recordsJs["ProjectYear"] = InputDialog.ProjectYear.Trim();
            recordsJs["ProjectName"] = InputDialog.ProjectName.Trim();






            Log.FilePath = Path.Combine(dirPath, $"{Path.GetFileName(fileName)}{RecordsJson}");
            Log.Append(recordsJs);
            //    SQFilesystemConnector.SaveFileResult saveFileResult = await FilesystemConnector.SaveFile(filePath, file.Data);
            File.WriteAllBytes(filePath, file.Data);
            // StartUpload(saveFolder, Path.Combine($"{Edocs_Utilities.DoubleQuotes}{SaveRootPath}{Edocs_Utilities.DoubleQuotes}",$"{Edocs_Utilities.DoubleQuotes}{saveFolder}{Edocs_Utilities.DoubleQuotes}"), "BSBPWD").ConfigureAwait(false).GetAwaiter().GetResult();
            StartUpload(saveFolder, Path.Combine(SaveRootPath,saveFolder), "BSBPWD").ConfigureAwait(false).GetAwaiter().GetResult();
        }
        //private async Task<int> GetTotalType()
        //{
        //    int totalTyped = InputDialog.PermitNum.Length;
        //    if (!(string.IsNullOrEmpty(InputDialog.ParcelNum)))
        //        totalTyped += InputDialog.ParcelNum.Length;

        //    if (!(string.IsNullOrEmpty(InputDialog.ZoneNumber)))
        //        totalTyped += InputDialog.ZoneNumber.Length;

        //    if (!(string.IsNullOrEmpty(InputDialog.GoCode)))
        //        totalTyped += InputDialog.GoCode.Length;

        //    if (!(string.IsNullOrEmpty(InputDialog.Address)))
        //        totalTyped += InputDialog.Address.Length;

        //    if (!(string.IsNullOrEmpty(InputDialog.OwnerLot)))
        //        totalTyped += InputDialog.OwnerLot.Length;

        //    if (!(string.IsNullOrEmpty(InputDialog.ConstCo)))
        //        totalTyped += InputDialog.ConstCo.Length;

        //    if (!(string.IsNullOrEmpty(InputDialog.ExePermitNumber)))
        //        totalTyped += InputDialog.ExePermitNumber.Length;
        //    totalTyped += 20;


        //    return totalTyped;
        //}\\

        private async Task LoadImage(IEnumerable<SQImage> images, System.Threading.CancellationToken cToken, IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress)
        {
            try
            {
                Init();
                string checkingMess = $"OCR Documents";
                progress.Report(new EdocsUSA.Utilities.ProgressEventArgs(0, 0, checkingMess));
                SB = new StringBuilder();
                DocICROCR = 0;
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
                if (OCRRecords)
                {

                    foreach (SQImage page in images)
                    {
                        DocICROCR++;
                        page.BeginEdit();
                        // SQImage image = page.Image;
                        // SQImageEditLock image_Lock = image.BeginEdit();

                        bitmap = page.LatestRevision.GetOriginalImageBitmap();



                        byte[] imageByte = ConvertImagePDF.ImageToBase64(page.LatestRevision.GetOriginalImageBitmap(), System.Drawing.Imaging.ImageFormat.Png);

                        string OCRResults = await ConvertImagePDF.OCRSrace(imageByte, true, false, OCRApiKey, "2", OCRISTable, uri, "image.png");
                        if (!(string.IsNullOrWhiteSpace(OCRResults)))
                        {
                            SB.AppendLine(OCRResults);

                        }
                    }
                    // await SaveImage(images, cToken, progress);
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Error loading Image {ex.Message}");
                //throw new OperationCanceledException($"Error loading Image {ex.Message}");
            }
        }

        private async Task ICROCRImage(IEnumerable<SQImage> images, int documentNumber, IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken, string outPutFolder)
        {
            //foreach (SQImage page in document.)
            // {

            try
            {
                Init();

                SB = new StringBuilder();
                outPutFolder = Path.Combine(outPutFolder, "Png");
                Edocs_Utilities.EdocsUtilitiesInstance.CreateDir(outPutFolder);
                string checkingMess = $"Getting Lcr Ocr txt";
                //  progress.Report(new ProgressEventArgs(1, images.Count(), checkingMess));
                foreach (SQImage img in images)
                {
                    try
                    {
                        documentNumber++;
                        img.BeginEdit();
                        progress.Report(new EdocsUSA.Utilities.ProgressEventArgs(documentNumber, images.Count(), checkingMess));

                        System.Drawing.Bitmap bitmap = img.LatestRevision.GetOriginalImageBitmap();
                        string imgFolder = System.IO.Path.Combine(outPutFolder, $"{Path.GetFileNameWithoutExtension(img.LatestRevision.OriginalImageFilePath)}.png");
                        bitmap.Save(imgFolder, System.Drawing.Imaging.ImageFormat.Png);
                        GetOcrText(imgFolder).ConfigureAwait(false).GetAwaiter().GetResult();
                    }
                    catch { }


                }

                // byte[] imageByte = ConvertImagePDF.ImageToBase64(bitmap, System.Drawing.Imaging.ImageFormat.Png);
                // Uri uri = new Uri(OCRWebApi);
                //  string file = @"C:\Archives\newlwbwel_v000.pdf";
                // string OCRResults = await ConvertImagePDF.OCRSrace(file,false,false, OCRApiKey, "1", OCRISTable, uri);
                //   OCRISTable = false;
                // string OCRResults = await ConvertImagePDF.OCRSrace(imageByte, true, false, OCRApiKey, "2", OCRISTable, uri, "image.png");


            }
            catch (Exception ex)
            {
                Console.WriteLine();
            }
            //}

        }
        private async Task ICROCRImage(SQDocument document, int documentNumber, IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken, string outPutFolder)
        {


            try
            {
                Init();
                SB = new StringBuilder();
                outPutFolder = Path.Combine(outPutFolder, "Png");
                Edocs_Utilities.EdocsUtilitiesInstance.CreateDir(outPutFolder);
                string checkingMess = $"Getting Lcr Ocr txt";

                //  progress.Report(new ProgressEventArgs(1, images.Count(), checkingMess));
                foreach (SQPage page in document.Pages)
                {

                    try
                    {
                        documentNumber++;

                        progress.Report(new EdocsUSA.Utilities.ProgressEventArgs(documentNumber, 1, checkingMess));

                        System.Drawing.Bitmap bitmap = page.Image.LatestRevision.GetOriginalImageBitmap();
                        string imgFolder = System.IO.Path.Combine(outPutFolder, $"{Path.GetFileNameWithoutExtension(page.Image.LatestRevision.OriginalImageFilePath)}.png");
                        bitmap.Save(imgFolder, System.Drawing.Imaging.ImageFormat.Png);
                        GetOcrText(imgFolder).ConfigureAwait(false).GetAwaiter().GetResult();

                    }
                    catch { }


                }

                // byte[] imageByte = ConvertImagePDF.ImageToBase64(bitmap, System.Drawing.Imaging.ImageFormat.Png);
                // Uri uri = new Uri(OCRWebApi);
                //  string file = @"C:\Archives\newlwbwel_v000.pdf";
                // string OCRResults = await ConvertImagePDF.OCRSrace(file,false,false, OCRApiKey, "1", OCRISTable, uri);
                //   OCRISTable = false;
                // string OCRResults = await ConvertImagePDF.OCRSrace(imageByte, true, false, OCRApiKey, "2", OCRISTable, uri, "image.png");


            }
            catch (Exception ex)
            {
                Console.WriteLine();
            }
            //}

        }
        private async Task ICROCRImage(SQPage page, int documentNumber, IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken, string outPutFolder)
        {


            try
            {

                Init();
                SB = new StringBuilder();
                outPutFolder = Path.Combine(outPutFolder, "Png");
                Edocs_Utilities.EdocsUtilitiesInstance.CreateDir(outPutFolder);
                string checkingMess = $"Getting Lcr Ocr txt";



                try
                {
                    documentNumber++;

                    //  progress.Report(new EdocsUSA.Utilities.ProgressEventArgs(documentNumber, 1, checkingMess));

                    System.Drawing.Bitmap bitmap = page.Image.LatestRevision.GetOriginalImageBitmap();
                    string imgFolder = System.IO.Path.Combine(outPutFolder, $"{Path.GetFileNameWithoutExtension(page.Image.LatestRevision.OriginalImageFilePath)}.png");
                    bitmap.Save(imgFolder, System.Drawing.Imaging.ImageFormat.Png);
                    GetOcrText(imgFolder).ConfigureAwait(false).GetAwaiter().GetResult();
                }
                catch { }




                // byte[] imageByte = ConvertImagePDF.ImageToBase64(bitmap, System.Drawing.Imaging.ImageFormat.Png);
                // Uri uri = new Uri(OCRWebApi);
                //  string file = @"C:\Archives\newlwbwel_v000.pdf";
                // string OCRResults = await ConvertImagePDF.OCRSrace(file,false,false, OCRApiKey, "1", OCRISTable, uri);
                //   OCRISTable = false;
                // string OCRResults = await ConvertImagePDF.OCRSrace(imageByte, true, false, OCRApiKey, "2", OCRISTable, uri, "image.png");


            }
            catch (Exception ex)
            {
                Console.WriteLine();
            }
            //}

        }
        private void Init()
        {




            InputDialog.ImageFolder = string.Empty;
        }

        //async Task IndexValues(string icrOcrTxt)
        //{
        //    string[] strIcrOcr = icrOcrTxt.Split(' ');
        //    int loop = 0;

        //    InputDialog.DocIssueDate = DateTime.Now.AddYears(-100);
        //    do
        //    {
        //        int loopCounter = 0;
        //        if (string.IsNullOrWhiteSpace(strIcrOcr[loop]))
        //        {
        //            loop++;
        //            continue;
        //        }
        //        if (loop == 28)
        //            Console.WriteLine();
        //        if (string.IsNullOrWhiteSpace(InputDialog.PermitNum))
        //        {
        //            if (strIcrOcr[loop].StartsWith("#"))
        //            {
        //                if (strIcrOcr[loop].Length > 1)
        //                    InputDialog.PermitNum = strIcrOcr[loop].Substring(strIcrOcr[loop].IndexOf("#") + 1);
        //                else
        //                    InputDialog.PermitNum = strIcrOcr[++loop];
        //                loop++;
        //                continue;

        //            }
        //            // InputDialog.PermitNum = FindMatch(lcrOcrTxt, ArchiverValuePermitOwner).ConfigureAwait(false).GetAwaiter().GetResult();
        //        }
        //        if (string.IsNullOrWhiteSpace(InputDialog.PNumber))
        //        {
        //            if (strIcrOcr[loop].ToLower().StartsWith("p"))
        //            {
        //                if (strIcrOcr[loop].Length == 1)
        //                    // InputDialog.PermitNum = strIcrOcr[loop].Substring(lcrOcrTxt.Trim().IndexOf("#") + 1);
        //                    //else
        //                    InputDialog.PNumber = strIcrOcr[++loop];
        //                loop++;
        //                continue;

        //            }
        //            // InputDialog.PermitNum = FindMatch(lcrOcrTxt, ArchiverValuePermitOwner).ConfigureAwait(false).GetAwaiter().GetResult();
        //        }
        //        if (string.IsNullOrWhiteSpace(InputDialog.ZoneNum))
        //        {
        //            if (strIcrOcr[loop].ToUpper().StartsWith("Z"))
        //            {
        //                if (strIcrOcr[loop].Length > 1)
        //                    InputDialog.ZoneNum = strIcrOcr[loop].Substring(strIcrOcr[loop].ToUpper().Trim().IndexOf("Z") + 1);
        //                else
        //                    InputDialog.ZoneNum = strIcrOcr[++loop];
        //                loop++;
        //                continue;

        //            }
        //            // InputDialog.PermitNum = FindMatch(lcrOcrTxt, ArchiverValuePermitOwner).ConfigureAwait(false).GetAwaiter().GetResult();
        //        }
        //        if (string.IsNullOrWhiteSpace(InputDialog.ParcelNum))
        //        {
        //            if ((strIcrOcr[loop].ToUpper().StartsWith("P")) && (strIcrOcr[loop].Length == 1))
        //            {
        //                InputDialog.PNumber = strIcrOcr[++loop];
        //                loop++;
        //                continue;

        //            }
        //            // InputDialog.PermitNum = FindMatch(lcrOcrTxt, ArchiverValuePermitOwner).ConfigureAwait(false).GetAwaiter().GetResult();
        //        }
        //        if (InputDialog.DocIssueDate.Year < 1950)
        //        {
        //            if ((strIcrOcr[loop].IndexOf("-") > 0) || (strIcrOcr[loop].IndexOf("/") > 0) || (strIcrOcr[loop].IndexOf("\\") > 0))
        //            {
        //                if (DateTime.TryParse(strIcrOcr[loop], out DateTime date))
        //                {
        //                    InputDialog.DocIssueDate = date;
        //                    loop++;
        //                    continue;

        //                }
        //            }

        //            // InputDialog.PermitNum = FindMatch(lcrOcrTxt, ArchiverValuePermitOwner).ConfigureAwait(false).GetAwaiter().GetResult();
        //        }
        //        if (string.IsNullOrWhiteSpace(InputDialog.ExePermitNumber))
        //        {
        //            if (strIcrOcr[loop].Trim().ToLower().StartsWith("exc"))
        //            {
        //                InputDialog.ExePermitNumber = strIcrOcr[++loop];
        //                loop++;
        //                continue;
        //            }

        //        }
        //        if (string.IsNullOrWhiteSpace(InputDialog.GCode))
        //        {
        //            if ((strIcrOcr[loop].Trim().ToLower().StartsWith("g")) && (strIcrOcr[loop].Length > 20) && (strIcrOcr[loop].Contains("-")))
        //            {
        //                InputDialog.GCode = strIcrOcr[loop];
        //                loop++;
        //                continue;
        //            }

        //        }
        //        if (string.IsNullOrWhiteSpace(InputDialog.CoAddress))
        //        {
        //            if (strIcrOcr[loop].Length >= 2)
        //            {
        //                bool invalidAddress = false;
        //                foreach (char c in strIcrOcr[loop])
        //                {
        //                    if (c < '0' || c > '9')
        //                    {
        //                        invalidAddress = true;
        //                        break;
        //                    }
        //                }
        //                if (!(invalidAddress))
        //                {

        //                    if ((Regex.IsMatch(strIcrOcr[(loop + 1)], @"^[a-zA-Z]+$", RegexOptions.IgnoreCase)))
        //                    {
        //                        if ((Regex.IsMatch(strIcrOcr[(loop + 2)], ArchiverValues, RegexOptions.IgnoreCase)))
        //                            InputDialog.CoAddress = $"{strIcrOcr[loop]} {strIcrOcr[++loop]}";
        //                        else
        //                            InputDialog.CoAddress = $"{strIcrOcr[loop]} {strIcrOcr[++loop]} {strIcrOcr[++loop]}";
        //                        loop++;
        //                        continue;
        //                    }
        //                }


        //            }
        //        }
        //        if (string.IsNullOrWhiteSpace(InputDialog.Constructionco))
        //        {
        //            if (strIcrOcr[loop].Trim().ToLower().StartsWith("con"))
        //            {
        //                if (!(strIcrOcr[(loop + 1)].ToLower().StartsWith("own")))
        //                {
        //                    //while (strIcrOcr[loop].Trim().ToLower().StartsWith("con"))
        //                    // loop++;
        //                    InputDialog.Constructionco = strIcrOcr[++loop];
        //                    loop++;
        //                    for (int tempL = loop; tempL < strIcrOcr.Length; tempL++)
        //                    {
        //                        if (!(strIcrOcr[tempL].ToLower().StartsWith("own")) && (strIcrOcr[tempL].Length > 1))
        //                        {
        //                            InputDialog.Constructionco = $"{InputDialog.Constructionco} {strIcrOcr[tempL]}";
        //                        }
        //                        else
        //                        {
        //                            loop = tempL - 1;
        //                            break;
        //                        }
        //                        if (loopCounter++ >= 3)
        //                        {
        //                            loop = tempL - 1;
        //                            break;
        //                        }
        //                    }
        //                    //  loop++;
        //                    continue;

        //                }


        //            }
        //        }
        //        if (string.IsNullOrWhiteSpace(InputDialog.CopOwner))
        //        {
        //            if (strIcrOcr[loop].Trim().ToLower().StartsWith("own"))
        //            {
        //                if (!(strIcrOcr[(loop + 1)].ToLower().StartsWith("con")))
        //                {
        //                    loop++;
        //                    if ((strIcrOcr[loop].StartsWith("-")))
        //                    {
        //                        loop++;
        //                    }
        //                    InputDialog.CopOwner = strIcrOcr[loop++];

        //                    for (int tempL = loop; tempL < strIcrOcr.Length; tempL++)
        //                    {
        //                        if ((strIcrOcr[tempL].StartsWith("-")))
        //                        {
        //                            loopCounter++;
        //                            tempL++;
        //                            continue;
        //                        }
        //                        if (!(strIcrOcr[tempL].ToLower().StartsWith("con")) && (strIcrOcr[tempL].Length > 1))
        //                        {
        //                            InputDialog.CopOwner = $"{InputDialog.CopOwner} {strIcrOcr[tempL]}";
        //                        }
        //                        else
        //                        {
        //                            loop = tempL - 1;
        //                            break;
        //                        }
        //                        if (loopCounter++ >= 3)
        //                        {
        //                            loop = tempL - 1;
        //                            break;
        //                        }

        //                    }



        //                }


        //            }
        //        }
        //        loop++;
        //    }
        //    while (loop < strIcrOcr.Length);


        //}


        //async Task GetArchiverValues(string lcrOcrTxtFile)
        //{
        //    string lcrOcrTxt = string.Empty;
        //    int gotAddress = 0;
        //    string address = string.Empty;
        //    // string s = File.ReadAllText(lcrOcrTxtFile);
        //    int ik = 0;
        //    using (StreamReader sr = new StreamReader(lcrOcrTxtFile))
        //    {
        //        while ((lcrOcrTxt = sr.ReadLine()) != null)
        //        {

        //            try
        //            {


        //                if (string.IsNullOrWhiteSpace(lcrOcrTxt))
        //                    continue;
        //                ik++;
        //                if (string.IsNullOrEmpty(address))
        //                    address = lcrOcrTxt;
        //                if ((gotAddress == 0))
        //                {
        //                    if (string.IsNullOrWhiteSpace(InputDialog.CoAddress))
        //                    {
        //                        if (lcrOcrTxt.Trim().StartsWith("3101") || lcrOcrTxt.Trim().StartsWith("3011") || lcrOcrTxt.Trim().StartsWith("33"))
        //                        {
        //                            InputDialog.CoAddress = lcrOcrTxt;
        //                            if (lcrOcrTxt.Trim().StartsWith("3011"))
        //                            {
        //                                lcrOcrTxt = sr.ReadLine();
        //                                InputDialog.CoAddress = $"{InputDialog.CoAddress} {lcrOcrTxt}";
        //                            }
        //                            gotAddress++;
        //                            continue;
        //                        }
        //                    }


        //                }
        //                if (string.IsNullOrWhiteSpace(InputDialog.PermitNum))
        //                {
        //                    if (lcrOcrTxt.Trim().IndexOf("#") > 0)
        //                    {
        //                        InputDialog.PermitNum = lcrOcrTxt.Substring(lcrOcrTxt.Trim().IndexOf("#") + 1);
        //                        continue;

        //                    }
        //                    // InputDialog.PermitNum = FindMatch(lcrOcrTxt, ArchiverValuePermitOwner).ConfigureAwait(false).GetAwaiter().GetResult();
        //                }
        //                if (string.IsNullOrWhiteSpace(InputDialog.Address))
        //                {
        //                    InputDialog.Address = FindMatch(lcrOcrTxt, ArchiverValueAddress).ConfigureAwait(false).GetAwaiter().GetResult();
        //                }
        //                if (string.IsNullOrWhiteSpace(InputDialog.ExePermitNumber))
        //                {
        //                    if (lcrOcrTxt.Trim().ToLower().StartsWith("exc"))
        //                    {
        //                        InputDialog.ExePermitNumber = lcrOcrTxt.Substring(lcrOcrTxt.IndexOf("ExcPermit") + 9);
        //                        continue;
        //                    }

        //                }
        //                if (string.IsNullOrWhiteSpace(InputDialog.Constructionco))
        //                {
        //                    if (lcrOcrTxt.Trim().ToLower().StartsWith("con"))
        //                    {
        //                        string[] strCon = lcrOcrTxt.Split(' ');
        //                        for (int k = 0; k < strCon.Length; k++)
        //                        {
        //                            if (!(string.IsNullOrWhiteSpace(strCon[k])))
        //                            {
        //                                if (strCon.Length > 3)
        //                                {
        //                                    {
        //                                        if ((strCon[k].Length > 2) && (!(strCon[k].ToLower().StartsWith("con"))))
        //                                            InputDialog.Constructionco = $"{InputDialog.Constructionco} {strCon[k]}";
        //                                    }
        //                                }
        //                            }
        //                        }

        //                        continue;
        //                    }

        //                }

        //                // if (string.IsNullOrWhiteSpace(InputDialog.ZoneNumber))
        //                // {
        //                //   InputDialog.ZoneNumber = FindMatch(lcrOcrTxt, ArchiverValueZone).ConfigureAwait(false).GetAwaiter().GetResult();
        //                //}
        //                //if (string.IsNullOrWhiteSpace(InputDialog.GoCode))
        //                //{
        //                //    InputDialog.ParcelNum = FindMatch(lcrOcrTxt, ArchiverValueCode).ConfigureAwait(false).GetAwaiter().GetResult();
        //                //}
        //                if (!(FoundDate))
        //                {
        //                    if ((lcrOcrTxt.IndexOf("-") > 0) || (lcrOcrTxt.IndexOf("/") > 0) || (lcrOcrTxt.IndexOf("\\") > 0))
        //                    {
        //                        if (DateTime.TryParse(lcrOcrTxt, out DateTime date))
        //                        {
        //                            InputDialog.DocIssueDate = date;
        //                            FoundDate = true;
        //                            continue;
        //                        }

        //                    }

        //                }
        //                //if (string.IsNullOrWhiteSpace(InputDialog.ExePermitNumber))
        //                //{
        //                //    InputDialog.ExePermitNumber = FindMatch(lcrOcrTxt, ArchiverValueExcPermit).ConfigureAwait(false).GetAwaiter().GetResult();
        //                //}
        //                //if (string.IsNullOrWhiteSpace(InputDialog.ParcelNum))
        //                //{
        //                //    InputDialog.ExePermitNumber = FindMatch(lcrOcrTxt, ArchiverValueExcPermit).ConfigureAwait(false).GetAwaiter().GetResult();
        //                //}

        //                if (string.IsNullOrWhiteSpace(InputDialog.CopOwner))
        //                {
        //                    if (lcrOcrTxt.Trim().ToLower().StartsWith("own"))
        //                    {
        //                        string[] own = lcrOcrTxt.Split(' ');
        //                        if (own.Length > 3)
        //                        {
        //                            for (int i = 0; i < own.Length; i++)
        //                            {
        //                                if (!(string.IsNullOrWhiteSpace(own[i])))
        //                                    if ((own[i].Length > 2) && (!(own[i].ToLower().StartsWith("ow"))))
        //                                        InputDialog.CopOwner = $"{InputDialog.CopOwner} {own[i]}";
        //                            }
        //                        }

        //                    }

        //                }

        //            }
        //            catch { }

        //            if (ik == 1)
        //                InputDialog.CoAddress = address;

        //        }
        //    }
        //}
        //public static void WriteRecord(string batchDir, SharepointBatchRecord record, byte[] fileData)
        //{
        //   // EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Batch dir is " + batchDir);
        //   // string batchId = BatchHelper.GetBatchId(batchDir);
        //    //EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Batch dir is " + batchDir);

        //    if (Directory.Exists(batchDir) == false)
        //    {
        //       // EDL.TraceLogger.TraceLoggerInstance.TraceError("Batch dir not found " + batchDir);
        //        throw new DirectoryNotFoundException(batchDir);
        //    }

        //    string batchFileName = batchId + "_records";
        //    string batchFileNameWithExt = Path.ChangeExtension(batchFileName, "json");
        //    string batchFilePath = Path.Combine(batchDir, batchFileNameWithExt);

        //    string recordEntry = Serializer.Serialize(record);
        //    File.AppendAllLines(batchFilePath, new string[] { recordEntry });

        //    string recordFilePath = Path.Combine(batchDir, record.FileName);
        //    EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Writing record data to " + batchFilePath);
        //    File.WriteAllBytes(recordFilePath, fileData);
        //}
        //async Task<string> FindMatch(string lcrOcrTxt, string matchValue)
        //{
        //    System.Text.RegularExpressions.MatchCollection match = System.Text.RegularExpressions.Regex.Matches(lcrOcrTxt, matchValue, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        //    string regxMatch = string.Empty;
        //    if (match.Count > 0)
        //    {
        //        for (int i = 0; i < match.Count; i++)
        //        {
        //            string value = lcrOcrTxt.Substring(match[i].Index);
        //            value = Regex.Replace(value, @"\r\n?|\n", " ");
        //            //  string[] str = value.Split('\n');
        //            string[] str = value.Split(' ');
        //            regxMatch = str[1];
        //            //foreach(string str in value.Split(' '))
        //            // {
        //            // regxMatch = str[1];
        //            // break;
        //            // }




        //        }
        //    }
        //    return regxMatch;
        //}
        async Task GetOcrText(string imgFolder)
        {
            try
            {
                DocICROCR = 0;
                string outPutFolder = Path.Combine(LCROCROutPutFolder, "Txt");
                InputDialog.ImageFolder = imgFolder;
                string args = string.Format(LCROCRExeParms, LCROCRMineType, imgFolder, outPutFolder);
                string lcrOcrExe = Path.Combine(SettingsManager.ApplicationDirectory, LCROCRExe);
                Edocs_Utilities.EdocsUtilitiesInstance.StartProcess(lcrOcrExe, args, true);
                string txtFile = $"{Path.Combine(outPutFolder, Path.GetFileNameWithoutExtension(imgFolder))}.txt";
                if (Edocs_Utilities.EdocsUtilitiesInstance.CHeckFileExists(txtFile))
                {
                    string lcrOcrText = File.ReadAllText($"{txtFile}");
                    lcrOcrText = lcrOcrText.Replace("\r", " ");
                    lcrOcrText = lcrOcrText.Replace("\n", " ");
                    SB.AppendLine(lcrOcrText);

                    //GetArchiverValues(txtFile).ConfigureAwait(false).GetAwaiter().GetResult();
                    //  IndexValues(lcrOcrText).ConfigureAwait(false).GetAwaiter().GetResult();
                    DocICROCR++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public string GetNewGuid()
        {
            return Guid.NewGuid().ToString();
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
                ImagesSaveFolder = $"OCRImagesBSBPWD\\{GetNewGuid()}";
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
                  //  if (!(File.Exists(imgFolder)))
                   //     MessageBox.Show($"File not saved {imgFolder}");
                    TotalImagesSaved++;
                    if (TotalImagesSaved == 1)
                    {
                        InputDialog.ImageViewerPWD.ClearAll(false, false);

                        InputDialog.ImageViewerPWD.Add(page);
                        InputDialog.ImageViewerPWD.NavigateTo(0);
                        InputDialog.ImageViewerPWD.ActiveImageViewer.ScaleToFitHeight();
                        InputDialog.ImageFolder = imgFolder;
                        if (!(SaveImages))
                            break;
                    }
                    //InputDialog.PSDImage.Add(page);
                    // InputDialog.PSDImage.NavigateTo(0);
                    //InputDialog.PSDImage.ActiveImageViewer.ScaleToFitHeight();
                    // byte[] imageByte = ConvertImagePDF.ImageToBase64(page.LatestRevision.GetOriginalImageBitmap(), System.Drawing.Imaging.ImageFormat.Png);
                    //string OCRResults = await ConvertImagePDF.OCRSrace(imageByte, true, false, OCRApiKey, "2", OCRISTable, uri, "image.png");
                    //bitmap = page.LatestRevision.GetOriginalImageBitmap();

                }
                TotalImagesSaved++;
            }
            catch (Exception ex)
            {
                throw new OperationCanceledException($"Error loading Image {ex.Message}");
            }
        }
        private async Task StartUpload(string batchID, string uploadFolder, string archiver)
        {
            try
            {


                // uploadFolder = uploadFolder.Replace("{ProjectName}", "").Trim();
                Process p = new Process();

                p.StartInfo.FileName = UploadExe;
                if (!(ShowCmdWindow))
                    p.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
                // p.StartInfo.Arguments = string.Format("/batchid:{0} /archiver:{1}", batchID, archiver);
                p.StartInfo.Arguments = string.Format(UploadExeParms, batchID, uploadFolder, archiver);
                EdocsUSA.Utilities.Logging.TraceLogger.TraceLoggerInstance.TraceInformation($"Starting process {p.StartInfo.FileName} with parms {p.StartInfo.Arguments}");
                p.Start();
                // await Task.Factory.StartNew(() => p.WaitForExit());
                if (WaitForExit)
                {
                    p.WaitForExit();
                    if (p.ExitCode != 0)
                    {
                        EdocsUSA.Utilities.Logging.TraceLogger.TraceLoggerInstance.TraceError($"running process {p.StartInfo.FileName} with parms {p.StartInfo.Arguments} exit code {p.ExitCode.ToString()}");
                        EdocsUSA.Utilities.Logging.TraceLogger.TraceLoggerInstance.TraceError($"running process {UploadExe} with parms {batchID} {uploadFolder}");
                        MessageBox.Show($"Process {UploadExe} with parms  {p.StartInfo.Arguments} did not start", "Error Uploading", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        throw new Exception(p.ExitCode.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                throw new OperationCanceledException(ex.Message);
            }
        }
    }

}
