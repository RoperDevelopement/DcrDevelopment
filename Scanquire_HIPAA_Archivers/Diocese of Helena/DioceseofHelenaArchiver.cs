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
using Edocs.Ocr.Convert.Libaray.Img.PDF;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
//L:\EdocsGItHub\Scanquire_V8\Scanquire\bin\Debug\Edocs.Icr.Ocr.Google\Edocs.Icr.Ocr.Google.exe /mt:image/png /pdfimg:D:\Diocese_Of_Helena_Images\JPG\5b4e0108-533f-465d-813d-8b0a990e2f6d.png /of:D:\Diocese_Of_Helena_Images\JPG\Txt
namespace Edocs.Diocese.Of.Helena.Archiver
{
    public class DioceseOfHelenaArchiver : SQArchiverBase
    {
        public DioceseofHelenaArchiverDialog InputDialog = new DioceseofHelenaArchiverDialog();
        private readonly string RecordsJson = "_records.json";
        private readonly string SettingsJson = "_settings.json";
        private readonly string Csv = ".csv";
        private readonly string LCROCTxtFile = "_lcrocr.txt";
        private readonly string ArchiverValueAddress = "address";
        private readonly string ArchiverValuePermitOwner = "permit owner|permit";

        private readonly string ArchiverValueContractor = "contractor|con";
        private readonly string ArchiverValueZone = "zone";
        private readonly string ArchiverValueCode = "code";
        private readonly string ArchiverValueDatePermit = "Date Permit";
        private readonly string ArchiverValueExcPermit = "ExcPermit";
        private readonly string ArchiverValueOwnerLot = "OwnerLot|owner|own";
        private readonly string ArchiverValueParcel = "Parcel";


        int documentNumber = 0;
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
        private SQDocument BSBDocument
        { get; set; }
        private JsonFileDictionaryLogger Log
        { get; set; }
        private int DocICROCR
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
        public override IEnumerable<System.Threading.Tasks.Task<SQImage>> AcquireFromScannerForNew(IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken)
        {


            //  InputDialog.TryShowDialog(DialogResult.OK);
            return base.AcquireFromScannerForNew(progress, cToken);
        }
        public override async Task Send(SQDocument document, int documentNumber, IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken)
        {
            ShowDialog = true;
            FileNumber = 0;
            ImgNumber = 0;
            ImgFolder = "NA";
            BSBDocument.Pages.Clear();
            if (CreateJpgImg)
                Edocs_Utilities.EdocsUtilitiesInstance.DeleteFiles(LCROCROutPutFolder);
            try
            {
                foreach (var page in document.Pages)
                {
                    documentNumber++;
                    if(CreateJpgImg)
                     await ICROCRImage(page, documentNumber, progress, cToken, LCROCROutPutFolder);
                     BSBDocument.Pages.Add(page);
                    await base.Send(BSBDocument, documentNumber, progress, cToken);
                   BSBDocument.Pages.Clear();

                }
               StartUpload(LFname, DirPath, "DOH").ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                throw new OperationCanceledException();
            }


        }
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
            BSBDocument = new SQDocument();
            string checkingMess = $"Getting Lcr Ocr txt";
            progress.Report(new ProgressEventArgs(1, images.Count(), checkingMess));
          //  ICROCRImage(images, documentNumber, progress, cToken, LCROCROutPutFolder).ConfigureAwait(false).GetAwaiter().GetResult();
            SaveFolder = GetNewGuid();
            
            Log = new JsonFileDictionaryLogger();
            await base.Send(images, progress, cToken);
        }
        public async override Task Send(SQFile file, int fileNumber, IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cancelToken)
        // public override async Task Send(SQDocument document, int documentNumber, IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken){
       
