using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edocs.PSE.Api.Models
{
    public class BSBUploadJsonModel
    {
        public string FileName
        { get; set; }
        public string FileUrl
        { get; set; }
        public string Date
        { get; set; }
        public string Title
        { get; set; }
        public string Collection
        { get; set; }

        public string ITSPath
        { get; set; }
        public string Description
        { get; set; }

        public string ScanBatch
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
        public string WebApi
        { get; set; }
        public DateTime DateUpload
        { get; set; }
        public string ScanMachine
        { get; set; }
    }
}
