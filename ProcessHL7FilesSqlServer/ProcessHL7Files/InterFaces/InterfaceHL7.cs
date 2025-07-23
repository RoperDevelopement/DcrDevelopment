using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using System.ComponentModel;
using static System.Net.WebRequestMethods;
using System.Net.PeerToPeer;
using System.Reflection;
using System.Security.Policy;

namespace ProcessHL7Files.InterFaces
{
    interface InterFaceHL7Patient
    {
        string FirstName
        { get; set; }
        string LastName
        { get; set; }
        string Address
        { get; set; }
        string State
        { get; set; }
        string PhoneNumber
        { get; set; }
        string ZipCode
        { get; set; }

    }
    interface InterFaceHL7MSH
    {
        string SendingApplication
        { get; set; }
        string SendingFacility { get; set; }
        string ReceivingApplication { get; set; }
        string ReceivingFacility { get; set; }
        DateTime DateTimeMessage { get; set; }
        string MessageType { get; set; }
        string Type { get; set; }
        string Event { get; set; }
        string MessageControlID { get; set; }
        string ProcessingID { get; set; }
        string VersionID { get; set; }
        string AcceptAckPointer { get; set; }
        string ApplicationAckType { get; set; }
        string MessageProfileIdentifier { get; set; }
        string EntityIdentifier { get; set; }
        string NamespaceID { get; set; }
        string UniversalID { get; set; }
        string UniversalIDType { get; set; }
    }
    interface InterFaceHL7Doctors
    {


        int DrCode { get; set; }

        string DrFirstName { get; set; }
        string DrLastName { get; set; }
        string DrAddress { get; set; }
        string DrPhoneNumber { get; set; }
        string DrCity { get; set; }
        string DrState { get; set; }
        int DrZipCode { get; set; }

    }
    interface InterFaceHL7PID
    {
        int SetID { get; set; }
        string EMPINumber { get; set; }
        string MedicalRecordNumber { get; set; }
        string FacilityIdentifier { get; set; }
        string NamespaceID { get; set; }
        string UniversalID { get; set; }
        string UniversalIDType { get; set; }
        string ClientCode { get; set; }
        string PatientName { get; set; }
        string MaidenName { get; set; }
        DateTime DateofBirth { get; set; }
        string Sex { get; set; }
        string PatientAlias { get; set; }
        string PatientTypeCode { get; set; }
        string Race { get; set; }
        string PatientAddress { get; set; }
        string StreetAddress { get; set; }
        string Address2 { get; set; }
        string City { get; set; }
        string State { get; set; }
        string Zip { get; set; }
        string Country { get; set; }
        string AddressTypeCode { get; set; }
        string CountyParishCode { get; set; }
        string CountyCode { get; set; }
        string HomePhone { get; set; }
        string HomePhoneNumber { get; set; }
        string TelecommunicationuseCode { get; set; }
        string TelecommunicationEquipmentType { get; set; }
        string EmailAddress { get; set; }
        string Extension { get; set; }
        string AnyText { get; set; }
        string LanguageCode { get; set; }
        string IdentifierLanguageCodeText { get; set; }
        string CodingSystem { get; set; }
        string MaritalStatus { get; set; }
        string MaritalTypeCode { get; set; }
        string TextDescription { get; set; }
        string Religion { get; set; }
        string PatientAccountNumber { get; set; }
        string CheckDigit { get; set; }
        string CheckDigitSchema { get; set; }
        string AssigningAuthority { get; set; }
        string UniversalTypeID { get; set; }
        string EncounterAliasTypeCode { get; set; }
        string AssigningFacility { get; set; }
        string SocialSecurity { get; set; }
        string DriversLicenseNumber { get; set; }
        string MothersIdentifier { get; set; }
        string EthicGroup { get; set; }
        string IdentifierText { get; set; }
        string NameofCodingSystem { get; set; }
        string BirthPlace { get; set; }
        string MultipleBirthCode { get; set; }
        string BirthOrder { get; set; }
        string Citizenship { get; set; }
        string CitizenshipCode { get; set; }
        string DisplayDescription { get; set; }
        string AlternateIdentifier { get; set; }
        string AlternateText { get; set; }
        string AlternateCodingSystem { get; set; }
        string VeteransMilitaryStatus { get; set; }
        string Nationality { get; set; }
        string NationalityCodeText { get; set; }
        DateTime PatientDeathDateTime { get; set; }
        string PatientDeathIndicator { get; set; }



    }

