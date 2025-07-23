using Scanquire.Public;
using Scanquire.Public.ArchivesConstants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EdocsUSA.Utilities.Extensions;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using EDL = EdocsUSA.Utilities.Logging;
using System.Text.RegularExpressions;
using Edocs.Ocr.Convert.Libaray.Img.PDF;
using Microsoft;
using static EdocsUSA.Utilities.Twain;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Scanquire.Clients.NYP
{
    public class DOHArchiver : AzureCloudArchiver
    {


        readonly string PatientID = "patient";
        readonly string PatientIdentifier = "dentifier";
        readonly string PatientIdentifiler = "dentifiler";
        // readonly string RegXAccessionNumber = @"\d+";
        readonly string RegXAccessionNumber = @"\d{1}-\d{2}-\d{3}-\d{5}";
        //  readonly string RegXAccessionNumber = @"[d-d-d]";
        readonly string ID = "id";
        public string OCRApiKey
        { get; set; }
        public string OCRWebApi
        { get; set; }
        public bool OCRISTable
        { get; set; }
        public bool OCRPatientID
        { get; set; }
        private string MedicalRecordNumber
        { get; set; }
        private string AccessionNumber
        { get; set; }
        private string CurrentAccessionNumber
        { get; set; }
        private string CurrentMRN
        { get; set; }
        private DateTime DateOfService
        { get; set; }
        public bool ShowDialogOCR
        { get; set; }
        private bool MoreThenOneDoc
        { get; set; }
        private SQDocument DohDocument
        { get; set; }
        public string RegXMatchMonth
        { get; set; }
        protected DOHRecordDialog recordDialog = new DOHRecordDialog();
        private bool ManuelIndexing
        { get; set; }
        private int TotalImages
        { get; set; }
        private DateTime CurrentDateOfService
        { get; set; }
        public override IEnumerable<Task<SQImage>> AcquireFromScannerForNew(IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken)
        {
            recordDialog.Clear();
            //   recordDialog.TryShowDialog(DialogResult.OK);
            return base.AcquireFromScannerForNew(progress, cToken);
        }

        public override async Task Send(IEnumerable<SQImage> images, IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken)
        {
            await Init();
            TotalImages = images.Count();
            if (TotalImages > 0)
                TotalImages = TotalImages / 2;
            await WriteBatchSettings(true);
            await base.Send(images, progress, cToken);

            if ((string.IsNullOrWhiteSpace(CurrentMRN) && (string.IsNullOrWhiteSpace(CurrentAccessionNumber))))
            {
                await GetAccMRN(DohDocument, progress, cToken);

            }

            else
            {
                if ((DohDocument != null) && (DohDocument.Pages.Count > 0))
                {

                    await base.Send(DohDocument, 1, progress, cToken);
                }
            }

            await SendDocuments();

            //   await GetFirstImage(images, progress, cToken);
            // await base.Send(images, progress, cToken);



        }
        private async Task Init()
        {
            DateOfService = DateTime.Now;
            AccessionNumber = string.Empty;
            MedicalRecordNumber = string.Empty;
            CurrentAccessionNumber = string.Empty;
            MoreThenOneDoc = false;
            CurrentMRN = string.Empty;
            DohDocument = new SQDocument();
            if (EnableAzureUpload)
                EnableAzureUpload = false;
            //   recordDialog.TryShowDialog(DialogResult.OK);

        }
        //private async Task GetFirstImage(IEnumerable<SQImage> images, IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken)
        //{
        //    foreach (var img in images)
        //    {
        //        SQImageEditLock image_Lock = img.BeginEdit();
        //        recordDialog.CurrentImage = (System.Drawing.Image)img.WorkingCopy;
        //        img.DiscardEdit(image_Lock);
        //        break;
        //    }

        //}
        public async Task GetAccMRN(SQDocument document, IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken)
        {
            //bool addPicture = true;
            //foreach (var img in images)
            //{
            //    SQImageEditLock image_Lock = img.BeginEdit();
            //    SQPage page = new SQPage(img);
            //    if (addPicture)
            //    {

            //        addPicture = false;
            //        recordDialog.CurrentImage = (System.Drawing.Image)img.WorkingCopy;
            //    }
            //    DohDocument.Pages.Add(page);
            //    img.DiscardEdit(image_Lock);

            //}
            //  recordDialog.DOHImage.li
            //  recordDialog.CurrentImage = document.Pages[0].Image.LatestRevision.GetOriginalImageBitmap();
            await LoadImage(document, cToken);
            await OpenDOHDialog();
            CurrentAccessionNumber = AccessionNumber;
            CurrentMRN = MedicalRecordNumber;
            CurrentDateOfService = DateOfService;
            await base.Send(DohDocument, 1, progress, cToken);
        }
        private async Task LoadImage(SQDocument document, System.Threading.CancellationToken cToken)
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
                foreach (SQPage page in document.Pages)
                {
                    SQImage image = page.Image;
                    SQImageEditLock image_Lock = image.BeginEdit();
                   recordDialog.DOHImage.ClearAll(false, false);
                    recordDialog.DOHImage.Add(image);
                    recordDialog.DOHImage.NavigateTo(0);
                    recordDialog.DOHImage.ActiveImageViewer.ScaleToFitHeight();
                 image.DiscardEdit(image_Lock);
                    break;
                }
            }
            catch (Exception ex)
            {
                throw new OperationCanceledException($"Error loading Image {ex.Message}");
            }
        }
        public override async Task Send(SQDocument document, int documentNumber, IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken)
        {

            DateTime dateOfService = DateTime.Now;
            SQCommand_Document_IndexField[] indexFieldCommands = document.Commands.OfType<SQCommand_Document_IndexField>().ToArray();
            SQCommand_Document_IndexField accessionNumberFieldCommand = indexFieldCommands.Where(c => c.Name.Equals(JsonsFieldConstants.JsonFieldAccessionNumber, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            AccessionNumber = (accessionNumberFieldCommand == null)
                 ? null
                : (string)accessionNumberFieldCommand.Value;
            ManuelIndexing = (string.IsNullOrWhiteSpace(AccessionNumber));

            await OCRImage(document, documentNumber, progress, cToken);
            if (((string.IsNullOrWhiteSpace(AccessionNumber))) || (string.IsNullOrWhiteSpace(MedicalRecordNumber)))
            {
                if (documentNumber == 1)
                    ManuelIndexing = true;
            }
            ManuelIndexing = true;
            if (ManuelIndexing)
            {
                await LoadImage(document, cToken);
                await OpenDOHDialog();

            }
            if (CurrentDateOfService.Year < 2000)
                CurrentDateOfService = DateOfService;
            if ((!(string.IsNullOrWhiteSpace(AccessionNumber))) && (string.IsNullOrWhiteSpace(CurrentAccessionNumber)))
            {
                AccessionNumber = await RemoveLastDigit(AccessionNumber);
                CurrentAccessionNumber = AccessionNumber;
            }
            if ((!(string.IsNullOrWhiteSpace(AccessionNumber))) && (string.Compare(AccessionNumber, CurrentAccessionNumber, true) != 0))
            {

                await base.Send(DohDocument, documentNumber, progress, cToken);
                DohDocument.Pages.Clear();
            }
            if ((string.IsNullOrWhiteSpace(CurrentMRN)) && (!(string.IsNullOrWhiteSpace(MedicalRecordNumber))))
                CurrentMRN = MedicalRecordNumber;

            if ((!(string.IsNullOrWhiteSpace(MedicalRecordNumber))) && (string.Compare(MedicalRecordNumber, CurrentMRN, true) != 0))
            {


                await base.Send(DohDocument, documentNumber, progress, cToken);
                DohDocument.Pages.Clear();
            }

            if (document.Pages.Count == 2)
            {
                DohDocument.Pages.Add(document.Pages[0]);
                DohDocument.Pages.Add(document.Pages[1]);
            }
            else
                DohDocument.Pages.Add(document.Pages[0]);
            //   if ((string.IsNullOrWhiteSpace(CurrentAccessionNumber))) && (!(string.IsNullOrWhiteSpace(AccessionNumber))))



        }
        private async Task OpenDOHDialog()
        {

            if (string.IsNullOrWhiteSpace(AccessionNumber))
                AccessionNumber = "0000000000";

            if (string.IsNullOrWhiteSpace(MedicalRecordNumber))
                MedicalRecordNumber = "0000000000";

            if (string.IsNullOrWhiteSpace(CurrentAccessionNumber))
                recordDialog.PrevAccessionNumber = "0000000000";
            else
                recordDialog.PrevAccessionNumber = CurrentAccessionNumber;

            if (string.IsNullOrWhiteSpace(CurrentMRN))
                recordDialog.PrevMRN = "0000000000";
            else
                recordDialog.PrevMRN = CurrentMRN;

            if (string.IsNullOrWhiteSpace(MedicalRecordNumber))
                MedicalRecordNumber = "0000000000";
            if (string.Compare(DateOfService.ToString("MM-dd-yyyy"), DateTime.Now.ToString("MM-dd-yyyy"), false) == 0)
            {
                if (CurrentDateOfService.Year > 2000)
                    DateOfService = CurrentDateOfService;
            }

            recordDialog.AccessionNumber = AccessionNumber;
            recordDialog.MedicalRecordNumber = MedicalRecordNumber;
            recordDialog.DateOfService = DateOfService;
            DateTime tempDate = DateOfService;
            recordDialog.TryShowDialog(DialogResult.OK);

            AccessionNumber = recordDialog.AccessionNumber.ToUpper();
            MedicalRecordNumber = recordDialog.MedicalRecordNumber.ToUpper();

            if (string.Compare(DateOfService.ToString("MM-dd-yyyy"), tempDate.ToString("MM-dd-yyyy"), true) != 0)
                DateOfService = recordDialog.DateOfService.Value;
            if (!(string.IsNullOrWhiteSpace(AccessionNumber)))
                AccessionNumber = await RemoveLastDigit(AccessionNumber);

        }
        private async Task WriteBatchSettings(bool azureBatch)
        {
            CurrentBatchId = GenerateNewBatchId();
            DateTime batchTime = DateTime.Now;
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Starting DOH archiver for batchid:{CurrentBatchId.ToString()} batchtime:{batchTime.ToString()}");

            if (azureBatch)
            {
                Dictionary<string, object> sharepointBatchSettings = JsonSettings.GetJsonBatchSettings(CurrentBatchId, batchTime.ToUniversalTime().ToString(SHAREPOINT_DATETIME_FORMAT), base.ScanStationMachineName, string.Empty, base.AzureDataBaseName, base.AzureShareName, base.AzureTableName, base.AzureSPName, base.AzureWebApiController);
                SharepointBatchHelper.WriteSettings(CurrentSharepointBatchDir, sharepointBatchSettings);

            }
        }

        public override async Task Send(SQFile sharepointFile, SQFile optixFile, int fileNumber, IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken)
        {


            string fileName = Guid.NewGuid().ToString();

            //if (EnableAzureBatch)
            //{
            MoreThenOneDoc = true;
            await CheckAccessionNumber();
            if (CurrentDateOfService.Year < 2000)
                CurrentDateOfService = DateTime.Now;
            //if (string.Compare(DateOfService.ToString("MM-dd-yyyy"), CurrentDateOfService.ToString("MM-dd-yyyy"), false) == 0)
            //{
            //    CurrentDateOfService = DateTime.Now;
            //}
            //   EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Creating Azure batch for DOH archiver for batchid:{CurrentBatchId.ToString()}  accessionNumber:{AccessionNumber}  //medicalRecordNumber:{MedicalRecordNumber} dateOfService{dateOfService.ToString()}");
            Dictionary<string, string> sharepointCommonFields = new Dictionary<string, string>();
            Dictionary<string, string> sharepointRecordFields = new Dictionary<string, string>();

            sharepointRecordFields[JsonsFieldConstants.JsonFieldAccessionNumber] = CurrentAccessionNumber;
            sharepointRecordFields[JsonsFieldConstants.JsonFieldMedicalRecordNumber] = CurrentMRN;
            sharepointRecordFields[JsonsFieldConstants.JsonFieldDateOfService] = CurrentDateOfService.ToUniversalTime().ToString(SHAREPOINT_DATETIME_FORMAT);

            string sharepointRecordFileName = Path.ChangeExtension(fileName, sharepointFile.FileExtension);
            SharepointBatchRecord sharepointBatchRecord = new SharepointBatchRecord(sharepointRecordFileName, sharepointRecordFields);
            await Task.Factory.StartNew(() =>
             { SharepointBatchHelper.WriteRecord(CurrentSharepointBatchDir, sharepointBatchRecord, sharepointFile.Data); });

            CurrentDateOfService = DateOfService;
            if (string.Compare(CurrentDateOfService.ToString("MM-dd-yyyy"), DateOfService.ToString("MM-dd-yyyy"), true) == 0)
            {
                DateOfService = DateTime.Now;
                CurrentDateOfService = CurrentDateOfService.AddYears(-30);
            }
            else
            {
                if (string.Compare(CurrentDateOfService.ToString("MM-dd-yyyy"), DateTime.Now.ToString("MM-dd-yyyy"), true) == 0)
                {

                    CurrentDateOfService = CurrentDateOfService.AddYears(-30);
                }
            }
            CurrentAccessionNumber = AccessionNumber;
            CurrentMRN = MedicalRecordNumber;

            //  }

        }
        private async Task<bool> GetDateofService(string dateOfServiceStr)
        {
            if (Regex.IsMatch(dateOfServiceStr, RegXMatchMonth, RegexOptions.IgnoreCase))
            {
                foreach (string s in dateOfServiceStr.Split('\t'))
                {
                    if (Regex.IsMatch(s, RegXMatchMonth, RegexOptions.IgnoreCase))
                    {
                        if (s.Length > 7)
                            continue;
                        if (DateTime.TryParse(s, out DateTime restlts))
                        {
                            if ((restlts.Year > (DateTime.Now.Year + 3)) || (restlts.Year < (DateTime.Now.Year - 3)))
                                return false;
                            DateOfService = restlts;
                            return true;
                        }

                    }
                }

            }
            return false;
        }
        private async Task SendDocuments()
        {
            return;

            await Task.Factory.StartNew(() =>
            {
                // progress.Report(new ProgressEventArgs(0, 0, "Uploading to Azure Cloud"));
                EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Uploading to Azure Cloud");
                Process sharepointUploadProcess = new Process();
                sharepointUploadProcess.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
                sharepointUploadProcess.StartInfo.FileName = AzureUploadScriptPath;
                string archiveFolder = System.IO.Path.GetFullPath(AzureBatchRootDir).TrimEnd(System.IO.Path.DirectorySeparatorChar);
                archiveFolder = archiveFolder.Split(System.IO.Path.DirectorySeparatorChar).Last();
                EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Starting process {AzureUploadScriptPath} with args /batchid:{CurrentBatchId} /archiver:{archiveFolder}");
                sharepointUploadProcess.StartInfo.Arguments = ($"/batchid:{CurrentBatchId} /archiver:{archiveFolder}");
                if (!(sharepointUploadProcess.Start()))
                {
                    EDL.TraceLogger.TraceLoggerInstance.TraceError($"Process {AzureUploadScriptPath} with args /batchid:{CurrentBatchId} /archiver:{archiveFolder} did not start:{sharepointUploadProcess.ExitCode.ToString()}");
                    throw new OperationCanceledException("Azure Cloud upload failed with " + sharepointUploadProcess.ExitCode.ToString());
                }

                if (WaitForAzureUpload)
                {
                    sharepointUploadProcess.WaitForExit();
                    if (sharepointUploadProcess.ExitCode != 0)
                    {
                        EDL.TraceLogger.TraceLoggerInstance.TraceError($"Process {AzureUploadScriptPath} with args /batchid:{CurrentBatchId} /archiver:{archiveFolder} exited with code:{sharepointUploadProcess.ExitCode.ToString()}");
                        throw new OperationCanceledException("Azure Cloud failed with " + sharepointUploadProcess.ExitCode.ToString());
                    }
                }
            });

        }

        private async Task OCRImage(SQDocument document, int documentNumber, IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken)
        {

            //  string checkingMess = $"Checking for new document document numnber {document} total documents {TotalImages}";
            // string checkingMess = $"Looking For New Accession # File {documentNumber} of {TotalImages}";
            string checkingMess = $"Looking For New Accession #";
            progress.Report(new EdocsUSA.Utilities.ProgressEventArgs(0, 0, checkingMess));
            ManuelIndexing = true;
            AccessionNumber = string.Empty;
            MedicalRecordNumber = string.Empty;
            ManuelIndexing = true;
            return;

            foreach (SQPage page in document.Pages)
            {
                try
                {
                    System.Drawing.Bitmap bitmap = page.Image.LatestRevision.GetOriginalImageBitmap();
                    byte[] imageByte = ConvertImagePDF.ImageToBase64(bitmap, System.Drawing.Imaging.ImageFormat.Png);
                    Uri uri = new Uri(OCRWebApi);
                    //  string file = @"C:\Archives\newlwbwel_v000.pdf";
                    // string OCRResults = await ConvertImagePDF.OCRSrace(file,false,false, OCRApiKey, "1", OCRISTable, uri);
                 //   OCRISTable = false;
                      string OCRResults = await ConvertImagePDF.OCRSrace(imageByte, true, false, OCRApiKey, "2", OCRISTable, uri, "image.png");

                    await GetPatientID(OCRResults);
                    break;
                }
                catch (Exception ex)
                {
                    EDL.TraceLogger.TraceLoggerInstance.TraceError($"OCRImage Looking For New Accession # {ex.Message}");
                    ManuelIndexing = true;
                }
            }
            checkingMess = $"File {documentNumber} of {TotalImages}";
            progress.Report(new EdocsUSA.Utilities.ProgressEventArgs(0, 0, checkingMess));
            if (ManuelIndexing)
            {
                checkingMess = $"Error OCR {documentNumber}";
                progress.Report(new EdocsUSA.Utilities.ProgressEventArgs(0, 0, checkingMess));
            }


        }
        private async Task GetPatientID(string OCRResults)
        {

            string ocrAccNum = AccessionNumber;
            AccessionNumber = string.Empty;
            MedicalRecordNumber = string.Empty;
            ManuelIndexing = true;

            try
            {
                //if (string.Compare(DateOfService.ToString("MM-dd-yyyy"), DateTime.Now.ToString("MM-dd-yyyy"), true) != 0)
                //    getDateOfService = false;
                EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Mehod GetPatientID() OCR image to get AccessionNumber Patient id");

                if (!(string.IsNullOrWhiteSpace(OCRResults)))
                {

                    string[] patID = OCRResults.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);


                    foreach (var str in patID)
                    {
                        if (string.IsNullOrWhiteSpace(AccessionNumber))
                        {

                            MatchCollection match = Regex.Matches(str, RegXAccessionNumber, RegexOptions.IgnoreCase);
                            if (match.Count != 0)
                            {
                                AccessionNumber = await GetAccessionNumber(str);

                            }


                            if (string.IsNullOrWhiteSpace(AccessionNumber))
                                EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"AccessionNumber not found in string {str}");
                            else
                                EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Found AccessionNumber {AccessionNumber} in string {str}");
                        }

                        //if (getDateOfService)
                        //    getDateOfService = await GetDateofService(str);
                        // if (str.ToLower().Contains(ID))
                        if (str.ToLower().StartsWith(PatientID))
                        {

                            string[] idPat = str.Split('\t');
                            if (idPat.Count() > 0)
                            {

                                if ((idPat[0].ToLower().IndexOf(PatientIdentifier) > 0) || (idPat[0].ToLower().IndexOf(PatientIdentifiler) > 0))
                                {
                                    await GetMRN(idPat, 0, 1);
                                    if (!(string.IsNullOrWhiteSpace(MedicalRecordNumber)))
                                    {
                                        if (string.Compare(CurrentMRN, MedicalRecordNumber, true) != 0)
                                        {

                                            ManuelIndexing = true;
                                            EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Found PatientLdentifier MRN {MedicalRecordNumber}");

                                        }
                                        break;
                                    }
                                    //string[] idenPat = idPat[0].Split(' ');
                                    //if (idenPat.Count() == 3)
                                    //{
                                    //    //   char[] dif = idPat[1].ToCharArray();
                                    //    if (Char.IsDigit(idenPat[2], 0))
                                    //    {
                                    //        if (string.Compare(CurrentMRN, idenPat[2], true) != 0)
                                    //        {
                                    //            MedicalRecordNumber = idenPat[2];
                                    //            ManuelIndexing = true;
                                    //            EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Found PatientLdentifier MRN {MedicalRecordNumber}");
                                    //            break;
                                    //        }
                                    //    }
                                    //}
                                }
                                if (idPat[0].ToLower().StartsWith(PatientID))
                                {
                                    await GetMRN(idPat, 1, 2);
                                    if (!(string.IsNullOrWhiteSpace(MedicalRecordNumber)))
                                        break;

                                    //                                        if (idPat.Count() == 3)
                                    //{
                                    //    //   char[] dif = idPat[1].ToCharArray();
                                    //    if (Char.IsDigit(idPat[1], 0))
                                    //    {
                                    //        MedicalRecordNumber = idPat[1];

                                    //        EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Found MRN {MedicalRecordNumber}");
                                    //        break;
                                    //    }
                                    //}
                                }
                            }

                        }

                    }


                    if (!(string.IsNullOrWhiteSpace(AccessionNumber)))
                        AccessionNumber = AccessionNumber.Replace("-", "").Trim();
                    if (!(string.IsNullOrWhiteSpace(ocrAccNum)))
                    {
                        ocrAccNum = await RemoveLastDigit(ocrAccNum);
                        if (string.IsNullOrWhiteSpace(AccessionNumber))
                        {
                            AccessionNumber = ocrAccNum;
                            if (ocrAccNum.StartsWith("11"))
                            {

                                ManuelIndexing = true;
                            }
                        }
                        else
                        {


                            AccessionNumber = await RemoveLastDigit(AccessionNumber);
                            if (string.Compare(ocrAccNum, AccessionNumber, true) != 0)
                            {
                                ManuelIndexing = await CompareLastDigits(ocrAccNum, AccessionNumber, 6);
                                //if (await FixBarCode(ocrAccNum, AccessionNumber))
                                //{
                                //    // AccessionNumber = ocrAccNum;
                                //    ManuelIndexing = true;
                                //}
                            }
                        }

                        //if (string.Compare(ocrAccNum.Substring(0, 3), AccessionNumber.Substring(0,3), false) != 0)
                        //{
                        //    AccessionNumber = ocrAccNum;
                        //    ManuelIndexing = true;
                        //}

                        //else
                        //{
                        //    if (ocrAccNum.StartsWith("11"))
                        //    {
                        //        AccessionNumber = ocrAccNum;
                        //        ManuelIndexing = true;
                        //    }
                        //}
                    }

                    if ((string.IsNullOrWhiteSpace(AccessionNumber)) && (!(string.IsNullOrWhiteSpace(MedicalRecordNumber))))
                    {
                        if (string.Compare(MedicalRecordNumber, CurrentMRN, true) != 0)
                            ManuelIndexing = true;
                    }

                    if (!(string.IsNullOrWhiteSpace(AccessionNumber)))
                    {
                        if (string.IsNullOrWhiteSpace(MedicalRecordNumber))
                        {
                            //   if ((string.IsNullOrWhiteSpace(CurrentMRN)))
                            //  {
                            ManuelIndexing = true;
                            // }
                        }
                        if ((AccessionNumber.Length < 10) || (AccessionNumber.Length > 12))
                            ManuelIndexing = true;
                        if (string.Compare(CurrentAccessionNumber, AccessionNumber, true) != 0)
                        {
                            DateOfService = DateTime.Now;
                            //  string[] dofService = OCRResults.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (var str in patID)
                            {
                                bool getDateOfService = await GetDateofService(str);
                                if (getDateOfService)
                                    break;

                            }

                            //  if (string.Compare(DateOfService.ToString("MM-dd-yyyy"), CurrentDateOfService.ToString("MM-dd-yyyy"), false) == 0)

                            //   DateOfService = DateTime.Now;
                        }
                    }
                    if ((!(string.IsNullOrWhiteSpace(MedicalRecordNumber))) && (MedicalRecordNumber.Length < 7))
                        ManuelIndexing = true;
                }
            }
            catch (Exception ex)
            {
                EDL.TraceLogger.TraceLoggerInstance.TraceError($"Mehod GetPatientID() OCR image to get AccessionNumber Patient id {ex.Message}");
                ManuelIndexing = true;
            }

        }
        private async Task GetMRN(string[] mrn, int startCount, int stopCount)
        {
            int numLoop = startCount;
            for (int i = startCount; i < mrn.Count(); i++)
            {
                if (i == stopCount)
                {
                    if (!(string.IsNullOrWhiteSpace(MedicalRecordNumber)))
                    {
                        MedicalRecordNumber = MedicalRecordNumber.Trim();
                        if (MedicalRecordNumber.Length < 7)
                            MedicalRecordNumber = string.Empty;
                        return;
                    }
                }
                numLoop++;
                string idStr = mrn[i];
                for (int j = 0; j < idStr.Length; j++)
                {


                    if (Char.IsDigit(idStr[j]))
                    {
                        MedicalRecordNumber += idStr[j];

                    }
                    else
                    {
                        if (char.IsWhiteSpace(idStr[j]))
                            continue;
                        if (!(string.IsNullOrWhiteSpace(MedicalRecordNumber)))
                        {
                            MedicalRecordNumber = MedicalRecordNumber.Trim();
                            if (MedicalRecordNumber.Length < 7)
                                MedicalRecordNumber = string.Empty;
                            return;
                        }
                    }

                }


            }
        }
        private async Task<bool> FixBarCode(string bCode, string accBarCode)
        {
            string newAccum = string.Empty;
            try
            {
                if (!(ManuelIndexing))
                {
                    string missingDigit = string.Empty;
                    bCode = bCode.Remove(0, 1);
                    char[] barCode = bCode.ToCharArray();
                    char[] aNumber = accBarCode.ToCharArray();

                    for (int i = 0; i < aNumber.Length; i++)
                    {
                        if (aNumber[i] == barCode[i])
                        {

                            newAccum += barCode[i];

                        }
                        else
                        {
                            newAccum += aNumber[i];
                            for (int j = 0; j < bCode.Length; j++)
                                newAccum += bCode[j];
                            break;
                            //if (aNumber[i] == '-')
                            //{
                            //    newAccum += bCode[i];
                            //}
                            //else
                            //{
                            //    newAccum += aNumber[i];
                            //    newAccum += bCode.Substring(i - 1);
                            //    break;
                            //  AccessionNumber = newAccum.Trim();
                            // ManuelIndexing = false;
                            //  }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                EDL.TraceLogger.TraceLoggerInstance.TraceError($"OCRImage Looking For New Accession # {ex.Message}");
                return true; ;
            }
            if (string.Compare(newAccum, accBarCode, true) != 0)
                return true;
            return false;
        }

        private async Task<bool> CompareLastDigits(string bCode, string accBarCode, int numDigits)
        {
            string newAccum = string.Empty;
            try
            {
                if (!(ManuelIndexing))
                {
                    bCode = bCode.Substring(bCode.Length - numDigits);
                    accBarCode = accBarCode.Substring(accBarCode.Length - numDigits);
                    if (string.Compare(bCode, accBarCode, true) != 0)
                        return true;
                }
            }
            catch (Exception ex)
            {
                EDL.TraceLogger.TraceLoggerInstance.TraceError($"OCRImage Looking For New Accession # {ex.Message}");
                return true; ;
            }

            return false;
        }
        private async Task<string> GetAccessionNumber(string aNumber)
        {
            string[] numberAcc = aNumber.Split('\t');
            string retStr = string.Empty;
            MatchCollection match = Regex.Matches(aNumber, RegXAccessionNumber, RegexOptions.IgnoreCase);
            if (match.Count == 1)
            {
                return match[0].Value;
            }

            foreach (var str in numberAcc)
            {
                if (str.Contains('-'))
                {
                    retStr = str.Replace("-", "").Trim();
                    if ((char.IsDigit(retStr, 0)) && (retStr.Length <= 12))
                        return retStr.Trim();
                }
                retStr = string.Empty;
            }
            return retStr;
        }
        private async Task CheckAccessionNumber()
        {
            if (string.IsNullOrWhiteSpace(CurrentMRN))
                CurrentMRN = "0000000000";
            if (string.IsNullOrWhiteSpace(CurrentAccessionNumber))
                CurrentAccessionNumber = "0000000000";

            char lastCharacter = CurrentAccessionNumber[CurrentAccessionNumber.Length - 1];

            CurrentAccessionNumber = await RemoveLastDigit(CurrentAccessionNumber);

        }

        private async Task<string> RemoveLastDigit(string str)
        {
            char lastCharacter = str[str.Length - 1];
            if (!(char.IsDigit(lastCharacter)))
                return str.Substring(0, (str.Length - 1));
            return str;
        }
    }


}
