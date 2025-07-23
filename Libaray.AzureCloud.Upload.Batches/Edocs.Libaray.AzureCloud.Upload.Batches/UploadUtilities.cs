using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.IO;
using EdocsUSA.Utilities;
using EdocsUSA.Utilities.Extensions;
using edl = EdocsUSA.Utilities.Logging;
using FreeImageAPI;
using DebenuPDFLibraryDLL0915;
using SE = ScanQuire_SendEmails;
using Edocs.Upload.Azure.Blob.Storage;
 
using Edocs.Ocr.Convert.Libaray.Img.PDF;
using System.Security.Policy;

namespace Edocs.Libaray.AzureCloud.Upload.Batches
{
    public static class UploadUtilities
    {
        static readonly int milliSecond = 1000;
        private static readonly string JsonRecordsExten = "_records.json";
        private static readonly string JsonBatchSettingExten = "_settings.json";

        public static string InputDir
        { get { return Properties.Settings.Default.ArchivesFolder; } }
        public static string EdocsWebApi
        { get { return Properties.Settings.Default.EdocsWebApi; } }
        public static bool DeleteBatchOnSuccess
        { get { return Properties.Settings.Default.DeleteBatchOnSuccess; } }
        public static string EdocsSupport
        { get { return Properties.Settings.Default.EdocsSupport; } }


        public static string BackUpImagesCopyFolder
        { get { return Properties.Settings.Default.BackUpImagesCopyFolder; } }
        public static string OcrLabReqsContainer
        { get { return Properties.Settings.Default.OcrLabReqsContainer; } }
        public static bool SaveOcrFile
        { get { return Properties.Settings.Default.SaveOcrFile; } }
        public static string JsonFilesBackUpFolder
        { get { return Properties.Settings.Default.JsonFilesBackUpFolder; } }
        public static string JsonFilesFolder
        { get { return Properties.Settings.Default.JsonFilesFolder; } }

        public static bool MakeCopyUploadImage
        { get { return Properties.Settings.Default.MakeCopyUploadImage; } }
        //public static bool CompressPDF
        //{ get { return Properties.Settings.Default.CompressPDF; } }

        public static string UploadedImagesCopyFolder
        { get { return Properties.Settings.Default.UploadedImagesCopyFolder; } }

        public static int DaysToKeepCopyFolder
        { get { return Properties.Settings.Default.DaysToKeepCopyFolder; } }

        public static string LogFolder
        { get { return (Properties.Settings.Default.LogFolder.Replace("{ApplicationDir}", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData))); } }

        public static int DaysToKeepLogFiles
        { get { return Properties.Settings.Default.DaysToKeepLogFiles; } }

        public static int UploadLoop
        { get { return Properties.Settings.Default.LoopRetryUpLoad; } }
        public static int ThreadSleep
        { get { return Properties.Settings.Default.ThreadSleep; } }
        public static bool SendEmailErrorsOnly
        { get { return Properties.Settings.Default.SendEmailErrorsOnly; } }

        public static int ThreadSleepSeconds
        { get { return Properties.Settings.Default.ThreadSleepSeconds; } }

        public static int LoopNotify
        { get { return Properties.Settings.Default.LoopNotifyUser; } }

        public static string AzureBlobAccountKey
        { get { return Properties.Settings.Default.AzureBlobAccountKey; } }
        public static string AzureBlobAccountName
        { get { return Properties.Settings.Default.AzureBlobAccountName; } }

        public static string AzureBlobStorageConnectionString
        { get { return Properties.Settings.Default.AzureBlobStorageConnectionString; } }

        public static int DateTimeMins
        { get { return Properties.Settings.Default.DateTimeMins; } }
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

