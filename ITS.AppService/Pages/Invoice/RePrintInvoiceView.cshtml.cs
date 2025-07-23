using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Edocs.ITS.AppService.ApisConst;
using Edocs.ITS.AppService.Interfaces;
using Edocs.ITS.AppService.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace Edocs.ITS.AppService.Pages.Invoice
{
    public class RePrintInvoiceViewModel : PageModel
    {
        private readonly IConfiguration configuration;
        private readonly IHttpClientFactory clientFactory;
        private readonly string InvFileName = "InvoiceSave.html";
        private readonly string InvFolderName = "htmlfiles";
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string InvoiceHtmlTemplate = "Invoice.html";
        private Uri WebApiUrl
        { get { return configuration.GetValue<Uri>(EdocsITSConstants.JsonWebApi); } }
        public UserLoginModel UserLogin
        { get; set; }
        string InvoiceFile
        { get; set; }

        public RePrintInvoiceViewModel(IConfiguration config, IHttpClientFactory factoryClient, IWebHostEnvironment webHostEnvironment)
        {
            configuration = config;
            clientFactory = factoryClient;
            _webHostEnvironment = webHostEnvironment;
        }
        public HtmlFileModel HtmlFile
        { get; set; }
        public string CustomerID
        { get; set; }
        public string Invoice
        { get; set; }
        [BindProperty]
        public IList<EdocsITCustomerIDNameModel> EdocsITCustomerIDs
        { get; set; }
        public IList<InvoiceNumberDateSentModel> InvNumDateSent
        { get; set; }
        public async Task<IActionResult> OnGetAsync(string cusID, string invNum = null)
        {
            if (!EdocsITSHelpers.CheckAuth(HttpContext.Session))
                return Redirect("/LogInOut/LoginView");

            HtmlFile = new HtmlFileModel();
            ViewData[GetSessionVariables.SessionKeyUserName] = GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyUserName).ConfigureAwait(true).GetAwaiter().GetResult();
            UserLogin = GetSessionVariables.SessionVarInstance.GetJsonSessionObject<UserLoginModel>(HttpContext.Session, EdocsITSConstants.SessionUserInfo).ConfigureAwait(false).GetAwaiter().GetResult();
            ViewData[GetSessionVariables.SessionKeyUserName] = EdocsITSHelpers.RepStr(ViewData[GetSessionVariables.SessionKeyUserName].ToString(), EdocsITSHelpers.QUOTE, string.Empty);


            EdocsITCustomerIDs = EdocsITSApis.GetITSCustomers(clientFactory, WebApiUrl, EdocsITSConstants.EdocsITSCustomersController, EdocsITSConstants.SpGetEdocsITSCustomers).ConfigureAwait(true).GetAwaiter().GetResult();
            // string webRootPath = Path.Combine(_webHostEnvironment.WebRootPath, InvFolderName);
            // string invoiceFile = Path.Combine(webRootPath, InvFileName);
            if (!(string.IsNullOrWhiteSpace(invNum)))
            {
                HtmlFile = EdocsITSApis.GetInventoryHtml(clientFactory, WebApiUrl, EdocsITSConstants.EdocsITSUploadHtmlFilesController, int.Parse(invNum)).ConfigureAwait(false).GetAwaiter().GetResult();
                HtmlFile.HtmlData = System.IO.File.ReadAllTextAsync(HtmlFile.HtmlData).ConfigureAwait(false).GetAwaiter().GetResult();
                //HtmlFile.HtmlData.Replace("~/img/edocs logo.gif", "/img/edocs logo.gif");
            }
            else
            {
                if (!(string.IsNullOrWhiteSpace(cusID)))
                {
                    InvNumDateSent = EdocsITSApis.GetInvoiceNumDateSent(clientFactory, WebApiUrl, EdocsITSConstants.EdocsITSInventoryTransferController, int.Parse(cusID)).ConfigureAwait(false).GetAwaiter().GetResult();
                    // Invoice = System.IO.File.ReadAllTextAsync(invoiceFile).ConfigureAwait(false).GetAwaiter().GetResult();

                }
            }
            return Page();
        }

    }
}
