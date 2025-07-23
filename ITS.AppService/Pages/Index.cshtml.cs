using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Edocs.ITS.AppService.ApisConst;
using Edocs.ITS.AppService.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace Edocs.ITS.AppService.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<IndexModel> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
private bool Testing
        { get { return configuration.GetValue<bool>(EdocsITSConstants.JsonTesting); } }
        public IndexModel(ILogger<IndexModel> logger, IWebHostEnvironment webHostEnvironment, IConfiguration config)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            configuration = config;
        }
       
        public UserLoginModel UserLogin
        { get; set; }
        public string HostingFolder
        { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            HostingFolder = _webHostEnvironment.WebRootPath;
         //   Authen(string.Empty).ConfigureAwait(false).GetAwaiter().GetResult();
       //     GetSessionVariables.SessionVarInstance.SetSessionOjbjectAsJson(HttpContext.Session, GetSessionVariables.SessionKeyUserName, "dan.roper@edocsusa.com").ConfigureAwait(false).GetAwaiter().GetResult();
          ///  ViewData[GetSessionVariables.SessionKeyUserName] = GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyUserName).ConfigureAwait(true).GetAwaiter().GetResult();
           // ViewData[GetSessionVariables.SessionKeyUserName] = GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, //GetSessionVariables.SessionKeyUserName).ConfigureAwait(true).GetAwaiter().GetResult();
         //   if (!(EdocsITSHelpers.UserAuth(HttpContext.Session)))
           //     return Redirect("/LogInOut/LoginView");
            //   GetSessionVariables.SessionVarInstance.SetSessionOjbjectAsJson(HttpContext.Session, GetSessionVariables.SessionKeyUserName, "dan.roper@edocsusa.com").ConfigureAwait(false).GetAwaiter().GetResult();
            // ViewData[GetSessionVariables.SessionKeyUserName] = GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyUserName).ConfigureAwait(true).GetAwaiter().GetResult();
            //if( string.IsNullOrWhiteSpace(ViewData[GetSessionVariables.SessionKeyUserName].ToString()))
            // if (ViewData[GetSessionVariables.SessionKeyUserName] == null)
            // {
            // return Redirect("/LogInOut/LoginView");
            //  }
            // else
            // {
            if(Testing)
            {
                Authen(string.Empty).ConfigureAwait(false).GetAwaiter().GetResult();
               
            }
            if (!EdocsITSHelpers.CheckAuth(HttpContext.Session))
                return Redirect("/LogInOut/LoginView");

            ViewData[GetSessionVariables.SessionKeyUserName] = GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyUserName).ConfigureAwait(true).GetAwaiter().GetResult();

            ViewData[GetSessionVariables.SessionKeyUserName] = EdocsITSHelpers.RepStr(ViewData[GetSessionVariables.SessionKeyUserName].ToString(), EdocsITSHelpers.QUOTE, string.Empty);
                UserLogin = GetSessionVariables.SessionVarInstance.GetJsonSessionObject<UserLoginModel>(HttpContext.Session, EdocsITSConstants.SessionUserInfo).ConfigureAwait(false).GetAwaiter().GetResult();
                if (string.Compare(ViewData[GetSessionVariables.SessionKeyUserName].ToString(), "Logout", true) == 0)
                    return Redirect("/LogInOut/LoginView");
            //}
            return Page();
        }
        public async Task<bool> Authen(string loginResults)
        {
           // string[] strResults = EdocsITSHelpers.SplitStr(loginResults, ',');
            DateTime lastMlf = DateTime.Now;
            DateTime date = DateTime.Now.AddDays(-5);
            UserLoginModel userLogin = new UserLoginModel();
            userLogin.CellPhoneNumber = "4064908968";
            userLogin.EmailAddress = "dan.roper@edocsusa.com";
            userLogin.IsCustomerAdmin = true;
            userLogin.IsEdocsAdmin = true;
            
            userLogin.LastMFLA = lastMlf.AddDays(5);
            userLogin.Password = string.Empty;
            userLogin.UserName = "dan.roper@edocsusa.com";
            userLogin.EdocsCustomerName = "e-Docs USA INC.";
            userLogin.EdocsCustomerID = 1000;
            GetSessionVariables.SessionVarInstance.SetSessionOjbjectAsJson(HttpContext.Session, EdocsITSConstants.SessionUserInfo, userLogin).ConfigureAwait(false).GetAwaiter().GetResult();
            GetSessionVariables.SessionVarInstance.SetSessionOjbjectAsJson(HttpContext.Session, GetSessionVariables.SessionKeyUserName, "dan.roper@edocsusa.com").ConfigureAwait(false).GetAwaiter().GetResult();
            GetSessionVariables.SessionVarInstance.SetSessionOjbjectAsJson(HttpContext.Session, EdocsITSConstants.IsUserAuth, "True").ConfigureAwait(false).GetAwaiter().GetResult();

            return true;
        }
    }
}
