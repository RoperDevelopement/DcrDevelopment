using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BinMonitorAppService.Models;
using BinMonitorAppService.ApiClasses;
using Microsoft.AspNetCore.Http;
using BinMonitorAppService.Constants;
using BinMonitorAppService.Logging;

namespace BinMonitorAppService.Pages.BinManager
{
    public class ViewManageBatchModel : PageModel
    {
        private readonly IConfiguration configuration;
        private ILog auditLogs;

        private string WebApiUrl
        { get { return configuration.GetValue<string>("BinMonitorApi").ToString(); } }


        public IList<string> LassignedBy
        { get; set; }
        public IList<string> LassignedTo
        { get; set; }


        


        public BinRegProcessModel ManageBatchModel
        { get; set; }
        public IList<string> BinsModel
        { get; set; }
        public string RegProcCloseBatch
        { get; set; }

        public string CurrentBatchNumber
        { get; set; }

        public IList<string> CategoryName
        { get; set; }

        public string SelCategoryName
        { get; set; }
        public SpectrumMonitorMenuRightsModel SpectrumMonitorMenuRightsModel
        { get; set; }

        public string SessionExpires
        { get; set; }
        public ViewManageBatchModel(IConfiguration config, ILog logConfig)
        {
            CurrentBatchNumber = string.Empty;
            configuration = config;
            ManageBatchModel = new BinRegProcessModel();
            auditLogs = logConfig;
        }

