using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BinMonitorAppService.Models;
using BinMonitorAppService.Constants;
using Microsoft.AspNetCore.Hosting;

using Microsoft.AspNetCore.Identity;
using System.Net.Http;
using System.Security.Claims;
using Microsoft.Net.Http;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Configuration;
using BinMonitorAppService.ApiClasses;
using Microsoft.AspNetCore.Http;
using BinMonitorAppService.Logging;
using Microsoft.AspNetCore.Authorization;

namespace BinMonitorAppService.Pages.Partial_Pages
{
    public class ReportTransferModel : PageModel
    {
    
        private readonly IConfiguration configuration;
        private ILog auditLogs;
     public   IDictionary<int, TransferReportModel> DicRepTrans
        { get; set; }
        private string WebApiUrl
        { get { return configuration.GetValue<string>("BinMonitorApi").ToString(); } }

        public ReportTransferModel(IConfiguration config, ILog logConfig)
        {
            configuration = config;

            auditLogs = logConfig;

        }
        private async Task GetLogInformaiton()
        {

            auditLogs = InitAuditLogs.LogAsync(auditLogs, HttpContext.Session).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        public async Task<IActionResult> OnGetAsync(DateTime repSDate, DateTime repEDate, string transType)
        {
            ViewData["CWID"] = User.Identity.Name;
            try
            {
                InitAuditLogs.StartStopWatch();
                GetLogInformaiton().ConfigureAwait(false).GetAwaiter().GetResult();
                auditLogs.LogInformation("Start  TransferPartialViewModel OnGetAsync");
                var qString = Request.QueryString;
                if((repEDate.Year > 2000) && (!(string.IsNullOrWhiteSpace(transType))))
                {
                    auditLogs.LogInformation($"CWID {User.Identity.Name} Running transfer report for query string {qString.Value}");
                    //    DateTime repSDate = DateTime.Parse(Request.Query["repSDate"].ToString());
                    //    DateTime repEDate = DateTime.Parse(Request.Query["repEDate"].ToString());
                    //    string transType = Request.Query["transType"].ToString();
                    DicRepTrans = GetApis.GetApisInctance.TransferRep(WebApiUrl, SqlConstants.ApiSpecMonitorLabReqsController, repSDate, repEDate, transType, auditLogs).ConfigureAwait(false).GetAwaiter().GetResult();
                 }
            }
            catch (Exception ex)
            {
                auditLogs.LogError($"End TransferPartialViewModel OnGetAsync total time: {InitAuditLogs.StopStopWatch()} ms {ex.Message}");
                return Redirect($"/BinUsers/DisplayErrorMessagesView?ErrMEss=Model TransferPartialViewModel OnGetAsync {ex.Message}");

            }
            return Page();
        }
    }
}
