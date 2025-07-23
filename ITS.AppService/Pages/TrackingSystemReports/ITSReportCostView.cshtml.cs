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
    public class ITSReportCostViewModel : PageModel
    {
        private readonly IConfiguration configuration;
        private readonly IHttpClientFactory clientFactory;
        private Uri WebApiUrl
        { get { return configuration.GetValue<Uri>(EdocsITSConstants.JsonWebApi); } }
        public UserLoginModel UserLogin
        { get; set; }
        public EdocsITSCustomersModel EdocsITSCustomers
        { get; set; }
        [Display(Name = "Report Start Date")]
        [DataType(DataType.Date)]
        public DateTime RepStartDate
        { get; set; }
        [BindProperty]
        [DataType(DataType.Date)]
        [Display(Name = "Report End Date")]

      
        public DateTime RepEndDate
        { get; set; }
        public string CustomerID
        { get; set; }
        [BindProperty]
        public IList<EdocsITCustomerIDNameModel> EdocsITCustomerIDs
        { get; set; }
        public ITSReportCostViewModel(IConfiguration config, IHttpClientFactory factoryClient)
        {
            configuration = config;
            clientFactory = factoryClient;
        }

        public async Task<IActionResult> OnGetAsync(string repSDate, string repSEndDate, string cusID )
        {
            if (!EdocsITSHelpers.CheckAuth(HttpContext.Session))
                return Redirect("/LogInOut/LoginView");
            ViewData[GetSessionVariables.SessionKeyUserName] = GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyUserName).ConfigureAwait(true).GetAwaiter().GetResult();
            UserLogin = GetSessionVariables.SessionVarInstance.GetJsonSessionObject<UserLoginModel>(HttpContext.Session, EdocsITSConstants.SessionUserInfo).ConfigureAwait(false).GetAwaiter().GetResult();
            ViewData[GetSessionVariables.SessionKeyUserName] = EdocsITSHelpers.RepStr(ViewData[GetSessionVariables.SessionKeyUserName].ToString(), EdocsITSHelpers.QUOTE, string.Empty);
            if ((string.IsNullOrWhiteSpace(cusID)) || (string.Compare(cusID, "Customer Name", true) == 0))
            {
                EdocsITCustomerIDs = EdocsITSApis.GetITSCustomers(clientFactory, WebApiUrl, EdocsITSConstants.EdocsITSCustomersController, EdocsITSConstants.SpGetEdocsITSCustomers).ConfigureAwait(true).GetAwaiter().GetResult();
            }
            else
            {
                IList<EdocsITSMinMaxDateModel> minMaxDate = EdocsITSApis.GetMaxMinDate(clientFactory, WebApiUrl, EdocsITSConstants.EdocsITSReportsController, cusID).ConfigureAwait(false).GetAwaiter().GetResult();
                if((minMaxDate != null) && (minMaxDate.Count()>0))
                {
                    RepStartDate = minMaxDate[0].MinDate;
                    RepEndDate = minMaxDate[0].MaxDate.AddDays(1);
                }
                else
                {
                    EdocsITCustomerIDs = EdocsITSApis.GetITSCustomers(clientFactory, WebApiUrl, EdocsITSConstants.EdocsITSCustomersController, EdocsITSConstants.SpGetEdocsITSCustomers).ConfigureAwait(true).GetAwaiter().GetResult();
                    RepStartDate = DateTime.Now.AddDays(-10);
                    RepEndDate = DateTime.Now;
                    ModelState.AddModelError("ErrorMessage", $"*No Records Found For Customer ID {cusID}");
                    cusID = string.Empty;
                }
            }
            CustomerID = cusID;
            return Page();
        }
        private bool CheckAuth()
        {
            if (!(EdocsITSHelpers.UserAuth(HttpContext.Session)))
                return false;
            return true;
        }
    }
}
