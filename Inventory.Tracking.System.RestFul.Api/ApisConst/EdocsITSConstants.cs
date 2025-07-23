using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edocs.Inventory.Tracking.System.RestFul.Api.ApisConst
{
    public class EdocsITSConstants
    {
        #region  sql parms
        public const string SpParamJsonFile = "JasonFile";
        public const string DoubleQuotes = "\"";
        public const string SpParmaLoginName = "@LoginName";
        public const string SpParmaUserPassWord = "@UserPassWord";
        public const string SpParmaNumDaysNextMFLA = "@NumDaysNextMFLA";
        public const string SpParmaTrackingID = "@TrackingID";
        public const string SpParmaCustomerID = "@CustomerID";
        public const string SpParmaUserFirstName = "@UserFirstName";
        public const string SpParmaUserLastName = "@UserLastName";
        public const string SpParmaUserEmail = "@UserEmail";
        public const string SpParmaCustomerAdmin = "@CustomerAdmin";
        public const string SpParmaEdocsAdmin = "@EdocsAdmin";
        public const string SpParmaCellPhoneNumber = "@CellPhoneNumber";
        public const string SpParmaEdocsCustomerName = "@EdocsCustomerName";
        public const string SpParmaScanType = "@ScanType";
        public const string SpParmaDeliveryMethod = "@DeliveryMethod";
        public const string SpParmaDateSent = "@DateSent";
        public const string SpParmaNumberDocsSent = "@NumberDocsSent";
        public const string SpParmaNumberDocsRecived = "@NumberDocsRecived";
        public const string SpParmaDateDocumentsReceived = "@DateDocumentsReceived";
        public const string SpNumberTypedPerDoc = "@NumberTypedPerDoc";
        public const string SpNumberDocOCR = "@NumberDocOCR";

        public const string SpParmaDateScanningStarted = "@DateScanningStarted";
        public const string SpParmaUserID = "@UserID";
        public const string SpParmaActive = "@Active";
        public const string SpParmaUserNewPassWord = "@UserNewPassWord";
        public const string SpParmaNumberDocsScanned = "@NumberDocsScanned";
        public const string SpParmaNumberDocsUploaded = "@NumberDocsUploaded";
        public const string SpParmaNumberDocsOCR = "@NumberDocsOCR";
        public const string SpParmaRepType = "@RepType";
        public const string SpParmaRepStDate = "@RepStDate";
        public const string SpParmaRepEndDate = "@RepEndDate";
        public const string SpParmaBatchID = "@BatchID";
        public const string SpParmaID = "@ID";
        public const string SpPaid = "@Paid";
        public const string SpIDTracking = "@IDTracking";
        
        public const string SpParmaJasonFile = "@JasonFile"; 
        public const string SpParmaScanMachine = "@ScanMachine";
        public const string SpParmaScanOperator = "@ScanOperator";
        public const string SpParmaFileName = "@FileName";
        public const string SpParmaStandardLargeDoc = "@StandardLargeDocument";
        public const string SpParmaDateOFService = "@DateOFService";
        public const string SpParmaSearch = "@Search";
        
        public const string SpParmaInvoiceStartDate = "@InvoiceStartDate";
        public const string SpParmaInvoiceEndDate = "@InvoiceEndDate";
        public const string SpParmaInvoiceTotalAmount = "@InvoiceTotalAmount";
        public const string SpParmaAcceptRject = "@AcceptRject";
        public const string SpParmaInvoiceNumber = "@InvoiceNumber";
        public const string SpParmaComments = "@Comments";
        public const string SpParmaHtmlData = "@HtmlData";
        public const string SpAmountPaid = "@AmountPaid";
        public const string SpTotalAmountPaid = "@TotalAmountPaid";
        public const string SpParmaItemDescription = "@ItemDescription";
        public const string SpParmaItemCost = "@ItemCost";
        public const string SpParmaFirstName = "@FirstName";
        public const string SpParmaLastName = "@LastName";
        public const string SpParmaDep = "@Dep";
        public const string SpParmaOrgDep= "@OrgDep";


        public const string TableColoumTotalPaid = "TPaid";
        public const string TableColoumAmountPaid = "APaid";
        public const string TableColoumTotalInvoiceAmount = "InvAmt";
        public const string TablePaid = "Paid";
        public const string ReportTypeDocName = "docname";
        public const string ReportTypeTrackID = "trackid";
        public const string AllTrackIDDocName = "all";



        #endregion
        #region stored proc
        
        public const string SpAddEocsITSInvoiceNumber = "[dbo].[sp_AddEocsITSInvoiceNumber]";
        public const string SpAddUpdateEdocsCustomers = "[dbo].[sp_AddUpdateEdocsCustomers]";
        public const string SpGetInvoiceBalance = "sp_GetInvoiceBalance";
        public const string SpGetCustomerInformationByCustName = "sp_GetCustomerInformationByCustName";
        public const string SpUpDateITSTrackingIDByTrackingID = "sp_UpDateITSTrackingIDByTrackingID";
        public const string SpRunReportByTrackingID = "RunReportByTrackingID";
        public const string SpGetDocumentNames = "sp_GetDocumentNames";
        public const string SpRunReportByTrackID_DocName = "sp_RunReportByTrackID_DocName";
        public const string SpGetTrackingIDsDocNameByCustomerID = "sp_GetTrackingIDsDocNameByCustomerID";
        public const string SpUploadPSUSDRecors = "sp_UploadPSUSDRecors";
        public const string SpUploadPSUSDFullText = "sp_UploadPSUSDFullText";
        public const string SpGetPSUSDRecordSearch = "sp_GetPSUSDRecordSearch";
        
        public const string SpAddAddInvoice = "[dbo].[sp_AddInvoice]";
        public const string SpGetDocumentsForReview = "[dbo].[sp_GetDocumentsForReview]";
        public const string SpGetTrackingIDsByCustomerID = "sp_GetTrackingIDsByCustomerID";
        public const string SpGetDocumentsForReviewByTrackingIDDocName = "sp_GetDocumentsForReviewByTrackingIDDocName";
        public const string SpRunReportInvoiceByTrackindIDPrice = "sp_RunReportInvoiceByTrackindIDPrice";
        
        public const string SpAddInventoryTransfer = "[dbo].[sp_AddInventoryTransfer]";
        public const string SpGetITSTrackingID = "[dbo].[sp_GetITSTrackingID]";
        public const string SpUpDateITSTrackingID = "[dbo].[sp_UpDateITSTrackingID]";
        public const string SpGetEdocsITSCustomers = "[dbo].[sp_GetEdocsITSCustomers]";
        public const string SpDeleteEdocsITSCustomer = "[dbo].[sp_DeleteEdocsITSCustomer]";
        public const string SpEdocsITSGetUsersIDs = "[dbo].[sp_EdocsITSGetUsersIDs]";
        public const string SpGetEdocsITSUser = "sp_GetEdocsITSUser";
        public const string SpEdocsITSGetUsersByEmailAddress = "sp_EdocsITSGetUsersByEmailAddress";
        public const string SpEdocsITSUpDateUserProfile = "[dbo].[sp_EdocsITSUpDateUserProfile]";
        public const string SpEdocsITSGetUserByUserName = "sp_EdocsITSGetUserByUserName";
        public const string SpAddITSDocsScanned = "sp_AddITSDocsScanned";
        public const string SpUpDateITSTrackingIDByProjectName = "sp_UpDateITSTrackingIDByProjectName";
        public const string SPRunReportByProjectName = "sp_RunReportByProjectName";
        public const string SPGetRepFileNameByID = "sp_GetRepFileNameByID";
        public const string SPGetMDTRecordsOCR = "sp_GetMDTRecordsOCR";
        public const string SPUpDateMDTOCRResults = "sp_UpDateMDTOCRResults";
        public const string SPOverWrireITSTrackingIDByProjectName = "[dbo].[sp_OverWrireITSTrackingIDByProjectName]";
        public const string SPGetMaxMinDateByProjectName = "sp_GetMaxMinDateByProjectName";
        public const string SPRunReportInvoiceByCustIDProjectNamePrice = "sp_RunReportInvoiceByCustIDProjectNamePrice";
        public const string SPUpdateAccRejectDocs = "sp_UpdateAccRejectDocs";
        public const string SPUpdateUploadInvoice = "[dbo].[sp_UploadInvoiceHtml]";
        public const string SPGetInvoiceToPrint = "[dbo].[sp_GetInvoiceToPrint]";
        public const string SPGetInvoiceNumbers = "[dbo].[sp_GetInvoiceNumbers]";
        public const string SPGetInvoiceNotPaid = "sp_GetInvoiceNotPaid";

        public const string SPGetCustomerOpenInvoiceNumbers = "[dbo].[sp_GetCustomerOpenInvoiceNumbers]";
        public const string SPUpdateInvoicePaid = "[dbo].[sp_UpdateInvoicePaid]";
        public const string SPAddCustomInvoice = "sp_AddCustomInvoice";
        public const string SPPSUSDRecordsByDepOrgDepartment = "sp_PSUSDRecordsByDepOrgDepartment";
        public const string SPPSUSDRecordsByFirstLastName = "[dbo].[sp_PSUSDRecordsByFirstLastName]";
        public const string SPPPSUSDRecordsByDepOrgDepartmentFLName = "sp_PSUSDRecordsByDepOrgDepartmentFLName";
        

        #endregion
        public const string EdocsInventoryTrackingSystemCS = "EdocsInventoryTrackingSystemCS";
        
    }
}
