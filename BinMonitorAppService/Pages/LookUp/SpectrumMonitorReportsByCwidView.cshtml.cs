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
    public enum DropListBinOptionOptions
    {

        [Display(Name = "CreatedBY")]
        CreatedBY,
        [Display(Name = "ProcessedBY")]
        ProcessedBy,
        [Display(Name = "ClosedBY")]
        ClosedBY

    }
    public enum DropListBinCwidRepOptions
    {
        [Display(Name = "All")]
        All,
        [Display(Name = "Register")]
        Register,
        [Display(Name = "Processing")]
        Processing,
        [Display(Name = "Closed")]
        Closed

    }
    public class SpectrumMonitorReportsByCwidViewModel : PageModel
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

        [BindProperty]
        [Display(Name = "Category")]
        [DataType(DataType.Text)]
        public IList<string> CategoryName
        { get; set; }
        public SpectrumMonitorMenuRightsModel SpectrumMonitorMenuRightsModel
        { get; set; }

        

        [BindProperty]
        [Display(Name = "Bin Status")]
        [DataType(DataType.Text)]
        public DropListBinCwidRepOptions ReportType { get; set; }
        [BindProperty]
        [Display(Name = "Cwid AssignedTo")]
        [DataType(DataType.Text)]
        public IList<string> ActiveUsers
        { get; set; }
        [BindProperty]
        public IDictionary<string, BinRegProcessModel> IDicBins
        { get; set; }
        public IDictionary<string, ReportByCWIDCatStatusModel> ReportByCWIDCatStatus
        { get; set; }
        public SpectrumMonitorReportsByCwidViewModel(IConfiguration config, ILog logConfig)
        {

            auditLogs = logConfig;
            configuration = config;

        }
        private async Task GetLogInformaiton()
        {

            auditLogs = InitAuditLogs.LogAsync(auditLogs, HttpContext.Session).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        public string BinStatus
        { get; set; }
        //   public async Task<ActionResult> OnGetAsync(DateTime UsageRepStDate, DateTime UsageRepEndDate, string ActiveUsers = null)
        public async Task<ActionResult> OnGetAsync(string ActiveUsers = null, string CategoryName = null)
        {

            InitAuditLogs.StartStopWatch();
            GetLogInformaiton().ConfigureAwait(false).GetAwaiter().GetResult();
            auditLogs.LogInformation("Start OnGetAsync SpectrumMonitorReportsByCwidViewModel");

            var qstr = Request.QueryString;

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
                    string rp = Request.Query["ReportType"].ToString();
                    DateTime std = DateTime.Parse(Request.Query["UsageRepStDate"].ToString());
                    DateTime ed = DateTime.Parse(Request.Query["UsageRepEndDate"].ToString());
                    string export = Request.Query["export"].ToString();
                    auditLogs.LogInformation($"Processing query str {qstr.Value} ms");
                    if (Enum.TryParse(rp, out DropListBinCwidRepOptions result))
                    {
                        ReportType = result;
                    }
                    BinStatus = ReportType.ToString();
                    auditLogs.LogInformation($"Getting report for report type {ReportType.ToString()} ms");
                    IDicBins = GetApis.GetApisInctance.ApiReportByCwid($"{WebApiUrl}{SqlConstants.ApiSpecMonitorReportsByCwidController}", ActiveUsers, ReportType.ToString(), SqlConstants.SpGetBinsReportsByCwid, std, ed, CategoryName, auditLogs).ConfigureAwait(false).GetAwaiter().GetResult();
                    GenerateReport().ConfigureAwait(false).GetAwaiter().GetResult();
                     

                    UsageRepStDate = std;
                    UsageRepEndDate = ed;
                    //if (!(string.IsNullOrWhiteSpace(export)))
                    //{
                    //    FileContentResult fileContentResult = ExportUsers().ConfigureAwait(false).GetAwaiter().GetResult();
                    //    return fileContentResult;
                    //}
                }
                else
                {
                    UsageRepStDate = DateTime.Now.AddDays(-10);
                    UsageRepEndDate = DateTime.Now;
                }

                GetViewData().ConfigureAwait(false).GetAwaiter().GetResult();
                Init().ConfigureAwait(false).GetAwaiter().GetResult();

                auditLogs.LogInformation($"End SpectrumMonitorReportsByCwidViewModel total time: {InitAuditLogs.StopStopWatch()} ms");
            }
            catch (Exception ex)
            {
                auditLogs.LogError($"ViewLookUpBatchesModel on get {ex.Message}");
                auditLogs.LogError($"End SpectrumMonitorReportsByCwidViewModel total time: {InitAuditLogs.StopStopWatch()} ms");
                return Redirect($"/BinUsers/DisplayErrorMessagesView?ErrMEss=Model SpectrumMonitorReportsByCwidViewModel OnGetAsync {ex.Message}");
            }
            return Page();
        }
        public async Task GenerateReport()
        {
            if ((IDicBins != null) && (IDicBins.Count() > 0))
            {
                ReportByCWIDCatStatus = new Dictionary<string, ReportByCWIDCatStatusModel>();
                foreach (var bins in IDicBins)
                {
                    string key = $"{bins.Value.BinID}{bins.Value.CategoryName}{bins.Value.RegCreatedBy}";
                    if (ReportByCWIDCatStatus.ContainsKey(key))
                    {
                        if (ReportByCWIDCatStatus.TryGetValue(key, out ReportByCWIDCatStatusModel value))
                        {
                            value.TotalVol++;

                          //  value.CompleteDuration = GetBiggerTime(bins.Value.CompleteDuration, value.CompleteDuration).ConfigureAwait(false).GetAwaiter().GetResult();
                           // value.RegDuration = GetBiggerTime(bins.Value.RegDuration, value.RegDuration).ConfigureAwait(false).GetAwaiter().GetResult();
                         //   value.ProcessDuration = GetBiggerTime(bins.Value.ProcessDuration, value.ProcessDuration).ConfigureAwait(false).GetAwaiter().GetResult();
                            ReportByCWIDCatStatus[key] = value;
                        }
                    }
                    else
                    {
                        ReportByCWIDCatStatusModel reportByCWIDCatStatusModel = new ReportByCWIDCatStatusModel();
                        reportByCWIDCatStatusModel.BinStatusClosed = reportByCWIDCatStatusModel.BinStatusProcessing = reportByCWIDCatStatusModel.BinStatusReg = "Open";

                        if (bins.Value.BinCompletedAt.Year > 2000)
                            reportByCWIDCatStatusModel.BinStatusClosed = "Closed";
                        if (bins.Value.ProcessCompletedAt.Year > 2000)

                            reportByCWIDCatStatusModel.BinStatusProcessing = "Closed";
                        if (bins.Value.RegCompletedAt.Year > 2000)
                            reportByCWIDCatStatusModel.BinStatusReg = "Closed";
                        reportByCWIDCatStatusModel.BinID = bins.Value.BinID;
                        reportByCWIDCatStatusModel.CategoryName = bins.Value.CategoryName;
                        reportByCWIDCatStatusModel.RegDuration = bins.Value.RegDuration;
                        reportByCWIDCatStatusModel.ProcessDuration = bins.Value.ProcessDuration;
                        reportByCWIDCatStatusModel.CompleteDuration = bins.Value.CompleteDuration;
                        reportByCWIDCatStatusModel.RegAssignedTo = bins.Value.RegAssignedTo;
                        reportByCWIDCatStatusModel.ProcessAssignedTo = bins.Value.ProcessAssignedTo;
                        reportByCWIDCatStatusModel.BinClosedBy = bins.Value.BinClosedBy;
                        reportByCWIDCatStatusModel.TotalVol = 1;
                        ReportByCWIDCatStatus.Add(key, reportByCWIDCatStatusModel);
                    }
                }
            }
        }
        private async Task<string> GetBiggerTime(string completeDur1, string completeDur2)
        {
            string[] dur1 = completeDur1.Split(":");
            string[] dur2 = completeDur2.Split(":");
            int totDur1 = 0;
            int totDur2 = 0;
            string totDur = string.Empty;
            foreach (string str in dur1)
            {
                totDur += str;
            }
            totDur1 = int.Parse(totDur.Trim());
            totDur = string.Empty;
            foreach (string str in dur2)
            {
                totDur += str;
            }
            totDur2 = int.Parse(totDur.Trim());
            if (totDur2 > totDur1)
                return completeDur2;
            return completeDur1;
        }
        //private async Task<FileContentResult> ExportUsers()
        //{
        //    auditLogs.LogInformation("Method ExportUsers");

        //    // GetLabRecUsers().ConfigureAwait(true).GetAwaiter().GetResult();
        //    if (UsageReport.Count > 0)
        //    {
        //        StringBuilder sb = new StringBuilder();
        //        sb.AppendLine("Bin Status,Bin ID,Category Name,Total Reqs,Category Duration");
        //        foreach (KeyValuePair<string, ReportByCWIDCatStatusModel> keyValuePair in ReportByCWIDCatStatus)
        //        {
        //            sb.AppendLine($@"{keyValuePair.Value.BinStatusProcessing},{keyValuePair.Value.BinID},{keyValuePair.Value.CategoryName},{keyValuePair.Value.TotalVol},{keyValuePair.Value.CompleteDuration");
        //        }

        //        // Response.Headers.Add("Content-Disposition", "attachment;filename=LabReqUsers.csv");
        //        return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", $"UsageReport_{DateTime.Now.ToString("MM-dd-yyyy_HH:mm:ss")}.csv");
        //    }
        //    return null;
        //}
        private async Task Init()
        {
            ActiveUsers = GetApis.GetApisInctance.ApiGetActiveBinUsers(WebApiUrl, SqlConstants.ApiSpecMonitorReportsByCwidController, auditLogs).ConfigureAwait(true).GetAwaiter().GetResult();
            CategoryName = await GetApis.GetApisInctance.ApiCategories(SqlConstants.ApiCatNameID, WebApiUrl, auditLogs,false);
            CategoryName.Insert(0, "All Categories");
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
