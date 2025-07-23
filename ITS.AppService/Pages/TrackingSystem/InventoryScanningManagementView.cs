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

namespace Edocs.ITS.AppService.Pages.TrackingSystem
{
    public class InventoryScanningManagementView : PageModel
    {
        private readonly IConfiguration configuration;
        private readonly IHttpClientFactory clientFactory;
        public InventoryScanningManagementView(IConfiguration config, IHttpClientFactory factoryClient)
        {
            configuration = config;
            clientFactory = factoryClient;
        }
        [BindProperty]
        public string TrackingID
        { get; set; }

        //[BindProperty]
        //public string LookUpTrackingID
        //{ get; set; }

        [BindProperty]
       public EdocsITSScanningManModel ScanningModel
        {
            get;set;
        }

        //[BindProperty]
        //public string NumDocs
        //{ get; set; }

        //[BindProperty]
        //public string CboxScanType
        //{ get; set; }
        //[BindProperty]
        //public string CboxDM
        //{ get; set; }
        private Uri WebApiUrl
        { get { return configuration.GetValue<Uri>(EdocsITSConstants.JsonWebApi); } }
        public UserLoginModel UserLogin
        { get; set; }
        public string Header
        { get; set; }
        public bool NofiyUser
        { get; set; }
        public DateTime DocumentsReceived
        { get; set; }
        public DateTime ScanningStarted
        { get; set; }
        public async Task<IActionResult> OnGetAsync(string TrackingID)
        {
            if (!EdocsITSHelpers.CheckAuth(HttpContext.Session))
                return Redirect("/LogInOut/LoginView");


            ScanningModel = new EdocsITSScanningManModel();
            ViewData[GetSessionVariables.SessionKeyUserName] = GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyUserName).ConfigureAwait(true).GetAwaiter().GetResult();
            ViewData[GetSessionVariables.SessionKeyUserName] = EdocsITSHelpers.RepStr(ViewData[GetSessionVariables.SessionKeyUserName].ToString(), EdocsITSHelpers.QUOTE, string.Empty);
            UserLogin = GetSessionVariables.SessionVarInstance.GetJsonSessionObject<UserLoginModel>(HttpContext.Session, EdocsITSConstants.SessionUserInfo).ConfigureAwait(false).GetAwaiter().GetResult();
            //  Header = $"Scanning Management for Company {UserLogin.EdocsCustomerName}";
            Header = $"Inventory Lookup";
            if (!(string.IsNullOrWhiteSpace(TrackingID)))
            {
                
                ScanningModel = EdocsITSApis.GetTaskID(clientFactory, WebApiUrl, EdocsITSConstants.EdocsITSScanningManController, TrackingID, UserLogin.EdocsCustomerName).ConfigureAwait(false).GetAwaiter().GetResult();
                if(ScanningModel == null)
                {
                    ModelState.AddModelError("TrackingIDNF", $"Tracking ID {TrackingID} not found");
                    ScanningModel = new EdocsITSScanningManModel();
                }
              
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(bool NofiyUser, EdocsITSScanningManModel ScanningModel, DateTime DocumentsReceived, DateTime ScanningStarted)
        {
            if (!EdocsITSHelpers.CheckAuth(HttpContext.Session))
                return Redirect("/LogInOut/LoginView");


            ViewData[GetSessionVariables.SessionKeyUserName] = GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyUserName).ConfigureAwait(true).GetAwaiter().GetResult();
            UserLogin = GetSessionVariables.SessionVarInstance.GetJsonSessionObject<UserLoginModel>(HttpContext.Session, EdocsITSConstants.SessionUserInfo).ConfigureAwait(false).GetAwaiter().GetResult();
            //if (CheckTrackingInfo().ConfigureAwait(false).GetAwaiter().GetResult())
            //{ 
            //    InventoryTransfer().ConfigureAwait(false).GetAwaiter().GetResult();
            //    ReSetVar();
            //}
            ScanningModel.UserName = UserLogin.UserName;
            if(ScanningStarted.Year > 1900)
                ScanningModel.DateScanningStarted = ScanningStarted;
            if(DocumentsReceived.Year > 1900)
            ScanningModel.DateDocumentsReceived = DocumentsReceived;
            if (CheckUpdateTrakingID(ScanningModel).ConfigureAwait(true).GetAwaiter().GetResult())
            { 
                EdocsITSApis.UpdateTaskID(clientFactory, WebApiUrl, EdocsITSConstants.EdocsITSScanningManController,ScanningModel).ConfigureAwait(true).GetAwaiter().GetResult();
            return Redirect("/TrackingSystem/InventoryScanningManagementView");
            }
            ViewData[GetSessionVariables.SessionKeyUserName] = EdocsITSHelpers.RepStr(ViewData[GetSessionVariables.SessionKeyUserName].ToString(), EdocsITSHelpers.QUOTE, string.Empty);
            return Page();
        }
        private async Task<bool> CheckUpdateTrakingID(EdocsITSScanningManModel ScanningModel)

