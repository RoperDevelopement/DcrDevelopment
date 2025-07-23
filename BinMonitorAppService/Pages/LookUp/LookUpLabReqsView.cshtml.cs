using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using BinMonitorAppService.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using BinMonitorAppService.Constants;
using BinMonitorAppService.ApiClasses;
using BinMonitorAppService.Logging;
using System.Text;
using BinMonitor.BinInterfaces;

namespace BinMonitorAppService.Pages.LookUp
{
    public class LookUpLabReqsViewModel : PageModel
    {
        private string WebApiUrl
        { get { return configuration.GetValue<string>("BinMonitorApi").ToString(); } }
        [BindProperty]
        [Display(Name = "Start Date:")]
        [DataType(DataType.Date)]
        public DateTime StDate
        { get; set; }

        [BindProperty]
        [DataType(DataType.Date)]
        [Display(Name = "End Date:")]
        public DateTime EndDate
        { get; set; }
        [BindProperty]
        [DataType(DataType.Text)]
        [Display(Name = "LabReq Number:")]
        public string LabReqNumber
        { get; set; }
        public string LookupLabReqNumber
        { get; set; }
        private readonly IConfiguration configuration;
        private ILog auditLogs;
        public SpectrumMonitorMenuRightsModel SpectrumMonitorMenuRightsModel
        { get; set; }
        public LookUpLabReqsViewModel(IConfiguration config, ILog logConfig)
        {

            auditLogs = logConfig;
            configuration = config;

        }
        public IList<BinRegProcessModel>LLabReq
        { get; set; }
        private async Task GetLogInformaiton()
        {

            auditLogs = InitAuditLogs.LogAsync(auditLogs, HttpContext.Session).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        public async Task<ActionResult> OnGetAsync(string LabReqNumber)
        { 
            InitAuditLogs.StartStopWatch();
            LookupLabReqNumber = LabReqNumber;
            GetLogInformaiton().ConfigureAwait(false).GetAwaiter().GetResult();
            auditLogs.LogInformation("Start OnGetAsync LookUpLabReqsViewModel");
          
            var qstr = Request.QueryString;
            if (!(User.Identity.IsAuthenticated))
            {
                auditLogs.LogInformation($"ReNewing session for LookUpLabReqsViewModel total time: {InitAuditLogs.StopStopWatch()} ms");
                if (qstr.HasValue)
                {

                    return Redirect($"/BinUsers/LoginView?returnUrl=/LookUp/LookUpLabReqsView{qstr.Value}");
                }

                else
                    return Redirect($"/BinUsers/LoginView?returnUrl=/LookUp/LookUpLabReqsView");
            }
            try
            {


                ViewData["CWID"] = User.Identity.Name;
                
                if (qstr.HasValue)
                {
                    auditLogs.LogInformation($"Query String {qstr.Value}");
                    DateTime std = DateTime.Parse(Request.Query["StDate"].ToString());
                    DateTime ed = DateTime.Parse(Request.Query["EndDate"].ToString());
                    auditLogs.LogInformation($"User {User.Identity.Name} Looking up LabReq ID {LabReqNumber} date range {std}-{ed}");
                    LLabReq = GetApis.GetApisInctance.GetSpecLabReq(WebApiUrl, SqlConstants.ApiSpecMonitorLabReqsController, SqlConstants.SpGetLabReq, std, ed, LabReqNumber, auditLogs).ConfigureAwait(false).GetAwaiter().GetResult();
                    if (LLabReq.Count() != 0)
                        LookupLabReqNumber = string.Empty;
                    StDate = std;
                    EndDate = ed;
                }
                else
                {
                    StDate = DateTime.Now.AddDays(-10);
                    EndDate = DateTime.Now;
                }
                GetViewData().ConfigureAwait(false).GetAwaiter().GetResult();
                auditLogs.LogInformation($"End LookUpLabReqsViewModel total time: {InitAuditLogs.StopStopWatch()} ms");
            }
            catch (Exception ex)
            {
                auditLogs.LogError($"End LookUpLabReqsViewModel total time: {InitAuditLogs.StopStopWatch()} ms");
                auditLogs.LogError($"LookUpLabReqsViewModel on get {ex.Message}");
                return Redirect($"/BinUsers/DisplayErrorMessagesView?ErrMEss=Model LookUpLabReqsViewModel  OnGetAsync {ex.Message}");
            }

            return Page();
        }
        private async Task GetViewData()
        {
            string Cwid = User.Identity.Name;
            SpectrumMonitorMenuRightsModel = ApiSetUserProfile.UserProfileInstance.GetBMMenuRights(Cwid, HttpContext.Session, auditLogs, WebApiUrl).ConfigureAwait(false).GetAwaiter().GetResult();
            ViewData[SqlConstants.ViewDataChangeCategories] = SpectrumMonitorMenuRightsModel.Categories;
            ViewData[SqlConstants.ViewDataEmailReports] = SpectrumMonitorMenuRightsModel.EmailReports;
            ViewData[SqlConstants.ViewDataRunReports] = SpectrumMonitorMenuRightsModel.RunReports;

        }

    }
}
