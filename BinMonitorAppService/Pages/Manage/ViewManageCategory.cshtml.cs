using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using BinMonitorAppService.Models;

using System.ComponentModel.DataAnnotations;
using BinMonitorAppService.Constants;
using BinMonitorAppService.ApiClasses;

using Microsoft.AspNetCore.Http;
using BinMonitorAppService.Logging;

namespace BinMonitorAppService.Pages.Manage
{
    public class ViewManageCategoryModel : PageModel
    {
        private readonly IConfiguration configuration;

        private ILog auditLogs;
        public SpectrumMonitorMenuRightsModel SpectrumMonitorMenuRightsModel
        { get; set; }

        private string WebApiUrl
        { get { return configuration.GetValue<string>("BinMonitorApi").ToString(); } }

        public IDictionary<string, CategoryColorModel> CategoryColors
        { get; set; }
        public IDictionary<string, CategoryColorCodesModel> CategoryColorCodesModel
        { get; set; }

        public CategoryCheckPointEmailModel CategoryCheckPointEmailModel
        { get; set; }

        public CategoryCheckPointModel CategoryCheckPointModel
        { get; set; }
        public IList<string> EmailTo
        { get; set; }
        public CategoryColorModel CategoryColor
        { get; set; }
        private bool FoundChanges
        { get; set; }
        public string SessionExpires
        { get; set; }
        public ViewManageCategoryModel(IConfiguration config, ILog logConfig)
        {

            configuration = config;

            auditLogs = logConfig;

        }
        private async Task GetLogInformaiton()
        {

            auditLogs = InitAuditLogs.LogAsync(auditLogs, HttpContext.Session).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        public async Task<ActionResult> OnGetAsync()
        {
            try
            {
                InitAuditLogs.StartStopWatch();
                GetLogInformaiton().ConfigureAwait(false).GetAwaiter().GetResult();
                auditLogs.LogInformation($"Start ViewManageCategoryModel on get");
                if (!(User.Identity.IsAuthenticated))
                {
                    auditLogs.LogInformation($"Session Expired ViewManageCategoryModel on get  total time: {InitAuditLogs.StopStopWatch()} ms");
                    return Redirect("/BinUsers/LoginView?returnUrl=/Manage/ViewManageCategory");
                }

                ViewData["CWID"] = await GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyCwid);
                if (ViewData["CWID"] == null)
                {
                    auditLogs.LogInformation($"Session Expired ViewManageCategoryModel on get  total time: {InitAuditLogs.StopStopWatch()} ms");
                    return Redirect("/BinUsers/LoginView?returnUrl=/Manage/ViewManageCategory");
                }

                int totMinutesLeft = ApiSetUserProfile.UserProfileInstance.GetSessionTime(HttpContext.Session, 1).ConfigureAwait(false).GetAwaiter().GetResult();
                auditLogs.LogInformation($"Session time left: {totMinutesLeft.ToString()} ms");
                if (totMinutesLeft < 0)
                    return Redirect("/BinUsers/LoginView?returnUrl=/Manage/ViewManageCategory");
                SessionExpires = totMinutesLeft.ToString();

                SpectrumMonitorMenuRightsModel = ApiSetUserProfile.UserProfileInstance.GetBMMenuRights(User.Identity.Name, HttpContext.Session, auditLogs, WebApiUrl).ConfigureAwait(false).GetAwaiter().GetResult();
                GetViewData().ConfigureAwait(true).GetAwaiter().GetResult();
                await Init();
             //   ViewData["UserProfile"] = await GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyUserProfile);
                auditLogs.LogInformation($"End ViewManageCategoryModel on get total time: {InitAuditLogs.StopStopWatch()} ms");
                
            }
            catch (Exception ex)
            {
                auditLogs.LogError($"ViewManageCategoryModel total time: {InitAuditLogs.StopStopWatch()} ms {ex.Message}");
                return Redirect($"/BinUsers/DisplayErrorMessagesView?ErrMEss=Model ViewManageCategoryModel OnGetAsync {ex.Message}");
                
                
            }
            return Page();
        }
        private async Task Init()
        {
            //  CategoryCheckPointModel = new CategoryCheckPointModel();
            auditLogs.LogInformation($"ViewManageCategoryModel Method Init CategoryColors weburl: {WebApiUrl} controller: {SqlConstants.WebApiBinMonitor}{SqlConstants.SpGetCategoryColors}");
            CategoryColors = await GetApis.GetApisInctance.ApiCategoriesColors($"{SqlConstants.WebApiBinMonitor}{SqlConstants.SpGetCategoryColors}", WebApiUrl, false, auditLogs);
            auditLogs.LogInformation($"ViewManageCategoryModel Method Init CategoryColorCodesModel weburl: {WebApiUrl} controller: {SqlConstants.WebApiBinMonitor}{SqlConstants.SpColorCodesModel}");
            CategoryColorCodesModel = await GetApis.GetApisInctance.ApiCategoriesColorsCodes($"{SqlConstants.WebApiBinMonitor}{SqlConstants.SpColorCodesModel}", WebApiUrl, auditLogs);
            auditLogs.LogInformation($"ViewManageCategoryModel Method Init email to weburl: {WebApiUrl} controller: {SqlConstants.WebApiBinMonitor}{SqlConstants.SpGetUserInfo}");
            EmailTo = await GetApis.GetApisInctance.ApiUserInfo($"{SqlConstants.WebApiBinMonitor}{SqlConstants.SpGetUserInfo}", WebApiUrl, auditLogs);
        }
        private void AddSelectCat()
        {
            auditLogs.LogInformation($"ViewManageCategoryModel Method AddSelectCat");
            CategoryColorModel keyValuePairs = new CategoryColorModel();
            keyValuePairs.Selected = true;
            keyValuePairs.CategoryName = "Select Category";
            CategoryColors.Add(keyValuePairs.CategoryName, keyValuePairs);
        }

