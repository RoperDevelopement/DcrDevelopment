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
    public class SpecMonitorDurationByCWIDViewModel : PageModel
    {

        private string WebApiUrl
        { get { return configuration.GetValue<string>("BinMonitorApi").ToString(); } }
        private string VolTotRepSqlConnectionStr
        {
            get { return configuration.GetValue<string>("BinMonitorCloudConnectionString").ToString(); }
        }
        public IDictionary<string, ReportVolDurModel> RepDurVol
        { get; set; }
        [BindProperty]
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime UsageRepStDate
        { get; set; }

        [BindProperty]
        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime UsageRepEndDate
        { get; set; }
        private readonly IConfiguration configuration;
        private ILog auditLogs;

        [BindProperty]
        [Display(Name = "Bin Status")]
        [DataType(DataType.Text)]
        public DropListBinOptionOptions ReportType { get; set; }
        [BindProperty]
        [Display(Name = "Cwid")]
        [DataType(DataType.Text)]
        public IList<string> ActiveUsers
        { get; set; }
        public SpectrumMonitorMenuRightsModel SpectrumMonitorMenuRightsModel
        { get; set; }
        public SpecMonitorDurationByCWIDViewModel(IConfiguration config, ILog logConfig)
        {

            auditLogs = logConfig;
            configuration = config;

        }

        private async Task GetLogInformaiton()
        {

            auditLogs = InitAuditLogs.LogAsync(auditLogs, HttpContext.Session).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        public async Task<ActionResult> OnGetAsync(string ActiveUsers = null)
        {
            InitAuditLogs.StartStopWatch();
            GetLogInformaiton().ConfigureAwait(false).GetAwaiter().GetResult();
            var qstr = Request.QueryString;
            auditLogs.LogInformation("Start OnGetAsync SpecMonitorDurationByCWIDViewModel");
            if (!(User.Identity.IsAuthenticated))
            {
                auditLogs.LogInformation($"ReNewing session for SpectrumMonitorReportsByCwidViewModel total time: {InitAuditLogs.StopStopWatch()} ms");
                if (qstr.HasValue)
                {

                    return Redirect($"/BinUsers/LoginView?returnUrl=/LookUp/SpectrumMonitorReportsByCwidView{qstr.Value}");
                }

                else
                    return Redirect($"/BinUsers/LoginView?returnUrl=/LookUp/SpectrumMonitorReportsByCwidView");
            }
            ViewData["CWID"] = User.Identity.Name;
            try
            {


                if (qstr.HasValue)
                {
                    DateTime std = DateTime.Parse(Request.Query["UsageRepStDate"].ToString());
                    DateTime ed = DateTime.Parse(Request.Query["UsageRepEndDate"].ToString());
                    auditLogs.LogInformation($"User {User.Identity.Name} running report for CWID {ActiveUsers} for date range {std}-{ed}");
                    RepDurVol = InternalSqlApi.InstanceSqlApi.ReportsByVolDur(SqlConstants.SpGetReportVolumeTotalDurByCWID, VolTotRepSqlConnectionStr, ActiveUsers, std, ed).ConfigureAwait(false).GetAwaiter().GetResult();
                    UsageRepStDate = std;
                    UsageRepEndDate = ed;
                    string export = Request.Query["export"].ToString();
                    if (!(string.IsNullOrWhiteSpace(export)))
                    {
                        FileContentResult fileContentResult = ExportVolDurRep().ConfigureAwait(false).GetAwaiter().GetResult();
                        return fileContentResult;
                    }
                }
                else
                {
                    UsageRepStDate = DateTime.Now.AddDays(-10);
                    UsageRepEndDate = DateTime.Now;
                }
                GetViewData().ConfigureAwait(false).GetAwaiter().GetResult();
                Init().ConfigureAwait(false).GetAwaiter().GetResult();
                auditLogs.LogInformation($"End SpecMonitorDurationByCWIDViewModel total time: {InitAuditLogs.StopStopWatch()} ms");

            }

            catch (Exception ex)
            {
                auditLogs.LogError($"End SpecMonitorDurationByCWIDViewModel total time: {InitAuditLogs.StopStopWatch()} ms");
                auditLogs.LogError($"SpecMonitorDurationByCWIDViewModel on get {ex.Message}");
                return Redirect($"/BinUsers/DisplayErrorMessagesView?ErrMEss=Model SpectrumMonitorReportsByCwidViewModel  OnGetAsync {ex.Message}");
            }
            return Page();

        }

        private async Task<FileContentResult> ExportVolDurRep()
        {
            auditLogs.LogInformation("Method ExportVolDurRep");

            // GetLabRecUsers().ConfigureAwait(true).GetAwaiter().GetResult();
            if ((RepDurVol != null) && (RepDurVol.Count > 0))
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("CWID,Volume,Duration");
                foreach (KeyValuePair<string, ReportVolDurModel> keyValuePair in RepDurVol)
                {
                    sb.AppendLine($@"{keyValuePair.Key},{keyValuePair.Value.TotalVolume},{keyValuePair.Value.TotalDur}");
                }

                // Response.Headers.Add("Content-Disposition", "attachment;filename=LabReqUsers.csv");
                return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", $"UsageReportByVolDur_{DateTime.Now.ToString("MM-dd-yyyy_HH:mm:ss")}.csv");
            }
            return null;
        }
        private async Task GetViewData()
        {
            string Cwid = ViewData["CWID"].ToString();
            SpectrumMonitorMenuRightsModel = ApiSetUserProfile.UserProfileInstance.GetBMMenuRights(Cwid, HttpContext.Session, auditLogs, WebApiUrl).ConfigureAwait(false).GetAwaiter().GetResult();
            ViewData[SqlConstants.ViewDataChangeCategories] = SpectrumMonitorMenuRightsModel.Categories;
            ViewData[SqlConstants.ViewDataEmailReports] = SpectrumMonitorMenuRightsModel.EmailReports;
            ViewData[SqlConstants.ViewDataRunReports] = SpectrumMonitorMenuRightsModel.RunReports;

        }
        private async Task Init()
        {
            ActiveUsers = GetApis.GetApisInctance.ApiGetActiveBinUsers(WebApiUrl, SqlConstants.ApiSpecMonitorReportsByCwidController, auditLogs).ConfigureAwait(true).GetAwaiter().GetResult();
        }
    }
}
