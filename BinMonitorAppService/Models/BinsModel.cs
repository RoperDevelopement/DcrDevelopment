using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BinMonitorAppService.Models
{
    public class BinsModel
    {
        public string Bin
        { get; set; }
          public DateTime StartTime
      // public string StartTime
        { get; set; }
        public string Category
        {
            get; set;
        }

        public string BinAssignedTo
        { get; set; }

        public bool AcvitveBin
        {
            get; set;
        }
        public bool BinRegStarted
        { get; set; }
        public bool BinRegCompleted
        { get; set; }
        public bool BinProcessCompleted
        { get; set; }
        public bool BinProcessStarted
        { get; set; }

        public string CategoryColor
        { get; set; }

        public string LabRecNumber
        { get; set; }
      //  public bool Flash
      // { get; set; }
    }
}
