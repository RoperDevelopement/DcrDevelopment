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

namespace Edocs.ITS.AppService.Pages.Partial_Pages
{
    public class DisplayUsersPartialViewModel : PageModel
    {
        private readonly IConfiguration configuration;
        private readonly IHttpClientFactory clientFactory;
        private Uri WebApiUrl
        { get { return configuration.GetValue<Uri>(EdocsITSConstants.JsonWebApi); } }


        public DisplayUsersPartialViewModel(IConfiguration config, IHttpClientFactory factoryClient)
        {
            configuration = config;
            clientFactory = factoryClient;



        }
        public UserLoginModel UserLogin
        { get; set; }
        public IDictionary<int, EdocsITSUsersModel> ListUserModel
        { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            UserLogin = GetSessionVariables.SessionVarInstance.GetJsonSessionObject<UserLoginModel>(HttpContext.Session, EdocsITSConstants.SessionUserInfo).ConfigureAwait(false).GetAwaiter().GetResult();
            if (UserLogin.IsEdocsAdmin)
                ListUserModel = EdocsITSUsersApi.GetAllUserNames(clientFactory, WebApiUrl, EdocsITSConstants.EdocsITSUsersController, EdocsITSConstants.SpEdocsITSGetUsersInformation, EdocsITSConstants.Edocs.ToLower()).ConfigureAwait(true).GetAwaiter().GetResult();
            else
                ListUserModel = EdocsITSUsersApi.GetAllUserNames(clientFactory, WebApiUrl, EdocsITSConstants.EdocsITSUsersController, EdocsITSConstants.SpEdocsITSGetUsersInformation, UserLogin.EdocsCustomerName).ConfigureAwait(true).GetAwaiter().GetResult();
            //ListUserModel = new Dictionary<int, EdocsITSUsersModel>();
            //EdocsITSUsersModel s = new EdocsITSUsersModel();
            //s.IsCustomerAdmin = true;
            //s.CellPhoneNumber = "4444";
            //s.DateLastLogin = DateTime.Now;
            //s.DatePasswordLastChanged = DateTime.Now;
            //s.EdocsCustomerName = "edocs";
            //s.EmailAddress = "dd@ff";
            //s.FirstName = "d";
            //s.IsEdocsAdmin = false;
            //s.IsUserActive = true;
            //s.LastName = "fff";
            //s.UserID = 1;
            //s.UserName = "lll";
            //ListUserModel.Add(1, s);
            
            //s.IsCustomerAdmin = true;
            //s.CellPhoneNumber = "4444";
            //s.DateLastLogin = DateTime.Now;
            //s.DatePasswordLastChanged = DateTime.Now;
            //s.EdocsCustomerName = "edocs";
            //s.EmailAddress = "dd@ff";
            //s.FirstName = "d";
            //s.IsEdocsAdmin = false;
            //s.IsUserActive = true;
            //s.LastName = "fff";
            //s.UserID = 1;
            //s.UserName = "lll";
            //ListUserModel.Add(2, s);
            
            //s.IsCustomerAdmin = true;
            //s.CellPhoneNumber = "4444";
            //s.DateLastLogin = DateTime.Now;
            //s.DatePasswordLastChanged = DateTime.Now;
            //s.EdocsCustomerName = "edocs";
            //s.EmailAddress = "dd@ff";
            //s.FirstName = "d";
            //s.IsEdocsAdmin = false;
            //s.IsUserActive = true;
            //s.LastName = "fff";
            //s.UserID = 1;
            //s.UserName = "lll";
            //ListUserModel.Add(3, s);
            
            //s.IsCustomerAdmin = true;
            //s.CellPhoneNumber = "4444";
            //s.DateLastLogin = DateTime.Now;
            //s.DatePasswordLastChanged = DateTime.Now;
            //s.EdocsCustomerName = "edocs";
            //s.EmailAddress = "dd@ff";
            //s.FirstName = "d";
            //s.IsEdocsAdmin = false;
            //s.IsUserActive = true;
            //s.LastName = "fff";
            //s.UserID = 1;
            //s.UserName = "lll";
            //ListUserModel.Add(4, s);
            
            //s.IsCustomerAdmin = true;
            //s.CellPhoneNumber = "4444";
            //s.DateLastLogin = DateTime.Now;
            //s.DatePasswordLastChanged = DateTime.Now;
            //s.EdocsCustomerName = "edocs";
            //s.EmailAddress = "dd@ff";
            //s.FirstName = "d";
            //s.IsEdocsAdmin = false;
            //s.IsUserActive = true;
            //s.LastName = "fff";
            //s.UserID = 1;
            //s.UserName = "lll";
            //ListUserModel.Add(5, s);
             
            //s.IsCustomerAdmin = true;
            //s.CellPhoneNumber = "4444";
            //s.DateLastLogin = DateTime.Now;
            //s.DatePasswordLastChanged = DateTime.Now;
            //s.EdocsCustomerName = "edocs";
            //s.EmailAddress = "dd@ff";
            //s.FirstName = "d";
            //s.IsEdocsAdmin = false;
            //s.IsUserActive = true;
            //s.LastName = "fff";
            //s.UserID = 1;
            //s.UserName = "lll";
            //ListUserModel.Add(6, s);
            //for(int i=7; i<100; i++)
            //    ListUserModel.Add(i, s);
            return Page();
        }
    }
}
