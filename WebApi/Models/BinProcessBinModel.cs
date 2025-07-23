using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BinMonitor.BinInterfaces;

namespace BinMonitorAppService.Models
{
    public class BinProcessBinModel: IBinProcessBin
    {
      public string ProcessAssignedBy
        { get; set; }
        public string ProcessAssignedTo
        { get; set; }
        public DateTime ProcessStartAt
        { get; set; }
        public string ProcessCompletedBy
        { get; set; }
        public DateTime ProcessCompletedAt
        { get; set; }
        public string ProcessDuration
        { get; set; }
    }

}
