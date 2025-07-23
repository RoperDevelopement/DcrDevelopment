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
namespace BinMonitorAppService.Pages.LookUp
{

    public enum DropListDislOptions
    {

        [Display(Name = "AllBins")]
        AllBins,
        [Display(Name = "Registered Bins")]
        RegisteredBins,
        [Display(Name = "Processing Bins")]
        ProcessingBins

    }
    //public enum DropLisBinOptions
    //{

    //    [Display(Name = " ")]
    //    All,
    //    [Display(Name = "Registered")]
    //    Registerd,
    //    [Display(Name = "Not Registered")]
    //    NotRegistered,
    //    [Display(Name = "Processed")]
    //    Processed,
    //    [Display(Name = "Not Processed")]
    //    NotProcessed


    //}

    public class ViewLookUpBatchesModel : PageModel
    {
        private readonly IConfiguration configuration;
        private ILog auditLogs;
        public SpectrumMonitorMenuRightsModel SpectrumMonitorMenuRightsModel
        { get; set; }
        public ViewLookUpBatchesModel(IConfiguration config, ILog logConfig)
        {

            auditLogs = logConfig;
            configuration = config;
            Init();
        }
        private async Task GetLogInformaiton()
        {

            auditLogs = InitAuditLogs.LogAsync(auditLogs, HttpContext.Session).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private void Init()
        {
            TotalRegBinNotAssigned = 0;
            TotalRegBinNotStarted = 0;
            TotalRegBinNotCompleted = 0;
            TotalProcBinNotAssigned = 0;
            TotalProcBinNotStarted = 0;
            TotalProcBinNotCompleted = 0;
            TotalBinNotClosed = 0;
            Loop = 0;
            PagingLinks = new List<string>();


        }
        public int Loop
        { get; set; }
        private string WebApiUrl
        { get { return configuration.GetValue<string>("BinMonitorApi").ToString(); } }


        [BindProperty]
        public DropListDislOptions ReportType { get; set; }


        [BindProperty]
        public IDictionary<string, BinRegProcessModel> IDicBins
        { get; set; }
        public IDictionary<string, CategoryIdName> IDicCategory
        { get; set; }

        [BindProperty]
        public string RadioBtnRep
        { get; set; }



        [BindProperty]
        public IList<string> CategoryName
        { get; set; }
        [BindProperty]
        [Display(Name = "Bin Status Start Date")]
        [DataType(DataType.Date)]
        public DateTime BinStatusStartDate
        { get; set; }
        [BindProperty]
        [DataType(DataType.Date)]
        [Display(Name = "Bin Status End Date")]
        public DateTime BinStatusEndDate
        { get; set; }
        [BindProperty]
        // [Display(Name = "Display Comments Contents")]
        [Display(Name = "Show Comments Contents")]


        public bool DisplayCommentContents
        { get; set; }

        public int TotalRegBinNotAssigned
        { get; set; }

        public int TotalRegBinNotStarted
        { get; set; }
        public int TotalRegBinNotCompleted
        { get; set; }

        public int TotalProcBinNotAssigned
        { get; set; }

        public int TotalProcBinNotStarted
        { get; set; }
        public int TotalProcBinNotCompleted
        { get; set; }
        public int TotalBinNotClosed
        { get; set; }

        public List<string> PagingLinks
        { get; set; }
        public async Task<ActionResult> OnGetAsync(DateTime BinStatusStartDate, DateTime BinStatusEndDate, string CategoryName = null)
        {
            var qstr = Request.QueryString;
            try
            {
               
                InitAuditLogs.StartStopWatch();
                GetLogInformaiton().ConfigureAwait(false).GetAwaiter().GetResult();
                auditLogs.LogInformation("Start ViewLookUpBatchesModel on get");
                if (!(User.Identity.IsAuthenticated))
                {
                    auditLogs.LogInformation($"Renewing session ViewLookUpBatchesModel on get total time: {InitAuditLogs.StopStopWatch()} ms");
                    if (qstr.HasValue)
                        return Redirect($"/BinUsers/LoginView?returnUrl=/LookUp/ViewLookUpBatches{qstr.Value}");
                    else
                    return Redirect("/BinUsers/LoginView?returnUrl=/LookUp/ViewLookUpBatches");
                }
                ViewData["CWID"] = await GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyCwid);
                if (ViewData["CWID"] == null)
                {
                    auditLogs.LogInformation($"Renewing session ViewLookUpBatchesModel on get total time: {InitAuditLogs.StopStopWatch()} ms");
                    if (qstr.HasValue)
                    return Redirect($"/BinUsers/LoginViewreturnUrl=/LookUp/ViewLookUpBatches{qstr.Value}");
                else
                    return Redirect("/BinUsers/LoginView?returnUrl=/LookUp/ViewLookUpBatches");
                }

                if (BinStatusStartDate.Year > 1)
                {
                    if((BinStatusStartDate.Date > BinStatusEndDate.Date) || (BinStatusEndDate.Date < BinStatusStartDate.Date))
                    {
                        BinStatusStartDate = DateTime.Now;
                        BinStatusEndDate = DateTime.Now.AddDays(1);
                    }
                    OnPostAsync(CategoryName, BinStatusStartDate, BinStatusEndDate).ConfigureAwait(false).GetAwaiter().GetResult();
                    DisplayCommentContents = false;
                    string dcc = Request.Query["DisplayCommentContents"].ToString();
                    if (dcc.ToLower().Trim().StartsWith("true"))
                        DisplayCommentContents = true;
                    string rp = Request.Query["ReportType"].ToString();
                    if(Enum.TryParse(rp,out DropListDislOptions result))
                    {
                        ReportType = result;
                    }

                }
                 
                if (BinStatusStartDate.Year <= 1)
                {

                    SpectrumMonitorMenuRightsModel = ApiSetUserProfile.UserProfileInstance.GetBMMenuRights(User.Identity.Name, HttpContext.Session, auditLogs, WebApiUrl).ConfigureAwait(false).GetAwaiter().GetResult();
                    GetViewData().ConfigureAwait(true).GetAwaiter().GetResult();
                    auditLogs.LogInformation($"ViewLookUpBatchesModel on get srearch batches controller:{SqlConstants.ApiActiveBinsModel}");
                    await GetData(true, SqlConstants.ApiActiveBinsModel);
                    if (IDicBins.Count == 0)
                    {
                        BinStatusStartDate = DateTime.Now.AddDays(-30);
                        BinStatusEndDate = DateTime.Now;
                        string apiParams = string.Format(SqlConstants.ApiGetBinStatus, SqlConstants.NoValue, BinStatusStartDate.ToString(), BinStatusEndDate.ToString());
                        auditLogs.LogInformation($"ViewLookUpBatchesModel on get srearch batches controller:{apiParams} start date:{ BinStatusStartDate} end date: {BinStatusEndDate}");
                        await GetData(true, apiParams);
                        if (IDicBins.Count == 0)
                        {
                            auditLogs.LogWarning($"ViewLookUpBatchesModel on get no results for start date:{ BinStatusStartDate} end date: {BinStatusEndDate} total time: {InitAuditLogs.StopStopWatch()} ms redirect to /BinManager/ViewManagerHome");
                            //throw new Exception($"No records fround for date range {BinStatusStartDate.ToString()}-{BinStatusEndDate.ToString()}");
                            return Redirect("/BinManager/ViewManagerHome");
                        }
                    }
                }
                ViewData["CWID"] = User.Identity.Name;
                //     ViewData["UserProfile"] = await GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyUserProfile);
                auditLogs.LogInformation($"ViewLookUpBatchesModel lookup for start date:{ BinStatusStartDate} end date: {BinStatusEndDate}");
                auditLogs.LogInformation($"End LookUpDrCodesViewModel total time: {InitAuditLogs.StopStopWatch()} ms");
            }
            catch (Exception ex)
            {
                auditLogs.LogError($"ViewLookUpBatchesModel on get {ex.Message}");
                return Redirect($"/BinUsers/DisplayErrorMessagesView?ErrMEss=Model ViewLookUpBatchesModel OnGetAsync {ex.Message}");
            }
           
              
            return Page();
        }
        public async Task GetData(bool init, string apiParms)
        {
            string url = WebApiUrl;
            auditLogs.LogInformation($"ViewLookUpBatchesModel Method GetData weburl:{url} controll: {apiParms}");
            // IDicBins = await GetApis.GetApisInctance.ApiActiveBins(url,apiParms );
            IDicBins = await GetApis.GetApisInctance.ApiActiveBinsByBatchId(url, apiParms, false, auditLogs);
            if (IDicBins.Count > 0)
            {
                if (init)
                {
                    DisplayCommentContents = false;

                    BinStatusStartDate = IDicBins.Min(p => p.Value.ClosedCreatedAt);
                    BinStatusEndDate = IDicBins.Max(p => p.Value.ClosedCreatedAt);
                }
                auditLogs.LogInformation($"ViewLookUpBatchesModel Method GetData get categories weburl:{url} controll: {SqlConstants.ApiCatNameID}");
                CategoryName = await GetApis.GetApisInctance.ApiCategories(SqlConstants.ApiCatNameID, url, auditLogs,true);
                IDicBins.OrderBy(p => p.Value.BinID);
            }
            auditLogs.LogInformation($"ViewLookUpBatchesModel Method GetData get categories weburl:{url} controll: {SqlConstants.ApiCatNameID}");
            CategoryName = await GetApis.GetApisInctance.ApiCategories(SqlConstants.ApiCatNameID, url, auditLogs,true);
             
        }
        public async Task<IActionResult> OnPostAsync(string catName, DateTime BinStatusStartDate, DateTime BinStatusEndDate)
        {
            try
            {

                ViewData["CWID"] = await GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyCwid);
                if (ViewData["CWID"] == null)
                    return Redirect("/BinUsers/LoginView");
                InitAuditLogs.StartStopWatch();
                GetLogInformaiton().ConfigureAwait(false).GetAwaiter().GetResult();
                auditLogs.LogInformation("Start ViewLookUpBatchesModel on post");
                SpectrumMonitorMenuRightsModel = ApiSetUserProfile.UserProfileInstance.GetBMMenuRights(User.Identity.Name, HttpContext.Session, auditLogs, WebApiUrl).ConfigureAwait(false).GetAwaiter().GetResult();
                GetViewData().ConfigureAwait(true).GetAwaiter().GetResult();

                string url = WebApiUrl;
                if (string.IsNullOrWhiteSpace(catName))
                    catName = SqlConstants.NoValue;
                auditLogs.LogInformation($"ViewLookUpBatchesModel on post category name: {catName}");

                string apiParams = string.Format(SqlConstants.ApiGetBinStatus, catName, BinStatusStartDate.ToString(), BinStatusEndDate.ToString());
                auditLogs.LogInformation($"ViewLookUpBatchesModel on post get data api prams: {apiParams}");
                await GetData(false, apiParams);

                //   ViewData["UserProfile"] = await GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyUserProfile);
                auditLogs.LogInformation($"End LookUpDrCodesViewModel total time: {InitAuditLogs.StopStopWatch()} ms");

            }
            catch (Exception ex)
            {
                auditLogs.LogError($"ViewLookUpBatchesModel on post {ex.Message}");
                return Redirect($"/BinUsers/DisplayErrorMessagesView?ErrMEss=Model ViewLookUpBatchesModel OnGetAsync {ex.Message}");


            }

            return Page();
        }
        private async Task GetViewData()
        {
            ViewData[SqlConstants.ViewDataChangeCategories] = SpectrumMonitorMenuRightsModel.Categories;
            ViewData[SqlConstants.ViewDataEmailReports] = SpectrumMonitorMenuRightsModel.EmailReports;
            ViewData[SqlConstants.ViewDataRunReports] = SpectrumMonitorMenuRightsModel.RunReports;

        }
    }
}