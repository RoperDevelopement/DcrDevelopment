using Edocs.HelperUtilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Edocs.Ocr.SearchablePdf.Models;
using SE = Edocs.Send.Emails;
namespace Edocs.Ocr.SearchablePdf.ConstProp
{
    public class Properties
    {
        internal static StringBuilder SB
        { get; set; }
        internal static Stopwatch Stopwatch
        { get; set; }
        public static Uri WebUri
        {
            get { return new Uri(Utilities.GetAppConfigSetting(Constants.AppConfigKeyWebUri)); }
        }
        internal static string WorkingFolder
        {
            get { return Utilities.GetAppConfigSetting(Constants.AppConfigKeyWorkingFolder); }
        }
        internal static string LogFile
        {
            get { return Utilities.GetAppConfigSetting(Constants.AppConfigKeyLogFile); }
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

        internal static string ZipFolder
        {
            get { return Utilities.GetAppConfigSetting(Constants.AppConfigZipFolder); }
        }
        internal static string OCRAPIKey
        {
            get { return Utilities.GetAppConfigSetting(Constants.AppConfigKeyOCRAPIKey); }
        }
        internal static async Task SendEmail(string emailMessage, string emailAttachment, string emailSubject)
        {

            SE.Send_Emails.EmailInstance.SendEmail(string.Empty, emailMessage, emailSubject, emailAttachment, true, string.Empty);

        }
        internal static string ArchiveFolder
        {
            get { return Utilities.GetAppConfigSetting(Constants.AppConfigArchiveFolder); }
        }
        internal static string BackUpFolder
        { get; set; } = string.Empty;


        internal static OcrSettingsModel SettingsOCR
        { get; set; }
        internal static UpLoadMDTrackingModel TrackingModel
        { get; set; }
        public static T GetBatchSettingsObject<T>(string batchSettingsFile) where T : new()
        {
            
          //  JavaScriptSerializer serializer = new JavaScriptSerializer();
            object className = JsonConvert.DeserializeObject<T>(System.IO.File.ReadAllText(batchSettingsFile));
            return (T)className;
        }
    }
}
