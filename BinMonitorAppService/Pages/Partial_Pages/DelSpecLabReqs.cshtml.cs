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
    public class DelSpecLabReqsModel : PageModel
    {
        private readonly IConfiguration configuration;
        private ILog auditLogs;
        private string WebApiUrl
        { get { return configuration.GetValue<string>("BinMonitorApi").ToString(); } }
        public DelSpecLabReqsModel(IConfiguration config, ILog logConfig)
        {
            configuration = config;

            auditLogs = logConfig;

        }
        private async Task GetLogInformaiton()
        {

            auditLogs = InitAuditLogs.LogAsync(auditLogs, HttpContext.Session).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        public async Task<ActionResult> OnGetAsync()
        {
            var qstr = Request.QueryString;
            GetLogInformaiton().ConfigureAwait(false).GetAwaiter().GetResult();
            auditLogs.LogInformation("Start OnGetAsync DelSpecLabReqsModel");
            try
            {
               if (qstr.HasValue)
                {
                    auditLogs.LogInformation($"Processing query string {qstr.Value}for CWID {User.Identity.Name}");
                    string labReq = Request.Query["labReq"].ToString();
                    string binID = Request.Query["bindID"].ToString();
                    string selCatName = Request.Query["selCatName"].ToString();
                    string returnurl = $"{Request.Query["returnurl"].ToString()}&selCatName={selCatName}&delRec=true";
                    auditLogs.LogInformation($"CWID {User.Identity.Name} Deleteing labreq {labReq} for binid {binID}");

                    PostApis.PostApisIntance.ApiDelLabReq(WebApiUrl, SqlConstants.WebApiBinMonitor, labReq, User.Identity.Name, auditLogs).ConfigureAwait(false).GetAwaiter().GetResult();
                    auditLogs.LogInformation($"Return url {returnurl} ms");
                    auditLogs.LogInformation($"End DelSpecLabReqsModel total time: {InitAuditLogs.StopStopWatch()} ms");
                    return Redirect($"{returnurl}");
                }
            }
            catch (Exception ex)
            {
                auditLogs.LogError($"DelSpecLabReqsModel {ex.Message}");
                auditLogs.LogError($"End DelSpecLabReqsModel total time: {InitAuditLogs.StopStopWatch()} ms");
                return Redirect($"/BinUsers/DisplayErrorMessagesView?ErrMEss=Model ViewBMCreateBatchModel OnGetAsync {ex.Message}");

            }

            return Page();
        }
    }
}
