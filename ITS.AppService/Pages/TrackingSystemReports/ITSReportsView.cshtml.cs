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
using System.ComponentModel.DataAnnotations;

namespace Edocs.ITS.AppService.Pages.TrackingSystemReports
{

    public class ITSReportsViewModel : PageModel
    {
        private readonly IConfiguration configuration;
        private readonly IHttpClientFactory clientFactory;
        private Uri WebApiUrl
        { get { return configuration.GetValue<Uri>(EdocsITSConstants.JsonWebApi); } }
        public UserLoginModel UserLogin
        { get; set; }
        public IDictionary<string, EdocsITSInventoryTransfer> ITSInventoryTransferSent
        { get; set; }
        public IDictionary<string, EdocsITSScanningManModel> ITSScanningManModelRec
        { get; set; }
        [DataType(DataType.Date)]
        //   [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [BindProperty]
        public DateTime RepStartDate
        { get; set; }
        [DataType(DataType.Date)]
        //   [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [BindProperty]
        public DateTime RepEDate
        { get; set; }

        public ITSReportsViewModel(IConfiguration config, IHttpClientFactory factoryClient)
        {
            configuration = config;
            clientFactory = factoryClient;
        }

        public async Task<IActionResult> OnGetAsync(string repType,string repSDate, string repSEndDate)
        {
            if(RepStartDate.Year < 2020)
            {
                RepStartDate = DateTime.Now.AddDays(-30);
                RepEDate = DateTime.Now.AddDays(1);
               // RepSDate = RepEDate.AddDays(-30);
            }
                     
            if (!EdocsITSHelpers.CheckAuth(HttpContext.Session))
                return Redirect("/LogInOut/LoginView");
            ViewData[GetSessionVariables.SessionKeyUserName] = GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyUserName).ConfigureAwait(true).GetAwaiter().GetResult();
            ViewData[GetSessionVariables.SessionKeyUserName] = EdocsITSHelpers.RepStr(ViewData[GetSessionVariables.SessionKeyUserName].ToString(), EdocsITSHelpers.QUOTE, string.Empty);
            UserLogin = GetSessionVariables.SessionVarInstance.GetJsonSessionObject<UserLoginModel>(HttpContext.Session, EdocsITSConstants.SessionUserInfo).ConfigureAwait(false).GetAwaiter().GetResult();
            if (!(string.IsNullOrWhiteSpace(repType)))
            {
                RepStartDate = DateTime.Parse(repSDate);
                RepEDate = DateTime.Parse(repSEndDate);
                if (string.Compare(repType, "docSent",true) == 0)
                    ITSInventoryTransferSent = EdocsITSApis.GetInventorySent(clientFactory, WebApiUrl, EdocsITSConstants.EdocsITSReportsController, repSDate, RepEDate.ToString("MM-dd-yyyy"), EdocsITSConstants.SpEdocsITSGetReports, repType, "Palm Springs Unified School District").ConfigureAwait(false).GetAwaiter().GetResult();
                else
                    ITSScanningManModelRec = EdocsITSApis.GetInventoryRec(clientFactory, WebApiUrl, EdocsITSConstants.EdocsITSReportsController, RepStartDate.ToString("MM-dd-yyyy"), RepEDate.ToString("MM-dd-yyyy"), EdocsITSConstants.SpEdocsITSGetReports, repType, "Palm Springs Unified School District").ConfigureAwait(false).GetAwaiter().GetResult();
            }
            
            return Page();
        }
    }
}
