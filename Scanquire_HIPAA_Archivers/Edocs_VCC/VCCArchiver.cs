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
using Microsoft;
using Edocs.Ocr.Convert.Libaray.Img.PDF;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
//L:\EdocsGItHub\Scanquire_V8\Scanquire\bin\Debug\Edocs.Icr.Ocr.Google\Edocs.Icr.Ocr.Google.exe /mt:image/png /pdfimg:D:\Diocese_Of_Helena_Images\JPG\5b4e0108-533f-465d-813d-8b0a990e2f6d.png /of:D:\Diocese_Of_Helena_Images\JPG\Txt
namespace Edocs.Dillion.VCC.Archiver
{
    public class VCCArchiver : SQArchiverBase
    {
        public VCCArchiverrDialog InputDialog = new VCCArchiverrDialog();
        private readonly string RecordsJson = "_records.json";
        private readonly string SettingsJson = "_settings.json";
        private readonly string Csv = ".csv";
        private readonly string LCROCTxtFile = "_lcrocr.txt";
        private int numberFile;
        private SQDocument VCCDocument
        { get; set; }

        private int TotalPagesProcessed
        { get; set; }
        public string SqlConnection
        { get; set; }
        public string ChurchCity
        { get; set; }
        public string Church
        { get; set; }
        public string BookType
        { get; set; }
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
        private string SaveFolder
        { get; set; }
        public string OCRApiKey
        { get; set; }
        public string OCRWebApi
        { get; set; }
        public bool OCRISTable
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

        private JsonFileDictionaryLogger Log
        { get; set; }
        private int VCCTotalICR
        { get; set; }
        private string ImgFolder
        { get; set; }
        private bool ShowDialog
        { get; set; }
        public bool CreateJpgImg
        { get; set; }
        public int EdocsCustomerNumber
        { get; set; }
        public bool ProcessWaitForExit
        { get; set; }
        private int FileNumber
        { get; set; }
        private string LFname
        { get; set; }
        private string DirPath
        { get; set; }
        private int ImgNumber
        { get; set; }
        private string InvoiceNumber
        { get; set; }
        private string InvoiceDate
        { get; set; }
        private string WODate
        { get; set; }
        private string InvoiceDescription
        { get; set; }
        private string DocInvoiceNumber
        { get; set; }
        private string SettingsJsFN
        { get; set; }
        private string BatchID
        {
            get; set;
        }
        private int PageNum
        { get; set; }
        public override IEnumerable<System.Threading.Tasks.Task<SQImage>> AcquireFromScannerForNew(IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken)
        {


            //  InputDialog.TryShowDialog(DialogResult.OK);
            return base.AcquireFromScannerForNew(progress, cToken);
        }
        #region send
        //public override async System.Threading.Tasks.Task Send(IEnumerable<SQImage> images, IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken)
        //{


        //    //Make sure optix is running
        //    Init().ConfigureAwait(false).GetAwaiter().GetResult();
        //    List<SQImage> sqL = new List<SQImage>();

        //    string checkingMess = $"Getting Lcr Ocr txt";
        //    //    progress.Report(new ProgressEventArgs(1, images.Count(), checkingMess));
        //    // ICROCRImage(images, documentNumber, progress, cToken, LCROCROutPutFolder).ConfigureAwait(false).GetAwaiter().GetResult();
        //    //  SaveFolder = GetNewGuid();
        //    Log = new JsonFileDictionaryLogger();
        //    await base.Send(images, progress, cToken);
        //}
        //public override async Task Send(SQDocument document, int documentNumber, IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken)
        //{
        //    Init().ConfigureAwait(false).GetAwaiter().GetResult();
        //    ShowDialog = true;
        //        FileNumber = 0;

        //        ImgNumber = 0;
        //        ImgFolder = "NA";


