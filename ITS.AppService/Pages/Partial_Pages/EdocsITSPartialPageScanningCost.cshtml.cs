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
using System.Text;
using Org.BouncyCastle.Utilities;
using Microsoft.VisualBasic;
using System.Reflection;

namespace Edocs.ITS.AppService.Pages.Partial_Pages
{
    public class EdocsITSPartialPageScanningCostModel : PageModel
    {
        private readonly IConfiguration configuration;
        private readonly IHttpClientFactory clientFactory;
        private Uri WebApiUrl
        { get { return configuration.GetValue<Uri>(EdocsITSConstants.JsonWebApi); } }


        public EdocsITSPartialPageScanningCostModel(IConfiguration config, IHttpClientFactory factoryClient)
        {
            configuration = config;
            clientFactory = factoryClient;



        }
        public int TotalDocuments
        { get; set; }
        public float TotalDocCost
        { get; set; }
        public float TotalCharCost
        { get; set; }
        public float TotalOcrCost
        { get; set; }
        public float TotalCost
        { get; set; }
        public IDictionary<int, EdocsITSCostScanningModel> ScanningCost
        { get; set; }
        public UserLoginModel UserLogin
        { get; set; }
        public IDictionary<int, EdocsITSUsersModel> ListUserModel
        { get; set; }
        public async Task<IActionResult> OnGetAsync(DateTime repSDate, DateTime repEDate, string custID, string export)
        {
            var qString = Request.QueryString;
            if ((qString.HasValue) && (repSDate.Year > 2015))
            {
                ScanningCost = EdocsITSApis.GetScanninCost(clientFactory, WebApiUrl, EdocsITSConstants.EdocsITSScanningCoastController, repSDate, repEDate, custID,"doc").ConfigureAwait(false).GetAwaiter().GetResult();
                if ((ScanningCost != null) && (ScanningCost.Count() > 0))
                {
                    TotalDocuments = ScanningCost.Sum(p => p.Value.Uploaded);
                    TotalDocCost = ScanningCost.Sum(p => p.Value.CostPerDoc);
                    TotalCharCost = ScanningCost.Sum(p => p.Value.CostPerChar);
                    TotalOcrCost = ScanningCost.Sum(p => p.Value.OcrCost);
                    TotalCost = TotalDocCost + TotalCharCost + TotalOcrCost;
                    if ((!(string.IsNullOrWhiteSpace(export))) && (string.Compare(export, "exportExcel", true) == 0))
                    {
                        FileContentResult fileContentResult = ExportUsers().ConfigureAwait(false).GetAwaiter().GetResult();
                        return fileContentResult;
                        
                    }
                }
            }
            return Page();
        }

        private async Task<FileContentResult> ExportUsers()
        {

            if (ScanningCost.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("  TrackinID,Scanned,UpLoaded,Price PerDocument,Cost Documents,OCR,Price OCR,Cost OCR,Char Typed,Price Char,Cost Char,Scan Date");
                foreach (KeyValuePair<int, EdocsITSCostScanningModel> items in ScanningCost)
                {
                    sb.AppendLine($@"{items.Value.TrackingID.Replace(",","-")},{items.Value.Scanned},{items.Value.Uploaded},{string.Format("{0:0.00}", items.Value.PricePerDocument)},{string.Format("{0:0.00}", items.Value.CostPerDoc)},{items.Value.DocsOCR},{string.Format("{0:0.00}", items.Value.PriceOCR)},{string.Format("{0:0.00}", items.Value.OcrCost)}, {items.Value.CharTyped}, {string.Format("{0:0.00}", items.Value.CostPerChar)}, {string.Format("{0:0.00}", items.Value.PriceCharTyped)},{items.Value.ScannDate}");
                }
                sb.AppendLine();
                sb.AppendLine($"Documents Scanned, {TotalDocuments}");
                sb.AppendLine($"Cost Documents Scanned, ${string.Format("{0:0.00}", TotalDocCost)}");
                sb.AppendLine($"Cost Documents OCR,$ {string.Format("{0:0.00}", TotalOcrCost)}");
                sb.AppendLine($"Cost Char Typed,$ {string.Format("{0:0.00}", TotalCharCost)}");
                sb.AppendLine($"Total Cost,$ {string.Format("{0:0.00}", TotalCost)}");
                // Response.Headers.Add("Content-Disposition", "attachment;filename=LabReqUsers.csv");
                return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", $"ScanningCost_{DateTime.Now.ToString("HH_mm_ss_MM_dd_yyyy")}.csv");
            }
            return null;
        }
    }
}
