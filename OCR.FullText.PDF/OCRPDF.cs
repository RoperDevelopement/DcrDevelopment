using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Drawing;
using HU = Edocs.HelperUtilities;
using OCRLib = Edocs.Ocr.Convert.Libaray;
using BS = Edocs.Upload.Azure.Blob.Storage;
using Edocs.OCR.FullText.PDF.Models;
using System.IO;
using TL = EdocsUSA.Utilities.Logging;
using SE = Edocs.Send.Emails.Send_Emails;
using ABS = Edocs.Upload.Azure.Blob.Storage.AzureBlobStorage;
using BR = Edocs.OCR.FullText.PDF.BarCodeReader;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics;

namespace Edocs.OCR.FullText.PDF
{
    class OCRPDF
    {
        static readonly string ArgScanStDate = "/stDate:";
        static readonly string ArgScanEndDate = "/endDate:";
        static readonly string ArgCheckReqNum = "/creqnum:";
        static readonly string ArgNumDays = "/numDays:";
        static readonly string ArgNumRecProcess = "/numrecproc:";
        static readonly string ArgWebApi = "/webapi:";
        static readonly string ArgMerge = "/merge:";

        static int TotalRecordsProcessed = 0;
        static int TotalRecordsErrors = 0;
        static int TotalLabReqsNotFound = 0;
        static int TotalLabReqsReturned = 0;
        static int TotalCorrectLabReqsNums = 0;
        static int TotalPDFSConvertedToImage = 0;
        static int TotalLabReqsNumsOCRUpLoaded = 0;
        static bool ConvertedPDFToImage = false;
        static StringBuilder SbErrors = new StringBuilder();
        //  static int TotalLabReqsFound = 0;
        static void Main(string[] args)
        {
            string logFile = string.Empty;

            try
            {
              //  OCRLib.PdfImageConvert.SavePDFAsImage(@"L:\New Bids\WA\ProcureWare Vendors_ Uploading Documents (1) (1).pdf", @"L:\", string.Empty, 300, ".png").ConfigureAwait(false).GetAwaiter().GetResult();
                CheckMultiProcessingRunning(args);
                System.Reflection.Assembly currentAssem = System.Reflection.Assembly.GetExecutingAssembly();
                logFile = OcrWebAPI.LogFolder;
                TL.TraceLogger.TraceLoggerInstance.CreateDirectory(logFile);
                TL.TraceLogger.TraceLoggerInstance.OpenTraceLogFile(logFile, "OCRPDF", true);
                TL.TraceLogger.TraceLoggerInstance.RunningAssembley = currentAssem.ManifestModule.Name;
                TL.TraceLogger.TraceLoggerInstance.RunningAssembley = HU.Utilities.GetAssemblyTitle();
                TL.TraceLogger.TraceLoggerInstance.StartStopStopWatch();
                TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Running on machine {Environment.MachineName} for user {TL.TraceLogger.TraceLoggerInstance.UserName}");
                TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Assembly Title {HU.Utilities.GetAssemblyTitle()}");
                TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Assembly Version {HU.Utilities.GetAssemblyVersion()}");
                TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Assembly Description {HU.Utilities.GetAssemblyDescription()}");
                if (args.Length == 0)
                    PrintUsage();
                else
                    GetInputArgs(args);

            }
            catch (Exception ex)
            {
                TL.TraceLogger.TraceLoggerInstance.TraceError(ex.Message);
                SbErrors.AppendLine($"<p>{ex.Message}</p>");
                PrintUsage();
                CreateHtmlFile(DateTime.Now.ToString("MM-dd-yyyy"), DateTime.Now.ToString("MM-dd-yyyy")).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Records Returned {TotalLabReqsReturned} Total Processed {TotalRecordsProcessed} Index/Fin Numbeer don't match {TotalLabReqsNotFound} Index/Fin Number match {TotalCorrectLabReqsNums} Errors {TotalRecordsErrors}");

            UpLoadLogAzure(logFile).ConfigureAwait(false).GetAwaiter().GetResult();
            //TL.TraceLogger.TraceLoggerInstance.CloseTraceFile();
        }
        static void GetInputArgs(string[] args)
        {
            OcrWebAPI.OCRApisInctance.UseWebApi = false;
            int numRecToProcess = 0;
            string scanStDate = string.Empty;
            string scanEndDate = string.Empty;
            bool checkReqNum = false;
            int merge = 0;
            foreach (string inputArgs in args)
            {
                if (inputArgs.StartsWith(ArgScanStDate, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (DateTime.TryParse(inputArgs.Substring(ArgScanStDate.Length), out DateTime results))
                    {

                        scanStDate = results.ToString("MM-dd-yyyy");
                        continue;
                    }
                    else
                        throw new Exception($"Invalid start date {inputArgs.Substring(ArgScanStDate.Length)} ");
                }
                if (inputArgs.StartsWith(ArgMerge, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (int.TryParse(inputArgs.Substring(ArgMerge.Length), out int results))
                        merge = results;
                     
                }
                    
                if (inputArgs.StartsWith(ArgScanEndDate, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (DateTime.TryParse(inputArgs.Substring(ArgScanEndDate.Length), out DateTime results))
                    {

                        scanEndDate = results.ToString("MM-dd-yyyy");
                        continue;
                    }
                    else
                        throw new Exception($"Invalid start date {inputArgs.Substring(ArgScanStDate.Length)} ");
                }
                if (inputArgs.StartsWith(ArgCheckReqNum, StringComparison.InvariantCultureIgnoreCase))
                {
                    checkReqNum = HU.Utilities.GetBool(inputArgs.Substring(ArgCheckReqNum.Length));
                    continue;

                }
                if (inputArgs.StartsWith(ArgWebApi, StringComparison.InvariantCultureIgnoreCase))
                {
                    OcrWebAPI.OCRApisInctance.UseWebApi = HU.Utilities.GetBool(inputArgs.Substring(ArgWebApi.Length));
                    continue;

                }
                if (inputArgs.StartsWith(ArgNumDays, StringComparison.InvariantCultureIgnoreCase))
                {
                    int numDays = HU.Utilities.ParseInt(inputArgs.Substring(ArgNumDays.Length));
                    if (numDays > 0)
                    {
                        scanEndDate = DateTime.Now.AddDays(numDays).ToString("MM-dd-yyyy");
                        scanStDate = DateTime.Now.ToString("MM-dd-yyyy");
                    }

                    else
                    {
                        scanEndDate = DateTime.Now.ToString("MM-dd-yyyy");
                        scanStDate = DateTime.Now.AddDays(numDays).ToString("MM-dd-yyyy");
                    }

                    continue;

                }
                if (inputArgs.StartsWith(ArgNumRecProcess, StringComparison.InvariantCultureIgnoreCase))
                {
                    numRecToProcess = HU.Utilities.ParseInt(inputArgs.Substring(ArgNumRecProcess.Length));
                    continue;

                }





            }
            if (DateTime.Parse(scanStDate) > DateTime.Parse(scanEndDate))
                throw new Exception($"Scan start date {scanStDate} cannot be larger then scan end date: {scanEndDate}");
            OcrIndexLabReqsNumbers(scanStDate, scanEndDate, checkReqNum, numRecToProcess, true).ConfigureAwait(true).GetAwaiter().GetResult();
            RunMerDem(DateTime.Parse(scanStDate),DateTime.Parse(scanEndDate),merge).ConfigureAwait(false).GetAwaiter().GetResult();
            CreateHtmlFile(scanStDate, scanEndDate).ConfigureAwait(false).GetAwaiter().GetResult();

        }

        static void PrintUsage()
        {
            Console.Beep();
            Console.WriteLine();
            ConsoleColor ccb = Console.BackgroundColor;
            Console.BackgroundColor = ConsoleColor.Gray;
            ConsoleColor ccfg = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Black;
            System.Reflection.Assembly currentAssem = System.Reflection.Assembly.GetExecutingAssembly();
            Console.WriteLine($"{currentAssem.ManifestModule.Name} current version:{HU.Utilities.GetAssemblyVersion()}");
            Console.WriteLine($"Usage:{currentAssem.ManifestModule.Name}");
            Console.WriteLine($"{currentAssem.ManifestModule.Name}.exe print usage no args");
            Console.WriteLine($"{currentAssem.ManifestModule.Name}.exe /stDate:scan start date /endDate:scan end date /creqnum:true or false /numrecproc:100  /webapi: true or false default false");
            Console.WriteLine($"{currentAssem.ManifestModule.Name}.exe /stDate:scan start date /endDate:scan end date /webapi:true");
            Console.WriteLine($"{currentAssem.ManifestModule.Name}.exe /stDate:scan start date /endDate:scan end date");
            Console.WriteLine($"{currentAssem.ManifestModule.Name}.exe /stDate:scan start date /endDate:scan end date /numrecproc:100 /webapi:true");
            Console.WriteLine($"{currentAssem.ManifestModule.Name}.exe /stDate:10-01-2020 /endDate:11-30-2020 /creqnum:true or false /numrecproc:100");
            Console.WriteLine($"{currentAssem.ManifestModule.Name}.exe /numDays:date range to process for scan start date and scan end date /creqnum:true or false /numrecproc:100");
            Console.WriteLine($"{currentAssem.ManifestModule.Name}.exe /numDays:20 of -20 /creqnum:true or false /numrecproc:100");
            Console.WriteLine($"{currentAssem.ManifestModule.Name}.exe /numDays:20 of -20");
            Console.WriteLine($"{currentAssem.ManifestModule.Name}.exe /numDays:20 of -20 /creqnum:true or false");
            Console.WriteLine($"{currentAssem.ManifestModule.Name}.exe /numDays:20 of -20 /numrecproc:100");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.BackgroundColor = ccb;
            Console.ForegroundColor = ccfg;
            Console.ReadKey();
            Console.Clear();
        }

        static async Task OcrIndexLabReqsNumbers(string scanStDate, string scanEndDate, bool checkLabReqNum, int numRecToProcess, bool indexLabReqs)
        {
            TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Method Getlabreqs scan start date {scanStDate} scan end date {scanEndDate} total records to process {numRecToProcess}");
            TL.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Method Getlabreqs scan start date {scanStDate} scan end date {scanEndDate} total records to process {numRecToProcess}");
            string apiKey = OcrWebAPI.OCRApisInctance.OCRAPIKey;
            string workingFolder = HU.Utilities.CheckFolderPath(OcrWebAPI.OCRApisInctance.WorkingFolder);
            string wfImage = Path.Combine(workingFolder, "ImageWE");
            int cWF = OcrWebAPI.OCRApisInctance.CleanWF;
            int maxOcrErrors = OcrWebAPI.OCRApisInctance.MaxOcrErrors;
            HU.Utilities.CreateDirectory(wfImage);
            HU.Utilities.CreateDirectory(OcrWebAPI.OCRApisInctance.IndexNumbersChanged);
            HU.Utilities.CreateDirectory(OcrWebAPI.OCRApisInctance.IndexNumbersChanged);
            HU.Utilities.CreateDirectory(workingFolder);
            HU.Utilities.DeleteFiles(workingFolder, 0);
            string pdfFile = string.Empty;
            string pdfText = string.Empty;
            string skippReqs = OcrWebAPI.OCRApisInctance.RegxSkip;
            IDictionary<int, LabReqsModel> pairs = null;
            if (OcrWebAPI.OCRApisInctance.UseWebApi)
                pairs = OcrWebAPI.OCRApisInctance.GetLabReqs(scanStDate, scanEndDate, OcrWebAPI.OCRApisInctance.WebUri, OCRConstants.LabReqsPDFFullTextController).ConfigureAwait(false).GetAwaiter().GetResult();
            else
                pairs = LocalSqlServer.SqlServerInstance.GetLabReqs(OcrWebAPI.OCRApisInctance.SqlConnectionStr, scanStDate, scanEndDate).ConfigureAwait(false).GetAwaiter().GetResult();
            int totRecsReturned = TotalLabReqsReturned = pairs.Count();
            TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Total Lab Reqs returned {totRecsReturned}");
            TL.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Total Lab Reqs returned {totRecsReturned}");
            foreach (var ocrPdf in pairs)
            {
                if (TotalRecordsErrors > maxOcrErrors)
                {
                    SbErrors.AppendLine($"{OCRConstants.StartTableRow}{OCRConstants.StartTableData}{ocrPdf.Key}{OCRConstants.EndTableDate}{OCRConstants.StartTableData}{ocrPdf.Value.IndexNumber}{OCRConstants.EndTableDate}" +
                       $"{OCRConstants.StartTableData}{ocrPdf.Value.FinancialNumber}{OCRConstants.EndTableDate}{OCRConstants.StartTableData}Max OCR erros set at {maxOcrErrors} total ocr errors {TotalRecordsErrors} stopping process {OCRConstants.EndTableDate}{OCRConstants.StartTableData}{ocrPdf.Value.ScanDate.ToString("MM-dd-yyyy")}{OCRConstants.EndTableDate} {OCRConstants.EndTableRow}");
                    break;
                }
                ConvertedPDFToImage = false;
                System.Diagnostics.Stopwatch runTimer = new System.Diagnostics.Stopwatch();
                runTimer.Start();
                Console.WriteLine();
                TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Processing lab req {ocrPdf.Value.FinancialNumber}");
                TL.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Processing lab req {ocrPdf.Value.FinancialNumber}");

                Console.WriteLine();
                Console.WriteLine();
                TL.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Total Records Left to process {totRecsReturned--}");
                TL.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Total Records Process {++TotalRecordsProcessed} of {TotalLabReqsReturned}");
                Console.WriteLine();

                try
                {
                    if (numRecToProcess > 0)
                    {
                        if (TotalRecordsProcessed > numRecToProcess)

                            break;
                    }
                    if (ocrPdf.Value.IndexNumber.ToUpper().StartsWith("TSTLOG"))
                    {
                        TL.TraceLogger.TraceLoggerInstance.TraceWarning($"Skipping since log file {ocrPdf.Value.IndexNumber} ");
                        TL.TraceLogger.TraceLoggerInstance.TraceWaringConsole($"Skipping since log file {ocrPdf.Value.IndexNumber} ");
                        pdfText = $"TSTLOG {ocrPdf.Value.IndexNumber} {ocrPdf.Value.FinancialNumber} {ocrPdf.Value.RequisitionNumber}";
                        UploadFullText(workingFolder, pdfText, ocrPdf.Key, ocrPdf.Value.ScanDate).ConfigureAwait(false).GetAwaiter().GetResult();
                        DeleteTifImage(ocrPdf.Value.FileUrl, OcrWebAPI.OCRApisInctance.AzureImageContanier).ConfigureAwait(false).GetAwaiter().GetResult();
                        runTimer.Stop();
                        Console.WriteLine();
                        TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Total time to process {runTimer.Elapsed} Lab Req Index Number {ocrPdf.Value.FinancialNumber} ");
                        TL.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Total time to process {runTimer.Elapsed} Lab Req Index Number {ocrPdf.Value.FinancialNumber} ");
                        continue;
                    }
                    pdfText = GetOCRText(ocrPdf.Value.FileUrl, wfImage, workingFolder).ConfigureAwait(false).GetAwaiter().GetResult();
                    // pdfFile = DownLoadPdf(new Uri(ocrPdf.Value.FileUrl.ToString()), workingFolder, Path.GetFileName(ocrPdf.Value.FileUrl)).ConfigureAwait(false).GetAwaiter().GetResult();
                    //  pdfFile = @"L:\NotOCR\bd5dbaa2-5416-400a-9af2-5114965141fc.pdf";
                    // pdfText = OCRLib.PdfImageConvert.GoogleOcr(pdfFile, false, false, apiKey).ConfigureAwait(false).GetAwaiter().GetResult();
                    //pdfText = GetImageText(pdfFile, wfImage, true).ConfigureAwait(false).GetAwaiter().GetResult();
                    //pdfText = OCRLib.PdfImageConvert.GoogleOcr(pdfFile, false, false, apiKey).ConfigureAwait(false).GetAwaiter().GetResult();
                    if (string.IsNullOrWhiteSpace(pdfText))
                    {

                        //  pdfText = GetImageText(pdfFile, wfImage, true).ConfigureAwait(false).GetAwaiter().GetResult();
                        //  if (string.IsNullOrWhiteSpace(pdfText))
                        //  {
                        SbErrors.AppendLine($"{OCRConstants.StartTableRow}{OCRConstants.StartTableData}{ocrPdf.Key}{OCRConstants.EndTableDate}{OCRConstants.StartTableData}{ocrPdf.Value.IndexNumber}{OCRConstants.EndTableDate}" +
                        $"{OCRConstants.StartTableData}{ocrPdf.Value.FinancialNumber}{OCRConstants.EndTableDate}{OCRConstants.StartTableData}No Text found{OCRConstants.EndTableDate}{OCRConstants.StartTableData}{ocrPdf.Value.ScanDate.ToString("MM-dd-yyyy")}{OCRConstants.EndTableDate} {OCRConstants.EndTableRow}");
                        TotalRecordsErrors++;
                        Match match = Regex.Match(ocrPdf.Value.FinancialNumber, skippReqs, RegexOptions.IgnoreCase);
                        if (match.Success)
                        {
                            TL.TraceLogger.TraceLoggerInstance.TraceWarning($"Skipping since log file {ocrPdf.Value.IndexNumber} ");
                            TL.TraceLogger.TraceLoggerInstance.TraceWaringConsole($"Skipping since log file {ocrPdf.Value.IndexNumber} ");
                            pdfText = $"Log File {ocrPdf.Value.IndexNumber}";
                            UploadFullText(workingFolder, pdfText, ocrPdf.Key, ocrPdf.Value.ScanDate).ConfigureAwait(false).GetAwaiter().GetResult();
                            DeleteTifImage(ocrPdf.Value.FileUrl, OcrWebAPI.OCRApisInctance.AzureImageContanier).ConfigureAwait(false).GetAwaiter().GetResult();
                        }
                        runTimer.Stop();
                        Console.WriteLine();
                        TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Total time to process {runTimer.Elapsed} Lab Req Index Number {ocrPdf.Value.FinancialNumber} ");
                        TL.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Total time to process {runTimer.Elapsed} Lab Req Index Number {ocrPdf.Value.FinancialNumber} ");
                        continue;
                        // }
                    }
                    if (checkLabReqNum)
                    {
                        if (indexLabReqs)
                        {
                            CheckLabReqNumber(workingFolder, pdfText, ocrPdf.Key, ocrPdf.Value.IndexNumber, ocrPdf.Value.IndexNumber, ocrPdf.Value.FinancialNumber, OcrWebAPI.OCRApisInctance.FinIndexNumberRegx, ocrPdf.Value.ScanDate, wfImage, pdfFile).ConfigureAwait(false).GetAwaiter().GetResult();
                        }
                        else
                            CheckLabReqNumber(workingFolder, pdfText, ocrPdf.Key, ocrPdf.Value.RequisitionNumber, ocrPdf.Value.IndexNumber, ocrPdf.Value.FinancialNumber, OcrWebAPI.OCRApisInctance.RequisitionNumberRegx, ocrPdf.Value.ScanDate, wfImage, pdfFile).ConfigureAwait(false).GetAwaiter().GetResult();
                    }

                    else
                    {
                        pdfText = $"{ocrPdf.Value.IndexNumber} {ocrPdf.Value.FinancialNumber} {ocrPdf.Value.RequisitionNumber} {pdfText}";
                        UploadFullText(workingFolder, pdfText, ocrPdf.Key, ocrPdf.Value.ScanDate).ConfigureAwait(false).GetAwaiter().GetResult();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine();
                    //pdfText = GetImageText(pdfFile, wfImage).ConfigureAwait(false).GetAwaiter().GetResult();
                    SbErrors.AppendLine($"{OCRConstants.StartTableRow}{OCRConstants.StartTableData}{ocrPdf.Key}{OCRConstants.EndTableDate}{OCRConstants.StartTableData}{ocrPdf.Value.IndexNumber}{OCRConstants.EndTableDate}" +
                    $"{OCRConstants.StartTableData}{ocrPdf.Value.FinancialNumber}{OCRConstants.EndTableDate}{OCRConstants.StartTableData}Error Processing Pfd File {ex.Message}{OCRConstants.EndTableDate}{OCRConstants.StartTableData}{ocrPdf.Value.ScanDate.ToString("MM-dd-yyyy")}{OCRConstants.EndTableDate} {OCRConstants.EndTableRow} </p>");
                    TL.TraceLogger.TraceLoggerInstance.TraceError($"Lab Req Index Number {ocrPdf.Value.FinancialNumber} {ex.Message}");
                    TL.TraceLogger.TraceLoggerInstance.TraceErrorConsole($"Lab Req Index Number {ocrPdf.Value.FinancialNumber} {ex.Message}");
                    TotalRecordsErrors++;

                }
              
               
                if ((TotalRecordsProcessed % cWF) == 0)
                {
                    TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Cleanup working folder {cWF}");
                    TL.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Cleanup working folder {cWF}");
                    OcrWebAPI.OCRApisInctance.CLeanUpFolder(workingFolder, 0);
                    // HU.Utilities.DeleteFiles(workingFolder, 0);
                }
                Console.WriteLine();
                runTimer.Stop();
                TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Total time to process {runTimer.Elapsed} Lab Req Index Number {ocrPdf.Value.FinancialNumber} ");
                TL.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Total time to process {runTimer.Elapsed} Lab Req Index Number {ocrPdf.Value.FinancialNumber} ");


            }
        }
        static async Task<string> GetOCRText(string pdfUrl, string wfImageFolder, string workingFolder)
        {
            TL.TraceLogger.TraceLoggerInstance.TraceInformation($"GetOCRText for pdf file {pdfUrl}");
            TL.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"GetOCRText for pdf file {pdfUrl}");
            string ocrText = string.Empty;
            Uri uriPath = new Uri(pdfUrl);
            string pngFname = string.Empty;
            string bCodes = string.Empty;
            string path = $@"{uriPath.Scheme.ToString()}://{uriPath.Host}/{OcrWebAPI.OCRApisInctance.AzureImageContanier}/";
            OcrWebAPI.OCRApisInctance.CLeanUpFolder(wfImageFolder, 0);
            //    HU.Utilities.DeleteFiles(wfImageFolder, 0);
            string ocrFile = DownLoadPdf(new Uri(path), workingFolder, $"{Path.GetFileNameWithoutExtension(pdfUrl)}.TIF").ConfigureAwait(false).GetAwaiter().GetResult();
            string tifFname = Path.Combine(workingFolder, $"{Path.GetFileNameWithoutExtension(pdfUrl)}.TIF");
            try
            {
                if (Edocs.HelperUtilities.Utilities.CheckFileExists(tifFname))
                {
                    //    tifFname = @"D:\Archives\Lab_Req_Copy\labreqs\2022\02-11\2389f15f-0901-4132-a5b6-33813bedea23\0c6e4a58-9c48-4fba-a32a-c447cacd902c.TIF";
                    using (System.Drawing.Image image = System.Drawing.Image.FromFile(tifFname))
                    {
                        pngFname = Path.Combine(wfImageFolder, $"{Path.GetFileNameWithoutExtension(pdfUrl)}.png");
                        image.Save(pngFname, System.Drawing.Imaging.ImageFormat.Png);
                        image.Dispose();
                        // byte[] bitMap = Edocs.Ocr.Convert.Libaray.PdfImageConvert.ImageToBase64(image, System.Drawing.Imaging.ImageFormat.Png);
                    }
                    ocrText = OCRLib.PdfImageConvert.OCRSrace(pngFname, true, false, OcrWebAPI.OCRApisInctance.OCRAPIKey, "image.png").ConfigureAwait(false).GetAwaiter().GetResult();
                    if (!(string.IsNullOrWhiteSpace(ocrText)))
                    {
                        bCodes = BR.BCREader.InstanceBRader.GetBarCodes(pngFname).ConfigureAwait(false).GetAwaiter().GetResult();
                        if (!(string.IsNullOrWhiteSpace(bCodes)))
                            ocrText = $"BarCodes Found {bCodes.Trim()} {ocrText}";
                    }



                }
                else
                {
                    TL.TraceLogger.TraceLoggerInstance.TraceWaringConsole($"GetOCRText file not found tif file {tifFname}");
                    TL.TraceLogger.TraceLoggerInstance.TraceWaringConsole($"GetOCRText file not found tif file {tifFname}");
             //       SbErrors.AppendLine($"{OCRConstants.StartTableRow}{OCRConstants.StartTableData}N/A{OCRConstants.StartTableData}N/A{OCRConstants.EndTableDate}" +
             //$"{OCRConstants.StartTableData}N/A{OCRConstants.EndTableDate}{OCRConstants.StartTableData}GetOCRText file not found tif file {tifFname} {OCRConstants.EndTableDate}{OCRConstants.StartTableData}{DateTime.Now.ToString("MM-dd-yyyy")}{OCRConstants.EndTableDate} {OCRConstants.EndTableRow}");
                }
            }
            catch (Exception ex)
            {
                TL.TraceLogger.TraceLoggerInstance.TraceError($"GetOCRText downloading file {path}{ Path.GetFileNameWithoutExtension(pdfUrl)}.TIF {ex.Message}");
                TL.TraceLogger.TraceLoggerInstance.TraceErrorConsole($"GetOCRText downloading file {path}{ Path.GetFileNameWithoutExtension(pdfUrl)}.TIF {ex.Message}");
     //           SbErrors.AppendLine($"{OCRConstants.StartTableRow}{OCRConstants.StartTableData}N/A{OCRConstants.StartTableData}N/A{OCRConstants.EndTableDate}" +
     //$"{OCRConstants.StartTableData}N/A{OCRConstants.EndTableDate}{OCRConstants.StartTableData}GetOCRText file not found tif file {tifFname} {ex.Message} {OCRConstants.EndTableDate}{OCRConstants.StartTableData}{DateTime.Now.ToString("MM-dd-yyyy")}{OCRConstants.EndTableDate} {OCRConstants.EndTableRow}");
            }
            if (string.IsNullOrWhiteSpace(ocrText))
            {
                TL.TraceLogger.TraceLoggerInstance.TraceWarning($"GetOCRText no text found in file {pngFname}");
                TL.TraceLogger.TraceLoggerInstance.TraceWarning($"GetOCRText no text found in file {pngFname}");
                DownLoadPdf(new Uri(pdfUrl), workingFolder, Path.GetFileName(pdfUrl)).ConfigureAwait(false).GetAwaiter().GetResult();
                tifFname = Path.Combine(workingFolder, Path.GetFileName(pdfUrl));
                workingFolder = Path.Combine(workingFolder, OCRConstants.PDFWorkingFolder);
                HelperUtilities.Utilities.CreateDirectory(workingFolder);
                ocrText = GetImageText(tifFname, workingFolder, true).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            // DownLoadPdf(new Uri(ocrPdf.Value.FileUrl.ToString()), workingFolder, Path.GetFileName(ocrPdf.Value.FileUrl)).ConfigureAwait(false).GetAwaiter().GetResult();
            path = $"{path}{Path.GetFileNameWithoutExtension(pdfUrl)}.TIF";
            DeleteTifImage(path, OcrWebAPI.OCRApisInctance.AzureImageContanier).ConfigureAwait(false).GetAwaiter().GetResult();
            return ocrText;
        }

        static async Task<string> GetImageText(string pdfFileName, string wfImage, bool throwException)
        {
            TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Method GetImageText for pdf file {pdfFileName}");
            TL.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Method GetImageText for pdf file {pdfFileName}");
            string pdfText = string.Empty;
            string retBcodes = string.Empty;
            ConvertedPDFToImage = true;
            try
            {

                OcrWebAPI.OCRApisInctance.CLeanUpFolder(wfImage, 0);
                string searchFiles = $"{Path.GetFileNameWithoutExtension(pdfFileName)}*.* ";
                OCRLib.PdfImageConvert.SavePDFAsImage(pdfFileName, wfImage, string.Empty, OcrWebAPI.OCRApisInctance.ImageDpi, "png").ConfigureAwait(false).GetAwaiter().GetResult();
                //   pdfFileName = Path.GetFileNameWithoutExtension(pdfFileName);
                foreach (var image in HU.Utilities.GetDirectoryFiles(wfImage, searchFiles, SearchOption.AllDirectories).ConfigureAwait(false).GetAwaiter().GetResult())
                {

                    TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Method GetImageText for image {image}");
                    TL.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Method GetImageText for image {image}");
                    //  string ocrTxt = OCRLib.PdfImageConvert.OCRASPOSE(image, true, false, @"L:\EdocsGitHub\Edocs.Ocr.Convert.Libaray\Aspose.OCR.NET.lic").ConfigureAwait(false).GetAwaiter().GetResult();
                    // string ocrTxt = OCRLib.PdfImageConvert.OCRSrace(image, true, false, OcrWebAPI.OCRApisInctance.OCRAPIKey).ConfigureAwait(false).GetAwaiter().GetResult();
                    //image
                    string ocrTxt = OCRLib.PdfImageConvert.OCRSrace(image, true, false, OcrWebAPI.OCRApisInctance.OCRAPIKey, "image.png").ConfigureAwait(false).GetAwaiter().GetResult();
                    if (!(string.IsNullOrWhiteSpace(ocrTxt)))
                        pdfText += ocrTxt;
                    string bCodes = BR.BCREader.InstanceBRader.GetBarCodes(image).ConfigureAwait(false).GetAwaiter().GetResult();
                    if (!(string.IsNullOrWhiteSpace(bCodes)))
                        retBcodes += $"{bCodes} ";
                    TotalPDFSConvertedToImage++;
                }
                if (!(string.IsNullOrWhiteSpace(retBcodes)))
                {
                    pdfText = $"BarCodes Found {retBcodes.Trim()} {pdfText}";
                }
                if (!(string.IsNullOrWhiteSpace(pdfText)))
                    pdfText = pdfText.Trim();
            }
            catch (Exception ex)
            {
                if (throwException)
                    throw new Exception(ex.Message);
                else
                {
                    TotalRecordsErrors++;
                    TL.TraceLogger.TraceLoggerInstance.TraceError($"Method GetImageText for image file {pdfFileName} {ex.Message}");
                    TL.TraceLogger.TraceLoggerInstance.TraceError($"Method GetImageText for image file {pdfFileName} {ex.Message}");
                    SbErrors.AppendLine($"{OCRConstants.StartTableRow}{OCRConstants.StartTableData}N/A{OCRConstants.StartTableData}N/A{OCRConstants.EndTableDate}" +
                $"{OCRConstants.StartTableData}N/A{OCRConstants.EndTableDate}{OCRConstants.StartTableData}Error getting image text for image {pdfFileName} {ex.Message}{OCRConstants.EndTableDate}{OCRConstants.StartTableData}{DateTime.Now.ToString("MM-dd-yyyy")}{OCRConstants.EndTableDate} {OCRConstants.EndTableRow}");
                }
            }
            return pdfText;
        }
        static async Task CheckLabReqNumber(string wf, string pdfText, int labReqID, string reqNumber, string indeNum, string finNum, string regxStr, DateTime scanDate, string wfImage, string pdfFileName)
        {
            TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Method CheckLabReqNumber lab id {labReqID} index number {indeNum} fin number {finNum} req number {reqNumber}");
            TL.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Method CheckLabReqNumber lab id {labReqID} index number {indeNum} fin number {finNum} req number {reqNumber}");
            if (!(string.IsNullOrEmpty(reqNumber)))
            {
                System.Text.RegularExpressions.Match match = OcrWebAPI.OCRApisInctance.RegxMatch(reqNumber, regxStr).ConfigureAwait(false).GetAwaiter().GetResult();
                if (match.Success)
                {
                    TotalCorrectLabReqsNums++;
                    pdfText = $"{indeNum}\r\n {finNum}\r\n {pdfText}";
                    UploadFullText(wf, pdfText, labReqID, scanDate).ConfigureAwait(false).GetAwaiter().GetResult();
                    //  TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Total time to process Lab Req Index Number {finNum}");
                    //TL.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Total time to process Lab Req Index Number {finNum}");
                    return;

                }



            }
            GetLabReqNumber(wf, pdfText, labReqID, reqNumber, indeNum, finNum, reqNumber, scanDate, wfImage, pdfFileName).ConfigureAwait(false).GetAwaiter().GetResult();
        }



        static async Task GetLabReqNumber(string wf, string pdfText, int labReqID, string reqNum, string indeNum, string finNum, string regxStr, DateTime scanDate, string wfImage, string pdfFileName)
        {

            TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Method GetLabReqNumber labb req {reqNum}");
            TL.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Method GetLabReqNumber labb req {reqNum}");
            bool found = false;
            string indexNumbersChanged = OcrWebAPI.OCRApisInctance.IndexNumbersChanged;
            indexNumbersChanged = indexNumbersChanged.Replace("{DATE}", DateTime.Now.ToString("MM_dd_yyyy"));
            // int loop = 0;
            // int numLoop = 2;
            try
            {

                if (!(File.Exists(indexNumbersChanged)))
                    File.AppendAllText(indexNumbersChanged, "labReqID,Index Number,Fin Number,Search number,Found,Scan Date\r\n");
                //  string found

                //  while (loop < numLoop)
                //  {
                //  loop++;
                IList<string> matches = OcrWebAPI.OCRApisInctance.RegxMatchCollectionList(pdfText).ConfigureAwait(false).GetAwaiter().GetResult();
                foreach (var matchesFound in matches)
                {
                    //         if (matchesFound.ToString().StartsWith("1100513"))
                    //         {

                    //             SbErrors.AppendLine($"{OCRConstants.StartTableRow}{OCRConstants.StartTableData}{labReqID}{OCRConstants.EndTableDate}{OCRConstants.EndTableDate}{OCRConstants.StartTableData}{indeNum}{OCRConstants.EndTableDate}" +
                    //$"{OCRConstants.StartTableData}{finNum}{OCRConstants.EndTableDate}{OCRConstants.StartTableData}found possible match {matchesFound}{OCRConstants.EndTableDate}{OCRConstants.StartTableData}{scanDate.ToString("MM-dd-yyyy")}{OCRConstants.EndTableDate} {OCRConstants.EndTableRow}");
                    //             Console.WriteLine(labReqID);
                    //             File.AppendAllText(indexNumbersChanged, $"{labReqID},{indeNum},{finNum},{reqNum},maybea match,{scanDate.ToString("MM-dd-yyyy")}\r\n");
                    //             // Console.ReadKey();
                    //         }
                    System.Text.RegularExpressions.Match match = OcrWebAPI.OCRApisInctance.RegxMatch(matchesFound, regxStr).ConfigureAwait(false).GetAwaiter().GetResult();
                    if (match.Success)
                    {
                        TotalCorrectLabReqsNums++;
                        found = true;
                        // loop++;
                        break;
                    }
                    else
                    {
                        if (string.Compare(matchesFound, indeNum, true) == 0)
                        {
                            File.AppendAllText(indexNumbersChanged, $"{labReqID},{indeNum},{finNum},{reqNum},found match {matchesFound},{scanDate.ToString("MM-dd-yyyy")}\r\n");
                            TotalCorrectLabReqsNums++;
                            found = true;
                            //    loop++;
                            break;
                        }
                    }
                }

                //   }
                //    if ((loop == 1) && (!(ConvertedPDFToImage)))
                //    {
                //        string tempText = pdfText;
                //        pdfText = GetImageText(pdfFileName, wfImage, false).ConfigureAwait(false).GetAwaiter().GetResult();
                //        if (string.IsNullOrWhiteSpace(pdfText))
                //        {
                //            pdfText = tempText;
                //            loop++;
                //        }
                //        else
                //        {
                //            if (string.Compare(pdfText, tempText, true) == 0)
                //            {
                //                loop++;
                //            }
                //        }
                //    }
                //    else
                //        loop++;
                //}
            }
            catch (Exception ex)
            {
                TotalRecordsErrors++;
                SbErrors.AppendLine($"{OCRConstants.StartTableRow}{OCRConstants.StartTableData}{labReqID}{OCRConstants.EndTableDate}{OCRConstants.EndTableDate}{OCRConstants.StartTableData}{indeNum}{OCRConstants.EndTableDate}" +
                 $"{OCRConstants.StartTableData}{finNum}{OCRConstants.EndTableDate}{OCRConstants.StartTableData}Error looking up lab req number {ex.Message}{OCRConstants.EndTableDate}{OCRConstants.StartTableData}{scanDate.ToString("MM-dd-yyyy")}{OCRConstants.EndTableDate} {OCRConstants.EndTableRow}");
                TL.TraceLogger.TraceLoggerInstance.TraceError($"lab req num {indeNum} fin number {reqNum} up lab req number {reqNum} scan date {scanDate.ToString("MM-dd-yyyy")} {ex.Message}");
                TL.TraceLogger.TraceLoggerInstance.TraceErrorConsole($"lab req num {indeNum} fin number {finNum} search lab fin/index number scan date {scanDate.ToString("MM-dd-yyyy")} {reqNum} {ex.Message}");
                throw new Exception($"In Method GetLabReqNumber index number {indeNum} req number {reqNum} scan date  {scanDate.ToString("MM-dd-yyyy")}{ex.Message}");
            }
            if (!(found))
            {

                TL.TraceLogger.TraceLoggerInstance.TraceWarning($"Index Number {indeNum} fin number {finNum} search lab fin/index number {reqNum}  scan date {scanDate.ToString("MM-dd-yyyy")} not found ");
                TL.TraceLogger.TraceLoggerInstance.TraceWarning($"Index Number {indeNum} fin number {finNum} search lab fin/index number {reqNum}  scan date {scanDate.ToString("MM-dd-yyyy")} not found ");
                SbErrors.AppendLine($"{OCRConstants.StartTableRow}{OCRConstants.StartTableData}{labReqID}{OCRConstants.EndTableDate}{OCRConstants.StartTableData}{indeNum}{OCRConstants.EndTableDate}" +
                 $"{OCRConstants.StartTableData}{finNum}{OCRConstants.EndTableDate}{OCRConstants.StartTableData}index/fin number don't match search lab fin/index number {reqNum}{OCRConstants.EndTableDate}{OCRConstants.StartTableData}{scanDate.ToString("MM-dd-yyyy")}{OCRConstants.EndTableDate} {OCRConstants.EndTableRow}");
                File.AppendAllText(indexNumbersChanged, $"{labReqID},{indeNum},{finNum},{reqNum},false,{scanDate.ToString("MM-dd-yyyy")}\r\n");
                TotalLabReqsNotFound++;

            }
            pdfText = $"{indeNum}\r\n{finNum}\r\n{pdfText}";
            UploadFullText(wf, pdfText, labReqID, scanDate).ConfigureAwait(false).GetAwaiter().GetResult();

        }

        static async Task UploadFullText(string wf, string pdfText, int labReqID, DateTime scanDate)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(pdfText))
                    throw new Exception($"pdf text empty for lab req id {labReqID}");

                TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Method UploadFullTextlab req num {labReqID}");
                TL.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Method UploadFullTextlab req num {labReqID}");
                string uploadFileName = Path.Combine(wf, "pdftext.txt");
                HU.Utilities.DeleteFile(uploadFileName);
                File.WriteAllText(uploadFileName, pdfText);
                string uploadText = string.Empty;
                PDFFullTextModel fullTextModel = new PDFFullTextModel();
                fullTextModel.ID = labReqID;
                using (StreamReader sr = new StreamReader(uploadFileName))
                {
                    while ((uploadText = sr.ReadLine()) != null)
                    {
                        if (!(string.IsNullOrWhiteSpace(uploadText)))
                        {
                            fullTextModel.PDFFullText += uploadText + "\r\n";

                        }
                    }
                }
                if (OcrWebAPI.OCRApisInctance.UseWebApi)
                    OcrWebAPI.OCRApisInctance.UpLoadPDFFullTextGetLabReqs(fullTextModel, OcrWebAPI.OCRApisInctance.WebUri, OCRConstants.LabReqsPDFFullTextController).ConfigureAwait(false).GetAwaiter().GetResult();
                else
                    LocalSqlServer.SqlServerInstance.AddNYPPDFFullText(fullTextModel, OcrWebAPI.OCRApisInctance.SqlConnectionStr, scanDate).ConfigureAwait(false).GetAwaiter().GetResult();
                TotalLabReqsNumsOCRUpLoaded++;
            }
            catch (Exception ex)
            {
                TotalRecordsErrors++;
                SbErrors.AppendLine($"{OCRConstants.StartTableRow}{OCRConstants.StartTableData}{labReqID}{OCRConstants.EndTableDate}{OCRConstants.StartTableData}{labReqID}{OCRConstants.EndTableDate}" +
                $"{OCRConstants.StartTableData}N/A{OCRConstants.EndTableDate}{OCRConstants.StartTableData}adding lab req id {labReqID} {ex.Message}{OCRConstants.EndTableDate}{OCRConstants.StartTableData}{DateTime.Now.ToString("MM-dd-yyyy")}{OCRConstants.EndTableDate} {OCRConstants.EndTableRow}");
                TL.TraceLogger.TraceLoggerInstance.TraceError($"In method UploadFullText index number {labReqID} {ex.Message}");
                TL.TraceLogger.TraceLoggerInstance.TraceErrorConsole($"In method UploadFullText index number {labReqID} {ex.Message}");
                throw new Exception($"In method UploadFullText index number {labReqID} {ex.Message}");
            }
        }
        static async Task<string> DownLoadPdf(Uri azureContainerURL, string wf, string pdfFilename)
        {
            TL.TraceLogger.TraceLoggerInstance.TraceInformation($"In method DownLoadPdf pdf filname {pdfFilename}");
            TL.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"In method DownLoadPdf pdf filname {pdfFilename}");
            string pdfAzureContainer = Path.GetDirectoryName(azureContainerURL.LocalPath).Replace("\\", "/") + "/";
            pdfAzureContainer = pdfAzureContainer.Remove(0, 1);
            try
            {

                BS.AzureBlobStorage.BlobStorageInstance.AzureBlobAccountKey = OcrWebAPI.OCRApisInctance.AzureBlobAccountKey;
                BS.AzureBlobStorage.BlobStorageInstance.AzureBlobAccountName = OcrWebAPI.OCRApisInctance.AzureBlobAccountName;
                BS.AzureBlobStorage.BlobStorageInstance.AzureBlobStorageConnectionString = OcrWebAPI.OCRApisInctance.AzureBlobStorageConnectionString;
                return BS.AzureBlobStorage.BlobStorageInstance.DownLoadFileBlockBlod(pdfAzureContainer, pdfFilename, wf).ConfigureAwait(false).GetAwaiter().GetResult();
                //    return BS.AzureBlobStorage.BlobStorageInstance.DownLoadFileBlockBlod("labrecs/nyplabreqs/labreqs/2020-09-30/", pdfFilename, wf).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                TL.TraceLogger.TraceLoggerInstance.TraceErrorConsole($"In method DownLoadPdf pdf filname {pdfFilename} {ex.Message}");
                TL.TraceLogger.TraceLoggerInstance.TraceError($"In method DownLoadPdf pdf filname {pdfFilename} {ex.Message}");
                TotalRecordsErrors++;
                SbErrors.AppendLine($"{OCRConstants.StartTableRow}{OCRConstants.StartTableData}N/A{OCRConstants.EndTableDate}{OCRConstants.StartTableData}{pdfFilename}{OCRConstants.EndTableDate}" +
                $"{OCRConstants.StartTableData}N/A{OCRConstants.EndTableDate}{OCRConstants.StartTableData}error downloading pdf file {pdfFilename} {ex.Message}{OCRConstants.EndTableDate}{OCRConstants.StartTableData}{DateTime.Now.ToString("MM-dd-yyyy")}{OCRConstants.EndTableDate} {OCRConstants.EndTableRow}");
                throw new Exception($"DownLoadPdf for pdf filename {pdfFilename} {ex.Message}");
            }
        }
        static async Task RunMerDem(DateTime stDate,DateTime endDate,int merge)
        {
            TimeSpan ts =  stDate- endDate;
            int numDays = ts.Days;
            string processArgs = string.Empty;
            try
            { 
            if(numDays == 0)
            {
                numDays = 5;
            }
            if (numDays < 0)
                numDays = numDays * -1;
                processArgs = OcrWebAPI.OCRApisInctance.ProcessArgs.Replace(OCRConstants.RepStrDaystoProcess,numDays.ToString());
                if(merge != 0)
                 processArgs = $"{processArgs} /merge:{merge}";
                TL.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"In method RunMerDem  startdate {stDate} end date {endDate} process {OcrWebAPI.OCRApisInctance.RunProcess} process args {processArgs} working folder {OcrWebAPI.OCRApisInctance.ProcessWorkingFolder} num days {numDays}");
                TL.TraceLogger.TraceLoggerInstance.TraceInformation($"In method RunMerDem  startdate {stDate} end date {endDate} process {OcrWebAPI.OCRApisInctance.RunProcess} process args {processArgs} working folder {OcrWebAPI.OCRApisInctance.ProcessWorkingFolder} num days {numDays}");
                HU.Utilities.RunTask(OcrWebAPI.OCRApisInctance.RunProcess, processArgs, OcrWebAPI.OCRApisInctance.ProcessWorkingFolder, false);
                SbErrors.AppendLine($"{OCRConstants.StartTableRow}{OCRConstants.StartTableData}N/A{OCRConstants.EndTableDate}{OCRConstants.StartTableData}Process Started{OCRConstants.EndTableDate}" +
               $"{OCRConstants.StartTableData}N/A{OCRConstants.EndTableDate}{OCRConstants.StartTableData}In method RunMerDem dor startdate {stDate} end date {endDate} process {OcrWebAPI.OCRApisInctance.RunProcess} process args {processArgs} working folder {OcrWebAPI.OCRApisInctance.ProcessWorkingFolder} num days {numDays}{OCRConstants.EndTableDate}{OCRConstants.StartTableData}{DateTime.Now.ToString("MM-dd-yyyy")}{OCRConstants.EndTableDate} {OCRConstants.EndTableRow}");
              
            }
            catch(Exception ex)
            {
                TL.TraceLogger.TraceLoggerInstance.TraceErrorConsole($"In method RunMerDem dor startdate {stDate} end date {endDate} process {OcrWebAPI.OCRApisInctance.RunProcess} process args {processArgs} working folder {OcrWebAPI.OCRApisInctance.ProcessWorkingFolder} num days {numDays} {ex.Message}");
                TL.TraceLogger.TraceLoggerInstance.TraceError($"In method RunMerDem dor startdate {stDate} end date {endDate} process {OcrWebAPI.OCRApisInctance.RunProcess} process args {processArgs} working folder {OcrWebAPI.OCRApisInctance.ProcessWorkingFolder} num days {numDays} {ex.Message}");
                TotalRecordsErrors++;
                SbErrors.AppendLine($"{OCRConstants.StartTableRow}{OCRConstants.StartTableData}N/A{OCRConstants.EndTableDate}{OCRConstants.StartTableData}Error Process not started{OCRConstants.EndTableDate}" +
                $"{OCRConstants.StartTableData}N/A{OCRConstants.EndTableDate}{OCRConstants.StartTableData}In method RunMerDem dor startdate {stDate} end date {endDate} process {OcrWebAPI.OCRApisInctance.RunProcess} process args {processArgs} working folder {OcrWebAPI.OCRApisInctance.ProcessWorkingFolder} num days {numDays} {ex.Message}{OCRConstants.EndTableDate}{OCRConstants.StartTableData}{DateTime.Now.ToString("MM-dd-yyyy")}{OCRConstants.EndTableDate} {OCRConstants.EndTableRow}");
            }

        }
        static void CheckMultiProcessingRunning(string[] args)
        {
            try
            {


                int processId = HU.Utilities.CheckMuptiProcessingRunning("Edocs.OCR.FullText.PDF");
                Stopwatch executionTimer = Stopwatch.StartNew();
                if (processId > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (string ar in args)
                        sb.AppendLine(ar);

                    string errMess = $"Processing Edocs.OCR.FullText.PDF all ready running <br/> stopping process id {processId}<br/> with args {sb.ToString()}";
                    string emilSubJect = "Error: Processing Edocs.OCR.FullText.PDF all ready running";
                    TimeSpan ts = DateTime.Now - DateTime.Now;
                    SE.EmailInstance.SendEmail(string.Empty, errMess,emilSubJect, string.Empty, false, string.Empty);
                   // EmailSend(errMess, executionTimer.Elapsed, true);
                  //  TL.TraceLogger.TraceLoggerInstance.CloseTraceFile();
                    HU.Utilities.EndTaskById(processId);
                    executionTimer.Stop();
                    Environment.Exit(-1);
                }
              
                //OpenLogFile();
            }
            catch (Exception ex)
            {
                Environment.Exit(-1);
            }
        }
        static async Task DeleteTifImage(string azureContainerURL, string azureContanier)
        {
            if(!(azureContainerURL.Contains(OcrWebAPI.OCRApisInctance.AzureImageContanier)))
            {
                string fName = $"{Path.GetFileNameWithoutExtension(azureContainerURL)}.TIF";
                Uri uriPath = new Uri(azureContainerURL);
                azureContainerURL = $@"{uriPath.Scheme}://{uriPath.Host}/{OcrWebAPI.OCRApisInctance.AzureImageContanier}/";
                azureContainerURL = Path.Combine(azureContainerURL, fName);
            }
          //  azureContainerURL = $"{azureContainerURL}{Path.GetFileNameWithoutExtension(azureContainerURL)}.TIF";
            TL.TraceLogger.TraceLoggerInstance.TraceInformation($"In method DeleteTifImage  filname {azureContainerURL} for azure contanier {azureContanier}");
            TL.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"In method DeleteTifImage  filname {azureContainerURL} for azure contanier {azureContanier}");
           
