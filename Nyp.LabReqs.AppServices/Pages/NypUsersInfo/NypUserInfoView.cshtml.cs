using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using EDocs.Nyp.LabReqs.AppServices.Models;
using EDocs.Nyp.LabReqs.AppServices.ApiClasses;
using EDocs.Nyp.LabReqs.AppServices.LabReqsConstants;
using EDocs.Nyp.LabReqs.AppService.Logging;

namespace EDocs.Nyp.LabReqs.AppServices
{
    public class NypUserInfoViewModel : PageModel
    {
        private readonly IConfiguration configuration;
        private ILog log;
        private string WebApiUrl
        {
            get { return configuration.GetValue<string>("LabResApi").ToString(); }
        }


        public NypUserInfoViewModel(IConfiguration config, ILog logConfig)
        {
            configuration = config;
            log = logConfig;
        }
        private async Task GetLogInformaiton()
        {

            log = InitAuditLogs.LogAsync(log, HttpContext.Session).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        public IDictionary<string, NypLabReqsUsersModel> LabRecsUser
        { get; private set; }

        [BindProperty]
        public NypLabReqsUsersModel LabReqsUsersModel
        { get; set; }

        public string SelUser
        { get; set; }

        [BindProperty]
        public string NewCwid
        { get; set; }
        [BindProperty]
        public bool NewUserViewAuditLogs
        { get; set; }
        [BindProperty]
        public bool NewUserIsAdmin
        { get; set; }


        public async Task<IActionResult> OnGetAsync(NypLabReqsUsersModel LabReqsUsersModel, string NewCwid = null, bool NewUserViewAuditLogs = false, bool NewUserIsAdmin = false)
        {
            var qString = Request.QueryString;
            if (!(User.Identity.IsAuthenticated))
            {
                if (qString.HasValue)
                {
                    if (!(string.IsNullOrWhiteSpace(Request.Query["addNewuser"].ToString())))
                    {
                        return Redirect($"/NypUsersInfo/LoginView?returnUrl=/NypUsersInfo/NypUserInfoView?addNewuser=addNewuser&NewCwid={NewCwid}&NewUserViewAuditLogs={NewUserViewAuditLogs}&NewUserIsAdmin={NewUserIsAdmin}");
                    }
                    else
                    {
                        return Redirect($"/NypUsersInfo/LoginView?returnUrl=/NypUsersInfo/NypUserInfoView{qString.Value}");
                    }
                }

                else
                    return Redirect($"/NypUsersInfo/LoginView?returnUrl=/NypUsersInfo/NypUserInfoView");
            }

            ViewData["CWID"] = GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyCwid).ConfigureAwait(true).GetAwaiter().GetResult();
            if (ViewData["CWID"] == null)
                return Redirect("/NypUsersInfo/LoginView?returnUrl=/NypUsersInfo/NypUserInfoView");
            try
            {
                LabReqHelpers.StartStopWatch();
                GetLogInformaiton().ConfigureAwait(false).GetAwaiter().GetResult();
                log.LogInformation("Start NypUserInfoViewModel");
                GetViewAudLogsAdmin();

                if (qString.HasValue)
                {
                    log.LogInformation($"NypUserInfoViewModel query string {qString.Value}");
                    if (!(string.IsNullOrWhiteSpace(Request.Query["addNewuser"].ToString())))
                    {
                        AddUserAsync(NewCwid, NewUserViewAuditLogs, NewUserIsAdmin).ConfigureAwait(true).GetAwaiter().GetResult();
                    }
                    else if (!(string.IsNullOrWhiteSpace(Request.Query["exportuser"].ToString())))
                    {
                        FileContentResult fileContentResult= ExportUsers().ConfigureAwait(false).GetAwaiter().GetResult();
                        return fileContentResult;
                    }
                    else
                    {
                        if (!(string.IsNullOrWhiteSpace(Request.Query["selCwid"].ToString())))
                        {
                            SelUser = Request.Query["selCwid"].ToString();
                            log.LogInformation($"NypUserInfoViewModel Selected user {SelUser}");
                        }
                    }



                }
                Init().ConfigureAwait(true).GetAwaiter().GetResult();
                GetUserInfor().ConfigureAwait(true).GetAwaiter().GetResult();
                log.LogInformation($"End NypUserInfoViewModel total time: {LabReqHelpers.StopStopWatch()} ms");
            }
            catch (Exception ex)
            {
                log.LogError($"End NypUserInfoViewModel total time: {LabReqHelpers.StopStopWatch()} ms {ex.Message}");
                return Redirect($"/NypUsersInfo/DisplayErrorMessages?ErrMEss= {ex.Message}");
            }
            return Page();
        }

