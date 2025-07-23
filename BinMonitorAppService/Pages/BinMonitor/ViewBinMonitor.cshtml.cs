using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BinMonitorAppService.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using BinMonitorAppService.Constants;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Data;
using System.Data.SqlClient;
using BinMonitorAppService.ApiClasses;
using BinMonitorAppService.Logging;



namespace BinMonitorAppService.Pages.BinMonitor
{
    public class ViewBinMonitorModel : PageModel
    {

        private ILog auditLogs;
        private string WebApiUrl
        { get { return configuration.GetValue<string>("BinMonitorApi").ToString(); } }
        public int LoopCounter
        { get; set; }
        public IDictionary<string, CategoryCheckPointModel> IDicCategory
        { get; set; }
        public IDictionary<string, BinsModel> IDicBins
        { get; set; }
        public DateTime CurrentUtcTime
        { get; set; }
        public IDictionary<string, BinsModel> IDicBinsLabRecs
        { get; set; }
        //  public IList<CategoriesModel> IListCategory
        //  { get; set; }
        [BindProperty]
        public BinsModel ModelBins
        { get; set; }
        private IDictionary<string, CategoryColorModel> CategoryColorCodes
        { get; set; }
        // [BindProperty]
        // public CategoriesModel CategoriesModel
        //  { get; set; }
        private readonly IConfiguration configuration;
        public SpectrumMonitorMenuRightsModel SpectrumMonitorMenuRightsModel
        { get; set; }
        public IList<string> ActiveBinIDS
        { get; set; }
        public string SessionExpires
        { get; set; }
        public ViewBinMonitorModel(IConfiguration config, ILog logsAudit)
        {
            auditLogs = logsAudit;
            configuration = config;
            LoopCounter = 0;
            IDictionary<string, BinsModel> IDicBinsLabRecs = new Dictionary<string, BinsModel>();
        }
        private async Task GetLogInformaiton()
        {

            auditLogs = InitAuditLogs.LogAsync(auditLogs, HttpContext.Session).ConfigureAwait(false).GetAwaiter().GetResult();
        }
#pragma warning disable 1998
        public async Task<IActionResult> OnGetAsync()
        {
            try
            {

                InitAuditLogs.StartStopWatch();
                GetLogInformaiton().ConfigureAwait(false).GetAwaiter().GetResult();
                auditLogs.LogInformation("Start ViewBinMonitorModel");
                if (!(User.Identity.IsAuthenticated))
                {
                    auditLogs.LogInformation($"End ViewBinMonitorModel OnGetAsyn session exired return url: /BinUsers/LoginView?returnUrl=/BinMonitor/ViewBinMonitor total time: {InitAuditLogs.StopStopWatch()} ms");
                    return Redirect("/BinUsers/LoginView?returnUrl=/BinMonitor/ViewBinMonitor");
                }
                ViewData["CWID"] = await GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyCwid);
                if (ViewData["CWID"] == null)
                {
                    auditLogs.LogInformation($"End ViewBinMonitorModel OnGetAsyn session exired return url: /BinUsers/LoginView?returnUrl=/BinMonitor/ViewBinMonitor total time: {InitAuditLogs.StopStopWatch()} ms");
                    return Redirect("/BinUsers/LoginView?returnUrl=/BinMonitor/ViewBinMonitor");
                }


                int totMinutesLeft = ApiSetUserProfile.UserProfileInstance.GetSessionTime(HttpContext.Session, 1).ConfigureAwait(false).GetAwaiter().GetResult();
                auditLogs.LogInformation($"ViewBinMonitorModel OnGetAsyn session expires in: {totMinutesLeft} ms");

                if (totMinutesLeft < 0)
                {
                    auditLogs.LogInformation($"ViewBinMonitorModel renewing session total time: {InitAuditLogs.StopStopWatch()} ms");
                    return Redirect("/BinUsers/LoginView?returnUrl=/BinMonitor/ViewBinMonitor");
                }
                SessionExpires = totMinutesLeft.ToString();
                auditLogs.LogInformation($"ViewBinMonitorModel session expires set to {SessionExpires} seconds");
                SpectrumMonitorMenuRightsModel = ApiSetUserProfile.UserProfileInstance.GetBMMenuRights(User.Identity.Name, HttpContext.Session, auditLogs, WebApiUrl).ConfigureAwait(false).GetAwaiter().GetResult();
                auditLogs.LogInformation($"ViewBinMonitorModel get catgory colors weburl:{WebApiUrl} controller: {SqlConstants.WebApiBinMonitor}{SqlConstants.SpGetCategoryColors}");
                CategoryColorCodes = await GetApis.GetApisInctance.ApiCategoriesColors($"{SqlConstants.WebApiBinMonitor}{SqlConstants.SpGetCategoryColors}", WebApiUrl, true, auditLogs);
                LoopCounter = 0;
                   CurrentUtcTime = DateTime.UtcNow.AddHours(-5);
            //    await GetAddTime();
                await ApiBins();
                await ApiCategories();
                await ApiActiveBins();
                await UpdateCatDic();
                await GetCategoryTotal(SqlConstants.SpGetTotalOpenByCategory);
                await GetCategoryTotal(SqlConstants.SpGetTotalClosedByCategory);
                auditLogs.LogInformation($"End ViewBinMonitorModel total time: {InitAuditLogs.StopStopWatch()} ms");
            }
            catch (Exception ex)
            {
                auditLogs.LogInformation($"End ViewBinMonitorModel total time: {InitAuditLogs.StopStopWatch()} ms {ex.Message}");
                return Redirect($"/BinUsers/DisplayErrorMessagesView?ErrMEss=Model ViewBinMonitorModel OnGetAsync {ex.Message}");
            }
            return Page();
        }
        private async Task  GetAddTime()
        {
            int addTime = await GetApis.GetApisInctance.GetAddTime(WebApiUrl, $"{SqlConstants.WebApiBinMonitor}{SqlConstants.SPGetSpecSettings}", auditLogs, HttpContext.Session);
            CurrentUtcTime = DateTime.UtcNow.AddHours(addTime);
        }
#pragma warning disable 1998
        private async Task UpdateCatDic()
        {
            Task.Factory.StartNew(async () =>
            {
                foreach (KeyValuePair<string, CategoryCheckPointModel> keyValuePair in IDicCategory)
                {

                    keyValuePair.Value.CategoryColor = keyValuePair.Value.CategoryColorCheckPointOne;

                }
            }).Wait();
        }
#pragma warning disable 1998
        private async Task GetCategoryTotal(string spName)
        {
            auditLogs.LogInformation($"Method GetCategoryTotal stored procedure name {spName}");
            Task.Factory.StartNew(async () =>
            {
                IDictionary<string, CategoriesModel> catTotal = await GetApis.GetApisInctance.ApiGetCategoriesTotal($"{SqlConstants.WebApiBinMonitor}{spName}", WebApiUrl, auditLogs);
                foreach (KeyValuePair<string, CategoriesModel> keyValuePair in catTotal)
                {
                    if (IDicCategory.TryGetValue(keyValuePair.Value.CategoryName, out CategoryCheckPointModel value))
                    {
                        if (string.Compare(SqlConstants.SpGetTotalOpenByCategory, spName, true) == 0)

                            value.TotalOpened = keyValuePair.Value.TotalOpened;
                        else
                            value.TotalCompleted = keyValuePair.Value.TotalCompleted;
                    }
                }
            }).Wait();
        }
#pragma warning disable 1998
        public async Task ApiCategories()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri($"{WebApiUrl}");
                    string catModelUrl = string.Format(SqlConstants.ApiCategoryDurations, "na", SqlConstants.SpCategoryCheckPointModel);
                    auditLogs.LogInformation($"Method ApiCategories weburl: {WebApiUrl} controller: {catModelUrl}");
                    //var responseTask = client.GetAsync($"{SqlConstants.ApiCategories}");
                    var responseTask = client.GetAsync(catModelUrl);
                    responseTask.Wait();
                    var results = responseTask.Result;
                    auditLogs.LogInformation($"Method ApiCategories weburl: {WebApiUrl} controller: {catModelUrl} results code: {results.StatusCode}");
                    if (results.IsSuccessStatusCode)
                    {
                        var readTask = results.Content.ReadAsAsync<CategoryCheckPointModel[]>();
                        readTask.Wait();

                        IDicCategory = readTask.Result.ToDictionary(p => p.CategoryName);

                    }
                    else
                        throw new Exception($"Method ApiCategories weburl: {WebApiUrl} controller: {catModelUrl} results code: {results.StatusCode}");
                }
            }
            catch (HttpRequestException httpex)
            {
                auditLogs.LogError($"HttpRequestException Method ApiCategories {httpex.Message}");
                throw new Exception($"HttpRequestException Method ApiCategories {httpex.Message}");
            }

            catch (ArgumentNullException ag)
            {
                auditLogs.LogError($"ArgumentNullException Method ApiCategories {ag.Message}");
                throw new Exception($"ArgumentNullException Method ApiCategories  {ag.Message}");
            }
            catch (Exception ex)
            {
                auditLogs.LogError($"Exception Method ApiCategories {ex.Message}");
                throw new Exception($"Exception Method ApiCategories {ex.Message}");
            }

        }
