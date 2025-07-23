using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Edocs.ITS.AppService.ApisConst;
using Edocs.ITS.AppService.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Reflection;
using Newtonsoft.Json;
using Edocs.ITS.AppService.Interfaces;
using System.IO;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Utilities;

namespace Edocs.ITS.AppService.Pages.Invoice
{
    class TotalCost
    {
        public string CostTotal { get; set; }
    }
    public class CreateCustomInvoiceViewModel : PageModel
    {
        private readonly IConfiguration configuration;
        private readonly IHttpClientFactory clientFactory;
        private readonly string InvFileName = "InvoiceSaveCustom.html";
        private readonly string InvFolderName = "htmlfiles";
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string InvoiceHtmlTemplate = "InvoiceCustom.html";
        private Uri WebApiUrl
        { get { return configuration.GetValue<Uri>(EdocsITSConstants.JsonWebApi); } }
        public IList<EdocsITCustomerIDNameModel> EdocsITCustomerIDs
        { get; set; }
        public UserLoginModel UserLogin
        { get; set; }
        public EdocsITSCustomersModel EdocsITSCustomers
        { get; set; }
        public EdocsITSCustomersModel EdocsModel
        { get; set; }
        [BindProperty]
        public IList<CustomInvoiceModel> InvoiceModel
        { get; set; }
        private string InvoicesHtmlFileFolder
        { get { return configuration.GetValue<string>(EdocsITSConstants.JsonInvoicesHtmlFile); } }
        private InvoiceNum NumInv
        { get; set; }
        public CreateCustomInvoiceViewModel(IConfiguration config, IHttpClientFactory factoryClient, IWebHostEnvironment webHostEnvironment)
        {
            configuration = config;
            clientFactory = factoryClient;
            _webHostEnvironment = webHostEnvironment;
        }


        public async Task<IActionResult> OnGetAsync(string custID)
        {

            if (!EdocsITSHelpers.CheckAuth(HttpContext.Session))
                return Redirect("/LogInOut/LoginView");

            ViewData[GetSessionVariables.SessionKeyUserName] = GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyUserName).ConfigureAwait(true).GetAwaiter().GetResult();
            UserLogin = GetSessionVariables.SessionVarInstance.GetJsonSessionObject<UserLoginModel>(HttpContext.Session, EdocsITSConstants.SessionUserInfo).ConfigureAwait(false).GetAwaiter().GetResult();
            ViewData[GetSessionVariables.SessionKeyUserName] = EdocsITSHelpers.RepStr(ViewData[GetSessionVariables.SessionKeyUserName].ToString(), EdocsITSHelpers.QUOTE, string.Empty);
            EdocsITCustomerIDs = EdocsITSApis.GetITSCustomers(clientFactory, WebApiUrl, EdocsITSConstants.EdocsITSCustomersController, EdocsITSConstants.SpGetEdocsITSCustomers).ConfigureAwait(true).GetAwaiter().GetResult();
            if (!(string.IsNullOrWhiteSpace(custID)))
            {
                EdocsITSCustomers = EdocsITSApis.GetEdocsCustomer(clientFactory, WebApiUrl, EdocsITSConstants.EdocsITSCustomersController, custID, EdocsITSConstants.SpGetCustomerInformation).ConfigureAwait(false).GetAwaiter().GetResult();
                EdocsModel = EdocsITSApis.GetEdocsCustomer(clientFactory, WebApiUrl, EdocsITSConstants.EdocsITSCustomersController, "1000", EdocsITSConstants.SpGetCustomerInformation).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            return Page();
        }
        //  public void OnPostAsyncInsertCustomInvoice(List<CustomInvoiceModel> customers)
        public void OnPostAsyncInsertCustomInvoice(List<Object> customers)
        {

            //Truncate Table to delete all old records.


            //Check for NULL.
            if (customers == null)
            {
             //   EdocsITSApis.AddCustomInvoice(clientFactory,WebApiUrl, EdocsITSConstants.EdocsITSEdocsITSCustomInvoiceController,customInvoices)
                //   customers = new List<CustomInvoiceModel>();
            }

            //Loop and insert records.
            //foreach (CustomInvoiceModel customer in customers)
            //{
            //    entities.Customers.Add(customer);
            //}
            //int insertedRecords = entities.SaveChanges();
            int insertedRecords = 1;
            //    return Json(insertedRecords); 

        }
        [HttpGet]
        public JsonResult OnGetGetTotalValue(string float1,string float2)
        {

            float strToFloat = float.Parse(float1) + float.Parse(float2);
            TotalCost totalCost = new TotalCost();
            totalCost.CostTotal = string.Format("{0:0.00}", strToFloat);
            var converted = JsonConvert.SerializeObject(string.Format("{0:0.00}", strToFloat));
            return new JsonResult(converted);
            //Truncate Table to delete all old records.


            //Check for NULL.


            //Loop and insert records.
            //foreach (CustomInvoiceModel customer in customers)
            //{
            //    entities.Customers.Add(customer);
            //}
            //int insertedRecords = entities.SaveChanges();
            int insertedRecords = 1;
            //    return Json(insertedRecords); 

        }
        