        private async Task GetLogInformaiton()
        {

            auditLogs = InitAuditLogs.LogAsync(auditLogs, HttpContext.Session).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public async Task<IActionResult> OnGetAsync(BinRegProcessModel BinRegistorModel)
        {

            string binId = string.Empty;
            bool getBinId = true;
          
            try
            {
                InitAuditLogs.StartStopWatch();
                var qString = Request.QueryString;
                GetLogInformaiton().ConfigureAwait(false).GetAwaiter().GetResult();
                auditLogs.LogInformation("Start ViewManageBatchModel OnGetAsync");

                if (!(User.Identity.IsAuthenticated))
                {
                    if (qString.HasValue)
                    {
                        auditLogs.LogInformation($"Renew session ViewManageBatchModel OnGetAsync  using url: /BinUsers/LoginView?returnUrl=/BinManager/ViewManageBatch{qString.Value} total time: {InitAuditLogs.StopStopWatch()} ms");
                        return Redirect($"/BinUsers/LoginView?returnUrl=/BinManager/ViewManageBatch{qString.Value}");
                    }

                    else
                    {
                        auditLogs.LogInformation($"Renew session ViewManageBatchModel OnGetAsync using url: /BinUsers/LoginView?returnUrl=/BinManager/ViewManageBatch total time: {InitAuditLogs.StopStopWatch()} ms");
                        return Redirect("/BinUsers/LoginView?returnUrl=/BinManager/ViewManageBatch");
                    }
                }

                ViewData["CWID"] = await GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyCwid);
                if (ViewData["CWID"] == null)
                {
                    auditLogs.LogInformation($"Renew session ViewManageBatchModel OnGetAsync return url /BinUsers/LoginView?returnUrl=/BinManager/ViewManageBatch total time: {InitAuditLogs.StopStopWatch()} ms");
                    return Redirect("/BinUsers/LoginView?returnUrl=/BinManager/ViewManageBatch");
                }
                //  ViewData["UserProfile"] = await GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyUserProfile);
               
                int totMinutesLeft = ApiSetUserProfile.UserProfileInstance.GetSessionTime(HttpContext.Session, 1).ConfigureAwait(false).GetAwaiter().GetResult();
                auditLogs.LogInformation($"ViewManageBatchModel OnGetAsync total time left in session: {totMinutesLeft.ToString()}");
                if (totMinutesLeft < 0)
                {
                    
                    if (qString.HasValue)
                    {
                        auditLogs.LogInformation($"Renew session ViewManageBatchModel OnGetAsync  using url: /BinUsers/LoginView?returnUrl=/BinManager/ViewManageBatch{qString.Value} total time: {InitAuditLogs.StopStopWatch()} ms");
                        return Redirect($"/BinUsers/LoginView?returnUrl=/BinManager/ViewManageBatch{qString.Value}");
                    }
                        
                    else
                    {
                        auditLogs.LogInformation($"Renew session ViewManageBatchModel OnGetAsync using url: /BinUsers/LoginView?returnUrl=/BinManager/ViewManageBatch total time: {InitAuditLogs.StopStopWatch()} ms");
                        return Redirect("/BinUsers/LoginView?returnUrl=/BinManager/ViewManageBatch");
                    }
                    
                }
                    
                
                SessionExpires = (totMinutesLeft).ToString();
               
                SpectrumMonitorMenuRightsModel = ApiSetUserProfile.UserProfileInstance.GetBMMenuRights(User.Identity.Name, HttpContext.Session, auditLogs, WebApiUrl).ConfigureAwait(false).GetAwaiter().GetResult();
                GetViewData().ConfigureAwait(true).GetAwaiter().GetResult();

                if (qString.HasValue)
                {
                    auditLogs.LogInformation($"ViewManageBatchModel query string {qString.Value}");
                    if (qString.Value.Contains("BatchId", StringComparison.OrdinalIgnoreCase))
                    {
                        // string qStrValue = qString.Value.Substring(qString.Value.IndexOf("=") + 1);
                        string qStrValue = Request.Query["BatchId"];
                        string url = $"{SqlConstants.ApiGetBinByBatchId}{qStrValue}";
                        auditLogs.LogInformation($"ViewManageBatchModel query string web url {url}");
                        await GetBinsByBatchID(qStrValue, auditLogs);
                        getBinId = false;
                        CurrentBatchNumber = ManageBatchModel.BinID;
                        SelCategoryName = ManageBatchModel.CategoryName;
                    }
                }


                if (getBinId)
                {
                    if (string.IsNullOrEmpty(BinRegistorModel.BinID))
                        await GetBinsTopBin(auditLogs);
                    else
                        await GetBinByBinId(BinRegistorModel.BinID, auditLogs);
                    CurrentBatchNumber = ManageBatchModel.BinID;
                    SelCategoryName = ManageBatchModel.CategoryName;
                    auditLogs.LogInformation($"ViewManageBatchModel BatchId: {CurrentBatchNumber} CategoryName: {SelCategoryName}");
                }



                BinsModel = await GetBins(auditLogs);
                BinsModel = BinsModel.Distinct().ToList();
                await ApiUserInfo(auditLogs).ConfigureAwait(false);
                await GetCatName().ConfigureAwait(false);
             

                await UpDateModel();
                string totalQueryTime = InitAuditLogs.StopStopWatch();
                auditLogs.LogInformation($"End ViewManageBatchModel total time: {totalQueryTime} ms");
                
            }
            catch (Exception ex)
            {
                auditLogs.LogError($"ViewManageBatchModel OnGetAsync total time: {InitAuditLogs.StopStopWatch()} ms {ex.Message}");
                return Redirect($"/BinUsers/DisplayErrorMessagesView?ErrMEss=Model ViewManageBatchModel OnGetAsync {ex.Message}");


            }
            return Page();

        }
        private async Task GetCatName()
        {
            CategoryName = await GetSessionVariables.SessionVarInstance.GetJsonSessionObject<IList<string>>(HttpContext.Session, "CategoryNameViewManageBatchModel").ConfigureAwait(true);
            if (CategoryName == null)
            {
                CategoryName = await GetApis.GetApisInctance.ApiCategories(SqlConstants.ApiCatNameID, WebApiUrl, auditLogs,true);

                await GetSessionVariables.SessionVarInstance.SetSessionOjbjectAsJson(HttpContext.Session, "CategoryNameViewManageBatchModel", CategoryName).ConfigureAwait(true);
            }


        }
        private async Task UpDateModel()
        {

            if (string.IsNullOrWhiteSpace(ManageBatchModel.BinClosedBy))
                ManageBatchModel.BinClosedBy = "Not CLosed";
            if ((string.IsNullOrWhiteSpace(ManageBatchModel.RegDuration)) || (ManageBatchModel.RegDuration.StartsWith("00:")))

            {
                TimeSpan ts = DateTime.Now - ManageBatchModel.RegStartedAt;
                ManageBatchModel.RegDuration = $"{ts.Hours.ToString().PadLeft(2, '0')}:{ts.Minutes.ToString().PadLeft(2, '0')}:{ts.Seconds.ToString().PadLeft(2, '0')}";
            }
            if (ManageBatchModel.ProcessStartAt.Year > 2000)
            {
                TimeSpan ts = DateTime.Now - ManageBatchModel.ProcessStartAt;
                ManageBatchModel.ProcessDuration = $"{ts.Hours.ToString().PadLeft(2, '0')}:{ts.Minutes.ToString().PadLeft(2, '0')}:{ts.Seconds.ToString().PadLeft(2, '0')}";
            }
            TimeSpan tsc = DateTime.Now - ManageBatchModel.ClosedCreatedAt;
            ManageBatchModel.CompleteDuration = $"{tsc.Hours.ToString().PadLeft(2, '0')}:{tsc.Minutes.ToString().PadLeft(2, '0')}:{tsc.Seconds.ToString().PadLeft(2, '0')}";


        }
        private async Task<IList<string>> GetBins(ILog log)
        {
            string sp = $"{SqlConstants.WebApiBinMonitor}{SqlConstants.SpGetActiveBinsModelByBInId}/";
            log.LogInformation($"Method GetBins controller: {sp}");
            IList<string> binModels = await GetApis.GetApisInctance.ApiBins(WebApiUrl, sp, false, log);
            return binModels;

        }
        private async Task GetBinsTopBin(ILog log)
        {

            string sp = $"{SqlConstants.WebApiBinMonitor}{SqlConstants.SpGetActiveBinsModelTop1}/";
            log.LogInformation($"Method GetBinsTopBin controller: {sp}");
            ManageBatchModel = await GetApis.GetApisInctance.ApiActiveBinsModel(WebApiUrl, sp, log);


        }
        private async Task GetBinsByBatchID(string batchID, ILog log)
        {

            string sp = $"{SqlConstants.WebApiBinMonitor}{SqlConstants.SpGetActiveBinsModelByBatchID}/{batchID}";
            log.LogInformation($"Method GetBinsByBatchID batchid:{batchID} controller: {sp}");
            ManageBatchModel = await GetApis.GetApisInctance.ApiActiveBinsModel(WebApiUrl, sp, log);


        }
        private async Task GetBinByBinId(string binID, ILog log)
        {
            string sp = $"{SqlConstants.WebApiBinMonitor}{SqlConstants.SpGetActiveBinsModelByBinId}/{binID}";
            log.LogInformation($"Method GetBinByBinId BinId:{binID} controller: {sp}");
            ManageBatchModel = await GetApis.GetApisInctance.ApiActiveBinsModel(WebApiUrl, sp, log);


        }
        private async Task GetViewData()
        {
            ViewData[SqlConstants.ViewDataChangeCategories] = SpectrumMonitorMenuRightsModel.Categories;
            ViewData[SqlConstants.ViewDataEmailReports] = SpectrumMonitorMenuRightsModel.EmailReports;
            ViewData[SqlConstants.ViewDataRunReports] = SpectrumMonitorMenuRightsModel.RunReports;

        }
        public async Task<IActionResult> OnPostAsync(BinRegProcessModel BinRegistorModel)
        {
            
            ViewData["CWID"] = await GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyCwid);
            if (ViewData["CWID"] == null)
                return Redirect("/BinUsers/LoginView");
            InitAuditLogs.StartStopWatch();
            
