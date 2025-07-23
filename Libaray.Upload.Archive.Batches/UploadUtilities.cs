using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.IO;
using System.Windows.Forms;
using EdocsUSA.Utilities;
using EdocsUSA.Utilities.Extensions;
using edl = EdocsUSA.Utilities.Logging;
using FreeImageAPI;
using DebenuPDFLibraryDLL0915;
//using SE = ScanQuire_SendEmails;
using EM = Edocs.Send.Emails;
using Edocs.Libaray.Upload.Archive.Batches.Properties;
//using OCR = Edocs.Ocr.Convert.Libaray.Img.PDF;
using Edocs.Libaray.Upload.Archive.Batches.Models;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;
using System.Text.RegularExpressions;

namespace Edocs.Libaray.Upload.Archive.Batches
{

    public static class UploadUtilities
    {
        static readonly int milliSecond = 1000;
        public static readonly string JsonRecordsExten = "_records.json";
        public static readonly string JsonBatchSettingExten = "_settings.json";
        public const string ArgOCR = "/ocr:";
        public const string ArgOCRSPDF = "/spdf:";
        public const string ArgOCRInputFolder = "/inf:";
        public const string ArgOCRPDFBackUp = "/pdfbf:";
        public const string ArgOCROutputFolder = "/sfn:";
        public const string ArgOCRFileName = "/fn:";
        public const string ArgOCRErrorFileName = "/efn:";
        public static readonly string Quoat = "\"";
        public static int TotalOCR
        { get; set; }
        public static string AzureBlobAccountKey
        { get { return Settings.Default.AzureBlobAccountKey; } }
        public static string AzureBlobStorageConnectionString
        { get { return Settings.Default.AzureBlobStorageConnectionString; } }
        public static string AzureContainer
        { get { return Settings.Default.AzureContainer; } }
        public static string LCROCROutPutFolder
        { get { return Settings.Default.LCROCROutPutFolder; } }
        public static string LCROCRExe
        { get { return Settings.Default.LCROCRExe; } }
        public static string OCRApiKey
        { get { return Settings.Default.OCRApiKey; } }
        public static string OCRWebApi
        { get { return Settings.Default.OCRWebApi; } }
        public static bool OCRISTable
        { get { return Settings.Default.OCRISTable; } }

        public static string InputDir
        { get { return Settings.Default.ArchivesFolder; } }
        public static string EdocsWebApi
        { get { return Settings.Default.EdocsWebApi; } }
        public static bool DeleteBatchOnSuccess
        { get { return Settings.Default.DeleteBatchOnSuccess; } }
        public static string EdocsSupport
        { get { return Settings.Default.EdocsSupport; } }

        public static string JsonFilesBackUpFolder
        { get { return Settings.Default.JsonFilesBackUpFolder; } }

        public static bool MakeCopyUploadImage
        { get { return Settings.Default.MakeCopyUploadImage; } }
        public static string OCRSearchablePDFExe
        { get { return Settings.Default.OCRExe; } }

        public static string UploadedImagesCopyFolder
        { get { return Settings.Default.UploadedImagesCopyFolder; } }
        public static int DaysToKeepCopyFolder
        { get { return Settings.Default.DaysToKeepCopyFolder; } }
        public static string LogFolder
        { get { return (Settings.Default.LogFolder.Replace("{ApplicationDir}", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData))); } }

        public static string OverWriteFolder
        { get { return Settings.Default.OverWriteFolder; } }
        public static int DaysToKeepLogFiles
        { get { return Settings.Default.DaysToKeepLogFiles; } }

        public static int UploadLoop
        { get { return Settings.Default.LoopRetryUpLoad; } }
        public static int ThreadSleep
        { get { return Settings.Default.ThreadSleep; } }
        public static bool SendEmailErrorsOnly
        { get { return Settings.Default.SendEmailErrorsOnly; } }

        public static int ThreadSleepSeconds
        { get { return Settings.Default.ThreadSleepSeconds; } }

        public static int LoopNotify
        { get { return Settings.Default.LoopNotifyUser; } }

