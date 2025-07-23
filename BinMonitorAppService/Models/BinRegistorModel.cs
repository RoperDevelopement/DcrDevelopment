using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BinMonitor.BinInterfaces;
namespace BinMonitorAppService.Models
{
    public class BinRegistorModel: IBinRegistration,IBinContentsComments
    {
      public  Guid BatchID
        { get; set; }
        public string BinID
        { get; set; }
        public string CategoryName
        { get; set; }
        public string RegCreatedBy
        { get; set; }

        public string RegAssignedBy
        { get; set; }

        public string RegAssignedTo
        { get; set; }
        public DateTime RegStartedAt
        { get; set; }
        public string RegCompletedBy
        { get; set; }
        public DateTime RegCompletedAt
        { get; set; }
        public string BinComments
        { get; set; }
       public string BinContents
        { get; set; }
        public BinProcessBinModel BinProcessBinModel
        { get; set; }
        public BinsClosedModel BinsClosedModel
        { get; set; }
        public string RegDuration
        { get; set; }
        public string RegProcesClose
        { get; set; }

        public string LabRecNumber
        { get; set; }
    }
}
