using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EDocs.Nyp.LabReqs.AppServices.Models
{
    public class LabReqsIsAdminVeiwAuditLogs
    {
        public bool ViewAuditLogs
        { get; set; }
        public bool IsAdmin
        { get; set; }
        public bool EditLRDocs
        { get; set; }
    }
}