#pragma warning disable 1998
        public async Task ApiActiveBins()
        {
            try
            {
                auditLogs.LogInformation($"Method ApiActiveBins weburl: {WebApiUrl} controller: {SqlConstants.ApiUrlActiveBins}");

                ActiveBinIDS = new List<string>();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri($"{WebApiUrl}");
                    var responseTask = client.GetAsync($"{SqlConstants.ApiUrlActiveBins}");
                    responseTask.Wait();
                    var results = responseTask.Result;
                    auditLogs.LogInformation($"Method ApiCategories weburl: {WebApiUrl} controller: {SqlConstants.ApiUrlActiveBins} results code: {results.StatusCode}");
                    if (results.IsSuccessStatusCode)
                    {
                        var readTask = results.Content.ReadAsAsync<BinsModel[]>();
                        readTask.Wait();
                        IDictionary<string, BinsModel> dicABM = readTask.Result.ToDictionary(p => p.LabRecNumber);
                        string binIds = string.Empty;
                        foreach (var activeBins in dicABM)
                        {
                            string acBin = $"{activeBins.Key}{activeBins.Value.Bin}";
                            if (IDicBins.TryGetValue(activeBins.Value.Bin, out BinsModel value))

                            {
                                if (!(ActiveBinIDS.Contains(activeBins.Value.Bin)))
                                    ActiveBinIDS.Add(activeBins.Value.Bin);

                                value.AcvitveBin = true;
                                value.LabRecNumber = activeBins.Value.LabRecNumber;
                                value.BinAssignedTo = activeBins.Value.BinAssignedTo;
                                value.Category = activeBins.Value.Category;
                                value.StartTime = activeBins.Value.StartTime;
                                value.CategoryColor = await
                                    BinsInformation.BinsApisInctance.CategoyColor(value.Category, value.StartTime, IDicCategory);

                                if (value.Category.Length > 3)
                                {
                                    if (value.Category.Length > 6)
                                        value.Category = activeBins.Value.Category.Substring(0, 6);
                                }

                            }
                        }


                    }
                    else
                        throw new Exception($"Method ApiCategories weburl: {WebApiUrl} controller: {SqlConstants.ApiUrlActiveBins} results code: {results.StatusCode}");
                }
            }
            catch (HttpRequestException httpex)
            {
                auditLogs.LogError($"HttpRequestException Method ApiCategories weburl: {WebApiUrl} controller: {SqlConstants.ApiUrlActiveBins} {httpex.Message}");
                throw new Exception($"HttpRequestException Method ApiCategories weburl: {WebApiUrl} controller: {SqlConstants.ApiUrlActiveBins} {httpex.Message}");
            }

            catch (ArgumentNullException ag)
            {
                auditLogs.LogError($"ArgumentNullException Method ApiCategories weburl: {WebApiUrl} controller: {SqlConstants.ApiUrlActiveBins} {ag.Message}");
                throw new Exception($"ArgumentNullException Method ApiCategories weburl: {WebApiUrl} controller: {SqlConstants.ApiUrlActiveBins} {ag.Message}");
            }
            catch (Exception ex)
            {
                auditLogs.LogError($"Exception Method ApiCategories weburl: {WebApiUrl} controller: {SqlConstants.ApiUrlActiveBins} {ex.Message}");
                throw new Exception($"Exception Method ApiCategories weburl: {WebApiUrl} controller: {SqlConstants.ApiUrlActiveBins} {ex.Message}");
            }

        }
