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

namespace Edocs.ITS.AppService.Pages.TrackingSystem
{
    public class InventoryTransferViewModel : PageModel
    {
        private readonly IConfiguration configuration;
        private readonly IHttpClientFactory clientFactory;
        public InventoryTransferViewModel(IConfiguration config, IHttpClientFactory factoryClient)
        {
            configuration = config;
            clientFactory = factoryClient;
        }
        [BindProperty]
        public string TrackingID
        { get; set; }

        [DataType(DataType.Date)]
     //   [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [BindProperty]
        public DateTime DateSent
        { get; set; }

        [BindProperty]
        public string NumDocs
        { get; set; }

        [BindProperty]
        public string CboxScanType
        { get; set; }
        [BindProperty]
        public string CboxDM
        { get; set; }
        private Uri WebApiUrl
        { get { return configuration.GetValue<Uri>(EdocsITSConstants.JsonWebApi); } }
        public UserLoginModel UserLogin
        { get; set; }
        public IList<EdocsITCustomerIDNameModel> EdocsITCustomerIDs
        { get; set; }
        public string TransferCustomerName
        { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            if (!EdocsITSHelpers.CheckAuth(HttpContext.Session))
                return Redirect("/LogInOut/LoginView");


            DateSent = DateTime.Now;
            ViewData[GetSessionVariables.SessionKeyUserName] = GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyUserName).ConfigureAwait(true).GetAwaiter().GetResult();
            ViewData[GetSessionVariables.SessionKeyUserName] = EdocsITSHelpers.RepStr(ViewData[GetSessionVariables.SessionKeyUserName].ToString(), EdocsITSHelpers.QUOTE, string.Empty);
            UserLogin = GetSessionVariables.SessionVarInstance.GetJsonSessionObject<UserLoginModel>(HttpContext.Session, EdocsITSConstants.SessionUserInfo).ConfigureAwait(false).GetAwaiter().GetResult();
            if (UserLogin.EdocsCustomerName.ToLower().Contains("e-docs"))
            {
                EdocsITCustomerIDs = EdocsITSApis.GetITSCustomers(clientFactory, WebApiUrl, EdocsITSConstants.EdocsITSCustomersController, EdocsITSConstants.SpGetEdocsITSCustomers).ConfigureAwait(true).GetAwaiter().GetResult();
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(string TrackingID, DateTime DateSent, string NumDocs, string CboxScanType, string CboxDM,string nameCust)
        {
            if (!EdocsITSHelpers.CheckAuth(HttpContext.Session))
                return Redirect("/LogInOut/LoginView");


            ViewData[GetSessionVariables.SessionKeyUserName] = GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyUserName).ConfigureAwait(true).GetAwaiter().GetResult();
            UserLogin = GetSessionVariables.SessionVarInstance.GetJsonSessionObject<UserLoginModel>(HttpContext.Session, EdocsITSConstants.SessionUserInfo).ConfigureAwait(false).GetAwaiter().GetResult();
         
            if (CheckTrackingInfo().ConfigureAwait(false).GetAwaiter().GetResult())
            {
                //    if (UserLogin.EdocsCustomerID//EdocsCustomerName.ToLower().Contains("e-docs"))
                if (UserLogin.EdocsCustomerID==1000)
                {
                    InventoryTransfer(nameCust).ConfigureAwait(false).GetAwaiter().GetResult();
                    TransferCustomerName = nameCust;
                   

                }
                else
                    InventoryTransfer(UserLogin.EdocsCustomerName).ConfigureAwait(false).GetAwaiter().GetResult();
                

                ReSetVar();
            }

            if (UserLogin.EdocsCustomerName.ToLower().Contains("e-docs"))
            {
                   
                EdocsITCustomerIDs = EdocsITSApis.GetITSCustomers(clientFactory, WebApiUrl, EdocsITSConstants.EdocsITSCustomersController, EdocsITSConstants.SpGetEdocsITSCustomers).ConfigureAwait(true).GetAwaiter().GetResult();

            }

            return Page();
        }
        private void ReSetVar()
        {
            TrackingID = string.Empty;
            DateSent = DateTime.Now;
            NumDocs = string.Empty;
        }
        public async Task<bool> CheckTrackingInfo()
        {
            bool retBool = true;
            if ((string.IsNullOrWhiteSpace(TrackingID)) || (TrackingID.Length < 6))
            {
                retBool = false;
                if (string.IsNullOrWhiteSpace(TrackingID))
                    ModelState.AddModelError("IDTracking", "Tracking ID required");
                else
                    ModelState.AddModelError("IDTracking", $"Invalid Tracking ID {TrackingID}");
            }
            if (DateSent.Year < 2000)
            {
                retBool = false;
                ModelState.AddModelError("SentDate", $"Invalid Date Sent {DateSent}");
            }
            if (!(string.IsNullOrWhiteSpace(NumDocs)))
            {
                if (!(int.TryParse(NumDocs, out int results)))
                {
                    retBool = false;
                    ModelState.AddModelError("DocsNum", $"Invalid Number of documents as must be numerica {NumDocs} ");
                }
            }
            else
            {
                retBool = false;
                ModelState.AddModelError("DocsNum", $"Number of documents cannot be blank");
            }

            if (string.Compare(CboxScanType, "blank", true) == 0)
            {
                retBool = false;
                ModelState.AddModelError("ScanType", $"Invalid Scan Type {CboxScanType}");
            }
            if (string.Compare(CboxDM, "blank", true) == 0)
            {
                retBool = false;
                ModelState.AddModelError("DMethod", $"Invalid Delivery method {CboxDM}");
            }


            return retBool;
        }
        public async Task InventoryTransfer(string custName)
        {
            EdocsITSInventoryTransfer inventoryTransfer = new EdocsITSInventoryTransfer();
            inventoryTransfer.DateSent = DateSent;
            inventoryTransfer.DeliveryMethod = CboxDM;
            inventoryTransfer.EdocsCustomerName = custName;
            inventoryTransfer.NumberDocsSent = int.Parse(NumDocs);
            inventoryTransfer.ScanType = CboxScanType;
            inventoryTransfer.TrackingID = TrackingID;
            inventoryTransfer.UserName = UserLogin.UserName;
            EdocsITSApis.InventoryTransfer(clientFactory, WebApiUrl, EdocsITSConstants.EdocsITSInventoryTransferController, inventoryTransfer).ConfigureAwait(true).GetAwaiter().GetResult();
            // EdocsITSApis.AddNewCustomer(clientFactory, WebApiUrl, EdocsITSConstants.EdocsITSCustomersController, EdocsITSCustomersModel).ConfigureAwait(true).GetAwaiter().GetResult();

        }
        private bool CheckAuth()
        {
            if (!(EdocsITSHelpers.UserAuth(HttpContext.Session)))
                return false;
            return true;
        }
    }
}
