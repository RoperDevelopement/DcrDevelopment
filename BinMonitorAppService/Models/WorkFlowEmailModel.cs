using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BinMonitorAppService.Models
{
    public class WorkFlowEmailModel
    {
        public Guid BatchID
        { get; set; }
        public bool EmailOnStart
        { get; set; }

        public bool EmailOnComplete
        { get; set; }
        public bool EmailContents
        { get; set; }

        public string EmailTo
        { get; set; }

    }
}
