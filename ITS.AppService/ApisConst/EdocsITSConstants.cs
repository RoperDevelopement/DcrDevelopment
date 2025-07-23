using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edocs.ITS.AppService.ApisConst
{
    public class EdocsITSConstants
    {
        #region misc
        public const string CurrentCustID = "1001";
        public const int CurrentCustIDInt = 1001;
        public const string JsonWebApi = "EdocsITSApi";
        public const string JsonInvoicesHtmlFile = "InvoicesHtmlFile";
        public const string JsonPDFViewFolder = "PDFViewFolder";
        public const string JsonTesting = "Testing";
        
        public const string DisplayPageSendAuthCode = "SendAuthCode";
        public const string SessionUserInfo = "SessionUserInfo";
        public const string CookieAuthCode = "AuthCode";
        public const string SqlErrorStartsWith = "error:";
        public const string SqlErrorUserNotFound = "error:user not found";
        public const string SqlErrorPW = "error:incorrect password";
        public const string SqlSuccess = "success";
        public const string Edocs = "e-Docs";
        public const string IsUserAuth = "IsUserAuth";
        public const string ExportUsersHeader = "User ID,Customer Name,Login Name,Email Adress,First Name,Last Name,Cell Phone Number,Last Logged in,Password Last Changed,Edocs Admin,Site Admin,Active";
        public const string ValidEmailAdd = @"^([A-Za-z0-9][^'!&\\#*$%^?<>()+=:;`~\[\]{}|/,₹€@ ][a-zA-z0-9-._][^!&\\#*$%^?<>()+=:;`~\[\]{}|/,₹€@ ]*\@[a-zA-Z0-9][^!&@\\#*$%^?<>()+=':;~`.\[\]{}|/,₹€ ]*\.[a-zA-Z]{2,6})$";

        #endregion
        #region stored procedures
        public const string SpGetEdocsITSCustomers = "sp_GetEdocsITSCustomersID";
        public const string SpGetEdocsITSCustomerInfo = "sp_GetEdocsITSCustomerInfo";
        public const string SpEdocsITSGetUsersIDs = "sp_EdocsITSGetUsersIDs";
        public const string SpUpDateLastMFLA = "sp_UpDateLastMFLA";
        public const string SpEdocsITSUserLogIn = "sp_EdocsITSUserLogIn";
        public const string SpUserLogIn = "sp_EdocsITSUserLogIn";
        public const string SpGetCustomerInformation = "sp_GetCustomerInformation";
        
        public const string SpEdocsITSAddUserLogin = "sp_EdocsITSAddUserLogin";
        public const string SpEdocsITSGetUsersInformation = "sp_EdocsITSGetUsersInformation";
        public const string SpEdocsITSGetUsersByEmailAddress = "sp_EdocsITSGetUsersByEmailAddress";
        public const string SpGetEdocsITSUser = "sp_GetEdocsITSUser";
        public const string SpEdocsITSGetUserByUserName = "sp_EdocsITSGetUserByUserName";
        public const string SpEdocsITSGetReports = "sp_EdocsITSGetReports";
        public const string SpRunReportByProjectName = "sp_RunReportByProjectName";
        public const string SpGetMaxMinDateByProjectName = " sp_GetMaxMinDateByProjectName";
        public const string SpGetCustomerInformationByCustName = "sp_GetCustomerInformationByCustName";
        public const string ReportTypeDocName = "docname";
        public const string ReportTypeTrackID = "trackid";

        #endregion
        #region controllers
        public const string AddEocsITSInvoiceNumberController = "AddEocsITSInvoiceNumber";
        
        public const string EdocsITSCustomersController = "EdocsITSCustomers";
        public const string EdocsITSGetCustomersByNameController = "EdocsITSGetCustomersByName";
        public const string TransferByTrackIDController = "EdocsITSInventoryTransferByTrackID/";
        public const string AddInvoiceController = "AddInvoice/";
        public const string EdocsITSAcceptRejectDocumentsController = "EdocsITSAcceptRejectDocuments";
        public const string UpdateInvoicesController = "UpdateInvoices";
        

        public const string UserLoginController = "EdocsITSUserLogin/";
        public const string EdocsITSInventoryTransferController = "EdocsITSInventoryTransfer/";
        public const string EdocsITSScanningManController = "EdocsITSScanningMan/";
        public const string EdocsITSUsersController = "EdocsITSUsers/";
        public const string EdocsITSReportsController = "EdocsITSReports/";
        public const string EdocsITSTrackingByProjectNameController = "EdocsITSTrackingByProjectName/";
        public const string EdocsITSScanningCoastController = "EdocsITSScanningCoast/";
        public const string EdocsITSUploadHtmlFilesController = "EdocsITSUploadHtmlFiles";
        public const string EdocsITSEdocsITSCustomInvoiceController = "EdocsITSCustomInvoice/";
        public const string EdocsITSPSUSDRecordsController = "PSUSDRecords/";
        public const string EdocsITPSUSDSearchTextController = "PSUSDSearchText/";
        public const string EdocsITPSUSDFLNameController = "PSUSDFLName/";
        

        public const string SPUpdateAccRejectDocs = "[dbo].[sp_UpAcceptRejDocs]";
        public const string SpGetPSUSDRecordSearch = "sp_GetPSUSDRecordSearch";
        #endregion
        #region session keys
        public const string SessionKeyEdocsITSUserNames = "EdocsITSUserNames";
        #endregion
        #region regulart const
        public const string Quoat = "\"";
      
        #endregion
    }
}