        //    await ICROCRImagePage(document.Pages[TotalPagesProcessed], documentNumber, progress, cToken, LCROCROutPutFolder);
        //    VCCDocument.Pages.Add(document.Pages[TotalPagesProcessed++]);
        //    if (TotalPagesProcessed == document.Pages.Count)
        //            {


        //                await base.Send(VCCDocument, TotalPagesProcessed, progress, cToken);
        //         }
        //    await base.Send(document, 1, progress, cToken);
        //}

        public override async Task Send(SQDocument document, int documentNumber, IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken)
        {
            Init().ConfigureAwait(false).GetAwaiter().GetResult();
            // Init().ConfigureAwait(false).GetAwaiter().GetResult();
            ShowDialog = true;
            FileNumber = 0;
            int totalPagesProcessed = 0;
            ImgNumber = 0;
            ImgFolder = "NA";
            VCCTotalICR = 0;

            //    if (InputDialog.TryShowDialog(DialogResult.OK) != DialogResult.OK)
            //  throw new OperationCanceledException();

            try
            {
                foreach (var page in document.Pages)
                {
                    totalPagesProcessed++;
                  
                    //  if(CreateJpgImg)
                    await ICROCRImagePage(page, documentNumber, progress, cToken, LCROCROutPutFolder);
                    //    VCCDocument.Pages.Add(page);


                    if (totalPagesProcessed == document.Pages.Count)
                    {
                        InvoiceNumber = DocInvoiceNumber;
                        VCCDocument.Pages.Add(page);
                        await base.Send(VCCDocument, VCCDocument.Pages.Count(), progress, cToken);
                    }
                    else if (string.IsNullOrWhiteSpace(InvoiceNumber))
                    {
                        VCCDocument.Pages.Add(page);
                    }

                    else
                    {
                        // InvoiceNumber = DocInvoiceNumber;
                        await base.Send(VCCDocument, documentNumber, progress, cToken);
                        VCCDocument.Pages.Clear();
                        VCCDocument.Pages.Add(page);
                    }

                    //await base.Send(VCCDocument, documentNumber, progress, cToken);
                    //VCCDocument.Pages.Clear();

                }
                //StartUpload(LFname, DirPath, "DOH").ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                throw new OperationCanceledException();
            }


        }
        public async override Task Send(SQFile file, int fileNumber, IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cancelToken)
        // public override async Task Send(SQDocument document, int documentNumber, IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken){
        {

            // OpenFileDialog _OpenFileDialog = new OpenFileDialog() { RestoreDirectory = true, Multiselect = true };
            // string[] paths = _OpenFileDialog.SelectFiles(null, null);
            //   EdocsUSA.Utilities.Extensions.OpenFileDialogExtensions.TryShowDialog(null,null);
            //await OCRImage(document, documentNumber, progress, cToken);d

            string upLoadDir = string.Empty;
            InputDialog.Text = DisPlayArchiverName;
            FoundDate = false;
            InputDialog.SqlConnection = SqlConnection;
            InputDialog.EdocsCustomerID = EdocsCustomerID;
            // string fileName = string.Empty;
            //  InputDialog.ShowConfirmDialogBox = ShowConfirmDialogBox;
            //  InputDialog.CmboxNumbers = BoxNumber;
            InputDialog.IncludeBlankDocs = IncludeBlankDocs;
            InputDialog.ShowTotalDocsScanned = ShowTotalDocsScanned;
            InputDialog.ImageFolder = ImgFolder;
            InputDialog.ChurchName = Church;
            InputDialog.ChurchCity = ChurchCity;
            InputDialog.ChurchBookType = BookType;




            //  string fileName = GetNewGuid();
            // fileName = $"{InputDialog.City} {InputDialog.Church} {InputDialog.BookType} {InputDialog.DateRangeSDate} thru {InputDialog.DateRangeEDate}";

            string fileNameWithExt = Path.ChangeExtension(InvoiceNumber, file.FileExtension);



            string filePath = Path.Combine(DirPath, fileNameWithExt);


            Dictionary<string, string> recordsJs = new Dictionary<string, string>();
            if (!(Edocs_Utilities.EdocsUtilitiesInstance.CHeckFileExists(SettingsJsFN)))
            {
                Dictionary<string, string> settingsJs = new Dictionary<string, string>();
                settingsJs["ScanBatchID"] = "BatchID";
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

                settingsJs["ImgOrgFileName"] = ImgFolder;
                settingsJs["DownLoadSubFolder"] = SaveFolder;
                //settingsJs["LCROCTxtFile"] = Path.Combine(dirPath, $"{saveFolder}{LCROCTxtFile}");

                settingsJs["StandardLargeDocument"] = InputDialog.LargeDoc.ToString();

                Log.FilePath = SettingsJsFN; //Path.Combine(DirPath, $"{LFname}{SettingsJson}");

                Log.Append(settingsJs);
            }
            //Log.FilePath = Path.Combine(dirPath, $"{fileName}{LCROCTxtFile}");
            //if (!(string.IsNullOrWhiteSpace(InputDialog.TextLCROCR)))
            //{
            //    File.WriteAllText(Log.FilePath, InputDialog.TextLCROCR);
            //}
            //else
            //{
            //    if (SB.Length > 0)
            //    {

            //        File.WriteAllText(Log.FilePath, SB.ToString());
            //    }
            //}
            if (File.Exists(filePath))
            {
                fileNameWithExt = Path.ChangeExtension($"{InvoiceNumber}_{numberFile++}", file.FileExtension);
                filePath = Path.Combine(DirPath, fileNameWithExt);
            }
            recordsJs["FileName"] = fileNameWithExt;
            recordsJs["WorkOrderNumber"] = InvoiceNumber;
            recordsJs["WorkOrderDate"] = InvoiceDate;





            //recordsJs["LCROCTxtFile"] = Path.Combine(dirPath, $"{fileName}{LCROCTxtFile}");


            Log.FilePath = Path.Combine(DirPath, $"{BatchID}{RecordsJson}");
            Log.Append(recordsJs);

            //    SQFilesystemConnector.SaveFileResult saveFileResult = await FilesystemConnector.SaveFile(filePath, file.Data);

            File.WriteAllBytes(filePath, file.Data);
            //StringBuilder sb = CreateCSVHeader().ConfigureAwait(false).GetAwaiter().GetResult();
            //sb.AppendLine($"{InputDialog.City},{InputDialog.Church},{InputDialog.BookType},{InputDialog.DateRangeSDate}-{InputDialog.DateRangeEDate},{PageFirst.ToString()}-{PageSecond.ToString()},{fileNameWithExt},{fileName}_ocr.txt,downloadfolder");
            //Log.FilePath = Path.Combine(dirPath, $"{fileName}{Csv}");
            //File.WriteAllText(Log.FilePath, sb.ToString());
            //Edocs_Utilities.EdocsUtilitiesInstance.CopyFile(ImgFolder,Path.Combine(dirPath, $"{fileName}.png"), true,true, true);

            //  StartUpload(fileName, dirPath, "DOH").ConfigureAwait(false).GetAwaiter().GetResult();

        }
        #endregion
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




