using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Threading.Tasks;
using Edocs.ITS.AppService.ApisConst;
using Edocs.ITS.AppService.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

using Org.BouncyCastle.Asn1.Ocsp;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;


namespace Edocs.ITS.AppService.Pages.Invoice
{
    public class CreateInvoiceTrackingIDsViewModel : PageModel
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
        public IDictionary<int, EdocsITSCostScanningModel> ScanningCost
        { get; set; }

        public IDictionary<int, EdocsITSCostScanningModel> ScanningCostTemp
        { get; set; }
        public IDictionary<int, EdocsITSUsersModel> ListUserModel
        { get; set; }
        public string CustomerID
        { get; set; }
        public int TotalDocuments
        { get; set; }
        public float TotalDocCost
        { get; set; }
        public float TotalCharCost
        { get; set; }
        public float TotalOcrCost
        { get; set; }
        public int TotalOcr
        { get; set; }
        public int TotalCharTyped
        { get; set; }
        public float TotalCost
        { get; set; }
        public EdocsITSCustomersModel EdocsITSCustomers
        { get; set; }
        public EdocsITSCustomersModel EdocsModel
        { get; set; }
        public float TotalInvoice
        { get; set; }
        public string ScanStartDate
        { get; set; }
        public string ScanEndDate
{ get; set; } = "N/A";
        [Display(Name = "Report Start Date")]
        [DataType(DataType.Date)]
        public DateTime RepStartDate
        { get; set; }
        [BindProperty]
        [DataType(DataType.Date)]
        [Display(Name = "Report End Date")]