        public async Task Init()
        {
            if (string.IsNullOrWhiteSpace(SelUser))
                SelUser = ViewData["CWID"].ToString();
            log.LogInformation($"NypUserInfoViewModel Method Init for User {SelUser}");
            GetLabRecUsers().ConfigureAwait(false).GetAwaiter().GetResult();


        }
        private void GetViewAudLogsAdmin()
        {
            LabReqsIsAdminVeiwAuditLogs IsAdminVeiwAuditLogs = LabReqsConstants.GetSessionVariables.SessionVarInstance.GetJsonSessionObject<LabReqsIsAdminVeiwAuditLogs>(HttpContext.Session, ConstNypLabReqs.SessionIsAdminVeiwAuditLogs).ConfigureAwait(false).GetAwaiter().GetResult();
            ViewData["IsAdmin"] = IsAdminVeiwAuditLogs.IsAdmin;
            ViewData["ViewAuditLogs"] = IsAdminVeiwAuditLogs.ViewAuditLogs;
            ViewData["EditLRDocs"] = IsAdminVeiwAuditLogs.EditLRDocs;
        }
        private async Task<FileContentResult> ExportUsers()
        {
            GetLabRecUsers().ConfigureAwait(true).GetAwaiter().GetResult();
            if(LabRecsUser.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Cwid,FirstName,LastName,Email,Last Logged In,View Audit Logs,Is Active");
            foreach(KeyValuePair<string,NypLabReqsUsersModel> keyValuePair in LabRecsUser)
            {
                    sb.AppendLine($@"{keyValuePair.Key},{keyValuePair.Value.FirstName},{keyValuePair.Value.LastName},{keyValuePair.Value.EmailAddress},{keyValuePair.Value.LastLoggedIn},{keyValuePair.Value.ViewAuditLogs.ToString()},{keyValuePair.Value.Active.ToString()}");
            }

               // Response.Headers.Add("Content-Disposition", "attachment;filename=LabReqUsers.csv");
                return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "LabReqUsers.csv");
            }
            return null;
        }
        private async Task GetLabRecUsers()
        {
            log.LogInformation("NypUserInfoViewModel Method GetLabRecUsers");
            LabRecsUser = GetSessionVariables.SessionVarInstance.GetJsonSessionObject<IDictionary<string, NypLabReqsUsersModel>>(HttpContext.Session, ConstNypLabReqs.SessionLabRecsUsers).ConfigureAwait(true).GetAwaiter().GetResult();
            if ((LabRecsUser == null) || (LabRecsUser.Count == 0))
            {

                string userInfor = string.Format(ConstNypLabReqs.ApiNypLabReqsUsersParam, ConstNypLabReqs.NANoSlash, ConstNypLabReqs.NANoSlash, ConstNypLabReqs.NANoSlash, ConstNypLabReqs.NANoSlash, ConstNypLabReqs.NypUserCommandGetUsers);
                log.LogInformation($"NypUserInfoViewModel Method GetLabRecUsers for web api controller {WebApiUrl}{ConstNypLabReqs.ApiNypLabReqsUsersController} for params {userInfor}");
                LabRecsUser = NypLabReqsUserInfo.NypLabReqsUserInfoApiIntance.GetAllNypUserInfor(new Uri($"{WebApiUrl}{ConstNypLabReqs.ApiNypLabReqsUsersController}"), userInfor, log).ConfigureAwait(true).GetAwaiter().GetResult();
                LabRecsUser.Remove("dar9229");
                GetSessionVariables.SessionVarInstance.SetSessionOjbjectAsJson(HttpContext.Session, ConstNypLabReqs.SessionLabRecsUsers, LabRecsUser).ConfigureAwait(true).GetAwaiter().GetResult();
            }
        }
        private async Task<NypLabReqsUsersModel> GetUserInfor(NypLabReqsUsersModel labReqsUsersModel)
        {
            log.LogInformation($"NypUserInfoViewModel Method GetUserInfor for user  {SelUser}");
            if (LabRecsUser != null)
            {
                if (LabRecsUser.TryGetValue(SelUser, out NypLabReqsUsersModel value))
                {
                    labReqsUsersModel = value;
                    log.LogInformation($"NypUserInfoViewModel Method GetUserInfor user {SelUser} found");
                }
                else
                {
                    log.LogWarning($"NypUserInfoViewModel Method GetUserInfor user {SelUser} not found");
                }


            }
            return labReqsUsersModel;
        }
        private async Task GetUserInfor()
        {
            log.LogInformation($"NypUserInfoViewModel Method GetUserInfor user {SelUser}");
            if (LabRecsUser != null)
            {
                if (LabRecsUser.TryGetValue(SelUser, out NypLabReqsUsersModel value))
                {

                    LabReqsUsersModel = value;
                    log.LogInformation($"NypUserInfoViewModel Method GetUserInfor user {SelUser} found");
                }

                else
                    log.LogInformation($"NypUserInfoViewModel Method GetUserInfor user {SelUser} not found");
            }
        }
        public async Task<bool> CheckForChnages(NypLabReqsUsersModel LabReqsUsersModel)
        {
            GetLabRecUsers().ConfigureAwait(true).GetAwaiter().GetResult();
            log.LogInformation($"NypUserInfoViewModel Method CheckForChnages for user {SelUser}");
            if (LabRecsUser != null)
            {

                if (LabRecsUser.TryGetValue(SelUser, out NypLabReqsUsersModel value))
                {
                    if (LabReqsUsersModel.Active != value.Active)
                    {
                        log.LogInformation($"NypUserInfoViewModel Method CheckForChnages Change active from {LabReqsUsersModel.Active.ToString()} to  {value.Active.ToString()} for user {SelUser}");
                        return true;
                    }

                    if (LabReqsUsersModel.IsAdmin != value.IsAdmin)
                    {
                        log.LogInformation($"NypUserInfoViewModel Method CheckForChnages Change admin from {LabReqsUsersModel.IsAdmin.ToString()} to  from {value.IsAdmin.ToString()} for user {SelUser}");
                        return true;
                    }

                    if (LabReqsUsersModel.ViewAuditLogs != value.IsAdmin)
                    {
                        log.LogInformation($"NypUserInfoViewModel Method CheckForChnages Change view audit logs from {LabReqsUsersModel.ViewAuditLogs.ToString()} to  from {value.ViewAuditLogs.ToString()} for user {SelUser}");
                        return true;
                    }

                    if (LabReqsUsersModel.DelUser != value.DelUser)
                    {
                        log.LogInformation($"NypUserInfoViewModel Method CheckForChnages Change delete user {SelUser}");
                        return true;
                    }

                }


            }
            log.LogWarning($"NypUserInfoViewModel Method CheckForChnages for user {SelUser} no changes found");
            return false;
        }
        public async Task<IActionResult> OnPostProfileUpDateAsync(NypLabReqsUsersModel LabReqsUsersModel)
        {
            //if (!(User.Identity.IsAuthenticated))
            //{
            //    return new RedirectToPageResult("/NypUsersInfo/LoginView", "OnGetAsync");
            //}
            ViewData["CWID"] = GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyCwid).ConfigureAwait(true).GetAwaiter().GetResult();
            if (ViewData["CWID"] == null)
                return new RedirectToPageResult("/Index");
           
