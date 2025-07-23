using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Edocs.ITS.AppService.ApisConst;
using Edocs.ITS.AppService.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace Edocs.ITS.AppService.Pages.Partial_Pages
{
    public class PartialPageCustomerInvoiceModel : PageModel
    {
        private readonly IConfiguration configuration;
        private readonly IHttpClientFactory clientFactory;
        private Uri WebApiUrl
        { get { return configuration.GetValue<Uri>(EdocsITSConstants.JsonWebApi); } }
        public UserLoginModel UserLogin
        { get; set; }
        public IList<UpdateInvoicesView> Invoice
        { get; set; }
        public PartialPageCustomerInvoiceModel(IConfiguration config, IHttpClientFactory factoryClient, IWebHostEnvironment webHostEnvironment)
        {
            configuration = config;
clientFactory = factoryClient;
           


        }
        public async Task<IActionResult> OnGetAsync(string invNum)
        {
            if(!CheckAuth())
                return Redirect("/LogInOut/LoginView");
            ViewData[GetSessionVariables.SessionKeyUserName] = GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyUserName).ConfigureAwait(true).GetAwaiter().GetResult();
            UserLogin = GetSessionVariables.SessionVarInstance.GetJsonSessionObject<UserLoginModel>(HttpContext.Session, EdocsITSConstants.SessionUserInfo).ConfigureAwait(false).GetAwaiter().GetResult();
            ViewData[GetSessionVariables.SessionKeyUserName] = EdocsITSHelpers.RepStr(ViewData[GetSessionVariables.SessionKeyUserName].ToString(), EdocsITSHelpers.QUOTE, string.Empty);
            Invoice = EdocsITSApis.GetInvoices(clientFactory, WebApiUrl, EdocsITSConstants.UpdateInvoicesController, int.Parse(invNum)).ConfigureAwait(false).GetAwaiter().GetResult();
            if((Invoice != null) && (invNum.Count() >0))
                foreach (var dp in Invoice.Where(w => w.DateInvoicePaid.Year < 2023))
                {
                    dp.DateInvoicePaid = DateTime.Now;
                }
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
