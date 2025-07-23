using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Edocs.ITS.AppService.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Configuration;
  using System.Net.Http;  
using Edocs.ITS.AppService.ApisConst;
using Edocs.ITS.AppService.Interfaces;

namespace Edocs.ITS.AppService.Pages.LogInOut
{
    public class AuthenticationUserViewModel : PageModel
    {
         
        private readonly IConfiguration configuration;
        private readonly IEmailSettings emailSettings;
        private readonly IHttpContextAccessor httpContextAccessor;
        private Uri WebApiUrl
        { get { return configuration.GetValue<Uri>(EdocsITSConstants.JsonWebApi); } }

        public AuthenticationUserViewModel(IConfiguration config, IEmailSettings settings,  IHttpContextAccessor contextAccessorHttp)
        {
            configuration = config;
            
            httpContextAccessor = contextAccessorHttp;
            emailSettings = settings;
            //   StudentRecordModel = new StudentRecordModel();
        }
        public UserLoginModel UserLoginModel
        { get; set; }
        public string LoginTitle
        { get; set; }
        [Display(Name = "Enter Authentication Code:")]
        [BindProperty]
        public string AuthCode { get; set; }

        [BindProperty]
        public string AuthMethod { get; set; }
        [BindProperty]
                
        public string DisplayPage
        { get; set; }
        
        public string AudCodeGen
        { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            //ViewData[GetSessionVariables.SessionKeyUserName] = GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyUserName).ConfigureAwait(true).GetAwaiter().GetResult();
           // ViewData[GetSessionVariables.SessionKeyUserName] = PSEHelpers.RepStr(ViewData[GetSessionVariables.SessionKeyUserName].ToString(), PSEHelpers.QUOTE, string.Empty);
             UserLoginModel = GetSessionVariables.SessionVarInstance.GetJsonSessionObject<UserLoginModel>(HttpContext.Session, EdocsITSConstants.SessionUserInfo).ConfigureAwait(true).GetAwaiter().GetResult();
            LoginTitle = $"Authentication User: {UserLoginModel.UserName}";
            //  int tempStr = UserLoginModel.EmailAddress.IndexOf("@");
            // UserLoginModel.EmailAddress = $"Email code to ***{UserLoginModel.EmailAddress.Substring(tempStr)}";
            //    tempStr = UserLoginModel.CellPhoneNumber.LastIndexOf("-") + 3;
            // UserLoginModel.CellPhoneNumber = $"Text code to ***-***-**{UserLoginModel.CellPhoneNumber.Substring(tempStr)}";
            //int authCode = PSEHelpers.GenerateRandomNumber(100000, 999999).ConfigureAwait(false).GetAwaiter().GetResult();
            // AuthCode = System.Text.ASCIIEncoding.ASCII.GetBytes(authCode.ToString());
            int tempStr = UserLoginModel.EmailAddress.IndexOf("@");
            UserLoginModel.EmailAddress = $"Email code to ***{UserLoginModel.EmailAddress.Substring(tempStr)}";
            tempStr = UserLoginModel.CellPhoneNumber.LastIndexOf("-") + 3;
            UserLoginModel.CellPhoneNumber = $"Text code to ***-***-**{UserLoginModel.CellPhoneNumber.Substring(8,2)}";
            AudCodeGen = "N/A";
             DisplayPage = "SendAuthCode";


            return Page();
        }
        public async Task<IActionResult> OnPostAsync(string AuthMethod, string displayPage,string AuthCode, string GenAudCode)
        {
            if ((string.IsNullOrWhiteSpace(AuthMethod) && string.Compare(displayPage, EdocsITSConstants.DisplayPageSendAuthCode, true) == 0))
                return Redirect("/LogInOut/LoginView");

         
            UserLoginModel = GetSessionVariables.SessionVarInstance.GetJsonSessionObject<UserLoginModel>(HttpContext.Session, EdocsITSConstants.SessionUserInfo).ConfigureAwait(true).GetAwaiter().GetResult();
            
            int authCode = 0;
            if (string.Compare(displayPage, EdocsITSConstants.DisplayPageSendAuthCode, true) == 0)
            {
                authCode = EdocsITSHelpers.GenerateRandomNumber(100000, 999999).ConfigureAwait(false).GetAwaiter().GetResult();
               
                Cookies.CookiesInstance.SetCookie(EdocsITSConstants.CookieAuthCode, authCode.ToString(), 5, httpContextAccessor).ConfigureAwait(false).GetAwaiter().GetResult();
                
                EmailService emailService = new EmailService(emailSettings);
                //  UserLoginModel.CellPhoneNumber = "406-490-3732";
                // UserLoginModel.UserName = "tressa.orizotti@edocsusa.com";
                // string codeAuth = System.Text.ASCIIEncoding.ASCII.GetString(authCode);
                
                if (string.Compare(AuthMethod, "CellPhone", true) == 0)
                {
                   // UserLoginModel.CellPhoneNumber = $"{UserLoginModel.CellPhoneNumber}@vtext.com";
                    emailService.SendText($"Authentication Code:{authCode} to access e-Docs USA Inventory Tracking System web application for login id:{UserLoginModel.UserName}", string.Empty, UserLoginModel.CellPhoneNumber, string.Empty);
                }
                else
                {
                    emailService.SendEmail($"Authentication Code:{authCode} to access e-Docs USA Inventory Tracking System web application for login id:{UserLoginModel.UserName}",$"Authentication Code Sent Time {DateTime.Now.ToShortDateString()}" , UserLoginModel.EmailAddress, string.Empty);
                }
            }
            else
            {
                
                string audCode = Request.Form["audCode"].ToString(); 
              //  string authStrCode = Cookies.CookiesInstance.GetCookie(PSEConstants.CookieAuthCode, httpContextAccessor).ConfigureAwait(false).GetAwaiter().GetResult();
                if (string.Compare(AuthCode, audCode, false) == 0)
                {
                    GetSessionVariables.SessionVarInstance.SetSessionOjbjectAsJson(HttpContext.Session, GetSessionVariables.SessionKeyUserName, UserLoginModel.UserName).ConfigureAwait(false).GetAwaiter().GetResult();
                    ViewData[GetSessionVariables.SessionKeyUserName] = GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyUserName).ConfigureAwait(true).GetAwaiter().GetResult();
                    ViewData[GetSessionVariables.SessionKeyUserName] = EdocsITSHelpers.RepStr(ViewData[GetSessionVariables.SessionKeyUserName].ToString(), EdocsITSHelpers.QUOTE, string.Empty);
                    Uri webUri = new Uri($"{WebApiUrl}{EdocsITSConstants.UserLoginController}", UriKind.RelativeOrAbsolute);
                    EdocsITSUsersApi.UsersInstance.UpDateNextMFLA(EdocsITSConstants.SpUpDateLastMFLA, UserLoginModel.UserName, webUri, 5).ConfigureAwait(false).GetAwaiter().GetResult();
                    GetSessionVariables.SessionVarInstance.SetSessionOjbjectAsJson(HttpContext.Session, EdocsITSConstants.IsUserAuth, "True").ConfigureAwait(false).GetAwaiter().GetResult();
                    return Redirect("/Index");
                }
                else
                {
                    ModelState.AddModelError("InvalidAuthCode", $"Invalid Authentication Code: {AuthCode}");
                }
            }
            LoginTitle = $"Validate Authentication Code for User: {UserLoginModel.UserName}";
            this.DisplayPage = "ValidateAuthCode";
           this.AudCodeGen = authCode.ToString();
            
            return Page();
        }

    }
}
