using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Edocs.ITS.AppService.Models;
using Edocs.ITS.AppService.ApisConst;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Security.Policy;

namespace Edocs.ITS.AppService.Pages.Invoice
{
    public class UpDateInvoiceViewModel : PageModel
    {
        private readonly IConfiguration configuration;
        private readonly IHttpClientFactory clientFactory;
        private Uri WebApiUrl
        { get { return configuration.GetValue<Uri>(EdocsITSConstants.JsonWebApi); } }
        public UserLoginModel UserLogin
        { get; set; }
        public IList<EdocsITCustomerIDNameModel> EdocsITCustomerIDs
        { get; set; }
        [BindProperty]
        public IList<UpdateInvoicesView> Invoice
        { get; set; }
        public IList<CustomersInvoicesModel> CustInvoices
        { get; set; }
        public float TotalOwed
        { get; set; }
        public float TotalPaid
        { get; set; }
        public float TotalAmount 
        { get; set; }
        public float Owed
        { get; set; }
        public string DateLastPaid
        { get; set; }
        public int CustNumber
        {
            get;set;
        }
        public UpDateInvoiceViewModel(IConfiguration config, IHttpClientFactory factoryClient)
        {
            configuration = config;
            clientFactory = factoryClient;
        }

        public async Task<IActionResult> OnGetAsync (string invNum,string totalPaid=null,string custId=null)
        {
            if (!EdocsITSHelpers.CheckAuth(HttpContext.Session))
                return Redirect("/LogInOut/LoginView");

            ViewData[GetSessionVariables.SessionKeyUserName] = GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyUserName).ConfigureAwait(true).GetAwaiter().GetResult();
            UserLogin = GetSessionVariables.SessionVarInstance.GetJsonSessionObject<UserLoginModel>(HttpContext.Session, EdocsITSConstants.SessionUserInfo).ConfigureAwait(false).GetAwaiter().GetResult();
            ViewData[GetSessionVariables.SessionKeyUserName] = EdocsITSHelpers.RepStr(ViewData[GetSessionVariables.SessionKeyUserName].ToString(), EdocsITSHelpers.QUOTE, string.Empty);
            var qString = Request.QueryString;
          
            if (!(string.IsNullOrWhiteSpace(invNum)))
            {
                Invoice = EdocsITSApis.GetInvoices(clientFactory, WebApiUrl, EdocsITSConstants.UpdateInvoicesController, int.Parse(invNum)).ConfigureAwait(false).GetAwaiter().GetResult();
                GetInvoiceNumbers(invNum).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            else
                if ((qString.HasValue))
            {
                int cID = int.Parse(Request.Query["custId"].ToString());
                int numInv = int.Parse(Request.Query["invoiceNumber"].ToString());
                string pAmount =  Request.Query["paidAmount"].ToString();
                Invoice = EdocsITSApis.UpdateInvoicePaid(clientFactory, WebApiUrl, EdocsITSConstants.UpdateInvoicesController, numInv,cID,pAmount).ConfigureAwait(false).GetAwaiter().GetResult();
                if((Invoice != null) && (Invoice.Count > 0))
                GetInvoiceNumbers(numInv.ToString()).ConfigureAwait(false).GetAwaiter().GetResult();
                //EdocsITSApis.GetInvoices(clientFactory, WebApiUrl, EdocsITSConstants.UpdateInvoicesController, 1003).ConfigureAwait(false).GetAwaiter().GetResult();

            }
           
            CustInvoices = EdocsITSApis.GetCustomerUnpaidInvoices(clientFactory,WebApiUrl, EdocsITSConstants.UpdateInvoicesController).ConfigureAwait(false).GetAwaiter().GetResult();
            return Page();
        }
        private async Task GetInvoiceNumbers(string invNum)
        {
           
            CustNumber = Invoice[0].EdocsCustomerID;
            if ((Invoice != null) && (Invoice.Count() > 0))
            {
                if ((Invoice != null) && (Invoice.Count() > 0))
                    foreach (var dp in Invoice.Where(w => w.DateInvoicePaid.Year < 2023))
                    {
                        dp.DateInvoicePaid = DateTime.Now;
                    }
                TotalPaid = Invoice.Sum(p => p.TotalPaid);
                TotalAmount = Invoice.Sum(p => p.TotalInvoiceAmount);
                TotalOwed =   TotalAmount- TotalPaid;
                var k =  Invoice.OrderByDescending(i => i.DateInvoicePaid).First();
                DateLastPaid = k.DateInvoicePaid.ToString("MM-dd-yyyy");
               

            }

        }
        public async Task<IActionResult> Post( )
        {
            CustInvoices = EdocsITSApis.GetCustomerUnpaidInvoices(clientFactory, WebApiUrl, EdocsITSConstants.UpdateInvoicesController).ConfigureAwait(false).GetAwaiter().GetResult();
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
