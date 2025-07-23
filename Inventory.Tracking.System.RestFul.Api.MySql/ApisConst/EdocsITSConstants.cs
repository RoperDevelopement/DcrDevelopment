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

        public const string SpParmaScanMachine = "@ScanMachine";
        public const string SpParmaScanOperator = "@ScanOperator";
        public const string SpParmaFileName = "@FileName";
        public const string SpParmaStandardLargeDoc = "@StandardLargeDocument";

        public const string SpParmaInvoiceStartDate = "@InvoiceStartDate";
        public const string SpParmaInvoiceEndDate = "@InvoiceEndDate";
        public const string SpParmaInvoiceTotalAmount = "@InvoiceTotalAmount";
        public const string SpParmaAcceptRject = "@AcceptRject";
        public const string SpParmaInvoiceNumber = "@InvoiceNumber";
        public const string SpParmaComments = "@Comments";
        public const string SpParmaHtmlData = "@HtmlData";

        

        #endregion
        #region stored proc
        public const string SpAddUpdateEdocsCustomers = "sp_AddUpdateEdocsCustomers";
        public const string SpAddAddInvoice = "sp_AddInvoice";
        public const string SpGetDocumentsForReview = "sp_GetDocumentsForReview";
        
        public const string SpAddInventoryTransfer = "sp_AddInventoryTransfer";
        public const string SpGetITSTrackingID = "sp_GetITSTrackingID";
        public const string SpUpDateITSTrackingID = "sp_UpDateITSTrackingID";
        public const string SpGetEdocsITSCustomers = "sp_GetEdocsITSCustomers";
        public const string SpDeleteEdocsITSCustomer = "sp_DeleteEdocsITSCustomer";
        public const string SpEdocsITSGetUsersIDs = "sp_EdocsITSGetUsersIDs";
        public const string SpGetEdocsITSUser = "sp_GetEdocsITSUser";
        public const string SpEdocsITSGetUsersByEmailAddress = "sp_EdocsITSGetUsersByEmailAddress";
        public const string SpEdocsITSUpDateUserProfile = "sp_EdocsITSUpDateUserProfile";
        public const string SpEdocsITSGetUserByUserName = "sp_EdocsITSGetUserByUserName";
        public const string SpAddITSDocsScanned = "sp_AddITSDocsScanned";
        public const string SpUpDateITSTrackingIDByProjectName = "sp_UpDateITSTrackingIDByProjectName";
        public const string SPRunReportByProjectName = "sp_RunReportByProjectName";
        public const string SPGetRepFileNameByID = "sp_GetRepFileNameByID";
        public const string SPGetMDTRecordsOCR = "sp_GetMDTRecordsOCR";
        public const string SPUpDateMDTOCRResults = "sp_UpDateMDTOCRResults";
        public const string SPOverWrireITSTrackingIDByProjectName = "sp_OverWrireITSTrackingIDByProjectName";
        public const string SPGetMaxMinDateByProjectName = "sp_GetMaxMinDateByProjectName";
        public const string SPRunReportInvoiceByCustIDProjectNamePrice = "sp_RunReportInvoiceByCustIDProjectNamePrice";
        public const string SPUpdateAccRejectDocs = "sp_UpdateAccRejectDocs";
        public const string SPUpdateUploadInvoice = "sp_UploadInvoiceHtml";
        public const string SPGetInvoiceToPrint = "sp_GetInvoiceToPrint";
        public const string SPGetInvoiceNumbers = "sp_GetInvoiceNumbers";
        public const string SPGetInvoiceNotPaid = "sp_GetInvoiceNotPaid";

        public const string SPGetCustomerOpenInvoiceNumbers = "sp_GetCustomerOpenInvoiceNumbers";

        #endregion
        public const string EdocsInventoryTrackingSystemCS = "EdocsInventoryTrackingSystemCS";
    }
}
