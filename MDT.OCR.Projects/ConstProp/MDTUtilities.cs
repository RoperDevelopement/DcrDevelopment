using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Edocs.HelperUtilities;
using SE = Edocs.Send.Emails;
namespace MDT.OCT.Projects.ConstProp
{
    internal class MDTUtilities
    {
        public const  string MDTOCRController = "MDTOCR";
        private const string AppConfigKeyWebUri = "WebUri";
        private const string AppConfigKeyWorkingFolder = "WorkingFolder";
        private const string AppConfigKeyLogFile = "LogFile";
        private const string AppConfigKeyCSVFile = "CSVFile";
        private const string AppConfigKeyOCRAPIKey = "OCRAPIKey";
        internal const string MDTHeader = "ID,Message,Error";
        internal const string Easement = "easement";
        internal static StringBuilder SB
        { get; set; }
        internal static Stopwatch Stopwatch
        { get; set; }
        public static Uri WebUri
        {
            get { return new Uri(Utilities.GetAppConfigSetting(AppConfigKeyWebUri)); }
        }
        internal static string WorkingFolder
        {
            get { return Utilities.GetAppConfigSetting(AppConfigKeyWorkingFolder); }
        }
        internal static string LogFile
        {
            get { return Utilities.GetAppConfigSetting(AppConfigKeyLogFile); }
        }
        internal static int TotalFilesProcessed
        { get; set; } = 0;
        internal static int TotalDocsToProcess
        { get; set; } = 0;
        internal static int TotalDocsNotFound
        { get; set; } = 0;
        internal static int TotalDocsOCR
        { get; set; } = 0;
        internal static int TotalErrors
        { get; set; } = 0;
        internal static int TotalSkipped
        { get; set; } = 0;
        internal static int DocPages
        { get; set; }
        internal static StringBuilder SBOCRResults
        { get; set; }
        internal static string CSVFile
        {
            get { return Utilities.GetAppConfigSetting(AppConfigKeyCSVFile); }
        }
        internal static string OCRAPIKey
        {
            get { return Utilities.GetAppConfigSetting(AppConfigKeyOCRAPIKey); }
        }
        internal static string WorkingPdfFolder
        {
             get;set;
        }

        internal static async Task SendEmail(string emailMessage, string emailAttachment,string emailSubject)
        {
            
            SE.Send_Emails.EmailInstance.SendEmail(string.Empty,emailMessage,emailSubject, emailAttachment, true, string.Empty);
            
        }
    }
}