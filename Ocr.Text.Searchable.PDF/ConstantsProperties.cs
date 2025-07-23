using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HU = Edocs.HelperUtilities;
using TL = EdocsUSA.Utilities.Logging;
namespace Edocs.Ocr.Text.Searchable.PDF
{
    class ConstantsProperties
    {
        public const string AppKeyWorkingFolder = "WorkingFolder";
        public const char Quota = (char)34;
        public const string RepStrApplicationDir = "{ApplicationDir}";
        public const string AppKeyLogFile = "LogFile";
        public const string AppKeyOCRAPIKey = "OCRAPIKey";
        public const string AppKeyOCRAPIFreeKey ="OCRAPIFreeKey";
    public const string AppKeyOCRAPIURL = "OCRAPIURL";
        public const string AppKeyOCRAPIURLFreeKey = "OCRAPIURLFreeKey";
        public static string OCRAPIKey
        {
            get { return HU.Utilities.GetAppConfigSetting(AppKeyOCRAPIKey); }
        }
        public static string OCRAPIFreeKey
        {
            get { return HU.Utilities.GetAppConfigSetting(AppKeyOCRAPIFreeKey); }
        }
        public static string WorkingFolder
        { get { return HU.Utilities.GetAppConfigSetting(AppKeyWorkingFolder); } }
        public static string LogFile
        {
            get
            {
                return HU.Utilities.GetAppConfigSetting(AppKeyLogFile).Trim();
            }
        }
        public static string LogFolder
        {
            get
            {

                string folderLog;
                folderLog = LogFile; //HU.Utilities.GetAppConfigSetting(AppKeyLogFile);
                folderLog = HU.Utilities.ReplaceString(folderLog, RepStrApplicationDir, HU.Utilities.GetApplicationDir());
                folderLog = $"{HU.Utilities.CheckFolderPath(folderLog)}OCRSearchablePDF_{DateTime.Now.ToString("MM_dd_yyyy_HH_mm_ss")}.log";
                return folderLog;
            }
        }

    }
}
