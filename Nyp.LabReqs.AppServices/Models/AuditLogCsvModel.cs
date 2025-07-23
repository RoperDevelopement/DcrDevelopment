using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EDocs.Nyp.LabReqs.AppServices.LabReqInterfaces;

namespace EDocs.Nyp.LabReqs.AppServices.Models
{
    public class AuditLogCsvModel:IAuditLogsCsv
    {
       public string Cwid
        { get; set; }
        public string AuditLogApplicationName
        { get; set; }
        public string AuditLogMessageType
        { get; set; }
        public string AuditLogDate
        { get; set; }

        public string AuditLogMessage
        { get; set; }

        public string AuditLogMachine
        { get; set; }
        public string AuditLogDomain
        { get; set; }
    }
}
