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
 
namespace Edocs.Demo.Application.Services.Pages.DemoPages
{
    public class BSBPublicWorksDepartmentViewModel : PageModel
    {
        private readonly IConfiguration configuration;
        private readonly IHttpClientFactory clientFactory;
        public BSBPublicWorksDepartmentViewModel(IConfiguration config, IHttpClientFactory factoryClient)
        {
            configuration = config;
            clientFactory = factoryClient;
        }
        private Uri WebApiUrl
        { get { return configuration.GetValue<Uri>(DemoConstants.JsonWebApi); } }
       public IList<BSBPlublicWorksDepartmentYearModel> BSBPlublicWorksDepartmentYears
        { get; set; }
        public IList<BSBPlublicWorksDepartmentModel> BSBPlublicWorksDepartmentProjectDep
        { get; set; }
        public async Task<IActionResult> OnGetAsync(string SearchText)
        {
            BSBPlublicWorksDepartmentYears = DemoApis.DemoInstance.GetBSBPWDYears(WebApiUrl, DemoConstants.BSBPlanningDepartmentController).ConfigureAwait(false).GetAwaiter().GetResult();
            BSBPlublicWorksDepartmentProjectDep = DemoApis.DemoInstance.GetBSBPWDDepartment(WebApiUrl, DemoConstants.BSBPlanningDepartmentController).ConfigureAwait(false).GetAwaiter().GetResult();
            return Page();
        }
    }
}
