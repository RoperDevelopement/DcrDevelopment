using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EdocsUSA.Utilities;
using edl = EdocsUSA.Utilities.Logging;
namespace NYPMigration.Utilities
{
    public class PropertiesConst
    {
        public readonly string SPGetMigrationLabReqs = "[dbo].[sp_MigrationGetLabReqs]";
        public readonly string SPGetMigrationLabReqsScanDate = "sp_MigrationGetLabReqsByScanDate";
        public readonly string SPUpDateNYPLabReqsMigration = "sp_MigrationUpDateNYPLabReqsDownloaded";
        public readonly string SPMigrationUpDateNYPCovidRecsDownloaded = "sp_MigrationUpDateNYPCovidRecsDownloaded";
        
        public readonly string SPCheckSPNypMerRunning = "sp_MigrationCheckSPNypMerRunning";
        public readonly string SPGetLabReqsPatientID = "sp_MigrationGetLabReqsPatientID";
        public readonly string SPGetNYPDOHRecords = "sp_MigrationGetNYPDOHRecords";
        public readonly string SPGetSendOutPackingSlips = "sp_MigrationGetSendOutPackingSlips";
        public readonly string SPGetMaintenanceLogsLogStations = "sp_MigrationGetMaintenanceLogsLogStations";
        public readonly string SPGetMaintenanceLogs = "sp_MigrationGetMaintenanceLogs";
        public readonly string SPGetMissingPunchFormsLocations = "sp_MigrationGetMissingPunchFormsLocations";
        public readonly string SPGetSendOutResultsPerformingLabCodes = "sp_MigrationGetSendOutResultsPerformingLabCodes";
        public readonly string SPGetSendOutResults = "sp_MigrationGetSendOutResults";
        public readonly string SPGetSpecimenRejection = "sp_MigrationGetSpecimenRejection";
        public readonly string SPGetDrCodes = "sp_MigrationGetDrCodes";
        public readonly string SPGetGetDOH = "sp_MigrationGetDOH";
        public readonly string SPMigrationLabRecsRecordsProcessed = "sp_MigrationLabRecsRecordsProcessed";
        public readonly string SPMigrationGetCovidLabReqs = "sp_MigrationGetCovidLabReqs";
        public readonly string SPMigrationGetLabReqsFullTextByScanDate = "sp_MigrationGetLabReqsFullTextByScanDate";
        

        public readonly string ParmaScanDate = "@ScanDate";
        public readonly string ParmaTotalProcessed = "@TotalProcessed";
        public readonly string ParmaTableName = "@TableName";
        public readonly string ParmaTotalErrors = "@TotalErrors";
        public readonly string SqliteNypSpecimenRejectionDB = "NypSpecimenRejection.db";
        public readonly string SqliteNypSendOutResultsDB = "NypSendOutResults.db";
        public readonly string SqliteNypSendOutPackingSlipsDB = "NypSendOutPackingSlips.db";
        public readonly string SqliteNypMissingPunchForms = "NypMissingPunchForms.db";
        public readonly string SqliteNypMaintenanceLogsDB = "NypMaintenanceLogs.db";
        public readonly string SqliteNypLabReqsDB = "NypLabReqs.db";
        public readonly string SqliteNypDOHDB = "NypDOH.db";




        public readonly string CheckSPGetLabReqsRonning = "SELECT object_name(st.objectid) as ProcName FROM sys.dm_exec_connections qs CROSS APPLY sys.dm_exec_sql_text(qs.most_recent_sql_handle) st WHERE object_name(st.objectid) is not null";
        public readonly string ArgLabRecs = "labreqs";
        public readonly string ArgCovLr = "covlr";
        public readonly string ArgPatID = "patid";
        public readonly string ArgML = "ml";
        public readonly string ArgMLS = "mls";
        public readonly string ArgMPF = "mpf";
        public readonly string ArgMPL = "mpl";
        public readonly string ArgSOPS = "sops";
        public readonly string ArgSOR = "sor";
        public readonly string ArgSOPLC = "soplc";
        public readonly string ArgSR = "sr";
      //  public readonly string PatID = "patid";
        public readonly string Doh = "doh";
        
        public readonly string ML = "ml";
        public readonly string MLS = "mls";
        public readonly string MPF = "mpf";
        public readonly string MPL = "mpl";
        public readonly string SOPS = "sops";
        public readonly string SOR = "sor";
        public readonly string SOPLC = "soplc";
        public readonly string SR = "sr";
        public readonly string ArgMerg = "/merg:";
        public readonly string ArgMaxErrors = "MaxErrors";
        
        public readonly string ArgLocalSqlServer = "/locsql";
        readonly string AppConfigKeyAzureSqlServer = "AzureSqlServer";
        readonly string AppConfigKeyLocalSqlSerever = "LocalSqlSerever";
        readonly string AppConfigKeySqlLightDatabase = "SqlLightDatabase";
        readonly string AppConfigKeyEmailTo = "EmailTo";
        readonly string AppConfigKeyEmailCC = "EmailCC";
        readonly string AppConfigKeyAzureBlobAccountKey = "AzureBlobAccountKey";
        readonly string AppConfigKeyAzureBlobStorageConnectionString = "AzureBlobStorageConnectionString";
        readonly string AppConfigKeyAzureBlobAccountName = "AzureBlobAccountName";
        readonly string AppConfigKeyNYPSaveFolder = "NYPSaveFolder";
        readonly string AppConfigKeyNYPMigrationLogFolder = "NYPMigrationLogFolder";
        readonly string AppConfigKeyThreadSleepSecs = "ThreadSleepSecs";
        readonly string AppConfigKeyNumberTimesLoop = "NumberTimesLoop";
        readonly string AppConfigKeyLabReqsJasonFolder = "LabReqsJasonFolder";
        readonly string AppConfigKeyHtmlErrFile = "HtmlErrFile";
        readonly string AppConfigKeyThreadBeforeLabReqs = "ThreadBeforeLabReqs";
        readonly string AppConfigKeyThreadSleepSqlite = "ThreadSleepSqlite";
        
