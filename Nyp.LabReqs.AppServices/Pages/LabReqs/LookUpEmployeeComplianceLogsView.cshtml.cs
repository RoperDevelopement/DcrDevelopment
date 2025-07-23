using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EDocs.Nyp.LabReqs.AppService.Logging;
using Microsoft.Extensions.Configuration;
using EDocs.Nyp.LabReqs.AppServices.Models;
using EDocs.Nyp.LabReqs.AppServices.ApiClasses;
using EDocs.Nyp.LabReqs.AppServices.LabReqsConstants;
using EDocs.Nyp.LabReqs.AppServices.LabReqInterfaces;

namespace EDocs.Nyp.LabReqs.AppServices
{
    public class LookUpEmployeeComplianceLogsViewModel : PageModel
    {
        public string StartDate
        { get; set; }

        public string EndDate
        { get; set; }

        private readonly IEmailSettings emailSettings;
        private readonly IConfiguration configuration;
        private ILog log;

        private string WebApiUrl
        { get { return configuration.GetValue<string>("LabResApi").ToString(); } }

        public LookUpEmployeeComplianceLogsViewModel(IConfiguration config, IEmailSettings email, ILog logConfig)
        {
            emailSettings = email;
            log = logConfig;
            configuration = config;
        }
        private async Task GetLogInformaiton()
        {

            log = InitAuditLogs.LogAsync(log, HttpContext.Session).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        public IList<string> EmployeeDepartments
        { get; private set; }

        public IList<string> EmployeeIds
        { get; private set; }

        public IList<string> EmployeeJobTitle
        { get; private set; }

        public IList<string> EmployeeDocType
        { get; private set; }

        public IDictionary<string, EmployeeComplianceLogsModel> IDicEmployeeComplianceLogsModel
        { get; private set; }
        public string TotalQueryTime
        { get; set; }
        [BindProperty]
        public EmployeeComplianceLogsModel ComplianceLogsModel
        { get; private set; }
    
        public async Task<IActionResult> OnGetAsync(EmployeeComplianceLogsModel ComplianceLogsModel)
        {
            
            
            try
            {
                var qString = Request.QueryString;
                if (!(User.Identity.IsAuthenticated))
                {
                    if(qString.HasValue)
                        return Redirect($"/NypUsersInfo/LoginView?returnUrl=/LabReqs/LookUpEmployeeComplianceLogsView{qString.Value}");
                    else
                        return Redirect($"/NypUsersInfo/LoginView?returnUrl=/LabReqs/LookUpEmployeeComplianceLogsView");
                }
                ViewData["CWID"] =   GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyCwid).ConfigureAwait(true).GetAwaiter().GetResult();
                if (ViewData["CWID"] == null)
                {
                    if (qString.HasValue)
                        return Redirect($"/NypUsersInfo/LoginView?returnUrl=/LabReqs/LookUpEmployeeComplianceLogsView{qString.Value}");
                    else
                        return Redirect($"/NypUsersInfo/LoginView?returnUrl=/LabReqs/LookUpEmployeeComplianceLogsView");
                }
                LabReqHelpers.StartStopWatch();
                GetLogInformaiton().ConfigureAwait(false).GetAwaiter().GetResult();
                log.LogInformation("Start LookUpEmployeeComplianceLogsViewModel");
                 GetViewAudLogsAdmin();
               

                if (qString.HasValue)
                {
                    log.LogInformation($"Class LookUpEmployeeComplianceLogsViewModel query string: {qString.Value}");
                    await GetEmployeeComlianceLogs(ComplianceLogsModel, Request.Query[ConstNypLabReqs.FormValueSearchBy], Request.Query[ConstNypLabReqs.FormValuePerfLabCode]).ConfigureAwait(true);
                }


                 await GetEmpList().ConfigureAwait(true);
                ComplianceLogsModel.SearchPartial = false;
                TotalQueryTime = LabReqHelpers.StopStopWatch();
                log.LogInformation($"End LookUpEmployeeComplianceLogsViewModel total time: {TotalQueryTime} ms");
                
            }
            catch (Exception ex)
            {
                log.LogError($"Class LookUpEmployeeComplianceLogsViewModel total time: {LabReqHelpers.StopStopWatch()} ms {ex.Message}");
                //  RedirectToPage($"/Error?ErrMEss=Model LookUpEmployeeComplianceLogsViewModel OngettAsync {ex.Message}");
                return Redirect($"/NypUsersInfo/DisplayErrorMessages?ErrMEss=Model LookUpEmployeeComplianceLogsViewModel OngettAsync {ex.Message}");

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

        private async Task GetEmpList()
        {
            EmployeeDepartments = await GetSessionVariables.SessionVarInstance.GetJsonSessionObject<IList<string>>(HttpContext.Session, "EmployeeDepartmentsCache").ConfigureAwait(true);
            if (EmployeeDepartments == null)
            {
                EmployeeDepartments = await NypEmployeeComplianceApi.EmployeeCompliamceIntance.NypEmployeeComplianceCodes(new Uri($"{WebApiUrl}{ConstNypLabReqs.ApiNypEmployeeComplianceController}", UriKind.RelativeOrAbsolute), $"{string.Format(ConstNypLabReqs.ApiSpecimenRejectionControllerParam, ConstNypLabReqs.SpNypEmployeeComplianceDepartment)}", log).ConfigureAwait(true);
                await GetSessionVariables.SessionVarInstance.SetSessionOjbjectAsJson(HttpContext.Session, "EmployeeDepartmentsCache", EmployeeDepartments).ConfigureAwait(true);
            }

            EmployeeIds = await GetSessionVariables.SessionVarInstance.GetJsonSessionObject<IList<string>>(HttpContext.Session, "EmployeeIdsCache").ConfigureAwait(true);
            if (EmployeeIds == null)
            {
                EmployeeIds = await NypEmployeeComplianceApi.EmployeeCompliamceIntance.NypEmployeeComplianceCodes(new Uri($"{WebApiUrl}{ConstNypLabReqs.ApiNypEmployeeComplianceController}", UriKind.RelativeOrAbsolute), $"{string.Format(ConstNypLabReqs.ApiSpecimenRejectionControllerParam, ConstNypLabReqs.SpNypEmployeeComplianceIdNumber)}", log).ConfigureAwait(true);
                await GetSessionVariables.SessionVarInstance.SetSessionOjbjectAsJson(HttpContext.Session, "EmployeeIdsCache", EmployeeIds).ConfigureAwait(true);
            }

            EmployeeJobTitle = await GetSessionVariables.SessionVarInstance.GetJsonSessionObject<IList<string>>(HttpContext.Session, "EmployeeJobTitleCache").ConfigureAwait(true);
            if (EmployeeJobTitle == null)
            {
                EmployeeJobTitle = await NypEmployeeComplianceApi.EmployeeCompliamceIntance.NypEmployeeComplianceCodes(new Uri($"{WebApiUrl}{ConstNypLabReqs.ApiNypEmployeeComplianceController}", UriKind.RelativeOrAbsolute), $"{string.Format(ConstNypLabReqs.ApiSpecimenRejectionControllerParam, ConstNypLabReqs.SpNypEmployeeComplianceJobTitle)}", log).ConfigureAwait(true);
                await GetSessionVariables.SessionVarInstance.SetSessionOjbjectAsJson(HttpContext.Session, "EmployeeJobTitleCache", EmployeeJobTitle).ConfigureAwait(true);
            }

            EmployeeDocType = await GetSessionVariables.SessionVarInstance.GetJsonSessionObject<IList<string>>(HttpContext.Session, "EmployeeDocTypeCache").ConfigureAwait(true);
            if (EmployeeDocType == null)
            {
                EmployeeDocType = await NypEmployeeComplianceApi.EmployeeCompliamceIntance.NypEmployeeComplianceCodes(new Uri($"{WebApiUrl}{ConstNypLabReqs.ApiNypEmployeeComplianceController}", UriKind.RelativeOrAbsolute), $"{string.Format(ConstNypLabReqs.ApiSpecimenRejectionControllerParam, ConstNypLabReqs.SpNypEmployeeComplianceDocType)}", log).ConfigureAwait(true);
                await GetSessionVariables.SessionVarInstance.SetSessionOjbjectAsJson(HttpContext.Session, "EmployeeDocTypeCache", EmployeeDocType).ConfigureAwait(true);
            }
        }

        private async Task GetEmployeeComlianceLogs(EmployeeComplianceLogsModel logsModel, string searchBy, string empCode)
        {

            DateTime dtStTime = DateTime.Now;
            DateTime dtEndTime = DateTime.Now;
            if (LabReqHelpers.GetStartEndDate(Request.Query[ConstNypLabReqs.FormValueStartSDate].ToString(), Request.Query[ConstNypLabReqs.FormValueEndSDate].ToString(), ref dtStTime, ref dtEndTime))
            {
                logsModel.ScanDate = dtStTime;
                logsModel.DateUpload = dtEndTime;
                StartDate = dtStTime.ToString("yyyy-MM-dd");
                EndDate = dtEndTime.ToString("yyyy-MM-dd");
                logsModel.DateUpload = LabReqHelpers.AddDayToSearchEndDate(dtEndTime);
            }
            switch (searchBy)
            {

                case ConstNypLabReqs.FormValueSearchByEmpID:
                    logsModel.FirstName = logsModel.LastName = logsModel.ScanOperator = string.Empty;
                    logsModel.EmpIdNumber = empCode;
                    logsModel.SearchPartial = false;

                    break;
                case ConstNypLabReqs.FormValueSearchByEmpDep:
                    logsModel.FirstName = logsModel.LastName = logsModel.ScanOperator = string.Empty;
                    logsModel.EmpDepartment = empCode;


                    break;
                case ConstNypLabReqs.FormValueSearchByEmpDocType:
                    logsModel.FirstName = logsModel.LastName = logsModel.ScanOperator = string.Empty;
                    logsModel.EmpDocumentType = empCode;


                    break;
                case ConstNypLabReqs.FormValueSearchByEmpJobTitle:
                    logsModel.FirstName = logsModel.LastName = logsModel.ScanOperator = string.Empty;
                    logsModel.EmpJobTitle = empCode;


                    break;
                case ConstNypLabReqs.FormValuePFName:
                    logsModel.ScanOperator = string.Empty;
                    break;
                case ConstNypLabReqs.FormValueSearchByLogScanDate:
                    logsModel.FirstName = logsModel.LastName = logsModel.ScanOperator = string.Empty;
                    break;
                case ConstNypLabReqs.FormValueSearchbyScanOperator:
                    logsModel.FirstName = logsModel.LastName = string.Empty;
                    break;


                default: throw new Exception("");
            }
            log.LogInformation($"Search Employee Compliance Logs by searchBy: {searchBy} EmpIdNumber: {logsModel.EmpIdNumber} EmpDepartment: {logsModel.EmpDepartment} EmpDocumentType: {logsModel.EmpDocumentType} EmpJobTitle: {logsModel.EmpJobTitle} FirstName: {logsModel.FirstName} LastName: {logsModel.LastName} ScanOperator: {logsModel.ScanOperator} start date: {logsModel.ScanDate.ToString()} end date: {logsModel.DateUpload.ToString()} ");
            string spname = $"{string.Format(ConstNypLabReqs.ApiSpecimenRejectionControllerParam, ConstNypLabReqs.SpNypEmployeeComplianceLogsByScanDate)}";
            IDicEmployeeComplianceLogsModel = await NypEmployeeComplianceApi.EmployeeCompliamceIntance.NypComplianceLogs(new Uri($"{WebApiUrl}{ConstNypLabReqs.ApiNypEmployeeComplianceController}"), spname, logsModel,log).ConfigureAwait(true);
        }
    }
}