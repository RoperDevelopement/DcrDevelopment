using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Microsoft.Extensions.Configuration;
using Edocs.ITS.AppService.ApisConst;
using Edocs.ITS.AppService.Interfaces;
using Edocs.ITS.AppService.Models;
namespace Edocs.ITS.AppService.Pages.LogInOut
{
    public class LoginViewModel : PageModel
    {
        private readonly IConfiguration configuration;
        
        private string WebApiUrl
        { get { return configuration.GetValue<string>(EdocsITSConstants.JsonWebApi).ToString(); } }
        public LoginViewModel(IConfiguration config)
        {
            configuration = config;

            //   StudentRecordModel = new StudentRecordModel();
        }
        [BindProperty]
        public LoginModel Login
        { get; set; }

        public string LoginView
        { get; set; }
        public string LoginTitle
        { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            LoginView = "Login";
            LoginTitle = "Login e-Docs USA Inventory Tracking System";

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(LoginModel Login, string loginuser)
        {
            //GetSessionVariables.SessionVarInstance.SetSessionOjbjectAsJson(HttpContext.Session, GetSessionVariables.SessionKeyUserName, Login.UserName).ConfigureAwait(false).GetAwaiter().GetResult();
            //return Redirect("/Index");
            LoginView = "Login";
            LoginTitle = "Login e-Docs USA Inventory Tracking System";
            if((string.IsNullOrWhiteSpace(Login.UserName)) || (string.IsNullOrWhiteSpace(Login.Password)))
            {
                if ((string.IsNullOrWhiteSpace(Login.UserName)))
                    ModelState.AddModelError("InvalidUserName", "Invalid user name cannot be empty");
                else
                    ModelState.AddModelError("InvalidPassWord", "Invalid password cannot be empty");

            }
            else
            { 
            Uri webUri = new Uri($"{WebApiUrl}", UriKind.RelativeOrAbsolute);
            string loginResults = EdocsITSUsersApi.UsersInstance.LoginUser(Login, EdocsITSConstants.UserLoginController, webUri, EdocsITSConstants.SpUserLogIn).ConfigureAwait(false).GetAwaiter().GetResult();
            //if (!(string.IsNullOrWhiteSpace(Login.Password)) && (!(string.IsNullOrWhiteSpace(Login.Password))))
            //{
            //    if ((string.Compare(Login.UserName.Trim(), "dan.roper@edocsusa.com", true) == 0) || (string.Compare(Login.UserName.Trim(), "tressa.orizotti@edocsusa.com", true) == 0))
            //    {
            //        GetSessionVariables.SessionVarInstance.SetSessionOjbjectAsJson(HttpContext.Session, GetSessionVariables.SessionKeyUserName, Login.UserName).ConfigureAwait(false).GetAwaiter().GetResult();
            //        ViewData[GetSessionVariables.SessionKeyUserName] = GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyUserName).ConfigureAwait(true).GetAwaiter().GetResult();
            //        return Redirect("/Index");
            //    }
            //    else
            //        ModelState.AddModelError("InvalidUserName", $"Invalid user name {Login.UserName}");
            //}

            if (loginResults.ToLower().StartsWith(EdocsITSConstants.SqlErrorStartsWith))
            {
                if (loginResults.ToLower().StartsWith(EdocsITSConstants.SqlErrorUserNotFound))
                    ModelState.AddModelError("InvalidUserName", $"Invalid user name {Login.UserName}");
                else if (loginResults.ToLower().StartsWith(EdocsITSConstants.SqlErrorPW))
                    ModelState.AddModelError("InvalidPassWord", $"Invalid password");
                else
                    ModelState.AddModelError("InvalidUserName", $"Invalid user name {loginResults}");

            }
            else
            {
               
                if (Authen(loginResults).ConfigureAwait(false).GetAwaiter().GetResult())
                {
                        GetSessionVariables.SessionVarInstance.SetSessionOjbjectAsJson(HttpContext.Session, GetSessionVariables.SessionKeyUserName, Login.UserName).ConfigureAwait(false).GetAwaiter().GetResult();
                        ViewData[GetSessionVariables.SessionKeyUserName] = GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyUserName).ConfigureAwait(true).GetAwaiter().GetResult();
                        return Redirect("/Index");
                }
                else
                    {
                        GetSessionVariables.SessionVarInstance.SetSessionOjbjectAsJson(HttpContext.Session, GetSessionVariables.SessionKeyUserName, "Logout").ConfigureAwait(false).GetAwaiter().GetResult();
                        return Redirect("/LogInOut/AuthenticationUserView");
                    }
                    
            }

            }

            return Page();
        }
        public async Task<bool> Authen(string loginResults)
        {
            string[] strResults = EdocsITSHelpers.SplitStr(loginResults,',');
            DateTime lastMlf = EdocsITSHelpers.StrToDate(strResults[0]);
            DateTime date = DateTime.Now.AddDays(-5);
            UserLoginModel userLogin = new UserLoginModel();
            userLogin.CellPhoneNumber = strResults[4];
            userLogin.EmailAddress = strResults[3];
            userLogin.IsCustomerAdmin = false;
            userLogin.IsEdocsAdmin = false;
            if (strResults[1] == "1")
                userLogin.IsCustomerAdmin = true;
            if (strResults[2] == "1")
                userLogin.IsEdocsAdmin = true;
            userLogin.LastMFLA = lastMlf.AddDays(5);
            userLogin.Password = string.Empty;
            userLogin.UserName = strResults[3];
            userLogin.EdocsCustomerName = strResults[6];
            userLogin.EdocsCustomerID = int.Parse(strResults[7]);
            GetSessionVariables.SessionVarInstance.SetSessionOjbjectAsJson(HttpContext.Session,EdocsITSConstants.SessionUserInfo ,userLogin ).ConfigureAwait(false).GetAwaiter().GetResult();
          //  if ((date.Date >= lastMlf.Date) || (lastMlf.Date == DateTime.Now.Date))
            //    return false;
            GetSessionVariables.SessionVarInstance.SetSessionOjbjectAsJson(HttpContext.Session, EdocsITSConstants.IsUserAuth,"True").ConfigureAwait(false).GetAwaiter().GetResult();
            
            return true;
        }
        public async Task<IActionResult> OnPostLogoutAsync()
        {

            GetSessionVariables.SessionVarInstance.SetSessionOjbjectAsJson(HttpContext.Session, GetSessionVariables.SessionKeyUserName, "Logout").ConfigureAwait(false).GetAwaiter().GetResult();
            ViewData[GetSessionVariables.SessionKeyUserName] = GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyUserName).ConfigureAwait(true).GetAwaiter().GetResult();
            HttpContext.Session.Clear();
            

            return Redirect("/Index");
        }
    }
}
