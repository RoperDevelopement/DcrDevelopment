using Edocs.Libaray.Upload.Archive.Batches.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace Edocs.Libaray.Upload.Archive.Batches.Interfaces
{
    interface ITrackingSystem
    {
        string ScanBatchID { get; set; }
        int EdocsCustomerID { get; set; }

        //  string 
        string ScanOperator
        { get; set; }
        string InventoryTrackingID
        { get; set; }
        string ScanMachine
        { get; set; }
        string InventoryTrackingSP
        { get; set; }
        string FileName
        { get; set; }
        bool OverWriteFile
        { get; set; }
    }
    interface IEdocCustomerInfo
    {
        int EdocsCustomerID
        { get; set; }
    }
    interface ITotalCount
    {
        int TotalScanned { get; set; }
        int TotalPageCount { get; set; }
        int TotalType { get; set; }
        int TotalOCR { get; set; }
    }
    interface IBatchID
    {
        string ScanBatchID
        { get; set; }
        string ScanOperator
        { get; set; }
    }
    interface IJasonBSBProdSettings : IEdocCustomerInfo, IBatchID
    {

        string InventoryTrackingApiUrl
        { get; set; }

        string UploadFolder
        { get; set; }
        string UploadApiUrl
        { get; set; }
        string UpLoadController
        { get; set; }
    }
    interface IJasonSettings : ITrackingSystem, ITotalCount
    {
        DateTime ScanDate
        { get; set; }
        string InventoryTrackingApiUrl
        { get; set; }

        string UploadFolder
        { get; set; }
        string UploadApiUrl
        { get; set; }
        string UpLoadController
        { get; set; }
    }
    interface ITrackingID
    {
        string TrackID
        { get; set; }
    }
    interface ITrackingInformation  
    {

        string InventoryTrackingApiUrl
        { get; set; }
        string InventoryTrackingSP
        { get; set; }

    }

    interface IJsonMDTRecords
    {
        string FileName
        { get; set; }
        string DocumentType
        { get; set; }
        string ProjectNumber
        { get; set; }
        string ProjectName
        { get; set; }
        string BoxNumber
        { get; set; }

    }
    interface IJsonBSBPropDelRecords
    {
        string FileName
        { get; set; }
        int PermitNumber
        { get; set; }
        int ParcelNumber
        { get; set; }
        int ZoneNumber
        { get; set; }
        int ExePermitNumber
        { get; set; }
        string GoCode
        { get; set; }
        string Address
        { get; set; }
        string OwnerLot
        { get; set; }
        string ConstCo
        { get; set; }
        //   DateTime DateIssued
        //   { get; set; }
        //   DateTime DateExpired
        //   { get; set; }
    }
    interface IJsonBSBPWDRecords
    {
        string FileName
        { get; set; }
        string FileUrl
        { get; set; }
        string ProjectDepartment
        { get; set; }
        int ProjectYear
        { get; set; }
        string ProjectName
        { get; set; }
        

    }
    interface INUmICROCRTyped
    {
        int TotalICROCR
        { get; set; }
        int TotalTyped
        { get; set; }
    }
    interface IArchiverTotalPages
    {
        int NumberDocsScanned
        { get; set; }
        int NumberTypedPerDoc
        { get; set; }
        int NumberDocsUploaded
        { get; set; }
        int NumberImagesSaved
        { get; set; }
        int NumberDocOCR
        { get; set; }
    }
    interface IUploadSearchTxt
    {
        int PermitNumber
        { get; set; }
        string SearchStr
        { get; set; }
    }
    interface IUploadFolders
    {
        string OCRImageFolder
        { get; set; }
        string UploadFolder
        { get; set; }
    }
    interface IStandSettingsJson : IBatchID, IArchiverTotalPages
    {
        string OCRImageFolder
        { get; set; }
          string UploadFolder
        { get; set; }
        string TrackinUpLoadController
        { get; set; }
    }
    interface ISettingsRecordsFiles
    {
        string JsonRecordsFile
        { get; set; }
        string JsonSettingFile
        { get; set; }
    }
    public interface IEdoscITSTrackingID
    {
        string TrackingID
        { get; set; }
        string UserName
        { get; set; }
    }
      interface IEdocsITSScanningManagement : IEdoscITSTrackingID, IEdocsCustomerName
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
      interface IEdocsCustomerName
    {


        string EdocsCustomerName { get; set; }
    }
    interface IStandardLargeDocument
    {
        bool StandardLargeDocument { get; set; }
    }
    interface IPDFFilename
    {
        string FileName { get; set; }
    }
    interface IDOH:IPDFFilename 
    {
         string City { get; set; }
        string Church { get; set; }
        string BookType { get; set; }
        string DateRangeStartDate { get; set; }
        string DateRangeEndDate { get; set; }
        string BookFirstPage { get; set; }
        string BookSecondPage { get; set; }
        string ImgOrgFileName { get; set; }
        string ImgFileName { get; set; }


    }
    interface IAzureCLoud
    {
        string AzureUpLoadContanier
        { get; set; }
    }
}
