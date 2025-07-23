using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Edocs.Libaray.Upload.Archive.Batches.Interfaces;
namespace Edocs.Libaray.Upload.Archive.Batches.Models
{
  public  class BSBProdDepSettingsModel: IJasonBSBProdSettings
    {
        
        public    string ScanBatchID
            { get; set; }
        public string InventoryTrackingApiUrl
            { get; set; }
        public string ScanOperator
            { get; set; }
        public string UploadFolder
            { get; set; }
        public string UploadApiUrl
            { get; set; }
        public string UpLoadController
            { get; set; }
       public int EdocsCustomerID
        { get; set; }
     
     
    }
}
