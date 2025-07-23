using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Edocs.Libaray.Upload.Archive.Batches.Models
{
    class BSBPWDSettingsModel
    {

        public string ScanBatchID
        { get; set; }
        public string ScanOperator
        { get; set; }
        public string InventoryTrackingApiUrl
        { get; set; }
        public string InventoryTrackingSP
        { get; set; }
        public string UploadApiUrl
        { get; set; }
        public string UpLoadController
        { get; set; }
        public string UploadFolder
        { get; set; }
        public int EdocsCustomerID
        { get; set; }
        public string LCROCTxtFile
        { get; set; }
        public int TotalScanned
        { get; set; }
        public int TotalImagesSaved
        { get; set; }
        public int TotalPageCount
        { get; set; }
        public int TotalOCR
        { get; set; }
        public int TotalType
        { get; set; }
        public bool OCRRecordsUpLoad
        { get; set; }
        public string CreateSearchabelPDF
        { get; set; }
        public string OCRImageFolder
        { get; set; }
        public string TrackinUpLoadController
        { get; set; }
    }
}