        public async Task<IActionResult> OnPostAsync(CategoryCheckPointEmailModel CategoryCheckPointEmailModel, CategoryCheckPointModel CategoryCheckPointModel)
        {

            try
            {
                if (!(User.Identity.IsAuthenticated))
                {
                    return Redirect("/BinUsers/LoginView?returnUrl=/Manage/ViewManageCategory");
                }

                ViewData["CWID"] = await GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyCwid);
                if (ViewData["CWID"] == null)
                    return Redirect("/BinUsers/LoginView?returnUrl=/Manage/ViewManageCategory");
                InitAuditLogs.StartStopWatch();
                GetLogInformaiton().ConfigureAwait(false).GetAwaiter().GetResult();
                auditLogs.LogInformation("Start ViewManageCategoryModel post");
                SpectrumMonitorMenuRightsModel = ApiSetUserProfile.UserProfileInstance.GetBMMenuRights(User.Identity.Name, HttpContext.Session, auditLogs, WebApiUrl).ConfigureAwait(false).GetAwaiter().GetResult();
                GetViewData().ConfigureAwait(true).GetAwaiter().GetResult();
                if ((!(string.IsNullOrWhiteSpace(Request.Form["rchanges"]))) || (!(string.IsNullOrWhiteSpace(Request.Form["btnreset"]))))
                {
                    auditLogs.LogInformation($"End ViewManageCategoryModel total time: {InitAuditLogs.StopStopWatch()} ms button reset or exit redirect to Manage/ViewManageCategory");
                    return Redirect("/BinUsers/LoginView?returnUrl=/Manage/ViewManageCategory");
                }
                else if ((!(string.IsNullOrWhiteSpace(Request.Form["btnsave"]))) || (!(string.IsNullOrWhiteSpace(Request.Form["schanges"]))))
                {
                    auditLogs.LogInformation("ViewManageCategoryModel save category changes");
                    await CheckChanges();
                    CategoryCheckPointEmailModel.CategoryName = Request.Form["catid"];
                    CategoryCheckPointModel.CategoryName = Request.Form["catid"];
                    auditLogs.LogInformation($"ViewManageCategoryModel Update Category: Checkpoint email category name: {CategoryCheckPointEmailModel.CategoryName} category name: {CategoryCheckPointModel.CategoryName}");
                    await UpDateCategoryCheckPoints(CategoryCheckPointModel);
                    auditLogs.LogInformation($"ViewManageCategoryModel Check category dur: Checkpoint email category name: {CategoryCheckPointEmailModel.Duration}");
                    await CheckDurChanges(CategoryCheckPointEmailModel);

                }

                else
                {
                    auditLogs.LogInformation($"End ViewManageCategoryModel total time: {InitAuditLogs.StopStopWatch()} ms button reset or exit redirect to ../BinManager/ViewManagerHome");
                    return Redirect("/BinUsers/LoginView?returnUrl=/Manage/ViewManageCategory");
                }
                    
                await Init();

              //  ViewData["UserProfile"] = await GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyUserProfile);
                
                int totMinutesLeft = ApiSetUserProfile.UserProfileInstance.GetSessionTime(HttpContext.Session, 1).ConfigureAwait(false).GetAwaiter().GetResult();
                
                auditLogs.LogInformation($"End ViewManageCategoryModel total time: {InitAuditLogs.StopStopWatch()} ms");
                auditLogs.LogInformation($"Session time left: {totMinutesLeft.ToString()} ms");
                if (totMinutesLeft < 0)
                    return Redirect("/BinUsers/LoginView?returnUrl=/Manage/ViewManageCategory");
                SessionExpires = totMinutesLeft.ToString();


            }
            catch (Exception ex)
            {
                auditLogs.LogError($"End ViewManageCategoryModel total time: {InitAuditLogs.StopStopWatch()} ms {ex.Message}");
                return Redirect($"/BinUsers/DisplayErrorMessagesView?ErrMEss=Model ViewManageCategoryModel OnGetAsync {ex.Message}");

            }
            return Page();
        }
        private async Task GetViewData()
        {
            ViewData[SqlConstants.ViewDataChangeCategories] = SpectrumMonitorMenuRightsModel.Categories;
            ViewData[SqlConstants.ViewDataEmailReports] = SpectrumMonitorMenuRightsModel.EmailReports;
            ViewData[SqlConstants.ViewDataRunReports] = SpectrumMonitorMenuRightsModel.RunReports;

        }
        private async Task UpDateCategoryCheckPoints(CategoryCheckPointModel CategoryCheckPointModel)
        {
            
            string nColor = Request.Form["newColor1"];
            auditLogs.LogInformation($"ViewManageCategoryModel Method UpDateCategoryCheckPoints new color: {nColor}");
            if (!(string.IsNullOrWhiteSpace(nColor)))
            {
                CategoryCheckPointModel.CategoryColorCheckPointOne = Request.Form["newColor1"];
                auditLogs.LogInformation($"ViewManageCategoryModel Method UpDateCategoryCheckPoints new color CategoryColorCheckPointOne: {CategoryCheckPointModel.CategoryColorCheckPointOne}");

            }
            else
                CategoryCheckPointModel.CategoryColorCheckPointOne = Request.Form["CategoryColorCheckPointOne"];
            if (!(string.IsNullOrWhiteSpace(Request.Form["newColor2"])))
            {
                CategoryCheckPointModel.CategoryColorCheckPointTwo = Request.Form["newColor2"];
                auditLogs.LogInformation($"ViewManageCategoryModel Method UpDateCategoryCheckPoints new color CategoryColorCheckPointTwo: {CategoryCheckPointModel.CategoryColorCheckPointTwo}");

            }
            else
                CategoryCheckPointModel.CategoryColorCheckPointTwo = Request.Form["CategoryColorCheckPointTwo"];
            if (!(string.IsNullOrWhiteSpace(Request.Form["newColor3"])))
            {
                CategoryCheckPointModel.CategoryColorCheckPointThree = Request.Form["newColor3"];
                auditLogs.LogInformation($"ViewManageCategoryModel Method UpDateCategoryCheckPoints new color CategoryColorCheckPointThree: {CategoryCheckPointModel.CategoryColorCheckPointThree}");

            }
            else
                CategoryCheckPointModel.CategoryColorCheckPointThree = Request.Form["CategoryColorCheckPointThree"];
            if (!(string.IsNullOrWhiteSpace(Request.Form["newColor4"])))
            {
                CategoryCheckPointModel.CategoryColorCheckPointFour = Request.Form["newColor4"];
                auditLogs.LogInformation($"ViewManageCategoryModel Method UpDateCategoryCheckPoints new color CategoryColorCheckPointFour: {CategoryCheckPointModel.CategoryColorCheckPointFour}");

            }
            else
                CategoryCheckPointModel.CategoryColorCheckPointFour = Request.Form["CategoryColorCheckPointFour"];
            auditLogs.LogInformation($"ViewManageCategoryModel Method UpDateCategoryCheckPoints new color webur: {WebApiUrl} controller:{SqlConstants.ApiUpDateCategoryCheckPoints}");
            await PostApis.PostApisIntance.ApiUpdateCategoryCheckPoints(WebApiUrl, SqlConstants.ApiUpDateCategoryCheckPoints, CategoryCheckPointModel,auditLogs);


        }
        private async Task CheckChanges()
        {
            FoundChanges = false;
            CategoryColor = new CategoryColorModel();
            CategoryColor.CategoryName = Request.Form["catid"];
            auditLogs.LogInformation($"ViewManageCategoryModel Method CheckChanges category name: {CategoryColor.CategoryName}");
            if (string.IsNullOrWhiteSpace(Request.Form["newColor"]))
            {
                CategoryColor.CategoryColorHexValue = Request.Form["OldColor"];
                auditLogs.LogInformation($"ViewManageCategoryModel Method CheckChanges category old color: {CategoryColor.CategoryColorHexValue}");
            }
            else
            {
                FoundChanges = true;
                CategoryColor.CategoryColorHexValue = Request.Form["newColor"];
                auditLogs.LogInformation($"ViewManageCategoryModel Method CheckChanges category new color: {CategoryColor.CategoryColorHexValue}");
            }
          
            if (FoundChanges)
            {
                auditLogs.LogInformation($"ViewManageCategoryModel Method CheckChanges category name: {CategoryColor.CategoryName} category new color {CategoryColor.CategoryColorHexValue} weburl:{WebApiUrl} controller: {SqlConstants.ApiUpDateCategoryColors}");
                await PostApis.PostApisIntance.ApiUpdateCategoryColors(WebApiUrl, $"{SqlConstants.ApiUpDateCategoryColors}", CategoryColor, auditLogs);
            }
            else
            {
                auditLogs.LogError($"ViewManageCategoryModel Method category color: {CategoryColor.CategoryColorHexValue} has not changed");
            }
        }

