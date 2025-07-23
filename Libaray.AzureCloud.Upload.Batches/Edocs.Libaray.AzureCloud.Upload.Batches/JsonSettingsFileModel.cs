using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edocs.Libaray.AzureCloud.Upload.Batches
{
    public class JsonSettingsFileModel
    {
        public string ScanBatch
        { get; set; }
        public DateTime ReceiptDate
        { get; set; }
        public string ReceiptStation
        { get; set; }
        public string Category
        { get; set; }
        public string AzureDataBaseName
        { get; set; }
        public string ScanOperator
        { get; set; }
        public string AzureShareName
        { get; set; }
        public DateTime ScanDate
        { get; set; }
        public string AzureTableName
        { get; set; }
            public string AzureSPName
        { get; set; }
       public string AzureWebApiController
        { get; set; }

        //   public string ArchiveFolder
        // { get; set; }
    }
}