            try
            {


                LabReqHelpers.StartStopWatch();
                GetLogInformaiton().ConfigureAwait(false).GetAwaiter().GetResult();
                log.LogInformation("Start NypUserInfoViewModel method OnPostProfileUpDateAsync");
                GetViewAudLogsAdmin();
                SelUser = LabReqsUsersModel.Cwid;
                log.LogInformation($"OnPostProfileUpDateAsync selected user {SelUser}");
                bool foundChange = CheckForChnages(LabReqsUsersModel).ConfigureAwait(true).GetAwaiter().GetResult();
                if (foundChange)
                {
                    log.LogInformation($"OnPostProfileUpDateAsync found changes for selected user {SelUser}");
                    if (LabReqsUsersModel.DelUser)
                    {
                        if (string.Compare(LabReqsUsersModel.Cwid, ViewData["CWID"].ToString(), true) == 0)
                        {
                            ModelState.AddModelError("ErrorMessage", $"Cannot delete user logged in with CWID: {SelUser}");
                            log.LogError($"OnPostProfileUpDateAsync Cannot delete user logged in with CWID: {SelUser}");
                            
                        }
                        else
                        {
                            log.LogInformation($"OnPostProfileUpDateAsync Delete usere for CWID: {SelUser}");
                            log.LogInformation($"OnPostProfileUpDateAsync web api controller {WebApiUrl}{ConstNypLabReqs.ApiNypLabReqsUsersController} param {SelUser}");
                            NypLabReqsUserInfo.NypLabReqsUserInfoApiIntance.DelNypUser(new Uri($"{WebApiUrl}{ConstNypLabReqs.ApiNypLabReqsUsersController}"), LabReqsUsersModel.Cwid, log).ConfigureAwait(true).GetAwaiter().GetResult();
                            SelUser = string.Empty;
                            if (LabRecsUser != null)
                                LabRecsUser.Clear();
                            GetSessionVariables.SessionVarInstance.SetSessionOjbjectAsJson(HttpContext.Session, ConstNypLabReqs.SessionLabRecsUsers, LabRecsUser).ConfigureAwait(true).GetAwaiter().GetResult();
                            ModelState.AddModelError("ErrorMessage", $"Cwid: {LabReqsUsersModel.Cwid} deleted");
                            log.LogInformation($"OnPostProfileUpDateAsync return to web page  /NypUsersInfo/NypUserInfoView total time: {LabReqHelpers.StopStopWatch()} ms");
                            return Redirect($"/NypUsersInfo/LoginView?returnUrl=/NypUsersInfo/NypUserInfoView");
                        }
                    }
                    else
                    {
                        if (LabRecsUser != null)
                            LabRecsUser.Clear();
                        log.LogInformation($"OnPostProfileUpDateAsync update user {SelUser} web api controller {WebApiUrl}{ConstNypLabReqs.ApiNypLabReqsUsersController} param {ConstNypLabReqs.NypUserCommandUpDateUser} {SelUser}");
                        NypLabReqsUserInfo.NypLabReqsUserInfoApiIntance.AddUpdateNypUser(new Uri($"{WebApiUrl}{ConstNypLabReqs.ApiNypLabReqsUsersController}"), ConstNypLabReqs.NypUserCommandUpDateUser, log, LabReqsUsersModel).ConfigureAwait(true).GetAwaiter().GetResult();
                        GetSessionVariables.SessionVarInstance.SetSessionOjbjectAsJson(HttpContext.Session, ConstNypLabReqs.SessionLabRecsUsers, LabRecsUser).ConfigureAwait(true).GetAwaiter().GetResult();
                        return Redirect($"/NypUsersInfo/LoginView?returnUrl=/NypUsersInfo/NypUserInfoView?selCwid={SelUser}");
                    }

                }

                else
                {
                    log.LogWarning($"OnPostProfileUpDateAsync Not updating user information for CWID: {SelUser} no changes found");
                    ModelState.AddModelError("ErrorMessage", $"Not updating user information for CWID: {SelUser} no changes found");
                     
                }



                Init().ConfigureAwait(true).GetAwaiter().GetResult();
                log.LogInformation($"End OnPostProfileUpDateAsync total time: {LabReqHelpers.StopStopWatch()} ms");
            }
            catch (Exception ex)
            {
                log.LogError($"End OnPostProfileUpDateAsync total time: {LabReqHelpers.StopStopWatch()} ms {ex.Message}");
                return Redirect($"/NypUsersInfo/DisplayErrorMessages?ErrMEss=OnPostProfileUpDateAsync {ex.Message}");
            }
           
