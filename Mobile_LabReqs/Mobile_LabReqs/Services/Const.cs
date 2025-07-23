using System;
using System.Collections.Generic;
using System.Text;

namespace Mobile_LabReqs.Services
{
   public class Const
    {
        public const string WebUrl= "https://edocsnypwebapi.azurewebsites.net/api/";
        #region webapi

        public const string ApiNypLabReqsController = @"NypLabReqs";
        public const string ApiSpecimenRejectionController = @"SpecimenRejection/";
        // public const string ApiNypLabReqsControllerParam = "lreqMod=test";
        public const string ApiNypLabReqsControllerParam = "labResp={0}";
        public const string ApiSpecimenRejectionControllerParam = @"storedProcedure={0}";
        public const string ApiNypDrCodesController = @"NypDrCodes/";
        public const string ApiNypGrantReceiptsController = @"NypGrantReceipts/";
        public const string ApiNypDOHController = @"NypDOH/";
        public const string ApiNypAuditLogs = @"AuditLogs/";
        public const string ApiNypMaintenanceLogsController = @"NypMaintenanceLogs/";
        public const string ApiSendOutPackingSlipsController = @"SendOutPackingSlips/";
        public const string ApiNypSendOutResultsController = @"NypSendOutResults/";
        public const string ApiNypEmployeeComplianceController = @"NypEmployeeCompliance/";
        public const string ApiNypInvoiceController = @"NypInvoice/";
        public const string ApiNypLabReqsUsersController = @"NypLabReqsUsers/";
        //   public const string ApiNypLabReqsUsersParam = "cwid={0}&command={1}";
        public const string ApiNypLabReqsUsersParam = "{0}/{1}/{2}/{3}/{4}";
        public const string NANoSlash = "NA";
        public const string RepStrCwid = "{CWID}";
        public const string RepStrEmailAddress = "{EmailAdddressNewUser}";
        public const string SessionIsAdminVeiwAuditLogs = "IsAdminVeiwAuditLogs";
        public const string ApiNypLabReqsKeyWords = @"NypLabReqsKeyWords/";
       


        #endregion
        #region nyp lab reqs
        public const string SpNypLabIndexFinancialNumber = "sp_NypLabIndexFinancialNumber";
        public const string SpNypLabRequestionNumber = "sp_NypLabRequestionNumber";
        public const string SpNypLabReqsPatientId = "sp_NypLabReqsPatientId";
        public const string SpNypLabReqsClientCode = "sp_NypLabReqsClientCode";
        public const string SpNypLabReqsDrCode = "sp_NypLabReqsDrCode";
        public const string SpNypLabReqsPatientFirstName = "sp_NypLabReqsPatientFirstName";
        public const string SpNypLabReqsPatientLastName = "sp_NypLabReqsPatientLastName";
        public const string SpNypLabReqsDrName = "sp_NypLabReqsDrName";
        public const string SpNypLabReqsScanOperator = "sp_NypLabReqsScanOperator";
        public const string SpNypLabReqsScanBatch = "sp_NypLabReqsScanBatch";
        public const string SpNypLabReqsDosRecScanDate = "sp_NypLabReqsDosRecScanDate";
        public const string SpNypLabMRNRecDate = "sp_NypLabMRNRecDate";
        public const string SearchByLogDate = "searchByLogDate";
        public const string SearchByScanDate = "scanDate";
        #endregion
        #region storedprocedure
        public const string LabReqsSP = "labResp=sp_NypLabIndexFinancialNumber";
        public const string MRNPatientIdSP = "labResp=sp_NypLabReqsPatientId";
        public const string KeyWordsSP = "KeyWords";
        #endregion
        public static DateTime SearchStartDate
        { get; set; }
        public static DateTime SearchEndDate
        { get; set; }
        public static string SearchStr
        { get; set; }

        public static bool SearchPart
        { get; set; }

        public static string SearchByDateofServiceScanDate
        { get; set; }

        public static DateTime ParaseDateTime(string dateTime)
        {
            if (DateTime.TryParse(dateTime, out DateTime results))
                return results;
            return DateTime.Now;
        }
        public static string StoreProcuder
        { get; set; }
       
    }
}
