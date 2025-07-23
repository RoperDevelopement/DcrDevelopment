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
namespace Edocs.ITS.AppService.Pages.EdocsITSCustomers
{
    public class EdocsITSEditCustomersView : PageModel
    {
        private readonly IConfiguration configuration;
        private readonly IHttpClientFactory clientFactory;
        public EdocsITSEditCustomersView(IConfiguration config, IHttpClientFactory factoryClient)
        {
            configuration = config;
            clientFactory = factoryClient;
        }
        private Uri WebApiUrl
        { get { return configuration.GetValue<Uri>(EdocsITSConstants.JsonWebApi); } }
        [BindProperty]
        public string CustomerState
        { get; set; }
        [BindProperty]
        public EdocsITSCustomersModel EdocsITSCustomers
        { get; set; }
        public IList<string> StateAbb
        { get; set; }
        [BindProperty]
        public IList<EdocsITCustomerIDNameModel> EdocsITCustomerIDs
        { get; set; }
        [BindProperty]
        public string CustomerID
        { get; set; }
        public UserLoginModel UserLogin
        { get; set; }
        [BindProperty]
        public bool DeactiveCustomer
        { get; set; }
        [BindProperty]
        public string StateCode
        { get; set; }
        public async Task<IActionResult> OnGetAsync(string CustomerID)
        {
            if (!EdocsITSHelpers.CheckAuth(HttpContext.Session))
                return Redirect("/LogInOut/LoginView");

            ViewData[GetSessionVariables.SessionKeyUserName] = GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyUserName).ConfigureAwait(true).GetAwaiter().GetResult();
            UserLogin = GetSessionVariables.SessionVarInstance.GetJsonSessionObject<UserLoginModel>(HttpContext.Session, EdocsITSConstants.SessionUserInfo).ConfigureAwait(false).GetAwaiter().GetResult();
            ViewData[GetSessionVariables.SessionKeyUserName] = EdocsITSHelpers.RepStr(ViewData[GetSessionVariables.SessionKeyUserName].ToString(), EdocsITSHelpers.QUOTE, string.Empty);
            EdocsITSCustomers = new EdocsITSCustomersModel();
            //EdocsITSCustomers.EdocsCustomerName = "fffff";
            //EdocsITSCustomers.EdosCustomerState = "MT";
            StateAbb = EdocsITSUtilites.GetStatesABB().ConfigureAwait(false).GetAwaiter().GetResult();
     
            if ((string.IsNullOrWhiteSpace(CustomerID)) || (string.Compare(CustomerID, "Customer Name", true) == 0))

                EdocsITCustomerIDs = EdocsITSApis.GetITSCustomers(clientFactory, WebApiUrl, EdocsITSConstants.EdocsITSCustomersController, EdocsITSConstants.SpGetEdocsITSCustomers).ConfigureAwait(true).GetAwaiter().GetResult();

            else
            {
                EdocsITSCustomers = EdocsITSApis.GetEdocsCustomer(clientFactory, WebApiUrl, EdocsITSConstants.EdocsITSCustomersController, CustomerID, EdocsITSConstants.SpGetEdocsITSCustomerInfo).ConfigureAwait(false).GetAwaiter().GetResult();
                if(!(string.IsNullOrWhiteSpace(EdocsITSCustomers.EdosCustomerState)))
                StateCode = EdocsITSCustomers.EdosCustomerState;
                //if (EdocsITSCustomers.Active)
                //    EdocsITSCustomers.Active = false;
                //else
                //    EdocsITSCustomers.Active = true;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(EdocsITSCustomersModel EdocsITSCustomers, string StateCode)
        {
            if (!CheckAuth())
                return Redirect("/LogInOut/LoginView");
            string delC = Request.Form["custdel"].ToString();
            ViewData[GetSessionVariables.SessionKeyUserName] = GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyUserName).ConfigureAwait(true).GetAwaiter().GetResult();
            UserLogin = GetSessionVariables.SessionVarInstance.GetJsonSessionObject<UserLoginModel>(HttpContext.Session, EdocsITSConstants.SessionUserInfo).ConfigureAwait(false).GetAwaiter().GetResult();
            ViewData[GetSessionVariables.SessionKeyUserName] = EdocsITSHelpers.RepStr(ViewData[GetSessionVariables.SessionKeyUserName].ToString(), EdocsITSHelpers.QUOTE, string.Empty);
            if (string.IsNullOrWhiteSpace(delC))
            {
                EdocsITSCustomers.EdocsCustomerModifyBy = UserLogin.UserName;
                EdocsITSCustomers.EdosCustomerState = StateCode;
                EdocsITSApis.AddNewCustomer(clientFactory, WebApiUrl, EdocsITSConstants.EdocsITSCustomersController, EdocsITSCustomers).ConfigureAwait(true).GetAwaiter().GetResult();
            }
            else
            {
                return Redirect("/EdocsITSCustomers/EdocsITSEditCustomersView");
            }
            StateAbb = EdocsITSUtilites.GetStatesABB().ConfigureAwait(false).GetAwaiter().GetResult();
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
