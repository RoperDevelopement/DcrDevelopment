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
using BinMonitor.BinInterfaces;
namespace BinMonitorAppService.Pages.LookUp
{
    public class SpecMonitorDelLabReqsRepModel : PageModel
    {
        private readonly IConfiguration configuration;
        private ILog auditLogs;
        private string WebApiUrl
        { get { return configuration.GetValue<string>("BinMonitorApi").ToString(); } }

        [BindProperty]
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime UsageRepStDate
        { get; set; }

        [BindProperty]
        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime UsageRepEndDate
        { get; set; }
        public SpectrumMonitorMenuRightsModel SpectrumMonitorMenuRightsModel
        { get; set; }
        public IDictionary<string, LabReqsDeletedModel> DelLabReqs
        { get; set; }
        public SpecMonitorDelLabReqsRepModel(IConfiguration config, ILog logConfig)
        {

            auditLogs = logConfig;
            configuration = config;

        }

        private async Task GetLogInformaiton()
        {

            auditLogs = InitAuditLogs.LogAsync(auditLogs, HttpContext.Session).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public async Task<ActionResult> OnGetAsync()
        {
            InitAuditLogs.StartStopWatch();
            GetLogInformaiton().ConfigureAwait(false).GetAwaiter().GetResult();
            auditLogs.LogInformation("Start OnGetAsync SpectrumMonitorReportsByCwidViewModel");
            var qstr = Request.QueryString;
            if (!(User.Identity.IsAuthenticated))
            {
                auditLogs.LogInformation($"Renewing session SpecMonitorChartsView on get total time: {InitAuditLogs.StopStopWatch()} ms");
                if (qstr.HasValue)
                    return Redirect($"/BinUsers/LoginViewreturnUrl?returnUrl=/LookUp/DeletedLabReqsView{qstr.Value}");
                else
                    return Redirect("/LookUp/SpecMonitorChartsView?returnUrl=/LookUp/DeletedLabReqsView");
            }
            ViewData["CWID"] = await GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyCwid);
            if (ViewData["CWID"] == null)
            {
                auditLogs.LogInformation($"Renewing session SpecMonitorChartsView on get total time: {InitAuditLogs.StopStopWatch()} ms");
                if (qstr.HasValue)
                    return Redirect($"/BinUsers/LoginViewreturnUrl?returnUrl=/LookUp/DeletedLabReqsView{qstr.Value}");
                else
                    return Redirect("/BinUsers/LoginViewreturnUrl?returnUrl=/LookUp/DeletedLabReqsView");
            }
            try
            {
                if (qstr.HasValue)
                {
                    auditLogs.LogInformation($"OnGetAsync DeletedLabReqsViewModel query string value {qstr.Value}");


                    DateTime std = DateTime.Parse(Request.Query["UsageRepStDate"].ToString());
                    DateTime ed = DateTime.Parse(Request.Query["UsageRepEndDate"].ToString());
                    if (ed.Date.Day == DateTime.Now.Date.Day)
                    {
                        ed = ed.AddDays(1);
                    }

                    auditLogs.LogInformation($"OnGetAsync DeletedLabReqsViewModel getting usage   for weburi {WebApiUrl} sp {SqlConstants.ApiSpecMonitorReportsByCwidController} start date {std} end date {ed} user requesting {User.Identity.Name}");
                    DelLabReqs = GetApis.GetApisInctance.ApiDeletedLabReqsReports(WebApiUrl, SqlConstants.SpGetDeletedLabReqs, SqlConstants.ApiSpecMonitorReportsByCwidController, "na", std, ed, auditLogs).ConfigureAwait(false).GetAwaiter().GetResult();
                    UsageRepStDate = std;
                    UsageRepEndDate = ed;

                }
                else
                {
                    UsageRepStDate = DateTime.Now.AddDays(-10);
                    UsageRepEndDate = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                auditLogs.LogError($"End DeletedLabReqsViewModel total time: {InitAuditLogs.StopStopWatch()} ms");
                auditLogs.LogError($"DeletedLabReqsViewModel on get {ex.Message}");
                return Redirect($"/BinUsers/DisplayErrorMessagesView?ErrMEss=Model DeletedLabReqsViewModel OnGetAsync {ex.Message}");
            }
            ViewData["CWID"] = User.Identity.Name;
            GetViewData().ConfigureAwait(false).GetAwaiter().GetResult();

            auditLogs.LogInformation($"End DeletedLabReqsViewModel total time: {InitAuditLogs.StopStopWatch()} ms");
            return Page();
        }
        private async Task GetViewData()
        {
            string Cwid = User.Identity.Name;
            SpectrumMonitorMenuRightsModel = ApiSetUserProfile.UserProfileInstance.GetBMMenuRights(Cwid, HttpContext.Session, auditLogs, WebApiUrl).ConfigureAwait(false).GetAwaiter().GetResult();
            ViewData[SqlConstants.ViewDataChangeCategories] = SpectrumMonitorMenuRightsModel.Categories;
            ViewData[SqlConstants.ViewDataEmailReports] = SpectrumMonitorMenuRightsModel.EmailReports;
            ViewData[SqlConstants.ViewDataRunReports] = SpectrumMonitorMenuRightsModel.RunReports;

        }
    }
}
