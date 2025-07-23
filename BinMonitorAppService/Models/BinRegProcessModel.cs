using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BinMonitor.BinInterfaces;
namespace BinMonitorAppService.Models
{
    public class BinRegProcessModel:IBinRegistration,IBinProcessBin,IBinClosed
    {
       public Guid BatchID
        { get; set; }
        public string BinID
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
        public string RegDuration
        { get; set; }
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

        public string BinClosedBy
        { get; set; }
        public DateTime BinCompletedAt
        { get; set; }
        public DateTime ClosedCreatedAt
        { get; set; }
        public string CompleteDuration
        { get; set; }
        
        public string BinComments
        { get; set; }
        public string BinContents
        { get; set; }
        public string CategoryName
        { get; set; }

        public string RegProcesClose
        { get; set; }

        public string LabRecNumber
        { get; set; }
        public DateTime DateBatchPickedUp
        { get; set; }
       public string BatchPickedUpBy
        { get; set; }
    }
}
