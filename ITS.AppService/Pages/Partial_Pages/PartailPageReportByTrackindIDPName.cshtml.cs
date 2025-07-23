using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Edocs.ITS.AppService.ApisConst;
using Edocs.ITS.AppService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Org.BouncyCastle.Ocsp;

namespace Edocs.ITS.AppService.Pages.Partial_Pages
{
    public class PartailPageReportByTrackindIDPNameModel : PageModel
    {
        public IDictionary<string, EdocsITSProjectNameNumber> DicEdocsITSProjectNameNumber
        { get; set; }
        public IDictionary<string, ITSTrackingIDModel> DicTrackingIDS
        { get; set; }
        private readonly IConfiguration configuration;
        private readonly IHttpClientFactory clientFactory;
        private Uri WebApiUrl
        { get { return configuration.GetValue<Uri>(EdocsITSConstants.JsonWebApi); } }
        public UserLoginModel UserLogin
        { get; set; }
        public int TotalScanned
        { get; set; }
        public int TotalUploaded
        { get; set; }
        public int TotalOCR
        { get; set; }
        public int TotalTypedChar
        { get; set; }
        public string TrackID
        { get; set; }
        public string ReportDate
        { get; set; }
       // ITSTrackingIDModel
        public PartailPageReportByTrackindIDPNameModel(IConfiguration config, IHttpClientFactory factoryClient)
        {
            configuration = config;
            clientFactory = factoryClient;
        }
        public async Task<IActionResult> OnGetAsync(string trackIDPname)
{
            string repSDate = DateTime.Now.AddDays(-100).ToString("MM-dd-yyyy");
            string repSEndDate = DateTime.Now.ToString("MM-dd-yyyy");
            TrackID = trackIDPname;
            ReportDate = DateTime.Now.ToString("MM-dd-yyyy");
            DicTrackingIDS = EdocsITSApis.ReportTrackingIDsByTrackingID(clientFactory, WebApiUrl, EdocsITSConstants.EdocsITSReportsController, trackIDPname, "sp_RunReportByTrackingID").ConfigureAwait(false).GetAwaiter().GetResult();
            if ((DicTrackingIDS != null) && (DicTrackingIDS.Count() > 0))
            {
                TotalScanned = DicTrackingIDS.Sum(p => p.Value.TotalScanned);
                TotalUploaded = DicTrackingIDS.Sum(p => p.Value.TotalRecordsUploaded);
                TotalOCR = DicTrackingIDS.Sum(p => p.Value.TotalDocsOCR);
                TotalTypedChar = DicTrackingIDS.Sum(p => p.Value.TotalCharTyped);
            }
            return Page();
        }
    }
}