        private async Task ICROCRImage(IEnumerable<SQImage> images, int documentNumber, IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken, string outPutFolder)
        {
            //foreach (SQImage page in document.)
            // {

            try
            {

                ImgFolder = string.Empty;
                SB = new StringBuilder();

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
                        ImgFolder = System.IO.Path.Combine(outPutFolder, $"{Path.GetFileNameWithoutExtension(img.LatestRevision.OriginalImageFilePath)}.png");
                        bitmap.Save(ImgFolder, System.Drawing.Imaging.ImageFormat.Png);
                        GetOcrText(ImgFolder).ConfigureAwait(false).GetAwaiter().GetResult();
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

                SB = new StringBuilder();
                outPutFolder = Path.Combine(outPutFolder, "Png");
                Edocs_Utilities.EdocsUtilitiesInstance.CreateDir(outPutFolder);
                string checkingMess = $"Getting Lcr Ocr txt";
                ImgFolder = string.Empty;
                //  progress.Report(new ProgressEventArgs(1, images.Count(), checkingMess));
                foreach (SQPage page in document.Pages)
                {

                    try
                    {
                        documentNumber++;

                        progress.Report(new EdocsUSA.Utilities.ProgressEventArgs(documentNumber, 1, checkingMess));

                        System.Drawing.Bitmap bitmap = page.Image.LatestRevision.GetOriginalImageBitmap();
                        //  ImgFolder = System.IO.Path.Combine(outPutFolder, $"{Path.GetFileNameWithoutExtension(page.Image.LatestRevision.OriginalImageFilePath)}.png");
                        ImgFolder = System.IO.Path.Combine(outPutFolder, $"{Path.GetFileNameWithoutExtension(page.Image.LatestRevision.OriginalImageFilePath)}.png");
                        bitmap.Save(ImgFolder, System.Drawing.Imaging.ImageFormat.Png);
                        //  GetOcrText(imgFolder).ConfigureAwait(false).GetAwaiter().GetResult();

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
        private async Task ICROCRImagePage(SQPage page, int documentNumber, IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken, string outPutFolder)
        {


            try
            {

                string fileWriterProgressCaption = "Ocr file " + documentNumber;
 //               Action<ProgressEventArgs> fileWriterProgressAction = new Action<ProgressEventArgs>(p =>
   //             { progress.Report(new ProgressEventArgs(p.Current, p.Total, fileWriterProgressCaption)); });
     //           Progress<ProgressEventArgs> fileWriterProgress = new Progress<ProgressEventArgs>(fileWriterProgressAction);
                progress.Report(new ProgressEventArgs(1,1,fileWriterProgressCaption));
                Edocs_Utilities.EdocsUtilitiesInstance.CreateDir(LCROCROutPutFolder);
                //Init();
                //  SB = new StringBuilder();
                // outPutFolder = Path.Combine(outPutFolder, "Jpeg");
                Edocs_Utilities.EdocsUtilitiesInstance.CreateDir(outPutFolder);
                string checkingMess = "Creating JPeg Image";
                InvoiceNumber = string.Empty;


                try
                {
                    documentNumber++;

                    //progress.Report(new EdocsUSA.Utilities.ProgressEventArgs(1, 1, checkingMess));
                    progress.Report(new EdocsUSA.Utilities.ProgressEventArgs(documentNumber, 1, fileWriterProgressCaption));
                    System.Drawing.Bitmap bitmap = page.Image.LatestRevision.GetOriginalImageBitmap();
                    string imgFolder = imgFolder = System.IO.Path.Combine(LCROCROutPutFolder, $"{Path.GetFileNameWithoutExtension(page.Image.LatestRevision.OriginalImageFilePath)}.png");
                    bitmap.Save(imgFolder, System.Drawing.Imaging.ImageFormat.Png);
                    // GetOcrText(ImgFolder).ConfigureAwait(false).GetAwaiter().GetResult();
                    // ImgFolder = System.IO.Path.Combine(outPutFolder, $"{Path.GetFileNameWithoutExtension(page.Image.LatestRevision.OriginalImageFilePath)}.jpeg");
                    //  ImgFolder = System.IO.Path.Combine(outPutFolder, $"{ImgNumber++}.jpeg");
                    // bitmap.Save(imgFolder, System.Drawing.Imaging.ImageFormat.Jpeg);
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
        private async Task Init()
        {
            VCCDocument = new SQDocument();

            DirPath = Path.Combine(SaveRootPath, GetNewGuid());
            Edocs_Utilities.EdocsUtilitiesInstance.CreateDir(DirPath);
            BatchID = GetNewGuid();
            SettingsJsFN = Path.Combine(DirPath, $"{BatchID}{SettingsJson}");

            Log = new JsonFileDictionaryLogger();
            VCCTotalICR = 0;
            TotalPagesProcessed = 0;
            numberFile = 0;
        }





        async Task GetOcrText(string imgFolder)
        {
            try
            {
                //DocICROCR = 0;

                string outPutFolder = Path.Combine(LCROCROutPutFolder, "Txt");
                InputDialog.ImageFolder = ImgFolder;
                string args = string.Format(LCROCRExeParms, LCROCRMineType, imgFolder, outPutFolder);
                string lcrOcrExe = Path.Combine(SettingsManager.ApplicationDirectory, LCROCRExe);
                Edocs_Utilities.EdocsUtilitiesInstance.StartProcess(lcrOcrExe, args, true);
                string txtFile = $"{Path.Combine(outPutFolder, Path.GetFileNameWithoutExtension(imgFolder))}.txt";
                if (Edocs_Utilities.EdocsUtilitiesInstance.CHeckFileExists(txtFile))
                {
                    string lcrOcrText = File.ReadAllText($"{txtFile}");
                    // lcrOcrText = lcrOcrText.Replace("\r\n", " ");
                    //  lcrOcrText = lcrOcrText.Replace("\r", " ");
                    //  lcrOcrText = Regex.Replace(lcrOcrText, @"\r?\n", " "); //lcrOcrText.Replace("\n", " ");
                    // string[] invoiceStr  = Regex.Split(lcrOcrText, @"\r?\n");
                    string[] invoiceStr = Regex.Split(lcrOcrText.ToLower(), @"\r?\n");

                    //  GetWorkOrderNumber(invoiceStr).ConfigureAwait(false).GetAwaiter().GetResult();
                    GetWorkOrderNumberDate(invoiceStr, Regex.Replace(lcrOcrText, @"\r?\n", " ")).ConfigureAwait(false).GetAwaiter().GetResult();

                    // SB.AppendLine(lcrOcrText);

                    //GetArchiverValues(txtFile).ConfigureAwait(false).GetAwaiter().GetResult();
                    //   IndexValues(lcrOcrText).ConfigureAwait(false).GetAwaiter().GetResult();
                    VCCTotalICR++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private async Task GetWorkOrderNumber(string inPutStr, string[] arrayStr)
        {

            if (Array.IndexOf(arrayStr, DocInvoiceNumber) < 0)
            {
                InvoiceNumber = DocInvoiceNumber;
                DocInvoiceNumber = string.Empty;
                MatchCollection matches = VccConstants.GetRegxMatch(inPutStr, VccConstants.InvoiceNumber).ConfigureAwait(false).GetAwaiter().GetResult();

                foreach (Match invMatch in matches)
                {
                    int index = Array.IndexOf(arrayStr, invMatch.Value);
                    if (index > 1)
                    {

                        index--;
                        if ((arrayStr[index].StartsWith("w.", StringComparison.OrdinalIgnoreCase)) || (arrayStr[index].StartsWith("work", StringComparison.OrdinalIgnoreCase)))
                        {
                            DocInvoiceNumber = invMatch.Value;
                            break;
                        }
                        index = Array.IndexOf(arrayStr, invMatch.Value);
                    }
                }
                if (string.IsNullOrWhiteSpace(DocInvoiceNumber))
                {
                    foreach (string inv in arrayStr)
                    {
                        if ((inv.StartsWith("w.o", StringComparison.OrdinalIgnoreCase)) || (inv.StartsWith("work", StringComparison.OrdinalIgnoreCase)))
                        {
                            Match match = Regex.Match(inv, @"\d+");
                            if (match.Value.Length < 4)
                                continue;
                            if (match.Success)
                                DocInvoiceNumber = match.Value;
                            break;
                        }
                    }

                }
                if (string.IsNullOrWhiteSpace(DocInvoiceNumber))
                {
                    DocInvoiceNumber = InvoiceNumber;
                    InvoiceNumber = string.Empty;
                }
            }
        }

        private async Task GetWorkOrderNumber(string[] arrayStr)
        {


            if (Array.IndexOf(arrayStr, DocInvoiceNumber) < 0)
            {
                InvoiceNumber = DocInvoiceNumber;
                DocInvoiceNumber = string.Empty;
                int index = Array.FindIndex(arrayStr, line => (line.StartsWith("w.o.") || line.StartsWith("work order")));

                if (index >= 0)
                {

                    while (index < arrayStr.Length)
                    {
                        Match match = Regex.Match(arrayStr[index], @"\d+");
                        if (match.Success)
                        {
                            if (match.Value.Length > 4)
                            {
                                DocInvoiceNumber = match.Value;

                                break;
                            }
                        }
                        index++;

                    }
                }
                if (string.Compare(DocInvoiceNumber, InvoiceNumber, true) == 0)
                    InvoiceNumber = string.Empty;
            }
            if (string.IsNullOrWhiteSpace(DocInvoiceNumber))
            {
                DocInvoiceNumber = InvoiceNumber;
                InvoiceNumber = string.Empty;
            }

        }
        private async Task GetWorkOrderNumberDate(string[] arrayStr, string dateStr)
        {


            if (Array.IndexOf(arrayStr, DocInvoiceNumber) < 0)
            {
                InvoiceNumber = DocInvoiceNumber;
                DocInvoiceNumber = string.Empty;
                int index = Array.FindIndex(arrayStr, line => (line.StartsWith("w.o.") || line.StartsWith("work order")));

                if (index >= 0)
                {

                    while (index < arrayStr.Length)
                    {
                        Match match = Regex.Match(arrayStr[index], @"\d+");
                        if (match.Success)
                        {
                            if (match.Value.Length > 4)
                            {
                                DocInvoiceNumber = match.Value;
                                GetInvDate(arrayStr, dateStr).ConfigureAwait(false).GetAwaiter().GetResult();
                                break;
                            }
                        }
                        index++;

                    }
                }
                if (string.Compare(DocInvoiceNumber, InvoiceNumber, true) == 0)
                    InvoiceNumber = string.Empty;
            }
            if (string.IsNullOrWhiteSpace(DocInvoiceNumber))
            {
                DocInvoiceNumber = InvoiceNumber;
                InvoiceNumber = string.Empty;
            }

        }
        private async Task GetInvDate(string[] inPutStr, string dateStr)
        {
            InvoiceDate = WODate;
            //InvoiceDate = string.Empty;
            //string pattern = @"\b(\d{4}[-/]\d{2}[-/]\d{2}|\d{2}[-/]\d{2}[-/]\d{4}|\d{1,2}(st|nd|rd|th)?\s+\w+\s+\d{4})\b";
            //string pattern1 = @"\b(?:\d{1,2}-\d{1,2}-\d{4}|\d{1,2}\d{4})\b";
            //MatchCollection matches = VccConstants.GetRegxMatch(dateStr, pattern1).ConfigureAwait(false).GetAwaiter().GetResult();

            //foreach(Match m in matches)
            //{
            //    InvoiceDate = m.Value;
            //}
            //matches = VccConstants.GetRegxMatch(dateStr,pattern).ConfigureAwait(false).GetAwaiter().GetResult();
            //foreach (Match m in matches)
            //{
            //    InvoiceDate = m.Value;
            //}

            int index = Array.FindIndex(inPutStr, line => (line.StartsWith("date")));
            if (index > 0)
            {

                int indDate = inPutStr[index].IndexOf(" ");
                if(indDate <=0 )
                {
                    WODate = inPutStr[++index];
                }
                else

                WODate = inPutStr[index].Substring(indDate);
                 if (!(DateTime.TryParse(WODate, out DateTime results)))
                    {
                        WODate = string.Empty;
                    }

                

            }

        }
        public string GetNewGuid()
        {
            return Guid.NewGuid().ToString();
        }
        private async Task<StringBuilder> CreateCSVHeader()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"City,Church,Book Type,Date Range,Pages,PDF FileName,OCR FileName,DownLoad Folder");
            return sb;
        }
        private async Task StartUpload(string batchID, string uploadFolder, string archiver)
        {
            return;
            Process p = new Process();
            try
            {


                // uploadFolder = uploadFolder.Replace("{ProjectName}", "").Trim();


                p.StartInfo.FileName = UploadExe;
                p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                if (ShowCmdWindow)
                    p.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;

                // p.StartInfo.Arguments = string.Format("/batchid:{0} /archiver:{1}", batchID, archiver);
                p.StartInfo.Arguments = string.Format(UploadExeParms, $"{Edocs_Utilities.DoubleQuotes}{batchID}{Edocs_Utilities.DoubleQuotes}", $"{Edocs_Utilities.DoubleQuotes}{uploadFolder}{Edocs_Utilities.DoubleQuotes}", archiver);
                EdocsUSA.Utilities.Logging.TraceLogger.TraceLoggerInstance.TraceInformation($"Starting process {p.StartInfo.FileName} with parms {p.StartInfo.Arguments}");
                p.Start();
                // await Task.Factory.StartNew(() => p.WaitForExit());
                if (ProcessWaitForExit)
                {
                    p.WaitForExit();
                    if (p.ExitCode != 0)
                    {
                        EdocsUSA.Utilities.Logging.TraceLogger.TraceLoggerInstance.TraceError($"running process {p.StartInfo.FileName} with parms {p.StartInfo.Arguments} exit code {p.ExitCode.ToString()}");
                        EdocsUSA.Utilities.Logging.TraceLogger.TraceLoggerInstance.TraceError($"running process {UploadExe} with parms {batchID} {uploadFolder}");
                        MessageBox.Show($"Process {UploadExe} with parms {batchID} {uploadFolder} did not start", "Error Uploading", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        throw new Exception(p.ExitCode.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                EdocsUSA.Utilities.Logging.TraceLogger.TraceLoggerInstance.TraceError($"running process {p.StartInfo.FileName} with parms {p.StartInfo.Arguments} exit code {p.ExitCode.ToString()} {ex.Message}");
                EdocsUSA.Utilities.Logging.TraceLogger.TraceLoggerInstance.TraceError($"running process {UploadExe} with parms {batchID} {uploadFolder}  {ex.Message}");
                throw new OperationCanceledException(ex.Message);
            }
        }
        private async Task LoadImage(SQPage page, System.Threading.CancellationToken cToken)
        {
            try
            {
                //   SQPage sQPage = document.Pages[0];
                //    SQImage image = sQPage.Image;
                //   SQImageEditLock image_Lock = image.BeginEdit();
                //   recordDialog.DOHImage.ClearAll(false, true);
                //   recordDialog.DOHImage.Add(image);
                //  recordDialog.DOHImage.NavigateTo(0);
                //   recordDialog.DOHImage.ActiveImageViewer.ScaleToFitHeight();
                //  image.DiscardEdit(image_Lock);
                
                    SQImage image = page.Image;
                    SQImageEditLock image_Lock = image.BeginEdit();
                  //  recordDialog.DOHImage.ClearAll(false, false);
                   // recordDialog.DOHImage.Add(image);
                   // recordDialog.DOHImage.NavigateTo(0);
                   // recordDialog.DOHImage.ActiveImageViewer.ScaleToFitHeight();
                    image.DiscardEdit(image_Lock);
                
            }
            catch (Exception ex)
            {
                throw new OperationCanceledException($"Error loading Image {ex.Message}");
            }
        }
    }

}
