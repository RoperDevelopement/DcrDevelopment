using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Configuration;
using BinMonitorAppService.Models;
using BinMonitorAppService.ApiClasses;
using BinMonitorAppService.Constants;
using Microsoft.AspNetCore.Http;
using BinMonitorAppService.Logging;
using System.Security.AccessControl;

namespace BinMonitorAppService.Pages.BinManager
{
    public class BinManagerHomeModel : PageModel
    {
        public readonly string SessionKeyCwid = "Cwid";
        public readonly string SessionKeyUserProfile = "UserProfile";
        private readonly IConfiguration configuration;
        private ILog auditLogs;
        private string WebApiUrl
        { get { return configuration.GetValue<string>("BinMonitorApi").ToString(); } }
        public SpectrumMonitorMenuRightsModel SpectrumMonitorMenuRightsModel
        { get; set; }


        public BinManagerHomeModel(IConfiguration config, ILog logConfig)
        {
            configuration = config;
            auditLogs = logConfig;
        }
        [BindProperty]
        public IDictionary<string, BinRegProcessModel> BeginRegModel
        { get; set; }
        [BindProperty]
        public IDictionary<string, BinRegProcessModel> CompleteRegModel
        { get; set; }
        [BindProperty]
        public IDictionary<string, BinRegProcessModel> BeginProcessModel
        { get; set; }
        [BindProperty]
        public IDictionary<string, BinRegProcessModel> CompleteProcessModel
        { get; set; }
        [BindProperty]
        public IDictionary<string, BinRegProcessModel> BeginBothModel
        { get; set; }
        [BindProperty]
        public IDictionary<string, BinRegProcessModel> CloseBatch
        { get; set; }

        [BindProperty]

        [MinLength(4)]
        public IList<string> BinID
        { get; set; }

        [BindProperty]
        [MaxLength(10)]
        public IList<string> CategoryName
        { get; set; }
        public SpecMonitorFormUserPre MonitorFormUserPre
        { get; set; }
        public List<string> LabReqNumbers
        { get; set; }
        public IList<string> TransFerCategoryName
        { get; set; }
        // IDictionary<string, BinRegProcessModel>  ActiveBinsCategoroy
        //{ get; set; }
        public IList<string> CatBins
        { get; set; }
        public string SessionExpires
        { get; set; }

        private async Task GetLogInformaiton()
        {

            auditLogs = InitAuditLogs.LogAsync(auditLogs, HttpContext.Session).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                InitAuditLogs.StartStopWatch();
                GetLogInformaiton().ConfigureAwait(false).GetAwaiter().GetResult();
                auditLogs.LogInformation("Start BinManagerHomeModel onGet");

                if (!(User.Identity.IsAuthenticated))
                {
                    auditLogs.LogInformation($"End BinManagerHomeModel onGet renew session using url /BinUsers/LoginView?returnUrl=/BinManager/ViewManagerHome total time: {InitAuditLogs.StopStopWatch()} ms");
                    return Redirect("/BinUsers/LoginView?returnUrl=/BinManager/ViewManagerHome");
                }
                ViewData["CWID"] = await GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyCwid);
                if (ViewData["CWID"] == null)
                {
                    auditLogs.LogInformation($"End BinManagerHomeModel onGet renew session using url /BinUsers/LoginView?returnUrl=/BinManager/ViewManagerHome total time: {InitAuditLogs.StopStopWatch()} ms");
                    return Redirect("/BinUsers/LoginView?returnUrl=/BinManager/ViewManagerHome");
                }

                int totMinutesLeft = ApiSetUserProfile.UserProfileInstance.GetSessionTime(HttpContext.Session, 1).ConfigureAwait(false).GetAwaiter().GetResult();
                auditLogs.LogInformation($"BinManagerHomeModel session seconds left: {totMinutesLeft} ms");
                if (totMinutesLeft < 0)
                    return Redirect("/BinUsers/LoginView?returnUrl=/BinManager/ViewManagerHome");

                SessionExpires = totMinutesLeft.ToString();
                SpectrumMonitorMenuRightsModel = ApiSetUserProfile.UserProfileInstance.GetBMMenuRights(User.Identity.Name, HttpContext.Session, auditLogs, WebApiUrl).ConfigureAwait(false).GetAwaiter().GetResult();
                GetViewData().ConfigureAwait(true).GetAwaiter().GetResult();

