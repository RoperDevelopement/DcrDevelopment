using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using BinMonitorAppService.Models;
using System.ComponentModel.DataAnnotations;
using BinMonitorAppService.Constants;
using BinMonitorAppService.ApiClasses;

using BinMonitorAppService.Logging;
namespace BinMonitorAppService.Pages.Partial_Pages
{
    public class CategoryDurationsPartialViewModel : PageModel
    {
        private readonly IConfiguration configuration;
        private ILog auditLogs;

        private string WebApiUrl
        { get { return configuration.GetValue<string>("BinMonitorApi").ToString(); } }
      
        public IList<string> EmailTo
        { get; set; }
        public CategoryCheckPointEmailModel CategoryCheckPointEmailModel
        { get; set; }
        public SpectrumMonitorMenuRightsModel SpectrumMonitorMenuRightsModel
        { get; set; }
        public CategoryDurationsPartialViewModel(IConfiguration config, ILog logConfig)
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
            try
            {
               
                ViewData["CWID"] = await GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyCwid);
                if (ViewData["CWID"] == null)
                    return Redirect("/BinUsers/LoginView");

                InitAuditLogs.StartStopWatch();
                GetLogInformaiton().ConfigureAwait(false).GetAwaiter().GetResult();
                auditLogs.LogInformation("Start CategoryDurationsPartialViewModel");
                SpectrumMonitorMenuRightsModel = ApiSetUserProfile.UserProfileInstance.GetBMMenuRights(User.Identity.Name, HttpContext.Session, auditLogs, WebApiUrl).ConfigureAwait(false).GetAwaiter().GetResult();
                GetViewData().ConfigureAwait(true).GetAwaiter().GetResult();

                var qString = Request.QueryString;
            CategoryCheckPointEmailModel = new CategoryCheckPointEmailModel();
            CategoryCheckPointEmailModel.Duration ="00:00";
            if (qString.HasValue)
            {
                    auditLogs.LogInformation($"CategoryDurationsPartialViewModel query string: {qString}");
                }
                auditLogs.LogInformation($"CategoryDurationsPartialViewModel emailto: weburl: {WebApiUrl} controller: {SqlConstants.WebApiBinMonitor}{SqlConstants.SpGetUserInfo}");
                EmailTo = await GetApis.GetApisInctance.ApiUserInfo($"{SqlConstants.WebApiBinMonitor}{SqlConstants.SpGetUserInfo}", WebApiUrl,auditLogs);
                auditLogs.LogInformation($"End CategoryDurationsPartialViewModel total time: {InitAuditLogs.StopStopWatch()} ms");
              
            }
            catch(Exception ex)
            {
                auditLogs.LogInformation($"End CategoryDurationsPartialViewModel total time: {InitAuditLogs.StopStopWatch()} ms {ex.Message}");
                
                return Redirect($"/BinUsers/DisplayErrorMessagesView?ErrMEss=Model CategoryCheckPointModelViewModel OnGetAsync {ex.Message}");
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