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
    public class LookupDohViewModel : PageModel
    {
        private readonly IConfiguration configuration;
        private ILog log;
        private string WebApiUrl
        { get { return configuration.GetValue<string>("LabResApi").ToString(); } }

        [BindProperty]
       
        public string StartDate
        { get; set; }

        public string EndDate
        { get; set; }

        public IDictionary<int, DohModel> IDicDohModel
        { get; private set; }

        public DohModel DohModel
        { get; set; }

        public string TotalQueryTime
        { get; set; }
        public LookupDohViewModel(IConfiguration config, ILog logConfig)
        {
            configuration = config;
            log = logConfig;
        }

        private async Task GetLogInformaiton()
        {

            log = InitAuditLogs.LogAsync(log, HttpContext.Session).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public async Task<IActionResult> OnGetAsync(DohModel DohModel)
        {


            try
            {
                var qString = Request.QueryString;
                if (!(User.Identity.IsAuthenticated))
                {
                    if (qString.HasValue)
                        return Redirect($"/NypUsersInfo/LoginView?returnUrl=/LabReqs/LookupDohView{qString.Value}");
                    else
                        return Redirect($"/NypUsersInfo/LoginView?returnUrl=/LabReqs/LookupDohView");
                }
                ViewData["CWID"] = GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyCwid).ConfigureAwait(true).GetAwaiter().GetResult();
                if (ViewData["CWID"] == null)
                    return Redirect("/NypUsersInfo/LoginView?returnUrl=/LabReqs/LookupDohView");
                LabReqHelpers.StartStopWatch();
                GetLogInformaiton().ConfigureAwait(false).GetAwaiter().GetResult();
                log.LogInformation("Start LookupDohViewModel");
                GetViewAudLogsAdmin();


                if (qString.HasValue)
                {
                    log.LogInformation($"Class LookupDohViewModel query string: {qString.Value}");
                    DohModel = GetStartEndDate(DohModel);
#pragma warning restore CA1062 // Validate arguments of public methods
                    if (DohModel.DateOFService.Year > 2000)
                    {
                        log.LogInformation("");

                        StartDate = DohModel.DateOFService.ToString("yyyy-MM-dd");
                        EndDate = DohModel.ScanDate.ToString("yyyy-MM-dd");
                        DohModel.ScanDate = LabReqHelpers.AddDayToSearchEndDate(DohModel.ScanDate);

                    }
                    await LookUpDohLabRecs(Request.Query[ConstNypLabReqs.FormValueSearchDohBy], DohModel, Request.Query[ConstNypLabReqs.FormValueSearchBy]).ConfigureAwait(true);
                }

               
                TotalQueryTime = LabReqHelpers.StopStopWatch();

                log.LogInformation($"End LookupDohViewModel total time: {TotalQueryTime} ms");

            }
            catch (Exception ex)
            {
                log.LogError($"LookupDohViewModel  total time: {LabReqHelpers.StopStopWatch()} {ex.Message}");
                return Redirect($"/NypUsersInfo/DisplayErrorMessages?ErrMEss=Model LookupDohViewModel OngettAsync {ex.Message}");
            }

            return Page();
        }

        private async Task LookUpDohLabRecs(string searchStr, DohModel DohModel, string srchLogScanDate)
        {
            if (string.IsNullOrWhiteSpace(srchLogScanDate))
                DohModel.ScanMachine = ConstNypLabReqs.NA;
            else
                DohModel.ScanMachine = srchLogScanDate;

            string spName = string.Empty;
            switch (searchStr)
            {
                case ConstNypLabReqs.FormValueSearchbyPatientName:
                
                    spName = string.Format(ConstNypLabReqs.ApiSpecimenRejectionControllerParam, ConstNypLabReqs.SpNypDohPatientNameDateOfService);
                    break;
                case ConstNypLabReqs.FormValueSearchbyAccessMrnNum:
                   
                    spName = string.Format(ConstNypLabReqs.ApiSpecimenRejectionControllerParam, ConstNypLabReqs.SpNypDohAssessionMrnNumberDateOfService);
                    break;
                case ConstNypLabReqs.FormValueSearchbyDrName:
                     
                    spName = string.Format(ConstNypLabReqs.ApiSpecimenRejectionControllerParam, ConstNypLabReqs.SpNypDohDrFirstLastNameDateOfService);
                    break;
                case ConstNypLabReqs.FormValueSearchByLogScanDate:
                  
                    spName = string.Format(ConstNypLabReqs.ApiSpecimenRejectionControllerParam, ConstNypLabReqs.SpNypDohDateOfService);
                    break;

                default: throw new Exception($"Search by {searchStr} not found");

            }
            log.LogInformation($"Method LookUpDohLabRecs search Doh records by AccessionNumber: {DohModel.AccessionNumber} MRN: {DohModel.MRN} ScanMachine: {DohModel.ScanMachine} search dates on: {DohModel.ScanMachine} start date:  {DohModel.DateOFService.ToString()} end date: {DohModel.ScanDate.ToString()}");
            IDicDohModel = await SpecimenRejectionApi.SpecimenRejectionIntance.DOHLabReqs(new Uri($"{WebApiUrl}{ConstNypLabReqs.ApiNypDOHController}", UriKind.RelativeOrAbsolute), spName, DohModel, log).ConfigureAwait(true);

            // DicSpecimenRejectionModel = await SpecimenRejectionApi.SpecimenRejectionIntance.RejectionLogs(WebApiUrl, spName, SpecimenRejection).ConfigureAwait(true);
        }

        private void GetViewAudLogsAdmin()
        {
            LabReqsIsAdminVeiwAuditLogs IsAdminVeiwAuditLogs = LabReqsConstants.GetSessionVariables.SessionVarInstance.GetJsonSessionObject<LabReqsIsAdminVeiwAuditLogs>(HttpContext.Session, ConstNypLabReqs.SessionIsAdminVeiwAuditLogs).ConfigureAwait(false).GetAwaiter().GetResult();
            ViewData["IsAdmin"] = IsAdminVeiwAuditLogs.IsAdmin;
            ViewData["ViewAuditLogs"] = IsAdminVeiwAuditLogs.ViewAuditLogs;
			    ViewData["EditLRDocs"] = IsAdminVeiwAuditLogs.EditLRDocs;
        }
        private DohModel GetStartEndDate(DohModel DohModel)
        {
            string seDate = Request.Query[ConstNypLabReqs.FormValueStartSDate].ToString().Trim();
            if (!(string.IsNullOrWhiteSpace(seDate)))
            {

                DohModel.DateOFService = GetNypLabReqs.NypLabReqsApisInctance.GetLabRecDate(seDate);
                seDate = Request.Query[ConstNypLabReqs.FormValueEndSDate].ToString();

                DohModel.ScanDate = GetNypLabReqs.NypLabReqsApisInctance.GetLabRecDate(seDate);
            }
            return DohModel;
        }

    }
}