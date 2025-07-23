using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BinMonitorAppService.Models;
using BinMonitorAppService.Constants;
using Microsoft.AspNetCore.Hosting;

using Microsoft.AspNetCore.Identity;
using System.Net.Http;
using System.Security.Claims;
using Microsoft.Net.Http;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Configuration;
using BinMonitorAppService.ApiClasses;
using Microsoft.AspNetCore.Http;
using BinMonitorAppService.Logging;
using Microsoft.AspNetCore.Authorization;

namespace BinMonitorAppService.Pages.Partial_Pages
{
    public class TransferPartialViewModel : PageModel
    {
        private readonly IConfiguration configuration;
        private ILog auditLogs;
        private string WebApiUrl
        { get { return configuration.GetValue<string>("BinMonitorApi").ToString(); } }
        public IDictionary<string, BinRegProcessModel> LabReqNumbers
        { get; set; }
        public IList<string> TransFerCategoryName
        { get; set; }
        // IDictionary<string, BinRegProcessModel>  ActiveBinsCategoroy
        //{ get; set; }
        public IList<string> CatBins
        { get; set; }
        public SpectrumMonitorMenuRightsModel SpectrumMonitorMenuRightsModel
        { get; set; }
        public IList<string> ActiveBins
        { get; set; }

        public string TransFer
        { get; set; }
        public TransferPartialViewModel(IConfiguration config, ILog logConfig)
        {
            configuration = config;

            auditLogs = logConfig;

        }
        private async Task GetLogInformaiton()
        {

            auditLogs = InitAuditLogs.LogAsync(auditLogs, HttpContext.Session).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        public async Task<IActionResult> OnGetAsync()
        {
             

            ViewData["CWID"] = await GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyCwid);
            if (ViewData["CWID"] == null)
                return Redirect("/BinUsers/LoginView");
            try
            {

            
            InitAuditLogs.StartStopWatch();
            GetLogInformaiton().ConfigureAwait(false).GetAwaiter().GetResult();
            auditLogs.LogInformation("Start  TransferPartialViewModel OnGetAsync");
                SpectrumMonitorMenuRightsModel = ApiSetUserProfile.UserProfileInstance.GetBMMenuRights(User.Identity.Name, HttpContext.Session, auditLogs, WebApiUrl).ConfigureAwait(false).GetAwaiter().GetResult();
                GetViewData().ConfigureAwait(true).GetAwaiter().GetResult();

                var qString = Request.QueryString;
            TransFer = string.Empty;
            if (qString.HasValue)
            {
                // if(string.Compare(Request.Query["modelName"].ToString(), "transFrom",true) == 0)
                if (qString.Value.EndsWith("transferCategores"))
                {
                    TransFer = "transferCategores";
                    auditLogs.LogInformation($"Getting information to transfer categories for query string {qString.Value}");
                    GetActiveBinsCategories().ConfigureAwait(false).GetAwaiter().GetResult();
                    if ((CatBins != null) && (CatBins.Count > 0))
                        GetCatName().ConfigureAwait(false).GetAwaiter().GetResult();

                }
                else if (qString.Value.EndsWith("transferLabReqs"))
                {
                    TransFer = "transferLabReqs";
                    auditLogs.LogInformation($"Getting information to transfer transferLabReqs for query string {qString.Value}");
                    LabReqNumbers = GetActiveBins().ConfigureAwait(false).GetAwaiter().GetResult();
                    if ((LabReqNumbers != null) && (LabReqNumbers.Count > 0))
                        GetBIns().ConfigureAwait(false).GetAwaiter().GetResult();

                }

                else
                    throw new Exception($"Invalid Query string: {qString.Value}");

            }

            auditLogs.LogInformation($"End TransferPartialViewModel OnGetAsync total time: {InitAuditLogs.StopStopWatch()} ms");
            }
            catch(Exception ex)
            {
                auditLogs.LogError($"End TransferPartialViewModel OnGetAsync total time: {InitAuditLogs.StopStopWatch()} ms {ex.Message}");
                return Redirect($"/BinUsers/DisplayErrorMessagesView?ErrMEss=Model TransferPartialViewModel OnGetAsync {ex.Message}");
                
            }
            return Page();
        }