        public DateTime RepEndDate
        { get; set; }
        public IList<EdocsITCustomerIDNameModel> EdocsITCustomerIDs
        { get; set; }
        public CreateInvoiceTrackingIDsViewModel (IConfiguration config, IHttpClientFactory factoryClient, IWebHostEnvironment webHostEnvironment)
        {
            configuration = config;
            clientFactory = factoryClient;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> OnGetAsync(DateTime repSDate, DateTime repSEndDate, string custID)
        {
            //  return Redirect("/LogInOut/LoginView");
            if (!EdocsITSHelpers.CheckAuth(HttpContext.Session))
                return Redirect("/LogInOut/LoginView");

            ViewData[GetSessionVariables.SessionKeyUserName] = GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyUserName).ConfigureAwait(true).GetAwaiter().GetResult();
            UserLogin = GetSessionVariables.SessionVarInstance.GetJsonSessionObject<UserLoginModel>(HttpContext.Session, EdocsITSConstants.SessionUserInfo).ConfigureAwait(false).GetAwaiter().GetResult();
            ViewData[GetSessionVariables.SessionKeyUserName] = EdocsITSHelpers.RepStr(ViewData[GetSessionVariables.SessionKeyUserName].ToString(), EdocsITSHelpers.QUOTE, string.Empty);
            if (repSDate.Year > 2000)
            {
                ScanStartDate = repSDate.ToString("MM-dd-yyyy");
                ScanEndDate = repSEndDate.ToString("MM-dd-yyyy");
                ScanningCostTemp  = EdocsITSApis.GetScanninCost(clientFactory, WebApiUrl, EdocsITSConstants.EdocsITSScanningCoastController, repSDate, repSEndDate, custID, "trackid").ConfigureAwait(false).GetAwaiter().GetResult();
                if ((ScanningCostTemp != null) && (ScanningCostTemp.Count() > 0))
                {
                    TotalDocuments = ScanningCost.Sum(p => p.Value.Uploaded);
                    TotalDocCost = ScanningCost.Sum(p => p.Value.CostPerDoc);
                    TotalCharCost = ScanningCost.Sum(p => p.Value.CostPerChar);
                    TotalOcrCost = ScanningCost.Sum(p => p.Value.OcrCost);
                    TotalOcr = ScanningCost.Sum(p => p.Value.DocsOCR);
                    TotalCharTyped = ScanningCost.Sum(p => p.Value.CharTyped);
                    TotalCost = TotalDocCost + TotalCharCost + TotalOcrCost;
                    EdocsITSCustomers = EdocsITSApis.GetEdocsCustomer(clientFactory, WebApiUrl, EdocsITSConstants.EdocsITSCustomersController, custID, EdocsITSConstants.SpGetCustomerInformation).ConfigureAwait(false).GetAwaiter().GetResult();
                    EdocsModel = EdocsITSApis.GetEdocsCustomer(clientFactory, WebApiUrl, EdocsITSConstants.EdocsITSCustomersController, "1000", EdocsITSConstants.SpGetCustomerInformation).ConfigureAwait(false).GetAwaiter().GetResult();
                    CreateInvoie().ConfigureAwait(false).GetAwaiter().GetResult();
                }
                else
                    TotalDocuments = -1;




            }
            else
            {

                TotalOcr = -1;
                EdocsITCustomerIDs = EdocsITSApis.GetITSCustomers(clientFactory, WebApiUrl, EdocsITSConstants.EdocsITSCustomersController, EdocsITSConstants.SpGetEdocsITSCustomers).ConfigureAwait(true).GetAwaiter().GetResult();
                if (!(string.IsNullOrWhiteSpace(custID)))
                {

                    IList<EdocsITSMinMaxDateModel> minMaxDate = EdocsITSApis.GetMaxMinDate(clientFactory, WebApiUrl, EdocsITSConstants.EdocsITSReportsController, custID).ConfigureAwait(false).GetAwaiter().GetResult();
                    if ((minMaxDate != null) && (minMaxDate.Count() > 0))
                    {
                        RepStartDate = minMaxDate[0].MinDate;
                        RepEndDate = minMaxDate[0].MaxDate.AddDays(1);
                        ScanStartDate = RepStartDate.ToString("MM-dd-yyyy");
                        ScanEndDate = RepEndDate.ToString("MM-dd-yyyy");
                        repSDate = RepStartDate;
                        repSEndDate = RepEndDate;
                        ScanningCostTemp = EdocsITSApis.GetScanninCost(clientFactory, WebApiUrl, EdocsITSConstants.EdocsITSScanningCoastController, repSDate, repSEndDate, custID, "tackid").ConfigureAwait(false).GetAwaiter().GetResult();
                        if ((ScanningCostTemp != null) && (ScanningCostTemp.Count() > 0))
                        {
                            GetTotalsTrackingIDs().ConfigureAwait(false).GetAwaiter().GetResult();

                            TotalDocuments = ScanningCost.Sum(p => p.Value.Uploaded);
                            TotalDocCost = ScanningCost.Sum(p => p.Value.CostPerDoc);
                            TotalCharCost = ScanningCost.Sum(p => p.Value.CostPerChar);
                            TotalOcrCost = ScanningCost.Sum(p => p.Value.OcrCost);
                            TotalOcr = ScanningCost.Sum(p => p.Value.DocsOCR);
                            TotalCharTyped = ScanningCost.Sum(p => p.Value.CharTyped);
                            TotalCost = TotalDocCost + TotalCharCost + TotalOcrCost;
                            EdocsITSCustomers = EdocsITSApis.GetEdocsCustomer(clientFactory, WebApiUrl, EdocsITSConstants.EdocsITSCustomersController, custID, EdocsITSConstants.SpGetCustomerInformation).ConfigureAwait(false).GetAwaiter().GetResult();
                            EdocsModel = EdocsITSApis.GetEdocsCustomer(clientFactory, WebApiUrl, EdocsITSConstants.EdocsITSCustomersController, "1000", EdocsITSConstants.SpGetCustomerInformation).ConfigureAwait(false).GetAwaiter().GetResult();
                            CreateInvoie().ConfigureAwait(false).GetAwaiter().GetResult();
                        }
                    }
                    else
                    {
                        RepStartDate = DateTime.Now;
                        RepEndDate = DateTime.Now.AddDays(1);
                        ScanStartDate = "No start date";
                    }
                }
            }
            CustomerID = custID;
            return Page();
        }
        private async Task GetTotalsTrackingIDs()