    interface InterFaceHL7PV1
    {
        string SetID { get; set; }
        string PatientClass { get; set; }
        string PatientLocation { get; set; }
        string NursingUnitOutpatientLocation { get; set; }
        string Room { get; set; }
        string Bed { get; set; }
        string FacilityCode { get; set; }
        string AdmitType { get; set; }
        string PreadmitNumber { get; set; }
        string PatientPriorLocation { get; set; }
        string AttendingDoctor { get; set; }
        string ID { get; set; }
        string PhysicianName { get; set; }
        string Facility { get; set; }
        string AssigningAuthorityAliasPoolCode { get; set; }
        string NamespaceID { get; set; }
        string UniversalID { get; set; }
        string UniversalIDType { get; set; }
        string IdentifierType { get; set; }
        string ReferringMD { get; set; }
        string ConsultingMD { get; set; }
        string HospitalService { get; set; }
        string PreadmitTestingCode { get; set; }
        string ReadmitIndicator { get; set; }
        string AdmitSource { get; set; }
        string AmbulatoryStatus { get; set; }
        string VIPIndicator { get; set; }
        string AdmittingMD { get; set; }
        string PatientType { get; set; }
        string VisitNumber { get; set; }
        string FinancialClass { get; set; }
        string CourtesyCode { get; set; }
        string DischargeDisposition { get; set; }
        string DischargedToLocation { get; set; }
        string DescriptionofLocation { get; set; }
        DateTime AdmitDateTime { get; set; }
        DateTime DischargeDateTime { get; set; }
        string AlternateVisitIdCLIENTCODE { get; set; }

    }
    interface InterFaceHL7ORCBlood
    {
        string OrderControl { get; set; }
        string OrderNumberfromSendingSystem { get; set; }
        string SystemIdentifier { get; set; }
        string MillenniumOrderNumber { get; set; }
        string MillenniumIdentifier { get; set; }
        string QuantityTiming { get; set; }
        DateTime DateTimeofTransaction { get; set; }
        string OrderingClinicalStaff { get; set; }

    }
    interface InterFaceHL7OBR
    {
        string SetID { get; set; }
        string OrderNumberfromSendingSystem { get; set; }
        string SystemIdentifier { get; set; }
        string MillenniumOrderNumber { get; set; }
        string MillenniumIdentifier { get; set; }
        string OrderCode { get; set; }
        string OrderDescription { get; set; }
        string CodingSystem { get; set; }
        string OriginalTextDisplay { get; set; }
        string Priority { get; set; }
        DateTime CollectionDateTime { get; set; }
        DateTime ObservationEndDateTime { get; set; }
        string CollectionVolume { get; set; }
        string Quantity { get; set; }
        string Units { get; set; }
        string Identifier { get; set; }
        string Text { get; set; }
        string CodingSystem1 { get; set; }
        string CollectedBy { get; set; }
        string ID { get; set; }
        string FamilyName { get; set; }
        string MiddleName { get; set; }
        string LastName { get; set; }
        string Degree { get; set; }
        string PersonalSuffix { get; set; }
        string SpecimenActionCode { get; set; }
        string DangerCode { get; set; }
        string IsolationCodeIdentifier { get; set; }
        string Text1 { get; set; }
        string CodingSystem1 { get; set; }
        string RelevantClincalInfo { get; set; }
        string Identifier1 { get; set; }
        string Text2 { get; set; }
        string NameofCodingSystem { get; set; }
        string OriginalText { get; set; }
        DateTime SpecimenReceivedDateTime { get; set; }
        string SpecimenSource { get; set; }
        string SourceCode { get; set; }
        string SpecimenType { get; set; }
        string SpecimenType1 { get; set; }
        string SourceComment { get; set; }
        string BodySite1 { get; set; }
        string BodySite2 { get; set; }
        string BodySite3 { get; set; }
        string OrderingProvider { get; set; }
        string PlacerField1 { get; set; }
        string AccessionNumber { get; set; }
        DateTime ResultsReportLastUpdateDateandTime { get; set; }
        string DiagnosticServiceSection { get; set; }
        string ResultStatus { get; set; }
        string ParentResult { get; set; }
        string ParentOrderID { get; set; }
        string ParentSubId { get; set; }
        string ParentResult1 { get; set; }
        string QuantityTiming { get; set; }
        string QuantityComponent { get; set; }
        string IntervalComponentfrequency { get; set; }
        DateTime OrderStartDateTime { get; set; }
        DateTime EndDateTime { get; set; }
        string CollectionPriorityReporting { get; set; }
        string ConditionComponentSpecinx { get; set; }
        string ConjunctionComponent { get; set; }
        string ResultCopiesTo { get; set; }
        string ParentNumber { get; set; }
        string ParentPlacerOrderNum { get; set; }
        string ParentFillerOrderNum { get; set; }
        string IndicatorforBloodBankProduct { get; set; }

    }
}


