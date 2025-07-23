using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using BinMonitorAppService.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using BinMonitorAppService.Constants;
using BinMonitorAppService.ApiClasses;
using BinMonitorAppService.Logging;
namespace BinMonitorAppService.Pages.Manage
{
    public class ViewSpectrumSettingsModel : PageModel
    {
        private readonly IConfiguration configuration;
        private ILog auditLogs;
        public SpectrumMonitorSettings SpectrumMonitorSettings
        { get; set; }
        public SpectrumMonitorMenuRightsModel SpectrumMonitorMenuRightsModel
        { get; set; }
        private string WebApiUrl
        { get { return configuration.GetValue<string>("BinMonitorApi").ToString(); } }

        public string SessionExpires
        { get; set; }
        public ViewSpectrumSettingsModel(IConfiguration config, ILog logConfig)
        {

            configuration = config;
            auditLogs = logConfig;
        }
        private async Task GetLogInformaiton()
        {

            auditLogs = InitAuditLogs.LogAsync(auditLogs, HttpContext.Session).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public async Task<ActionResult> OnGet()
        {
            try
            {
                InitAuditLogs.StartStopWatch();
                GetLogInformaiton().ConfigureAwait(false).GetAwaiter().GetResult();
                auditLogs.LogInformation("Start ViewSpectrumSettingsModel on get");
                if (!(User.Identity.IsAuthenticated))
                {
                    auditLogs.LogInformation($"Renewings session ViewSpectrumSettingsModel on get total time: {InitAuditLogs.StopStopWatch()} ms");
                    return Redirect("/BinUsers/LoginView?returnUrl=/Manage/ViewSpectrumSettings");
                }
                ViewData["CWID"] = await GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyCwid);
                if (ViewData["CWID"] == null)
                    return Redirect("/BinUsers/LoginView");

                int totMinutesLeft = ApiSetUserProfile.UserProfileInstance.GetSessionTime(HttpContext.Session, 1).ConfigureAwait(false).GetAwaiter().GetResult();
                auditLogs.LogInformation($"Session time left: {totMinutesLeft.ToString()}");
                if (totMinutesLeft < 0)
                {
                    auditLogs.LogInformation($"Renewings session ViewSpectrumSettingsModel on get total time: {InitAuditLogs.StopStopWatch()} ms");
                    return Redirect("/BinUsers/LoginView?returnUrl=/Manage/ViewSpectrumSettings");
                }
                SessionExpires = totMinutesLeft.ToString();
                SpectrumMonitorMenuRightsModel = ApiSetUserProfile.UserProfileInstance.GetBMMenuRights(User.Identity.Name, HttpContext.Session, auditLogs, WebApiUrl).ConfigureAwait(false).GetAwaiter().GetResult();
                GetViewData().ConfigureAwait(true).GetAwaiter().GetResult();
                auditLogs.LogInformation($"ViewSpectrumSettingsModel getting spec monitor setting for weburl: {WebApiUrl} controller: {SqlConstants.WebApiBinMonitor}{SqlConstants.SPGetSpecSettings} ");
                SpectrumMonitorSettings = await GetApis.GetApisInctance.GetSpecumMonitorSettigs(WebApiUrl, $"{SqlConstants.WebApiBinMonitor}{SqlConstants.SPGetSpecSettings}", auditLogs);
                //  ViewData["UserProfile"] = await GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyUserProfile);
                SpectrumMonitorSettings.NotUploadFolder = SpectrumMonitorSettings.NotUploadFolder.Replace("{Working}", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
                SpectrumMonitorSettings.WorkingFolder = SpectrumMonitorSettings.WorkingFolder.Replace("{Working}", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
                SpectrumMonitorSettings.UpDateFolder = SpectrumMonitorSettings.UpDateFolder.Replace("{Working}", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
                auditLogs.LogInformation($"End ViewSpectrumSettingsModel on get total time: {InitAuditLogs.StopStopWatch()} ms");

            }
            catch (Exception ex)
            {
                auditLogs.LogError($"ViewSpectrumSettingsModel on get total time: {InitAuditLogs.StopStopWatch()} ms {ex.Message}");
                return Redirect($"/BinUsers/DisplayErrorMessagesView?ErrMEss=Model SpectrumSettingsModel OnGetAsync {ex.Message}");


            }
            return Page();
        }
        public async Task<ActionResult> OnPostAsync(SpectrumMonitorSettings SpectrumMonitorSettings)
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
                auditLogs.LogInformation("Start ViewSpectrumSettingsModel post");
                SpectrumMonitorMenuRightsModel = ApiSetUserProfile.UserProfileInstance.GetBMMenuRights(User.Identity.Name, HttpContext.Session, auditLogs, WebApiUrl).ConfigureAwait(false).GetAwaiter().GetResult();
                GetViewData().ConfigureAwait(true).GetAwaiter().GetResult();
                auditLogs.LogInformation($"ViewSpectrumSettingsModel post update spec mon settings weburl:{WebApiUrl} controller:{string.Format(SqlConstants.ApiSpecMonitorUpdateSettings, SqlConstants.SpUpdateSpectrumMonitorSettings)}");
                await BinsInformation.BinsApisInctance.UpDateSpecMonSettings(SpectrumMonitorSettings, WebApiUrl, $"{string.Format(SqlConstants.ApiSpecMonitorUpdateSettings, SqlConstants.SpUpdateSpectrumMonitorSettings)}", auditLogs).ConfigureAwait(true);

                // ViewData["UserProfile"] = await GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyUserProfile);
                int totMinutesLeft = ApiSetUserProfile.UserProfileInstance.GetSessionTime(HttpContext.Session, 1).ConfigureAwait(false).GetAwaiter().GetResult();
                auditLogs.LogInformation($"Session time left: {totMinutesLeft.ToString()}");
                auditLogs.LogInformation($"End ViewSpectrumSettingsModel on post total time: {InitAuditLogs.StopStopWatch()} ms");
                if (totMinutesLeft < 0)
                {
                    auditLogs.LogInformation($"Renewings session ViewSpectrumSettingsModel on get total time: {InitAuditLogs.StopStopWatch()} ms");
                    return Redirect("/BinUsers/LoginView?returnUrl=/Manage/ViewSpectrumSettings");
                }
                SessionExpires = totMinutesLeft.ToString();
            }
            catch (Exception ex)
            {
                auditLogs.LogError($"ViewSpectrumSettingsModel on post total time: {InitAuditLogs.StopStopWatch()} ms {ex.Message}");
                return Redirect($"/BinUsers/DisplayErrorMessagesView?ErrMEss=Model SpectrumSettingsModel OnGetAsync {ex.Message}");
            }
            return Page();
        }

        public async Task<ActionResult> OnPutAsync(SpectrumMonitorSettings SpectrumMonitorSettings)
        {
            try
            {
                if (!(User.Identity.IsAuthenticated))
                {
                    return Redirect("/BinUsers/LoginView?returnUrl=/Manage/ViewSpectrumSettings");
                }
                ViewData["CWID"] = await GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyCwid);
                if (ViewData["CWID"] == null)
                    return Redirect("/BinUsers/LoginView?returnUrl=/Manage/ViewSpectrumSettings");
                InitAuditLogs.StartStopWatch();
                GetLogInformaiton().ConfigureAwait(false).GetAwaiter().GetResult();
                auditLogs.LogInformation("Start ViewSpectrumSettingsModel post");
                auditLogs.LogInformation($"ViewSpectrumSettingsModel post update spec mon settings weburl:{WebApiUrl} controller:{string.Format(SqlConstants.ApiSpecMonitorUpdateSettings, SqlConstants.SpUpdateSpectrumMonitorSettings)}");
                await BinsInformation.BinsApisInctance.UpDateSpecMonSettings(SpectrumMonitorSettings, WebApiUrl, $"{string.Format(SqlConstants.ApiSpecMonitorUpdateSettings, SqlConstants.SpUpdateSpectrumMonitorSettings)}", auditLogs).ConfigureAwait(true);
                auditLogs.LogInformation($"End ViewSpectrumSettingsModel on post total time: {InitAuditLogs.StopStopWatch()} ms");
            }
            catch (Exception ex)
            {
                auditLogs.LogError($"ViewSpectrumSettingsModel on post total time: {InitAuditLogs.StopStopWatch()} ms {ex.Message}");
                return Redirect($"/BinUsers/DisplayErrorMessagesView?ErrMEss=Model SpectrumSettingsModel OnGetAsync {ex.Message}");
            }
            return Redirect("/BinUsers/LoginView?returnUrl=/Manage/ViewSpectrumSettings");
        }
        private async Task GetViewData()
        {
            ViewData[SqlConstants.ViewDataChangeCategories] = SpectrumMonitorMenuRightsModel.Categories;
            ViewData[SqlConstants.ViewDataEmailReports] = SpectrumMonitorMenuRightsModel.EmailReports;
            ViewData[SqlConstants.ViewDataRunReports] = SpectrumMonitorMenuRightsModel.RunReports;

        }
    }
}