using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OCR = Edocs.Ocr.Convert.Libaray;
using EDU = Edocs.HelperUtilities;
using TL = EdocsUSA.Utilities.Logging;
using System.IO;
using System.Net;

namespace Edocs.OCR.Searchable.PDF
{
    class OCRPDF
    {
        static void Main(string[] args)
        {
            GetInputArgs(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private static async Task GetInputArgs(string[] args)
        {
            try
            {
                //   ConstProp.OCRPDFUtilities.FileName = args[0];
                foreach (string inputArgs in args)
                {
                    // break;
                    if (inputArgs.StartsWith(ConstProp.OCRPDFUtilities.ArgOCR, StringComparison.OrdinalIgnoreCase))
                    {
                        ConstProp.OCRPDFUtilities.OCR = true;
                    }

                    else if (inputArgs.StartsWith(ConstProp.OCRPDFUtilities.ArgSPDF, StringComparison.OrdinalIgnoreCase))
                    {
                        ConstProp.OCRPDFUtilities.PDF = true;
                    }
                    else if (inputArgs.StartsWith(ConstProp.OCRPDFUtilities.ArgOutputFolder, StringComparison.OrdinalIgnoreCase))
                    {
                        ConstProp.OCRPDFUtilities.SaveFolder = inputArgs.Substring(ConstProp.OCRPDFUtilities.ArgOutputFolder.Length);
                    }
                    else if (inputArgs.StartsWith(ConstProp.OCRPDFUtilities.ArgInputFolder, StringComparison.OrdinalIgnoreCase))
                    {
                        ConstProp.OCRPDFUtilities.InputFolder = inputArgs.Substring(ConstProp.OCRPDFUtilities.ArgInputFolder.Length);
                    }
                    else if (inputArgs.StartsWith(ConstProp.OCRPDFUtilities.ArgFileName, StringComparison.OrdinalIgnoreCase))
                    {
                        ConstProp.OCRPDFUtilities.FileName = inputArgs.Substring(ConstProp.OCRPDFUtilities.ArgFileName.Length);
                    }
                    else if (inputArgs.StartsWith(ConstProp.OCRPDFUtilities.ArgPDFBackUp, StringComparison.OrdinalIgnoreCase))
                    {
                        ConstProp.OCRPDFUtilities.PDFBackUpFolder = inputArgs.Substring(ConstProp.OCRPDFUtilities.ArgPDFBackUp.Length);
                    }
                    else if (inputArgs.StartsWith(ConstProp.OCRPDFUtilities.ArgErrorFileName, StringComparison.OrdinalIgnoreCase))
                    {
                        ConstProp.OCRPDFUtilities.ErrorFileName = inputArgs.Substring(ConstProp.OCRPDFUtilities.ArgErrorFileName.Length);
                    }
                    
                    else
                        throw new Exception($"Invalid arg {inputArgs}");
                }
                if (!(string.IsNullOrWhiteSpace(ConstProp.OCRPDFUtilities.PDFBackUpFolder)))
                    EDU.Utilities.CreateDirectory(ConstProp.OCRPDFUtilities.PDFBackUpFolder);
                if (!string.IsNullOrWhiteSpace(ConstProp.OCRPDFUtilities.ErrorFileName))
                {
                    EDU.Utilities.CreateDirectory(ConstProp.OCRPDFUtilities.ErrorFileName);
                }
                EDU.Utilities.CreateDirectory(ConstProp.OCRPDFUtilities.SaveFolder);

                if (ConstProp.OCRPDFUtilities.PDF)
                    CreateSearchablePDF().ConfigureAwait(true).GetAwaiter().GetResult();
                else
                    GetOCRTxt().ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Environment.Exit(1);
            }
        }
        private static async Task GetOCRTxt()
        {
            int total = 0;
            foreach (string files in Edocs.HelperUtilities.Utilities.GetDirectoryFiles(ConstProp.OCRPDFUtilities.InputFolder, ConstProp.OCRPDFUtilities.FileName, SearchOption.TopDirectoryOnly).ConfigureAwait(true).GetAwaiter().GetResult())
            {
                TL.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Processing file {files}");
                TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Processing file {files}");
                total++;
                string ocrText = Edocs.Ocr.Convert.Libaray.PdfImageConvert.OCRSrace(files, null, true, false, ConstProp.OCRPDFUtilities.OCRApiKey, "2", ConstProp.OCRPDFUtilities.OCRISTable, "image.png").ConfigureAwait(true).GetAwaiter().GetResult();
                if(!(string.IsNullOrWhiteSpace(ocrText)))
                {
                    TL.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Writing txt for file {files}");
                    TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Writing txt for {files}");
                    File.AppendAllText(ConstProp.OCRPDFUtilities.SaveFolder, $"{ocrText}\r\n");
                }
                
            }
            if (!string.IsNullOrWhiteSpace(ConstProp.OCRPDFUtilities.ErrorFileName))
            {
                File.WriteAllText(ConstProp.OCRPDFUtilities.ErrorFileName, total.ToString());
            }
        }
        private static async Task CreateSearchablePDF()
        {
            int total = 0;
            foreach (string files in Edocs.HelperUtilities.Utilities.GetDirectoryFiles(ConstProp.OCRPDFUtilities.InputFolder, ConstProp.OCRPDFUtilities.FileName, SearchOption.TopDirectoryOnly).ConfigureAwait(true).GetAwaiter().GetResult())
            {
                string sPDF = Edocs.Ocr.Convert.Libaray.PdfImageConvert.SearchablePdf(files,ConstProp.OCRPDFUtilities.OCRApiKey,false).ConfigureAwait(true).GetAwaiter().GetResult();


                if (!(string.IsNullOrWhiteSpace(sPDF)))
                {
                    string saveFileName = Path.Combine(ConstProp.OCRPDFUtilities.SaveFolder, Path.GetFileName(files));
                    if (!(string.IsNullOrWhiteSpace(ConstProp.OCRPDFUtilities.PDFBackUpFolder)))
                    {
                        string destFname = Path.Combine(ConstProp.OCRPDFUtilities.PDFBackUpFolder, Path.GetFileName(files));
                        HelperUtilities.Utilities.CopyFile(files, destFname, true);
                    }
                    using (WebClient client = new WebClient())
                    {
                        byte[] pdfByte = client.DownloadDataTaskAsync(new Uri(sPDF)).ConfigureAwait(false).GetAwaiter().GetResult();

                        File.WriteAllBytes(saveFileName, pdfByte);

                    }
                    total++;

                }
            }
            if(!string.IsNullOrWhiteSpace(ConstProp.OCRPDFUtilities.ErrorFileName))
            {
                File.WriteAllText(ConstProp.OCRPDFUtilities.ErrorFileName, total.ToString());
            }



        }
        static async Task OpenTraceLog()
        {

            string traceLog = Path.Combine(EDU.Utilities.GetApplicationDir(), $"{EDU.Utilities.GetAssemblyTitle()}_OCRPDF_{DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")}.log");
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
