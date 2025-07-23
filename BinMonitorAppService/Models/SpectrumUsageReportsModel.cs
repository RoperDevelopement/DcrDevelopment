using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BinMonitor.BinInterfaces;


namespace BinMonitorAppService.Models
{
    public class SpectrumUsageReportsModel: IUsageReport
    {
      public  string BinID
        { get; set; }
        public string CWID
        { get; set; }

        public string CategoryName
        { get; set; }

        public int TotalLabReqs
       { get; set; }
    }
}
