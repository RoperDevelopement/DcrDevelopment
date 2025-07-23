using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Edocs.Demo.Application.Services.ApiConsts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Edocs.Demo.Application.Services.Models;
using System.Security.Policy;

namespace Edocs.Demo.Application.Services.Pages.Partial_Pages
{
    public class AutoCompletProjectNamesPartialPageModel : PageModel
    {
        private readonly IConfiguration configuration;
        private readonly IHttpClientFactory clientFactory;

        public AutoCompletProjectNamesPartialPageModel(IConfiguration config, IHttpClientFactory factoryClient)
        {
            try
            {
                configuration = config;
                clientFactory = factoryClient;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private Uri WebApiUrl
        { get { return configuration.GetValue<Uri>(DemoConstants.JsonWebApi); } }
        public List<BSBPlublicWorksProjectNameModel> PName
        { get; set; }
        public void OnGet()
        {
        }
        public JsonResult OnPost(string prefix)
        {
            //            Console.WriteLine();
            //          return null;
            //List<string> l = new List<string>();
            // l.Add("dddd");
            // l.Add("ddddddd");
            PName = DemoApis.DemoInstance.GetBSBPWDPNames(clientFactory, WebApiUrl, DemoConstants.BSBPlanningDepartmentController, prefix, DemoConstants.SpGetBSBPWDProjectNames).ConfigureAwait(false).GetAwaiter().GetResult();
            var pName = PName.Where(p => p.ProjectName.ToLower().StartsWith(prefix.ToLower())).Select(p => p.ProjectName).ToList();
            JsonResult jsonResult = new JsonResult(pName);
            return jsonResult;
        }
    }
}
