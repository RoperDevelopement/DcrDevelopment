using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edocs.ITS.AppService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Edocs.ITS.AppService.ApisConst;
using Edocs.ITS.AppService.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Drawing.Drawing2D;
using System.IO;
using Edocs.ITS.AppService.Interfaces;
using System.Numerics;

namespace Edocs.ITS.AppService.Pages.TrackingSystem
{
    public class SignOffPickUpModel : PageModel
    {
        private readonly IConfiguration configuration;
        private readonly IHttpClientFactory clientFactory;
        private readonly string SigFileName = "SignatureCustom.html";
        private readonly string InvFolderName = "htmlfiles";
        private readonly string InvFileName = "SignSave.html";
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string InvoiceHtmlTemplate = "Invoice.html";
        private Uri WebApiUrl
        { get { return configuration.GetValue<Uri>(EdocsITSConstants.JsonWebApi); } }
        private string InvoicesHtmlFileFolder
        { get { return configuration.GetValue<string>(EdocsITSConstants.JsonInvoicesHtmlFile); } }
        public IList<EdocsITCustomerIDNameModel> EdocsITCustomerIDs
        { get; set; }
        public string SignatureDataUrl
        { get; set; }
        public IDictionary<string, EdocsITSInventoryTransfer> ITSInventoryTransferSent
        { get; set; }
        public UserLoginModel UserLogin
        { get; set; }
        public string TransferCustomerName
        { get; set; }
        public SignOffPickUpModel(IConfiguration config, IHttpClientFactory factoryClient, IWebHostEnvironment webHostEnvironment)
        {
            configuration = config;
            clientFactory = factoryClient;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> OnGetAsync(string repSDate, string repSEndDate, string custName)
        {
            ViewData[GetSessionVariables.SessionKeyUserName] = GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyUserName).ConfigureAwait(true).GetAwaiter().GetResult();
            UserLogin = GetSessionVariables.SessionVarInstance.GetJsonSessionObject<UserLoginModel>(HttpContext.Session, EdocsITSConstants.SessionUserInfo).ConfigureAwait(false).GetAwaiter().GetResult();
            ViewData[GetSessionVariables.SessionKeyUserName] = EdocsITSHelpers.RepStr(ViewData[GetSessionVariables.SessionKeyUserName].ToString(), EdocsITSHelpers.QUOTE, string.Empty);
            EdocsITCustomerIDs = EdocsITSApis.GetITSCustomers(clientFactory, WebApiUrl, EdocsITSConstants.EdocsITSCustomersController, EdocsITSConstants.SpGetEdocsITSCustomers).ConfigureAwait(true).GetAwaiter().GetResult();
            if (!(string.IsNullOrWhiteSpace(repSEndDate)))
            {

                ITSInventoryTransferSent = EdocsITSApis.GetInventorySent(clientFactory, WebApiUrl, EdocsITSConstants.EdocsITSReportsController, repSDate, repSEndDate, EdocsITSConstants.SpEdocsITSGetReports, "docSent", custName).ConfigureAwait(false).GetAwaiter().GetResult();
                if ((ITSInventoryTransferSent != null) && (ITSInventoryTransferSent.Count() > 0))
                    GetSessionVariables.SessionVarInstance.SetSessionOjbjectAsJson(HttpContext.Session, "SigData", ITSInventoryTransferSent).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            return Page();
        }
        public async Task<IActionResult> OnPost(string SignatureDataUrl)
        {
            //var base64Signature = SignatureDataUrl.Split(",")[1];
            //var binarySignature = Convert.FromBase64String(base64Signature);

            //System.IO.File.WriteAllBytes("L:\\Signature.png", binarySignature);
           string sigFile = CreateSignDoc(SignatureDataUrl).ConfigureAwait(false).GetAwaiter().GetResult();
            if(!(string.IsNullOrEmpty(sigFile)))
                return Redirect($"/TrackingSystem/PrintsSgnatureView?sigatureFile={sigFile}"); 
            EdocsITCustomerIDs = EdocsITSApis.GetITSCustomers(clientFactory, WebApiUrl, EdocsITSConstants.EdocsITSCustomersController, EdocsITSConstants.SpGetEdocsITSCustomers).ConfigureAwait(true).GetAwaiter().GetResult();
            return Page();
        }
        private async Task<string> CreateSignDoc(string signatureDataUrl)
        {
            ITSInventoryTransferSent = GetSessionVariables.SessionVarInstance.GetJsonSessionObject<IDictionary<string, EdocsITSInventoryTransfer>>(HttpContext.Session, "SigData").ConfigureAwait(false).GetAwaiter().GetResult();
           // EdocsITSInventoryTransfer cName = ITSInventoryTransferSent.Values.ElementAt[0];
            string cName = ITSInventoryTransferSent.Values.ElementAt(0).EdocsCustomerName;
            var base64Signature = signatureDataUrl.Split(",")[1];
            var binarySignature = Convert.FromBase64String(base64Signature);
            EdocsITSCustomersModel EdocsITSCustomers = EdocsITSApis.GetEdocsCustomerByName(clientFactory, WebApiUrl, EdocsITSConstants.EdocsITSGetCustomersByNameController, cName, EdocsITSConstants.SpGetCustomerInformationByCustName).ConfigureAwait(false).GetAwaiter().GetResult();
           EdocsITSCustomersModel EdocsModel = EdocsITSApis.GetEdocsCustomer(clientFactory, WebApiUrl, EdocsITSConstants.EdocsITSCustomersController, "1000", EdocsITSConstants.SpGetCustomerInformation).ConfigureAwait(false).GetAwaiter().GetResult();
            string webRootPath = Path.Combine(_webHostEnvironment.WebRootPath, InvFolderName);
            string tempPlate = Path.Combine(webRootPath, SigFileName);
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            string invoice = System.IO.File.ReadAllText(tempPlate);
            invoice = RepString(invoice, "EdocsFirstName", EdocsModel.EdosCustomerFirstName).ConfigureAwait(false).GetAwaiter().GetResult();
            invoice = RepString(invoice, "EdocsLastName", EdocsModel.EdocsCustomerLastName).ConfigureAwait(false).GetAwaiter().GetResult();
            // invoice = RepString(invoice, "EdocsAddress", EdocsModel.EdocsCustomerAddress).ConfigureAwait(false).GetAwaiter().GetResult();
            //  invoice = RepString(invoice, "EdocsCity", EdocsModel.EdocsCustomerCity).ConfigureAwait(false).GetAwaiter().GetResult();
            // invoice = RepString(invoice, "EdosState", EdocsModel.EdosCustomerState).ConfigureAwait(false).GetAwaiter().GetResult();
            // invoice = RepString(invoice, "EdocsZipCode", EdocsModel.EdocsCustomerZipCode).ConfigureAwait(false).GetAwaiter().GetResult();
            
            string pNumber = $"({EdocsModel.EdocsCustomerCellPhoneNumber.Substring(0, 3)}) {EdocsModel.EdocsCustomerCellPhoneNumber.Substring(3, 3)}-{EdocsModel.EdocsCustomerCellPhoneNumber.Substring(6, 4)}";
            //    invoice = RepString(invoice, "EdocsPhoneNumber", $"({EdocsModel.EdocsCustomerCellPhoneNumber.Substring(0,3)})-{EdocsModel.EdocsCustomerCellPhoneNumber.Substring(3,3)}-{EdocsModel.EdocsCustomerCellPhoneNumber.Substring(6,4)}").ConfigureAwait(false).GetAwaiter().GetResult();
            invoice = RepString(invoice, "EdocsPhoneNumber",pNumber).ConfigureAwait(false).GetAwaiter().GetResult();
            invoice = RepString(invoice, "EdocsEmailAddress", EdocsModel.EdocsCustomerEmailAddress).ConfigureAwait(false).GetAwaiter().GetResult();
          //  invoice = RepString(invoice, "InvoiceDate", DateTime.Now.ToString("MM-dd-yyyy")).ConfigureAwait(false).GetAwaiter().GetResult();
            invoice = RepString(invoice, "{edocslogo}", EdocsModel.ImgStr).ConfigureAwait(false).GetAwaiter().GetResult();
            invoice = RepString(invoice, "{customerLogo}", EdocsITSCustomers.ImgStr).ConfigureAwait(false).GetAwaiter().GetResult();

            invoice = RepString(invoice, "EdocsCustomerName", EdocsITSCustomers.EdocsCustomerName).ConfigureAwait(false).GetAwaiter().GetResult();
            invoice = RepString(invoice, "EdocsCustomerAddress", EdocsITSCustomers.EdocsCustomerAddress).ConfigureAwait(false).GetAwaiter().GetResult();
            invoice = RepString(invoice, "EdocsCustomerCity", EdocsITSCustomers.EdocsCustomerCity).ConfigureAwait(false).GetAwaiter().GetResult();
            invoice = RepString(invoice, "EdosCustomerState", EdocsITSCustomers.EdosCustomerState).ConfigureAwait(false).GetAwaiter().GetResult();
            invoice = RepString(invoice, "EdocsCustomerZipCode", EdocsITSCustomers.EdocsCustomerZipCode).ConfigureAwait(false).GetAwaiter().GetResult();

            invoice = RepString(invoice, "EdocsCustomerPhoneNumber", $"({EdocsITSCustomers.EdocsCustomerCellPhoneNumber.Substring(0, 3)}) {EdocsITSCustomers.EdocsCustomerCellPhoneNumber.Substring(3, 3)}-{EdocsITSCustomers.EdocsCustomerCellPhoneNumber.Substring(6, 4)}").ConfigureAwait(false).GetAwaiter().GetResult();
            invoice = RepString(invoice, "{EdocsCustomerEmailAddress}", EdocsITSCustomers.EdocsCustomerEmailAddress).ConfigureAwait(false).GetAwaiter().GetResult();
            //  invoice = RepString(invoice, "Scan Dates", $"Scan Dates {ScanStartDate}-{ScanEndDate}").ConfigureAwait(false).GetAwaiter().GetResult();
            invoice = RepString(invoice, "{EdocsCustomerID}", EdocsITSCustomers.EdocsCustomerID.ToString()).ConfigureAwait(false).GetAwaiter().GetResult();
            invoice = RepString(invoice, "{SignOff}", signatureDataUrl).ConfigureAwait(false).GetAwaiter().GetResult();
           // invoice = RepString(invoice, "{TBoxesPickup}", ITSInventoryTransferSent.Count().ToString()).ConfigureAwait(false).GetAwaiter().GetResult();
            int tDocs = ITSInventoryTransferSent.Sum(x => x.Value.NumberDocsSent);
            invoice = RepString(invoice, "{PDate}",DateTime.Now.ToString("MM-dd-yyyy")).ConfigureAwait(false).GetAwaiter().GetResult();
           
          //  invoice = RepString(invoice, "{TDocsPickup}",tDocs.ToString()).ConfigureAwait(false).GetAwaiter().GetResult();
            //invoice = RepString(invoice, "EdocsCustomerZipCode", EdocsITSCustomers.EdocsCustomerCity).ConfigureAwait(false).GetAwaiter().GetResult();
            foreach (KeyValuePair<string, EdocsITSInventoryTransfer> items in ITSInventoryTransferSent)
            {
                sb.AppendLine($"<tr class={EdocsITSConstants.Quoat}w-auto{EdocsITSConstants.Quoat} style={EdocsITSConstants.Quoat}font-size:medium{EdocsITSConstants.Quoat}>");
                sb.AppendLine($"<td style={EdocsITSConstants.Quoat}font-size:medium{EdocsITSConstants.Quoat}>{items.Value.TrackingID}</td>");
                sb.AppendLine($"<td style={EdocsITSConstants.Quoat}font-size:medium{EdocsITSConstants.Quoat}>{items.Value.UserName} </td>");
                sb.AppendLine($"<td style={EdocsITSConstants.Quoat}font-size:medium{EdocsITSConstants.Quoat}>{items.Value.DateSent} </td>");
                sb.AppendLine($"<td style={EdocsITSConstants.Quoat}font-size:medium{EdocsITSConstants.Quoat}>{items.Value.NumberDocsSent}</td>");
                sb.AppendLine($"<td style={EdocsITSConstants.Quoat}font-size:medium{EdocsITSConstants.Quoat}>{items.Value.ScanType}</td>");
                sb.AppendLine($"<td style={EdocsITSConstants.Quoat}font-size:medium{EdocsITSConstants.Quoat}>{items.Value.DeliveryMethod}</td>");
               sb.AppendLine("</tr>");
            }
            invoice = RepString(invoice, "{tableData}", sb.ToString()).ConfigureAwait(false).GetAwaiter().GetResult();
            sb.Clear();
          
            sb.AppendLine($"<p class={EdocsITSConstants.Quoat}text-info text-right{EdocsITSConstants.Quoat} style={EdocsITSConstants.Quoat}font-size:medium;margin:0;{EdocsITSConstants.Quoat}>Total Boxes {ITSInventoryTransferSent.Count().ToString()}</p> ");
            sb.AppendLine($"<p class={EdocsITSConstants.Quoat}text-info text-right{EdocsITSConstants.Quoat} style={EdocsITSConstants.Quoat}font-size:medium;margin:0;{EdocsITSConstants.Quoat}>Total Documents {tDocs}</p>");
            invoice = RepString(invoice, "{invsum}", sb.ToString()).ConfigureAwait(false).GetAwaiter().GetResult();
            string invFileName = Path.Combine(InvoicesHtmlFileFolder, $"{EdocsITSCustomers.EdocsCustomerName}_{DateTime.Now.ToString("MM-dd-yyyy")}.html");
            System.IO.File.WriteAllTextAsync(invFileName, invoice).ConfigureAwait(false).GetAwaiter().GetResult();
            return invFileName;
            //    System.IO.File.WriteAllTextAsync(Path.Combine(@"L:\", InvFileName), invoice).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        private async Task<string> RepString(string instr, string oldstr, string newstr)
        {
            return instr.Replace(oldstr, newstr);
        }
    }
}
