using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POCR = Edocs.Ocr.Convert.Libaray;
using EDU = Edocs.HelperUtilities;
using TL = EdocsUSA.Utilities.Logging;
using Edocs.Ocr.SearchablePdf.ConstProp;
using Edocs.Ocr.SearchablePdf.Models;
using Edocs.Ocr.SearchablePdf.WebApis;
using System.IO;
using System.IO.Compression;

namespace Edocs.Ocr.SearchablePdf
{
    class OcrSearchPdf
    {

        static void Main(string[] args)
        {
             //  Environment.Exit(0);
            GetInputArgs(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        static async Task GetInputArgs(string[] args)
        {
            string settingFile = string.Empty;
            bool sPdf = false;
            try
            {
                foreach (string arg in args)
                {
                    if (arg.StartsWith(Constants.ArgSettingsFile, StringComparison.OrdinalIgnoreCase))
                    {
                        Properties.SettingsOCR = Properties.GetBatchSettingsObject<OcrSettingsModel>(arg.Substring(Constants.ArgSettingsFile.Length));
                          settingFile = arg.Substring(Constants.ArgSettingsFile.Length);
                      
                        //TL.TraceLogger.TraceLoggerInstanceInstance.TraceInformation($"Using xml file {xmlFile}");
                        continue;
                    }
                    if (arg.StartsWith(Constants.ArgSearchPDF, StringComparison.OrdinalIgnoreCase))
                    {

                        sPdf = true;

                        //TL.TraceLogger.TraceLoggerInstanceInstance.TraceInformation($"Using xml file {xmlFile}");
                        continue;
                    }
                    if (arg.StartsWith(Constants.ArgImgagePdfFolder, StringComparison.OrdinalIgnoreCase))
                    {
                         
                        settingFile = arg.Substring(Constants.ArgImgagePdfFolder.Length);

                        //TL.TraceLogger.TraceLoggerInstanceInstance.TraceInformation($"Using xml file {xmlFile}");
                        continue;
                    }
                    
                    //if (arg.StartsWith(Constants.ArgDataBase, StringComparison.OrdinalIgnoreCase))
                    //{
                    //    dbName = arg.Substring(Constants.ArgDataBase.Length);
                    //  //  TL.TraceLoggerInstance.TraceInformation($"Deleting records from database {dbName}");
                    //   // Sb.AppendLine($"<p style={Constants.Quote}color:#008000{Constants.Quote}>Deleting records from database {dbName}</p>");
                    //    continue;
                    //}
                    //if (arg.StartsWith(Constants.ArgUsage, StringComparison.OrdinalIgnoreCase))
                    //{
                    //   // ShowUsage().ConfigureAwait(false).GetAwaiter().GetResult();
                    //    showUsage = true;
                    //    break;
                    //}

                }
                // Uri uri = new Uri(Properties.SettingsOCR.InventoryTrackingApiUrl);
                if (!(sPdf))
                {
                    Properties.BackUpFolder = Properties.ArchiveFolder.Replace("{date}", DateTime.Now.ToString("MM-dd-yyyy")).Replace("{pname}", Properties.SettingsOCR.InventoryTrackingID);
                    EDU.Utilities.CreateDirectory(Properties.BackUpFolder);
                    EDU.Utilities.CopyFile(settingFile, Path.Combine(Properties.BackUpFolder, Path.GetFileName(settingFile)), true);
                    //   EDU.Utilities.DeleteFile(settingFile);
                    OcrImages().ConfigureAwait(false).GetAwaiter().GetResult();
                    ZipProjectFolder().ConfigureAwait(false).GetAwaiter().GetResult();
                }
                else 
                    CreateSearchablePdf(settingFile).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex) { }


        }
        static async Task CreateSearchablePdf(string pdfFName)
        {
           string pdfUrl = POCR.PdfImageConvert.OCRSraceCreateSearchablePdf(pdfFName, Properties.OCRAPIKey).ConfigureAwait(false).GetAwaiter().GetResult();
            string savePdf = $"{Path.GetFileNameWithoutExtension(pdfFName)}_Search_pdf.pdf";
            byte[] pdfBytes = File.ReadAllBytes(pdfUrl);
            File.WriteAllBytes($"{Path.Combine(Path.GetDirectoryName(pdfFName),savePdf)}", pdfBytes);

        }
        static async Task OcrImages()
        {
            Properties.SBOCRResults = new StringBuilder();
            Properties.TrackingModel = new UpLoadMDTrackingModel();
            Properties.SettingsOCR.OCRFolder = EDU.Utilities.CheckFolderPath(Properties.SettingsOCR.OCRFolder);

            foreach (string imgFile in EDU.Utilities.GetDirectoryFiles(Properties.SettingsOCR.OCRFolder, "*.*", SearchOption.TopDirectoryOnly).ConfigureAwait(false).GetAwaiter().GetResult())
            {
                try
                {


                    string ocrTxt = POCR.PdfImageConvert.OCRSrace(imgFile, null, true, false, Properties.OCRAPIKey, "2", true, "img.png").ConfigureAwait(false).GetAwaiter().GetResult();
                    Properties.TrackingModel.TotalOCR++;
                    if (!(string.IsNullOrWhiteSpace(ocrTxt)))
                    {
                        Properties.SBOCRResults.AppendLine(ocrTxt);
                        string destFolder = Path.Combine(Properties.BackUpFolder,$"\\Img\\{Path.GetFileName(imgFile)}");
                        EDU.Utilities.CopyFile(imgFile, destFolder, true);
                      // EDU.Utilities.DeleteFile(imgFile);

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
            if (Properties.SBOCRResults.Length > 0)
                SavePdfOCRData().ConfigureAwait(false).GetAwaiter().GetResult();
            UpLoadTrackingInformation().ConfigureAwait(false).GetAwaiter().GetResult();

        }
        static async Task ZipProjectFolder()
        {
            EDU.Utilities.CreateDirectory(Properties.ZipFolder);
            ZipFile.CreateFromDirectory(Path.GetDirectoryName(Properties.SettingsOCR.PdfSavedFile), Path.Combine(Properties.ZipFolder, $"{Properties.SettingsOCR.InventoryTrackingID}.zip"), CompressionLevel.Optimal, true);
            EDU.Utilities.WriteTextAppend(Path.Combine(Properties.ZipFolder, "CRG_Zip.txt"), $"{Properties.SettingsOCR.InventoryTrackingID}.zip");
            EDU.Utilities.CopyFile(Path.Combine(Properties.ZipFolder, $"{Properties.SettingsOCR.InventoryTrackingID}.zip"),Path.Combine(Properties.BackUpFolder, $"{Properties.SettingsOCR.InventoryTrackingID}.zip"),true);
           
        }
        static async Task SavePdfOCRData()
        {
            EDU.Utilities.CreateDirectory(Properties.SettingsOCR.PdfSavedFile);
            string saveFolder = Path.Combine(Path.GetDirectoryName(Properties.SettingsOCR.PdfSavedFile), $"{Properties.SettingsOCR.InventoryTrackingID}.txt");
            EDU.Utilities.WriteTextAppend(saveFolder, Properties.SBOCRResults.ToString());

        }
        static async Task UpLoadTrackingInformation()
        {
            try
            {
                Properties.TrackingModel.EdocsCustomerID = Properties.SettingsOCR.EdocsCustomerID;
                Properties.TrackingModel.FileName = Path.Combine(Properties.BackUpFolder, Path.GetFileName(Properties.SettingsOCR.PdfSavedFile));
                EDU.Utilities.CopyFile(Properties.SettingsOCR.PdfSavedFile, Properties.TrackingModel.FileName, true);

                Properties.TrackingModel.InventoryTrackingID = Properties.SettingsOCR.InventoryTrackingID;
                Properties.TrackingModel.InventoryTrackingSP = Properties.SettingsOCR.InventoryTrackingSP;
                Properties.TrackingModel.OverWriteFile = false;
                Properties.TrackingModel.ScanBatchID = Properties.SettingsOCR.ScanBatchID;
                Properties.TrackingModel.ScanMachine = Environment.MachineName;
                Properties.TrackingModel.ScanOperator = Environment.UserName;
                Properties.TrackingModel.TotalCharTyped = Properties.SettingsOCR.TotalCharTyped;
                Properties.TrackingModel.TotalPageCount = Properties.SettingsOCR.TotalPageCount;
                Properties.TrackingModel.TotalScanned = Properties.SettingsOCR.TotalScanned;
                Properties.TrackingModel.TotalType = Properties.SettingsOCR.TotalCharTyped;

                Apis.UploadEdocsITS(Properties.SettingsOCR.InventoryTrackingApiUrl, Properties.SettingsOCR.InventoryTrackingUpLoadController, Properties.TrackingModel).ConfigureAwait(false).GetAwaiter().GetResult();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }

        }
        static async Task OpenTraceLog()
        {

            string traceLog = Path.Combine(Properties.LogFile.Replace("{ApplicationDir}", EDU.Utilities.GetApplicationDir()), $"{EDU.Utilities.GetAssemblyTitle()}_MDTOCR_{DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")}.log");
            TL.TraceLogger.TraceLoggerInstance.CreateDirectory(traceLog);
            TL.TraceLogger.TraceLoggerInstance.RunningAssembley = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.exe";
            TL.TraceLogger.TraceLoggerInstance.OpenTraceLogFile(traceLog, EDU.Utilities.GetAssemblyTitle(), true);
            TL.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Opened trace log file:{traceLog}");
            TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Opened trace log file:{traceLog}");
            TL.TraceLogger.TraceLoggerInstance.TraceInformation($"AssemblyTitle:{EDU.Utilities.GetAssemblyTitle()}");
            TL.TraceLogger.TraceLoggerInstance.TraceInformation($"AssemblyCopyright:{EDU.Utilities.GetAssemblyCopyright()}");
            TL.TraceLogger.TraceLoggerInstance.TraceInformation($"AssemblyDescription:{EDU.Utilities.GetAssemblyDescription()}");
            TL.TraceLogger.TraceLoggerInstance.TraceInformation($"AssemblyVersion:{EDU.Utilities.GetAssemblyVersion()}");
        }
    }
}
