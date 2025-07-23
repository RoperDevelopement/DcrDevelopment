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
    public enum DropListChartTypes
    {
        [Display(Name = "bar")]
        bar,
        [Display(Name = "pie")]
        pie,
        [Display(Name = "line")]
        line

    }
    public class SpecMonitorChartsViewModel : PageModel
    {
        private string WebApiUrl
        { get { return configuration.GetValue<string>("BinMonitorApi").ToString(); } }
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
        public SpectrumMonitorMenuRightsModel SpectrumMonitorMenuRightsModel
        { get; set; }
        [BindProperty]
        [Display(Name = "Bin Status")]
        [DataType(DataType.Text)]
        public DropListBinOptionOptions ReportType { get; set; }
        public DropListChartTypes TypeOfChart { get; set; }
        public IDictionary<string, SpectrumUsageReportsModel> UsageReport
        {
            get; set;
        }
        public IList<ChartModel> UsageReportChart
        {
            get; set;
        }
        public string SelectedChartType
        { get; set; }
        public SpecMonitorChartsViewModel(IConfiguration config, ILog logConfig)
        {
            UsageReportChart = new List<ChartModel>();
            auditLogs = logConfig;
            configuration = config;

        }
        private async Task GetLogInformaiton()
        {

            auditLogs = InitAuditLogs.LogAsync(auditLogs, HttpContext.Session).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public async Task<ActionResult> OnGetAsync()
        {
            //pie bar line' 
            SelectedChartType = "pie";
            var qstr = Request.QueryString;
            InitAuditLogs.StartStopWatch();
            GetLogInformaiton().ConfigureAwait(false).GetAwaiter().GetResult();
            auditLogs.LogInformation("Start OnGetAsync SpectrumMonitorReportsByCwidViewModel");

            if (!(User.Identity.IsAuthenticated))
            {
                auditLogs.LogInformation($"Renewing session SpecMonitorChartsView on get total time: {InitAuditLogs.StopStopWatch()} ms");
                if (qstr.HasValue)
                    return Redirect($"/BinUsers/LoginViewreturnUrl?returnUrl=/LookUp/SpecMonitorChartsView{qstr.Value}");
                else
                    return Redirect("/LookUp/SpecMonitorChartsView?returnUrl=/LookUp/SpecMonitorChartsView");
            }
            ViewData["CWID"] = await GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyCwid);
            if (ViewData["CWID"] == null)
            {
                auditLogs.LogInformation($"Renewing session SpecMonitorChartsView on get total time: {InitAuditLogs.StopStopWatch()} ms");
                if (qstr.HasValue)
                    return Redirect($"/BinUsers/LoginViewreturnUrl?returnUrl=/LookUp/SpecMonitorChartsView{qstr.Value}");
                else
                    return Redirect("/BinUsers/LoginViewreturnUrl?returnUrl=/LookUp/SpecMonitorChartsView");
            }
            try
            {
                if (qstr.HasValue)
                {
                    auditLogs.LogInformation($"OnGetAsync SpectrumMonitorReportsByCwidViewModel query string value {qstr.Value}");
                    string rp = Request.Query["ReportType"].ToString();
                    string ct = Request.Query["TypeOfChart"].ToString();

                    DateTime std = DateTime.Parse(Request.Query["UsageRepStDate"].ToString());
                    DateTime ed = DateTime.Parse(Request.Query["UsageRepEndDate"].ToString());
                    if (ed.Date.Day == DateTime.Now.Date.Day)
                    {
                        ed = ed.AddDays(1);
                    }
                    if (Enum.TryParse(rp, out DropListBinOptionOptions result))
                    {
                        ReportType = result;
                    }
                    if (Enum.TryParse(ct, out DropListChartTypes resultChartType))
                    {
                        TypeOfChart = resultChartType;
                    }
                    auditLogs.LogInformation($"OnGetAsync SpectrumMonitorReportsByCwidViewModel getting usage report type {ReportType.ToString()} for chart type {TypeOfChart.ToString()} for weburi {WebApiUrl} sp {SqlConstants.ApiSpecMonitorReportsByCwidController} all cwids start date {std} end date {ed} ");
                    UsageReport = GetApis.GetApisInctance.ApiUsageReports(WebApiUrl, SqlConstants.SpUsageReportByCWID, SqlConstants.ApiSpecMonitorReportsByCwidController, "na", ReportType.ToString(), std, ed, auditLogs).ConfigureAwait(false).GetAwaiter().GetResult();
                    UsageRepStDate = std;
                    UsageRepEndDate = ed;
                    if ((UsageReport != null) && (UsageReport.Count > 0))
                        GetChartTotals().ConfigureAwait(false).GetAwaiter().GetResult();
                }


                else
                {
                    UsageRepStDate = DateTime.Now.AddDays(-10);
                    UsageRepEndDate = DateTime.Now;
                }
                ViewData["CWID"] = User.Identity.Name;
                GetViewData().ConfigureAwait(false).GetAwaiter().GetResult();

                auditLogs.LogInformation($"End SpectrumMonitorReportsByCwidViewModel total time: {InitAuditLogs.StopStopWatch()} ms");
            }
            catch (Exception ex)
            {
                auditLogs.LogError($"End SpectrumMonitorReportsByCwidViewModel total time: {InitAuditLogs.StopStopWatch()} ms");
                auditLogs.LogError($"SpectrumMonitorReportsByCwidViewModel on get {ex.Message}");
                return Redirect($"/BinUsers/DisplayErrorMessagesView?ErrMEss=Model SpectrumMonitorReportsByCwidViewModel  OnGetAsync {ex.Message}");
            }

            // BinStatusStartDate = st;
            //  BinStatusEndDate = se;
            return Page();
        }

        private async Task GetChartTotals()
        {
            auditLogs.LogInformation("method GetChartTotals");
            ChartModel chartModel = null;
            foreach (var cwid in UsageReport)
            {
                chartModel = new ChartModel();
                chartModel.CWID = cwid.Value.CWID;
                chartModel.TotalLabReqs = cwid.Value.TotalLabReqs;
                if (UsageReportChart.Count > 0)
                {
                    var toUpdate = UsageReportChart.SingleOrDefault(x => x.CWID == chartModel.CWID);

                    if (toUpdate != null)
                    {
                        toUpdate.TotalLabReqs += cwid.Value.TotalLabReqs;
                    }
                    else
                        UsageReportChart.Add(chartModel);
                }
                else
                {
                    UsageReportChart.Add(chartModel);
                }

            }

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
