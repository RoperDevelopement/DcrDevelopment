using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using EDocs.Nyp.LabReqs.AppServices.Models;
using EDocs.Nyp.LabReqs.AppServices.ApiClasses;
using EDocs.Nyp.LabReqs.AppServices.LabReqsConstants;
using EDocs.Nyp.LabReqs.AppService.Logging;

namespace EDocs.Nyp.LabReqs.AppServices.Pages.NypUsersInfo
{
    public class AuditLogsViewModel : PageModel
    {
        private readonly IConfiguration configuration;
        private ILog log;

        public string TotalQueryTime
        { get; set; }

        public IDictionary<int, AuditLogsModel> AuditLogs
        { get; private set; }

        private Uri WebApiUrl
        { get { return configuration.GetValue<Uri>("LabResApi"); } }

        [BindProperty]
        public string StartDate
        { get; set; }
        [BindProperty]
        public string EndDate
        { get; set; }
        [Display(Name = "Application Name:")]
        public IList<ALogAppName> LALogAppName
        { get; set; }
        [Display(Name = "Log Message:")]
        public IList<ALogMessType> LALogMessageType
        { get; set; }
        [Display(Name = "Cwid:")]
        public IList<ALogCwid> LALogCwid
        { get; set; }
        public AuditLogsViewModel(IConfiguration config, ILog logConfig)
        {
            configuration = config;
            log = logConfig;

        }
        private async Task GetLogInformaiton()
        {

            log = InitAuditLogs.LogAsync(log, HttpContext.Session).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        public async Task<IActionResult> OnGetAsync(DateTime startSDate, DateTime endSDate, string AlCwid = null, string AlAppName = null, string AlErrorType = null)
        {
            var qStr = Request.QueryString;
            if (!(User.Identity.IsAuthenticated))
            {
                if (qStr.HasValue)
                    return Redirect($"/NypUsersInfo/LoginView?returnUrl=/NypUsersInfo/AuditLogsView{qStr.Value}");
                else

                    return Redirect($"/NypUsersInfo/LoginView?returnUrl=/NypUsersInfo/AuditLogsView");

            }
            ViewData["CWID"] = GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyCwid).ConfigureAwait(true).GetAwaiter().GetResult();
            if (ViewData["CWID"] == null)
                return Redirect($"/NypUsersInfo/LoginView?returnUrl=/NypUsersInfo/AuditLogsView");
            try
            {


                LabReqHelpers.StartStopWatch();
                GetLogInformaiton().ConfigureAwait(false).GetAwaiter().GetResult();
                log.LogInformation("Start AuditLogsViewModel");
                if (qStr.HasValue)
                {
                    log.LogInformation($"Start AuditLogsViewModel getting information for query string {qStr.Value}");
                }

                GetAppNames().ConfigureAwait(false).GetAwaiter().GetResult();
                GetMessagesTypes().ConfigureAwait(false).GetAwaiter().GetResult();
                GetCwids().ConfigureAwait(false).GetAwaiter().GetResult();
                GetViewAudLogsAdmin();
                GetAuditLogs(startSDate, endSDate, AlCwid, AlAppName, AlErrorType).ConfigureAwait(false).GetAwaiter().GetResult();
                log.LogInformation($"End AuditLogsViewModel {LabReqHelpers.StopStopWatch()}");
            }
            catch (Exception ex)
            {
                log.LogError($"AuditLogsViewModel {LabReqHelpers.StopStopWatch()} ms {ex.Message}");
                return Redirect($"/NypUsersInfo/DisplayErrorMessages?ErrMEss=Model AuditLogsViewModel OngettAsync {ex.Message}");
            }

            return Page();
        }
        private async Task GetAuditLogs(DateTime sDate, DateTime eDate, string cwids, string appName, string errorType)
        {
            if (sDate.Year > 1)
            {
                log.LogInformation($"Method GetAuditLogs information for Audit log begin date {sDate.Date.ToString()} end date {eDate.Date.ToString()} cwid {cwids} application name {appName} message {errorType} ");
                AuditLogsModel logsModel = new AuditLogsModel();
                if ((!(string.IsNullOrWhiteSpace(appName))) && (string.Compare(appName, ConstNypLabReqs.FormAppNameValueCwid, true) != 0))
                {
                    logsModel.AuditLogApplicationName = appName;
                }
                if ((!(string.IsNullOrWhiteSpace(errorType))) && (string.Compare(errorType, ConstNypLabReqs.FormMessageTypeValue, true) != 0))
                {
                    logsModel.AuditLogMessageType = errorType;
                }
                if ((!(string.IsNullOrWhiteSpace(cwids))) && (string.Compare(cwids, ConstNypLabReqs.FormCBValueCwid, true) != 0))
                {
                    logsModel.Cwid = cwids;
                }
                //if(cwids.Length > 0)
                //{
                //    foreach(string id in cwids)
                //    {
                //        if(string.Compare(id, ConstNypLabReqs.FormCBValueCwid, true) == 0)
                //            continue;
                //        logsModel.Cwid = $"'{id}',";
                //    }
                //    if(logsModel.Cwid.EndsWith(','))
                //    {
                //        logsModel.Cwid = logsModel.Cwid.Remove(logsModel.Cwid.Length-1, 1);
                //    }
                //}
                if (sDate.Year > 1900)
                {

                    logsModel.AuditLogDate = sDate.Date;

                    if (sDate.Date == eDate.Date)
                    {
                        logsModel.AuditLogUpLoadDate = sDate.Date.AddDays(1);
                    }
                    else
                        logsModel.AuditLogUpLoadDate = eDate.Date;

                    StartDate = sDate.ToString("yyyy-MM-dd");
                    EndDate = eDate.ToString("yyyy-MM-dd");
                }
                log.LogInformation($"Method GetAuditLogs calling webapi {WebApiUrl} controller {ConstNypLabReqs.ApiNypAuditLogs}{ConstNypLabReqs.SPNypGetAuditLogs} for Audit log begin date {logsModel.AuditLogDate.Date.ToString()} end date {logsModel.AuditLogUpLoadDate.Date.ToString()} cwid {logsModel.Cwid} application name {logsModel.AuditLogApplicationName} message {logsModel.AuditLogMessageType} ");
                AuditLogs = ApiAuditLogs.NypApiAuditLogsIntance.GetNypAuditLogs(WebApiUrl, $"{ConstNypLabReqs.ApiNypAuditLogs}{ConstNypLabReqs.SPNypGetAuditLogs}", logsModel, log).ConfigureAwait(false).GetAwaiter().GetResult();
            }
        }


