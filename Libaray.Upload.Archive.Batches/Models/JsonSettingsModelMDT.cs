using Edocs.Libaray.Upload.Archive.Batches.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Edocs.Libaray.Upload.Archive.Batches.Models
{
    class JsonSettingsModelMDT //: IJasonSettings
    {
     public   string ScanBatchID { get; set; }
        public int EdocsCustomerID { get; set; }
        public int TotalScanned { get; set; }
        public int TotalPageCount { get; set; }
        //  string 
        public string ScanOperator
        { get; set; }
        public string InventoryTrackingID
        { get; set; }
        public string ScanMachine
        { get; set; }
        public DateTime ScanDate
        { get; set; }
        public string InventoryTrackingApiUrl
        { get; set; }
        public string InventoryTrackingSP
        { get; set; }
        public string UploadFolder
        { get; set; }
        public string FileName
        { get; set; }
        public bool OverWriteFile
        { get; set; } = false;
        public int TotalType { get; set; }
    }
}
