using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Threading.Tasks;
using Edocs.ITS.AppService.ApisConst;
using System.IO;
using Edocs.ITS.AppService.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Edocs.ITS.AppService.Interfaces;

namespace Edocs.ITS.AppService.Pages.Partial_Pages
{
    public class GenerateInvoicePartialViewModel : PageModel
    {
        private readonly IConfiguration configuration;
        private readonly IHttpClientFactory clientFactory;
        private readonly string InvFileName = "InvoiceSave.html";
        private readonly string InvFolderName = "htmlfiles";
        private readonly IWebHostEnvironment _webHostEnvironment;
        private Uri WebApiUrl
        { get { return configuration.GetValue<Uri>(EdocsITSConstants.JsonWebApi); } }
        private string InvoicesHtmlFileFolder
        { get { return configuration.GetValue<string>(EdocsITSConstants.JsonInvoicesHtmlFile); } }
        public string InvoiceNum
        { get; set; }
        public InvoiceNum NumInv
        { get; set; }
        public GenerateInvoicePartialViewModel(IConfiguration config, IHttpClientFactory factoryClient, IWebHostEnvironment webHostEnvironment)
        {
            configuration = config;
            clientFactory = factoryClient;
            _webHostEnvironment = webHostEnvironment;


        }
        public void Get()
        { Console.WriteLine(); }
        public async Task<IActionResult> OnGetAsync(DateTime invSDate, DateTime invEDate, string custID, string invTotalCost)
        //  public async Task<IActionResult> OnGetAsync()
        {
            InvoiceModel invMod = new InvoiceModel
            {
                FileName = "N/A",
                InvoiceStartDate = invSDate,
                InvoiceEndDate = invEDate,
                EdocsCustomerID = int.Parse(custID),
                InvoiceTotalAmount = float.Parse(invTotalCost),

            };
            //   System.IO.File.WriteAllText(@"l:\h.html", jsonHtml);
            NumInv = EdocsITSApis.CreateInvoiceNumber(clientFactory, WebApiUrl, EdocsITSConstants.AddInvoiceController, invMod).ConfigureAwait(false).GetAwaiter().GetResult();
            UpLoadInvoice(invMod.EdocsCustomerID).ConfigureAwait(false).GetAwaiter().GetResult();
            //InvoiceNum = "10000";
            return Page();
        }
        public async Task UpLoadInvoice(int edocsCustID)
        {
            string webRootPath = Path.Combine(_webHostEnvironment.WebRootPath, InvFolderName);
            string fileInv = Path.Combine(webRootPath, InvFileName);
            string invHtml = System.IO.File.ReadAllTextAsync(fileInv).ConfigureAwait(false).GetAwaiter().GetResult();
            invHtml = invHtml.Replace("{EdocsInvoice#}", $"{NumInv.NumberInvoice.ToString()}");
            string invFileName = Path.Combine(InvoicesHtmlFileFolder, $"{NumInv.NumberInvoice}.html");
                 System.IO.File.WriteAllTextAsync(invFileName, invHtml).ConfigureAwait(false).GetAwaiter().GetResult();
            HtmlFileModel fileModel = new HtmlFileModel
            {
                HtmlData = invFileName,
                InvoiceNum = int.Parse(NumInv.NumberInvoice),
            };

            EdocsITSApis.UploadInventoryHtml(clientFactory, WebApiUrl, EdocsITSConstants.EdocsITSUploadHtmlFilesController, fileModel).ConfigureAwait(false).GetAwaiter().GetResult();
            AddInvoiceNumberModel addInvoiceNumberModel = new AddInvoiceNumberModel
            {
                EdocsCustomerID = edocsCustID,
                InvoiceNum = fileModel.InvoiceNum

            };
        EdocsITSApis.AddInvoiceNumber(clientFactory, WebApiUrl, EdocsITSConstants.AddEocsITSInvoiceNumberController, addInvoiceNumberModel).ConfigureAwait(false).GetAwaiter().GetResult();


        }
        public async Task OnPostAsync(string jsonHtml)
        {
            Console.WriteLine("");
        }
    }
}
