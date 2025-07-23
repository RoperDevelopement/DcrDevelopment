using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
using EDocs.Nyp.LabReqs.AppServices.LabReqInterfaces;


namespace EDocs.Nyp.LabReqs.AppServices.Models
{
    public class AuditLogsModel:IAuditLogs
    {
        public int AuditLogID
        { get; set; }
       public string Cwid
        { get; set; }
       public string AuditLogApplicationName
        { get; set; }
       public string AuditLogMessageType
        { get; set; }
       public DateTime AuditLogDate
        { get; set; }
       public DateTime AuditLogUpLoadDate
        { get; set; }
        public Uri AuditLogUrl
        { get; set; }
    }
}
