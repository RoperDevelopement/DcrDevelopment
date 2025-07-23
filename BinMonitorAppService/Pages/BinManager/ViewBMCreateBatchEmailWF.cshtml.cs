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
using System.Net.Http;

using Microsoft.Net.Http;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Configuration;
using BinMonitorAppService.ApiClasses;
using BinMonitorAppService.Logging;
namespace BinMonitorAppService.Pages.BinManager
{
    public class ViewBMCreateBatchEmailWF : PageModel
    {
        private ILog auditLogs;
        private readonly IConfiguration configuration;
        private string WebApiUrl
        { get { return configuration.GetValue<string>("BinMonitorApi").ToString(); } }

        public SpecMonitorFormUserPre MonitorFormUserPre
        { get; set; }
        [MinLength(4)]
        public IList<string> BinID
        { get; set; }


        [MaxLength(10)]
        public IList<string> CategoryName
        { get; set; }

        public IList<string> LassignedBy
        { get; set; }
        public IList<string> LassignedTo
        { get; set; }
        [BindProperty]
        public BinRegProcessModel BinRegistorModel
        { get; set; }
        public bool IsRegistered
        { get; set; }

        public WorkFlowEmailModel WorkFlowEmailModelCreate
        { get; set; }
        public WorkFlowEmailModel WorkFlowEmailModelRegister
        { get; set; }
        public WorkFlowEmailModel WorkFlowEmailModelProcess
        { get; set; }

        public ViewBMCreateBatchEmailWF(IConfiguration config, ILog logConfig)
        {
            configuration = config;
            IsRegistered = false;

            BinRegistorModel = new BinRegProcessModel();
            auditLogs = logConfig;

        }
        private async Task GetLogInformaiton()
        {

            auditLogs = InitAuditLogs.LogAsync(auditLogs, HttpContext.Session).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        public async Task<IActionResult> OnGetAsync()
        {
            
            InitAuditLogs.StartStopWatch();
            try
            {
                if (!(User.Identity.IsAuthenticated))
                {
                    return Redirect("/BinUsers/LoginView");
                }
                ViewData["SessionExpire"] = "120000";
                ViewData["ErrMess"] = string.Empty;

                GetLogInformaiton().ConfigureAwait(false).GetAwaiter().GetResult();
                auditLogs.LogInformation("Start ViewBMCreateBatchEmailWF onGetAsync");
                await Init();
                ViewData["CWID"] = await GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyCwid);
              //  ViewData["UserProfile"] = await GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyUserProfile);
                MonitorFormUserPre = await GetSessionVariables.SessionVarInstance.GetJsonSessionObject<SpecMonitorFormUserPre>(HttpContext.Session, "ProfileUser");
                string totalQueryTime = InitAuditLogs.StopStopWatch();
                auditLogs.LogInformation($"End ViewBMCreateBatchEmailWF onGetAsync total time: {totalQueryTime} ms");


                
            }
            catch (Exception ex)
            {
                auditLogs.LogError($"Start ViewBMCreateBatchEmailWF onGetAsync total time: {InitAuditLogs.StopStopWatch()} ms  { ex.Message}");
                // RedirectToPage($"/Error?ErrMEss=Model ViewBMCreateBatchEmailWF onGetAsync on get {ex.Message}");
                ViewData["ErrMess"] = $"/Error?ErrMEss=Model ViewBMCreateBatchEmailWF onGetAsync on get {ex.Message}";
                //throw new Exception(ex.Message);
            }
            return Page();

        }
        private async Task Init()
        {
            await ApiOpenBins();
            await ApiUserInfo();
            await ApiCategories();
        }
        public async Task<IActionResult> OnPostAsync(bool IsRegistered, BinRegProcessModel BinRegistorModel, WorkFlowEmailModel WorkFlowEmailModelCreate,
            WorkFlowEmailModel WorkFlowEmailModelRegister, WorkFlowEmailModel WorkFlowEmailModelProcess)

