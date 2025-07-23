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
    public class GetTackingIDsDocNameAcceptRecjectModel : PageModel
    {
        private readonly IConfiguration configuration;
        private readonly IHttpClientFactory clientFactory;
        private Uri WebApiUrl
        { get { return configuration.GetValue<Uri>(EdocsITSConstants.JsonWebApi); } }
        public IList<TrackingIDProjectNameModel> TackingIDs
        { get; set; }
        public GetTackingIDsDocNameAcceptRecjectModel(IConfiguration config, IHttpClientFactory factoryClient)
        {
            configuration = config;
            clientFactory = factoryClient;
        }

        public void OnGet()
        {
        }
        public JsonResult OnPost(string prefix)

        {

            string repType = EdocsITSConstants.ReportTypeTrackID;
            string sDate = string.Empty;
            string eDate = string.Empty;
            if (prefix.Contains("/"))
            {
                string[] TIDDocName = prefix.Split("/");
                if (TIDDocName.Length > 1)
                {
                    if (string.Compare(TIDDocName[1], "dn", true) == 0)
                    {
                        repType = EdocsITSConstants.ReportTypeDocName;
                        
                    }
                    sDate = TIDDocName[2];
                    eDate = TIDDocName[3];
                    prefix = TIDDocName[0];
                }
            }
            //TackingIDs = new List<TrackingIDProjectNameModel>();
            //TrackingIDProjectNameModel trackingIDProject = new TrackingIDProjectNameModel();
            //trackingIDProject.TrackingID = "ll";
            //TackingIDs.Add(trackingIDProject);
            //  trackingIDProject = new TrackingIDProjectNameModel();

            //trackingIDProject.TrackingID = "dan";
            //TackingIDs.Add(trackingIDProject);
            //trackingIDProject = new TrackingIDProjectNameModel();
            //trackingIDProject.TrackingID = "dan roper";
            //TackingIDs.Add(trackingIDProject);
            //trackingIDProject = new TrackingIDProjectNameModel();
            //trackingIDProject.TrackingID = "dan edocs";
            //TackingIDs.Add(trackingIDProject);
            //TackingIDs = GetSessionVariables.SessionVarInstance.GetJsonSessionObject<List<TrackingIDProjectNameModel>>(HttpContext.Session, "TrackingIDsPNames").ConfigureAwait(false).GetAwaiter().GetResult();
            //if (getTIDDocNames)
            //    TackingIDs = EdocsITSApis.GetTrackingIDs(clientFactory, WebApiUrl, EdocsITSConstants.TransferByTrackIDController, "1001").ConfigureAwait(false).GetAwaiter().GetResult();
            //else
            //    TackingIDs = EdocsITSApis.GetTrackingDocNames(clientFactory, WebApiUrl, EdocsITSConstants.TransferByTrackIDController, "1001", sDate, eDate).ConfigureAwait(false).GetAwaiter().GetResult();
            var idsTracking = EdocsITSApis.GetTrackingIDsDocNamesAcceptReject(clientFactory, WebApiUrl, EdocsITSConstants.TransferByTrackIDController, EdocsITSConstants.CurrentCustID, sDate, eDate, prefix, repType).ConfigureAwait(false).GetAwaiter().GetResult();
            //var tIDs = idsTracking.Where(p => p.TrackingID.ToLower().StartsWith(prefix.ToLower())).Select(p => p.TrackingID).ToList();
            var tIDs = idsTracking.Select(p => p.TrackingID).ToList();
            if ((tIDs != null) && (tIDs.Count() > 0))
            {

                if (string.Compare(repType, EdocsITSConstants.ReportTypeTrackID, true) == 0)
                    tIDs.Insert(0, "All TrackingIDs");
                else
                    tIDs.Insert(0, "All Documents");
            }
            else
            {
                if (string.Compare(repType, EdocsITSConstants.ReportTypeTrackID, true) == 0)
                    tIDs.Add("All TrackingIDs");
                else
                    tIDs.Add("All Documents");
            }

                    //var persons = (from person in this.TackingIDs
                    //               where person.TackingIDPName.StartsWith(prefix)
                    //               select new
                    //               {

                    //                   val = person.TackingIDPName
                    //               }).ToList();
                    JsonResult jsonResult = new JsonResult(tIDs);
            return jsonResult;

        }
        [HttpPost]
        public JsonResult AutoComplete(string prefix)
        {
            Console.WriteLine();
            var persons = (from person in this.TackingIDs
                           where person.TrackingID.StartsWith(prefix)
                           select new
                           {

                               val = person.TrackingID
                           }).ToList();
            JsonResult jsonResult = new JsonResult(persons);
            return jsonResult;

        }
    }
}
