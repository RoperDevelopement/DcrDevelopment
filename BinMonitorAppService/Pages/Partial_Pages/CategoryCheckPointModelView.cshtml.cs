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
    public class CategoryCheckPointModelViewModel : PageModel
    {
        private readonly IConfiguration configuration;
        private ILog auditLogs;
        private string WebApiUrl
        { get { return configuration.GetValue<string>("BinMonitorApi").ToString(); } }
        public SpectrumMonitorMenuRightsModel SpectrumMonitorMenuRightsModel
        { get; set; }

        public IList<string> EmailTo
        { get; set; }
        
        public CategoryCheckPointModel CategoryCheckPointModel
        { get; set; }
        public CategoryCheckPointEmailModel CategoryCheckPointEmailModel
        { get; set; }

        public CategoryCheckPointModelViewModel(IConfiguration config, ILog logConfig) 
        {
            configuration = config;
            auditLogs = logConfig;
        }
#pragma warning disable 1998
        private async Task GetLogInformaiton()
        {

            auditLogs = InitAuditLogs.LogAsync(auditLogs, HttpContext.Session).ConfigureAwait(false).GetAwaiter().GetResult();
        }


#pragma warning disable 1998
        public async Task<ActionResult> OnGetAsync()
        {
            try
            {
                
                ViewData["CWID"] = await GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyCwid);
                if (ViewData["CWID"] == null)
                    return Redirect("/BinUsers/LoginView");

                InitAuditLogs.StartStopWatch();
                GetLogInformaiton().ConfigureAwait(false).GetAwaiter().GetResult();
                auditLogs.LogInformation("Start CategoryCheckPointModelViewModel on get");
                SpectrumMonitorMenuRightsModel = ApiSetUserProfile.UserProfileInstance.GetBMMenuRights(User.Identity.Name, HttpContext.Session, auditLogs, WebApiUrl).ConfigureAwait(false).GetAwaiter().GetResult();
                GetViewData().ConfigureAwait(true).GetAwaiter().GetResult();
                var qString = Request.QueryString;
                CategoryCheckPointEmailModel = new CategoryCheckPointEmailModel();

                CategoryCheckPointEmailModel.Duration = "00:00";
                if (qString.HasValue)
                {
                    string catName = qString.Value.Substring(qString.Value.IndexOf("=") + 1);
                    auditLogs.LogInformation($"CategoryCheckPointModelViewModel on get query string: {qString.Value}");
                    auditLogs.LogInformation($"CategoryCheckPointModelViewModel on get category checkpoint email model weburl{WebApiUrl} controller: {string.Format(SqlConstants.ApiCategoryDurations, catName, SqlConstants.SpCategoryDurationsModel)} catname:{catName}");
                    CategoryCheckPointEmailModel = await GetApis.GetApisInctance.ApiGetCategoriesDurations(string.Format(SqlConstants.ApiCategoryDurations, catName, SqlConstants.SpCategoryDurationsModel), WebApiUrl, auditLogs);
                    if (string.IsNullOrEmpty(CategoryCheckPointEmailModel.Duration))
                        CategoryCheckPointEmailModel.Duration = "0.0";
                    auditLogs.LogInformation($"CategoryCheckPointModelViewModel on get category checkpoint  model weburl{WebApiUrl} controller: {string.Format(SqlConstants.ApiCategoryDurations, catName, SqlConstants.SpCategoryCheckPointModel)} catname:{catName}");
                    CategoryCheckPointModel = await GetApis.GetApisInctance.ApiGetCategoriesCheckPointModel(string.Format(SqlConstants.ApiCategoryDurations, catName, SqlConstants.SpCategoryCheckPointModel), WebApiUrl, auditLogs);
                    auditLogs.LogInformation($"CategoryCheckPointModelViewModel on get email to weburl:  {WebApiUrl} controller: {SqlConstants.WebApiBinMonitor}{SqlConstants.SpGetUserInfo} catname:{catName}");
                    EmailTo = await GetApis.GetApisInctance.ApiUserInfo($"{SqlConstants.WebApiBinMonitor}{SqlConstants.SpGetUserInfo}", WebApiUrl, auditLogs);
                    await RemoveEmails();
                    auditLogs.LogInformation($"End CategoryCheckPointModelViewModel on get total time: {InitAuditLogs.StopStopWatch()} ms");

                }

                
            }
            catch (Exception ex)
            {
                auditLogs.LogError($"CategoryCheckPointModelViewModel on get total time: {InitAuditLogs.StopStopWatch()} ms {ex.Message}");
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

#pragma warning disable 1998
        private async Task RemoveEmails()
        {
            if (string.IsNullOrWhiteSpace(CategoryCheckPointEmailModel.EmailTo))
                return;
            Task.Factory.StartNew(async () =>
            {
                foreach (string em in CategoryCheckPointEmailModel.EmailTo.Split(';'))
                {
                    if (!(string.IsNullOrWhiteSpace(em)))
                    {
                        if (EmailTo.Contains(em))
                        {
                            auditLogs.LogInformation($"CategoryCheckPointModelViewModel method RemoveEmails remove email: {em}");
                        EmailTo.Remove(em);
                        }
                    }
                }
            }).Wait();
        }
    }
}