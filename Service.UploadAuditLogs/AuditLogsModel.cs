using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edocs.Service.UploadAuditLogs
{
   public class AuditLogsModel
    {
        public DateTime AuditLogDate
        { get; set; }
        public string AuditLogApplicationName
        { get; set; }
        public Uri AuditLogUrl
        { get; set; }

        public string Cwid
        { get; set; }

        public string AuditLogMessageType
        { get; set; }
    }
}
