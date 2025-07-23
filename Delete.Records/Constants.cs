using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edocs.Delete.Records
{
    public class Constants
    {
        public static readonly string AppKeyLabReqsDataBaseName = "LabReqsDataBaseName";
        public static readonly string AppKeyAzureBlobStorageConnectionString = "AzureBlobStorageConnectionString";
        public static readonly string AppKeyAzureBlobAccountName = "AzureBlobAccountName";
        public static readonly string AppKeyAzureBlobAccountKey = "AzureBlobAccountKey";
        public static readonly string AppKeySqlServerTimeOut = "SqlServerTimeOut";
        public static readonly string AppKeyAuditLogsFolder = "AuditLogsFolder";
        public static readonly string AppConfigKeyRepStrApplicationDir = "{ApplicationDir}";

        public static readonly string AppKeyAzureBlobContanierAuditShare = "AzureBlobContanierAuditShare";
        public static readonly string AppKeySqlServerName = "SqlServerName";
        public static readonly string AppKeySqlServerUserPw = "SqlServerUserPw";
        public static readonly string AppKeySqlServerUserName = "SqlServerUserName";
        public static readonly string AppKeyEmailTo = "EmailTo";
        public static readonly string AppKeyEmailCC = "EmailCC";
        public static readonly string AppKeyEmailSubject = "EmailSubject";
        public static readonly string AppKeyAzureNotDeleteFile = "AzureNotDeleteFile";


        public static readonly string XmlElementDataBaseName = "DataBaseName";
        public static readonly string XmlElementTableName = "TableName";
        public static readonly string XmlElementNumberYrsKeep = "NumberYrsKeep";
        public static readonly string XmlElementNumberDaysKeep = "NumberDaysKeep";
        public static readonly string XmlElementNumberMonthsKeep = "NumberMonthsKeep";
        public static readonly string XmlElementDeleteStoredProcedure = "DeleteStoredProcedure";
        public static readonly string XmlElementGetLabRecsToDeleteStoredProcedure = "GetLabRecsStoredProcedure";

        public static readonly string LabReqsSpParmaDataBaseName = "@DataBaseName";
        public static readonly string SpParmaDateToDelete = "@DateToDelete";
        public static readonly string LabReqsParmaID = "@ID";
        public static readonly string LabReqsRetParaID = "ID";
        public static readonly string LabReqsRetParaFileURl = "FileURl";
        public static readonly string LabReqsRetParaScanBatch = "ScanBatch";
        public static readonly string LabReqsRetParaScanDate = "ScanDate";
        public static readonly string Quote = "\"";
        public static readonly string ArgXmlFile = "/xml:";
        public static readonly string ArgDataBase = "/db:";
        public static readonly string AllDB = "all";
        public static readonly string LabRecDB = "labrecs";
        public static readonly string HL7DB = "hl7";
        public static readonly string BMDB = "bm";
        public static readonly string ALDB = "alogs";
        public static readonly string ArgUsage = "/?";
        public static readonly string RepStrApplicationDir = "{ApplicationDir}";
        





    }
}