        {
            string totalQueryTime = string.Empty;
            ViewData["ErrMess"] = string.Empty;
            InitAuditLogs.StartStopWatch();
            try
            {
                if (!(User.Identity.IsAuthenticated))
                {
                    return Redirect("/BinUsers/LoginView");
                }
                GetLogInformaiton().ConfigureAwait(false).GetAwaiter().GetResult();
                auditLogs.LogInformation("Start ViewBMCreateBatchEmailWF onpostasync");

                MonitorFormUserPre = await GetSessionVariables.SessionVarInstance.GetJsonSessionObject<SpecMonitorFormUserPre>(HttpContext.Session, "ProfileUser");

                bool valid = true;
                string transFerFrom = Request.Form["oldBinID"];
                if (!(string.IsNullOrWhiteSpace(transFerFrom)))
                {
                    string newBinId = Request.Form["newBinID"];
                    auditLogs.LogInformation($"ViewBMCreateBatchEmailWF onpostasync transfer old binId:{transFerFrom} to new bindID: {newBinId}");
                    await TransFerBin(transFerFrom, newBinId, BinRegistorModel.BinComments, auditLogs);
                    totalQueryTime = InitAuditLogs.StopStopWatch();
                    auditLogs.LogInformation($"End ViewBMCreateBatchEmailWF onpostasync total time: {totalQueryTime} ms redirect to ViewBMCreateBatchEmailWF");
                    return RedirectToPage("ViewBMCreateBatchEmailWF");
                }
                else
                {

                    if (IsRegistered)
                    {
                        IsRegistered = false;
                        BinRegistorModel.RegAssignedTo = Request.Form["LassignedBy"].ToString();
                        BinRegistorModel.ProcessAssignedTo = Request.Form["LassignedTo"].ToString();
                        if (!(string.IsNullOrWhiteSpace(BinRegistorModel.RegAssignedTo)))
                            BinRegistorModel.RegAssignedBy = Environment.UserName;
                        if (!(string.IsNullOrWhiteSpace(BinRegistorModel.ProcessAssignedTo)))
                        {

                            BinRegistorModel.ProcessAssignedBy = Environment.UserName;
                        }
                    }
                    else
                    {
                        BinRegistorModel.RegAssignedTo = Environment.UserName;
                        BinRegistorModel.RegCompletedBy = Environment.UserName;
                        BinRegistorModel.RegStartedAt = DateTime.Now;
                        BinRegistorModel.RegCompletedAt = DateTime.Now;
                        BinRegistorModel.RegAssignedBy = Environment.UserName;

                    }



                    if (!(string.IsNullOrWhiteSpace(BinRegistorModel.BinContents)))
                    {
                        BinRegistorModel.BinContents = BinRegistorModel.BinContents.Replace("{UserName}", Environment.UserName);
                    }
                    if (!(string.IsNullOrWhiteSpace(BinRegistorModel.BinComments)))
                    {
                        BinRegistorModel.BinComments = BinRegistorModel.BinContents.Replace("{UserName}", Environment.UserName);
                    }

                    string workFlowCreate = string.Empty;
                    string workFlowReg = string.Empty;
                    string workFlowProc = string.Empty;
                    valid = CheckVaild(BinRegistorModel);
                    if (valid)
                    {
                        valid = true;
                        workFlowCreate = CheckWorkFlow(WorkFlowEmailModelCreate, "emailRecCreate");
                        if (string.Compare(workFlowCreate, "Invalid", false) == 0)
                            valid = false;
                        else
                        {
                            workFlowReg = CheckWorkFlow(WorkFlowEmailModelRegister, "emailRecRegister");
                            if (string.Compare(workFlowReg, "Invalid", false) == 0)
                                valid = false;
                            else
                            {
                                workFlowProc = CheckWorkFlow(WorkFlowEmailModelProcess, "emailRecProcess");
                                if (string.Compare(workFlowProc, "Invalid", false) == 0)
                                    valid = false;
                            }
                        }


                    }


                    // BinRegistorModel.BinComments =
                    //if (!(string.IsNullOrWhiteSpace(BinRegistorModel.BinID)))
                    if (valid)
                    {
                        BinRegistorModel.BatchID = Guid.NewGuid();
                        auditLogs.LogInformation($"ViewBMCreateBatchEmailWF create new batch BatchID: {BinRegistorModel.BatchID.ToString()} BinID: {BinRegistorModel.BinID} LabRecNumber: {BinRegistorModel.LabRecNumber}");
                        await ApiCreateBatch(BinRegistorModel, auditLogs);
                        if (string.Compare(workFlowCreate, "NoWorkFlow", true) != 0)
                        {
                            WorkFlowEmailModelCreate.BatchID = BinRegistorModel.BatchID;
                            WorkFlowEmailModelCreate.EmailTo = workFlowCreate.Replace(",", ";");
                            auditLogs.LogInformation($"ViewBMCreateBatchEmailWF create work flow email reports: BatchID:{WorkFlowEmailModelCreate.BatchID.ToString()} BinID: {BinRegistorModel.BinID} LabRecNumber: {BinRegistorModel.LabRecNumber} EmailTo {WorkFlowEmailModelCreate.EmailTo}");
                            await PostApis.PostApisIntance.ApiEmailWorkFlow(WebApiUrl, $"{SqlConstants.ApiWorkFlowEmailController}{SqlConstants.EmailSpecimsCreate}", WorkFlowEmailModelCreate, auditLogs);

                        }
                        if (string.Compare(workFlowReg, "NoWorkFlow", true) != 0)
                        {
                            WorkFlowEmailModelRegister.BatchID = BinRegistorModel.BatchID;
                            WorkFlowEmailModelRegister.EmailTo = workFlowReg.Replace(",", ";");
                            auditLogs.LogInformation($"ViewBMCreateBatchEmailWF register work flow email reports: BatchID:{WorkFlowEmailModelRegister.BatchID.ToString()} BinID: {BinRegistorModel.BinID} LabRecNumber: {BinRegistorModel.LabRecNumber} EmailTo {WorkFlowEmailModelRegister.EmailTo}");
                            await PostApis.PostApisIntance.ApiEmailWorkFlow(WebApiUrl, $"{SqlConstants.ApiWorkFlowEmailController}{SqlConstants.EdocsUserFN}", WorkFlowEmailModelRegister, auditLogs);

                        }
                        if (string.Compare(workFlowProc, "NoWorkFlow", true) != 0)
                        {
                            WorkFlowEmailModelProcess.BatchID = BinRegistorModel.BatchID;
                            WorkFlowEmailModelProcess.EmailTo = workFlowProc.Replace(",", ";");
                            auditLogs.LogInformation($"ViewBMCreateBatchEmailWF process work flow email reports: BatchID:{WorkFlowEmailModelProcess.BatchID.ToString()} BinID: {BinRegistorModel.BinID} LabRecNumber: {BinRegistorModel.LabRecNumber} EmailTo {WorkFlowEmailModelProcess.EmailTo}");
                            await PostApis.PostApisIntance.ApiEmailWorkFlow(WebApiUrl, $"{SqlConstants.ApiWorkFlowEmailController}{SqlConstants.EmailSpecimsProcess}", WorkFlowEmailModelProcess, auditLogs);

                        }
                        //  RedirectToAction("ViewBMCreateBatch");
                        totalQueryTime = InitAuditLogs.StopStopWatch();
                        auditLogs.LogInformation($"End ViewBMCreateBatchEmailWF onpostasync total time: {totalQueryTime} ms redirect to page ViewBMCreateBatchEmailWF");
                        return RedirectToPage("ViewBMCreateBatchEmailWF");
                    }
                    else
                    {
                        ViewData["CWID"] = await GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyCwid);
                    //    ViewData["UserProfile"] = await GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyUserProfile);
                        await Init();
                        totalQueryTime = InitAuditLogs.StopStopWatch();
                        auditLogs.LogInformation($"End ViewBMCreateBatchEmailWF on postasync total time: {totalQueryTime} ms");
                        
                    }


                }
                return Page();
            }
            catch (Exception ex)
            {
                auditLogs.LogError($"ViewBMCreateBatchEmailWF on post total time: {InitAuditLogs.StopStopWatch()} ms {ex.Message}");
                //  RedirectToPage($"/Error?ErrMEss=Model ViewBMCreateBatchEmailWF onGetAsync on get {ex.Message}");
                ViewData["ErrMess"] = $"/Error?ErrMEss=Model ViewBMCreateBatchEmailWF onGetAsync on get {ex.Message}";
              //  throw new Exception(ex.Message);
            }
            return Page();

        }
        private async Task TransFerBin(string oldBinId, string newBinId, string comments, ILog log)
        {
            string sp = $"{SqlConstants.WebApiBinMonitor}{SqlConstants.SpGetActiveBinsModelByBinId}/{oldBinId}";
            BinRegProcessModel ManageBatchModel = await GetApis.GetApisInctance.ApiActiveBinsModel(WebApiUrl, sp, log);
            string oldBatchId = ManageBatchModel.BatchID.ToString();
            ManageBatchModel.BatchID = Guid.NewGuid();
            ManageBatchModel.BinID = newBinId;
            log.LogInformation($"Metod TransFerBin old bindid: {oldBinId} new binid: {newBinId} new batch id:{ManageBatchModel.BatchID.ToString()}");
            if (!(string.IsNullOrEmpty(ManageBatchModel.BinComments)))
            {
                ManageBatchModel.BinComments = $"{ManageBatchModel.BinComments} {comments.Replace("{UserName}", Environment.UserName)}";
            }
            else
                ManageBatchModel.BinComments = comments.Replace("{UserName}", Environment.UserName); ;
            
            await ApiCreateBatch(ManageBatchModel, log);
            await PostApis.PostApisIntance.ApiUpdateTransFromEmailInfo(WebApiUrl, oldBatchId, ManageBatchModel.BatchID.ToString(), log);

        }
        private string CheckWorkFlow(WorkFlowEmailModel workFlowEmailModel, string emailAdd)
        {
            if ((workFlowEmailModel.EmailContents) || (workFlowEmailModel.EmailOnComplete) || (workFlowEmailModel.EmailOnStart))
            {
                if (string.IsNullOrEmpty(Request.Form[emailAdd]))
                    return "Invalid";
                return Request.Form[emailAdd];

            }
            return "NoWorkFlow";
        }
        private bool CheckVaild(BinRegProcessModel BinRegistorModel)
        {
            if (string.IsNullOrEmpty(BinRegistorModel.BinID))
            {
                ModelState.AddModelError("BinID", "BinId required");
                return false; ;
            }

            if (string.IsNullOrEmpty(BinRegistorModel.CategoryName))
            {
                ModelState.AddModelError("CategoryName", "Category Name required");
                return false; ;

            }
            return true;
        }
        public async Task ApiCreateBatch(BinRegProcessModel binRegistorProcModel, ILog log)
        {
            try
            {

                log.LogInformation($"Metod ApiCreateBatch BinID: {binRegistorProcModel.BinID} LabRecNumber: {binRegistorProcModel.LabRecNumber} weburl: {WebApiUrl} controller:{SqlConstants.ApiCreateBatch}");
                using (var client = new HttpClient())
                {
                    BinRegistorModel binRegistorModel = await GetApis.GetApisInctance.ConvertBinRegProcessModelToBinRegistorModel(binRegistorProcModel, SqlConstants.RegAllBatch);
                    client.BaseAddress = new Uri($"{WebApiUrl}");
                    var jsonString = JsonConvert.SerializeObject(binRegistorModel);
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    var responseTask = client.PostAsync($"{client.BaseAddress.AbsoluteUri}{SqlConstants.ApiCreateBatch}", content);
                    responseTask.Wait();
                    var result = await client.PostAsync("Method Address", content);
                    string resultContent = await result.Content.ReadAsStringAsync();
                    var results = responseTask.Result;
                    content.Dispose();
                    log.LogInformation($"Metod ApiCreateBatch BinID: {binRegistorProcModel.BinID} LabRecNumber: {binRegistorProcModel.LabRecNumber} weburl: {WebApiUrl} controller:{SqlConstants.ApiCreateBatch} results status code:{result.StatusCode}");
                    if (!(results.IsSuccessStatusCode))
                    {

                        throw new Exception($"Metod ApiCreateBatch BinID: {binRegistorProcModel.BinID} LabRecNumber: {binRegistorProcModel.LabRecNumber} weburl: {WebApiUrl} controller:{SqlConstants.ApiCreateBatch} results status code:{result.StatusCode}");
                    }
                }
            }
            catch (HttpRequestException httpex)
            {
                log.LogError($"HttpRequestException Metod ApiCreateBatch BinID: {binRegistorProcModel.BinID} LabRecNumber: {binRegistorProcModel.LabRecNumber} weburl: {WebApiUrl} controller:{SqlConstants.ApiCreateBatch} {httpex.Message}");
                throw new Exception($"Metod  HttpRequestException Metod ApiCreateBatch BinID: {binRegistorProcModel.BinID} LabRecNumber: {binRegistorProcModel.LabRecNumber} weburl: {WebApiUrl} controller:{SqlConstants.ApiCreateBatch} {httpex.Message}");
            }

            catch (ArgumentNullException ag)
            {
                log.LogError($"ArgumentNullException Metod ApiCreateBatch BinID: {binRegistorProcModel.BinID} LabRecNumber: {binRegistorProcModel.LabRecNumber} weburl: {WebApiUrl} controller:{SqlConstants.ApiCreateBatch} {ag.Message}");
                throw new Exception($"ArgumentNullException Metod ApiCreateBatch BinID: {binRegistorProcModel.BinID} LabRecNumber: {binRegistorProcModel.LabRecNumber} weburl: {WebApiUrl} controller:{SqlConstants.ApiCreateBatch} {ag.Message}");
            }
            catch (Exception ex)
            {
                log.LogError($"Exception Metod ApiCreateBatch BinID: {binRegistorProcModel.BinID} LabRecNumber: {binRegistorProcModel.LabRecNumber} weburl: {WebApiUrl} controller:{SqlConstants.ApiCreateBatch} {ex.Message}");
                throw new Exception($"Exception Metod ApiCreateBatch BinID: {binRegistorProcModel.BinID} LabRecNumber: {binRegistorProcModel.LabRecNumber} weburl: {WebApiUrl} controller:{SqlConstants.ApiCreateBatch} {ex.Message}");
            }
        }

