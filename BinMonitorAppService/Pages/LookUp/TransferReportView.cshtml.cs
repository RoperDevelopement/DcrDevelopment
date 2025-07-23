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
using System.Security.Policy;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Net.NetworkInformation;
using System.Xml.Linq;
using System.Collections;
using Newtonsoft.Json.Linq;

namespace BinMonitorAppService.Pages.LookUp
{
    public class TransferReportViewModel : PageModel
{
        private readonly IConfiguration configuration;
        private ILog auditLogs;
        private string WebApiUrl
        { get { return configuration.GetValue<string>("BinMonitorApi").ToString(); } }
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime UsageRepStDate
        { get; set; }

        [BindProperty]
        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime UsageRepEndDate
        { get; set; }
        public SpectrumMonitorMenuRightsModel SpectrumMonitorMenuRightsModel
        { get; set; }
        private async Task GetLogInformaiton()
        {

            auditLogs = InitAuditLogs.LogAsync(auditLogs, HttpContext.Session).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        public TransferReportViewModel(IConfiguration config, ILog logConfig)
        {

            auditLogs = logConfig;
            configuration = config;

        }
        public async Task<ActionResult> OnGetAsync()
        {
            InitAuditLogs.StartStopWatch();
            GetLogInformaiton().ConfigureAwait(false).GetAwaiter().GetResult();
            auditLogs.LogInformation("Start OnGetAsync TransferReportViewModel");
            ViewData["CWID"] = User.Identity.Name;
            var qstr = Request.QueryString;
            if (!(User.Identity.IsAuthenticated))
            {
                auditLogs.LogInformation($"ReNewing session for TransferReportViewModel total time: {InitAuditLogs.StopStopWatch()} ms");
                if (qstr.HasValue)
                {

                    return Redirect($"/BinUsers/LoginView?returnUrl=/LookUp/TransferReportView{qstr.Value}");
                }

                else
                    return Redirect($"/BinUsers/LoginView?returnUrl=/LookUp/TransferReportView");
            }
            try
            {


                if (qstr.HasValue)
                {
                    auditLogs.LogInformation($"CWID {User.Identity.Name} running transfer reports for query string {qstr.Value}");
                    string tt = Request.Query["transtype"].ToString();
                    DateTime std = DateTime.Parse(Request.Query["UsageRepStDate"].ToString());
                    DateTime ed = DateTime.Parse(Request.Query["UsageRepEndDate"].ToString());
                    UsageRepStDate = std;
                    UsageRepEndDate = ed;
                }
                else
                {
                    UsageRepStDate = DateTime.Now.AddDays(-10);
                    UsageRepEndDate = DateTime.Now;
                }
                GetViewData().ConfigureAwait(false).GetAwaiter().GetResult();
                auditLogs.LogInformation($"End TransferReportView total time: {InitAuditLogs.StopStopWatch()} ms");
            }
            catch (Exception ex)
            {
                auditLogs.LogError($"ViewLookUpBatchesModel on get {ex.Message}");
                auditLogs.LogError($"End SpectrumMonitorReportsByCwidViewModel total time: {InitAuditLogs.StopStopWatch()} ms");
                return Redirect($"/BinUsers/DisplayErrorMessagesView?ErrMEss=Model SpectrumMonitorReportsByCwidViewModel OnGetAsync {ex.Message}");
            }
            return Page();
        }
        private async Task GetViewData()
        {
            string Cwid = ViewData["CWID"].ToString();
            SpectrumMonitorMenuRightsModel = ApiSetUserProfile.UserProfileInstance.GetBMMenuRights(Cwid, HttpContext.Session, auditLogs, WebApiUrl).ConfigureAwait(false).GetAwaiter().GetResult();
            ViewData[SqlConstants.ViewDataChangeCategories] = SpectrumMonitorMenuRightsModel.Categories;
            ViewData[SqlConstants.ViewDataEmailReports] = SpectrumMonitorMenuRightsModel.EmailReports;
            ViewData[SqlConstants.ViewDataRunReports] = SpectrumMonitorMenuRightsModel.RunReports;

        }
    }
}