        public static async Task<string> CombPath(string str1, string str2)
        {
            return Path.Combine(str1, str2);
        }
        public static string BatchId
        { get; set; }
        public static string BatchDir
        { get; set; }
        public static async Task<string> GetArchiveName(string archiveFolder, string batchID)
        {
            Edocs_Utilities.EdocsUtilitiesInstance.CheckDirectExists(CombPath(archiveFolder, batchID).ConfigureAwait(false).GetAwaiter().GetResult());
            string fullPath = Path.GetFullPath(archiveFolder).TrimEnd(Path.DirectorySeparatorChar);
            string archiverName = fullPath.Split(Path.DirectorySeparatorChar).Last();
            return archiverName;
            // return CombPath(archiveFolder, batchID).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        public static async Task<string> GetSearchableTxt(string folder, string appendText)
        {
            try
            {
                if (Edocs_Utilities.EdocsUtilitiesInstance.CHeckFileExists(folder))
                    appendText += $"{appendText} {File.ReadAllText(folder)}";
                if (!string.IsNullOrWhiteSpace(appendText))
                {
                    appendText = appendText.Replace('/', '-');
                    appendText = appendText.Replace('\\', '-');
                    appendText = Regex.Replace(appendText, @"\r\n?|\n", " ").Trim();
                    File.WriteAllText(folder, appendText);
                }
            }
            catch { }
            return appendText;

        }
        public static async Task<string> GetBatchID(string archiveFolder)
        {
            Edocs_Utilities.EdocsUtilitiesInstance.CheckDirectExists(archiveFolder);
            string fullPath = Path.GetFullPath(archiveFolder).TrimEnd(Path.DirectorySeparatorChar);
            string archiverName = fullPath.Split(Path.DirectorySeparatorChar).Last();
            return archiverName;
            // return CombPath(archiveFolder, batchID).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        public static Dictionary<string, object> GetBatchSettings(string batchSettingsFile)
        {
            edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Getting batch setting for file{batchSettingsFile}");
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return (Dictionary<string, object>)(serializer.DeserializeObject(File.ReadAllText(batchSettingsFile)));
        }
        public static T GetBatchSettingsObject<T>(string batchSettingsFile, object className) where T : new()
        {
            edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Getting batch setting for file{batchSettingsFile}");
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            className = serializer.Deserialize<T>(File.ReadAllText(batchSettingsFile));
            return (T)className;
        }
        public static T GetBatchSettingsObject<T>(string batchSettingsFile) where T : new()
        {
            edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Getting batch setting for file{batchSettingsFile}");
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            object className = serializer.Deserialize<T>(File.ReadAllText(batchSettingsFile));
            return (T)className;
        }
        public static List<object> GetRecords<T>(ref int totRecodsRead, ref int totRecordsInFile, string batchRecordsFile)
        {
            edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Getting lab recs from file{batchRecordsFile}");
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<object> records = new List<object>();
            foreach (string line in File.ReadAllLines(batchRecordsFile))
            {
                totRecordsInFile++;
                if (string.IsNullOrWhiteSpace(line))
                { continue; }
                totRecodsRead++;
                object className = serializer.Deserialize<T>(line);
                records.Add(className);
                //        records.Add((Dictionary<string, object>)(serializer.DeserializeObject(line)));
            }
            return records;
        }

        //public static async Task<Uri> SaveTffFileAzureCloud(string batchDir, string azureContanier, string saveFileName)
        //{

        //    //  Edocs_Utilities.EdocsUtilitiesInstance.CHeckFileExists($"{batchDir}{uploadFileName}", false);
        //    byte[] pdfFile = UploadUtilities.ConvertToPdfFile($"{batchDir}{saveFileName}");
        //    saveFileName = $"{batchDir}{saveFileName.Replace(Path.GetExtension(saveFileName), ".pdf")}";
        //    //string pdfSaveFName = $"{uploadFolder}{uploadFileName}";
        //    //File.WriteAllBytes(pdfSaveFName, pdfFile);
        //    Uri url = AzureBlobStorage.BlobStorageInstance.UploadAzureBlob(saveFileName, azureContanier, pdfFile).GetAwaiter().GetResult();
        //    return url;
        //    //edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Saving pdf file to upload folder {pdfSaveFName}");

        //}
        public static async Task<string> SaveTffFileLocal(string batchDir, string destfoler, string saveFileName)
        {

            //  Edocs_Utilities.EdocsUtilitiesInstance.CHeckFileExists($"{batchDir}{uploadFileName}", false);
            byte[] pdfFile = UploadUtilities.ConvertToPdfFile($"{batchDir}{saveFileName}");
            destfoler = destfoler.Replace('/', '\\');
            Edocs_Utilities.EdocsUtilitiesInstance.CreateDir(destfoler);
            saveFileName = $"{destfoler}{saveFileName.Replace(Path.GetExtension(saveFileName), ".pdf")}";

            //string pdfSaveFName = $"{uploadFolder}{uploadFileName}";
            File.WriteAllBytes(saveFileName, pdfFile);
            // Uri url = AzureBlobStorage.BlobStorageInstance.UploadAzureBlob(saveFileName, azureContanier, pdfFile).GetAwaiter().GetResult();
            return saveFileName;
            //edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Saving pdf file to upload folder {pdfSaveFName}");

        }
        public static async Task<string> SavePdfLocalFile(string sourceFolder, string batchID, string destFolder, string saveFileName)
        {
            destFolder = destFolder.Replace('/', '\\');
            destFolder = $"{Edocs_Utilities.EdocsUtilitiesInstance.CheckDirectoryPath(destFolder)}{batchID}\\";

            Edocs_Utilities.EdocsUtilitiesInstance.CreateDir(destFolder);

            //  Edocs_Utilities.EdocsUtilitiesInstance.CHeckFileExists($"{sourceFolder}{uploadFileName}", false);
            //string pdfSFName = $"{Edocs_Utilities.EdocsUtilitiesInstance.CheckDirectoryPath(sourceFolder)}{saveFileName}";

            // destFolder = $"{destFolder}{saveFileName}";
            edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Copying sourefile:{sourceFolder} to destfile:{destFolder} file {saveFileName}");
            Edocs_Utilities.EdocsUtilitiesInstance.CopyFile(sourceFolder, destFolder, true, saveFileName, false);
            //Uri url = AzureBlobStorage.BlobStorageInstance.UploadAzureBlob(pdfSFName, destFolder).GetAwaiter().GetResult();
            return $"{destFolder}{saveFileName}";
        }

        private static void BackUpOverWriteFiles(string sourceFile)
        {
            edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Method BackUpOverWriteFiles source file:{sourceFile}");
            string destFile = $"{Path.GetFileNameWithoutExtension(sourceFile)}_{DateTime.Now.ToString("MM-dd-yyyy-HH-mm-ss")}.pdf";
            Edocs_Utilities.EdocsUtilitiesInstance.CreateDir(OverWriteFolder);
            destFile = Path.Combine(OverWriteFolder, destFile);
            edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Copy source file:{sourceFile} to {destFile}");
            File.Copy(sourceFile, destFile, false);
        }

        public static async Task<bool> SavePdfLocalFile(string sourceFile, string destFile)
        {
            if (!(Edocs_Utilities.EdocsUtilitiesInstance.CHeckFileExists(sourceFile)))
                throw new Exception($"File Not found {sourceFile}");
            bool overWrite = false;
            if (Path.HasExtension(destFile))
                Edocs_Utilities.EdocsUtilitiesInstance.CreateDir(Path.GetDirectoryName(destFile));
            else
                Edocs_Utilities.EdocsUtilitiesInstance.CreateDir(destFile);

            //  Edocs_Utilities.EdocsUtilitiesInstance.CHeckFileExists($"{sourceFolder}{uploadFileName}", false);
            //string pdfSFName = $"{Edocs_Utilities.EdocsUtilitiesInstance.CheckDirectoryPath(sourceFolder)}{saveFileName}";
            if (Edocs_Utilities.EdocsUtilitiesInstance.CHeckFileExists(destFile))
            {
                edl.TraceLogger.TraceLoggerInstance.TraceWarning($"Destfile:{destFile} exists");
                DialogResult dr = MessageBox.Show($"Overwrite file {destFile}", "File Exists", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    edl.TraceLogger.TraceLoggerInstance.TraceInformation($"BackingUP Destfile:{destFile}");
                    BackUpOverWriteFiles(destFile);
                    overWrite = true;
                    edl.TraceLogger.TraceLoggerInstance.TraceWarning($"User Overwrites Destfile:{destFile}");
                }

                //  else
                /// throw new Exception($"File exists {destFile} copy cancled");
            }
            // destFolder = $"{destFolder}{saveFileName}";
            edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Copying sourefile:{sourceFile} to destfile:{destFile}");
            File.Copy(sourceFile, destFile, overWrite);
            //  Edocs_Utilities.EdocsUtilitiesInstance.cCopyFile(sourceFolder, destFolder, true, saveFileName, false);
            //Uri url = AzureBlobStorage.BlobStorageInstance.UploadAzureBlob(pdfSFName, destFolder).GetAwaiter().GetResult();
            return overWrite;
        }

        //public static async Task<Uri> SavePdfFileAzureCloud(string sourceFolder, string destFolder, string saveFileName)
        //{
        //    string uploadFileName = saveFileName.Replace(Path.GetExtension(saveFileName), ".pdf");
        //    //  Edocs_Utilities.EdocsUtilitiesInstance.CHeckFileExists($"{sourceFolder}{uploadFileName}", false);
        //    string pdfSFName = $"{sourceFolder}{uploadFileName}";
        //  //  destFolder = $"{destFolder}{uploadFileName}";
        //    edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Copying sourefile:{pdfSFName} to destfile:{destFolder}");
        //    Uri url = AzureBlobStorage.BlobStorageInstance.UploadAzureBlob(pdfSFName, destFolder).GetAwaiter().GetResult();
        //    return url;
        //}


        public static List<Dictionary<string, object>> GetRecords(string batchRecordsFile, ref int totRecodsRead, ref int totRecordsInFile)
        {
            edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Getting lab recs from file{batchRecordsFile}");
            List<Dictionary<string, object>> records = new List<Dictionary<string, object>>();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            foreach (string line in File.ReadAllLines(batchRecordsFile))
            {
                totRecordsInFile++;
                if (string.IsNullOrWhiteSpace(line))
                { continue; }
                totRecodsRead++;
                records.Add((Dictionary<string, object>)(serializer.DeserializeObject(line)));
            }
            return records;
        }

        public static string GetBatchRecordsFileName(string batchDir, string batchId)
        {
            return Path.Combine(batchDir, ($"{batchId}{JsonRecordsExten}"));
        }
        public static int ExitCode
        { get; set; } = 0;
        public static string GetBatchSettingsFileName(string batchDir, string batchId)
        {
            return Path.Combine(batchDir, ($"{batchId}{JsonBatchSettingExten}"));
        }

        public static byte[] ConvertToPdfFile(string filePath)
        {

            byte[] tiffData = File.ReadAllBytes(filePath); ;
            return TiffToPdf(tiffData);

        }
        public static UploadBatchRecordsModel[] ReadRecords(string batchRecordsFile, ref int totalRecods)
        {
            JavaScriptSerializer Serializer = new JavaScriptSerializer();

            //if (Directory.Exists(batchRecordsFile) == false)
            //{ throw new DirectoryNotFoundException(batchRecordsFile); }


            string[] recordLines = File.ReadAllLines(batchRecordsFile);
            totalRecods = recordLines.Length;
            return (from string recordLine in recordLines select Serializer.Deserialize<UploadBatchRecordsModel>(recordLine)).ToArray();
        }

        public static byte[] TiffToPdf(byte[] tiff)
        {
            PDFLibrary pdf = new PDFLibrary();
            try
            {


                pdf.CompressImages((int)PDFLibrary.ImageCompression.None);

                pdf.SetOrigin((int)PDFLibrary.DocumentOrigin.TopLeft);
                pdf.SetMeasurementUnits((int)PDFLibrary.MeasurementUnit.Points);
                pdf.SetInformation((int)PDFLibrary.DocumentProperty.Creator, "Scanquire");
                pdf.SetInformation((int)PDFLibrary.DocumentProperty.Producer, "E-Docs USA");

                IEnumerator<FreeImageBitmap> imagesEnumerator = GetImagesFromTiff(tiff).GetEnumerator();
                if (imagesEnumerator.MoveNext() == false)
                { throw new Exception("No images in tiff"); }
                DrawImage(pdf, imagesEnumerator.Current);
                while (imagesEnumerator.MoveNext())
                {

                    pdf.NewPage();

                    DrawImage(pdf, imagesEnumerator.Current);
                }
                //int pC = pdf.PageCount();
                //for (int i = 1; i <= pC; i++)
                //{
                //    pdf.SelectPage(i);

                //    // pdf.CompressPage();
                //   int k= pdf.CompressContent();
                //}
                return pdf.SaveToString();
            }
            finally { pdf.ReleaseLibrary(); }
        }

        static IEnumerable<FreeImageBitmap> GetImagesFromTiff(byte[] data)
        {
            using (MemoryStream stream = new MemoryStream(data))
            using (FreeImageBitmap fib = new FreeImageBitmap(stream))
            {
                int frameCount = fib.FrameCount;
                int progressCurrent = 0;
                int progressTotal = frameCount;

                //Loop through the image's frames and yeild a new SQImage for each frame.			
                for (int i = 0; i < frameCount; i++)
                {
                    progressCurrent++;
                    fib.SelectActiveFrame(i);
                    yield return (FreeImageBitmap)(fib.Clone());
                }
            }
        }

        private static byte[] EncodeImage(FreeImageBitmap image)
        {
            FREE_IMAGE_FORMAT imageFormat;
            FREE_IMAGE_SAVE_FLAGS imageSaveFlags;
            //TODO: Change to used colors instead of colordepth?
            if (image.ColorDepth == 1)
            {
                imageFormat = FREE_IMAGE_FORMAT.FIF_TIFF;
                imageSaveFlags = FREE_IMAGE_SAVE_FLAGS.TIFF_CCITTFAX4;
            }
            else if (image.ColorDepth <= 8)
            {
                imageFormat = FREE_IMAGE_FORMAT.FIF_TIFF;
                imageSaveFlags = FREE_IMAGE_SAVE_FLAGS.TIFF_LZW;
            }
            else
            {
                // imageFormat = FREE_IMAGE_FORMAT.FIF_JPEG;

                //  imageFormat = FREE_IMAGE_FORMAT.FIF_JPEG;
                imageFormat = FREE_IMAGE_FORMAT.FIF_JPEG;
                //if (CompressPDF)
                //    imageSaveFlags = FREE_IMAGE_SAVE_FLAGS.JPEG_QUALITYNORMAL;
                //else
                imageSaveFlags = FREE_IMAGE_SAVE_FLAGS.JPEG_QUALITYGOOD;


            }

            using (MemoryStream stream = new MemoryStream())
            {
                image.Save(stream, imageFormat, imageSaveFlags);
                return stream.ToArray();
            }
        }

        private static void DrawImage(PDFLibrary pdf, FreeImageBitmap image)
        {

            float imageWidth = ImageTools.PixelsToPoints(image.Width, image.HorizontalResolution);
            float imageHeight = ImageTools.PixelsToPoints(image.Height, image.VerticalResolution);

            //Convert to inches and round to nearest half.
            float widthInInches = ImageTools.PointsToInches(imageWidth);
            widthInInches = MathExtensions.RoundToNearestDecimal(widthInInches, 0.5M);
            float heightInInches = ImageTools.PointsToInches(imageHeight);
            heightInInches = MathExtensions.RoundToNearestDecimal(heightInInches, 0.5M);

            //Convert back to points
            float scaledWidth = ImageTools.InchesToPoints(widthInInches);
            float scaledHeight = ImageTools.InchesToPoints(heightInInches);

            pdf.SetPageDimensions(scaledWidth, scaledHeight);

            byte[] imageData = EncodeImage(image);
            int imageId = pdf.AddImageFromString(imageData, (int)PDFLibrary.AddImageOption.Default);




            pdf.DrawImage(0, 0, pdf.PageWidth(), pdf.PageHeight());
            pdf.ReleaseImage(imageId);
            image.Dispose();
        }
        public static string GetUploadFolder(string azureShareName, DateTime scanDate)
        {
            if (!(azureShareName.EndsWith("/")))
                azureShareName += "/";
            string uploadFolder = $"{azureShareName}{scanDate.ToString("yyyy-MM-dd")}/";
            //  Edocs_Utilities.EdocsUtilitiesInstance.CreateDir(uploadFolder);
            // uploadFolder = Edocs_Utilities.EdocsUtilitiesInstance.CheckDirectoryPath(uploadFolder);
            return uploadFolder;
        }
        public static async Task OCRImages(string errorFileName, string imagesFolder, string imagesExtension, string ocrImageSaveFolder)
        {
            string args = $"/ocr: /inf:{Quoat}{imagesFolder}{Quoat} /fn:{imagesExtension} /efn:{Quoat}{errorFileName}{Quoat} /sfn:{Quoat}{ocrImageSaveFolder}{Quoat}";
            OCRSearchablePDF(args).ConfigureAwait(true).GetAwaiter().GetResult();
        }
        public static async Task SearchablePDF(string errorFileName, string imagesFolder, string imagesExtension, string ocrImageSaveFolder, string backupFolder)
        {
            string args = $"/spdf: /inf:{Quoat}{imagesFolder}{Quoat} /fn:{imagesExtension} /efn:{Quoat}{errorFileName}{Quoat} /sfn:{Quoat}{ocrImageSaveFolder}{Quoat} /pdfbf:{Quoat}{backupFolder}{Quoat}";
            OCRSearchablePDF(args).ConfigureAwait(true).GetAwaiter().GetResult();
        }
        public static async Task GetOcrText(string errorFileName, string imagesFolder, string imagesExtension, string ocrTxtSaveFolder)
        {
            string args = $"/ocr: /inf:{Quoat}{imagesFolder}{Quoat} /fn:{imagesExtension} /efn:{Quoat}{errorFileName}{Quoat} /sfn:{Quoat}{ocrTxtSaveFolder}{Quoat}";
            OCRSearchablePDF(args).ConfigureAwait(true).GetAwaiter().GetResult();
        }
        public static async Task OCRSearchablePDF(string args)
        {
            EdocsUSA.Utilities.Edocs_Utilities.EdocsUtilitiesInstance.StartProcess(OCRSearchablePDFExe, args, true, true);
        }

        public static void NotifyUser(string bId, string errmessage)
        {
            ConsoleColor background = Console.BackgroundColor;
            ConsoleColor foreground = Console.ForegroundColor;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.BackgroundColor = ConsoleColor.White;
            try
            {

                string message = EdocsSupport.Replace("{bid}", bId);
                int tSleep = ThreadSleepSeconds * milliSecond;
                for (int loop = 0; loop < LoopNotify; loop++)
                {
                    Console.Clear();
                    Console.WriteLine($"Error message:{errmessage}");
                    Console.WriteLine("");
                    Console.WriteLine("");
                    Console.WriteLine((message));
                    Console.WriteLine("");
                    Console.WriteLine("Press any key to exit");
                    Thread.Sleep(tSleep);
                    if (Console.KeyAvailable)
                    {
                        Console.ForegroundColor = foreground;
                        Console.BackgroundColor = background;
                        Console.Clear();
                        return;
                    }

                }
            }

            catch (Exception ex)
            {
                edl.TraceLogger.TraceLoggerInstance.TraceError($"Notify user {ex.Message}");
            }
            Console.ForegroundColor = foreground;
            Console.BackgroundColor = background;

            Console.Clear();
        }
        //private static void EncryptEmailPw(string emPw)
        //{
        //    edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Encrypring email pw:{emPw}");
        //    EM.EmailInstance.SendEmail(.Send_Emails.EmailInstance.UpDateEmailPw(emPw);
        //    edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Email pw:{emPw} encrypted");
        //}
        public static void SEmail(string message, bool err)
        {
            try
            {
                // EncryptEmailPw("6746edocs");
                edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Sending email message:{message}");
                if ((SendEmailErrorsOnly) && (!(err)))
                {
                    edl.TraceLogger.TraceLoggerInstance.TraceWarning("Not sending email since set to only send on errors");
                    return;
                }
                string emailSubject = string.Empty;
                if (err)
                {
                    edl.TraceLogger.TraceLoggerInstance.TraceError("Errors found when uploading");
                    emailSubject = $"Error uploading to PSUSD PDF Files runtime {DateTime.Now.ToString()}";
                }
                else
                {
                    edl.TraceLogger.TraceLoggerInstance.TraceError("No Errors found when uploading");
                    emailSubject = $"No Errors uploading  PSUSD PDF Files runtime {DateTime.Now.ToString()}";
                }
                edl.TraceLogger.TraceLoggerInstance.TraceInformation("Sending email");
                EM.Send_Emails.EmailInstance.SendEmail(string.Empty, Properties.Settings.Default.OverWriteEmailCC, message, emailSubject, string.Empty, false, string.Empty);

                // SE.Send_Emails.EmailInstance.SendEmail(string.Empty, message, emailSubject);
                edl.TraceLogger.TraceLoggerInstance.TraceInformation("Email sent");
                //if (message.Length > 30)
                //{
                //    message = message.Substring(0, 30);
                //}
                //edl.TraceLogger.TraceLoggerInstance.TraceInformation("Sending text message");
                ////  SE.Send_Emails.EmailInstance.SendTxtMessage(message, err);
                //edl.TraceLogger.TraceLoggerInstance.TraceInformation("Text message sent");
            }
            catch (Exception ex)
            {
                edl.TraceLogger.TraceLoggerInstance.TraceError($"Sending email or text message {ex.Message}");
            }
        }
        public static void CLeanUp(string sourceFolder, string archiveName, string batchID)
        {
            string destFolder = string.Empty;
            try
            {
                edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Cleaing up folder source folder:{sourceFolder} for archiver:{archiveName}");
                Edocs_Utilities.EdocsUtilitiesInstance.CheckDirectExists(sourceFolder);

                if (MakeCopyUploadImage)
                {
                    destFolder = $"{Edocs_Utilities.EdocsUtilitiesInstance.CheckDirectoryPath(UploadedImagesCopyFolder)}{archiveName}";
                    destFolder = $"{Edocs_Utilities.EdocsUtilitiesInstance.AddDateTimeToFolderName(destFolder)}{batchID}\\";
                    edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Copying files from source folder:{sourceFolder} to dest folder:{destFolder} for archiver:{archiveName} batchid:{batchID}");
                    Edocs_Utilities.EdocsUtilitiesInstance.CopyFiles(sourceFolder, destFolder, false, "*.*");
                }
                if (DeleteBatchOnSuccess)
                {
                    edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Deleting files under folder folder:{sourceFolder} for archiver:{archiveName} batchid:{batchID}");
                    Edocs_Utilities.EdocsUtilitiesInstance.DelFolder(sourceFolder, true);
                }
                edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Deleting files under folder folder:{LogFolder} number of days:{DaysToKeepLogFiles.ToString()}");
                //  Edocs_Utilities.EdocsUtilitiesInstance.CleanUpLogFiles(LogFolder, DaysToKeepLogFiles, "*.*");
                destFolder = $"{Edocs_Utilities.EdocsUtilitiesInstance.CheckDirectoryPath(UploadedImagesCopyFolder)}{archiveName}";
                edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Deleting copy upload image folder:{destFolder} for DaysToKeepCopyFolder:{DaysToKeepCopyFolder}");
                //    Edocs_Utilities.EdocsUtilitiesInstance.DelDirectores(destFolder, DaysToKeepCopyFolder, "*.*", SearchOption.AllDirectories);
            }
            catch (Exception ex)
            {
                edl.TraceLogger.TraceLoggerInstance.TraceError($"Cleaing up folder source folder:{sourceFolder} for archiver:{archiveName} for batchid:{batchID} copyfolder:{destFolder} {ex.Message}");

            }
        }
        public static async Task UploadPSSDRecords(PSUSDRecordsModel pSUSDRecordsModel, int custID, string trackingID)
        {
            //DateOfRecords":"05.19.2023-05.19.2023
            string[] dateRange = pSUSDRecordsModel.DateOfRecords.Split('-');

            PSUSDUploadRecordsModel pSUSDUploadRecordsModel = new PSUSDUploadRecordsModel();
            pSUSDUploadRecordsModel.DateOfBirth = pSUSDRecordsModel.DateOfBirth;
            pSUSDUploadRecordsModel.DateOfRecords = pSUSDRecordsModel.DateOfRecords;
            pSUSDUploadRecordsModel.MethOfFiling = pSUSDRecordsModel.MethOfFiling;
            pSUSDUploadRecordsModel.OrginationDepartment = pSUSDRecordsModel.OrginationDepartment;

            pSUSDUploadRecordsModel.Department = pSUSDRecordsModel.Department;
            pSUSDUploadRecordsModel.DescriptionOfRecords = pSUSDRecordsModel.DescriptionOfRecords;
            pSUSDUploadRecordsModel.EdocsCustomerID = custID;
            pSUSDUploadRecordsModel.FirsName = pSUSDRecordsModel.FirsName;
            pSUSDUploadRecordsModel.LastName = pSUSDRecordsModel.LastName;
            pSUSDUploadRecordsModel.OrginationDepartment = pSUSDRecordsModel.OrginationDepartment;
            pSUSDUploadRecordsModel.RecordSDate = DateTime.Parse(dateRange[0]);
            pSUSDUploadRecordsModel.RecordEndDate = DateTime.Parse(dateRange[1]);
            pSUSDUploadRecordsModel.TrackindID = trackingID;
            WebApi.UploadPSUSDRec(WebApi.EdocsWebApi, "UploadPSUSDRecords", pSUSDUploadRecordsModel).ConfigureAwait(false).GetAwaiter().GetResult();

        }
        public static async Task UploadPSSDFullText(string trackingID, string ocrFileName, PSUSDRecordsModel pSUSDRecordsModel, string batchid)
        {
            //DateOfRecords":"05.19.2023-05.19.2023
            if (Edocs_Utilities.EdocsUtilitiesInstance.CHeckFileExists(ocrFileName))
            {
                string[] dateRange = pSUSDRecordsModel.DateOfRecords.Split('-');
                PSUSDUpLoadFullText fullText = new PSUSDUpLoadFullText();
                fullText.TrackindID = trackingID;
                string ocrText = File.ReadAllText(ocrFileName);


                fullText.FullText = $"[edocsbeginginsetr {pSUSDRecordsModel.DateOfBirth} {pSUSDRecordsModel.DateOfRecords} {pSUSDRecordsModel.MethOfFiling} {pSUSDRecordsModel.OrginationDepartment} {pSUSDRecordsModel.Department} {pSUSDRecordsModel.DescriptionOfRecords} {pSUSDRecordsModel.FirsName} {pSUSDRecordsModel.LastName} { pSUSDRecordsModel.OrginationDepartment} {trackingID} {batchid} edocendinsetr]\r\n{ocrText}";

                WebApi.UploadPSUSDRec(WebApi.EdocsWebApi, "UploadPSUSDSearchText", fullText).ConfigureAwait(false).GetAwaiter().GetResult();
            }
        }

        //public static async Task<StringBuilder> GedtOCRText(string folder, string exension)
        //{
        //    Uri uri = new Uri(OCRWebApi);
        //    StringBuilder sb = new StringBuilder();
        //    foreach (string imgFile in Edocs_Utilities.EdocsUtilitiesInstance.GetFiles(folder, exension))
        //    {

        //        TotalOCR++;
        //        string ocrReults = ConvertImagePDF.OCRSrace(imgFile, true, false, OCRApiKey, "2", OCRISTable, uri).ConfigureAwait(false).GetAwaiter().GetResult();
        //        if (!(string.IsNullOrWhiteSpace(ocrReults)))
        //            sb.AppendLine(ocrReults);
        //    }

        //    return sb;
        //}
    }
}
