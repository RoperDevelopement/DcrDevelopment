using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Edocs.Libaray.Upload.Archive.Batches.Interfaces;
using Edocs.Libaray.Upload.Archive.Batches.Models;
namespace Edocs.Libaray.Upload.Archive.Batches.Models
{
    class PSUSDSettingsModel : IStandSettingsJson, ITrackingInformation
    {
        public string OCRImageFolder
        { get; set; }
      public  int NumberDocsScanned
        { get; set; }
        public int NumberTypedPerDoc
        { get; set; }
        public int NumberDocsUploaded
        { get; set; }
        public int NumberImagesSaved
        { get; set; }
        public int NumberDocOCR
        { get; set; }
        public string ScanBatchID
        { get; set; }
        public string ScanOperator
        { get; set; }
        public int EdocsCustomerID
        { get; set; }
        public string InventoryTrackingApiUrl
        { get; set; }
        public string InventoryTrackingSP
        { get; set; }
        public string UploadFolder
        { get; set; }
        public string TrackinUpLoadController
        { get; set; }
        public string TransferByTrackIDController
        { get; set; }
        public bool OCRRecords
        { get; set; }
    }
}
