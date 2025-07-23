using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using EDocs.Nyp.LabReqs.AppServices.Models;
using EDocs.Nyp.LabReqs.AppServices.ApiClasses;
using EDocs.Nyp.LabReqs.AppServices.LabReqsConstants;
using EDocs.Nyp.LabReqs.AppService.Logging;

namespace EDocs.Nyp.LabReqs.AppServices
{
    public class LookUpMaintenanceLogsViewModel : PageModel
    {
        private ILog log;
        public string StartDate
        { get; set; }
        public string EndDate
        { get; set; }

        public IList<string> LogStation
        { get; private set; }

        public IDictionary<string, MaintenanceLogsModel> IDicMaintenanceLogsLogStation
        { get; private set; }
        private readonly IConfiguration configuration;

        private string WebApiUrl
        { get { return configuration.GetValue<string>("LabResApi").ToString(); } }

        public MaintenanceLogsModel MaintenanceLogsModel
        { get; set; }
        public MaintenanceLogsLogStation LogsLogStation
        { get; set; }
        IList<string> CacheMaintenanceLogsLogStation
        { get; set; }

        public string TotalQueryTime
        { get; set; }
        public LookUpMaintenanceLogsViewModel(IConfiguration config, ILog logConfig)
        {
            configuration = config;
            log = logConfig;
        }
        private async Task GetLogInformaiton()
        {

            log = InitAuditLogs.LogAsync(log, HttpContext.Session).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        public async Task<IActionResult> OnGetAsync(MaintenanceLogsModel MaintenanceLogsModel)
        {


            try
            {
                var qString = Request.QueryString;
                if (!(User.Identity.IsAuthenticated))
                {
                    if(qString.HasValue)
                        return Redirect($"/NypUsersInfo/LoginView?returnUrl=/LabReqs/LookUpMaintenanceLogsView{qString.Value}");
                    else
                    return Redirect("/NypUsersInfo/LoginView?returnUrl=/LabReqs/LookUpMaintenanceLogsView");
                }
                ViewData["CWID"] = GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyCwid).ConfigureAwait(true).GetAwaiter().GetResult();
                if (ViewData["CWID"] == null)
                    return Redirect("/NypUsersInfo/LoginView?returnUrl=/LabReqs/LookUpMaintenanceLogsView");

                LabReqHelpers.StartStopWatch();
                GetLogInformaiton().ConfigureAwait(false).GetAwaiter().GetResult();
                log.LogInformation("Start LookUpMaintenanceLogsViewModel");
                GetViewAudLogsAdmin();
               
                if (qString.HasValue)
                {
                    DateTime dtStTime = DateTime.Now;
                    DateTime dtEndTime = DateTime.Now;
                    if (LabReqHelpers.GetStartEndDate(Request.Query[ConstNypLabReqs.FormValueStartSDate].ToString(), Request.Query[ConstNypLabReqs.FormValueEndSDate].ToString(), ref dtStTime, ref dtEndTime))
                    {
                        MaintenanceLogsModel.LogDate = dtStTime;
                        MaintenanceLogsModel.ScanDate = dtEndTime;
                        StartDate = dtStTime.ToString("yyyy-MM-dd");
                        EndDate = dtEndTime.ToString("yyyy-MM-dd");
                        MaintenanceLogsModel.ScanDate = LabReqHelpers.AddDayToSearchEndDate(MaintenanceLogsModel.ScanDate);
                    }
                    MaintenanceLogsModel.LogStation = Request.Query[ConstNypLabReqs.FormValuePerfLabCode];
                    await GetMLogs(MaintenanceLogsModel, Request.Query[ConstNypLabReqs.FormValueSearchBy]).ConfigureAwait(true);
                }

                CacheMaintenanceLogsLogStation = await GetSessionVariables.SessionVarInstance.GetJsonSessionObject<IList<string>>(HttpContext.Session, "LogsStations").ConfigureAwait(true);
                if (CacheMaintenanceLogsLogStation == null)
                {
                    LogStation = await SpecimenRejectionApi.SpecimenRejectionIntance.NypManLogStations(new Uri($"{WebApiUrl}{ConstNypLabReqs.ApiNypMaintenanceLogsController}", UriKind.RelativeOrAbsolute), $"{string.Format(ConstNypLabReqs.ApiSpecimenRejectionControllerParam, ConstNypLabReqs.SpNypMaintenanceLogStation)}", log).ConfigureAwait(true);
                    
                    CacheMaintenanceLogsLogStation = LogStation;
                    await GetSessionVariables.SessionVarInstance.SetSessionOjbjectAsJson(HttpContext.Session, "LogsStations", CacheMaintenanceLogsLogStation).ConfigureAwait(true);
                }
                else
                {
                    LogStation = CacheMaintenanceLogsLogStation;
                }
                TotalQueryTime = LabReqHelpers.StopStopWatch();
                log.LogInformation($"End LookUpMaintenanceLogsViewModel total time: {TotalQueryTime} ms");
            }
            catch (Exception ex)
            {
                log.LogError($"LookUpMaintenanceLogsViewModel  total time: {LabReqHelpers.StopStopWatch()} ms {ex.Message}");

                return Redirect($"/NypUsersInfo/DisplayErrorMessages?ErrMEss=Model LookUpMaintenanceLogsViewModel OngettAsync {ex.Message}");
            }
            return Page();
        }
        private void GetViewAudLogsAdmin()
        {
            LabReqsIsAdminVeiwAuditLogs IsAdminVeiwAuditLogs = LabReqsConstants.GetSessionVariables.SessionVarInstance.GetJsonSessionObject<LabReqsIsAdminVeiwAuditLogs>(HttpContext.Session, ConstNypLabReqs.SessionIsAdminVeiwAuditLogs).ConfigureAwait(false).GetAwaiter().GetResult();
            ViewData["IsAdmin"] = IsAdminVeiwAuditLogs.IsAdmin;
            ViewData["ViewAuditLogs"] = IsAdminVeiwAuditLogs.ViewAuditLogs;
            ViewData["EditLRDocs"] = IsAdminVeiwAuditLogs.EditLRDocs;
        }
        private async Task GetMLogs(MaintenanceLogsModel MaintenanceLogsModel, string srchLogScanDate)
        {
            string spname = $"{string.Format(ConstNypLabReqs.ApiSpecimenRejectionControllerParam, ConstNypLabReqs.SpNypMaintenanceLogsByLogDate)}";
            if (string.IsNullOrWhiteSpace(srchLogScanDate))
                MaintenanceLogsModel.ScanMachine = ConstNypLabReqs.NA;
            else
                MaintenanceLogsModel.ScanMachine = srchLogScanDate;
            if (((string.IsNullOrWhiteSpace(MaintenanceLogsModel.ScanOperator))) && (string.IsNullOrWhiteSpace(MaintenanceLogsModel.LogStation)))
                spname = $"{string.Format(ConstNypLabReqs.ApiSpecimenRejectionControllerParam, ConstNypLabReqs.SpNypMaintenanceLogsLogDate)}";
            log.LogInformation($"Search Maintenance Logs search by:{MaintenanceLogsModel.ScanMachine} LogStation: {MaintenanceLogsModel.LogStation} ScanMachine: {MaintenanceLogsModel.ScanMachine} Start date: {MaintenanceLogsModel.LogDate.ToString()} End Date: {MaintenanceLogsModel.ScanDate.ToString()}");
            IDicMaintenanceLogsLogStation = await SpecimenRejectionApi.SpecimenRejectionIntance.NypMaintenanceLogs(new Uri($"{WebApiUrl}{ConstNypLabReqs.ApiNypMaintenanceLogsController}", UriKind.RelativeOrAbsolute), spname, MaintenanceLogsModel,log).ConfigureAwait(true);
        }
    }
}