using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Edocs.ITS.AppService.ApisConst;
using System.Net.Http;
using Edocs.ITS.AppService.Models;
using System.Security.Policy;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;


namespace Edocs.ITS.AppService.Pages.TrackingSystemReports
{
    public class ITSReportByTrackingIDViewModel : PageModel
    {
        private readonly IConfiguration configuration;
        private readonly IHttpClientFactory clientFactory;
        private Uri WebApiUrl
        { get { return configuration.GetValue<Uri>(EdocsITSConstants.JsonWebApi); } }
        public UserLoginModel UserLogin
        { get; set; }
        public ITSReportByTrackingIDViewModel(IConfiguration config, IHttpClientFactory factoryClient)
        {
            configuration = config;
            clientFactory = factoryClient;
        }
        public string PNameTrackingID
        { get; set; }
        public IList<TrackingIDProjectNameModel> TackingIDs
        { get; set; }
        public int CustomerID
        { get; set; }
        public string TrackingID
        { get; set; }
        public async Task<IActionResult> OnGetAsync()

        {
            if (!EdocsITSHelpers.CheckAuth(HttpContext.Session))
                return Redirect("/LogInOut/LoginView");

            ViewData[GetSessionVariables.SessionKeyUserName] = GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyUserName).ConfigureAwait(true).GetAwaiter().GetResult();
            ViewData[GetSessionVariables.SessionKeyUserName] = EdocsITSHelpers.RepStr(ViewData[GetSessionVariables.SessionKeyUserName].ToString(), EdocsITSHelpers.QUOTE, string.Empty);
            UserLogin = GetSessionVariables.SessionVarInstance.GetJsonSessionObject<UserLoginModel>(HttpContext.Session, EdocsITSConstants.SessionUserInfo).ConfigureAwait(false).GetAwaiter().GetResult();
           
            //TackingIDs = EdocsITSApis.GetTrackingIDsProjectName(clientFactory, WebApiUrl, EdocsITSConstants.TransferByTrackIDController, "1001").ConfigureAwait(false).GetAwaiter().GetResult();
          //  GetSessionVariables.SessionVarInstance.SetSessionOjbjectAsJson(HttpContext.Session, "TrackingIDsPNames", TrackingID).ConfigureAwait(false).GetAwaiter().GetResult();
            return Page();
        }

    }
}
