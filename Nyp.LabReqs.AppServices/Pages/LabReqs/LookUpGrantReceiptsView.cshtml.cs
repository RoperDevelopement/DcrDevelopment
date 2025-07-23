using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using EDocs.Nyp.LabReqs.AppServices.Models;
using EDocs.Nyp.LabReqs.AppServices.ApiClasses;
using EDocs.Nyp.LabReqs.AppServices.LabReqsConstants;

using EDocs.Nyp.LabReqs.AppService.Logging;
namespace EDocs.Nyp.LabReqs.AppServices
{
    public class LookUpGrantReceiptsViewModel : PageModel
    {
        private readonly IConfiguration configuration;
        private ILog log;
        public string StartDate
        { get; set; }
        public string EndDate
        { get; set; }
        public IDictionary<string, GrantReceiptsModel> IDicGrantReceiptsModel
        { get; private set; }
        public GrantReceiptsModel GrantReceiptsModel
        { get; private set; }
        private string WebApiUrl
        { get { return configuration.GetValue<string>("LabResApi").ToString(); } }

        public string TotalQueryTime
        { get; set; }


        public LookUpGrantReceiptsViewModel(IConfiguration config, ILog logConfig)
        {
            configuration = config;
            log = logConfig;
        }
        private async Task GetLogInformaiton()
        {

            log = InitAuditLogs.LogAsync(log, HttpContext.Session).ConfigureAwait(false).GetAwaiter().GetResult();
        }



        public async Task<IActionResult> OnGetAsync(GrantReceiptsModel GrantReceiptsModel)
        {
            var qString = Request.QueryString;
            try
            {
                if (!(User.Identity.IsAuthenticated))
                {
                    if (qString.HasValue)
                    {
                        return Redirect($"/NypUsersInfo/LoginView?returnUrl=/LabReqs/LookUpGrantReceiptsView{qString.Value}");
                    }
                        else
                    return Redirect("/NypUsersInfo/LoginView?returnUrl=/LabReqs/LookUpGrantReceiptsView");
                }
               
                ViewData["CWID"] = GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyCwid).ConfigureAwait(true).GetAwaiter().GetResult();
                if (ViewData["CWID"] == null)
                    return Redirect("/NypUsersInfo/LoginView?returnUrl=/LabReqs/LookUpGrantReceiptsView");

                LabReqHelpers.StartStopWatch();
                GetLogInformaiton().ConfigureAwait(false).GetAwaiter().GetResult();

                log.LogInformation("Start LookUpGrantReceiptsViewModel");
                GetViewAudLogsAdmin();
                if (qString.HasValue)
                {
                    log.LogInformation($"LookUpGrantReceiptsViewModel query string: {qString.Value}");
                    if ((string.IsNullOrWhiteSpace(GrantReceiptsModel.ClientCode)) && (string.IsNullOrWhiteSpace(GrantReceiptsModel.ScanOperator)))
                    {
                        log.LogWarning("LookUpGrantReceiptsViewModel Need at least Client Code, * for all Client Code, or Scan Operator.");
                        ModelState.AddModelError("ErrorMessage", $"Need at least Client Code, * for all Client Code, or Scan Operator.");
                    }
                    else if ((!(string.IsNullOrWhiteSpace(GrantReceiptsModel.ClientCode))) && (!(string.IsNullOrWhiteSpace(GrantReceiptsModel.ScanOperator))))
                    {
                        log.LogWarning("LookUpGrantReceiptsViewModel Can only have Client Code or * for all Client Code, or  Scan Operator.");
                        ModelState.AddModelError("ErrorMessage", $"Can only have Client Code or * for all Client Code, or  Scan Operator.");
                    }

                    else
                    {
                        GrantReceiptsModel = GetStartEndDate(GrantReceiptsModel);
                        if (GrantReceiptsModel.DocumentDate.Year > 2000)
                        {
                            StartDate = GrantReceiptsModel.DocumentDate.ToString("yyyy-MM-dd");
                            EndDate = GrantReceiptsModel.ScanDate.ToString("yyyy-MM-dd");
                            GrantReceiptsModel.ScanDate = LabReqHelpers.AddDayToSearchEndDate(GrantReceiptsModel.ScanDate);
                        }
                        GrantReceiptsModel.ScanByDate = Request.Query[ConstNypLabReqs.FormValueSearchBy].ToString();
                        if (string.IsNullOrWhiteSpace(GrantReceiptsModel.ScanByDate))
                            GrantReceiptsModel.ScanByDate = ConstNypLabReqs.FormValueSearchByLogScanDate;
                        log.LogInformation($"Searach Grant Receipts by ClientCode: {GrantReceiptsModel.ClientCode} ScanOperator: {GrantReceiptsModel.ScanOperator} start date: {GrantReceiptsModel.DocumentDate.ToString()} end date: {GrantReceiptsModel.ScanDate.ToString()}");
                        IDicGrantReceiptsModel = await SpecimenRejectionApi.SpecimenRejectionIntance.GranteReceipts(WebApiUrl, ConstNypLabReqs.ApiNypGrantReceiptsController, GrantReceiptsModel, log).ConfigureAwait(true);
                    }
                }
                TotalQueryTime = LabReqHelpers.StopStopWatch();
                log.LogInformation($"End LookUpGrantReceiptsViewModel total time: {TotalQueryTime} ms");
               

            }
            catch (Exception ex)
            {
                log.LogError($"LookUpGrantReceiptsViewModel {LabReqHelpers.StopStopWatch()} ms {ex.Message}");
                return Redirect($"/NypUsersInfo/DisplayErrorMessages?ErrMEss=Model LookUpGrantReceiptsViewModel OngettAsync {ex.Message}");
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


        private GrantReceiptsModel GetStartEndDate(GrantReceiptsModel receiptsModel)
        {
            string seDate = Request.Query[ConstNypLabReqs.FormValueStartSDate].ToString().Trim();
            if (!(string.IsNullOrWhiteSpace(seDate)))
            {

                receiptsModel.DocumentDate = GetNypLabReqs.NypLabReqsApisInctance.GetLabRecDate(seDate);
                seDate = Request.Query[ConstNypLabReqs.FormValueEndSDate].ToString();

                receiptsModel.ScanDate = GetNypLabReqs.NypLabReqsApisInctance.GetLabRecDate(seDate);
            }
            return receiptsModel;
        }
    }
}