        public async Task ApiOpenBins()
        {
            try
            {
                auditLogs.LogInformation($"Method ApiOpenBins weburl:{WebApiUrl} controller:{SqlConstants.ApiOpenBins}");
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri($"{WebApiUrl}");
                    var responseTask = client.GetAsync($"{SqlConstants.ApiOpenBins}");
                    responseTask.Wait();
                    var results = responseTask.Result;
                    auditLogs.LogInformation($"Method ApiOpenBins weburl:{WebApiUrl} controller:{SqlConstants.ApiOpenBins} results code: {results.StatusCode}");
                    if (results.IsSuccessStatusCode)
                    {
                        var readTask = results.Content.ReadAsAsync<BinModel[]>();
                        readTask.Wait();
                        List<string> tempList = readTask.Result.Select(p => p.BinID).ToList();
                        tempList.Add("  ");
                        tempList.Sort();
                        BinID = tempList;
                    }
                    else
                        throw new Exception($"Method ApiOpenBins weburl:{WebApiUrl} controller:{SqlConstants.ApiOpenBins} results code: {results.StatusCode}");
                }
            }
            catch (HttpRequestException httpex)
            {
                auditLogs.LogError($"HttpRequestException Method ApiOpenBins weburl:{WebApiUrl} controller:{SqlConstants.ApiOpenBins} {httpex.Message}");
                throw new Exception($"HttpRequestException Method ApiOpenBins weburl:{WebApiUrl} controller:{SqlConstants.ApiOpenBins} {httpex.Message}");
            }

