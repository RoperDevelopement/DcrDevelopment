using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinMonitor.BinInterfaces;

namespace BinMonitorAppService.Models
{
    
    public class ReportVolDurModel : IUsageReportByVolDur
    {
       public string CWID
        { get; set; }

        public int TotalVolume
        { get; set; }

        public string TotalDur
        { get; set; }
    }
}
