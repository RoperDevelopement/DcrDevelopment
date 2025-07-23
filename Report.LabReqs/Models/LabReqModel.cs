using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Edocs.Report.LabReqs.Interfaces;
namespace Edocs.Report.LabReqs.Models
{
    class LabReqModel : ILabReq
    {
        public string BatchID
        { get; set; }
        public string IndexNumber
        { get; set; }
        public string FinNumber
        { get; set; }
        public DateTime DateOfServices
        { get; set; }
        public string PatID
        { get; set; }
        public string ReqNum
        { get; set; }
        public string ClientCode
        { get; set; }
        public DateTime ScanDate
        { get; set; }
        public bool Merged
        { get; set; }
        public string MRN
        { get; set; }
    }
}
