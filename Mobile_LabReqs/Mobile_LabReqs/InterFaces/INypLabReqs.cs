using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Mobile_LabReqs.InterFaces
{
    public interface ILabReqs : ILabReqsCommon
    {
        //     Guid FileName
        //    { get; set; }

        int LabReqID
        { get; set; }
        string IndexNumber
        { get; set; }

        string FinancialNumber
        { get; set; }




        string DrCode
        { get; set; }
        string PatientID
        { get; set; }
        string ClientID
        { get; set; }
        string RequisitionNumber
        { get; set; }
        string ClientCode
        { get; set; }
        DateTime DateOfService
        { get; set; }

        DateTime ReceiptDate
        { get; set; }


        string Category
        { get; set; }


        int BeingReviewed
        { get; set; }
        int Merged
        { get; set; }

        string PatientName
        { get; set; }

        string DrName
        { get; set; }
        string MRN
        { get; set; }

    }

   public  interface ILabReqsCommon
    {
        string FileUrl
        { get; set; }

        string ScanBatch
        { get; set; }


        DateTime DateUpload
        { get; set; }

        string ScanOperator
        { get; set; }

        string ScanMachine
        { get; set; }

        DateTime ScanDate
        { get; set; }

        string FileExtension
        { get; set; }

        DateTime DateModify
        { get; set; }

        string ModifyBy
        { get; set; }

        bool SearchPartial
        { get; set; }

    }

      interface ISpecimenRejection : ILabReqsCommon
    {
        DateTime LogDate
        { get; set; }
        string FromYear
        { get; set; }
        string FromNumber
        { get; set; }

        string FromReason
        { get; set; }
    }

      interface IDrCodes
    {
        string FileUrl
        { get; set; }

        string ScanBatch
        { get; set; }
        string DrCode
        { get; set; }
        bool SearchPartial
        { get; set; }
        string ScanOperator
        { get; set; }
        string DrFName
        { get; set; }
        string DrLName
        { get; set; }
        DateTime ScanDate
        { get; set; }
        DateTime ScanEndDate
        { get; set; }
    }
      interface IGranitReceipts
    {
        string FileUrl
        { get; set; }
        string ClientCode
        { get; set; }
        string Comments
        { get; set; }
        DateTime DocumentDate
        { get; set; }
        DateTime ScanDate
        { get; set; }
        string ScanOperator
        { get; set; }
        bool SearchPartial
        { get; set; }
    }

      interface IDoh : ILabReqsCommon
    {
        string PerformingLabCode
        { get; set; }

        string AccessionNumber
        { get; set; }

        string MRN
        { get; set; }

        DateTime DateOFService
        { get; set; }


    }
      interface IMaintenanceLogsModel : ILabReqsCommon
    {
        string LogStation
        { get; set; }

        DateTime LogDate
        { get; set; }




    }
      interface ISendOutPackingSlips : ILabReqsCommon
    {


        DateTime DateOfService
        { get; set; }




    }
      interface ISendOutResults : IDoh
    {
        string FirstName
        { get; set; }
        string LastName
        { get; set; }
        string FinancialNumber
        { get; set; }

    }
      interface IEmployeeComplianceLogs : ILabReqsCommon
    {
        string FirstName
        { get; set; }
        string LastName
        { get; set; }
        string EmpIdNumber
        { get; set; }

        string EmpDepartment
        { get; set; }

        string EmpJobTitle
        { get; set; }

        string EmpDocumentType
        { get; set; }


    }

      interface IInvoices : ILabReqsCommon
    {
        string Department
        { get; set; }
        string Category
        { get; set; }
        DateTime InvoiceDate
        { get; set; }

        string Account
        { get; set; }
        string Reference
        { get; set; }
    }

      interface IEmailSettings
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
      interface INypUsers
    {
        string Cwid
        { get; set; }
        string FirstName
        { get; set; }
        string LastName
        { get; set; }
        string EmailAddress
        { get; set; }
        DateTime LastLoggedIn
        { get; set; }
        bool ViewAuditLogs
        { get; set; }
        bool Active
        { get; set; }
        bool IsAdmin
        { get; set; }
        bool DelUser
        { get; set; }

    }
      interface IAuditLogs
    {
        string Cwid
        { get; set; }
        string AuditLogApplicationName
        { get; set; }
        string AuditLogMessageType
        { get; set; }
        DateTime AuditLogDate
        { get; set; }
        DateTime AuditLogUpLoadDate
        { get; set; }
        int AuditLogID
        { get; set; }
        Uri AuditLogUrl
        { get; set; }
    }

      interface IAuditLogsCsv
    {
        string Cwid
        { get; set; }
        string AuditLogApplicationName
        { get; set; }
        string AuditLogMessageType
        { get; set; }
        string AuditLogDate
        { get; set; }
        
        string AuditLogMessage
        { get; set; }
        
        string AuditLogMachine
        { get; set; }
        string AuditLogDomain
        { get; set; }
    }
    interface IArchiver
    {
        string ArchiverName
        { get; set; }
        string ArchiverFileName
        { get; set; }

        byte[] ArchiverImage
        { get; set; }
    }
    
}

