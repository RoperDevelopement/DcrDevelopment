using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Edocs.Ocr.SearchablePdf.Interfaces;
namespace Edocs.Ocr.SearchablePdf.Models
{
  public  class OcrSettingsModel: OcrSettings
    {
        public Guid ScanBatchID { get; set; }
        public int EdocsCustomerID
        { get; set; }
        public string InventoryTrackingID
        { get; set; }
        public  string OCRFolder
        { get; set; }
        public string InventoryTrackingApiUrl
        { get; set; }
        public string InventoryTrackingUpLoadController
        { get; set; }
        public string PdfSavedFile
        { get; set; }
     public   int TotalScanned
        { get; set; }
      public  int TotalPageCount
        { get; set; }

      public  int TotalOCR
        { get; set; }

      public  int TotalCharTyped
        { get; set; }
        public string InventoryTrackingSP
        { get; set; }

    }
}
