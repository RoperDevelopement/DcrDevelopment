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

namespace Edocs.ITS.AppService.Pages.Partial_Pages
{
    public class PartialPageLookUpPSUDRecsModel : PageModel
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
        public PSUSDFullTextModel FullText
        { get; set; }
        public IDictionary<int, PSUSDRecsordsModel> Recs
        { get; set; }
        public PartialPageLookUpPSUDRecsModel(IConfiguration config, IHttpClientFactory factoryClient)
        {
            configuration = config;
            clientFactory = factoryClient;
        }

        public async Task<IActionResult> OnGetAsync(string sType, string sDate, string eDate, string dep, string orgDep, string fName, string lName, string stext)
        {
            var qString = Request.QueryString;

            //   sType = Request.Query["sType"];
            //string sDate = Request.Query["sDate"];
            // string eDate = Request.Query["eDate"];
            //string dep = Request.Query["dep"];
            //string orgDep = Request.Query["orgDep"];
            //string fName = Request.Query["fname"];
            //string lName = Request.Query["lName"];
            //string sText = Request.Query["stext"];

            if (string.Compare(sType, "keywordSesearch", true) == 0)
            {
                PSUSDFullTextModel pSUSDFullText = new PSUSDFullTextModel();
                pSUSDFullText.SearchText = stext;
                Recs = EdocsITSApis.PSUSDRecordsFullText(clientFactory, WebApiUrl, EdocsITSConstants.EdocsITPSUSDSearchTextController, pSUSDFullText).ConfigureAwait(true).GetAwaiter().GetResult();
            }

            else if ((string.Compare(dep, "Select Department", true) != 0) || (string.Compare(orgDep, "Select Orgination Department", true) != 0))
            {
                Recs = EdocsITSApis.PSUSDRecordsDept(clientFactory, WebApiUrl, EdocsITSConstants.EdocsITPSUSDFLNameController, fName, lName, sDate, eDate, dep, orgDep).ConfigureAwait(true).GetAwaiter().GetResult();
            }
            else
            {
                if((string.IsNullOrWhiteSpace(fName)) && (string.IsNullOrWhiteSpace(lName)))
                    Recs = EdocsITSApis.PSUSDRecordsDept(clientFactory, WebApiUrl, EdocsITSConstants.EdocsITPSUSDFLNameController, fName, lName, sDate, eDate, "NA", "NA").ConfigureAwait(true).GetAwaiter().GetResult();
                else
                Recs = EdocsITSApis.PSUSDRecordsFLName(clientFactory, WebApiUrl, EdocsITSConstants.EdocsITPSUSDFLNameController, fName, lName, sDate, eDate).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            return Page();
        }
    }
}
