using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BinMonitor.BinInterfaces;

namespace BinMonitorAppService.Models
{
    public class TransferReportModel:TransFerModel, ITransFerRep
    {
     public   int ID
        { get; set; }
        public string OldCategoryName
        { get; set; }
       public string OldLabReqNumber
        { get; set; }


       public string OldProcAssignedTo
        { get; set; }
       public string OldRegAssignedTo
        { get; set; }
        public string RegAssignedTo
        { get; set; }
        public DateTime TransferTime
        { get; set; }
    }
}
