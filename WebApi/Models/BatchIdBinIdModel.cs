using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BinMonitor.BinInterfaces;
namespace BinMonitorAppService.Models
{
    public class BatchIdBinIdModel: IBatchIdBinId
    {
       public Guid BatchId
        { get; set; }
       public string BinId
        { get; set; }
    }
}
