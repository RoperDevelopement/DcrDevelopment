using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using EDocs.Nyp.LabReqs.AppServices.Models;
using EDocs.Nyp.LabReqs.AppService.Logging;
using EDocs.Nyp.LabReqs.AppServices.LabReqsConstants;
namespace EDocs.Nyp.LabReqs.AppServices.Pages.LabReqs
{
    public class LookUpCovid19LRViewModel : PageModel
    {
        private readonly IConfiguration configuration;
        private ILog log;
        private string WebApiUrl
        { get { return configuration.GetValue<string>("LabResApi").ToString(); } }

      

   
        public IDictionary<Guid, LabReqsModel> LabReqs
        { get; set;  }

        [BindProperty]
        public LabReqsModel SearchLabReqs
        { get; set; }

        [BindProperty]
        public string ScanDate
        { get; set; }

        public string TotalQueryTime
        { get; set; }
        public LookUpCovid19LRViewModel(IConfiguration config, ILog logConfig)
        {
            configuration = config;
            log = logConfig;
        }
        private async Task GetLogInformaiton()
        {

            log = InitAuditLogs.LogAsync(log, HttpContext.Session).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        public async Task<IActionResult> OnGetAsync(LabReqsModel SearchLabReqs)
        {    
            try
            {
                var qString = Request.QueryString;
                if (!(User.Identity.IsAuthenticated))
                {
                    if(qString.HasValue)
                        return Redirect($"/NypUsersInfo/LoginView?returnUrl=/LabReqs/LookUpCovid19LRView{qString.Value}");
                    else
                     return Redirect("/NypUsersInfo/LoginView?returnUrl=/LabReqs/LookUpCovid19LRView");
                }

                ViewData["CWID"] = GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyCwid).ConfigureAwait(true).GetAwaiter().GetResult();
               
                if (ViewData["CWID"] == null)
                    return Redirect("/NypUsersInfo/LoginView?returnUrl=/LabReqs/LookUpCovid19LRView");

                LabReqHelpers.StartStopWatch();
                GetLogInformaiton().ConfigureAwait(false).GetAwaiter().GetResult();
                log.LogInformation("Start LookUpCovid19LRViewModel");
                GetViewAudLogsAdmin();
                
            if (qString.HasValue)
            {
                    throw new Exception($"Query stirng should have no value: {qString.Value}");
            }
            else
            {
                SearchLabReqs = new LabReqsModel();
             
            }

                TotalQueryTime = LabReqHelpers.StopStopWatch();
                log.LogInformation($"End LookUpCovid19LRViewModel total time: {TotalQueryTime} ms");
               
                
            }
            catch(Exception ex)
            {
                log.LogError($"End LookUpLabReqsModel total time: {LabReqHelpers.StopStopWatch()} ms {ex.Message}");
                // RedirectToPage($"/Error?ErrMEss=Model LookUpLabReqsModel OngettAsync {ex.Message}");

                return  Redirect($"/NypUsersInfo/DisplayErrorMessages?ErrMEss=Model LookUpLabReqsModel OngettAsync {ex.Message}");
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
    }
}