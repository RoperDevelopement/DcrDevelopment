using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Web.UI.WebControls;

namespace Edocs.Libaray.Upload.Archive.Batches.Interfaces
{
    interface ITrackingSystem
    {
        string ScanBatchID { get; set; }
        int EdocsCustomerID {  get; set; }
        
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
        bool StandardLargeDocument
        { get; set; }

    }
    interface ITotalCount
    {
        int TotalScanned { get; set; }
        int TotalPageCount { get; set; }
        int TotalType { get; set; }
        int TotalOCR { get; set; }
     
    }

    interface IJasonSettings:ITrackingSystem,ITotalCount
    {
        DateTime ScanDate
        { get; set; }
        string InventoryTrackingApiUrl
        { get; set; }
       
        string UploadFolder
        { get; set; }
    }
    
    interface IJsonMDTRecords:ITotalCount
    {
        string FileName
        { get; set; }
        string DocumentType
        { get; set; }
        string ProjectNumber
        { get; set; }
        string ProjectName
        { get; set; }

    }
}