            bool valid = true;
            try
            {
                GetLogInformaiton().ConfigureAwait(false).GetAwaiter().GetResult();
                auditLogs.LogInformation("Start ViewManageBatchModel OnPostAsync ");
                SpectrumMonitorMenuRightsModel = ApiSetUserProfile.UserProfileInstance.GetBMMenuRights(User.Identity.Name, HttpContext.Session, auditLogs, WebApiUrl).ConfigureAwait(false).GetAwaiter().GetResult();
                GetViewData().ConfigureAwait(true).GetAwaiter().GetResult();
                CurrentBatchNumber = ManageBatchModel.BinID;
                string bStatus = Request.Form["binStatus"];
                
                CurrentBatchNumber = BinRegistorModel.BinID;

                if (!(string.IsNullOrWhiteSpace(Request.Form["binComments"])))
                {

                    BinRegistorModel.BinComments = Request.Form["binComments"];
                    BinRegistorModel.BinComments = BinRegistorModel.BinComments.Replace(",", "").Trim();
                    BinRegistorModel.BinComments = BinRegistorModel.BinComments.Replace("{UserName}", ViewData["CWID"].ToString());
                }

                
                ManageBatchModel = BinRegistorModel;
                SelCategoryName = ManageBatchModel.CategoryName;
                if (valid)
                {
                    if (string.Compare(bStatus, "changeCategory", true) == 0)
                    {
                        TransFerModel transFer = new TransFerModel();
                        transFer.BinID = CurrentBatchNumber;
                        transFer.OldBinId = string.Empty;
                        transFer.CategoryName = Request.Form["selcat"];
                        transFer.Comments = BinRegistorModel.BinComments;
                        transFer.LabReqNumber = BinRegistorModel.CategoryName;
                        auditLogs.LogInformation($"ViewManageBatchModel OnPostAsync transfer category: BinID: {transFer.BinID} CategoryName: {transFer.CategoryName} LabReqNumber: {transFer.LabReqNumber}");
                        await BinsInformation.BinsApisInctance.TransFer(transFer, WebApiUrl, SqlConstants.ApiTransFer, auditLogs);
                    }
                    else
                    {
                        BinRegistorModel = RegBin(BinRegistorModel);


                        /// await ConvertModels(BinRegistorModel);
                    }

                    if ((string.IsNullOrWhiteSpace(CurrentBatchNumber)))
                    {
                        await GetBinsTopBin(auditLogs);
                        CurrentBatchNumber = ManageBatchModel.BinID;
                        SelCategoryName = ManageBatchModel.CategoryName;
                        auditLogs.LogInformation($"ViewManageBatchModel OnPostAsync current: BinID: {CurrentBatchNumber} CategoryName: {SelCategoryName}");
                    }
                    else
                    {
                        await GetBinsByBatchID(BinRegistorModel.BatchID.ToString(), auditLogs);
                    }
                     
                }
                BinsModel = await GetBins(auditLogs);
                BinsModel = BinsModel.Distinct().ToList();
                await ApiUserInfo(auditLogs);
                await GetCatName().ConfigureAwait(false);
                await UpDateModel();
                string totalQueryTime = InitAuditLogs.StopStopWatch();
                auditLogs.LogInformation($"End ViewManageBatchModel OnPostAsync total time: {totalQueryTime} ms");
                
            }
            catch (Exception ex)
            {
                auditLogs.LogError($"ViewManageBatchModel OnPostAsync  total time: {InitAuditLogs.StopStopWatch()} ms {ex.Message} ");
                return Redirect($"/BinUsers/DisplayErrorMessagesView?ErrMEss=Model ViewManageBatchModel OnGetAsync {ex.Message}");


            }
            if (valid)
            {

            if(!(string.IsNullOrWhiteSpace(BinRegistorModel.ProcessCompletedBy)))
                    return Redirect($"/BinUsers/LoginView?returnUrl=/BinManager/ViewManageBatch?BinID={CurrentBatchNumber}");
            else
                    return Redirect($"/BinUsers/LoginView?returnUrl=/BinManager/ViewManageBatch");
            }
            return Page();
        }
        private async Task ConvertModels(BinRegProcessModel binRegProcessModel)