        private async Task GetMessagesTypes()
        {
            LALogMessageType = GetSessionVariables.SessionVarInstance.GetJsonSessionObject<IList<ALogMessType>>(HttpContext.Session, "ALCombBoxMessageType").ConfigureAwait(false).GetAwaiter().GetResult();

            if ((LALogMessageType == null) || (LALogMessageType.Count == 0))
            {
                log.LogInformation($"Method GetMessagesTypes calling webapi {WebApiUrl} stored procedure {ConstNypLabReqs.SPNypAuditLogMessageType} controller ConstNypLabReqs.ApiNypAuditLogs");
                LALogMessageType = ApiAuditLogs.NypApiAuditLogsIntance.GetAuditLogsCwid<ALogMessType>($"{ConstNypLabReqs.SPNypAuditLogMessageType}", WebApiUrl, ConstNypLabReqs.ApiNypAuditLogs).ConfigureAwait(false).GetAwaiter().GetResult();
                ALogMessType aLogMessType = new ALogMessType();
                aLogMessType.AuditLogMessageType = ConstNypLabReqs.FormMessageTypeValue;
                LALogMessageType.Insert(0, aLogMessType);
                GetSessionVariables.SessionVarInstance.SetSessionOjbjectAsJson(HttpContext.Session, "ALCombBoxMessageType", LALogMessageType).ConfigureAwait(true).GetAwaiter().GetResult();
            }

        }
        private async Task GetAppNames()
        {
            LALogAppName = GetSessionVariables.SessionVarInstance.GetJsonSessionObject<IList<ALogAppName>>(HttpContext.Session, "ALCombBoxAppName").ConfigureAwait(false).GetAwaiter().GetResult();
            if ((LALogAppName == null) || (LALogAppName.Count == 0))
            {
                // LALogAppName = ApiAuditLogs.NypApiAuditLogsIntance.GetAuditLogsCwid<ALogAppName>($"spName={ConstNypLabReqs.SPNypAuditLogApplicationName}", WebApiUrl, ConstNypLabReqs.ApiNypAuditLogs.Replace("/", "?")).ConfigureAwait(false).GetAwaiter().GetResult();
                log.LogInformation($"Method GetAppNames calling webapi {WebApiUrl} stored procedure {ConstNypLabReqs.SPNypAuditLogApplicationName} controller ConstNypLabReqs.ApiNypAuditLogs");
                LALogAppName = ApiAuditLogs.NypApiAuditLogsIntance.GetAuditLogsCwid<ALogAppName>($"{ConstNypLabReqs.SPNypAuditLogApplicationName}", WebApiUrl, ConstNypLabReqs.ApiNypAuditLogs).ConfigureAwait(false).GetAwaiter().GetResult();
                ALogAppName aLogAppName = new ALogAppName();
                aLogAppName.AuditLogApplicationName = ConstNypLabReqs.FormAppNameValueCwid;
                LALogAppName.Insert(0, aLogAppName);
                GetSessionVariables.SessionVarInstance.SetSessionOjbjectAsJson(HttpContext.Session, "ALCombBoxAppName", LALogAppName).ConfigureAwait(true).GetAwaiter().GetResult();

            }
        }
        private async Task GetCwids()
        {
            LALogCwid = GetSessionVariables.SessionVarInstance.GetJsonSessionObject<IList<ALogCwid>>(HttpContext.Session, "ALCombBoxCwid").ConfigureAwait(false).GetAwaiter().GetResult();
            if ((LALogCwid == null) || (LALogCwid.Count == 0))
            {
                log.LogInformation($"Method GetCwids calling webapi {WebApiUrl} stored procedure {ConstNypLabReqs.SpNypAuditLogCwid} controller ConstNypLabReqs.ApiNypAuditLogs");
                LALogCwid = ApiAuditLogs.NypApiAuditLogsIntance.GetAuditLogsCwid<ALogCwid>($"{ConstNypLabReqs.SpNypAuditLogCwid}", WebApiUrl, ConstNypLabReqs.ApiNypAuditLogs).ConfigureAwait(false).GetAwaiter().GetResult();
                ALogCwid aLogCwid = new ALogCwid();
                aLogCwid.Cwid = ConstNypLabReqs.FormCBValueCwid;
                LALogCwid.Insert(0, aLogCwid);
                GetSessionVariables.SessionVarInstance.SetSessionOjbjectAsJson(HttpContext.Session, "ALCombBoxCwid", LALogCwid).ConfigureAwait(true).GetAwaiter().GetResult();
            }
        }
        private void GetViewAudLogsAdmin()
        {
            LabReqsIsAdminVeiwAuditLogs IsAdminVeiwAuditLogs = LabReqsConstants.GetSessionVariables.SessionVarInstance.GetJsonSessionObject<LabReqsIsAdminVeiwAuditLogs>(HttpContext.Session, ConstNypLabReqs.SessionIsAdminVeiwAuditLogs).ConfigureAwait(false).GetAwaiter().GetResult();
            ViewData["IsAdmin"] = IsAdminVeiwAuditLogs.IsAdmin;
            ViewData["ViewAuditLogs"] = IsAdminVeiwAuditLogs.ViewAuditLogs;
            ViewData["EditLRDocs"] = IsAdminVeiwAuditLogs.EditLRDocs;
        }
    }
}