        {
            IList<string> tIDS = GetTrackingIDs().ConfigureAwait(false).GetAwaiter().GetResult();
            ScanningCost = new Dictionary<int, EdocsITSCostScanningModel>();
            int count = 0;
            foreach (string value in tIDS)
            {
                EdocsITSCostScanningModel edocsITSCostScanning = new EdocsITSCostScanningModel();
                edocsITSCostScanning.Uploaded = ScanningCostTemp.Where(k=>k.Value.TrackingID == value).Sum(p =>p.Value.Uploaded);
                edocsITSCostScanning.CharTyped = ScanningCostTemp.Where(k => k.Value.TrackingID == value).Sum(p => p.Value.CharTyped);
                edocsITSCostScanning.CostPerChar = ScanningCostTemp.Select(p=>p.Value.CostPerChar).FirstOrDefault();
                edocsITSCostScanning.CostPerDoc = ScanningCostTemp.Where(k => k.Value.TrackingID == value).Sum(p => p.Value.CostPerDoc);//ScanningCostTemp.Select(p=>p.Value.CostPerDoc).FirstOrDefault();
                edocsITSCostScanning.DocsOCR =  ScanningCostTemp.Where(k => k.Value.TrackingID == value).Sum(p => p.Value.DocsOCR);
                edocsITSCostScanning.ID = ++count;
                edocsITSCostScanning.OcrCost = ScanningCostTemp.Select(p=>p.Value.OcrCost).FirstOrDefault();
                edocsITSCostScanning.PriceCharTyped = ScanningCostTemp.Where(p=>p.Value.TrackingID == value).Select(k => k.Value.PriceCharTyped).FirstOrDefault();
                edocsITSCostScanning.PriceOCR = ScanningCostTemp.Where(p => p.Value.TrackingID == value).Select(k => k.Value.OcrCost).FirstOrDefault();
                edocsITSCostScanning.PricePerDocument = ScanningCostTemp.Where(p => p.Value.TrackingID == value).Select(k => k.Value.PricePerDocument).FirstOrDefault(); 
                edocsITSCostScanning.ScannDate  = ScanningCostTemp.Where(p => p.Value.TrackingID == value).Select(k => k.Value.ScannDate).FirstOrDefault(); 
                edocsITSCostScanning.Scanned = ScanningCostTemp.Where(k => k.Value.TrackingID == value).Sum(p => p.Value.Scanned);
                edocsITSCostScanning.TrackingID = value;
                ScanningCost.Add(count, edocsITSCostScanning);

            }
           

            
           
        }
        private async Task<IList<string>> GetTrackingIDs()
        {
            IList<string> retList = new List<string>();
            foreach (var value in ScanningCostTemp)
            {
                if (!(retList.Contains(value.Value.TrackingID)))
                    retList.Add(value.Value.TrackingID);
                
            }
            return retList.OrderBy(x => x.FirstOrDefault()).ToList();
        }
        private async Task CreateInvoie()
        {
            string webRootPath = Path.Combine(_webHostEnvironment.WebRootPath, InvFolderName);
            string tempPlate = Path.Combine(webRootPath, InvoiceHtmlTemplate);
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            string invoice = System.IO.File.ReadAllText(tempPlate);
            invoice = RepString(invoice, "EdocsAddress", EdocsModel.EdocsCustomerAddress).ConfigureAwait(false).GetAwaiter().GetResult();
            invoice = RepString(invoice, "EdocsCity", EdocsModel.EdocsCustomerCity).ConfigureAwait(false).GetAwaiter().GetResult();
            invoice = RepString(invoice, "EdosState", EdocsModel.EdosCustomerState).ConfigureAwait(false).GetAwaiter().GetResult();
            invoice = RepString(invoice, "EdocsZipCode", EdocsModel.EdocsCustomerZipCode).ConfigureAwait(false).GetAwaiter().GetResult();
            string pnUmber = $"({EdocsModel.EdocsCustomerCellPhoneNumber.Substring(0, 3)}) {EdocsModel.EdocsCustomerCellPhoneNumber.Substring(3, 3)}-{EdocsModel.EdocsCustomerCellPhoneNumber.Substring(6, 4)}";
            invoice = RepString(invoice, "EdocsPhoneNumber", $"({EdocsModel.EdocsCustomerCellPhoneNumber.Substring(0, 3)}) {EdocsModel.EdocsCustomerCellPhoneNumber.Substring(3, 3)}-{EdocsModel.EdocsCustomerCellPhoneNumber.Substring(6, 4)}").ConfigureAwait(false).GetAwaiter().GetResult();
            // invoice = RepString(invoice, "EmailAddress", EdocsModel.EdocsCustomerEmailAddress).ConfigureAwait(false).GetAwaiter().GetResult();
            invoice = RepString(invoice, "InvoiceDate", DateTime.Now.ToString("MM-dd-yyyy")).ConfigureAwait(false).GetAwaiter().GetResult();
            invoice = RepString(invoice, "{edocslogo}", EdocsModel.ImgStr).ConfigureAwait(false).GetAwaiter().GetResult();
            invoice = RepString(invoice, "{customerLogo}", EdocsITSCustomers.ImgStr).ConfigureAwait(false).GetAwaiter().GetResult();

            invoice = RepString(invoice, "EdocsCustomerName", EdocsITSCustomers.EdocsCustomerName).ConfigureAwait(false).GetAwaiter().GetResult();
            invoice = RepString(invoice, "EdocsCustomerAddress", EdocsITSCustomers.EdocsCustomerAddress).ConfigureAwait(false).GetAwaiter().GetResult();
            invoice = RepString(invoice, "EdocsCustomerCity", EdocsITSCustomers.EdocsCustomerCity).ConfigureAwait(false).GetAwaiter().GetResult();
            invoice = RepString(invoice, "EdosCustomerState", EdocsITSCustomers.EdosCustomerState).ConfigureAwait(false).GetAwaiter().GetResult();
            invoice = RepString(invoice, "EdocsCustomerZipCode", EdocsITSCustomers.EdocsCustomerZipCode).ConfigureAwait(false).GetAwaiter().GetResult();

            invoice = RepString(invoice, "EdocsCustomerPhoneNumber", $"({EdocsITSCustomers.EdocsCustomerCellPhoneNumber.Substring(0, 3)}) {EdocsITSCustomers.EdocsCustomerCellPhoneNumber.Substring(3, 3)}-{EdocsITSCustomers.EdocsCustomerCellPhoneNumber.Substring(6, 4)}").ConfigureAwait(false).GetAwaiter().GetResult();
            invoice = RepString(invoice, "{EdocsCustomerEmailAddress}", EdocsITSCustomers.EdocsCustomerEmailAddress).ConfigureAwait(false).GetAwaiter().GetResult();
            invoice = RepString(invoice, "Scan Dates", $"Scan Dates {ScanStartDate}-{ScanEndDate}").ConfigureAwait(false).GetAwaiter().GetResult();
            invoice = RepString(invoice, "{EdocsCustomerID}", EdocsITSCustomers.EdocsCustomerID.ToString()).ConfigureAwait(false).GetAwaiter().GetResult();
            //invoice = RepString(invoice, "EdocsCustomerZipCode", EdocsITSCustomers.EdocsCustomerCity).ConfigureAwait(false).GetAwaiter().GetResult();
            foreach (KeyValuePair<int, EdocsITSCostScanningModel> items in ScanningCost)
            {
                sb.AppendLine($"<tr class={EdocsITSConstants.Quoat}w-auto{EdocsITSConstants.Quoat} style={EdocsITSConstants.Quoat}font-size:medium{EdocsITSConstants.Quoat}>");
                sb.AppendLine($"<td style={EdocsITSConstants.Quoat}font-size:medium{EdocsITSConstants.Quoat}>{items.Value.TrackingID}</td>");
                sb.AppendLine($"<td style={EdocsITSConstants.Quoat}font-size:medium{EdocsITSConstants.Quoat}>{items.Value.IDTracking}</td>");
                sb.AppendLine($"<td style={EdocsITSConstants.Quoat}font-size:medium{EdocsITSConstants.Quoat}>{items.Value.Scanned} </td>");
                sb.AppendLine($"<td style={EdocsITSConstants.Quoat}font-size:medium{EdocsITSConstants.Quoat}>{items.Value.Uploaded} </td>");
                sb.AppendLine($"<td style={EdocsITSConstants.Quoat}font-size:medium{EdocsITSConstants.Quoat}>${string.Format("{0:0.00}", items.Value.PricePerDocument)}</td>");
                sb.AppendLine($"<td style={EdocsITSConstants.Quoat}font-size:medium{EdocsITSConstants.Quoat}>${string.Format("{0:0.00}", items.Value.CostPerDoc)}</td>");
                sb.AppendLine($"<td style={EdocsITSConstants.Quoat}font-size:medium{EdocsITSConstants.Quoat}>{items.Value.DocsOCR}</td>");
                sb.AppendLine($"<td style={EdocsITSConstants.Quoat}font-size:medium{EdocsITSConstants.Quoat}>${string.Format("{0:0.00}", items.Value.PriceOCR)}</td>");
                sb.AppendLine($"<td style={EdocsITSConstants.Quoat}font-size:medium{EdocsITSConstants.Quoat}>${string.Format("{0:0.00}", items.Value.OcrCost)}</td>");
                sb.AppendLine($"<td style={EdocsITSConstants.Quoat}font-size:medium{EdocsITSConstants.Quoat}>{items.Value.CharTyped}</td>");
                sb.AppendLine($"<td style={EdocsITSConstants.Quoat}font-size:medium{EdocsITSConstants.Quoat}>${string.Format("{0:0.00}", items.Value.PriceCharTyped)}</td>");
                sb.AppendLine($"<td style={EdocsITSConstants.Quoat}font-size:medium{EdocsITSConstants.Quoat}>${string.Format("{0:0.00}", items.Value.CostPerChar)} </td>");
                sb.AppendLine($"<td style={EdocsITSConstants.Quoat}font-size:medium{EdocsITSConstants.Quoat}>{items.Value.ScannDate.ToString("MM-dd-yyyyy")} </td>");
                float totCost = items.Value.CostPerDoc + items.Value.OcrCost + items.Value.CostPerChar;
                sb.AppendLine($"<td style={EdocsITSConstants.Quoat}font-size:medium{EdocsITSConstants.Quoat}>${string.Format("{0:0.00}", totCost)}</td>");
                sb.AppendLine("</tr>");
            }
            invoice = RepString(invoice, "{tableData}", sb.ToString()).ConfigureAwait(false).GetAwaiter().GetResult();
            sb.Clear();
            sb.AppendLine($"<td style={EdocsITSConstants.Quoat}font-size:medium{EdocsITSConstants.Quoat}>Total</td>");
            sb.AppendLine($"<td style= {EdocsITSConstants.Quoat}font-size:medium{EdocsITSConstants.Quoat}>{TotalDocuments}</td>");
            sb.AppendLine($"<td style={EdocsITSConstants.Quoat}font-size:medium{EdocsITSConstants.Quoat}>{TotalDocuments} </td>");
            sb.AppendLine($"<td style={EdocsITSConstants.Quoat}font-size:medium{EdocsITSConstants.Quoat}>N/A </td>");
            sb.AppendLine($" <td style={EdocsITSConstants.Quoat}font-size:medium{EdocsITSConstants.Quoat} >${string.Format("{0:0.00}", TotalDocCost)}</td>");
            sb.AppendLine($"<td style={EdocsITSConstants.Quoat}font-size:medium{EdocsITSConstants.Quoat}>{TotalOcr}</td>");
            sb.AppendLine($"<td style ={EdocsITSConstants.Quoat}font-size:medium{EdocsITSConstants.Quoat}>N/A</td>");
            sb.AppendLine($"<td style ={EdocsITSConstants.Quoat}font-size:medium{EdocsITSConstants.Quoat}>N/A </td >");
            sb.AppendLine($"<td style={EdocsITSConstants.Quoat}font-size:medium{EdocsITSConstants.Quoat}>{TotalCharTyped}</td>");
            sb.AppendLine($"<td style={EdocsITSConstants.Quoat}font-size:medium{EdocsITSConstants.Quoat}>N/A </td>");
            sb.AppendLine($"<td style={EdocsITSConstants.Quoat}font-size:medium{EdocsITSConstants.Quoat}>${string.Format("{0:0.00}", TotalCharCost)} </td>");
            sb.AppendLine($"<td style={EdocsITSConstants.Quoat}font-size:medium{EdocsITSConstants.Quoat}>N/A</td>");
            sb.AppendLine($"<td style={EdocsITSConstants.Quoat}font-size:medium{EdocsITSConstants.Quoat}>${string.Format("{0:0.00}", TotalCost)}</td>");
            invoice = RepString(invoice, "{tablefooterData}", sb.ToString()).ConfigureAwait(false).GetAwaiter().GetResult();
            sb.Clear();
            sb.AppendLine($"<p class={EdocsITSConstants.Quoat}text-info text-right{EdocsITSConstants.Quoat} style={EdocsITSConstants.Quoat}font-size:medium{EdocsITSConstants.Quoat}>Documents Scanned {TotalDocuments}</p>");
            sb.AppendLine($"<p class={EdocsITSConstants.Quoat}text-info text-right{EdocsITSConstants.Quoat} style={EdocsITSConstants.Quoat}font-size:medium{EdocsITSConstants.Quoat}>Cost Documents Scanned ${string.Format("{0:0.00}", TotalDocCost)}</p>");
            sb.AppendLine($"<p class={EdocsITSConstants.Quoat}text-info text-right{EdocsITSConstants.Quoat} style={EdocsITSConstants.Quoat}font-size:medium{EdocsITSConstants.Quoat}>Cost Documents OCR ${string.Format("{0:0.00}", TotalOcrCost)}</p>");
            sb.AppendLine($"<p class={EdocsITSConstants.Quoat}text-info text-right{EdocsITSConstants.Quoat} style={EdocsITSConstants.Quoat}font-size:medium{EdocsITSConstants.Quoat}>Cost Char Typed ${string.Format("{0:0.00}", TotalCharCost)}</p>");
            sb.AppendLine($"<p class={EdocsITSConstants.Quoat}text-info text-right{EdocsITSConstants.Quoat} style={EdocsITSConstants.Quoat}font-size:medium{EdocsITSConstants.Quoat}>SubTotal ${string.Format("{0:0.00}", TotalCost)}</p>");
            sb.AppendLine($"<p class={EdocsITSConstants.Quoat}text-info text-right{EdocsITSConstants.Quoat} style={EdocsITSConstants.Quoat}font-size:medium{EdocsITSConstants.Quoat}>Tax Rate 0.00%</p>");
            sb.AppendLine($"<p class={EdocsITSConstants.Quoat}text-info text-right{EdocsITSConstants.Quoat} style={EdocsITSConstants.Quoat}font-size:medium{EdocsITSConstants.Quoat}>Other Fees $0.00</p>");
            sb.AppendLine($"<p class={EdocsITSConstants.Quoat}text-info text-right{EdocsITSConstants.Quoat} style={EdocsITSConstants.Quoat}font-size:medium{EdocsITSConstants.Quoat}>Total Cost ${string.Format("{0:0.00}", TotalCost)}</p>");
            invoice = RepString(invoice, "{invsum}", sb.ToString()).ConfigureAwait(false).GetAwaiter().GetResult();
            System.IO.File.WriteAllTextAsync(Path.Combine(webRootPath, InvFileName), invoice).ConfigureAwait(false).GetAwaiter().GetResult();

        }
        private async Task<string> RepString(string instr, string oldstr, string newstr)
        {
            return instr.Replace(oldstr, newstr);
        }
        public async Task<IActionResult> OnPostAsync(DateTime repSDate, DateTime repSEndDate, string custID)
        {


            return Page();
        }
    }
}
