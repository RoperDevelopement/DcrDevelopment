using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Xml.Linq;
using Edocs.ITS.AppService.ApisConst;
using Edocs.ITS.AppService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Edocs.ITS.AppService.Pages.Partial_Pages
{
    public class PartialPagePSUSDFNameLastNameModel : PageModel
    {
        private readonly IConfiguration configuration;
        private readonly IHttpClientFactory clientFactory;
        private Uri WebApiUrl
        { get { return configuration.GetValue<Uri>(EdocsITSConstants.JsonWebApi); } }
        List<PSUDFirstNameModel> FNameModel
        { get; set; }
        List<PSUDLastNameModel> LNameModel
        { get; set; }
        public PartialPagePSUSDFNameLastNameModel(IConfiguration config, IHttpClientFactory factoryClient)
        {
            configuration = config;
            clientFactory = factoryClient;
        }

        public void OnGet()
        {
        }
        public JsonResult OnPost(string prefix)

        {


            string sDate = string.Empty;
            string eDate = string.Empty;
            string fNameLastName = string.Empty;

            if (prefix.Contains("/"))
            {
                string[] data = prefix.Split("/");
                if (data.Length > 1)
                {

                    fNameLastName = data[1];
                    sDate = data[2];
                    eDate = data[3];
                    prefix = data[0];
                }
            }
            //GetPSUSDLookRecordsFirstName(IHttpClientFactory clientFactory, Uri uri, string controller, string spName, string searchFor, string repType, DateTime stDate, DateTime endDate)
            if (string.Compare(fNameLastName, "FirstName", true) == 0)
            {
                FNameModel = EdocsITSApis.GetPSUSDLookRecordsFirstName(clientFactory, WebApiUrl, EdocsITSConstants.EdocsITSPSUSDRecordsController, EdocsITSConstants.SpGetPSUSDRecordSearch, prefix, fNameLastName, DateTime.Parse(sDate), DateTime.Parse(eDate)).ConfigureAwait(false).GetAwaiter().GetResult();
               var fName= FNameModel.Select(p => p.FirstName).ToList();
                JsonResult jsonResultFName = new JsonResult(fName);
                return jsonResultFName;
            }
            else
            {
               LNameModel = EdocsITSApis.GetPSUSDLookRecordsLastName(clientFactory, WebApiUrl, EdocsITSConstants.EdocsITSPSUSDRecordsController, EdocsITSConstants.SpGetPSUSDRecordSearch, prefix, fNameLastName, DateTime.Parse(sDate), DateTime.Parse(eDate)).ConfigureAwait(false).GetAwaiter().GetResult().ToList();
                var lName = LNameModel.Select(p => p.LastName).ToList();
                JsonResult jsonResultLName = new JsonResult(lName);
                return jsonResultLName;
            }
           
        }
    }
}