                auditLogs.LogInformation($"End BinManagerHomeModel total time: {InitAuditLogs.StopStopWatch()} ms");
            }
            catch (Exception ex)
            {
                auditLogs.LogError($"BinManagerHomeModel total time: {InitAuditLogs.StopStopWatch()} ms {ex.Message}");
                return Redirect($"/BinUsers/DisplayErrorMessagesView?ErrMEss=Model BinManagerHomeModel OngetAsync {ex.Message}");

            }
            return Page();

        }

        private async Task GetViewData()
        {
            ViewData[SqlConstants.ViewDataChangeCategories] = SpectrumMonitorMenuRightsModel.Categories;
            ViewData[SqlConstants.ViewDataEmailReports] = SpectrumMonitorMenuRightsModel.EmailReports;
            ViewData[SqlConstants.ViewDataRunReports] = SpectrumMonitorMenuRightsModel.RunReports;

        }

        public async Task<IActionResult> OnPostAsync()
        {


            string totalQueryTime = string.Empty;
            try
            {
                if (!(User.Identity.IsAuthenticated))
                {
                    return Redirect("/BinUsers/LoginView?returnUrl=/BinManager/ViewManagerHome");

                }
                ViewData["CWID"] = await GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyCwid);
                if (ViewData["CWID"] == null)
                    Redirect("/BinUsers/LoginView");

                InitAuditLogs.StartStopWatch();
                GetLogInformaiton().ConfigureAwait(false).GetAwaiter().GetResult();
                auditLogs.LogInformation("Start BinManagerHomeModel OnPostAsync");

                ViewData["CWID"] = User.Identity.Name;
                SpectrumMonitorMenuRightsModel = ApiSetUserProfile.UserProfileInstance.GetBMMenuRights(User.Identity.Name, HttpContext.Session, auditLogs, WebApiUrl).ConfigureAwait(false).GetAwaiter().GetResult();
                GetViewData().ConfigureAwait(true).GetAwaiter().GetResult();
                if (!(string.IsNullOrWhiteSpace(Request.Form["compReg"])))
                {
                    if ((string.Compare(Request.Form["compReg"], "regComp", true) == 0))
                    {
                        auditLogs.LogInformation("Start BinManagerHomeModel OnPostAsync Complete Registration");
                        await UpDateCompleteRegistration();
                    }



                }
                else if (!(string.IsNullOrWhiteSpace(Request.Form["TransLabReqsBinID"])))
                {
                    TransLabReqsAsync().ConfigureAwait(true).GetAwaiter().GetResult();
                }
                else if (!(string.IsNullOrWhiteSpace(Request.Form["tranProc"])))
                {
                  
                    TransFerProcess(User.Identity.Name).ConfigureAwait(true).GetAwaiter().GetResult();
                }
                else if (!(string.IsNullOrWhiteSpace(Request.Form["oldTransBinID"])))
                {
                    TransBinsAsync().ConfigureAwait(true).GetAwaiter().GetResult();
                }
                else if (!(string.IsNullOrWhiteSpace(Request.Form["catTrans"])))
                {
                    TransBinsCategories().ConfigureAwait(true).GetAwaiter().GetResult();
                }

                else if (!(string.IsNullOrWhiteSpace(Request.Form["beginReg"])))
                {
                    if (string.Compare(Request.Form["beginReg"], "regBegin", true) == 0)
                    {
                        auditLogs.LogInformation("Start BinManagerHomeModel OnPostAsync Begin Registration");
                        await UpDateBeginRegistration();
                    }
                }
                else if (!(string.IsNullOrWhiteSpace(Request.Form["beginProc"])))
                {
                    if (string.Compare(Request.Form["beginProc"], "procBegin", true) == 0)
                    {
                        auditLogs.LogInformation("Start BinManagerHomeModel OnPostAsync Begin Processing");
                        await UpDateBeginProcess(await GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyCwid));
                    }

                }
                else if (!(string.IsNullOrWhiteSpace(Request.Form["compProc"])))
                {
                    if (string.Compare(Request.Form["compProc"], "procComp", true) == 0)
                    {
                        auditLogs.LogInformation("Start BinManagerHomeModel OnPostAsync Complete Processing");
                        await UpDateCompleteProcess();
                    }

                }
                else if (!(string.IsNullOrWhiteSpace(Request.Form["batchClose"])))
                {
                    auditLogs.LogInformation("Start BinManagerHomeModel OnPostAsync Close Batch");
                    await CompletBatch(await GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyCwid));
                }
                else if (!(string.IsNullOrWhiteSpace(Request.Form["batchcompregstprocess"])))
                {
                    auditLogs.LogInformation("Start BinManagerHomeModel OnPostAsync Complete reg start proc");
                    await CompRegStProcess();
                }
                else
                    throw new Exception("Model not found");
                //  ViewData["UserProfile"] = await GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyUserProfile);
                //    MonitorFormUserPre = await GetSessionVariables.SessionVarInstance.GetJsonSessionObject<SpecMonitorFormUserPre>(HttpContext.Session, "ProfileUser");

                totalQueryTime = InitAuditLogs.StopStopWatch();

                auditLogs.LogInformation($"End  BinManagerHomeModel OnPostAsyn total time: {totalQueryTime} ms redirect to page BinManager/ViewManagerHome");





            }
            catch (Exception ex)
            {
                auditLogs.LogInformation($"End  BinManagerHomeModel OnPostAsyn total time: {InitAuditLogs.StopStopWatch()} ms redirect to page BinManager/ViewManagerHome");
                throw new Exception($"/Error?ErrMEss=Model BinManagerHomeModel OnPostAsync {ex.Message}");
               // new RedirectToPageResult($"/Error?ErrMEss=Model BinManagerHomeModel OnPostAsync {ex.Message}");

            }
            return Redirect("/BinUsers/LoginView?returnUrl=/BinManager/ViewManagerHome");


        }


        public async Task TransBinsCategories()
        {
            //ViewData["CWID"] = await GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyCwid);
            //if (ViewData["CWID"] == null)
            //    Redirect("/BinUsers/LoginView");
            try
            {
                //  InitAuditLogs.StartStopWatch();
                // GetLogInformaiton().ConfigureAwait(false).GetAwaiter().GetResult();
                auditLogs.LogInformation("Start BinManagerHomeModel OnPostTransBinsCategoriesAsync");
                SpectrumMonitorMenuRightsModel = ApiSetUserProfile.UserProfileInstance.GetBMMenuRights(User.Identity.Name, HttpContext.Session, auditLogs, WebApiUrl).ConfigureAwait(false).GetAwaiter().GetResult();
                ChangeCategories().ConfigureAwait(false).GetAwaiter().GetResult();

            }
            catch (Exception ex)
            {
                auditLogs.LogError($"End BinManagerHomeModel total time: {InitAuditLogs.StopStopWatch()} {ex.Message}");
                throw new Exception(ex.Message);
            }

        }
        private async Task ChangeCategories()
        {

            string newCatID = Request.Form["catTrans"].ToString();
            string catTransFer = Request.Form["catNewTrans"].ToString();
            auditLogs.LogInformation($"Method ChangeCategories() BinManagerHomeModel new category: {newCatID} old categories: {catTransFer}");
            Dictionary<string, BinRegProcessModel> keyValuePairs = GetSessionVariables.SessionVarInstance.GetJsonSessionObject<Dictionary<string, BinRegProcessModel>>(HttpContext.Session, SqlConstants.CachBinRegProcessModel).ConfigureAwait(false).GetAwaiter().GetResult();

            //transFerModel.BinID = string.Empty;
            //transFerModel.OldBinId = string.Empty;
            //transFerModel.CategoryName = newCatID;
            //transFerModel.LabReqNumber = string.Empty;
            foreach (string nameCat in catTransFer.Split(','))
            {
                TransFerModel transFerModel = new TransFerModel();
                string[] cat = nameCat.Split('-');
                transFerModel.CategoryName = newCatID;
                transFerModel.BinID = cat[0];
                transFerModel.OldBinId = cat[1];
                transFerModel.Comments = $"CWID {User.Identity.Name} transfer binid category {cat[0]} to new category {newCatID} in binid {cat[1]}";
                transFerModel.TransferBy = User.Identity.Name;
                transFerModel.TransFerType = TransferType.TC.ToString();
                auditLogs.LogInformation($"{transFerModel.Comments}");
                TransferApi.GetTransFerApisIntance.TransFer(transFerModel, WebApiUrl, SqlConstants.ApiTransFer, auditLogs).Wait();
            }
        }

        private async Task ChangeLabReqs()
        {

            string newLabReqBinID = Request.Form["TransLabReqsBinID"].ToString();
            string oldLabReqsBinId = Request.Form["TransLabReqsOldLabReqBinID"].ToString();
            auditLogs.LogInformation($"Method ChangeLabReqs() BinManagerHomeModel OnPostTransLabReqsAsync for new binid:{newLabReqBinID} for old binid:{oldLabReqsBinId}");


            foreach (string nameLabReq in oldLabReqsBinId.Split(','))
            {
                string[] lReq = nameLabReq.Split('-');
                TransFerModel transFerModel = new TransFerModel();
                transFerModel.BinID = newLabReqBinID;
                transFerModel.OldBinId = lReq[0];
                transFerModel.LabReqNumber = lReq[1];
                transFerModel.TransFerType = TransferType.TL.ToString();
                transFerModel.TransferBy = User.Identity.Name;
                auditLogs.LogInformation($"CWID {User.Identity.Name} transfering labreq to new bin {transFerModel.BinID} from old bindid {transFerModel.OldBinId} labreq {transFerModel.LabReqNumber}");
             
                TransferApi.GetTransFerApisIntance.TransFer(transFerModel, WebApiUrl, SqlConstants.ApiTransFer, auditLogs).Wait();
                
            }
        }
        public async Task TransLabReqsAsync()
        {
            ViewData["CWID"] = await GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyCwid);
            if (ViewData["CWID"] == null)
                Redirect("/BinUsers/LoginView");
            try
            {
                InitAuditLogs.StartStopWatch();
                GetLogInformaiton().ConfigureAwait(false).GetAwaiter().GetResult();
                auditLogs.LogInformation("Start BinManagerHomeModel OnPostTransLabReqsAsync");
                SpectrumMonitorMenuRightsModel = ApiSetUserProfile.UserProfileInstance.GetBMMenuRights(User.Identity.Name, HttpContext.Session, auditLogs, WebApiUrl).ConfigureAwait(false).GetAwaiter().GetResult();
                GetViewData().ConfigureAwait(true).GetAwaiter().GetResult();
                ChangeLabReqs().ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                auditLogs.LogInformation($"End BinManagerHomeModel OnPostTransLabReqsAsync  {InitAuditLogs.StopStopWatch()} ms {ex.Message}");
                throw new Exception(ex.Message);
            }



        }
        public async Task TransBinsAsync()
        {
            ViewData["CWID"] = await GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyCwid);

            try
            {

                SpectrumMonitorMenuRightsModel = ApiSetUserProfile.UserProfileInstance.GetBMMenuRights(User.Identity.Name, HttpContext.Session, auditLogs, WebApiUrl).ConfigureAwait(false).GetAwaiter().GetResult();
                GetViewData().ConfigureAwait(true).GetAwaiter().GetResult();
                BinsTrans().ConfigureAwait(false).GetAwaiter().GetResult();


            }
            catch (Exception ex)
            {
                auditLogs.LogError($"End BinManagerHomeModel OnPostTransLabReqsAsync total time: {InitAuditLogs.StopStopWatch()} ms {ex.Message}");
                throw new Exception(ex.Message);
            }


        }
        private async Task BinsTrans()
        {
            string transFerFrom = Request.Form["oldTransBinID"].ToString();
            string newBinId = Request.Form["newTransBinID"].ToString();
            string comments = Request.Form["#tfbincomm"].ToString();
            foreach (var oBID in transFerFrom.Split(","))
            {
                TransFerModel transFer = new TransFerModel();
                transFer.BinID = newBinId;
                transFer.OldBinId = oBID;
                //transFer.TransferBy = User.Identity.Name;
                transFer.TransferBy = User.Identity.Name;
                transFer.TransFerType = TransferType.TB.ToString();
                transFer.Comments = comments;
                auditLogs.LogInformation($"BinsTrans Transfering bins  for webapi:{WebApiUrl} for old bindid: {oBID} to new binid: {newBinId} requested by user: {User.Identity.Name}");
                TransferApi.GetTransFerApisIntance.TransFer(transFer, WebApiUrl, SqlConstants.ApiTransFer, auditLogs).ConfigureAwait(false).GetAwaiter().GetResult();
                // TransferApi.GetTransFerApisIntance.TransFerBin(WebApiUrl, oBID, newBinId, comments, User.Identity.Name, auditLogs).ConfigureAwait(false).GetAwaiter().GetResult();
            }

        }
        private async Task UpDateCompleteRegistration()
        {
            BinRegProcessModel completeRegModel = new BinRegProcessModel();
            string bId = Request.Form["BinID"];
            completeRegModel.BinID = Request.Form[bId + "crBinID"];
            completeRegModel.BatchID = Guid.Parse(Request.Form[bId + "CrBatchId"]);
            completeRegModel.CategoryName = Request.Form[bId + "CategoryNameCReg"];
            completeRegModel.RegAssignedBy = Request.Form[bId + "RegAssignedByCReg"];
            completeRegModel.RegAssignedTo = Request.Form[bId + "RegAssignedToCReg"];
            completeRegModel.RegCompletedBy = Environment.UserName;
            completeRegModel.RegCompletedAt = DateTime.Now;
            completeRegModel.RegStartedAt = DateTime.Parse(Request.Form[bId + "RegStartedAtCReg"]);
            completeRegModel.RegCreatedBy = Request.Form[bId + "RegCreatedByCReg"];
            auditLogs.LogInformation($"UpDateCompleteRegistration BinID: {completeRegModel.BinID} BatchID: {completeRegModel.BatchID.ToString()} CategoryName: {completeRegModel.CategoryName} RegCreatedBy:{completeRegModel.RegCreatedBy}");
            await GetApis.GetApisInctance.ApiCreateUpdateBatch(completeRegModel, WebApiUrl, "regbatch", auditLogs);
        }

        private async Task UpDateBeginRegistration()
        {
            BinRegProcessModel completeRegModel = new BinRegProcessModel();
            string bId = Request.Form["BinID"];
            completeRegModel.BinID = bId;
            completeRegModel.CategoryName = Request.Form["CategoryName"];
            completeRegModel.RegAssignedBy = Environment.UserName;
            completeRegModel.RegAssignedTo = Environment.UserName;
            completeRegModel.RegCompletedBy = string.Empty;
            completeRegModel.RegStartedAt = DateTime.Now;
            completeRegModel.RegCreatedBy = Environment.UserName;
            auditLogs.LogInformation($"UpDateBeginRegistration BinID: {completeRegModel.BinID} BatchID: {completeRegModel.BatchID.ToString()} CategoryName: {completeRegModel.CategoryName} RegCreatedBy:{completeRegModel.RegCreatedBy}");
            await GetApis.GetApisInctance.ApiCreateUpdateBatch(completeRegModel, WebApiUrl, SqlConstants.RegAllBatch, auditLogs);
        }

        private async Task CompRegStProcess()
        {
            BinRegProcessModel completeRegModel = new BinRegProcessModel();
            string bId = Request.Form["BinIDCompRegStProc"];
            completeRegModel.BinID = bId;
            completeRegModel.BatchID = Guid.Parse(Request.Form[bId + "BatchIdCompRegStProc"]);
            completeRegModel.CategoryName = Request.Form[bId + "CategoryNameCompRegStProc"];
            completeRegModel.RegAssignedBy = Request.Form[bId + "RegAssignedByCompRegStProc"];
            completeRegModel.RegAssignedTo = Request.Form[bId + "RegAssignedToCompRegStProc"];
            completeRegModel.RegCompletedBy = Environment.UserName;
            completeRegModel.RegCompletedAt = DateTime.Now;
            completeRegModel.RegStartedAt = DateTime.Parse(Request.Form[bId + "RegStartedAtCompRegStProc"]);
            completeRegModel.RegCreatedBy = Request.Form[bId + "RegCreatedByCompRegStProc"];
            completeRegModel.ProcessAssignedBy = Environment.UserName;
            completeRegModel.ProcessAssignedTo = Environment.UserName;
            completeRegModel.ProcessCompletedAt = DateTime.Parse(Request.Form[bId + "ProcessCompletedAtCompRegStProc"]);
            completeRegModel.ProcessStartAt = DateTime.Now;
            completeRegModel.ProcessCompletedBy = Request.Form[bId + "ProcessCompletedByCompRegStProc"];
            auditLogs.LogInformation($"CompRegStProcess BinID: {completeRegModel.BinID} BatchID: {completeRegModel.BatchID.ToString()} CategoryName: {completeRegModel.CategoryName} RegCreatedBy:{completeRegModel.RegCreatedBy}");
            await GetApis.GetApisInctance.ApiCreateUpdateBatch(completeRegModel, WebApiUrl, "regbatch", auditLogs);
            await GetApis.GetApisInctance.ApiCreateUpdateBatch(completeRegModel, WebApiUrl, "regProcessBatches", auditLogs);
        }


        private async Task UpDateBeginProcess(string cwid)

        {

            string bId = Request.Form["BinIDProcssBegin"];
            if (string.IsNullOrWhiteSpace(bId))
            {
                throw new Exception("Invalid bin id");
            }
            string assTo = Request.Form["ProcessAssignedTo"];
            if (!(string.IsNullOrWhiteSpace(assTo)))
                cwid = assTo;
            foreach (var bpBinId in bId.Split(","))
            {
                auditLogs.LogInformation($"UpDateBeginProcess BinID: {bpBinId} cwid: {cwid}");
                await CloseBatches.CloseBatchesInctance.UpDateBeginProcess(cwid, bpBinId, WebApiUrl, auditLogs);
            }
        }

        private async Task TransFerProcess(string cwid)

        {

            string bId = Request.Form["BinIDProcssBegin"];
            if (string.IsNullOrWhiteSpace(bId))
            {
                throw new Exception("Invalid bin id");
            }
            string assTo = Request.Form["TransProcessAssignedTo"];
            if ((string.IsNullOrWhiteSpace(assTo)))
                assTo = cwid;
            if (string.Compare(assTo, "Current Cwid", true) == 0)
                assTo = User.Identity.Name;
            foreach (var bpBinId in bId.Split(","))
            {

                TransFerModel transFer = new TransFerModel();
                transFer.BinID = bpBinId;
                //  /transFer.TransferBy = User.Identity.Name;
                transFer.TransferBy = User.Identity.Name;
                transFer.TransFerType = TransferType.TP.ToString();
                transFer.Processing = assTo;
                auditLogs.LogInformation($"CWID {User.Identity.Name} transfering process for binid {transFer.BinID} to CWID {transFer.Processing}");
                //  auditLogs.LogInformation($"BinsTrans Transfering bins  for webapi:{WebApiUrl} for old bindid: {oBID} to new binid: {newBinId} requested by user: {User.Identity.Name}");
                TransferApi.GetTransFerApisIntance.TransFer(transFer, WebApiUrl, SqlConstants.ApiTransFer, auditLogs).ConfigureAwait(false).GetAwaiter().GetResult();
            }
        }


        private async Task UpDateCompleteProcess()
        {
            BinRegProcessModel completeRegModel = new BinRegProcessModel();
            string bId = Request.Form["BinIDCompleteProc"];
            completeRegModel.BinID = bId;
            completeRegModel.BatchID = Guid.Parse(Request.Form[bId + "BatchIdcompProc"]);
            completeRegModel.ProcessAssignedBy = Request.Form[bId + "ProcessAssignedBycompProc"];
            completeRegModel.ProcessAssignedTo = Request.Form[bId + "ProcessAssignedTocompProc"];
            completeRegModel.ProcessCompletedAt = DateTime.Now;
            completeRegModel.ProcessStartAt = DateTime.Now;
            completeRegModel.ProcessCompletedBy = Environment.UserName;
            auditLogs.LogInformation($"UpDateCompleteProcess BinID: {completeRegModel.BinID} BatchID: {completeRegModel.BatchID.ToString()} CategoryName: {completeRegModel.CategoryName} RegCreatedBy:{completeRegModel.RegCreatedBy} ProcessAssignedBy {completeRegModel.ProcessAssignedBy} ProcessAssignedTo {completeRegModel.ProcessAssignedTo} ");
            await GetApis.GetApisInctance.ApiCreateUpdateBatch(completeRegModel, WebApiUrl, "regProcessBatches", auditLogs);
        }

        private async Task CompletBatch(string cwid)
        {
            string bId = Request.Form["BinIDCloseBatch"];
            foreach (var closeBinid in bId.Split(','))
            {
                auditLogs.LogInformation($"CompletBatch BinID: {closeBinid} user cwid {cwid}");
                await CloseBatches.CloseBatchesInctance.CompletBatch(cwid, closeBinid, WebApiUrl, auditLogs);
            }
        }



    }
}