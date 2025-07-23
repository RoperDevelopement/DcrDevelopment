using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edocs.Demos.Restful.Api.Interfaces;
namespace Edocs.Demos.Restful.Api.Models
{
    public class BSPProdDepUloadRecords//: IJsonBSBPropDepRecords
    {
        public string FileName
        { get; set; }
        public int PermitNumber
        { get; set; }
        public int ParcelNumber
        { get; set; }
        public int ZoneNumber
        { get; set; }
        public string GoCode
        { get; set; }
        public string Address
        { get; set; }
        public string OwnerLot
        { get; set; }
        public string ConstCo
        { get; set; }
        public string DateIssued
        { get; set; }
        public string DateExpired
        { get; set; }
        public int ExePermitNumber
        { get; set; }
        public int TotalScanned { get; set; }
        public int TotalPageCount { get; set; }
        public Guid ScanBatchID
        { get; set; }
        public int TotalType

        { get; set; }
        public string FileUrl
        { get; set; }
        public string ScanOperator
        { get; set; }
    }
}