            //  string pdfAzureContainer = Path.GetDirectoryName(azureContainerURL.LocalPath).Replace("\\", "/") + "/";
            //  pdfAzureContainer = pdfAzureContainer.Remove(0, 1);
            try
            {

                BS.AzureBlobStorage.BlobStorageInstance.AzureBlobAccountKey = OcrWebAPI.OCRApisInctance.AzureBlobAccountKey;
                BS.AzureBlobStorage.BlobStorageInstance.AzureBlobAccountName = OcrWebAPI.OCRApisInctance.AzureBlobAccountName;
                BS.AzureBlobStorage.BlobStorageInstance.AzureBlobStorageConnectionString = OcrWebAPI.OCRApisInctance.AzureBlobStorageConnectionString;
                BS.AzureBlobStorage.BlobStorageInstance.DeleteAzureBlobFile(azureContainerURL, azureContanier).ConfigureAwait(false).GetAwaiter().GetResult();
                //    return BS.AzureBlobStorage.BlobStorageInstance.DownLoadFileBlockBlod("labrecs/nyplabreqs/labreqs/2020-09-30/", pdfFilename, wf).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                TL.TraceLogger.TraceLoggerInstance.TraceErrorConsole($"In method DeleteTifImage tif filname {azureContainerURL} {ex.Message}");
                TL.TraceLogger.TraceLoggerInstance.TraceError($"In method DeleteTifImage tif filname {azureContainerURL} {ex.Message}");
               // TotalRecordsErrors++;
                //SbErrors.AppendLine($"{OCRConstants.StartTableRow}{OCRConstants.StartTableData}N/A{OCRConstants.EndTableDate}{OCRConstants.StartTableData}{azureContainerURL}{OCRConstants.EndTableDate}" +
                //$"{OCRConstants.StartTableData}N/A{OCRConstants.EndTableDate}{OCRConstants.StartTableData}error DeleteTifImage tif file {azureContainerURL} {ex.Message}{OCRConstants.EndTableDate}{OCRConstants.StartTableData}{DateTime.Now.ToString("MM-dd-yyyy")}{OCRConstants.EndTableDate} {OCRConstants.EndTableRow}");
                //   throw new Exception($"Deleting file {deleteTifImage} {ex.Message}");
            }
        }
        static async Task CreateHtmlFile(string scanStDate, string scanEDate)
        {
            TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Method CreateHtmlFIle");
            TL.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Method CreateHtmlFIle");
            string htmlF = OcrWebAPI.OCRApisInctance.HtmlFile;
            HU.Utilities.CreateDirectory(htmlF);
            string ts = TL.TraceLogger.TraceLoggerInstance.StopStopWatch();

            try
            {
                string htmlStr = File.ReadAllText(OcrWebAPI.OCRApisInctance.HTMLtemplate);
                htmlStr = HU.Utilities.ReplaceString(htmlStr, "{Message}", $"Done ocring pdf files total run time {ts} run date {DateTime.Now.ToString()}");
                htmlStr = HU.Utilities.ReplaceString(htmlStr, "{scandates}", $"Scan Start Date: {scanStDate} Scan End Date: {scanEDate} ran on machine {Environment.MachineName}");

                if (SbErrors.Length > 0)
                    htmlStr = HU.Utilities.ReplaceString(htmlStr, "{Errors}", SbErrors.ToString());
                else
                    htmlStr = HU.Utilities.ReplaceString(htmlStr, "{Errors}", $"{OCRConstants.StartTableRow}{OCRConstants.StartTableData}No Errors{OCRConstants.EndTableDate}{OCRConstants.StartTableData}No Errors{OCRConstants.EndTableDate}{OCRConstants.StartTableData}No Errors{OCRConstants.EndTableDate}{OCRConstants.StartTableData}No Errors{OCRConstants.EndTableDate}{OCRConstants.StartTableData}{DateTime.Now.ToString("MM-dd-yyyy")}{OCRConstants.EndTableDate}{OCRConstants.EndTableRow}");


                //htmlStr = HU.Utilities.ReplaceString(htmlStr, "{Totals}", $"<br /><p>{TotalRecordsProcessed}</p><br /><p> Not Found {TotalLabReqsNotFound}</p> <br /><p>Errors {TotalRecordsErrors}</p><br /><p> Not CHanged {TotalCorrectLabReqsNums}</p><br /><p> changed {TotalLabReqsChanged}</p>");
                htmlStr = HU.Utilities.ReplaceString(htmlStr, "{Totals}", $"<p>Total Records Returned {TotalLabReqsReturned }</p><p>Records Processed {TotalRecordsProcessed}</p><p>Fin/Index Numbers Don't Match {TotalLabReqsNotFound}</p><p>Index/Fin Numbers Match {TotalCorrectLabReqsNums}</p><p>Errors Found {TotalRecordsErrors}</p><p>Records OCR UpLoaded {TotalLabReqsNumsOCRUpLoaded}</p><p>Pdfs Converted To Images {TotalPDFSConvertedToImage}");
                File.WriteAllText(htmlF, htmlStr);
                EmailSend(ts, htmlF, true, scanStDate, scanEDate).ConfigureAwait(true).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Method CreateHtmlFIle {ex.Message}");
                TL.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Method CreateHtmlFIle {ex.Message}");
            }
            TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Total time to run {ts}");
            TL.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Total time to run {ts}");

        }
        static async Task UpLoadLogAzure(string logFileName)
        {
            try
            {

                TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Uploading log file {logFileName} to Azure cloud");
                TL.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Uploading log file {logFileName} to Azure cloud");
                TL.TraceLogger.TraceLoggerInstance.CloseTraceFile();
                ABS.BlobStorageInstance.AzureBlobAccountKey = OcrWebAPI.OCRApisInctance.AzureBlobAccountKey;
                ABS.BlobStorageInstance.AzureBlobAccountName = OcrWebAPI.OCRApisInctance.AzureBlobAccountName;
                ABS.BlobStorageInstance.AzureBlobStorageConnectionString = OcrWebAPI.OCRApisInctance.AzureBlobStorageConnectionString;
                logFileName = Path.GetDirectoryName(logFileName);
                foreach (var file in HU.Utilities.GetDirFilesName(logFileName))
                {
                    string fileName = Path.Combine(logFileName, file);
                    string upLoadText = File.ReadAllText(fileName, System.Text.Encoding.UTF8);

                    ABS.BlobStorageInstance.UploadAzureBlobTextFile(file, OcrWebAPI.OCRApisInctance.AzureBlobContanierAuditShare, upLoadText).ConfigureAwait(false).GetAwaiter().GetResult();

                    HU.Utilities.DeleteFile(fileName);
                }
            }
            catch (Exception ex)
            {
                try
                {


                    string message = $"Error uploading clean logs to azure cloud running assembly {System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.exe on machine {Environment.MachineName} runtime {DateTime.Now.ToString()} {ex.Message}";
                    SE.EmailInstance.SendEmail(string.Empty, ex.Message, $"Error uploading clean logs azure cloud machine {Environment.MachineName}", string.Empty, false, message);

                }
                catch { }
            }

        }
        static async Task EmailSend(string ts, string htmFile, bool useTraceLog, string scanStDate, string scanEDate)
        {
            if (useTraceLog)
                TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Sending email");
            try
            {


                string subject = $"Execution Summary process {System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.exe on machine {Environment.MachineName} runtime {DateTime.Now.ToString()}"; ;
                if (SbErrors.Length > 0)
                    subject = $"Execution Summary error running process {System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.exe machine {Environment.MachineName} runtime {DateTime.Now.ToString()}";
                //SE.EmailInstance.(string.Empty)
                string body = "Summary"
                    + "<h1>" + $"Scan Start Date: {scanStDate} Scan End Date: {scanEDate}<br/>"
                   + "<h1>Totals</h1><br/>Records Returned: " + TotalLabReqsReturned + "<br/>Records Processed: " + TotalRecordsProcessed++
                + "<br/>Index/Fin Number don't match: " + TotalLabReqsNotFound
                 + "<br/>Index/Fin Number match:  " + TotalCorrectLabReqsNums
                   + "<br/>Errors Found: " + TotalRecordsErrors
                   + "<br/>Records OCR Uploaded:" + TotalLabReqsNumsOCRUpLoaded
                   + "<br/>PDF Converted To Images:" + +TotalPDFSConvertedToImage
                + "<br/>Execution Time: " + ts;
                if (string.IsNullOrWhiteSpace(htmFile))
                    SE.EmailInstance.SendEmail(string.Empty, body, subject, string.Empty, true, string.Empty);
                else
                {

                    SE.EmailInstance.SendEmail(string.Empty, body, subject, htmFile, true, string.Empty);
                }
                if ((SbErrors.Length > 0) && (OcrWebAPI.OCRApisInctance.SendTextOnError == 1))
                {
                    if (useTraceLog)
                        TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Sending txt");
                    string message = $"Error running  {System.Reflection.Assembly.GetExecutingAssembly().GetName().Name} total errors:{TotalRecordsErrors}";
                    SE.EmailInstance.SendTxtMessage(message, true);
                    if (useTraceLog)
                        TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Text Sent");
                }
                if (useTraceLog)
                    TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Email Sent");
            }
            catch (Exception ex)
            {
                if (useTraceLog)
                    TL.TraceLogger.TraceLoggerInstance.TraceError($"Error sending email {ex.Message}");
            }
            if (useTraceLog)
                TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Done Method Sending email");
        }
    }
}

