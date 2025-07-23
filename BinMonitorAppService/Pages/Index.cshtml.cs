using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using BinMonitorAppService.Constants;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Data;
using System.Data.SqlClient;
using BinMonitorAppService.ApiClasses;
using BinMonitorAppService.Logging;
using BinMonitorAppService.Models;
using Microsoft.AspNetCore.Diagnostics;
namespace BinMonitorAppService.Pages
{
    public class IndexModel : PageModel
    {
        
        private IHttpContextAccessor accessor;

        private ILog auditLogs;
        public string Cwid
        { get; set; }

        public string UserProfile
        { get; set; }

        public List<string> Users
        { get; set; }
        private string WebApiUrl
        { get { return configuration.GetValue<string>("BinMonitorApi").ToString(); } }
        private readonly IConfiguration configuration;

        public SpectrumMonitorMenuRightsModel SpectrumMonitorMenuRightsModel
        { get; set; }



        public IndexModel(IConfiguration config, ILog logConfig, IHttpContextAccessor httpContextAccessor)
        {

            configuration = config;
            auditLogs = logConfig;
            accessor = httpContextAccessor;
        }
        private async Task GetLogInformaiton()
        {

            auditLogs = InitAuditLogs.LogAsync(auditLogs, HttpContext.Session).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                if (!(User.Identity.IsAuthenticated))
                {
                    return Redirect("/BinUsers/LoginView");
                }
                Cwid = User.Identity.Name;
                InitAuditLogs.StartStopWatch();
                GetLogInformaiton().ConfigureAwait(false).GetAwaiter().GetResult();
                auditLogs.LogInformation("Start IndexModel");
                ViewData["CWID"] = User.Identity.Name;
                string totalQueryTime = InitAuditLogs.StopStopWatch();
                var clientIPAddress = accessor.HttpContext.Connection.RemoteIpAddress.ToString();
                auditLogs.LogInformation($"User: {User.Identity.Name} Logged in from ipdaaress {clientIPAddress} ");
                auditLogs.LogInformation($"End IndexModel total time: {totalQueryTime} ms");
                SpectrumMonitorMenuRightsModel = ApiSetUserProfile.UserProfileInstance.GetBMMenuRights(User.Identity.Name, HttpContext.Session, auditLogs, WebApiUrl).ConfigureAwait(false).GetAwaiter().GetResult();
				 GetViewData().ConfigureAwait(false).GetAwaiter().GetResult();
                
            }
            catch (Exception ex)
            {
                auditLogs.LogError($"End LookUpDrCodesViewModel total time: {InitAuditLogs.StopStopWatch()} ms {ex.Message}");
                RedirectToPage("/Error", $"ErrMEss=Model IndexModel {ex.Message}");

            }
            return Page();
        }

          private async Task GetViewData()
        {
            ViewData[SqlConstants.ViewDataChangeCategories] = SpectrumMonitorMenuRightsModel.Categories;
            ViewData[SqlConstants.ViewDataEmailReports] = SpectrumMonitorMenuRightsModel.EmailReports;
            ViewData[SqlConstants.ViewDataRunReports] = SpectrumMonitorMenuRightsModel.RunReports;

        }

    }
}
