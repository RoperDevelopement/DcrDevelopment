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
using System.Security.Policy;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System.Reflection.Metadata;
using Org.BouncyCastle.Ocsp;

namespace Edocs.ITS.AppService.Pages.TrackingSystemReports
{
    public class ITSReportsByProjectViewModel : PageModel
    {
        private readonly IConfiguration configuration;
        private readonly IHttpClientFactory clientFactory;
        private Uri WebApiUrl
        { get { return configuration.GetValue<Uri>(EdocsITSConstants.JsonWebApi); } }
        public UserLoginModel UserLogin
        { get; set; }
        public IDictionary<string, EdocsITSProjectNameNumber> DicEdocsITSProjectNameNumber
        { get; set; }
        public int TotalScanned
        { get; set; }
        public int TotalUploaded
        { get; set; }
        public int TotalOCR
        { get; set; }
        public int TotalTypedChar
        { get; set; }
        [Display(Name = "Document Start Date")]
        [DataType(DataType.Date)]
        public DateTime RepStartDate
        { get; set; }
        [BindProperty]
        [DataType(DataType.Date)]
        [Display(Name = "Document End Date")]
        public DateTime RepEndDate
        { get; set; }
        public ITSReportsByProjectViewModel(IConfiguration config, IHttpClientFactory factoryClient)
        {
            configuration = config;
            clientFactory = factoryClient;
        }

        public async Task<IActionResult> OnGetAsync(string repSDate, string repSEndDate, string trackID, string docName)
        {
            if (!EdocsITSHelpers.CheckAuth(HttpContext.Session))
                return Redirect("/LogInOut/LoginView");
            ViewData[GetSessionVariables.SessionKeyUserName] = GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyUserName).ConfigureAwait(true).GetAwaiter().GetResult();
            ViewData[GetSessionVariables.SessionKeyUserName] = EdocsITSHelpers.RepStr(ViewData[GetSessionVariables.SessionKeyUserName].ToString(), EdocsITSHelpers.QUOTE, string.Empty);
            UserLogin = GetSessionVariables.SessionVarInstance.GetJsonSessionObject<UserLoginModel>(HttpContext.Session, EdocsITSConstants.SessionUserInfo).ConfigureAwait(false).GetAwaiter().GetResult();
            //    if (!(string.IsNullOrWhiteSpace(repSDate)))
            if (string.IsNullOrWhiteSpace(repSDate))
            {
                RepStartDate = DateTime.Now.AddDays(-15);
                RepEndDate = DateTime.Now.AddDays(1);
            }
            else
            {
                string repType = EdocsITSConstants.ReportTypeTrackID;

                if (!(string.IsNullOrWhiteSpace(docName)))
                { 
                    repType = EdocsITSConstants.ReportTypeDocName;
                    trackID = docName;
                }

                RepEndDate = DateTime.Parse(repSEndDate);
                RepStartDate = DateTime.Parse(repSDate);
                
                if ((string.Compare(docName, "All Documents", true) == 0) || (string.Compare(trackID, "All TrackingIDs", true) == 0))
                    DicEdocsITSProjectNameNumber = EdocsITSApis.GetReportsByProjectNameNum(clientFactory, WebApiUrl, EdocsITSConstants.EdocsITSReportsController, repSDate, repSEndDate, EdocsITSConstants.SpRunReportByProjectName).ConfigureAwait(false).GetAwaiter().GetResult();
                else
                    DicEdocsITSProjectNameNumber = EdocsITSApis.GetReportsByTrackIDDocName(clientFactory, WebApiUrl, EdocsITSConstants.EdocsITSReportsController, repSDate, repSEndDate, trackID,repType).ConfigureAwait(false).GetAwaiter().GetResult();
                
               
                if ((DicEdocsITSProjectNameNumber != null) && (DicEdocsITSProjectNameNumber.Count() > 0))
                {
                    TotalScanned = DicEdocsITSProjectNameNumber.Sum(p => p.Value.TotalScanned);
                    TotalUploaded = DicEdocsITSProjectNameNumber.Sum(p => p.Value.TotalRecordsUploaded);
                    TotalOCR = DicEdocsITSProjectNameNumber.Sum(p => p.Value.TotalDocsOCR);
                    TotalTypedChar = DicEdocsITSProjectNameNumber.Sum(p => p.Value.TotalCharTyped);
                }
            }
            //if (string.Compare(repType, "docSent", true) == 0)
            //    ITSInventoryTransferSent = EdocsITSApis.GetInventorySent(clientFactory, WebApiUrl, EdocsITSConstants.EdocsITSReportsController, repSDate, repSEndDate, EdocsITSConstants.SpEdocsITSGetReports, repType, UserLogin.EdocsCustomerName).ConfigureAwait(false).GetAwaiter().GetResult();
            //else
            //    ITSScanningManModelRec = EdocsITSApis.GetInventoryRec(clientFactory, WebApiUrl, EdocsITSConstants.EdocsITSReportsController, repSDate, repSEndDate, EdocsITSConstants.SpEdocsITSGetReports, repType, UserLogin.EdocsCustomerName).ConfigureAwait(false).GetAwaiter().GetResult();


            return Page();
        }
    }
}
