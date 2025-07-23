using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edocs.Ocr.SearchablePdf.Interfaces
{
    public interface IEdocsCustIDTrackingID  
    {
        int EdocsCustomerID
        { get; set; }
        string InventoryTrackingID
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
        int TotalPageCount
        { get; set; }
      
        int TotalOCR
        { get; set; }

        int TotalCharTyped
        { get; set; }
        

    }
  public  interface ZipFolder
    {
        bool ZipDownLoadFolder
        { get; set; }
    }
  public  interface ITrackingSystem
    {
        Guid ScanBatchID { get; set; }
        
        //  string 
        string ScanOperator
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
  public  interface OcrSettings: IEdocsCustIDTrackingID,ITotalRecords
    {
        Guid ScanBatchID { get; set; }
        string OCRFolder
        { get; set; }
        string InventoryTrackingApiUrl
        { get; set; }
        string InventoryTrackingSP
        { get; set; }
        string InventoryTrackingUpLoadController
        { get; set; }
        string PdfSavedFile
        { get; set; }
       
         
    }
}