        // OpenFileDialog _OpenFileDialog = new OpenFileDialog() { RestoreDirectory = true, Multiselect = true };
        // string[] paths = _OpenFileDialog.SelectFiles(null, null);
        //   EdocsUSA.Utilities.Extensions.OpenFileDialogExtensions.TryShowDialog(null,null);
        //await OCRImage(document, documentNumber, progress, cToken);
        { 
          
        string upLoadDir = string.Empty;
            InputDialog.Text = DisPlayArchiverName;
            FoundDate = false;
            InputDialog.SqlConnection = SqlConnection;
            InputDialog.EdocsCustomerID = EdocsCustomerID;
            string fileName = string.Empty;
           //  InputDialog.ShowConfirmDialogBox = ShowConfirmDialogBox;
           //  InputDialog.CmboxNumbers = BoxNumber;
           InputDialog.IncludeBlankDocs = IncludeBlankDocs;
            InputDialog.ShowTotalDocsScanned = ShowTotalDocsScanned;
            InputDialog.ImageFolder = ImgFolder;
            InputDialog.ChurchName = Church;
            InputDialog.ChurchCity = ChurchCity;
            InputDialog.ChurchBookType = BookType;

            if(ShowDialog)
            { 
                if (InputDialog.TryShowDialog(DialogResult.OK) != DialogResult.OK)
                    throw new OperationCanceledException();
                fileName = $"{InputDialog.City} {InputDialog.Church} {InputDialog.BookType} {InputDialog.DateRangeSDate} thru {InputDialog.DateRangeEDate}";
                ShowDialog = false;
                LFname = fileName;
            }
           else
                fileName = $"{InputDialog.City} {InputDialog.Church} {InputDialog.BookType} {InputDialog.DateRangeSDate} thru {InputDialog.DateRangeEDate}_{FileNumber++}";



            SaveFolder = $"{InputDialog.City}\\{InputDialog.Church}\\{InputDialog.BookType}";
            DirPath = Path.Combine(SaveRootPath, SaveFolder);

             

            //  string fileName = GetNewGuid();
             // fileName = $"{InputDialog.City} {InputDialog.Church} {InputDialog.BookType} {InputDialog.DateRangeSDate} thru {InputDialog.DateRangeEDate}";

            string fileNameWithExt = Path.ChangeExtension(fileName, file.FileExtension);

            Path.ChangeExtension(fileName, file.FileExtension);

            string filePath = Path.Combine(DirPath, fileNameWithExt);


            Dictionary<string, string> recordsJs = new Dictionary<string, string>();
            if (!(Edocs_Utilities.EdocsUtilitiesInstance.CHeckFileExists(Path.Combine(DirPath, $"{LFname}{SettingsJson}"))))
            {
                Dictionary<string, string> settingsJs = new Dictionary<string, string>();
                settingsJs["ScanBatchID"] = fileName;
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
                settingsJs["AzureUpLoadContanier"] = $"{InputDialog.City.ToLower()}/{InputDialog.Church.ToLower()}/{InputDialog.BookType.ToLower()}";
                settingsJs["ImgOrgFileName"] = ImgFolder;
                settingsJs["DownLoadSubFolder"] = SaveFolder;
                //settingsJs["LCROCTxtFile"] = Path.Combine(dirPath, $"{saveFolder}{LCROCTxtFile}");
                settingsJs["TotalScanned"] = InputDialog.TotalScanned;
                settingsJs["TotalPageCount"] = InputDialog.TotalScanned;
                settingsJs["StandardLargeDocument"] = InputDialog.LargeDoc.ToString();
                
                Log.FilePath = Path.Combine(DirPath, $"{LFname}{SettingsJson}");

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

            recordsJs["FileName"] = fileNameWithExt;
            recordsJs["City"] = InputDialog.City;
            recordsJs["Church"] = InputDialog.Church;
            recordsJs["BookType"] = InputDialog.BookType;
          
            recordsJs["DateRangeStartDate"] = InputDialog.DateRangeSDate;
            recordsJs["DateRangeEndDate"] = InputDialog.DateRangeEDate;
          
            recordsJs["ImgFileName"] = ImgFolder;




            //recordsJs["LCROCTxtFile"] = Path.Combine(dirPath, $"{fileName}{LCROCTxtFile}");


            Log.FilePath = Path.Combine(DirPath, $"{LFname}{RecordsJson}");
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

        private async Task ICROCRImage(IEnumerable<SQImage> images, int documentNumber, IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken, string outPutFolder)
        {
            //foreach (SQImage page in document.)
            // {

            try
            {
                Init();
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
                Init();
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
        private async Task ICROCRImage(SQPage page, int documentNumber, IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken, string outPutFolder)
        {


            try
            {

                Init();
              //  SB = new StringBuilder();
               // outPutFolder = Path.Combine(outPutFolder, "Jpeg");
                Edocs_Utilities.EdocsUtilitiesInstance.CreateDir(outPutFolder);
                string checkingMess = "Creating JPeg Image";
                ImgFolder = string.Empty;


                try
                {
                    documentNumber++;

                       progress.Report(new EdocsUSA.Utilities.ProgressEventArgs(1, 1, checkingMess));

                    System.Drawing.Bitmap bitmap = page.Image.LatestRevision.GetOriginalImageBitmap();
                    // ImgFolder = System.IO.Path.Combine(outPutFolder, $"{Path.GetFileNameWithoutExtension(page.Image.LatestRevision.OriginalImageFilePath)}.jpeg");
                    ImgFolder = System.IO.Path.Combine(outPutFolder, $"{ImgNumber++}.jpeg");
                    bitmap.Save(ImgFolder, System.Drawing.Imaging.ImageFormat.Jpeg);
                    // GetOcrText(imgFolder).ConfigureAwait(false).GetAwaiter().GetResult();
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
                  //  lcrOcrText = lcrOcrText.Replace("\n", " ");
                    
                    SB.AppendLine(lcrOcrText);
                  
                    //GetArchiverValues(txtFile).ConfigureAwait(false).GetAwaiter().GetResult();
                    //   IndexValues(lcrOcrText).ConfigureAwait(false).GetAwaiter().GetResult();
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
        private  async Task<StringBuilder> CreateCSVHeader()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"City,Church,Book Type,Date Range,Pages,PDF FileName,OCR FileName,DownLoad Folder");
            return sb;
        }
        private async Task StartUpload(string batchID, string uploadFolder, string archiver)
        {
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
    }

}