        readonly int MillSecondstoSeconds = 1000;

        private static PropertiesConst instance = null;
        public static PropertiesConst PropertiesConstInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PropertiesConst();
                }
                return instance;
            }
        }
        private PropertiesConst()
        { }
        public string LabReqsJasonFolder
        {
            get { return Edocs_Utilities.EdocsUtilitiesInstance.GetAppConfigSetting(AppConfigKeyLabReqsJasonFolder); }
        }
        public string HtmlErrFile
        {
            get { return Edocs_Utilities.EdocsUtilitiesInstance.GetAppConfigSetting(AppConfigKeyHtmlErrFile); }
        }
        public IList<string> MigrationErrors
        { get; set; }

        public int TotalErrors
        { get; set; } = 0;
            public int MaxErrors
        {
            get { return (int.Parse(Edocs_Utilities.EdocsUtilitiesInstance.GetAppConfigSetting(ArgMaxErrors))); }
        }
        public int TotalLabRecsProcessed
        { get; set; } = 0;
        
        public int ThreadSleepSecs
        {
            get { return (int.Parse(Edocs_Utilities.EdocsUtilitiesInstance.GetAppConfigSetting(AppConfigKeyThreadSleepSecs)) * MillSecondstoSeconds); }
        }
        public int ThreadSleepSqlite
        {
            get { return (int.Parse(Edocs_Utilities.EdocsUtilitiesInstance.GetAppConfigSetting(AppConfigKeyThreadSleepSqlite)) * MillSecondstoSeconds); }
        }

         
        public int ThreadBeforeLabReqs
        {
            get { return (int.Parse(Edocs_Utilities.EdocsUtilitiesInstance.GetAppConfigSetting(AppConfigKeyThreadBeforeLabReqs)) * MillSecondstoSeconds); }
        }
        public int NumberTimesLoop
        {
            get { return int.Parse(Edocs_Utilities.EdocsUtilitiesInstance.GetAppConfigSetting(AppConfigKeyNumberTimesLoop)); }
        }
        public string NYPMigrationLogFolder
        {
            get { return Edocs_Utilities.EdocsUtilitiesInstance.GetAppConfigSetting(AppConfigKeyNYPMigrationLogFolder); }
        }
        public string NYPSaveFolder
        {
            get { return Edocs_Utilities.EdocsUtilitiesInstance.GetAppConfigSetting(AppConfigKeyNYPSaveFolder); }
        }
        public string AzureSqlServer
        {
            get { return Edocs_Utilities.EdocsUtilitiesInstance.GetAppConfigSetting(AppConfigKeyAzureSqlServer); }
        }
        public string SQLServer
        { get; set; }
        public string LocalSqlSerever
        {
            get { return Edocs_Utilities.EdocsUtilitiesInstance.GetAppConfigSetting(AppConfigKeyLocalSqlSerever); }
        }
        public string SqlLightDatabase
        {
            get { return Edocs_Utilities.EdocsUtilitiesInstance.GetAppConfigSetting(AppConfigKeySqlLightDatabase); }
        }
        public string AzureBlobAccountKey
        {
            get { return Edocs_Utilities.EdocsUtilitiesInstance.GetAppConfigSetting(AppConfigKeyAzureBlobAccountKey); }
        }
        public string AzureBlobStorageConnectionString
        {
            get { return Edocs_Utilities.EdocsUtilitiesInstance.GetAppConfigSetting(AppConfigKeyAzureBlobStorageConnectionString); }
        }
        public string AzureBlobAccountName
        {
            get { return Edocs_Utilities.EdocsUtilitiesInstance.GetAppConfigSetting(AppConfigKeyAzureBlobAccountName); }
        }
        public string EmailTo
        {
            get { return Edocs_Utilities.EdocsUtilitiesInstance.GetAppConfigSetting(AppConfigKeyEmailTo); }
        }
        public string EmailCC
        {
            get { return Edocs_Utilities.EdocsUtilitiesInstance.GetAppConfigSetting(AppConfigKeyEmailCC); }
        }
        public void GetAzureCont(Uri uri, ref string azureContainer, ref string downloadFName)
        {
            string[] uriSeg = uri.Segments;
            azureContainer = uriSeg[1];
            downloadFName = "/";
            for (int i = 2; i < uriSeg.Count(); i++)
            {
                downloadFName = downloadFName + uriSeg[i];
            }
        }

        public string GetSaveFolder(string folderName)
        {
            int indexColon = folderName.IndexOf(":");
            return folderName.Substring(++indexColon);
        }

        public void WriteInformation(string message)
        {
            
            edl.TraceLogger.TraceLoggerInstance.TraceInformation($"{message}");
            edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"{message}");
            

        }
        public void UpdateErrors(string message)
        {
            PropertiesConstInstance.TotalErrors++;
            edl.TraceLogger.TraceLoggerInstance.TraceError($"{message}");
            edl.TraceLogger.TraceLoggerInstance.TraceErrorConsole($"{message}");
            PropertiesConst.PropertiesConstInstance.MigrationErrors.Add($"{message}\r\n");

        }
        public void WriteWarnings(string message)
        {
            
            edl.TraceLogger.TraceLoggerInstance.TraceWarning($"{message}");
            edl.TraceLogger.TraceLoggerInstance.TraceWarning($"{message}");
            //PropertiesConst.PropertiesConstInstance.MigrationErrors.Add($"{message}\r\n");

        }
    }
}