        {
            Task.Factory.StartNew(async () =>
            {
                BinRegistorModel binRegistorModel = new BinRegistorModel();
                binRegistorModel.BinProcessBinModel = new BinProcessBinModel();
                binRegistorModel.BinsClosedModel = new BinsClosedModel();
                binRegistorModel.BinID = binRegProcessModel.BinID;
                binRegistorModel.BatchID = binRegProcessModel.BatchID;
                binRegistorModel.BinComments = binRegProcessModel.BinComments;
                binRegistorModel.RegAssignedTo = binRegProcessModel.RegAssignedTo;
                binRegistorModel.RegCompletedAt = binRegProcessModel.RegCompletedAt;
                binRegistorModel.RegCompletedBy = binRegProcessModel.RegCompletedBy;
                binRegistorModel.RegAssignedBy = binRegProcessModel.RegAssignedBy;
                binRegistorModel.BinContents = binRegProcessModel.BinContents;
                binRegistorModel.CategoryName = binRegProcessModel.CategoryName;
                binRegistorModel.RegAssignedBy = binRegProcessModel.RegAssignedBy;

                binRegistorModel.RegCreatedBy = binRegProcessModel.RegCreatedBy;
                binRegistorModel.RegStartedAt = binRegProcessModel.RegStartedAt;
                binRegistorModel.RegProcesClose = RegProcCloseBatch;
                binRegistorModel.BinProcessBinModel.ProcessAssignedBy = binRegProcessModel.ProcessAssignedBy;
                binRegistorModel.BinProcessBinModel.ProcessAssignedTo = binRegProcessModel.ProcessAssignedTo;
                binRegistorModel.BinProcessBinModel.ProcessCompletedAt = binRegProcessModel.ProcessCompletedAt;
                binRegistorModel.BinProcessBinModel.ProcessCompletedBy = binRegProcessModel.ProcessCompletedBy;
                binRegistorModel.BinProcessBinModel.ProcessStartAt = binRegProcessModel.ProcessStartAt;

                binRegistorModel.BinsClosedModel.BinClosedBy = binRegProcessModel.BinClosedBy;
                binRegistorModel.BinsClosedModel.BinCompletedAt = binRegProcessModel.BinCompletedAt;
                binRegistorModel.BinsClosedModel.ClosedCreatedAt = binRegProcessModel.ClosedCreatedAt;

                await ApiUpDateBatch(binRegistorModel);
            }).Wait();
        }
        private BinRegProcessModel RegBin(BinRegProcessModel manageBatchMode)
        {
            string bStatus = Request.Form["binStatus"];
            auditLogs.LogInformation($"Method RegBin binstatus: {bStatus}");
            if (!(string.IsNullOrWhiteSpace(bStatus)))
            {
                if (bStatus == "beginreg")
                {
                    RegProcCloseBatch = SqlConstants.RegBatch;
                    manageBatchMode.RegStartedAt = DateTime.Now;
                    if (string.IsNullOrWhiteSpace(manageBatchMode.RegAssignedBy))
                        manageBatchMode.RegAssignedBy = Environment.UserName;
                }


                else if (bStatus == "compreg")
                {
                    if (string.IsNullOrWhiteSpace(manageBatchMode.RegCompletedBy))
                    {
                        auditLogs.LogWarning("ViewManageBatchModel Registration completed by needs a Cwid");
                        ModelState.AddModelError("Error", "Registration completed by needs a Cwid");
                        ModelState.AddModelError("RegCompletedBy", "Registration completed by needs a Cwid");
                    }
                    else
                    {
                        CloseBatches.CloseBatchesInctance.UpDateBeginProcess(manageBatchMode.RegCompletedBy, manageBatchMode.BinID, WebApiUrl, auditLogs).Wait();
                    }

                    //manageBatchMode.RegCompletedBy = Environment.UserName;


                }
                else if (bStatus == "beginproc")
                {
                    if (string.IsNullOrWhiteSpace(manageBatchMode.ProcessAssignedTo))
                    {
                        auditLogs.LogWarning("ViewManageBatchModel Processing assigned to needs a Cwid");
                        ModelState.AddModelError("Error", "Processing assigned to needs a Cwid");
                        ModelState.AddModelError("ProcessAssignedTo", "Processing assigned to needs a Cwid");

                    }
                    else
                    {
                        CloseBatches.CloseBatchesInctance.UpDateBeginProcess(manageBatchMode.ProcessAssignedTo, manageBatchMode.BinID, WebApiUrl, auditLogs).Wait();
                    }

                    //manageBatchMode.RegCompletedBy = Environment.UserName;

                }
                else if (bStatus == "compProc")
                {
                    if (string.IsNullOrWhiteSpace(manageBatchMode.ProcessCompletedBy))
                    {
                        ModelState.AddModelError("Error", "Processing assigned to needs a Cwid");
                        ModelState.AddModelError("ProCompletedBy", "Processing completed by needs a Cwid");

                    }
                    else
                    {
                        CloseBatches.CloseBatchesInctance.CompletBatch(manageBatchMode.ProcessCompletedBy, manageBatchMode.BinID, WebApiUrl, auditLogs).Wait();
                        CurrentBatchNumber = string.Empty;
                    }


                }
                else
                {
                    if (bStatus == "closeBatch")
                    {
                        CloseBatches.CloseBatchesInctance.CompletBatch(ViewData["CWID"].ToString(), manageBatchMode.BinID, WebApiUrl, auditLogs).Wait();
                        CurrentBatchNumber = string.Empty;
                    }
                    else
                        throw new Exception("invalid batch status");

                }
            }

            else
                throw new Exception("invalid batch status");

            return manageBatchMode;
        }
        public async Task ApiUpDateBatch(BinRegistorModel manageBatchMode)
        {
            try
            {


                auditLogs.LogInformation($"Method ApiUpDateBatch weburl:{WebApiUrl} controller: {SqlConstants.ApiCreateBatch} ");
                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri($"{WebApiUrl}");
                    var jsonString = JsonConvert.SerializeObject(manageBatchMode);
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    var responseTask = client.PostAsync($"{client.BaseAddress.AbsoluteUri}{SqlConstants.ApiCreateBatch}", content);
                    responseTask.Wait();
                    var result = await client.PostAsync("Method Address", content);
                    string resultContent = await result.Content.ReadAsStringAsync();
                    var results = responseTask.Result;
                    content.Dispose();
                    auditLogs.LogInformation($"Method ApiUpDateBatch weburl:{WebApiUrl} controller: {SqlConstants.ApiCreateBatch} results code: {results.StatusCode}");
                    if (!(results.IsSuccessStatusCode))
                    {
                        throw new Exception($"Method ApiUpDateBatch weburl:{WebApiUrl} controller: {SqlConstants.ApiCreateBatch} results code: {results.StatusCode}");


                    }
                }
            }
            catch (HttpRequestException httpex)
            {
                auditLogs.LogError($"HttpRequestException Method ApiUpDateBatch weburl:{WebApiUrl} controller: {SqlConstants.ApiCreateBatch} {httpex.Message}");
                throw new Exception($"HttpRequestException Method ApiUpDateBatch weburl:{WebApiUrl} controller: {SqlConstants.ApiCreateBatch} {httpex.Message}");
            }

