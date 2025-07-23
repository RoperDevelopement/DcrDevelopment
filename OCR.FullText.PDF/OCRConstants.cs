using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edocs.OCR.FullText.PDF
{
    class OCRConstants
    {
        public const string AppKeyAuditLogsFolder = "AuditLogsFolder";
        public const string AppKeyRegxSkip = "RegxSkip";
        public const string AppKeyAzureImageContanier = "AzureImageContanier";
        public const string AppKeyRunProcess = "RunProcess";
        public const string AppKeyProcessWorkingFolder = "ProcessWorkingFolder";
        public const string AppKeySendTextOnError = "SendTextOnError";
        
        public const string AppKeyProcessArgs = "ProcessArgs";
        public const string RepStrDaystoProcess = "{DaystoProcess}";
        public const string AppKeyAzureBlobStorageConnectionString = "AzureBlobStorageConnectionString";
        public const string AppKeyAzureBlobContanierAuditShare = "AzureBlobContanierAuditShare";
        public const string AppKeyAzureBlobAccountName = "AzureBlobAccountName";
        public const string AppKeyAzureBlobAccountKey = "AzureBlobAccountKey";
        public const string AppKeyWebUri = "WebUri";
        public const string LabReqsPDFFullTextController = "LabReqsPDFFullText/";
        public const string AppKeyWorkingFolder = "WorkingFolder";
        public const string AppKeyRequisitionNumberRegx = "RequisitionNumberRegx";
        public const string AppKeyOCRAPIKey = "OCRAPIKey";
        public const string AppKeyOCRAPIFreeKey = "OCRAPIFreeKey";
        public const string AppKeyIndexNumbersChanged = "IndexNumbersChanged";
        public const string AppKeyRunDate = "RunDate";
        public const string AppKeyLogFile = "LogFile";
        public  const string RepStrApplicationDir = "{ApplicationDir}";
        public const string AppKeyHtmlFile = "HtmlFile";
        public const char Quota = (char)34;
        public const string AppKeyHTMLtemplate = "HTMLtemplate";
        public const string AppKeyFinIndexNumberRegx = "FinIndexNumberRegx";
        public const string StartTableRow = "<tr>";
        public const string EndTableRow = "</tr>";
        public const string StartTableData = "<td>";
        public const string EndTableDate = "</td>";
        public const string AppKeyImageDpi = "ImageDpi";
        public const string AppKeyCleanWF = "CleanWF";
        public const string AppKeyMaxOcrErrors = "MaxOcrErrors";
        public const string SpNypLabReqsGetPDFSToOCR = "sp_NypLabReqsGetPDFSToOCR";
        public const string SpParmaScanStartDate = "@ScanStartDate";
        public const string SpParmaScanDate = "@ScanDate";
        public const string SpParmaScanEndDate = "@ScanEndDate";
        public const string AppKeySqlConnectionStr = "SqlConnectionStr";
        public const string SpParmaPDFFullText = "@PDFFullText";
        public const string SpParmaLabReqID = "@LabReqID";
        public const string SpUploadLabReqsFullText = "sp_UploadLabReqsFullText";
        public const string PDFWorkingFolder = "PDFWorkingFolder";







    }
}
