using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EdocsUSA.Utilities;
using System.Web.Script.Serialization;
using System.IO;
namespace Edocs.DOH.Upload.Report
{
   public class Utilities_Const
    {
        public const string SpDOHUploadedDocsReport = "[dbo].[sp_DOHUploadedDocsReport]";
        public const string SpDOHGetJsonFileID = "[dbo].[sp_DOHGetJsonFileID]";
        public const string SpGetTotalBooksScanned = "[dbo].[sp_GetTotalBooksScanned]";
        public const string SpJsonFilesNotFound = "[dbo].[sp_JsonFilesNotFound]";
        
        public const string SqlParmaID = "@ID";
        public const string SpUpDateJsonCompare = "[dbo].[sp_UpDateJsonCompare]";
        public const string SpDOHCompareJsonFiles = "[dbo].[sp_DOHCompareJsonFiles]";
        public const string SqlParmaCity = "@City";
        public const string SqlParmaChurch = "@Church";
        public const string SqlParmaBookType = "@BookType";
        public const string SqlParmaStartDate = "@StartDate";
        public const string SqlParmaEndDate = "@EndDate";
        public readonly string ArgsRunDate = "/dldate:";
        public readonly string ArgsDateAdd = "/dateadd:";
        public readonly string ArgsReportFolder = "/rfolder:";
        public readonly string ArgsReportName = "/rname:";
        public readonly string ArgsEmailReport = "/ereport:";
        public readonly string ArgsLocalSqlServer = "/sqllocal:";
        private readonly string AppConfigKeyAzureSql = "AzureSqlServer";
        private readonly string AppConfigKeyEmailTo = "EmailTo";
        private readonly string AppConfigKeyEmailCC = "EmailCC";
        
        public readonly string RecJsonFileExt = "_records.json";
        public readonly string SettingsJsonFileExt = "_settings.json";
        private readonly string AppConfigKeyLocalSql = "LocalSqlSerever";
        private readonly string RepServername = "{servername}";
        public readonly string ArgsRunJsonCompare = "/jc:";
        public readonly string ArgsJsonFolder = "/jf:";
        public readonly string ArgsJsonSaveFolder = "/jsf:";
        public readonly string ArgsJsonSaveFileName = "/jsfn:";
        public readonly string ArgsJsonProcessFolder = "/jspfn:";
        public readonly string ArgsRunUpLoadReport = "/ru:";

        private static Utilities_Const instance = null;

        public string EmailTo
        { get { return Edocs_Utilities.EdocsUtilitiesInstance.GetAppConfigSetting(AppConfigKeyEmailTo); } }
        public StringBuilder SbErrors
        { get; set; }
        public int DateAdd
        { get; set; }
        public DateTime ReportDate
        { get; set; } = DateTime.Now;
        public string ReportFolder
        { get; set; } = "D:\\DohDownload_Report";
        public string ReportName
        { get; set; } = "DOHDownLoadReport.html";
        public bool EmailReport
        { get; set; } = true;
        public bool AzureSqlServer
        { get; set; } = true;
        public bool RunJsonCompare
        { get; set; } = true;
        public bool RunUploadReport
        { get; set; } = true;
        public string SqlServerConnStr
        { get; set; }  
        public string AzureSqlServerConnStr
        { get { return Edocs_Utilities.EdocsUtilitiesInstance.GetAppConfigSetting(AppConfigKeyAzureSql); } }
        public string AzureLocalSqlServerConnStr
        { get { return Edocs_Utilities.EdocsUtilitiesInstance.GetAppConfigSetting(AppConfigKeyLocalSql); } }
        public string EmailCC
        { get { return Edocs_Utilities.EdocsUtilitiesInstance.GetAppConfigSetting(AppConfigKeyEmailCC); } }

        
        public string JsonFolder
        { get; set; } = @"D:\Archives\JsonFilesCurrent";
        public string JsonProcessFolder
        { get; set; } =@"D:\Archives\JsonProcessed";
        public string JsonHtmlFileName
        { get; set; } = @"JasonCompareFile.html";
        private Utilities_Const() { }

        public static  Utilities_Const UtilityConstInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Utilities_Const();
                }
                return instance;
            }
        }

        public async Task<string> GetServerConnection(bool sqlServer)
        {
            //string sqlConnStr = string.Empty;
            if (sqlServer)
                return AzureSqlServerConnStr;
            return AzureLocalSqlServerConnStr.Replace(RepServername, Environment.MachineName);
            
        }
        public async Task<List<object>> GetRecords<T>(string batchRecordsFile)
        {
            
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<object> records = new List<object>();
            foreach (string line in File.ReadAllLines(batchRecordsFile))
            {
                 
                if (string.IsNullOrWhiteSpace(line))
                { continue; }
                 
                object className = serializer.Deserialize<T>(line);
                records.Add(className);
                break;
                //        records.Add((Dictionary<string, object>)(serializer.DeserializeObject(line)));
            }
            return records;
        }
        public  async Task<T> GetBatchSettingsObject<T>(string batchSettingsFile) where T : new()
        {
             
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            object className = serializer.Deserialize<T>(File.ReadAllText(batchSettingsFile));
            return (T)className;
        }

    }
}