        private async Task CheckDurChanges(CategoryCheckPointEmailModel CategoryCheckPointEmailModel)
        {
            FoundChanges = false;
            auditLogs.LogInformation($"ViewManageCategoryModel Method CheckDurChanges");
            if (Request.Form["currentDuration"] != CategoryCheckPointEmailModel.Duration)
            {
                auditLogs.LogInformation($"ViewManageCategoryModel Method CheckDurChanges category old duration: {Request.Form["currentDuration"]} category new duration:{CategoryCheckPointEmailModel.Duration} category name: {CategoryCheckPointEmailModel.CategoryName}");
                FoundChanges = true;

            }
            else
                CategoryCheckPointEmailModel.Duration = Request.Form["currentDuration"];
            bool fTrueFalse = bool.Parse(Request.Form["currentFlash"]);
            if (fTrueFalse != CategoryCheckPointEmailModel.Flash)
            {
                FoundChanges = true;

            }
            else
                CategoryCheckPointEmailModel.Flash = fTrueFalse;
            fTrueFalse = bool.Parse(Request.Form["currentEmailAlerts"]);
            if (fTrueFalse != CategoryCheckPointEmailModel.EmailAlerts)
            {
                FoundChanges = true;
                if (!(CategoryCheckPointEmailModel.EmailAlerts))
                {
                    CategoryCheckPointEmailModel.Duration = "0.0";
                    CategoryCheckPointEmailModel.EmailTo = string.Empty;
                }


            }
            else
                CategoryCheckPointEmailModel.EmailAlerts = fTrueFalse;
            if (CategoryCheckPointEmailModel.EmailAlerts)
            {
                string emTo = Request.Form["emTo"];
                if (emTo.Length > 3)
                {
                      CategoryCheckPointEmailModel.EmailTo = emTo.Replace(",", ";");
                    
                }
                else
                {

                    CategoryCheckPointEmailModel.EmailTo = Request.Form["currentEmailTo"];
                }
            }

            if (FoundChanges)
            {
                auditLogs.LogInformation($"ViewManageCategoryModel Method CheckDurChanges update category durations: category new duration:{CategoryCheckPointEmailModel.Duration} category name: {CategoryCheckPointEmailModel.CategoryName} weburl:{WebApiUrl} controller:{SqlConstants.ApiUpDateCategoryDurations}");
                await PostApis.PostApisIntance.ApiUpdateCategoryDurations(WebApiUrl, SqlConstants.ApiUpDateCategoryDurations, CategoryCheckPointEmailModel,auditLogs);
            }
            else
            {
                auditLogs.LogInformation($"ViewManageCategoryModel Method CheckDurChanges no changes found for category durations: category new duration:{CategoryCheckPointEmailModel.Duration} category name: {CategoryCheckPointEmailModel.CategoryName}");
            }
        }

    }
}