            catch (ArgumentNullException ag)
            {
                auditLogs.LogError($"ArgumentNullException Method ApiUpDateBatch weburl:{WebApiUrl} controller: {SqlConstants.ApiCreateBatch} {ag.Message}");
                throw new Exception($"ArgumentNullException Method ApiUpDateBatch weburl:{WebApiUrl} controller: {SqlConstants.ApiCreateBatch} {ag.Message}");
            }
            catch (Exception ex)
            {
                auditLogs.LogError($"Exception Method ApiUpDateBatch weburl:{WebApiUrl} controller: {SqlConstants.ApiCreateBatch} {ex.Message}");
                throw new Exception($"Exception Method ApiUpDateBatch weburl:{WebApiUrl} controller: {SqlConstants.ApiCreateBatch} {ex.Message}");
            }
        }

        public async Task ApiActiveBins(string apiUrl)
        {
            try
            {


                auditLogs.LogInformation($"Method ApiUpDateBatch  weburl:{WebApiUrl} controller: {apiUrl}");
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri($"{WebApiUrl}");
                    var responseTask = client.GetAsync($"{apiUrl}");
                    responseTask.Wait();
                    var results = responseTask.Result;
                    auditLogs.LogInformation($"Method ApiUpDateBatch  weburl:{WebApiUrl} controller: {apiUrl} results code: {results.StatusCode}");
                    if (results.IsSuccessStatusCode)
                    {
                        var s = results.Content.ReadAsStringAsync();
                        s.Wait();
                        string j = s.Result.ToString();
                        var readTask = results.Content.ReadAsAsync<BinRegProcessModel[]>();
                        readTask.Wait();


                    }
                    else
                        throw new Exception($"Method ApiUpDateBatch  weburl:{WebApiUrl} controller: {apiUrl} results code: {results.StatusCode}");
                }
            }
            catch (HttpRequestException httpex)
            {
                auditLogs.LogError($"HttpRequestException Method ApiUpDateBatch  weburl:{WebApiUrl} controller: {apiUrl} {httpex.Message}");
                throw new Exception($"HttpRequestException Method ApiUpDateBatch  weburl:{WebApiUrl} controller: {apiUrl} {httpex.Message}");
            }

            catch (ArgumentNullException ag)
            {
                auditLogs.LogError($"ArgumentNullException Method ApiUpDateBatch  weburl:{WebApiUrl} controller: {apiUrl} {ag.Message}");
                throw new Exception($"ArgumentNullException Method ApiUpDateBatch  weburl:{WebApiUrl} controller: {apiUrl} {ag.Message}");
            }
            catch (Exception ex)
            {
                auditLogs.LogError($"Exception Method ApiUpDateBatch  weburl:{WebApiUrl} controller: {apiUrl} {ex.Message}");
                throw new Exception($"Exception Method ApiUpDateBatch  weburl:{WebApiUrl} controller: {apiUrl} {ex.Message}");
            }
        }


        public async Task ApiUserInfo(ILog log)
        {
            try
            {
                log.LogInformation($"Method ApiUserInfo weburl: {WebApiUrl} controller: {SqlConstants.ApiUserInfo}");
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri($"{WebApiUrl}");
                    var responseTask = client.GetAsync($"{SqlConstants.ApiUserInfo}");
                    responseTask.Wait();
                    var results = responseTask.Result;
                    log.LogInformation($"Method ApiUserInfo weburl: {WebApiUrl} controller: {SqlConstants.ApiUserInfo} results status code: {results.StatusCode}");
                    if (results.IsSuccessStatusCode)
                    {
                        var readTask = results.Content.ReadAsAsync<UsersModel[]>();
                        readTask.Wait();
                        //  List<string> tempUserList = readTask.Result.Where(p => !(p.EmailAddress.ToLower().Contains("edocs"))).Select(p => p.FirstName + " " + p.LastName).ToList();
                        List<string> tempUserList = readTask.Result.Where(p => !(p.EmailAddress.ToLower().Contains("edocs"))).Select(p => p.Cwid).ToList();
                        tempUserList.Add("  ");
                        tempUserList.Sort();
                        LassignedBy = tempUserList;
                        LassignedTo = LassignedBy;
                    }
                    else
                        throw new Exception($"Method ApiUserInfo weburl: {WebApiUrl} controller: {SqlConstants.ApiUserInfo} results status code: {results.StatusCode}");
                }
            }
            catch (HttpRequestException httpex)
            {
                auditLogs.LogError($"HttpRequestException Method ApiUserInfo weburl: {WebApiUrl} controller: {SqlConstants.ApiUserInfo} {httpex.Message}");
                throw new Exception($"Metod  HttpRequestException Method ApiUserInfo weburl: {WebApiUrl} controller: {SqlConstants.ApiUserInfo} {httpex.Message}");
            }

            catch (ArgumentNullException ag)
            {
                auditLogs.LogError($"ArgumentNullException Method ApiUserInfo weburl: {WebApiUrl} controller: {SqlConstants.ApiUserInfo} {ag.Message}");
                throw new Exception($"ArgumentNullException Method ApiUserInfo weburl: {WebApiUrl} controller: {SqlConstants.ApiUserInfo} {ag.Message}");
            }
            catch (Exception ex)
            {
                auditLogs.LogError($"Exception Method ApiUserInfo weburl: {WebApiUrl} controller: {SqlConstants.ApiUserInfo} {ex.Message}");
                throw new Exception($"Exception Method ApiUserInfo weburl: {WebApiUrl} controller: {SqlConstants.ApiUserInfo} {ex.Message}");
            }
        }
    }//end of class
}