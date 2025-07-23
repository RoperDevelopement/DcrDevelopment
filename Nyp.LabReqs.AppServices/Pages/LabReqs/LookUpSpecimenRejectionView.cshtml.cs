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




    public class LookUpSpecimenRejectionViewModel : PageModel
    {
        private readonly IConfiguration configuration;
        private ILog log;
        private string WebApiUrl
        { get { return configuration.GetValue<string>("LabResApi").ToString(); } }


        public string TotalQueryTime
        { get; set; }


        [BindProperty]
#pragma warning disable CA2227 // Collection properties should be read only
        public IList<string> RejectLogsReson
#pragma warning restore CA2227 // Collection properties should be read only
        { get; private set; }

#pragma warning disable CA2227 // Collection properties should be read only
        public IDictionary<string, SpecimenRejectionModel> DicSpecimenRejectionModel
#pragma warning restore CA2227 // Collection properties should be read only
        { get; private set; }

        [BindProperty]
        public SpecimenRejectionModel SpecimenRejection
        { get; private set; }

        public string StartDate
        { get; set; }

        public string EndDate
        { get; set; }
        public LookUpSpecimenRejectionViewModel(IConfiguration config, ILog logConfig)
        {
            configuration = config;
            log = logConfig;

        }
        private async Task GetLogInformaiton()
        {

            log = InitAuditLogs.LogAsync(log, HttpContext.Session).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        public async Task<IActionResult> OnGetAsync(SpecimenRejectionModel SpecimenRejection)
        {

            try
            {
                var qString = Request.QueryString;
                if (!(User.Identity.IsAuthenticated))
                {
                    if(qString.HasValue)
                        return Redirect($"/NypUsersInfo/LoginView?returnUrl=/LabReqs/LookUpSpecimenRejectionView{qString.Value}");
                    else
                    return Redirect("/NypUsersInfo/LoginView?returnUrl=/LabReqs/LookUpSpecimenRejectionView");
                }

                ViewData["CWID"] = GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyCwid).ConfigureAwait(true).GetAwaiter().GetResult();
                if (ViewData["CWID"] == null)
                    return Redirect("/NypUsersInfo/LoginView?returnUrl=/LabReqs/LookUpSpecimenRejectionView");

                LabReqHelpers.StartStopWatch();
                GetLogInformaiton().ConfigureAwait(false).GetAwaiter().GetResult();
                log.LogInformation("Start LookUpSpecimenRejectionViewModel");
                GetViewAudLogsAdmin();
               

                if (qString.HasValue)
                {
                    log.LogInformation($"Class LookUpSpecimenRejectionViewModel query string: {qString.Value}");
                    SpecimenRejection.ModifyBy = ConstNypLabReqs.SearchParStr;
#pragma warning disable CA1062 // Validate arguments of public methods
                    SpecimenRejection = GetStartEndDate(SpecimenRejection);
#pragma warning restore CA1062 // Validate arguments of public methods
                    if (SpecimenRejection.LogDate.Year > 2000)
                    {
                        StartDate = SpecimenRejection.LogDate.ToString("yyyy-MM-dd");
                        EndDate = SpecimenRejection.ScanDate.ToString("yyyy-MM-dd");
                        SpecimenRejection.ScanDate = LabReqHelpers.AddDayToSearchEndDate(SpecimenRejection.ScanDate);
                    }
                    await LookSpecimenRejection(Request.Query[ConstNypLabReqs.FormValueSearchBy], SpecimenRejection, Request.Query[ConstNypLabReqs.FormValueSearchByLogScanDate]).ConfigureAwait(true);
                    SpecimenRejection.SearchPartial = false;
                    SpecimenRejection.FromReason = string.Empty;
                }
                //     SpecimenRejection = new SpecimenRejectionModel();

                RejectLogsReson = await GetSessionVariables.SessionVarInstance.GetJsonSessionObject<IList<string>>(HttpContext.Session, "RejectLogsResonCace").ConfigureAwait(true);

                if (RejectLogsReson == null)
                {
                    string cont = $"{ConstNypLabReqs.ApiSpecimenRejectionController}{string.Format(ConstNypLabReqs.ApiSpecimenRejectionControllerParam, ConstNypLabReqs.SpNypGetNypRejectionLogsReason)}";
                    RejectLogsReson = await SpecimenRejectionApi.SpecimenRejectionIntance.RejectionLogsReason(WebApiUrl, cont, log).ConfigureAwait(true);
                    if (RejectLogsReson != null)
                        await GetSessionVariables.SessionVarInstance.SetSessionOjbjectAsJson(HttpContext.Session, "RejectLogsResonCace", RejectLogsReson).ConfigureAwait(true);
                }

                TotalQueryTime = LabReqHelpers.StopStopWatch();
                log.LogInformation($"End LookUpSpecimenRejectionViewModel total time: {TotalQueryTime} ms");


                
            }
            catch (Exception ex)
            {
                log.LogInformation($"End LookUpSpecimenRejectionViewModel total time: {LabReqHelpers.StopStopWatch()} ms {ex.Message}");
                return Redirect($"/NypUsersInfo/DisplayErrorMessages?ErrMEss=Model LookUpSpecimenRejectionViewModel OngettAsync {ex.Message}");

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
        private async Task LookSpecimenRejection(string searchStr, SpecimenRejectionModel SpecimenRejection, string srchLogScanDate)
        {
            string spName = string.Empty;
            switch (searchStr)
            {
                case ConstNypLabReqs.FormValueSearchByFormYearNum:
                    SpecimenRejection.ScanOperator = SpecimenRejection.ScanBatch = string.Empty;
                    spName = GetFromTOSp(SpecimenRejection, srchLogScanDate);
                    break;
                case ConstNypLabReqs.FormValueSearchByLogScanDate:
                    SpecimenRejection.FromReason = SpecimenRejection.FromYear = SpecimenRejection.FromReason = SpecimenRejection.ScanOperator = string.Empty;
                    spName = GetSearchLogScanDate(SpecimenRejection, srchLogScanDate);
                    break;
                case ConstNypLabReqs.FormValueSearchRejectResons:
                    SpecimenRejection.FromReason = Request.Query[ConstNypLabReqs.FormValueRejectlogreason];
                    spName = GetSpRectionResons(SpecimenRejection, srchLogScanDate);
                    break;
                case ConstNypLabReqs.FormValueSearchbyScanOperator:
                    SpecimenRejection.FromReason = SpecimenRejection.FromYear = SpecimenRejection.FromReason = string.Empty;
                    spName = GetSpRectionResonsScanOperaotor(srchLogScanDate);
                    break;
                case ConstNypLabReqs.FormValueScanBatchId:
                    SpecimenRejection.FromReason = SpecimenRejection.FromYear = SpecimenRejection.FromReason = SpecimenRejection.ScanOperator = string.Empty;
                    spName = GetSpRectionResonsScanBatcId(srchLogScanDate);
                    break;


                    //  default: throw new Exception($"Search bu {searchStr} not foound");

            }

            log.LogInformation($"Search Specimen Rejection ViewModel by:{srchLogScanDate} FromNumber: {SpecimenRejection.FromNumber} FromYear: {SpecimenRejection.FromYear} FromReason: {SpecimenRejection.FromReason} ScanOperator: {SpecimenRejection.ScanOperator} Start Date: {SpecimenRejection.LogDate} End Date: {SpecimenRejection.ScanDate}");

            DicSpecimenRejectionModel = await SpecimenRejectionApi.SpecimenRejectionIntance.RejectionLogs(WebApiUrl, spName, SpecimenRejection, log).ConfigureAwait(true);
        }

        private string GetSpRectionResonsScanOperaotor(string logscanDate)
        {
            if ((string.IsNullOrWhiteSpace(logscanDate)) || (string.Compare(logscanDate, ConstNypLabReqs.FormValueSearchByLogDate, true) == 0))
            {
                return $"{ConstNypLabReqs.ApiSpecimenRejectionController}{string.Format(ConstNypLabReqs.ApiSpecimenRejectionControllerParam, ConstNypLabReqs.SpNypSpecimenRejectionReasonScanOperatorByLogDate)}";
            }
            else
            {
                return $"{ConstNypLabReqs.ApiSpecimenRejectionController}{string.Format(ConstNypLabReqs.ApiSpecimenRejectionControllerParam, ConstNypLabReqs.SpNypSpecimenRejectionReasonScanOperatorByScanDate)}";
            }
        }
        private string GetSpRectionResonsScanBatcId(string logscanDate)
        {
            if ((string.IsNullOrWhiteSpace(logscanDate)) || (string.Compare(logscanDate, ConstNypLabReqs.FormValueSearchByLogDate, true) == 0))
            {
                return $"{ConstNypLabReqs.ApiSpecimenRejectionController}{string.Format(ConstNypLabReqs.ApiSpecimenRejectionControllerParam, ConstNypLabReqs.SpNypSpecimenRejectionReasonScanBatchIdByLogDate)}";
            }
            else
            {
                return $"{ConstNypLabReqs.ApiSpecimenRejectionController}{string.Format(ConstNypLabReqs.ApiSpecimenRejectionControllerParam, ConstNypLabReqs.SpNypSpecimenRejectionReasonScanBatchIdByScanDate)}";
            }
        }
        private string GetSearchLogScanDate(SpecimenRejectionModel SpecimenRejection, string logscanDate)
        {
            SpecimenRejection.ModifyBy = string.Empty;
            if (string.Compare(logscanDate, ConstNypLabReqs.FormValueSearchByLogDate, true) == 0)
            {
                return $"{ConstNypLabReqs.ApiSpecimenRejectionController}{string.Format(ConstNypLabReqs.ApiSpecimenRejectionControllerParam, ConstNypLabReqs.SpNypSpecimenRejectionLogDate)}";
            }
            else
            {
                return $"{ConstNypLabReqs.ApiSpecimenRejectionController}{string.Format(ConstNypLabReqs.ApiSpecimenRejectionControllerParam, ConstNypLabReqs.SpNypSpecimenRejectionScanDate)}";
            }
        }
        private string GetSpRectionResons(SpecimenRejectionModel SpecimenRejection, string logscanDate)
        {
            try
            {
                if ((string.IsNullOrWhiteSpace(logscanDate)) || (string.Compare(logscanDate, ConstNypLabReqs.FormValueSearchByLogDate, true) == 0))
                {
                    if (!(string.IsNullOrWhiteSpace(SpecimenRejection.FromNumber)) && (!(string.IsNullOrWhiteSpace(SpecimenRejection.FromYear))))
                        return $"{ConstNypLabReqs.ApiSpecimenRejectionController}{string.Format(ConstNypLabReqs.ApiSpecimenRejectionControllerParam, ConstNypLabReqs.SpNypSpecimenRejectionReasonFromYearNumberByLogDate)}";

                    else if (!(string.IsNullOrWhiteSpace(SpecimenRejection.FromNumber)))
                        return $"{ConstNypLabReqs.ApiSpecimenRejectionController}{string.Format(ConstNypLabReqs.ApiSpecimenRejectionControllerParam, ConstNypLabReqs.SpNypSpecimenRejectionResonFormNumberByLogDate)}";
                    else if (!(string.IsNullOrWhiteSpace(SpecimenRejection.FromYear)))
                        return $"{ConstNypLabReqs.ApiSpecimenRejectionController}{string.Format(ConstNypLabReqs.ApiSpecimenRejectionControllerParam, ConstNypLabReqs.SpNypSpecimenRejectionResonFormYearByLogDate)}";
                    else

                        return $"{ConstNypLabReqs.ApiSpecimenRejectionController}{string.Format(ConstNypLabReqs.ApiSpecimenRejectionControllerParam, ConstNypLabReqs.SpNypSpecimenRejectionReasonByLogDate)}";



                }
                else
                {
                    if (!(string.IsNullOrWhiteSpace(SpecimenRejection.FromNumber)) && (!(string.IsNullOrWhiteSpace(SpecimenRejection.FromYear))))
                        return $"{ConstNypLabReqs.ApiSpecimenRejectionController}{string.Format(ConstNypLabReqs.ApiSpecimenRejectionControllerParam, ConstNypLabReqs.SpNypSpecimenRejectionReasonFromYearNumberByScanDate)}";
                    else if (!(string.IsNullOrWhiteSpace(SpecimenRejection.FromNumber)))
                        return $"{ConstNypLabReqs.ApiSpecimenRejectionController}{string.Format(ConstNypLabReqs.ApiSpecimenRejectionControllerParam, ConstNypLabReqs.SpNypSpecimenRejectionResonFormNumberByScanDate)}";
                    else if (!(string.IsNullOrWhiteSpace(SpecimenRejection.FromYear)))
                        return $"{ConstNypLabReqs.ApiSpecimenRejectionController}{string.Format(ConstNypLabReqs.ApiSpecimenRejectionControllerParam, ConstNypLabReqs.SpNypSpecimenRejectionReasonFromYearNumberByScanDate)}";
                    else
                        return $"{ConstNypLabReqs.ApiSpecimenRejectionController}{string.Format(ConstNypLabReqs.ApiSpecimenRejectionControllerParam, ConstNypLabReqs.SpNypSpecimenRejectionReasonByScanDate)}";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return string.Empty;
        }
        private string GetFromTOSp(SpecimenRejectionModel SpecimenRejection, string logscanDate)
        {
            if ((string.IsNullOrWhiteSpace(logscanDate)) || (string.Compare(logscanDate, ConstNypLabReqs.FormValueSearchByLogDate, true) == 0))
            {
                if (!(string.IsNullOrWhiteSpace(SpecimenRejection.FromNumber)) && (!(string.IsNullOrWhiteSpace(SpecimenRejection.FromYear))))
                    return $"{ConstNypLabReqs.ApiSpecimenRejectionController}{string.Format(ConstNypLabReqs.ApiSpecimenRejectionControllerParam, ConstNypLabReqs.SpNypSpecimenRejectionFromYearNumberByLogDate)}";
                else if (!(string.IsNullOrWhiteSpace(SpecimenRejection.FromNumber)))
                    return $"{ConstNypLabReqs.ApiSpecimenRejectionController}{string.Format(ConstNypLabReqs.ApiSpecimenRejectionControllerParam, ConstNypLabReqs.SpNypSpecimenRejectionFromNumberByLogDate)}";
                else
                    return $"{ConstNypLabReqs.ApiSpecimenRejectionController}{string.Format(ConstNypLabReqs.ApiSpecimenRejectionControllerParam, ConstNypLabReqs.SpNypSpecimenRejectionFromYearByLogDate)}";
            }
            else
            {
                if (!(string.IsNullOrWhiteSpace(SpecimenRejection.FromNumber)) && (!(string.IsNullOrWhiteSpace(SpecimenRejection.FromYear))))
                    return $"{ConstNypLabReqs.ApiSpecimenRejectionController}{string.Format(ConstNypLabReqs.ApiSpecimenRejectionControllerParam, ConstNypLabReqs.SpNypSpecimenRejectionFromYearNumberByScanDate)}";
                else if (!(string.IsNullOrWhiteSpace(SpecimenRejection.FromNumber)))
                    return $"{ConstNypLabReqs.ApiSpecimenRejectionController}{string.Format(ConstNypLabReqs.ApiSpecimenRejectionControllerParam, ConstNypLabReqs.SpNypSpecimenRejectionFromNumberByScanDate)}";
                else
                    return $"{ConstNypLabReqs.ApiSpecimenRejectionController}{string.Format(ConstNypLabReqs.ApiSpecimenRejectionControllerParam, ConstNypLabReqs.SpNypSpecimenRejectionFromYearByScanDate)}";
            }
          //  return string.Empty;
        }
        private SpecimenRejectionModel GetStartEndDate(SpecimenRejectionModel SpecimenRejection)
        {
            string seDate = Request.Query[ConstNypLabReqs.FormValueStartSDate].ToString().Trim();
            if (!(string.IsNullOrWhiteSpace(seDate)))
            {

                SpecimenRejection.LogDate = GetNypLabReqs.NypLabReqsApisInctance.GetLabRecDate(seDate);
                seDate = Request.Query[ConstNypLabReqs.FormValueEndSDate].ToString();

                SpecimenRejection.ScanDate = GetNypLabReqs.NypLabReqsApisInctance.GetLabRecDate(seDate);
            }
            return SpecimenRejection;
        }
    }
}