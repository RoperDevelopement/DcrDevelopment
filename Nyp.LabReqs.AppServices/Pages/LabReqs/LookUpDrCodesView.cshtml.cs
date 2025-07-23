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
using Microsoft.AspNetCore.Http;
using EDocs.Nyp.LabReqs.AppService.Logging;

namespace EDocs.Nyp.LabReqs.AppServices
{
    public class LookUpDrCodesViewModel : PageModel
    {
        private readonly IConfiguration configuration;
        private ILog log;
        private string WebApiUrl
        { get { return configuration.GetValue<string>("LabResApi").ToString(); } }

        public IDictionary<string, DrCodeModel> IDicDrcodes
        { get; private set; }

        public DrCodeModel DrCodeModel
        { get; private set; }

        public string StartDate
        { get; set; }
        public string EndDate
        { get; set; }
        public string TotalQueryTime
        { get; set; }

        public LookUpDrCodesViewModel(IConfiguration config, ILog logConfig)
        {
            configuration = config;
            log = logConfig;
        }
        
        private async Task GetLogInformaiton()
        {

            log = InitAuditLogs.LogAsync(log, HttpContext.Session).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public async Task<IActionResult> OnGetAsync(DrCodeModel DrCodeModel)
        {
            try
            {
                var qString = Request.QueryString;
                if (!(User.Identity.IsAuthenticated))
                {
                    if(qString.HasValue)
                    return Redirect($"/NypUsersInfo/LoginView?returnUrl=/LabReqs/LookUpDrCodesView{qString.Value}");
                    else
                        return Redirect($"/NypUsersInfo/LoginView?returnUrl=/LabReqs/LookUpDrCodesView");
                }

                ViewData["CWID"] = GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyCwid).ConfigureAwait(true).GetAwaiter().GetResult();
                if (ViewData["CWID"] == null)
                    return Redirect("/NypUsersInfo/LoginView?returnUrl=/LabReqs/LookUpDrCodesView");
                LabReqHelpers.StartStopWatch();
                GetLogInformaiton().ConfigureAwait(false).GetAwaiter().GetResult();
                log.LogInformation("Start LookUpDrCodesViewModel");
                GetViewAudLogsAdmin();
               

                if (qString.HasValue)
                {
                    log.LogInformation($"Class LookUpDrCodesViewModel for query string:{qString.Value}");
                    bool noErros = true;
                    DrCodeModel = GetStartEndDate(DrCodeModel);
                    if (DrCodeModel.ScanDate.Year > 2000)
                    {
                        StartDate = DrCodeModel.ScanDate.ToString("yyyy-MM-dd");
                        EndDate = DrCodeModel.ScanEndDate.ToString("yyyy-MM-dd");
                        DrCodeModel.ScanEndDate = LabReqHelpers.AddDayToSearchEndDate(DrCodeModel.ScanEndDate);
                    }
                    if ((string.IsNullOrWhiteSpace(DrCodeModel.DrCode)) && (string.IsNullOrWhiteSpace(DrCodeModel.DrFName)) && (string.IsNullOrWhiteSpace(DrCodeModel.DrLName)) && (string.IsNullOrWhiteSpace(DrCodeModel.ScanOperator)))
                    {
                        noErros = false;
                        ModelState.AddModelError("ErrorMessage", $"Need at least Dr Code or Dr First or Last Name or Scan Operator. For all Dr Code use *");
                        log.LogWarning("Class LookUpDrCodesViewModel Need at least Dr Code or Dr First or Last Name or Scan Operator.For all Dr Code use * ");
                    }
                    else if (!(string.IsNullOrWhiteSpace(DrCodeModel.ScanOperator)))
                    {
                        if (!(string.IsNullOrWhiteSpace(DrCodeModel.DrCode)) && (!(string.IsNullOrWhiteSpace(DrCodeModel.DrFName))) && (!(string.IsNullOrWhiteSpace(DrCodeModel.DrLName))))
                        {
                            noErros = false;
                            ModelState.AddModelError("ErrorMessage", $"When lookup ScanOperator Dr Code Dr First or Last Name cannot contain characters..");
                            log.LogWarning("Class LookUpDrCodesViewModel When lookup ScanOperator Dr Code Dr First or Last Name cannot contain characters..");
                        }
                    }
                    else if (!(string.IsNullOrWhiteSpace(DrCodeModel.DrCode)))
                    {
                        if (DrCodeModel.DrCode.Trim() == "*")
                        {
                            if (!(string.IsNullOrWhiteSpace(DrCodeModel.DrFName)) || (!(string.IsNullOrWhiteSpace(DrCodeModel.DrLName))) || (!(string.IsNullOrWhiteSpace(DrCodeModel.ScanOperator))))
                            {
                                noErros = false;
                                ModelState.AddModelError("ErrorMessage", $"When lookup Dr Code * Dr First or Last Name or Scan Operator cannot contain characters..");
                                log.LogWarning("Class LookUpDrCodesViewModel When lookup ScanOperator Dr Code Dr First or Last Name cannot contain characters..");

                            }
                        }
                    }


                    if (noErros)
                    {
                        await GetDrCodes(DrCodeModel).ConfigureAwait(true);
                    }

                }
                TotalQueryTime = LabReqHelpers.StopStopWatch();
                log.LogInformation($"End LookUpDrCodesViewModel total time: {TotalQueryTime} ms");
                
            }
            catch (Exception ex)
            {
                log.LogError($"LookUpDrCodesViewModel total time: { LabReqHelpers.StopStopWatch()} ms {ex.Message}");
                return Redirect($"/NypUsersInfo/DisplayErrorMessages?ErrMEss=Model LookUpDrCodesViewModel OngettAsync {ex.Message}");
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

        private async Task GetDrCodes(DrCodeModel DrCodeModel)
        {
            log.LogInformation($"Method GetDrCodes search  DrCode on DrCode: {DrCodeModel.DrCode} DrFName: {DrCodeModel.DrFName} DrLName: {DrCodeModel.DrLName} ScanOperator: {DrCodeModel.ScanOperator} startdate:{StartDate.ToString()} end date:{EndDate.ToString()} ");

            IDicDrcodes = await SpecimenRejectionApi.SpecimenRejectionIntance.GetDrCodes(WebApiUrl, ConstNypLabReqs.ApiNypDrCodesController, DrCodeModel, log).ConfigureAwait(true);
        }
        private DrCodeModel GetStartEndDate(DrCodeModel DrCodeModel)
        {
            string seDate = Request.Query[ConstNypLabReqs.FormValueStartSDate].ToString().Trim();
            if (!(string.IsNullOrWhiteSpace(seDate)))
            {

                DrCodeModel.ScanDate = GetNypLabReqs.NypLabReqsApisInctance.GetLabRecDate(seDate);
                seDate = Request.Query[ConstNypLabReqs.FormValueEndSDate].ToString();

                DrCodeModel.ScanEndDate = GetNypLabReqs.NypLabReqsApisInctance.GetLabRecDate(seDate);
            }
            return DrCodeModel;
        }
    }
}