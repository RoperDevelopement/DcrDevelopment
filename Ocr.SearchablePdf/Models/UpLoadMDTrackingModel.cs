 
using System;
using System.Collections.Generic;
using System.Linq;
using Edocs.Ocr.SearchablePdf.Interfaces;
namespace Edocs.Ocr.SearchablePdf.Models
{
 public   class UpLoadMDTrackingModel : ITrackingSystem, ITotalRecords
    {
        public Guid ScanBatchID { get; set; }
        public int EdocsCustomerID { get; set; }

        public string ScanOperator
        { get; set; }
        public string InventoryTrackingID
        { get; set; }
        public string ScanMachine
        { get; set; }
        public string InventoryTrackingSP
        { get; set; }
     public   int TotalScanned
        { get; set; }
       public int TotalPageCount
        { get; set; }

       public int TotalOCR
        { get; set; }

     public  int TotalCharTyped
        { get; set; }
        public string FileName
        { get; set; }
        public bool OverWriteFile
        { get; set; } = false;
        public int TotalType { get; set; }
        public bool StandardLargeDocument
        { get; set; } = false;

    }
}