#pragma warning disable 1998
        public async Task ApiBins()
        {
            try
            {
                auditLogs.LogInformation($"Method ApiBins weburl: {WebApiUrl} controller: {SqlConstants.ApiUrlBins}");

                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri($"{WebApiUrl}");
                    var responseTask = client.GetAsync($"{SqlConstants.ApiUrlBins}");
                    responseTask.Wait();
                    var results = responseTask.Result;

                    auditLogs.LogInformation($"Method ApiBins weburl: {WebApiUrl} controller: {SqlConstants.ApiUrlBins} results code: {results.StatusCode}");
                    if (results.IsSuccessStatusCode)
                    {
                        var readTask = results.Content.ReadAsAsync<BinsModel[]>();
                        readTask.Wait();

                        IDicBins = readTask.Result.ToDictionary(p => p.Bin);
                        if (!(IDicBins.Any()))
                            auditLogs.LogInformation($"Method ApiBins weburl: {WebApiUrl} controller: {SqlConstants.ApiUrlBins} results code: {results.StatusCode} no result for BinsModel");

                    }
                    else
                        throw new Exception($"Method ApiBins weburl: {WebApiUrl} controller: {SqlConstants.ApiUrlBins} results code: {results.StatusCode}");


                }
            }
            catch (HttpRequestException httpex)
            {
                auditLogs.LogError($"HttpRequestException Method ApiBins weburl: {WebApiUrl} controller: {SqlConstants.ApiUrlBins} {httpex.Message}");
                throw new Exception($"HttpRequestException Method ApiBins weburl: {WebApiUrl} controller: {SqlConstants.ApiUrlBins} {httpex.Message}");
            }

            catch (ArgumentNullException ag)
            {
                auditLogs.LogError($"ArgumentNullException Method ApiBins weburl: {WebApiUrl} controller: {SqlConstants.ApiUrlBins} {ag.Message}");
                throw new Exception($"ArgumentNullException  Method ApiBins weburl: {WebApiUrl} controller: {SqlConstants.ApiUrlBins} {ag.Message}");
            }
            catch (Exception ex)
            {
                auditLogs.LogError($"Exception  Method ApiBins weburl: {WebApiUrl} controller: {SqlConstants.ApiUrlBins} {ex.Message}");
                throw new Exception($"Exception Method ApiBins weburl: {WebApiUrl} controller: {SqlConstants.ApiUrlBins} {ex.Message}");
            }
        }

        //public async Task ActiveBins()
        //{
        //    using (SqlConnection sqlConnection = new SqlConnection(configuration.GetConnectionString("BinMonitorCloudConnectionString")))
        //    {

        //        using (SqlCommand sqlCmd = new SqlCommand(SqlConstants.SpActiveBins, sqlConnection))
        //        {
        //            sqlCmd.CommandType = CommandType.StoredProcedure;
        //            await sqlConnection.OpenAsync();
        //            SqlDataReader dr = await sqlCmd.ExecuteReaderAsync();
        //            while (await dr.ReadAsync())
        //            {
        //                string binID = dr[SqlConstants.SqlAliasBinId].ToString();
        //               if (IDicBins.TryGetValue(binID, out BinsModel value))
        //                {
        //                    value.AcvitveBin = true;
        //                    value.BinAssignedTo =
        //                    value.Category = 
        //                  //  value.StartTime = Convert.ToDateTime(dr[SqlConstants.SqlAliasStarted].ToString());
        //                }

        //            }
        //        }
        //    }
        //}
        //public async Task Bins()
        //{
        //    using (SqlConnection sqlConnection = new SqlConnection(configuration.GetConnectionString("BinMonitorCloudConnectionString")))
        //    {

        //        using (SqlCommand sqlCmd = new SqlCommand(SqlConstants.SpBins, sqlConnection))
        //        {
        //            sqlCmd.CommandType = CommandType.StoredProcedure;
        //            await sqlConnection.OpenAsync();
        //            SqlDataReader dr = await sqlCmd.ExecuteReaderAsync();
        //            while (await dr.ReadAsync())
        //            {
        //                BinsModel bins = new BinsModel
        //                {
        //                    Bin = dr[SqlConstants.SqlAliasBinId].ToString(),
        //                    // StartTime = DateTime.Now,
        //                    Category = "Opened",
        //                    BinAssignedTo = string.Empty,
        //                    // AcvitveBins = false,
        //                    // BinRegStarted = false,
        //                    //  BinRegCompleted = false,
        //                    //  BinProcessCompleted = false,
        //                    //   BinProcessStarted = false
        //                };
        //                IDicBins.Add(bins.Bin, bins);
        //            }
        //        }
        //    }
        //}