            catch (ArgumentNullException ag)
            {
                auditLogs.LogError($"ArgumentNullException Method ApiOpenBins weburl:{WebApiUrl} controller:{SqlConstants.ApiOpenBins} {ag.Message}");
                throw new Exception($"ArgumentNullException  Method ApiOpenBins weburl:{WebApiUrl} controller:{SqlConstants.ApiOpenBins} {ag.Message}");
            }
            catch (Exception ex)
            {
                auditLogs.LogError($"Exception Method ApiOpenBins weburl:{WebApiUrl} controller:{SqlConstants.ApiOpenBins}  {ex.Message}");
                throw new Exception($"Exception Method ApiOpenBins weburl:{WebApiUrl} controller:{SqlConstants.ApiOpenBins} {ex.Message}");
            }

        }

        public async Task ApiUserInfo()
        {
            try
            {
                auditLogs.LogInformation($"Method ApiUserInfo weburl:{WebApiUrl} controller:{SqlConstants.ApiUserInfo}");

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri($"{WebApiUrl}");
                    var responseTask = client.GetAsync($"{SqlConstants.ApiUserInfo}");
                    responseTask.Wait();
                    var results = responseTask.Result;
                    auditLogs.LogInformation($"Method ApiUserInfo weburl:{WebApiUrl} controller:{SqlConstants.ApiUserInfo} results code: {results.StatusCode}");
                    if (results.IsSuccessStatusCode)
                    {
                        var readTask = results.Content.ReadAsAsync<UsersModel[]>();
                        readTask.Wait();
                        List<string> tempUserList = readTask.Result.Where(p => !(p.EmailAddress.ToLower().Contains("edocs"))).Select(p => p.FirstName + " " + p.LastName).ToList();
                        tempUserList.Add("  ");
                        tempUserList.Sort();
                        LassignedBy = tempUserList;
                        LassignedTo = LassignedBy;
                        //UserFirstLastName = readTask.Result.Select(p => p.FirstName + " " + p.LastName).ToList();
                        //LassignedBy = readTask.Result.Where(p => !(p.EmailAddress.ToLower().Contains("edocs"))).Select(p => p.FirstName + " " + p.LastName).ToList();

                        //LassignedTo = LassignedBy;
                        //CategoryName = readTask.Result.Where(p => !(p.CategoryName.ToLower().Contains("edocs"))).Select(p => p.CategoryName).ToList();

                    }
                    else
                        throw new Exception($"Method ApiUserInfo weburl:{WebApiUrl} controller:{SqlConstants.ApiUserInfo} results code: {results.StatusCode}");
                }
            }
            catch (HttpRequestException httpex)
            {
                auditLogs.LogError($"HttpRequestException Method ApiUserInfo weburl:{WebApiUrl} controller:{SqlConstants.ApiUserInfo}  {httpex.Message}");
                throw new Exception($"HttpRequestException Method ApiUserInfo weburl:{WebApiUrl} controller:{SqlConstants.ApiUserInfo}  {httpex.Message}");
            }

            catch (ArgumentNullException ag)
            {
                auditLogs.LogError($"ArgumentNullException Method ApiUserInfo weburl:{WebApiUrl} controller:{SqlConstants.ApiUserInfo}  {ag.Message}");
                throw new Exception($"ArgumentNullException  Method ApiUserInfo weburl:{WebApiUrl} controller:{SqlConstants.ApiUserInfo}  {ag.Message}");
            }
            catch (Exception ex)
            {
                auditLogs.LogError($"Exception Method ApiUserInfo weburl:{WebApiUrl} controller:{SqlConstants.ApiUserInfo}  {ex.Message}");
                throw new Exception($"Exception Method ApiUserInfo weburl:{WebApiUrl} controller:{SqlConstants.ApiUserInfo}  {ex.Message}");
            }
        }
        public async Task ApiCategories()
        {
            try
            {
                auditLogs.LogInformation($"Method ApiCategories weburl:{WebApiUrl} controller:{SqlConstants.ApiCatNameID}");

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri($"{WebApiUrl}");
                    var responseTask = client.GetAsync($"{SqlConstants.ApiCatNameID}");
                    responseTask.Wait();
                    var results = responseTask.Result;
                    auditLogs.LogInformation($"Method ApiCategories weburl:{WebApiUrl} controller:{SqlConstants.ApiCatNameID} results code: {results.StatusCode}");

                    if (results.IsSuccessStatusCode)
                    {
                        var readTask = results.Content.ReadAsAsync<CategoryIdName[]>();
                        readTask.Wait();
                        List<string> tempList = readTask.Result.Select(p => p.CategoryName).ToList();
                        tempList.Add("  ");
                        tempList.Sort();
                        CategoryName = tempList;
                    }
                    else
                        throw new Exception($"Method ApiCategories weburl:{WebApiUrl} controller:{SqlConstants.ApiCatNameID} results code: {results.StatusCode}");
                }
            }
            catch (HttpRequestException httpex)
            {
                auditLogs.LogError($"HttpRequestException Method ApiCategories weburl:{WebApiUrl} controller:{SqlConstants.ApiCatNameID} {httpex.Message}");
                throw new Exception($"HttpRequestException Method ApiCategories weburl:{WebApiUrl} controller:{SqlConstants.ApiCatNameID} {httpex.Message}");
            }

            catch (ArgumentNullException ag)
            {
                auditLogs.LogError($"ArgumentNullException Method ApiCategories weburl:{WebApiUrl} controller:{SqlConstants.ApiCatNameID} {ag.Message}");
                throw new Exception($"ArgumentNullException  Method ApiCategories weburl:{WebApiUrl} controller:{SqlConstants.ApiCatNameID} {ag.Message}");
            }
            catch (Exception ex)
            {
                auditLogs.LogError($"Exception Method ApiCategories weburl:{WebApiUrl} controller:{SqlConstants.ApiCatNameID} {ex.Message}");
                throw new Exception($"Exception Method ApiCategories weburl:{WebApiUrl} controller:{SqlConstants.ApiCatNameID} {ex.Message}");
            }

        }
    }
}