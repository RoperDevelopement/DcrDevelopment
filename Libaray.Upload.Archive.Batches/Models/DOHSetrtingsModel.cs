using System;
using System.Collections.Generic;

using System.Text;
using Edocs.Libaray.Upload.Archive.Batches.Interfaces;
namespace Edocs.Libaray.Upload.Archive.Batches.Models
{
    public class DOHSetrtingsModel : IJasonBSBProdSettings, IAzureCLoud
    {
        public string InventoryTrackingApiUrl
        { get; set; }
        public string UploadFolder
        { get; set; }
        public string UploadApiUrl
        { get; set; }
        public string UpLoadController
        { get; set; }
        public int EdocsCustomerID
        { get; set; }
        public string ScanBatchID
        { get; set; }
        public string ScanOperator
        { get; set; }
        public string AzureUpLoadContanier
        { get; set; }
    }
}
