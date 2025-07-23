using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;

namespace Edocs.ITS.AppService.Interfaces
{

    public interface IEmailSettings
    {
        string EmailServer
        { get; set; }
        string EmailFrom
        { get; set; }
        string EmailPassWord
        { get; set; }
        int EmailPort
        { get; set; }
        string EmailTo
        { get; set; }
        string EmailCC
        { get; set; }
        string TextTo
        { get; set; }
        string TextCC
        { get; set; }
    }
    public interface IInvNumber
    {
       int InvoiceNum {get;set; }
    }
    public interface IInvDateSent
    {
        DateTime DateInvoiceSent
        { get; set; }
    }
        
    public interface IUserLogin
    {
        string UserName
        { get; set; }
        string Password
        { get; set; }
    }

    
    public interface IUserEmailAddress
    {
        string EmailAddress
        { get; set; }
    }
    public interface IEdocsCustomerName
    {


        string EdocsCustomerName { get; set; }
    }
    public interface IInventroyTransfer : IEdocsCustomerName, IEdoscITSTrackingID
    {


        DateTime DateSent
        { get; set; }

        int NumberDocsSent
        { get; set; }
        string ScanType
        { get; set; }

        string DeliveryMethod
        { get; set; }


    }
    public interface IEdoscITSTrackingID
    {
        string TrackingID
        { get; set; }
        string UserName
        { get; set; }
    }
    public interface IEdocsITSScanningManagement : IEdoscITSTrackingID, IEdocsCustomerName
    {
        int IDTracking
        { get; set; }
        DateTime DateDocumentsReceived
        { get; set; }
        DateTime DateScanningStarted
        { get; set; }
        int NumberDocumentsReceived
        { get; set; }
    }
    public interface IEdocsCustID : IEdocsCustomerName
    {
        int EdocsCustomerID
        { get; set; }
    }
     
     
    public interface IEdocsITSDocsScannedTrackingID : ITotalRecords
    {
        string SpAddITSDocsScanned
        { get; set; }
    }
    public interface ITotalRecords
    {
        int TotalScanned
        { get; set; }
        int TotalRecordsUploaded
        { get; set; }

        string TrackingID
        { get; set; }
       int TotalDocsOCR
        { get; set; }
	  int  TotalCharTyped
        { get; set; }
        float TotalOcrCost
        { get; set; }
        float TotalCostPerDoc
        { get; set; }
        float TotalCostPerChar
        { get; set; }
    }
    public interface ICustomerID
    {
        int EdocsCustomerID
        { get; set; }
    }
    public interface IEdocsITSCustomers : IEdocsCustomerName, IStorage
    {

       

        string EdocsCustomerAddress { get; set; }

        string EdocsCustomerCity { get; set; }

        string EdosCustomerState { get; set; }

        string EdocsCustomerZipCode { get; set; }

        string EdosCustomerFirstName { get; set; }

        string EdocsCustomerLastName { get; set; }

        string EdocsCustomerEmailAddress { get; set; }

        string EdocsCustomerPhoneNumber { get; set; }

        string EdocsCustomerCellPhoneNumber { get; set; }

        string EdocsCustomerModifyBy { get; set; }
        DateTime EdocsCustomerDateAdded { get; set; }


        DateTime EdocsCustomerDateModify { get; set; }
        string EdocsCustomerNotes { get; set; }
        bool Active
        { get; set; }

    }
    public interface IPrices
    {
        float PriceStoreByMonth
        { get; set; }
        float PricePerDocument
        { get; set; }
        float PriceSetUpFee
        { get; set; }
        float PriceOcr
        { get; set; }
        float PricePerChar
        { get; set; }
        float PricePerLargeDocuments
        { get; set; }
    }
    public interface IInvoicePaid
    {
        float TotalPaid { get; set; }
        float TotalAmountPaid { get; set; }
        float TotalInvoiceAmount { get; set; }
        DateTime  DateInvoicePaid { get; set; }
      DateTime InvoiceStartDate { get; set; }
      DateTime  InvoiceEndDate { get; set; }
        bool InvoicePaid { get; set; }
    }
    public interface IStorage: IPrices
    {
        
        bool StoreDocuments
        { get; set; }
        int StorageYears
        { get; set; }
        int StorageDays
        { get; set; }
        int StorageMonths
        { get; set; }

      
    }
    public interface IUserCellPhone
    {
        string CellPhoneNumber
        { get; set; }
    }
    public interface IEdocsITSUserName : IEdocsCustomerName
    {

        string UserName
        { get; set; }
        int UserID
        { get; set; }

    }
    public interface IAcceptRecDoc
    {
        bool AcceptRejectDoc
        { get; set; }
    }
    public interface IUserLoginInfo : IUserLogin, IUserEmailAddress, IUserCellPhone, IEdocsCustomerName
    {

        DateTime LastMFLA
        { get; set; }
        bool IsCustomerAdmin
        { get; set; }
        bool IsEdocsAdmin
        { get; set; }

        string NewPassword
        { get; set; }


    }
    public interface IEdocsITSUser : IUserLoginInfo
    {
        string FirstName
        { get; set; }
        string LastName
        { get; set; }
        DateTime DateLastLogin
        { get; set; }

        DateTime DatePasswordLastChanged
        { get; set; }
        bool IsUserActive
        { get; set; }
    }
    public interface IDateUploaded
    { 
        DateTime ScannDate
        { get; set; }
           
    }
     public interface IMinMaxDate
    {
        DateTime MinDate
        { get; set; }
        DateTime MaxDate
        { get; set; }
    }
    public interface IID
    {
        int ID
        { get; set; }
    }
    public interface ITSCoastRep: IDateUploaded,IEdocsCustomerName, IUserEmailAddress,IID
    {
        
        string TrackingID
        { get; set; }
         int Scanned
        { get; set; }
        int  Uploaded
        { get; set; }
          int DocsOCR
        { get; set; }

          float OcrCost
        { get; set; }

          int CharTyped
        { get; set; }
           float CostPerDoc
        { get; set; }
          float CostPerChar
        { get; set; }

          float PricePerDocument
        { get; set; }
          float PriceOCR
        { get; set; }
          float PriceCharTyped
        { get; set; }
       
    }
    public interface IFileName
    {
        string FileName { get; set; }
    }
    public interface IInvoice:IFileName 
    {
        int EdocsCustomerID
        { get; set; }
        DateTime InvoiceStartDate { get; set; }
    DateTime InvoiceEndDate { get; set; }
    float InvoiceTotalAmount { get; set; }
	
    }
    
    public interface IEmailService
    {
        public void SendEmail(string message, string subject);
        public void SendText(string message, string subject, string textTo, string textCC);
        public void SendEmail(string message, string subject, string emailTo, string emailCC);
    }
}
