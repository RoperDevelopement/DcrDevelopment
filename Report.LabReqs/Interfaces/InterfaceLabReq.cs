using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edocs.Report.LabReqs.Interfaces
{
    interface ILabReq
    {
        
        string BatchID
        { get; set; }
      string IndexNumber
        { get; set; }
      string FinNumber
        { get; set; }
      DateTime DateOfServices
        { get; set; }
     string PatID
        { get; set; }
      string ReqNum
        { get; set; }
      string ClientCode
        { get; set; }
      DateTime ScanDate
        { get; set; }
      bool Merged
        { get; set; }
      string MRN
        { get; set; }
    }
}
