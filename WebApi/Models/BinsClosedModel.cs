 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BinMonitor.BinInterfaces;
namespace BinMonitorAppService.Models
{
    public class BinsClosedModel:IBinClosed
    {
       public string BinClosedBy
        { get; set; }
        public DateTime BinCompletedAt
        { get; set; }
        public DateTime ClosedCreatedAt
        { get; set; }
        public string CompleteDuration
        { get; set; }
      public DateTime DateBatchPickedUp
        { get; set; }

        public string BatchPickedUpBy
        { get; set; }
    }
}
