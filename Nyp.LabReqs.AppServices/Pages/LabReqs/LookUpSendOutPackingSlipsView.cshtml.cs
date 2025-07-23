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
    public class LookUpSendOutPackingSlipsViewModel : PageModel
    {
        [BindProperty]
        public SendOutPackingSlipsModel PackSlipsModel
        { get; set; }
        private string WebApiUrl
        { get { return configuration.GetValue<string>("LabResApi").ToString(); } }
        public IDictionary<string, SendOutPackingSlipsModel> IDicSendOutPackingSLips
        { get; private set; }
        private readonly IConfiguration configuration;
        private ILog log;

        public string TotalQueryTime
        { get; set; }
        public string StartDate
        { get; set; }
        public string EndDate
        { get; set; }
       
        public LookUpSendOutPackingSlipsViewModel(IConfiguration config, ILog logConfig)
        {
            configuration = config;
            log = logConfig;
        }
        private async Task GetLogInformaiton()
        {

            log = InitAuditLogs.LogAsync(log, HttpContext.Session).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        public async Task<IActionResult> OnGetAsync(SendOutPackingSlipsModel PackSlipsModel)
        {

            try
            {
                var qString = Request.QueryString;
                if (!(User.Identity.IsAuthenticated))
                {
                    if(qString.HasValue)
                        return Redirect($"/NypUsersInfo/LoginView?returnUrl=/LabReqs/LookUpSendOutPackingSlipsView{qString.Value}");
                    else
                    return Redirect("/NypUsersInfo/LoginView?returnUrl=/LabReqs/LookUpSendOutPackingSlipsView");
                }
                ViewData["CWID"] = GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyCwid).ConfigureAwait(true).GetAwaiter().GetResult();
                if (ViewData["CWID"] == null)
                    return Redirect("/NypUsersInfo/LoginView?returnUrl=/LabReqs/LookUpSendOutPackingSlipsView");

                LabReqHelpers.StartStopWatch();
                GetLogInformaiton().ConfigureAwait(false).GetAwaiter().GetResult();
                log.LogInformation("Start LookUpSendOutPackingSlipsViewModel");
                GetViewAudLogsAdmin();
                
                if (qString.HasValue)
                {
                    log.LogInformation($"LookUpSendOutPackingSlipsViewModel query string: {qString.Value}");
                    DateTime dtStTime = DateTime.Now;
                    DateTime dtEndTime = DateTime.Now;
                    if (LabReqHelpers.GetStartEndDate(Request.Query[ConstNypLabReqs.FormValueStartSDate].ToString(), Request.Query[ConstNypLabReqs.FormValueEndSDate].ToString(), ref dtStTime, ref dtEndTime))
                    {
                        PackSlipsModel.DateOfService = dtStTime;
                        PackSlipsModel.ScanDate = dtEndTime;
                        StartDate = dtStTime.ToString("yyyy-MM-dd");
                        EndDate = dtEndTime.ToString("yyyy-MM-dd");
                        PackSlipsModel.ScanDate = LabReqHelpers.AddDayToSearchEndDate(PackSlipsModel.ScanDate);
                    }

                    await GetPackingSlips(PackSlipsModel, Request.Query[ConstNypLabReqs.FormValueSearchBy]).ConfigureAwait(true);
                }



                TotalQueryTime = LabReqHelpers.StopStopWatch();
                log.LogInformation($"End LookUpSendOutPackingSlipsViewModel total time: {TotalQueryTime} ms");
               

                
            }
            catch (Exception ex)
            {
                log.LogError($"LookUpSendOutPackingSlipsViewModel total time: {LabReqHelpers.StopStopWatch()} ms {ex.Message}");

                return Redirect($"/NypUsersInfo/DisplayErrorMessages?ErrMEss=Model LookUpSendOutPackingSlipsViewModel OngettAsync {ex.Message}");
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
        private async Task GetPackingSlips(SendOutPackingSlipsModel PackSlipsModel, string srchLogScanDate)
        {
            string spname = $"{string.Format(ConstNypLabReqs.ApiSpecimenRejectionControllerParam, ConstNypLabReqs.SpNypSendOutPackingSlipsByDateOfService)}";
            if (string.IsNullOrWhiteSpace(srchLogScanDate))
                PackSlipsModel.ScanMachine = ConstNypLabReqs.NA;
            else
                PackSlipsModel.ScanMachine = srchLogScanDate;
            log.LogInformation($"Search Send OutPacking Slips by:{PackSlipsModel.ScanMachine} ScanOperator: {PackSlipsModel.ScanOperator} Start Date: {PackSlipsModel.DateOfService.ToString()} End Date: {PackSlipsModel.ScanDate.ToString()}");
            IDicSendOutPackingSLips = await SpecimenRejectionApi.SpecimenRejectionIntance.NypSendOutPackingSlips(new Uri($"{WebApiUrl}{ConstNypLabReqs.ApiSendOutPackingSlipsController}", UriKind.RelativeOrAbsolute), spname, PackSlipsModel,log).ConfigureAwait(true);
        }
    }
}