        {
            bool retBol = true;
            if(ScanningModel.NumberDocumentsReceived == 0)
            {
                ModelState.AddModelError("NDR", $"Number Documents in Box: {ScanningModel.NumberDocumentsReceived} invalid");
                retBol = false;
            }
            if((ScanningModel.DateDocumentsReceived.Year == 1900) && (ScanningModel.DateScanningStarted.Year == 1900))
            {
                ModelState.AddModelError("DDR", $"Documents Received Date: {ScanningModel.DateDocumentsReceived} invalid");
                ModelState.AddModelError("NDR", $"Scanning Started: Date: {ScanningModel.DateScanningStarted} invalid");
                retBol = false;
            }
            else
            {
                if ((ScanningModel.DateDocumentsReceived.Year != 1900) && (ScanningModel.DateScanningStarted.Year != 1900))
                    if(ScanningModel.DateDocumentsReceived > ScanningModel.DateScanningStarted)
                    { 
                        ModelState.AddModelError("DDR", $"Documents Received Date: {ScanningModel.DateDocumentsReceived} cannot be greater then Scanning Started: Date: {ScanningModel.DateScanningStarted}");
                        retBol = false;
                    }
            }
            return retBol;
             
            
        }
        //public async Task<bool> CheckTrackingInfo()
        //{
        //    bool retBool = true;
        //    if ((string.IsNullOrWhiteSpace(TrackingID)) || (TrackingID.Length < 6))
        //    {
        //        retBool = false;
        //        if (string.IsNullOrWhiteSpace(TrackingID))
        //            ModelState.AddModelError("IDTracking", "Tracking ID required");
        //        else
        //            ModelState.AddModelError("IDTracking", $"Invalid Tracking ID {TrackingID}");
        //    }
        //    if (DateSent.Year < 2000)
        //    {
        //        retBool = false;
        //        ModelState.AddModelError("SentDate", $"Invalid Date Sent {DateSent}");
        //    }
        //    if(!(string.IsNullOrWhiteSpace(NumDocs)))
        //    {
        //        if(!(int.TryParse(NumDocs,out int results)))
        //        {
        //            retBool = false;
        //            ModelState.AddModelError("DocsNum", $"Invalid Number of documents as must be numerica {NumDocs} ");
        //        }
        //    }
        //    else
        //    {
        //        retBool = false;
        //        ModelState.AddModelError("DocsNum", $"Number of documents cannot be blank");
        //    }

        //    if (string.Compare(CboxScanType, "blank", true) == 0)
        //    {
        //        retBool = false;
        //        ModelState.AddModelError("ScanType", $"Invalid Scan Type {CboxScanType}");
        //    }
        //    if (string.Compare(CboxDM, "blank", true) == 0)
        //    {
        //        retBool = false;
        //        ModelState.AddModelError("DMethod", $"Invalid Delivery method {CboxDM}");
        //    }


        //    return retBool;
        //}
        //public async Task InventoryTransfer()
        //{
        //    EdocsITSInventoryTransfer inventoryTransfer = new EdocsITSInventoryTransfer();
        //    inventoryTransfer.DateSent = DateSent;
        //    inventoryTransfer.DeliveryMethod = CboxDM;
        //    inventoryTransfer.EdocsCustomerName = UserLogin.CustomerName;
        //    inventoryTransfer.NumberDocsSent = int.Parse(NumDocs);
        //    inventoryTransfer.ScanType = CboxScanType;
        //    inventoryTransfer.TrackingID = TrackingID;
        //    inventoryTransfer.UserName = UserLogin.UserName;
        //    EdocsITSApis.InventoryTransfer(clientFactory, WebApiUrl, EdocsITSConstants.EdocsITSInventoryTransferController, inventoryTransfer).ConfigureAwait(true).GetAwaiter().GetResult();
        //    // EdocsITSApis.AddNewCustomer(clientFactory, WebApiUrl, EdocsITSConstants.EdocsITSCustomersController, EdocsITSCustomersModel).ConfigureAwait(true).GetAwaiter().GetResult();

        //}
        private bool CheckAuth()
        {
            if (!(EdocsITSHelpers.UserAuth(HttpContext.Session)))
                return false;
            return true;
        }
    }
}
