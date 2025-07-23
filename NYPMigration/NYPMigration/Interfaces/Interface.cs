using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NYPMigration.Interfaces
{
    public interface INYPRecordsID
    {
        int ID { get; set; }
        DateTime ScanDate { get; set; }
    }
    public interface INYPRecords
    {

        string FileName { get; set; }
        string FileUrl { get; set; }

        string BatchID { get; set; }
    }
    public interface INYPDOH : IMrn, IDateofService, INYPRecordsID, INYPRecords, IDateUpLoaded
    {
        string AccessionNumber
        { get; set; }
        string DrID
        { get; set; }

    }
    public interface IDateUpLoaded
    {
        DateTime DateUpload
        { get; set; }

    }
    public interface IMrn
    {
        string MRN { get; set; }
    }
    public interface IDateofService
    {
        DateTime DateOfService { get; set; }
    }
    public interface INYPLabReqs : IMrn, IDateofService
    {
        string IndexNumber { get; set; }
        string FinancialNumber { get; set; }


        string DrID { get; set; }
        string PatientID { get; set; }
        string ClientID { get; set; }
        string RequisitionNumber { get; set; }
        string ClientCode { get; set; }
    }
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
    public interface ILabReqsPatientID
    {
        string PatientID
        { get; set; }
        string PatientFirstName
        { get; set; }
        string PatientLastName
        { get; set; }
        string ClientCode
        { get; set; }
    }
    public interface INYPMaintenanceLogsLogStations
    {
        int ID { get; set; }
        string LogStation { get; set; }
    }

    public interface INYPMaintenanceLog : INYPRecordsID, INYPRecords, IDateUpLoaded
    {
        DateTime LogDate
        { get; set; }
        int LogStationId
        { get; set; }
        string Checksum
        { get; set; }

    }
    public interface INYPMissingPunchFormsLocation
    {
        int ID
        { get; set; }
        string PunchFormsLocation
        { get; set; }
    }
    public interface INYPMissingPunchForms : INYPRecordsID, IDateUpLoaded, INYPRecords
    {
        DateTime LogDate
        { get; set; }
        int Location
        { get; set; }
    }
    public interface INYPSendOutResultsPerformingLabCodes
    {
        int Id
        { get; set; }
        string PerformingLabCode
        { get; set; }
    }
    public interface INYPSendOutResults : INYPRecordsID, INYPRecords, IMrn, IDateofService, IDateUpLoaded
    {
        int PerformingLabCode
        { get; set; }
        string AccessionNumber
        { get; set; }
        string FinancialNumber
        { get; set; }
        string LastName
        { get; set; }
        string FirstName
        { get; set; }
    }

    public interface INYPSpecimenRejection : INYPRecordsID, INYPRecords
    {
        DateTime LogDate
        { get; set; }
      string CaseNumber
        { get; set; }
      int RejectionReasonID
        { get; set; }
    }
    public interface INYPRejectionLogsReason
    {
       int ID
        { get; set; }
      string Reason
        { get; set; }
    }
    public interface INYPDrCodes
    {
       string DrCode
        { get; set; }
      string DrFirstName
        { get; set; }
      string DrLastName
        { get; set; }
    }


}
