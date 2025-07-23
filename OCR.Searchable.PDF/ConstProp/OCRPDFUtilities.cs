using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Edocs.HelperUtilities;
namespace Edocs.OCR.Searchable.PDF.ConstProp
{
    class OCRPDFUtilities
    {
        public const string MDTOCRController = "MDTOCR";
        private const string AppConfigKeyOCRApiKey = "OCRApiKey";
        private const string AppConfigKeyOCRWebApi = "OCRWebApi";
        private const string AppConfigKeyOCRISTable = "OCRISTable";
        private const string AppConfigKeyCSVFile = "CSVFile";
        private const string AppConfigKeyOCRAPIKey = "OCRAPIKey";
        internal const string MDTHeader = "ID,Message,Error";
        internal const string Easement = "easement";
        public const string ArgOCR = "/ocr:";
        public const string ArgSPDF = "/spdf:";
        public const string ArgInputFolder = "/inf:";
        public const string ArgPDFBackUp = "/pdfbf:";
        public const string ArgOutputFolder = "/sfn:";
        public const string ArgFileName = "/fn:";
        public const string ArgErrorFileName = "/efn:";
        internal static bool OCR
        { get; set; }
        internal static bool PDF
        { get; set; }
        internal static string InputFolder
        { get; set; }
        internal static string SaveFolder
        { get; set; }
        internal static string FileName
        { get; set; }
        internal static string PDFBackUpFolder
        { get; set; }
        internal static string ErrorFileName
        { get; set; }
        internal static string  OCRApiKey
            {
              get { return Edocs.HelperUtilities.Utilities.GetAppConfigSetting(AppConfigKeyOCRApiKey);
    }
    }

        internal static Uri OCRWebApi
        {
            get
            {
                return new Uri(Edocs.HelperUtilities.Utilities.GetAppConfigSetting(AppConfigKeyOCRWebApi));
            }
        }
        internal static bool OCRISTable
        {
            get
            {
                return bool.Parse(Edocs.HelperUtilities.Utilities.GetAppConfigSetting(AppConfigKeyOCRISTable));
            }
        }
        
    }
}
