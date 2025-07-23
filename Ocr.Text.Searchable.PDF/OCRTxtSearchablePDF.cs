using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using OCRPDF = Edocs.Ocr.Convert.Libaray;
using HU = Edocs.HelperUtilities;
using TL = EdocsUSA.Utilities.Logging;
namespace Edocs.Ocr.Text.Searchable.PDF
{
    class OCRTxtSearchablePDF
    {
        static void Main(string[] args)
        {
            System.Reflection.Assembly currentAssem = System.Reflection.Assembly.GetExecutingAssembly();
           string logFile = ConstantsProperties.LogFolder;
            TL.TraceLogger.TraceLoggerInstance.OpenTraceLogFile(logFile, "OCRPDFSearchablePdf", true);
            TL.TraceLogger.TraceLoggerInstance.RunningAssembley = currentAssem.ManifestModule.Name;
            TL.TraceLogger.TraceLoggerInstance.RunningAssembley = HU.Utilities.GetAssemblyTitle();
            TL.TraceLogger.TraceLoggerInstance.StartStopStopWatch();
            TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Running on machine {Environment.MachineName} for user {TL.TraceLogger.TraceLoggerInstance.UserName}");
            TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Assembly Title {HU.Utilities.GetAssemblyTitle()}");
            TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Assembly Version {HU.Utilities.GetAssemblyVersion()}");
            TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Assembly Description {HU.Utilities.GetAssemblyDescription()}");
            //  string results = OCRPDF.PdfImageConvert.OCRSrace(@"M:\MDTPDFFiles\Box 3 Railroad Easements 360---999\F 389 (1) C.M.ST.P.&P\Document_C.M.ST.P.&P. F 389 (1).pdf", false,false, ConstantsProperties.OCRAPIKey).ConfigureAwait(false).GetAwaiter().GetResult();
            //    string results = OCRPDF.PdfImageConvert.SearchablePdf(@"L:\stateofhawaii\IFB Documents B23005OED.pdf", ConstantsProperties.OCRAPIKey, false).ConfigureAwait(false).GetAwaiter().GetResult();
            //   string results = string.Empty;
            //foreach (var k in OCRPDF.PdfImageConvert.GetPdfImagesByFileName(@"M:\MDTPDFFiles\MDTPDFFiles\Box 4 RIGHT OF WAY Easements\DS 302 (14) U.S. Forest Service\Easement_U.S. Forest Service DS 302 (14).pdf", string.Empty))
            //{
            //    Console.WriteLine("");
                  string results = OCRPDF.PdfImageConvert.OCRSrace(@"L:\Clev\ITB_85-22\ri_33.png", true, false, ConstantsProperties.OCRAPIKey).ConfigureAwait(false).GetAwaiter().GetResult();
            //}
            //string results = string.Empty;
            //foreach (var k in OCRPDF.PdfImageConvert.GetPdfImages(@"L:\Box15-18\Box 18_v000.pdf", string.Empty))
            //{
            //    Console.WriteLine("");
             
            //   string results  = OCRPDF.PdfImageConvert.OCRSrace(@"L:\Clev\ITB_85-22.pdf", false, false, ConstantsProperties.OCRAPIKey,true).ConfigureAwait(false).GetAwaiter().GetResult();
                
          //  }
            System.IO.File.WriteAllText(@"L:\res.txt",results);
            TL.TraceLogger.TraceLoggerInstance.CloseTraceFile();
        }
    }
}