            return Page();

        }
        // public async Task<IActionResult> OnPostAddUserAsync(string NewCwid, bool NewUserViewAuditLogs, bool NewUserIsAdmin)
        public async Task<IActionResult> AddUserAsync(string NewCwid, bool NewUserViewAuditLogs, bool NewUserIsAdmin)
        {
            //if (!(User.Identity.IsAuthenticated))
            //{
            //    return new RedirectToPageResult("/NypUsersInfo/LoginView", "OnGetAsync");
            //}


            try
            {
                log.LogInformation("Start NypUserInfoViewModel method OnPostAddUserAsync");
                Init().ConfigureAwait(true).GetAwaiter().GetResult();
                GetViewAudLogsAdmin();

                if (string.IsNullOrWhiteSpace(NewCwid))
                {
                    ModelState.AddModelError("ErrorMessage", "CWID cannot be empty");
                    ModelState.AddModelError("NewUserBlank", "CWID cannot be empty");
                    log.LogError("NypUserInfoViewModel method OnPostAddUserAsync cwid empty");

                }
                else
                {

                    GetLogInformaiton().ConfigureAwait(true).GetAwaiter().GetResult();
                    SelUser = NewCwid;
                    log.LogInformation($"NypUserInfoViewModel method OnPostAddUserAsync create new user cwid {SelUser}  view audit logs {NewUserViewAuditLogs.ToString()} is admin {NewUserIsAdmin.ToString()}");
                    LabReqsUsersModel = new NypLabReqsUsersModel();
                    LabReqsUsersModel = GetUserInfor(LabReqsUsersModel).ConfigureAwait(true).GetAwaiter().GetResult();
                    if (string.IsNullOrWhiteSpace(LabReqsUsersModel.Cwid))
                    {
                        LabReqsUsersModel.Cwid = NewCwid;
                        LabReqsUsersModel.Active = true;
                        LabReqsUsersModel.EmailAddress = ConstNypLabReqs.NANoSlash;
                        LabReqsUsersModel.FirstName = ConstNypLabReqs.NANoSlash;
                        LabReqsUsersModel.IsAdmin = NewUserIsAdmin;
                        LabReqsUsersModel.LastName = ConstNypLabReqs.NANoSlash;
                        LabReqsUsersModel.ViewAuditLogs = NewUserViewAuditLogs;
                        if (LabRecsUser.Count > 0)
                            LabRecsUser.Clear();
                        log.LogInformation($"NypUserInfoViewModel method OnPostAddUserAsync create new user cwid {SelUser}  view audit logs {NewUserViewAuditLogs.ToString()} is admin {NewUserIsAdmin.ToString()} web api {WebApiUrl}{ConstNypLabReqs.ApiNypLabReqsUsersController} params {ConstNypLabReqs.NypUserCommandAddUser}");
                        NypLabReqsUserInfo.NypLabReqsUserInfoApiIntance.AddUpdateNypUser(new Uri($"{WebApiUrl}{ConstNypLabReqs.ApiNypLabReqsUsersController}"), ConstNypLabReqs.NypUserCommandAddUser, log, LabReqsUsersModel).ConfigureAwait(true).GetAwaiter().GetResult();
                        //GetSessionVariables.SessionVarInstance.GetJsonSessionObject<IDictionary<string, NypLabReqsUsersModel>>(HttpContext.Session, ConstNypLabReqs.SessionLabRecsUsers).ConfigureAwait(true).GetAwaiter().GetResult();
                        GetSessionVariables.SessionVarInstance.SetSessionOjbjectAsJson(HttpContext.Session, ConstNypLabReqs.SessionLabRecsUsers, LabRecsUser).ConfigureAwait(true).GetAwaiter().GetResult();
                    }
                    else
                    {
                        log.LogWarning($"NypUserInfoViewModel method OnPostAddUserAsync CWID {SelUser} all ready exists");
                        ModelState.AddModelError("ErrorMessage", $"CWID {SelUser} all ready exists ");
                        ModelState.AddModelError("NewUserBlank", $"CWID {SelUser} all ready exists ");

                    }
                }

                Init().ConfigureAwait(true).GetAwaiter().GetResult();
                GetUserInfor().ConfigureAwait(true).GetAwaiter().GetResult();
                log.LogInformation($"End NypUserInfoViewModel method OnPostAddUserAsync total time: {LabReqHelpers.StopStopWatch()} ms");
            }
            catch (Exception ex)
            {
                log.LogError($"End NypUserInfoViewModel method OnPostAddUserAsync total time: {LabReqHelpers.StopStopWatch()} ms {ex.Message}");
                return Redirect($"/NypUsersInfo/DisplayErrorMessages?ErrMEss=NypUserInfoViewModel method OnPostAddUserAsync {ex.Message}");
            }

            return Page();
        }
    }
}