        private async Task GetBIns()
        {
            auditLogs.LogInformation(" Method TransferPartialViewModel GetBIns");
            auditLogs.LogInformation($"Calling webapi {WebApiUrl} with params: {SqlConstants.ApiOpenBins}");
            ActiveBins = GetApis.GetApisInctance.ApiBins(WebApiUrl, SqlConstants.ApiOpenBins, false, auditLogs).ConfigureAwait(false).GetAwaiter().GetResult();
            if ((ActiveBins != null) && (ActiveBins.Count > 0))
            {
                auditLogs.LogInformation($"Calling webapi {WebApiUrl} with params: {SqlConstants.ApiActiveBinsModel}");
                IList<string> BinModels = await GetApis.GetApisInctance.ApiBins(WebApiUrl, SqlConstants.ApiActiveBinsModel, false, auditLogs);

                var tempList = ActiveBins.Concat(BinModels).Distinct().ToList();
                tempList.Sort();
                ActiveBins.Clear();
                ActiveBins = tempList;
            }
            else
            {
                auditLogs.LogWarning($"No bins returned for webapi {WebApiUrl} with params: {SqlConstants.ApiActiveBinsModel}");
            }
                
        }
        private async Task GetCatName()
        {
            auditLogs.LogInformation(" Method TransferPartialViewModel GetCatName");
            TransFerCategoryName = await GetSessionVariables.SessionVarInstance.GetJsonSessionObject<IList<string>>(HttpContext.Session, "CategoryNameViewManageBatchModel").ConfigureAwait(true);
            if (TransFerCategoryName == null)
            {
                auditLogs.LogInformation($"Method TransferPartialViewModel GetCatName for webapi: {WebApiUrl} with params: {SqlConstants.ApiCatNameID}");
                TransFerCategoryName = await GetApis.GetApisInctance.ApiCategories(SqlConstants.ApiCatNameID, WebApiUrl, auditLogs,true);

                await GetSessionVariables.SessionVarInstance.SetSessionOjbjectAsJson(HttpContext.Session, "CategoryNameViewManageBatchModel", TransFerCategoryName).ConfigureAwait(true);
            }


        }
        private async Task<IDictionary<string, BinRegProcessModel>> GetActiveBins()
            {
            auditLogs.LogInformation("Method GetActiveBins TransferPartialViewModel ");
            auditLogs.LogInformation($"Method GetActiveBins for webapi:{WebApiUrl}{SqlConstants.WebApiBinMonitor} params:{SqlConstants.SpGetActiveBinsModel} {SqlConstants.BinRegProcessMondelIndexBatchId}");
            IDictionary<string, BinRegProcessModel> bpm = GetApis.GetApisInctance.ApiGetActiveBinsModel($"{WebApiUrl}{SqlConstants.WebApiBinMonitor}", SqlConstants.SpGetActiveBinsModel, SqlConstants.BinRegProcessMondelIndexBatchId, auditLogs).ConfigureAwait(false).GetAwaiter().GetResult();
            GetSessionVariables.SessionVarInstance.SetSessionOjbjectAsJson(HttpContext.Session, SqlConstants.CachBinRegProcessModel, bpm).ConfigureAwait(false).GetAwaiter().GetResult();
            return bpm;
        }
        private async Task GetActiveBinsCategories()
        {
            auditLogs.LogInformation("Method GetActiveBinsCategories TransferPartialViewModel ");
            IDictionary<string, BinRegProcessModel> ActiveBinsCategoroy = GetActiveBins().ConfigureAwait(false).GetAwaiter().GetResult();
            CatBins = new List<string>();
            foreach (KeyValuePair<string, BinRegProcessModel> key in ActiveBinsCategoroy)
            {
                if(!(CatBins.Contains($"{key.Value.BinID}-{key.Value.CategoryName}")))
                {
                    CatBins.Add($"{key.Value.BinID}-{key.Value.CategoryName}");
                }
            }
             
        }

        private async Task GetActiveBinsLabRecs()
        {
            auditLogs.LogInformation("Method GetActiveBinsLabRecs TransferPartialViewModel ");
            IDictionary<string, BinRegProcessModel> ActiveBinsCategoroy = GetApis.GetApisInctance.ApiGetActiveBinsModel($"{WebApiUrl}{SqlConstants.WebApiBinMonitor}", SqlConstants.SpGetActiveBinsModel, SqlConstants.BinRegProcessMondelIndexBatchId, auditLogs).ConfigureAwait(false).GetAwaiter().GetResult();
            CatBins = new List<string>();
            foreach (KeyValuePair<string, BinRegProcessModel> key in ActiveBinsCategoroy)
            {
                if (!(CatBins.Contains($"{key.Value.BinID}-{key.Value.LabRecNumber}")))
                {
                    CatBins.Add($"{key.Value.BinID}-{key.Value.CategoryName}");
                }
            }

        }

        private async Task GetViewData()
        {
            ViewData[SqlConstants.ViewDataChangeCategories] = SpectrumMonitorMenuRightsModel.Categories;
            ViewData[SqlConstants.ViewDataEmailReports] = SpectrumMonitorMenuRightsModel.EmailReports;
            ViewData[SqlConstants.ViewDataRunReports] = SpectrumMonitorMenuRightsModel.RunReports;

        }
    }
}