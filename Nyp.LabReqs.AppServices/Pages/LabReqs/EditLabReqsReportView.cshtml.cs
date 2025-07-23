using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EDocs.Nyp.LabReqs.AppService.Logging;
using EDocs.Nyp.LabReqs.AppServices.LabReqsConstants;
using EDocs.Nyp.LabReqs.AppServices.Models;
using EDocs.Nyp.LabReqs.AppServices.ApiClasses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
 
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace EDocs.Nyp.LabReqs.AppServices.Pages.LabReqs
{
    public class EditLabReqsReportViewModel : PageModel
    {
        private readonly IConfiguration configuration;
        private ILog log;
        private string WebApiUrl
        { get { return configuration.GetValue<string>("LabResApi").ToString(); } }

        [Display(Name = "MRN:")]
        public string MRN
        { get; set; }
        [Display(Name = "Index Number:")]
        public string IndexNumber
        { get; set; }
        [Display(Name = "CSN Number:")]
        public string FinancialNumber
        { get; set; }
        public IDictionary<int,EditLabReqsReportModel> LabReqsModel
        { get; set; }
        public EditLabReqsReportViewModel(IConfiguration config, ILog logConfig)
        {
            configuration = config;
            log = logConfig;
        }
        private async Task GetLogInformaiton()
        {

            log = InitAuditLogs.LogAsync(log, HttpContext.Session).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        private void GetViewAudLogsAdmin()
        {

            LabReqsIsAdminVeiwAuditLogs IsAdminVeiwAuditLogs = LabReqsConstants.GetSessionVariables.SessionVarInstance.GetJsonSessionObject<LabReqsIsAdminVeiwAuditLogs>(HttpContext.Session, ConstNypLabReqs.SessionIsAdminVeiwAuditLogs).ConfigureAwait(false).GetAwaiter().GetResult();
            ViewData["IsAdmin"] = IsAdminVeiwAuditLogs.IsAdmin;
            ViewData["ViewAuditLogs"] = IsAdminVeiwAuditLogs.ViewAuditLogs;
            ViewData["EditLRDocs"] = IsAdminVeiwAuditLogs.EditLRDocs;
        }  

        
        public async Task<IActionResult> OnGetAsync(DateTime scanDate, DateTime receiptDate, bool runRep, string IndexNumber, string FinancialNumber, string MRN)
        {
            try
            {

            
            var qString = Request.QueryString;
            if (!(User.Identity.IsAuthenticated))
            {
                if (qString.HasValue)
                    return Redirect($"/NypUsersInfo/LoginView?returnUrl=/LabReqs/EditLabReqsReportView{qString.Value}");
                else
                    return Redirect($"/NypUsersInfo/LoginView?returnUrl=/LabReqs/EditLabReqsReportView");
            }
            ViewData["CWID"] = GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyCwid).ConfigureAwait(true).GetAwaiter().GetResult();
            if (ViewData["CWID"] == null)
                return Redirect($"/NypUsersInfo/LoginView?returnUrl=/LabReqs/EditLabReqsReportView");
            GetLogInformaiton().ConfigureAwait(false).GetAwaiter().GetResult();
            log.LogInformation("Start EditLabReqsReportViewModel");
            if (runRep)
            {
                log.LogInformation($"Getting labreq changed for query string {qString.Value}");
                GetLabChanged(receiptDate,scanDate,IndexNumber, FinancialNumber, MRN).ConfigureAwait(false).GetAwaiter().GetResult();
            }
           
            GetViewAudLogsAdmin();
            log.LogInformation($"End EditLabReqsReportViewModel total time: {LabReqHelpers.StopStopWatch()} ms");
            }
            catch(Exception ex)
            {
                log.LogError($"EditLabReqsReportViewModel  total time: {LabReqHelpers.StopStopWatch()} {ex.Message}");
                return Redirect($"/NypUsersInfo/DisplayErrorMessages?ErrMEss=Model EditLabReqsReportViewModel OngettAsync {ex.Message}");
            }
            return Page();
        }

        public async Task GetLabChanged(DateTime sDate, DateTime eDate,string indexNumber, string csnFinNumber, string MRNPatID)
        {
            string dateSt = string.Empty;
            string dateEnd = string.Empty;
            if((sDate.Year < 2000) ||(eDate.Year < 2000))
            {
                dateSt = ConstNypLabReqs.StrEdocsNoData;
                dateEnd = ConstNypLabReqs.StrEdocsNoData;

            }
            else
            {
                dateSt = sDate.ToString("MM-dd-yyyy");
                dateEnd = eDate.ToString("MM-dd-yyyy");
            }
            if (string.IsNullOrWhiteSpace(indexNumber))
                indexNumber = ConstNypLabReqs.StrEdocsNoData;
            if (string.IsNullOrWhiteSpace(csnFinNumber))
                csnFinNumber = ConstNypLabReqs.StrEdocsNoData;
            if (string.IsNullOrWhiteSpace(MRNPatID))
                MRNPatID = ConstNypLabReqs.StrEdocsNoData;
            log.LogInformation($"User {User.Identity.Name} getting changed labreq for date range {dateSt}-{dateEnd} for index number {indexNumber} csn fin number {csnFinNumber} MRN-Patient ID {MRNPatID}");
            LabReqsModel = GetNypLabReqs.NypLabReqsApisInctance.GetLabReqsChangedRep(WebApiUrl, ConstNypLabReqs.ApiEditLabReqsController, dateSt, dateEnd, indexNumber, csnFinNumber, MRNPatID, log).ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}
