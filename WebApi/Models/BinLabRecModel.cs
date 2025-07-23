using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinMonitor.BinInterfaces;
namespace BinMonitorAppService.Models
{
   public class BinLabRecModel : IActiveBins
    {
       public string BinID
        { get; set; }

       

        public bool AcvitveBin
        { get; set; }

        public DateTime StartTime
        { get; set; }

        public DateTime ProcessSt
        { get; set; }
       public string CategoryName
        { get; set; }


       public string CategoryColor
        { get; set; }

       public int CategoryDurationHrs
        { get; set; }

        public string BinAssignedTo
        { get; set; }

        public bool BinRegStarted
        { get; set; }
        public bool BinRegCompleted
        { get; set; }
        public bool BinProcessCompleted
        { get; set; }
        public bool BinProcessStarted
        { get; set; }

        public int Total
        { get; set; }
        public bool Flash
        { get; set; }
    }
}


