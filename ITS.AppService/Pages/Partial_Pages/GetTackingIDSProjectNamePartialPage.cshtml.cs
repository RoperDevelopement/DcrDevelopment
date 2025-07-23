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
using Newtonsoft.Json;
namespace Edocs.ITS.AppService.Pages.Partial_Pages
{
    public class GetTackingIDSProjectNamePartialPageModel : PageModel
    {
        private readonly IConfiguration configuration;
        private readonly IHttpClientFactory clientFactory;
        private Uri WebApiUrl
        { get { return configuration.GetValue<Uri>(EdocsITSConstants.JsonWebApi); } }
        public IList<TrackingIDProjectNameModel> TackingIDs
        { get; set; }
        public GetTackingIDSProjectNamePartialPageModel(IConfiguration config, IHttpClientFactory factoryClient)
        {
            configuration = config;
            clientFactory = factoryClient;
        }
         
        public void OnGet()
        {
        }
        public JsonResult OnPost(string prefix)
            
        {
            
            bool getTIDDocNames = true;
            string sDate = string.Empty;
            string eDate = string.Empty;
            if(prefix.Contains("/"))
            {
                string[] TIDDocName = prefix.Split("/");
                if(TIDDocName.Length > 1)
                {
                    if (string.Compare(TIDDocName[1], "dn", true) == 0)
                    { 
                        getTIDDocNames = false;
                        sDate = TIDDocName[2];
                        eDate = TIDDocName[3];
                    }
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
            if(getTIDDocNames)
                TackingIDs = EdocsITSApis.GetTrackingIDs(clientFactory, WebApiUrl, EdocsITSConstants.TransferByTrackIDController, EdocsITSConstants.CurrentCustID).ConfigureAwait(false).GetAwaiter().GetResult();
            else
                TackingIDs = EdocsITSApis.GetTrackingDocNames(clientFactory, WebApiUrl, EdocsITSConstants.TransferByTrackIDController, EdocsITSConstants.CurrentCustID, sDate,eDate).ConfigureAwait(false).GetAwaiter().GetResult();
            var persons = TackingIDs.Where(p => p.TrackingID.ToLower().StartsWith(prefix.ToLower())).Select(p => p.TrackingID).ToList();
            if ((persons != null) && (persons.Count() > 0))
            {


                if (getTIDDocNames)
                    persons.Insert(0, "All TrackingIDs");
                else
                    persons.Insert(0, "All Documents");
            }
            else
            {

                if (getTIDDocNames)
                    persons.Add("All TrackingIDs");
                else
                    persons.Add("All Documents");
            }
            
            //var persons = (from person in this.TackingIDs
            //               where person.TackingIDPName.StartsWith(prefix)
            //               select new
            //               {

                    //                   val = person.TackingIDPName
                    //               }).ToList();
            JsonResult jsonResult = new JsonResult(persons);
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