#pragma warning disable 1998
        //public async Task BinCategories()
        //{

        //    using (SqlConnection sqlConnection = new SqlConnection(configuration.GetConnectionString("BinMonitorCloudConnectionString")))
        //    {

        //        using (SqlCommand sqlCmd = new SqlCommand(SqlConstants.SpGetCatgories, sqlConnection))
        //        {
        //            sqlCmd.CommandType = CommandType.StoredProcedure;
        //            await sqlConnection.OpenAsync();
        //            SqlDataReader dr = await sqlCmd.ExecuteReaderAsync();
        //            while (await dr.ReadAsync())
        //            {
        //                CategoriesModel categoriesModel = new CategoriesModel
        //                {
        //                    CategoryId = int.Parse(dr[SqlConstants.SqlAliasCatId].ToString()),
        //                    CategoryName = dr[SqlConstants.SqlAliasCatName].ToString(),
        //                    TotalCompleted = 0,
        //                    TotalOpened = 0
        //                };
        //                IDicCategory.Add(categoriesModel.CategoryId, categoriesModel);
        //            }
        //            CategoriesModel categoriesModel1 = new CategoriesModel
        //            {
        //                CategoryId = 1000,
        //                CategoryName = "Summary",
        //                TotalOpened = 0,
        //                TotalCompleted = 0
        //            };
        //            IDicCategory.Add(categoriesModel1.CategoryId, categoriesModel1);
        //        }
        //    }
        //}
    }
}
