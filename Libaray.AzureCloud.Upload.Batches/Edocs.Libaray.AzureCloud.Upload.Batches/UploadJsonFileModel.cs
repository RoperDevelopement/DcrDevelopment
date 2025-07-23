using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edocs.Libaray.AzureCloud.Upload.Batches
{
   public class JsonFileModel
    {
        public string FileName
        { get; set; }
        public string FileUrl
        { get; set; }
        public string ScanBatch
        { get; set; }
        public DateTime DateUpload
        { get; set; }
        public string ScanOperator
        { get; set; }
        public string ScanMachine
        { get; set; }
        public DateTime ScanDate
        { get; set; }
        public string FileExtension
        { get; set; }
        public string AzureTableName
        { get; set; }
        public string AzureStpredProcedureName
        { get; set; }
    }
    class JsonFileLabRecsModel:JsonFileModel
    {
        public string IndexNumber
        { get; set; }
        public DateTime DateOfService
        { get; set; }
        public string PatientID
        { get; set; }
        public string RequisitionNumber
        { get; set; }
        public string ClientCode
        { get; set; }
        public DateTime ReceiptDate
        { get; set; }
        public string Category
        { get; set; }
        public string CsnNumber
        { get; set; }


    }
}
