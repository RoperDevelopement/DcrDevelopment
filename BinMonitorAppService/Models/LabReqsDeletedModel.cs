using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BinMonitor.BinInterfaces;

namespace BinMonitorAppService.Models
{
    public class LabReqsDeletedModel : IDelLabReqsReport
    {
        public Guid BatchId
        { get; set; }
        public string BinId
        { get; set; }
       public string LabRecNumber
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
        public string RegDuration
        { get; set; }
        public DateTime DeletedAt
        { get; set; }
        public string DeletedBy
        { get; set; }
    }
}
