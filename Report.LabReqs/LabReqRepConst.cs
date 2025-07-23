using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Edocs.HelperUtilities;
namespace Edocs.Report.LabReqs
{
    class LabReqRepConst
    {
        public static readonly string SpGenerateLabReqRep = "sp_GenerateLabReqRep";
        public static readonly string SpParamScanStartDate = "@ScanStartDate";
        public static readonly string SpParmScanEndDate = "@ScanEndDate";
        public static readonly string AppKeySqlServerName = "SqlServerName";
        public static readonly string AppKeySqlServerUserName = "SqlServerUserName";
        public static readonly string AppKeySqlServerUserPw = "SqlServerUserPw";
        public static readonly string AppKeySqlDBName = "SqlDBName";
        public static readonly string AppKeyEmailTo = "EmailTo";
        public static readonly string AppKeyEmailCC = "EmailCC";
        public static readonly string AppKeyEmailSubject = "EmailSubject";
        public static readonly string AppKeyWorkingDir = "WorkingDir";
        public static readonly string RepStrWF = "{ApplictionDir}";
        public static readonly string ArgsDateAdd = "/dateadd:";
        public static readonly string ArgsScanStDate = "/ssd:";
        public static readonly string ArgsScanEndDate = "/sed:";
        public static readonly int LabReqsNumYears = -7;
        public static readonly string Quote = "\"";
        public static readonly string LabReqsRepName = "LabReqsRep";
        public static readonly string RepStrError = "{Error}";
        public static readonly string RepStrReportTime = "{reportTime}";


        public static readonly string FieldBatchID = "BatchID";
        public static readonly string FieldIndexNumber = "IndexNumber";
        public static readonly string FieldFinNumber = "FinNumber";
        public static readonly string FieldDateOfServices = "DateOfServices";
        public static readonly string FieldPatID = "PatID";
        public static readonly string FieldReqNum = "ReqNum";
        public static readonly string FieldClientCode = "ClientCode";
        public static readonly string FieldScanDate = "ScanDate";
        public static readonly string FieldMerged = "Merged";
        public static readonly string FieldMRN = "MRN";

        public static readonly string HeaderIndexNumber = "Index Number";
        public static readonly string HeaderFinanical = "Finanical Number";
        public static readonly string HeaderRequisition = "Requisition Number";
        public static readonly string HeaderPatientID = "Patient ID";
        public static readonly string HeaderMRN = "MRN";
        public static readonly string HeaderClientCode = "Client Code";
        public static readonly string HeaderBatchID = "BatchID";
        public static readonly string HeaderScanDate = "Scan Date";
        public static readonly string HeaderDateOfServices = "Date Of Services";
        public static readonly string HeaderMerged = "Merged";


       
        public static string WorkingFolder
        {
            get { return Utilities.GetAppConfigSetting(AppKeyWorkingDir).Replace(RepStrWF, Utilities.GetApplicationDir()); }


        }
        public static string EmailTo
        {
            get { return Utilities.GetAppConfigSetting(AppKeyEmailTo); }
        }

        public static string EmailCC
        {
            get { return Utilities.GetAppConfigSetting(AppKeyEmailCC); }
        }
        public static string EmailSubject
        {
            get { return Utilities.GetAppConfigSetting(AppKeyEmailSubject); }
        }

    }
}
