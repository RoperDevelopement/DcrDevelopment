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
    public class LookUpSendOutResultsViewModel : PageModel
    {
        private readonly IConfiguration configuration;
        private ILog log;
        public IDictionary<string, SendOutPackingSlipsModel> IDicSendOutPackingSLips
        { get; private set; }

        public string TotalQueryTime
        { get; set; }
        public IList<string> PerformingLab
        { get; private set; }
        private string WebApiUrl
        { get { return configuration.GetValue<string>("LabResApi").ToString(); } }
        [BindProperty]
        public SendOutResultsModel SendOutResultsModel
        { get; set; }
        [BindProperty]
        public IDictionary<string,SendOutResultsModel> IDicSendOutResultsModel
        { get; private set; }

        public string StartDate
        { get; set; }
        public string EndDate
        { get; set; }
       


        public LookUpSendOutResultsViewModel(IConfiguration config, ILog logConfig)
        {
            configuration = config;
            log = logConfig;
        }
        private async Task GetLogInformaiton()
        {
            
            log = InitAuditLogs.LogAsync(log, HttpContext.Session).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public async Task<IActionResult> OnGetAsync(SendOutResultsModel SendOutResultsModel)
        { 
            
            try
            {
                var qString = Request.QueryString;
                if (!(User.Identity.IsAuthenticated))
                {
                    if(qString.HasValue)
                        return Redirect($"/NypUsersInfo/LoginView?returnUrl=/LabReqs/LookUpSendOutResultsView{qString.Value}");
                    
                    else
                        return Redirect($"/NypUsersInfo/LoginView?returnUrl=/LabReqs/LookUpSendOutResultsView");
                    
                }
                ViewData["CWID"] = GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyCwid).ConfigureAwait(true).GetAwaiter().GetResult();
                if (ViewData["CWID"] == null)
                    return new RedirectToPageResult("/Index");



                LabReqHelpers.StartStopWatch();
                GetLogInformaiton().ConfigureAwait(false).GetAwaiter().GetResult();
                log.LogInformation("Start LookUpSendOutResultsViewModel");
                GetViewAudLogsAdmin();
              
            if (qString.HasValue)
            {

                    log.LogInformation($"Class LookUpSendOutResultsViewModel query string: {qString.Value}");
                    DateTime dtStTime = DateTime.Now;
                DateTime dtEndTime = DateTime.Now;
                if (LabReqHelpers.GetStartEndDate(Request.Query[ConstNypLabReqs.FormValueStartSDate].ToString(), Request.Query[ConstNypLabReqs.FormValueEndSDate].ToString(), ref dtStTime, ref dtEndTime))
                {
                    SendOutResultsModel.DateOFService = dtStTime;
                    SendOutResultsModel.ScanDate = dtEndTime;
                    StartDate = dtStTime.ToString("yyyy-MM-dd");
                    EndDate = dtEndTime.ToString("yyyy-MM-dd");
                }
                await LookUpSendOutResults(Request.Query[ConstNypLabReqs.FormValuePerfLabCode], SendOutResultsModel, Request.Query[ConstNypLabReqs.FormValueSearchBy]).ConfigureAwait(true);
            }
                PerformingLab = await GetSessionVariables.SessionVarInstance.GetJsonSessionObject<IList<string>>(HttpContext.Session, "PerfLabSR").ConfigureAwait(true);
            if (PerformingLab == null)
            {
                PerformingLab = await SpecimenRejectionApi.SpecimenRejectionIntance.DOHPerformingLabCodes(new Uri($"{WebApiUrl}{ConstNypLabReqs.ApiNypDOHController}", UriKind.RelativeOrAbsolute), $"{string.Format(ConstNypLabReqs.ApiSpecimenRejectionControllerParam, ConstNypLabReqs.SpSendOutResultsPerformingLabCodes)}",log).ConfigureAwait(true);
                
                await GetSessionVariables.SessionVarInstance.SetSessionOjbjectAsJson(HttpContext.Session, "PerfLabSR", PerformingLab).ConfigureAwait(true);
            }
                TotalQueryTime = LabReqHelpers.StopStopWatch();

                log.LogInformation($"End LookUpSendOutResultsViewModel total time: {TotalQueryTime} ms");
      

                
            }
            catch(Exception ex)
            {
                log.LogInformation($"End LookUpSendOutResultsViewModel total time: {LabReqHelpers.StopStopWatch()} ms");

                return Redirect($"/NypUsersInfo/DisplayErrorMessages?ErrMEss=Model LookUpSendOutResultsViewModel OngettAsync {ex.Message}");

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
        private async Task LookUpSendOutResults(string perfLCode, SendOutResultsModel SendOutResultsModel, string srchLogScanDate)
        {
            SendOutResultsModel.PerformingLabCode = perfLCode;
            if (string.IsNullOrWhiteSpace(srchLogScanDate))
                SendOutResultsModel.ScanMachine = ConstNypLabReqs.NA;
            else
                SendOutResultsModel.ScanMachine = srchLogScanDate;
            string spName = string.Format(ConstNypLabReqs.ApiSpecimenRejectionControllerParam, ConstNypLabReqs.SpNypSendOutResultsByDateOfService);
            log.LogInformation($"Search Send Out Results by:{SendOutResultsModel.ScanMachine} PerformingLabCode: {SendOutResultsModel.PerformingLabCode} AccessionNumber: {SendOutResultsModel.AccessionNumber} FinancialNumber: {SendOutResultsModel.FinancialNumber} FirstName: {SendOutResultsModel.FirstName} LastName:{SendOutResultsModel.LastName}  ScanOperator:{SendOutResultsModel.ScanOperator} Start Date: {SendOutResultsModel.DateOFService.ToString()} End Date: {SendOutResultsModel.ScanDate.ToString()}");
            IDicSendOutResultsModel = await SpecimenRejectionApi.SpecimenRejectionIntance.NypSendOutResults(new Uri($"{WebApiUrl}{ConstNypLabReqs.ApiNypSendOutResultsController}", UriKind.RelativeOrAbsolute), spName, SendOutResultsModel,log).ConfigureAwait(true);

            // DicSpecimenRejectionModel = await SpecimenRejectionApi.SpecimenRejectionIntance.RejectionLogs(WebApiUrl, spName, SpecimenRejection).ConfigureAwait(true);
        }
    }
}