        public static async Task<Uri> SaveTffFileAzureCloud(string batchDir, string azureContanier, string saveFileName)
        {

            //  Edocs_Utilities.EdocsUtilitiesInstance.CHeckFileExists($"{batchDir}{uploadFileName}", false);
            byte[] pdfFile = UploadUtilities.ConvertToPdfFile($"{batchDir}{saveFileName}");
            saveFileName = $"{batchDir}{saveFileName.Replace(Path.GetExtension(saveFileName), ".pdf")}";
            //string pdfSaveFName = $"{uploadFolder}{uploadFileName}";
            //File.WriteAllBytes(pdfSaveFName, pdfFile);
            Uri url = AzureBlobStorage.BlobStorageInstance.UploadAzureBlob(saveFileName, azureContanier, pdfFile).GetAwaiter().GetResult();
            return url;
            //edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Saving pdf file to upload folder {pdfSaveFName}");

        }
        public static async Task<Uri> UploadTiffFileAzureCloud(string tiffFName, string azureContanier)
        {
            try
            {
                //  Edocs_Utilities.EdocsUtilitiesInstance.CHeckFileExists($"{batchDir}{uploadFileName}", false);
                // byte[] pdfFile = UploadUtilities.ConvertToPdfFile($"{batchDir}{saveFileName}");
                //   saveFileName = $"{batchDir}{saveFileName.Replace(Path.GetExtension(saveFileName), ".pdf")}";
                //string pdfSaveFName = $"{uploadFolder}{uploadFileName}";
                //File.WriteAllBytes(pdfSaveFName, pdfFile);
                Uri url = AzureBlobStorage.BlobStorageInstance.UploadAzureBlob(tiffFName, azureContanier).GetAwaiter().GetResult();
                return url;
            }
            catch (Exception ex)
            {
                edl.TraceLogger.TraceLoggerInstance.TraceError($"Copying tiff file {tiffFName}  to contanier {azureContanier}");
                edl.TraceLogger.TraceLoggerInstance.TraceErrorConsole($"Copying tiff file {tiffFName}  to contanier {azureContanier}");
            }
            return null;
            //edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Saving pdf file to upload folder {pdfSaveFName}");

        }
        public static async Task<string> SaveTffFileLocal(string batchDir, string destfoler, string saveFileName)
        {

            //  Edocs_Utilities.EdocsUtilitiesInstance.CHeckFileExists($"{batchDir}{uploadFileName}", false);
            byte[] pdfFile = UploadUtilities.ConvertToPdfFile($"{batchDir}{saveFileName}");
            destfoler = destfoler.Replace('/', '\\');
            if (!(destfoler.ToLower().StartsWith("d")))
                destfoler = $"d:\\{destfoler}";
            Edocs_Utilities.EdocsUtilitiesInstance.CreateDir(destfoler);


            saveFileName = $"{destfoler}{saveFileName.Replace(Path.GetExtension(saveFileName), ".pdf")}";

            //string pdfSaveFName = $"{uploadFolder}{uploadFileName}";
            File.WriteAllBytes(saveFileName, pdfFile);
            // Uri url = AzureBlobStorage.BlobStorageInstance.UploadAzureBlob(saveFileName, azureContanier, pdfFile).GetAwaiter().GetResult();
            return saveFileName;
            //edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Saving pdf file to upload folder {pdfSaveFName}");

        }
        private static async Task<bool> BackupFiles(string jsonRecordsFile, string jsonSettingFile, string jBackupFolder)
        {
            try
            {

                Edocs_Utilities.EdocsUtilitiesInstance.CopyFile(jsonRecordsFile, jBackupFolder, true, string.Empty, false);

                Edocs_Utilities.EdocsUtilitiesInstance.CopyFile(jsonSettingFile, jBackupFolder, true, string.Empty, false);

                return true;
            }
            catch (Exception ex)
            {
                edl.TraceLogger.TraceLoggerInstance.TraceError($"Copying Json  Files {jsonRecordsFile} {jsonSettingFile} to folder {jBackupFolder} message {ex.Message}");
                SEmail($"Copying Json  Files {jsonRecordsFile} {jsonSettingFile} to folder {jBackupFolder} message {ex.Message}", true);
            }
            return false;
        }
        public static async Task BackupFiles(string jsonRecordsFile, string jsonSettingFile)
        {

            if (!(BackupFiles(jsonRecordsFile, jsonSettingFile, JsonFilesBackUpFolder).ConfigureAwait(false).GetAwaiter().GetResult()))
            {
                if (!(BackupFiles(jsonRecordsFile, jsonSettingFile, JsonFilesFolder).ConfigureAwait(false).GetAwaiter().GetResult()))
                {
                    edl.TraceLogger.TraceLoggerInstance.TraceError($"Copying Json  Files {jsonRecordsFile} {jsonSettingFile} to folder {JsonFilesBackUpFolder}");
                    throw new Exception($"Copying Json File {jsonRecordsFile} {jsonSettingFile} to folder {JsonFilesFolder}");
                }


            }


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

        public static async Task<Uri> SavePdfFileAzureCloud(string sourceFolder, string destFolder, string saveFileName)
        {
            string uploadFileName = saveFileName.Replace(Path.GetExtension(saveFileName), ".pdf");
            //  Edocs_Utilities.EdocsUtilitiesInstance.CHeckFileExists($"{sourceFolder}{uploadFileName}", false);
            string pdfSFName = $"{sourceFolder}{uploadFileName}";
            //  destFolder = $"{destFolder}{uploadFileName}";
            edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Copying sourefile:{pdfSFName} to destfile:{destFolder}");
            Uri url = AzureBlobStorage.BlobStorageInstance.UploadAzureBlob(pdfSFName, destFolder).GetAwaiter().GetResult();
            return url;
        }


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

        public static string GetBatchSettingsFileName(string batchDir, string batchId)
        {
            return Path.Combine(batchDir, ($"{batchId}{JsonBatchSettingExten}"));
        }

        public static byte[] ConvertToPdfFile(string filePath)
        {

            byte[] tiffData = File.ReadAllBytes(filePath); ;
            return TiffToPdf(tiffData);

        }
        public static AzureCloudBatchRecordsModel[] ReadRecords(string batchRecordsFile, ref int totalRecods)
        {
            JavaScriptSerializer Serializer = new JavaScriptSerializer();

            //if (Directory.Exists(batchRecordsFile) == false)
            //{ throw new DirectoryNotFoundException(batchRecordsFile); }


            string[] recordLines = File.ReadAllLines(batchRecordsFile);
            totalRecods = recordLines.Length;
            return (from string recordLine in recordLines select Serializer.Deserialize<AzureCloudBatchRecordsModel>(recordLine)).ToArray();
        }

        public static byte[] TiffToPdf(byte[] tiff)
        {
            PDFLibrary pdf = new PDFLibrary();
            try
            {

                //if(CompressPDF)
                //    pdf.CompressImages((int)PDFLibrary.ImageCompression.Flate);
                //else
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


        public static void NotifyUser(string bId, string errmessage)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.BackgroundColor = ConsoleColor.White;
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
                        return;
                }
            }
            catch (Exception ex)
            {
                edl.TraceLogger.TraceLoggerInstance.TraceError($"Notify user {ex.Message}");
            }
        }
        public static void SEmail(string message, bool err)
        {
            try
            {
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
                    emailSubject = $"Error uploading to Azure cloud runtime {DateTime.Now.ToString()}";
                }
                else
                {
                    edl.TraceLogger.TraceLoggerInstance.TraceError("No Errors found when uploading");
                    emailSubject = $"No Errors uploading to Azure cloud runtime {DateTime.Now.ToString()}";
                }
                edl.TraceLogger.TraceLoggerInstance.TraceInformation("Sending email");
                SE.Send_Emails.EmailInstance.SendEmail(string.Empty, message, emailSubject);
                edl.TraceLogger.TraceLoggerInstance.TraceInformation("Email sent");
                if (message.Length > 30)
                {
                    message = message.Substring(0, 30);
                }
                edl.TraceLogger.TraceLoggerInstance.TraceInformation("Sending text message");
                SE.Send_Emails.EmailInstance.SendTxtMessage(message, err);
                edl.TraceLogger.TraceLoggerInstance.TraceInformation("Text message sent");
            }
            catch (Exception ex)
            {
                edl.TraceLogger.TraceLoggerInstance.TraceError($"Sending email or text message {ex.Message}");
            }
        }
        public static bool BackUpImages(string sourceFolder, string destFolder)
        {
            try
            {
                edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Backing up images source folder:{sourceFolder} to dest folder:{destFolder}");
                edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Backing up images source folder:{sourceFolder} to dest folder:{destFolder}");
                Edocs_Utilities.EdocsUtilitiesInstance.CopyFiles(sourceFolder, destFolder, false, "*.*");
                return true;
            }
            catch (Exception ex)
            {
                edl.TraceLogger.TraceLoggerInstance.TraceError($"Backing up images source folder:{sourceFolder} to dest folder:{destFolder}  {ex.Message}");
                edl.TraceLogger.TraceLoggerInstance.TraceErrorConsole($"Backing up images source folder:{sourceFolder} to dest folder:{destFolder}  {ex.Message}");
                UploadUtilities.SEmail($"Backing up images source folder:{sourceFolder} to dest folder:{destFolder}  {ex.Message}", true);
            }
            return false;

        }

        public static void CLeanUp(string sourceFolder, string archiveName, string batchID)
        {
            string destFolder = string.Empty;
            try
            {
                edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Cleaing up folder source folder:{sourceFolder} for archiver:{archiveName}");
                edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Cleaing up folder source folder:{sourceFolder} for archiver:{archiveName}");
                Edocs_Utilities.EdocsUtilitiesInstance.CheckDirectExists(sourceFolder);

                if (MakeCopyUploadImage)
                {
                    try
                    {
                        destFolder = $"{Edocs_Utilities.EdocsUtilitiesInstance.CheckDirectoryPath(UploadedImagesCopyFolder)}{archiveName}";
                        Edocs_Utilities.EdocsUtilitiesInstance.CreateDir(destFolder);
                        Edocs_Utilities.EdocsUtilitiesInstance.CheckDirectExists(destFolder);

                        //   destFolder = $"{Edocs_Utilities.EdocsUtilitiesInstance.CheckDirectoryPath(UploadedImagesCopyFolder)}{archiveName}";
                    }
                    catch (Exception ex)
                    {
                        edl.TraceLogger.TraceLoggerInstance.TraceError($"Backing up images source folder:{sourceFolder} for upload folder UploadedImagesCopyFolder:{UploadedImagesCopyFolder}  {ex.Message}");
                        edl.TraceLogger.TraceLoggerInstance.TraceErrorConsole($"Backing up images source folder:{sourceFolder} for upload folder UploadedImagesCopyFolder:{UploadedImagesCopyFolder}  {ex.Message}");
                        UploadUtilities.SEmail($"Backing up images source folder:{sourceFolder} for upload folder UploadedImagesCopyFolder:{UploadedImagesCopyFolder} for archiver {archiveName} batchid {batchID}  {ex.Message}", true);
                        destFolder = destFolder = $"{Edocs_Utilities.EdocsUtilitiesInstance.CheckDirectoryPath(BackUpImagesCopyFolder)}{archiveName}";
                    }

                    destFolder = $"{Edocs_Utilities.EdocsUtilitiesInstance.AddDateTimeToFolderName(destFolder)}{batchID}\\";

                    edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Backing up images source folder:{sourceFolder} to dest folder:{destFolder}");
                    edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Backing up images source folder:{sourceFolder} to dest folder:{destFolder}");
                    Edocs_Utilities.EdocsUtilitiesInstance.CreateDir(destFolder);
                    Edocs_Utilities.EdocsUtilitiesInstance.CopyFiles(sourceFolder, destFolder, false, "*.*");
                    //   Edocs_Utilities.EdocsUtilitiesInstance.CopyFiles(sourceFolder, destFolder, false, "*.*");

                }
                if (DeleteBatchOnSuccess)
                {
                    edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Deleting files under folder folder:{sourceFolder} for archiver:{archiveName} batchid:{batchID}");
                    Edocs_Utilities.EdocsUtilitiesInstance.DelFolder(sourceFolder, true);
                }
                //edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Deleting files under folder folder:{LogFolder} number of days:{DaysToKeepLogFiles.ToString()}");
                //Edocs_Utilities.EdocsUtilitiesInstance.CleanUpLogFiles(LogFolder, DaysToKeepLogFiles, "*.*");
                //destFolder = $"{Edocs_Utilities.EdocsUtilitiesInstance.CheckDirectoryPath(UploadedImagesCopyFolder)}{archiveName}";
                //edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Deleting copy upload image folder:{destFolder} for DaysToKeepCopyFolder:{DaysToKeepCopyFolder}");
                //Edocs_Utilities.EdocsUtilitiesInstance.DelDirectores(destFolder, DaysToKeepCopyFolder, "*.*", SearchOption.AllDirectories);
            }
            catch (Exception ex)
            {
                edl.TraceLogger.TraceLoggerInstance.TraceError($"Cleaing up folder source folder:{sourceFolder} for archiver:{archiveName} for batchid:{batchID} copyfolder:{destFolder} {ex.Message}");
                UploadUtilities.SEmail($"Cleaing up images source folder:{sourceFolder} for upload folder UploadedImagesCopyFolder:{UploadedImagesCopyFolder} for archiver {archiveName} batchid {batchID}  {ex.Message}", true);
            }
        }
    }
}