         public async Task<JsonResult> OnPostAsync([FromBody] List<CustomInvoiceModel> customers)
        {

            //Truncate Table to delete all old records.


            //Check for NULL.
            if (customers != null)
            {
                
                   EdocsITSApis.AddCustomInvoice(clientFactory, WebApiUrl, EdocsITSConstants.EdocsITSEdocsITSCustomInvoiceController, customers).ConfigureAwait(false).GetAwaiter().GetResult();
                float itemToal = customers.Sum(p => p.ItemTotal);
                DateTime minDate = customers.Min(p => p.DateofService);
                DateTime maxDate = customers.Max(p => p.DateofService);
                GetInvoiceNumber(minDate, customers[0].EdocsCustomerID, itemToal).ConfigureAwait(false).GetAwaiter().GetResult();
                CreateInvoice(customers, customers[0].EdocsCustomerID.ToString(),itemToal,minDate,maxDate).ConfigureAwait(false).GetAwaiter().GetResult();
                //   customers = new List<CustomInvoiceModel>();


            }
            return new JsonResult(NumInv.NumberInvoice.ToString());

            //Loop and insert records.
            //foreach (CustomInvoiceModel customer in customers)
            //{
            //    entities.Customers.Add(customer);
            //}
            //int insertedRecords = entities.SaveChanges();
            int insertedRecords = 1;
           
            //    return Json(insertedRecords); 

        }
        private async Task CreateInvoice(List<CustomInvoiceModel> invIteams,string cusID,float invtotal,DateTime minInvDate,DateTime maxInvDate)
{
            EdocsITSCustomers = EdocsITSApis.GetEdocsCustomer(clientFactory, WebApiUrl, EdocsITSConstants.EdocsITSCustomersController, cusID, EdocsITSConstants.SpGetCustomerInformation).ConfigureAwait(false).GetAwaiter().GetResult();
            EdocsModel = EdocsITSApis.GetEdocsCustomer(clientFactory, WebApiUrl, EdocsITSConstants.EdocsITSCustomersController, "1000", EdocsITSConstants.SpGetCustomerInformation).ConfigureAwait(false).GetAwaiter().GetResult();
            string webRootPath = Path.Combine(_webHostEnvironment.WebRootPath, InvFolderName);
            string tempPlate = Path.Combine(webRootPath, InvoiceHtmlTemplate);
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            string invoice = System.IO.File.ReadAllText(tempPlate);
            invoice = RepString(invoice, "EdocsAddress", EdocsModel.EdocsCustomerAddress).ConfigureAwait(false).GetAwaiter().GetResult();
            invoice = RepString(invoice, "EdocsCity", EdocsModel.EdocsCustomerCity).ConfigureAwait(false).GetAwaiter().GetResult();
            invoice = RepString(invoice, "EdosState", EdocsModel.EdosCustomerState).ConfigureAwait(false).GetAwaiter().GetResult();
            invoice = RepString(invoice, "EdocsZipCode", EdocsModel.EdocsCustomerZipCode).ConfigureAwait(false).GetAwaiter().GetResult();
            string pNum = $"({EdocsModel.EdocsCustomerCellPhoneNumber.Substring(0, 3)})-({EdocsModel.EdocsCustomerCellPhoneNumber.Substring(3, 3)})-({EdocsModel.EdocsCustomerCellPhoneNumber.Substring(6, 4)})";
            invoice = RepString(invoice, "EdocsPhoneNumber", $"({EdocsModel.EdocsCustomerCellPhoneNumber.Substring(0,3)}) {EdocsModel.EdocsCustomerCellPhoneNumber.Substring(3,3)}-{EdocsModel.EdocsCustomerCellPhoneNumber.Substring(6, 4)}").ConfigureAwait(false).GetAwaiter().GetResult();
            invoice = RepString(invoice, "EdosFirstName", EdocsModel.EdosCustomerFirstName).ConfigureAwait(false).GetAwaiter().GetResult();
            invoice = RepString(invoice, "EdocsLastName", EdocsModel.EdocsCustomerLastName).ConfigureAwait(false).GetAwaiter().GetResult();
            invoice = RepString(invoice, "EdocsEmailAddress", EdocsModel.EdocsCustomerEmailAddress).ConfigureAwait(false).GetAwaiter().GetResult();

            // invoice = RepString(invoice, "EmailAddress", EdocsModel.EdocsCustomerEmailAddress).ConfigureAwait(false).GetAwaiter().GetResult();
            invoice = RepString(invoice, "InvoiceDate", DateTime.Now.ToString("MM-dd-yyyy")).ConfigureAwait(false).GetAwaiter().GetResult();
            invoice = RepString(invoice, "{edocslogo}", EdocsModel.ImgStr).ConfigureAwait(false).GetAwaiter().GetResult();
            invoice = RepString(invoice, "{customerLogo}", EdocsITSCustomers.ImgStr).ConfigureAwait(false).GetAwaiter().GetResult();
            invoice = RepString(invoice,"{EdocsInvoice#}", $"{NumInv.NumberInvoice.ToString()}").ConfigureAwait(false).GetAwaiter().GetResult();
            invoice = RepString(invoice, "EdocsCustomerName", EdocsITSCustomers.EdocsCustomerName).ConfigureAwait(false).GetAwaiter().GetResult();
            invoice = RepString(invoice, "EdocsCustomerAddress", EdocsITSCustomers.EdocsCustomerAddress).ConfigureAwait(false).GetAwaiter().GetResult();
            invoice = RepString(invoice, "EdocsCustomerCity", EdocsITSCustomers.EdocsCustomerCity).ConfigureAwait(false).GetAwaiter().GetResult();
            invoice = RepString(invoice, "EdosCustomerState", EdocsITSCustomers.EdosCustomerState).ConfigureAwait(false).GetAwaiter().GetResult();
            invoice = RepString(invoice, "EdocsCustomerZipCode", EdocsITSCustomers.EdocsCustomerZipCode).ConfigureAwait(false).GetAwaiter().GetResult();

            invoice = RepString(invoice, "EdocsCustomerPhoneNumber", $"({EdocsITSCustomers.EdocsCustomerCellPhoneNumber.Substring(0, 3)}) {EdocsITSCustomers.EdocsCustomerCellPhoneNumber.Substring(3, 3)}-{EdocsITSCustomers.EdocsCustomerCellPhoneNumber.Substring(6, 4)}").ConfigureAwait(false).GetAwaiter().GetResult();
            invoice = RepString(invoice, "{EdocsCustomerEmailAddress}", EdocsITSCustomers.EdocsCustomerEmailAddress).ConfigureAwait(false).GetAwaiter().GetResult();
                   invoice = RepString(invoice, "Scan Dates", $"Invoivce Date {minInvDate.ToString("MM-dd-yyyy")}-{maxInvDate.ToString("MM-dd-yyyy")}").ConfigureAwait(false).GetAwaiter().GetResult();
            invoice = RepString(invoice, "{EdocsCustomerID}", EdocsITSCustomers.EdocsCustomerID.ToString()).ConfigureAwait(false).GetAwaiter().GetResult();
            //invoice = RepString(invoice, "EdocsCustomerZipCode", EdocsITSCustomers.EdocsCustomerCity).ConfigureAwait(false).GetAwaiter().GetResult();
            foreach (var items in invIteams)
            {
                sb.AppendLine($"<tr class={EdocsITSConstants.Quoat}w-auto{EdocsITSConstants.Quoat} style={EdocsITSConstants.Quoat}font-size:medium{EdocsITSConstants.Quoat}>");
                sb.AppendLine($"<td style={EdocsITSConstants.Quoat}font-size:medium{EdocsITSConstants.Quoat}>{items.DateofService.ToString("MM-dd-yyyy")}</td>");
                sb.AppendLine($"<td style={EdocsITSConstants.Quoat}font-size:medium{EdocsITSConstants.Quoat}>{items.ItemQuantity} </td>");
                sb.AppendLine($"<td style={EdocsITSConstants.Quoat}font-size:medium{EdocsITSConstants.Quoat}>${string.Format("{0:0.00}", items.ItemCost)}</td>");
                
                sb.AppendLine($"<td style={EdocsITSConstants.Quoat}font-size:medium{EdocsITSConstants.Quoat}>{items.ItemDescription} </td>");
                sb.AppendLine($"<td style={EdocsITSConstants.Quoat}font-size:medium{EdocsITSConstants.Quoat}>${string.Format("{0:0.00}", items.ItemTotal)}</td>");
                
                //sb.AppendLine($"<td style={EdocsITSConstants.Quoat}font-size:medium{EdocsITSConstants.Quoat}>{items.DateofService.ToString("MM-dd-yyyyy")} </td>");
                //sb.AppendLine($"<td style={EdocsITSConstants.Quoat}font-size:medium{EdocsITSConstants.Quoat}>{items.Value.DocsOCR}</td>");
                //sb.AppendLine($"<td style={EdocsITSConstants.Quoat}font-size:medium{EdocsITSConstants.Quoat}>${string.Format("{0:0.00}", items.Value.PriceOCR)}</td>");
                //sb.AppendLine($"<td style={EdocsITSConstants.Quoat}font-size:medium{EdocsITSConstants.Quoat}>${string.Format("{0:0.00}", items.Value.OcrCost)}</td>");
                //sb.AppendLine($"<td style={EdocsITSConstants.Quoat}font-size:medium{EdocsITSConstants.Quoat}>{items.Value.CharTyped}</td>");
                //sb.AppendLine($"<td style={EdocsITSConstants.Quoat}font-size:medium{EdocsITSConstants.Quoat}>${string.Format("{0:0.00}", items.Value.PriceCharTyped)}</td>");
                //sb.AppendLine($"<td style={EdocsITSConstants.Quoat}font-size:medium{EdocsITSConstants.Quoat}>${string.Format("{0:0.00}", items.Value.CostPerChar)} </td>");

                //float totCost = items.Value.CostPerDoc + items.Value.OcrCost + items.Value.CostPerChar;
                //sb.AppendLine($"<td style={EdocsITSConstants.Quoat}font-size:medium{EdocsITSConstants.Quoat}>${string.Format("{0:0.00}", totCost)}</td>");
                sb.AppendLine("</tr>");
                string s = sb.ToString();
            }
            invoice = RepString(invoice, "{tableData}", sb.ToString()).ConfigureAwait(false).GetAwaiter().GetResult();
            invoice = RepString(invoice, "{totalCost}", "$"+string.Format("{0:0.00}",invtotal)).ConfigureAwait(false).GetAwaiter().GetResult();

sb.Clear();
            string invFileName = Path.Combine(InvoicesHtmlFileFolder, $"{NumInv.NumberInvoice}.html");

            System.IO.File.WriteAllTextAsync(invFileName,invoice).ConfigureAwait(false).GetAwaiter().GetResult();
            HtmlFileModel fileModel = new HtmlFileModel
            {
                HtmlData = invFileName,
                InvoiceNum = int.Parse(NumInv.NumberInvoice),
            };

            EdocsITSApis.UploadInventoryHtml(clientFactory, WebApiUrl, EdocsITSConstants.EdocsITSUploadHtmlFilesController, fileModel).ConfigureAwait(false).GetAwaiter().GetResult();

        }
        private async Task<string> RepString(string instr, string oldstr, string newstr)
        {
            return instr.Replace(oldstr, newstr);
        }
        
        private async Task GetInvoiceNumber(DateTime invSDate,int custID,float invTotalCost)
        {
            int invNum = 0;
            InvoiceModel invMod = new InvoiceModel
            {
                FileName = "N/A",
                InvoiceStartDate = invSDate,
                InvoiceEndDate = DateTime.Now,
                EdocsCustomerID = custID,
                InvoiceTotalAmount = invTotalCost,

            };
            //   System.IO.File.WriteAllText(@"l:\h.html", jsonHtml);
            NumInv = EdocsITSApis.CreateInvoiceNumber(clientFactory, WebApiUrl, EdocsITSConstants.AddInvoiceController, invMod).ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}
