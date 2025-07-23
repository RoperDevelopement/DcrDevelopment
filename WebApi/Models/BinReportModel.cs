using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinMonitor.BinInterfaces;
namespace BinMonitorAppService.Models
{
    public class BinReportModel: IBinReportModel
    {
       public string BinID
        { get; set; }
        public string LabReqNum
        { get; set; }
        public string RegCreatedBY
        { get; set; }
        public string ProcessCreatedBY
        { get; set; }
        public string BinCLosedBY
        { get; set; }

        public string CategoryName
        { get; set; }
        public DateTime LabReqRegStDate
        { get; set; }

        public DateTime LabReqRegEndDate
        { get; set; }

    }
}
