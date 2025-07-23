using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BinMonitorAppService.Constants;
using Microsoft.Extensions.Configuration;
using BinMonitorAppService.ApiClasses;
using BinMonitorAppService.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using BinMonitorAppService.Logging;
namespace BinMonitorAppService.Pages.Manage
{
    public class ViewEmailReportsModel : PageModel
    {
        private readonly IConfiguration configuration;
        private ILog auditLogs;
        public SpectrumMonitorMenuRightsModel SpectrumMonitorMenuRightsModel
        { get; set; }
        private string WebApiUrl
        { get { return configuration.GetValue<string>("BinMonitorApi").ToString(); } }
        public IList<string> EmailTo
        { get; set; }
        public IList<string> EmailCc
        { get; set; }
        public EmailReportModel EmailReportModel
        { get; set; }
        public string SessionExpires
        { get; set; }
        public ViewEmailReportsModel(IConfiguration config, ILog logConfig)
        {
            configuration = config;
            auditLogs = logConfig;
        }
        private async Task GetLogInformaiton()
        {

            auditLogs = InitAuditLogs.LogAsync(auditLogs, HttpContext.Session).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                InitAuditLogs.StartStopWatch();
                GetLogInformaiton().ConfigureAwait(false).GetAwaiter().GetResult();
                auditLogs.LogInformation("Start ViewEmailReportsModel");
                if (!(User.Identity.IsAuthenticated))
                {
                    auditLogs.LogInformation($"End Renew session ViewEmailReportsModel  total time: {InitAuditLogs.StopStopWatch()} ms");
                    return Redirect("/BinUsers/LoginView?returnUrl=/Manage/ViewEmailReports");
                }
                ViewData["CWID"] = await GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyCwid);
                if (ViewData["CWID"] == null)
                    return Redirect("/BinUsers/LoginView?returnUrl=/Manage/ViewEmailReports");
                int totMinutesLeft = ApiSetUserProfile.UserProfileInstance.GetSessionTime(HttpContext.Session, 1).ConfigureAwait(false).GetAwaiter().GetResult();
                auditLogs.LogInformation($"Session time left: {totMinutesLeft.ToString()} ViewEmailReportsModel");
                if (totMinutesLeft < 0)
                {
                    auditLogs.LogInformation($"End Renew session ViewEmailReportsModel  total time: {InitAuditLogs.StopStopWatch()} ms");
                    return Redirect("/BinUsers/LoginView?returnUrl=/Manage/ViewEmailReports");
                }
                SessionExpires = totMinutesLeft.ToString();
                SpectrumMonitorMenuRightsModel = ApiSetUserProfile.UserProfileInstance.GetBMMenuRights(User.Identity.Name, HttpContext.Session, auditLogs, WebApiUrl).ConfigureAwait(false).GetAwaiter().GetResult();
                GetViewData().ConfigureAwait(true).GetAwaiter().GetResult();
                await GetEmailInfo();
                auditLogs.LogInformation($"End ViewEmailReportsModel total time: {InitAuditLogs.StopStopWatch()} ms");

            }
            catch (Exception ex)
            {
                auditLogs.LogError($"ViewEmailReportsModel post total time: {InitAuditLogs.StopStopWatch()} ms {ex.Message}");
                return Redirect($"/BinUsers/DisplayErrorMessagesView?ErrMEss=Model ViewEmailReportsModel OnGetAsync {ex.Message}");


            }
            return Page();

        }
        private async Task GetViewData()
        {
            ViewData[SqlConstants.ViewDataChangeCategories] = SpectrumMonitorMenuRightsModel.Categories;
            ViewData[SqlConstants.ViewDataEmailReports] = SpectrumMonitorMenuRightsModel.EmailReports;
            ViewData[SqlConstants.ViewDataRunReports] = SpectrumMonitorMenuRightsModel.RunReports;

        }
        private async Task GetEmailInfo()
        {
            auditLogs.LogInformation($"ViewEmailReportsModel method GetEmailInfo emailto infor weburl: {WebApiUrl} controller: {SqlConstants.WebApiBinMonitor}{SqlConstants.SpGetUserInfo}");

            EmailTo = await GetApis.GetApisInctance.ApiUserInfo($"{SqlConstants.WebApiBinMonitor}{SqlConstants.SpGetUserInfo}", WebApiUrl, auditLogs);
            auditLogs.LogInformation($"ViewEmailReportsModel method GetEmailInfo emilreport model info weburl: {WebApiUrl} controller: {SqlConstants.WebApiBinMonitor}{SqlConstants.SpEmailReportsUsers}");
            EmailReportModel = await GetApis.GetApisInctance.ApiEmailReportModel(WebApiUrl, $"{SqlConstants.WebApiBinMonitor}{SqlConstants.SpEmailReportsUsers}", auditLogs);
            EmailCc = EmailTo;
            EmailCc.RemoveAt(0);
            if (string.IsNullOrEmpty(EmailReportModel.EmailTo))
            {
                EmailReportModel.EmailTo = "Select Email To";
                EmailTo.Add(EmailReportModel.EmailTo);
                EmailReportModel.EmailFrequency = 0;
            }
            if (string.IsNullOrEmpty(EmailReportModel.EmailCC))
                EmailReportModel.EmailCC = string.Empty;
            else
            {
                EmailCc.Remove("Select Email To");
                foreach (string emailCc in EmailReportModel.EmailCC.Split(";"))
                {
                    EmailCc.Remove(emailCc);
                }
            }
        }

        public async Task<IActionResult> OnPostAsync(EmailReportModel EmailReportModel)
        {
            try
            {
                if (!(User.Identity.IsAuthenticated))
                {
                    return Redirect("/BinUsers/LoginView");
                }
                ViewData["CWID"] = await GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyCwid);
                if (ViewData["CWID"] == null)
                    return Redirect("/BinUsers/LoginView");

                InitAuditLogs.StartStopWatch();

                GetLogInformaiton().ConfigureAwait(false).GetAwaiter().GetResult();
                auditLogs.LogInformation("Start ViewEmailReportsModel on post");
                SpectrumMonitorMenuRightsModel = ApiSetUserProfile.UserProfileInstance.GetBMMenuRights(User.Identity.Name, HttpContext.Session, auditLogs, WebApiUrl).ConfigureAwait(false).GetAwaiter().GetResult();
                GetViewData().ConfigureAwait(true).GetAwaiter().GetResult();
                bool modelValid = true;
                if (!(string.IsNullOrWhiteSpace(Request.Form["EmailCc"])))
                {
                    EmailReportModel.EmailCC = Request.Form["EmailCc"];
                    EmailReportModel.EmailCC = EmailReportModel.EmailCC.Replace("Select Email CC", "").Trim();
                    EmailReportModel.EmailCC = EmailReportModel.EmailCC.Replace(",", ";");
                    auditLogs.LogInformation($"ViewEmailReportsModel on post email cc: {EmailReportModel.EmailCC}");
                }
                if ((int.TryParse(Request.Form["EmailFrequency"], out int results)))
                {
                    EmailReportModel.EmailFrequency = results;
                    if ((results <= 0) || (results > 24))
                    {
                        auditLogs.LogError($"ViewEmailReportsModel on post Invalid email Frequency {EmailReportModel.EmailFrequency} must be between 1 and 24");
                        modelValid = false;
                        ModelState.AddModelError("EmailRepFreq", $"Invalid email Frequency {EmailReportModel.EmailFrequency} must be between 1 and 24");
                    }
                }
                else
                {
                    modelValid = false;
                    auditLogs.LogError($"ViewEmailReportsModel on post Invalid email Frequency {Request.Form["EmailFrequency"]}");
                    ModelState.AddModelError("EmailRepFreq", $"Invalid email Frequency {Request.Form["EmailFrequency"]}");
                }


                if (string.Compare(EmailReportModel.EmailTo, "Select Email To", true) == 0)
                {
                    auditLogs.LogError($"ViewEmailReportsModel on post Invalid EmailTo {EmailReportModel.EmailTo}");
                    ModelState.AddModelError("EmailToRep", $"Invalid EmailTo {EmailReportModel.EmailTo}");
                    modelValid = false;
                }



                if (modelValid)
                {
                    auditLogs.LogInformation($"ViewEmailReportsModel on post add email infor weburl: {WebApiUrl} controller: {SqlConstants.ApiUpdateEmailReports} EmailTo: {EmailReportModel.EmailTo} EmailFrequency: {EmailReportModel.EmailFrequency} ");
                    await PostApis.PostApisIntance.ApiUpdateEmailInfo(WebApiUrl, $"{SqlConstants.ApiUpdateEmailReports}", EmailReportModel, auditLogs);
                    auditLogs.LogInformation($"End ViewEmailReportsModel  on post total time: {InitAuditLogs.StopStopWatch()} ms redirect to ../BinManager/ViewManagerHome");
                    return Redirect("/BinUsers/LoginView");
                }
                else
                    await GetEmailInfo();

                //  ViewData["UserProfile"] = await GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyUserProfile);
                int totMinutesLeft = ApiSetUserProfile.UserProfileInstance.GetSessionTime(HttpContext.Session, 1).ConfigureAwait(false).GetAwaiter().GetResult();
                auditLogs.LogInformation($"Session time left: {totMinutesLeft.ToString()} ViewEmailReportsModel");
                auditLogs.LogInformation($"End ViewEmailReportsModel  on post total time: {InitAuditLogs.StopStopWatch()} ms");
                if (totMinutesLeft < 0)
                {
                    auditLogs.LogInformation($"End Renew session ViewEmailReportsModel  total time: {InitAuditLogs.StopStopWatch()} ms");
                    return Redirect("/BinUsers/LoginView?returnUrl=/Manage/ViewEmailReports");
                }
                SessionExpires = totMinutesLeft.ToString();
            }
            catch (Exception ex)
            {
                auditLogs.LogInformation($"End ViewEmailReportsModel  on post total time: {InitAuditLogs.StopStopWatch()} ms {ex.Message}");
                return Redirect($"/BinUsers/DisplayErrorMessagesView?ErrMEss=Model ViewEmailReportsModel OnGetAsync {ex.Message}");

            }
            return Page();
        }
    }
}