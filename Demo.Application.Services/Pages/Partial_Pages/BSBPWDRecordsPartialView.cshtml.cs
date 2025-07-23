using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Edocs.Demo.Application.Services.Models;
using Edocs.Demo.Application.Services.ApiConsts;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using System.Security.Cryptography;
using System.Xml.Linq;

namespace Edocs.Demo.Application.Services.Pages.Partial_Pages
{
    public class BSBPWDRecordsPartialViewModel : PageModel
    {
        private readonly IConfiguration configuration;
        private readonly IHttpClientFactory clientFactory;
        public BSBPWDRecordsPartialViewModel(IConfiguration config, IHttpClientFactory factoryClient)
        {
            configuration = config;
            clientFactory = factoryClient;
        }
        public IDictionary<string, BSBPlublicWorksDepartmentProjectNameModel> PNames
        { get; set; }
        private Uri WebApiUrl
        { get { return configuration.GetValue<Uri>(DemoConstants.JsonWebApi); } }
        public async Task<IActionResult> OnGetAsync(string sType, string pname, string pyear, string pDep, string kWords)
        {

            if (string.Compare(sType, "advSearch", true) == 0)
                AdvSearch(pname, pyear, pDep, kWords).ConfigureAwait(true).GetAwaiter().GetResult();
          //  PNames = DemoApis.DemoInstance.GetBSBPWDRecords(clientFactory, WebApiUrl, DemoConstants.BSBPlanningDepartmentController, "Water", "2023", "test1", "na").ConfigureAwait(false).GetAwaiter().GetResult();
          else
            {
                KeyWords(kWords).ConfigureAwait(true).GetAwaiter().GetResult();
            }
            return Page();
        }

        private async Task AdvSearch(string pname, string pyear, string pDep, string kWords)
        {
            if (pDep.StartsWith("Optional"))
                pDep = DemoConstants.NA;
            if (pyear.StartsWith("Optional"))
                pyear = DemoConstants.NA;
            if (pname.Length < 3)
                pname = DemoConstants.NA;

            PNames = DemoApis.DemoInstance.GetBSBPWDRecords(clientFactory, WebApiUrl, DemoConstants.BSBPlanningDepartmentController, pDep, pyear,pname,DemoConstants.NA).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        private async Task KeyWords(string kWords)
        {
            
            
           

            PNames = DemoApis.DemoInstance.GetBSBPWDRecords(clientFactory, WebApiUrl, DemoConstants.BSBPlanningDepartmentController, DemoConstants.NA,DemoConstants.NA, DemoConstants.NA,kWords).ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}
